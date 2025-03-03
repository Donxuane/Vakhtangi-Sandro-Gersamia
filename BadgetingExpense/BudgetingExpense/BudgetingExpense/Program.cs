using System.Data.Common;
using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.DataAccess.Repository.FinanceManageRepository;
using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.Repository.LimitsRepository;
using BudgetingExpense.DataAccess.Repository.ReportsRepository;
using BudgetingExpense.DataAccess.UnitOfWork;
using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IAuthenticationService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IFinanceManageServices;
using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsService;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service;
using BudgetingExpenses.Service.Service.AuthenticationService;
using BudgetingExpenses.Service.Service.ManageFinanceServices;
using BudgetingExpenses.Service.Service.ReportsServices;
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("default"), b=>b.MigrationsAssembly("BudgetingExpense.DataAccess")));

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

builder.Services.AddScoped<DbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("default")));

builder.Services.ConfigureJWTBearerToken(builder.Configuration);

builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IManageFinancesRepository<Expense>, ExpenseManageRepository>();
builder.Services.AddScoped<IManageFinancesRepository<Income>, IncomeManageRepository>();
builder.Services.AddScoped<IIncomeRecordsRepository, IncomeReportsRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IIncomeManageService, IncomeManageService>();
builder.Services.AddScoped<IExpenseManageService, ExpenseManageService>();
builder.Services.AddScoped<IIncomeReportsService, UserIncomeReportsService>();
builder.Services.AddScoped<ILimitsRepository, LimitsRepository>();
builder.Services.AddScoped<ILimitsManageService, LimitsService>();
builder.Services.AddScoped<ConfigureRoles>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<ConfigureRoles>();
    roles.RoleCeeder().GetAwaiter().GetResult();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<UserItemsMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
