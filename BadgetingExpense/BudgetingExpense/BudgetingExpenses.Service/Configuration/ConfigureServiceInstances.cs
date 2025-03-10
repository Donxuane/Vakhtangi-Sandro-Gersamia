using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifyUser;
using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpenses.Service.Service.Authentication;
using BudgetingExpenses.Service.Service.Limitations;
using BudgetingExpenses.Service.Service.ManageFinances;
using BudgetingExpenses.Service.Service.Messaging;
using BudgetingExpenses.Service.Service.NotifyUser;
using BudgetingExpenses.Service.Service.Reports;
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
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBudgetPlanningService, BudgetPlanningService>();
        services.AddScoped<ISavingsAnalyticService, SavingsAnalyticService>();
        services.AddScoped<IIncomeReceiveNotificationService, IncomeReceiveNotificationService>();
        services.AddScoped<IForecastService,IncomeForecastService>();
        services.AddScoped<IForecastService, ExpenseForecastService>();
    }
}
