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

    public async Task<bool> AddIncomeType(IncomeTypeDTO model)
    {
        var finalModel = new UserIncome
        {
            IncomeType = model.IncomeType,
            Currency = model.Currency,
            UserId = model.UserId
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
            var check = "";
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
}
