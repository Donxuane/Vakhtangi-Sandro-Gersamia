﻿using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using BudgetingExpense.Domain.CustomValidationAttributes;

namespace BudgetingExpense.Domain.Models.MainModels;

public class Income
{
    [Key]
    public int Id { get; set; }
    [Required]
    public Currencies Currency { get; set; }
    [Required]
    public double Amount { get; set; }
    [CategoryType(FinancialTypes.Income)]
    public int? CategoryId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string UserId { get; set; }
}
