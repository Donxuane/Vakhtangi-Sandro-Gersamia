using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpense.api.ViewModels.IncomeViewModels
{
    public class UpdateIncomeViewModel
    {
        public Income? Income { get; set; }
        public CategoryDto? CategoryDto { get; set; }
    }
}
