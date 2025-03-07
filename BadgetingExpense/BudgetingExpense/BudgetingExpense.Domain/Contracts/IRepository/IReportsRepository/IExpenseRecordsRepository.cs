﻿using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;

public interface IExpenseRecordsRepository
{
    public Task<IEnumerable<ExpenseRecord>> GetUserExpenseRecords(string userId);
}
