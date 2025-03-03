using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsService;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel;

namespace BudgetingExpenses.Service.Service.ReportsServices;

public class UserIncomeReportsService : IIncomeReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserIncomeReportsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrecncyPeriod(GetIncomeRecord model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(model.UserId);
            var period = DateTime.UtcNow.AddMonths(-model.Period);
            return records.Where(x => x.Currency == model.Currency && x.IncomeDate >= period);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
