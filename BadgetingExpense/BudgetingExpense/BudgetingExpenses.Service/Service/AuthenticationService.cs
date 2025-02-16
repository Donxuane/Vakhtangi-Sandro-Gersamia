﻿using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetingExpenses.Service.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    public  Task<string> GenerateJwtTokenAsync(string userId, string userRole)
    {
        return Task.Run(() =>
        {
            var Jwt = _configuration.GetSection("Jwt");
            var JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt["Key"]));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, userRole)
            };
            var token = new JwtSecurityToken(
                issuer: Jwt["Issuer"],
                audience: Jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(Jwt["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(JwtKey, SecurityAlgorithms.HmacSha256)
               
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        });
    }

    public async Task<bool> LoginUserServiceAsync(LoginDto user)
    {
        var check = await _unitOfWork.Authentication.CheckUserAsync(user.Email, user.Password);
        if(check == true)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> RegisterUserServiceAsync(RegisterDto user)
    {
        var userMapped = new User
        {
            Email = user.Email,
            Password = user.Password,
            UserSurname = user.UserSurname,
            UserName = user.UserName
        };
        var result = await _unitOfWork.Authentication.RegisterUserAsync(userMapped);
        if(result == true)
        {
            return true;
        }
        return false;
    }
}
