using BudgetingExpense.Domain.Contracts.IServiceContracts.IAuthenticationService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IFinanceManageServices;
using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IMessageService;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;
using BudgetingExpenses.Service.Service.AuthenticationService;
using BudgetingExpenses.Service.Service.LimitsService;
using BudgetingExpenses.Service.Service.ManageFinanceServices;
using BudgetingExpenses.Service.Service.MessageService;
using BudgetingExpenses.Service.Service.ReportsServices;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetingExpenses.Service.Configuration;

public static class ConfigureServiceInstances
{
    public static void AddServiceInstances(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IIncomeManageService, IncomeManageService>();
        services.AddScoped<IIncomeReportsService, UserIncomeReportsService>();
        services.AddScoped<IExpenseManageService, ExpenseManageService>();
        services.AddScoped<IExpenseReportsService, UserExpenseReportsService>();
        services.AddScoped<ILimitsManageService, LimitsService>();
        services.AddScoped<IEmailService, SendMailService>();
        services.AddScoped<IBudgetPlanningService, BudgetPlanningService>();
        services.AddScoped<ISavingsAnalyticService, SavingsAnalyticService>();
    }
}
