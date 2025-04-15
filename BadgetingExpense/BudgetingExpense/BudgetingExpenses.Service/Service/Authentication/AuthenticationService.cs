using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using Microsoft.AspNetCore.Http;

namespace BudgetingExpenses.Service.Service.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ITokenAuthenticationService _tokenAuthService;
    public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration,
        ILogger<AuthenticationService> logger, IMemoryCache cache, IEmailService emailService,
        IHttpContextAccessor httpContext, ITokenAuthenticationService tokenAuthService)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _cache = cache;
        _emailService = emailService;
        _httpContext = httpContext;
        _tokenAuthService = tokenAuthService;
    }

    public async Task AddUserRolesAsync(string email, string role)
    {
        try
        {
            await _unitOfWork.Authentication.AddUserRoleAsync(email, role);
            _logger.LogInformation("Added role to new user email:{email} role:{role}", email, role);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception: {ex}", ex.Message);
            throw;
        }
    }

    public async Task<string?> LoginUserServiceAsync(Login user)
    {
        try
        {
            var check = await _unitOfWork.Authentication.CheckUserAsync(user.Email, user.Password);
            if (check)
            {
                var model = await _unitOfWork.Authentication.GetUserByEmailAsync(user.Email);
                var roles =  await _unitOfWork.Authentication.GetUserRolesAsync(user.Email);
                var token = await _tokenAuthService.GenerateJwtTokenAsync(model.Id, roles.FirstOrDefault());
                var cookies = _httpContext.HttpContext.Request.Cookies["refreshToken"];
                var handler = new JwtSecurityTokenHandler();
                if (cookies == null || handler.ReadJwtToken(cookies).Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value != model.Id)
                { 
                    var refreshToken = await _tokenAuthService.GenerateRefreshToken(model.Id);
                    _httpContext.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                }
                _logger.LogInformation("Logged in user {email}", user.Email);
                return token;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Log in fail email:{email} ex:{ex}",user.Email, ex.Message);
            throw;
        }
        return null;
    }

   

    public async Task<bool> VerifyUserEmailAsync(string email, string verificationCode)
    {
        try
        {
            var check = _cache.TryGetValue(email, out Tuple<string, Register>? value);
            if (check)
            {
                if(verificationCode == value.Item1)
                {
                    var registerUser = await RegisterUserAsync(value.Item2);
                    if (registerUser)
                    {
                        _cache.Remove(email);
                        _logger.LogInformation("Cache memory cleaned up Key: {key}", email);
                        return true;
                    }
                }
            }
            return false;
        }catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex);
            throw;
        }
    }

    public bool CacheNewUserCredentialsInMemory(Register user)
    {
        try
        {
            var email = _configuration.GetSection("VerifyEmail");
            string? subject = email["Subject"];
            string? message = email["Message"];
            string key = user.Email;
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            if (!_cache.TryGetValue(key, out Tuple<string, Register>? value))
            {
                string code = new Random().Next(0, 10000).ToString("D4");
                _cache.Set(key, Tuple.Create(code, user), options);
                message = message.Replace("{code}", code);
                _emailService.SendEmailAsync(new EmailModel
                {
                    Email = user.Email,
                    Message = message,
                    Subject = subject
                });
                return true;
            }
            
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{wx}",ex);
        }
        return false;
    }
    private async Task<bool> RegisterUserAsync(Register user)
    {
        try
        {
            var userMapped = new User
            {
                Email = user.Email,
                Password = user.Password,
                Surname = user.Surname,
                Name = user.Name
            };
            var result = await _unitOfWork.Authentication.RegisterUserAsync(userMapped);
            if (result)
            {
                await AddUserRolesAsync(user.Email, "User");
                _logger.LogInformation("New user registered {email}", user.Email);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Register new user {ex}", ex.Message);
            throw;
        }
    }
}
