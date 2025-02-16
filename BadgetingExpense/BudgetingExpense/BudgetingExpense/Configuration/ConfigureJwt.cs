using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BudgetingExpense.api.Configuration;

public static class ConfigureJwt
{
    public static void ConfigureJWTBearerToken(this IServiceCollection services, IConfiguration configuration)
    {
        var Jwt = configuration.GetSection("Jwt");
        var JwtKey = Encoding.UTF8.GetBytes(Jwt["Key"]);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Jwt["Issuer"],
                    ValidAudience = Jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(JwtKey)
                };
            });
        services.AddAuthorization();
    }
}
