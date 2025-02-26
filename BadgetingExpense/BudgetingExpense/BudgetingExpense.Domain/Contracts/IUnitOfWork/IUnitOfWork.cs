﻿using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IUnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public IAuthentication Authentication { get; }
    public IManageFinancesRepository<Expense> ExpenseManage { get; }
    public IManageFinancesRepository<Income> IncomeManage { get; }
}
