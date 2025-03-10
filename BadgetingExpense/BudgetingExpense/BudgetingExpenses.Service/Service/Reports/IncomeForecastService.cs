using BudgetingExpense.Domain.Contracts.IServices.IReposrts;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.Reports
{
    public class IncomeForecastService : IIncomeForecastService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncomeForecastService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId)
        {
            var incomeRecords = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            var filteredByCurrency = incomeRecords.DistinctBy(x => x.Currency);
            var filteredByCategory = incomeRecords.DistinctBy(x => x.CategoryName);
            List<GetForecastCategory> model = [];
            foreach (var currency in filteredByCurrency)
            {
                foreach (var category in filteredByCategory)
                {
                    var amount = incomeRecords.Where(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName).Sum(x=>x.Amount);
                    model.Add(new GetForecastCategory
                    {
                        Amount = amount,
                        CategoryName = category.CategoryName,
                        Currencies = currency.Currency
                    });
                }
            }
            return model;
        }
    }
}
