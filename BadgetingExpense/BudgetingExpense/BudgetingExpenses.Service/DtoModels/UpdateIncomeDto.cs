﻿using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpenses.Service.DtoModels;

public class UpdateIncomeDto
{
    public int Id { get; set; }
    public Currencies? Currency { get; set; }
    public double? Amount { get; set; }
    [CategoryType(FinancialTypes.Income)]
    public int? CategoryId { get; set; }
    public DateTime? Date { get; set; }
}
