using System;
using System.Collections.Generic;
namespace BudgetingExpense.Domain.Models.DatabaseViewModels;

public class IncomeRecord
{
    public string UserId { get; set; }
    public DateTime IncomeDate { get; set; }
    public double Amount { get; set; }
    public int Currency { get; set; }
    public string CategoryName { get; set; }
}
