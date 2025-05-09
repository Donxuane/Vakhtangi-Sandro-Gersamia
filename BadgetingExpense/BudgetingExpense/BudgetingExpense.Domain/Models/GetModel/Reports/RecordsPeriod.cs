﻿using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class RecordsPeriod
{
    [Required]
    public string UserId {  get; set; }
    [Required]
    public int Period {  get; set; }
    public Currencies Currency { get; set; } 
}
