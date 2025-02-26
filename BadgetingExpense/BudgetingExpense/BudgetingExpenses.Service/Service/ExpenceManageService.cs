using BudgetingExpense.Domain.Contracts.IUnitOfWork;
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

    public async Task<int> AddExpenseCategoryAsync(string categoryName)
    {
        try
        {
            var category = new Category { Name = categoryName };
            var result = await _unitOfWork.ExpenseManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return result;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return 0;
    }

    public async Task<bool> AddExpenseAsync(ExpenseDto model)
    {
        try
        {
            var expense = new Expense
            {
                Amount = model.Amount,
                Currency = model.Currency,
                CategoryId = model.CategoryId,
                Date = model.Date,
                UserId = model.UserId
            };
            await _unitOfWork.ExpenseManage.AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return false;
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId)
    {
        try
        {
            await _unitOfWork.ExpenseManage.DeleteAsync(expenseId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return false;
    }

    public async Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId)
    {
        try
        {
            var categories = await _unitOfWork.ExpenseManage.GetCategoriesAsync(userId);
            if (categories.Any())
            {
                return categories;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId)
    {
        try
        {
            var records = await _unitOfWork.ExpenseManage.GetAllAsync(userId);
            if (records.Any())
            {
                return records;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}
