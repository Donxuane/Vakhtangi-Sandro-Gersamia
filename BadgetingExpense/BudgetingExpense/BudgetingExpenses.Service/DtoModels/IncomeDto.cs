using BudgetingExpense.Domain.CustomAttributes;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public class IncomeDto
{
    public Currencies Currency { get; set; }
    public double Amount {  get; set; }
    [CategoryTypeValidation(FinancialTypes.Income)]
    public int CategoryId {  get; set; }
    public DateTime Date { get; set; }
}
