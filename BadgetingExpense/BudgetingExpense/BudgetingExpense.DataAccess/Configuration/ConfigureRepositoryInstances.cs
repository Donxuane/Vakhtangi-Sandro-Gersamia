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
        services.AddScoped<IGetRepository, GetRepository>();
        services.AddScoped<IToggleNotificationsRepository,ToggleNotificationsRepository>();
        services.AddScoped<IBudgetPlaningRepository, BudgetPlaningRepository>();
        services.AddScoped<IGetAllCategory, GetAllCategories>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        services.AddScoped<Func<IAuthentication>>(x => x.GetRequiredService<IAuthentication>);
        services.AddScoped<Func<IManageFinancesRepository<Expense>>>(x=>x.GetRequiredService<IManageFinancesRepository<Expense>>);
        services.AddScoped<Func<IManageFinancesRepository<Income>>>(x => x.GetRequiredService<IManageFinancesRepository<Income>>);
        services.AddScoped<Func<IIncomeRecordsRepository>>(x=>x.GetRequiredService<IIncomeRecordsRepository>);
        services.AddScoped<Func<IExpenseRecordsRepository>>(x=>x.GetRequiredService<IExpenseRecordsRepository>);
        services.AddScoped<Func<IBudgetLimitsRepository>>(x => x.GetRequiredService<IBudgetLimitsRepository>);
        services.AddScoped<Func<IGetRepository>>(x => x.GetRequiredService<IGetRepository>);
        services.AddScoped<Func<IToggleNotificationsRepository>>(x => x.GetRequiredService<IToggleNotificationsRepository>);
        services.AddScoped<Func<IBudgetPlaningRepository>>(x => x.GetRequiredService<IBudgetPlaningRepository>);
    }
}
