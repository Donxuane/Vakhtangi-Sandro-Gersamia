
namespace BudgetingExpense.Domain.Contracts.IRepository.IGet;

public interface IGetUserCredentials
{
    public Task<IEnumerable<string>> GetAllUsersIdiesAsync();
}

