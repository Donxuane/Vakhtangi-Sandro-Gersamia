namespace BudgetingExpenses.Service.DtoModels;

public class IncomeDto
{
    public int Currency { get; set; }
    public double Amount {  get; set; }
    public int CategoryId {  get; set; }
    public DateTime Date { get; set; }
}
