namespace BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

public class RecordsDto
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public int Currency { get; set; }
    public string CategoryName { get; set; }
}
