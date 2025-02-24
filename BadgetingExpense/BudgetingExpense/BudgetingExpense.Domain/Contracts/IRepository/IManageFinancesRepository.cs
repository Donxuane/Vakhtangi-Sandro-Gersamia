﻿using BudgetingExpense.Domain.Models;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository;

public interface IManageFinancesRepository<T> where T : class
{
    public Task<int> AddCategoryAsync(Category category);
    public Task AddAsync(T model);
    public void SetTransaction(DbTransaction transaction);
    public Task DeleteAsync(int Id);
    public Task<IEnumerable<T>> GetAllAsync(string UserId);
    public Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
}
