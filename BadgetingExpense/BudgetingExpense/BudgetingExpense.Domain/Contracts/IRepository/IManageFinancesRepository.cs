using BudgetingExpense.Domain.Models;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository;

public interface IManageFinancesRepository<T> where T : class
{
    public Task<int> AddCategory(Category category);
    public Task Add(T model);
    public void SetTransaction(DbTransaction transaction);
    public Task Delete(int Id);
    public Task<IEnumerable<T>> GetAll(string UserId);
    public Task<IEnumerable<Category>> GetCategories(string userId);
}
