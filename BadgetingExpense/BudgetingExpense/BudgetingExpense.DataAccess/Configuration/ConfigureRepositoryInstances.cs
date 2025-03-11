using BudgetingExpense.DataAccess.Repository.FinanceManageRepository;
using BudgetingExpense.DataAccess.Repository.Get;
using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.Repository.LimitsRepository;
using BudgetingExpense.DataAccess.Repository.NotificationsRepository;
using BudgetingExpense.DataAccess.Repository.ReportsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IFinance;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Contracts.IRepository.INotifications;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetingExpense.DataAccess.Configuration;

public static class ConfigureRepositoryInstances
{
    public static void AddRepositoryInstances(this IServiceCollection services)
    {
        services.AddScoped<IAuthentication, Authentication>();
        services.AddScoped<IManageFinancesRepository<Expense>, ExpenseManageRepository>();
        services.AddScoped<IManageFinancesRepository<Income>, IncomeManageRepository>();
        services.AddScoped<IIncomeRecordsRepository, IncomeReportsRepository>();
        services.AddScoped<IExpenseRecordsRepository, ExpenseReportsRepository>();
        services.AddScoped<IBudgetLimitsRepository, LimitsRepository>();
        services.AddScoped<IBudgetPlaningRepository, BudgetPlaningRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IGetRepository, GetRepository>();
        services.AddScoped<IToggleNotificationsRepository,ToggleNotificationsRepository>();
    }
}
