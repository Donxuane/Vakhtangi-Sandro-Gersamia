using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace BudgetingExpense.Api.CustomFilters;

public class CategoryValidationFilter : IActionFilter
{
    private readonly IGetCategory _getCategory;
    private readonly ILogger<CategoryValidationFilter> _logger;
    public CategoryValidationFilter(IGetCategory getCategory, ILogger<CategoryValidationFilter> logger)
    {
        _getCategory = getCategory;
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context) 
    {
        try
        {
            var model = context.ActionArguments.Values.FirstOrDefault();
            if (model != null)
            {
                var properties = model.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<CategoryTypeValidationAttribute>();
                    if (attribute != null)
                    {
                        var value = property.GetValue(model);
                        if (value is int categoryId)
                        {
                            var type = _getCategory.GetCategoryTypeAsync(categoryId);
                            if (type != (int)attribute.FinancialTypes)
                            {
                                context.Result = new BadRequestObjectResult("Category Id Does Not Match The Context");
                                return;
                            }
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
