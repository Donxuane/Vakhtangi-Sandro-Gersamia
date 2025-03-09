using BudgetingExpense.Domain.Contracts.IServices.IReposrts;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;

namespace BudgetingExpenses.Service.Service.Reports;

public class SavingsAnalyticService : ISavingsAnalyticService
{
    private readonly IUnitOfWork _unitOfWork;

    public SavingsAnalyticService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<double> SavingsAnalyticByPeriodAsync(double expense, double income)
    {
        try
        {
            return await Task.Run(() =>
            {
                var savings = income - expense;
                return savings;
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 0;
        }
    }

    public async Task<double> SavingPercentageAsync(double expense, double income)
    {
        try
        {
            return await Task.Run(() =>
            {
                var saving = income - expense;

                var percentage = saving * 100 / income;

                return Math.Round(percentage, 2);
            });

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 0;
        }
    }

    public async Task<(double expense, double income)> FinanceRecords(string userId, int month)
    {
        try
        {
            var expense = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(userId);
            var income = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            if (expense != null && income != null)
            {
                var period = DateTime.UtcNow.AddMonths(-month);
                if (expense.Any())
                {
                    var sumOfIncomes = income.Where(x => x.IncomeDate >= period).Sum(x => x.Amount);
                    var sumOfExpenses = expense.Where(x => x.Date >= period).Sum(x => x.Amount);
                    return (sumOfExpenses, sumOfIncomes);
                }
            return (0, 0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return (0, 0);
        }
    }
}
