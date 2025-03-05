using System.Data.Common;
using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.DataAccess.Configuration;
using BudgetingExpense.DataAccess.Repository.FinanceManageRepository;
using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.Repository.LimitsRepository;
using BudgetingExpense.DataAccess.Repository.ReportsRepository;
using BudgetingExpense.DataAccess.UnitOfWork;
using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitationsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IAuthenticationService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IFinanceManageServices;
using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IMessageService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Configuration;
using BudgetingExpenses.Service.Service.AuthenticationService;
using BudgetingExpenses.Service.Service.LimitsService;
using BudgetingExpenses.Service.Service.ManageFinanceServices;
using BudgetingExpenses.Service.Service.MessageService;
using BudgetingExpenses.Service.Service.ReportsServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorization();
builder.Services.ConfigureDatabaseRules(builder.Configuration);
builder.Services.ConfigureJWTBearerToken(builder.Configuration);
builder.Services.AddRepositoryInstances();
builder.Services.AddServiceInstances();
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
app.UseMiddleware<UserCredentialsMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
