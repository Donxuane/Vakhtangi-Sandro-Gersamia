using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpense.api.ViewModels.ExpenseViewModel
{
    public class UpdateExpenseViewModel
    {
        public UpdateExpenseDto expenses { get; set; }
        public Category category { get; set; }

    }
}
