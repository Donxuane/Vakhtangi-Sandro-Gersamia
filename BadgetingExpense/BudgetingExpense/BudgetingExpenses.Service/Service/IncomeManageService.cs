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

    public async Task<bool> AddIncomeCategory(string CategoryName)
    {
        try
        {
            var category = new Category { Name = CategoryName };
            await _unitOfWork.IncomeTypeManage.AddCategory(category);
            await _unitOfWork.SaveChangesAsync();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return false;
    }

    public async Task<bool> AddIncomeType(IncomeTypeDTO model)
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
            await _unitOfWork.IncomeTypeManage.Add(finalModel);
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

    public async Task<bool> DeleteIncomeType(int incomeTypeId)
    {
        try
        {
            await _unitOfWork.IncomeTypeManage.Delete(incomeTypeId);
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

    public async Task<IEnumerable<Category>?> GetAllIncomeCategoryRecords(string userId)
    {
        try
        {
            var categories = await _unitOfWork.IncomeTypeManage.GetCategories(userId);
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

    public async Task<IEnumerable<Income>?> GetAllIncomeRecords(string userId)
    {
        try
        {
            var incomeRecords = await _unitOfWork.IncomeTypeManage.GetAll(userId);
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
