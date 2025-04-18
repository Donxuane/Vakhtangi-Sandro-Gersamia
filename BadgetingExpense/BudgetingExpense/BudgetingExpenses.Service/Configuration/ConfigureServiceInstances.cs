﻿using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpenses.Service.BackgroundServices;
using BudgetingExpenses.Service.Service.ApiService;
using BudgetingExpenses.Service.Service.Authentication;
using BudgetingExpenses.Service.Service.Limitations;
using BudgetingExpenses.Service.Service.ManageFinances;
using BudgetingExpenses.Service.Service.Messaging.Email;
using BudgetingExpenses.Service.Service.Notifications;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetingExpenses.Service.Configuration;

public static class ConfigureServiceInstances
{
    public static IServiceCollection AddServiceInstances(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IIncomeManageService, IncomeManageService>();
        services.AddScoped<IIncomeReportsService, IncomeReportsService>();
        services.AddScoped<IExpenseManageService, ExpenseManageService>();
        services.AddScoped<IExpenseReportsService, ExpenseReportsService>();
        services.AddScoped<ILimitsManageService, LimitsService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISavingsAnalyticService, SavingsAnalyticService>();
        services.AddScoped<IIncomeReceiveNotificationService, IncomeReceiveNotificationService>();
        services.AddScoped<IncomeForecastService<IncomeRecord>,IncomeForecastService>();
        services.AddScoped<IncomeForecastService<ExpenseRecord>, ExpenseForecastService>();
        services.AddScoped<IToggleNotificationsService, ToggleNotificationService>();
        services.AddScoped<ILimitNotificationService, LimitNotificationService>();
        services.AddScoped<IExpenseAddedNotificationService, ExpenseAddedNotificationService>();
        services.AddScoped<ITokenAuthenticationService, TokenAuthenticationService>();
        services.AddScoped<ICurrencyRateService,CurrencyRateService>();
        services.AddHostedService<LimitsCleanupService>();
        services.AddHttpContextAccessor();

        return services;
    }
}
