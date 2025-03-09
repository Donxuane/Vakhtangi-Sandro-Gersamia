namespace BudgetingExpenses.Service.DtoModels;

public  class LimitsDto
{   
    public int CategoryId { get; set; }
    public double Amount { get; set; }
    public int Period { get; set; }
    public DateTime DateAdded { get; set; }
}
