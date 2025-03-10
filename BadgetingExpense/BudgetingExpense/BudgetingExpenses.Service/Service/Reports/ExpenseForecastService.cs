using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.Reports
{
  public class ExpenseForecastService : IForecastService
  {
      private readonly IUnitOfWork _unitOfWork;

      public ExpenseForecastService(IUnitOfWork unitOfWork)
      {
          _unitOfWork= unitOfWork;
      }
        public async Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId)
        {
            var expenseRecords = await _unitOfWork.ExpenseRecords.GetUserExpenseRecords(userId);
            var filteredByCurrency = expenseRecords.DistinctBy(x => x.Currency);
            var filteredByCategory = expenseRecords.DistinctBy(x => x.CategoryName);
            List < GetForecastCategory > model= [];
            foreach (var currency in filteredByCurrency )
            {
                foreach (var category in filteredByCategory)
                {
                    var count = expenseRecords.Count(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName);
                    var amount = expenseRecords.Count(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName);
                 model.Add(new GetForecastCategory()
                 {
                     Expected = amount/count,
                     CategoryName = category.CategoryName,
                     Currency = currency.Currency
                 });
                }
            } 
            return model;
        }
    }
}
