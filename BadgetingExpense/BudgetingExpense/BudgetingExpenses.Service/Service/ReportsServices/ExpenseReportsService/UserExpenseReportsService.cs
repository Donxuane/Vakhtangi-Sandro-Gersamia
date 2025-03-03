using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices.IExpenseReportsService;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.ReportsServices.ExpenseReportsService;

public class UserExpenseReportsService : IExpenseReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserExpenseReportsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriod(GetRecordsPeriod model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(model.UserId);
            if(records != null)
            {
                List<ExpenseRecord> final = [];
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                var filteredBasedPeriod = records.Where(x => x.Date >= period).OrderByDescending(x=>x.Amount);
                foreach(var item in filteredBasedPeriod)
                {
                    if(filteredBasedPeriod.Count() == 10)
                    {
                        break;
                    }
                    final.Add(item);
                }
                return final;
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> GetAllRecords(string userId)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(userId);
            if(records!=null && records.Any())
            {
                return records;
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if(model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category);
                }
                return records.Where(x => x.CategoryName == model.Category && x.Date >= period);
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriod(GetRecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(model.UserId);
            if(records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if(model.Currency>0 && model.Period == 0)
                {
                    return records.Where(x => x.Currency == model.Currency);
                }
                return records.Where(x => x.Currency == model.Currency && x.Date >= period);
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
