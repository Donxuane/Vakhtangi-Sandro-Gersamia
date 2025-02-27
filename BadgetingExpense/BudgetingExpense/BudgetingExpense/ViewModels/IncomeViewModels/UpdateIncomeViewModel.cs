using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpense.api.ViewModels.IncomeViewModels
{
    public class UpdateIncomeViewModel
    {
        public UpdateIncomeDto? Income { get; set; }
        public Category? Category { get; set; }
    }
}
