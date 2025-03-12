using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class SavingsAnalyticService : ISavingsAnalyticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SavingsAnalyticService> _logger;

    public SavingsAnalyticService(IUnitOfWork unitOfWork, ILogger<SavingsAnalyticService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Savings?> SavingsAnalyticsAsync(string userId, int month)
    {
        try
        {
            var (expense, income) = await FinanceRecordsAsync(userId, month);
            var actual = income - expense;
            var percentage = await SavingPercentageAsync(expense, income);
            return new Savings { Expense = expense, Income = income, Saved = actual, Percentage = percentage};
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }

    private async Task<double> SavingPercentageAsync(double expense, double income)
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
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return 0;
        }
    }

    private async Task<(double expense, double income)> FinanceRecordsAsync(string userId, int month)
    {
        try
        {
            var expense = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(userId);
            var income = await _unitOfWork.IncomeRecords.GetUserIncomeRecordsAsync(userId);
            if (income != null)
            {
                var period = DateTime.UtcNow.AddMonths(-month);
                var sumOfIncomes = income.Where(x => x.IncomeDate >= period).Sum(x => x.Amount);
                double sumOfExpenses = 0;
                if (expense.Any())
                {
                    sumOfExpenses = expense.Where(x => x.Date >= period).Sum(x => x.Amount);
                }
                return (sumOfExpenses, sumOfIncomes);
            }
            return (0, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return (0, 0);
        }
    }
}
