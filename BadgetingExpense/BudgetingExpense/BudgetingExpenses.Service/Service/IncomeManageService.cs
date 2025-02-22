using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;

namespace BudgetingExpenses.Service.Service;

public class IncomeManageService : IIncomeManageService
{
    private readonly IUnitOfWork _unitOfWork;

    public IncomeManageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> AddIncomeCategoryAsync(string CategoryName)
    {
        try
        {
            var category = new Category { Name = CategoryName };
            var result = await _unitOfWork.IncomeManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return result;
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return 0;
    }

    public async Task<bool> AddIncomeAsync(IncomeDto model)
    {
        var finalModel = new Income
        {
            Currency = model.Currency,
            Amount = model.Amount,
            CategoryId = model.CategoryId,
            Date = model.Date,
            UserId = model.UserId,
        };
        try
        {    
            await _unitOfWork.IncomeManage.AddAsync(finalModel);
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteIncomeAsync(int incomeTypeId)
    {
        try
        {
            await _unitOfWork.IncomeManage.DeleteAsync(incomeTypeId);
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId)
    {
        try
        {
            var categories = await _unitOfWork.IncomeManage.GetCategoriesAsync(userId);
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

    public async Task<IEnumerable<Income>?> GetAllIncomeRecordsAsync(string userId)
    {
        try
        {
            var incomeRecords = await _unitOfWork.IncomeManage.GetAllAsync(userId);
            if (incomeRecords.Any())
            {
                return incomeRecords;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}
