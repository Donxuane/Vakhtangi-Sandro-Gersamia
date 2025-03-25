using BudgetingExpense.Domain.CustomBinder;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.MainModels;
[ModelBinder(typeof(CustomModelBinder<Category>))]
public class Category
{
    public int? Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int? Type { get; set; }
}
