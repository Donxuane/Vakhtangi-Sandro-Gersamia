﻿using System.ComponentModel.DataAnnotations;

namespace BudgetingExpenses.Service.DtoModels;

public class ExpenseDto
{
    [Required]
    public int Currency {  get; set; }
    [Required]
    public double Amount {  get; set; }
    public int? CategoryId {  get; set; }
    [Required]
    public DateTime Date {  get; set; }
    [Required]
    public string UserId {  get; set; }
}
