using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.Reports;

public class UserExpenseReportsService : IExpenseReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserExpenseReportsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriodAsync(GetRecordsPeriod model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                List<ExpenseRecord> final = [];
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                var filteredBasedPeriod = records.Where(x => x.Date >= period).OrderByDescending(x => x.Amount);
                foreach (var item in filteredBasedPeriod)
                {
                    if (filteredBasedPeriod.Count() == 10)
                    {
                        break;
                    }
                    final.Add(item);
                }
                return final;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> GetAllRecordsAsync(string userId)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(userId);
            if (records != null && records.Any())
            {
                return records.OrderByDescending(x => x.Date);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriodAsync(GetRecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category).OrderByDescending(x => x.Date);
                }
                return records.Where(x => x.CategoryName == model.Category && x.Date >= period).OrderByDescending(x => x.Date);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriodAsync(GetRecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Currency > 0 && model.Period == 0)
                {
                    return records.Where(x => x.Currency == model.Currency).OrderByDescending(x => x.Date);
                }
                return records.Where(x => x.Currency == model.Currency && x.Date >= period).OrderByDescending(x => x.Date);
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
