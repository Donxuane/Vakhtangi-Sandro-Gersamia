<<<<<<< HEAD
﻿using BudgetingExpense.Domain.CustomValidationAttributes;
using BudgetingExpense.Domain.Enums;
=======
﻿using BudgetingExpense.Domain.Enums;
>>>>>>> 578beaae104d3c323dff0b084b9ee2acc55bb79d
using System.ComponentModel.DataAnnotations;
using BudgetingExpense.Domain.CustomValidationAttributes;

namespace BudgetingExpense.Domain.Models.MainModels;

public class Expense
{
    [Key]
    public int? Id { get; set; }
    [Required]
    public Currencies Currency { get; set; }
    [Required]
    public double Amount { get; set; }
    [CategoryTypeValidation(FinancialTypes.Expense)]
    public int? CategoryId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string UserId { get; set; }
}
