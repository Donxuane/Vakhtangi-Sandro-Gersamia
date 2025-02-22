using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;

namespace BudgetingExpenses.Service.Service;

public class ExpenceManageService : IExpenseManageService
{
    private readonly IUnitOfWork _unitOfWork;
    public ExpenceManageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddExpenseCategoryAsync(string categoryName)
    {
        try
        {
            var category = new Category { Name = categoryName };
            await _unitOfWork.ExpenseTypeManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return false;
    }

    public Task<bool> AddExpenseAsync(ExpenseDto model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Income>?> GetAllExpenseRecordsAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
