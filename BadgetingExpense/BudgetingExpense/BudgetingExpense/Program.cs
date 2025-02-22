using System.Data.Common;
using BudgetingExpense.api.Configuration;
using BudgetingExpense.DataAccess.Repository;
using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.UnitOfWork;
using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.IServiceContracts;
using BudgetingExpenses.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("default2"), b=>b.MigrationsAssembly("BudgetingExpense.DataAccess")));

builder.Services.AddIdentity<IdentityModel, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<DbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("default2")));

builder.Services.ConfigureJWTBearerToken(builder.Configuration);

builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IManageFinances<UserExpenses>, ExpenseTypeManage>();
builder.Services.AddScoped<IManageFinances<UserIncome>, IncomeTypeManage>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IIncomeManageService, IncomeManageService>();

builder.Services.AddScoped<ConfigureRoles>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<ConfigureRoles>();
    roles.RoleCeeder().Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
