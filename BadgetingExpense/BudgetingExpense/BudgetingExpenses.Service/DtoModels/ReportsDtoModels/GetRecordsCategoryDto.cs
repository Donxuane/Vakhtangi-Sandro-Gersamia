using BudgetingExpense.Domain.CustomBinder;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpenses.Service.DtoModels.ReportsDtoModels;

[ModelBinder(typeof(CustomModelBinder<GetRecordsCategoryDto>))]
public class GetRecordsCategoryDto
{
    public int Period {  get; set; }
    public string? CategoryName { get; set; }
}
