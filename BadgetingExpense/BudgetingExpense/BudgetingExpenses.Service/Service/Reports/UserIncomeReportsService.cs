using BudgetingExpense.Domain.Contracts.IServices.IReposrts;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.Reports;

public class UserIncomeReportsService : IIncomeReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserIncomeReportsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<IncomeRecord>?> GetAllRecords(string userId)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            if (records != null)
            {
                return records.OrderByDescending(x => x.IncomeDate);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category).OrderByDescending(x => x.IncomeDate);
                }
                return records.Where(x => x.CategoryName == model.Category && x.IncomeDate >= period).OrderByDescending(x => x.IncomeDate);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrencyPeriod(GetRecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Currency >= 0)
                {
                    return records.Where(x => x.Currency == model.Currency).OrderByDescending(x => x.IncomeDate);
                }
                return records.Where(x => x.Currency == model.Currency && x.IncomeDate >= period).OrderByDescending(x => x.IncomeDate);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
