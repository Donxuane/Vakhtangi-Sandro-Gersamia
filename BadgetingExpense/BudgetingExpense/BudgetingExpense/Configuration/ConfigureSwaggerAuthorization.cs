using Microsoft.OpenApi.Models;

namespace BudgetingExpense.api.Configuration;

public static class ConfigureSwaggerAuthorization
{
    public static void AddSwaggerAuthorization(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
         {
             options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 In = ParameterLocation.Header,
                 Description = "Enter 'Bearer' [space] and then your token",
                 Name = "Authorization",
                 Type = SecuritySchemeType.ApiKey
             });
             options.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
             });
        });
    }
}
