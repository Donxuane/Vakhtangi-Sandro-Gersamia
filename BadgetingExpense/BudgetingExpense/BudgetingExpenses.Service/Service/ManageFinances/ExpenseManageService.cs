using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.ManageFinances;

public class ExpenseManageService : IExpenseManageService
{
    private readonly IUnitOfWork _unitOfWork;
    public ExpenseManageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> AddExpenseCategoryAsync(string categoryName)
    {
        try
        {
            var category = new Category { Name = categoryName };
            await _unitOfWork.BeginTransaction();
            var result = await _unitOfWork.ExpenseManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return 0;
        }
    }

    public async Task<bool> AddExpenseAsync(Expense model)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.ExpenseManage.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.ExpenseManage.DeleteAsync(expenseId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var categories = await _unitOfWork.ExpenseManage.GetCategoriesAsync(userId);
            if (categories.Any())
            {
                return categories;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<bool> UpdateExpenseAsync(Expense expense)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.ExpenseManage.UpdateAsync(expense);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }



    public async Task<bool> UpdateCategoryAsync(Category Category)
    {

        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.ExpenseManage.UpdateCategoryAsync(Category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _unitOfWork.RollBackAsync();
            return false;
        }

    }

    public async Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var records = await _unitOfWork.ExpenseManage.GetAllAsync(userId);
            if (records.Any())
            {
                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

}
