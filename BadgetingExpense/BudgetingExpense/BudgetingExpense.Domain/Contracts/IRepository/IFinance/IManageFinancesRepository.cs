﻿using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository.IFinance;

public interface IManageFinancesRepository<T> where T : class
{
    public Task<int> AddCategoryAsync(Category category);
    public Task AddAsync(T model);
    public void SetTransaction(DbTransaction transaction);
    public Task DeleteAsync(int Id, string userId);
    public Task<IEnumerable<T>> GetAllAsync(string UserId);
    public Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
    public Task UpdateCategoryAsync(Category category);
    public Task UpdateAsync(Update model); 
}
