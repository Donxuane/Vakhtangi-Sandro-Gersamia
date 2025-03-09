using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;

namespace BudgetingExpenses.Service.Service.ReportsServices
{
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
            }
        }

        public async Task<double> SavingPercentageAsync(double expense, double income)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var saving = income - expense;

                    var percentage = (saving * 100) / income;

                    return percentage;
                });
           
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
              
            }
        }

      public async Task<(double expense, double income)> FinanceRecords(string userId, int month)
        {
            try
            {
             var expense = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(userId);
            var income = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            if (expense is not null && income is not null)
            {
                var period = DateTime.UtcNow.AddMonths(-month);
                var sumOfIncomes = income.Where(x => x.IncomeDate >= period).Sum(x => x.Amount);
                var sumOfExpenses = expense.Where(x => x.Date >= period).Sum(x => x.Amount);
                return (sumOfExpenses, sumOfIncomes);
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
           

            return (0,0);
        }
    }
}
