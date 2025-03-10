﻿using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class UpdateExpenseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public Currencies Currency { get; set; }
    [Required]
    public double Amount { get; set; }
    public int? CategoryId { get; set; }
    [Required]
    public DateTime Date { get; set; }
}