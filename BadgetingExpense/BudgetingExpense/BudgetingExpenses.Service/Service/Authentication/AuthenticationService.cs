using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
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
        }
    }

    public Task<string>? GenerateJwtTokenAsync(string userId, string userRole)
    {
        try
        {
            return Task.Run(() =>
            {
                var tokenConfiguration = _configuration.GetSection("Jwt");
                var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration["Key"]));

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, userRole)
                };
                var token = new JwtSecurityToken
                (
                    issuer: tokenConfiguration["Issuer"],
                    audience: tokenConfiguration["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenConfiguration["ExpiryMinutes"])),
                    signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256)
                );
                _logger.LogInformation("Token generated for {userId}", userId);
                return new JwtSecurityTokenHandler().WriteToken(token);
            });
            
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception {ex}", ex.Message);
            return null;
        }
    }

    public async Task<IList<string>?> GetRoleAsync(string email)
    {
        try
        {
            var roles = await _unitOfWork.Authentication.GetUserRolesAsync(email);
            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError("Getting roles {ex}", ex.Message);
        }
        return null;
    }

    public async Task<User?> GetUserAsync(string email)
    {
        try
        {
            var user = await _unitOfWork.Authentication.GetUserByEmailAsync(email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Getting user {ex}", ex.Message);
        }
        return null;
    }

    public async Task<bool> LoginUserServiceAsync(Login user)
    {
        try
        {
            var check = await _unitOfWork.Authentication.CheckUserAsync(user.Email, user.Password);
            if (check)
            {
                _logger.LogInformation("Logged in user {email}", user.Email);
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Log in fail email:{email} ex:{ex}",user.Email, ex.Message);
        }
        return false;
    }

    public async Task<bool> RegisterUserServiceAsync(Register user)
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
                _logger.LogInformation("New user registered {email}", user.Email);
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Register new user {ex}", ex.Message);
        }
        return false;
    }
}
