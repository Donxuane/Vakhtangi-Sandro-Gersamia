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
                 Description = "Enter Only your token",
                 Name = "Authorization",
                 Type = SecuritySchemeType.Http,
                 Scheme = "Bearer",
                 BearerFormat = "JWT"
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
