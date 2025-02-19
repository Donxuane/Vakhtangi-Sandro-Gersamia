namespace BudgetingExpense.Domain.Contracts.IRepository;

public interface IManageFinances<T> where T : class
{
    public Task Add(T model);
    public Task Update(T model);
    public Task Delete(int Id);
    public Task<IEnumerable<T>> GetAll(string UserId);
}
