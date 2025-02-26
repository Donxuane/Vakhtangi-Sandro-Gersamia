using BudgetingExpense.Domain.Models;
using BudgetingExpense.Domain.Models.DtoModels;

namespace BudgetingExpense.api.ViewModels
{
    public class UpdateIncomeViewModel
    {
        public Income Income { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
