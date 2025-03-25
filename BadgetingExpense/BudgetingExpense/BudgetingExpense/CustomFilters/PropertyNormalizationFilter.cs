using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetingExpense.Api.CustomFilters;

public class PropertyNormalizationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach(var argument in context.ActionArguments)
        {
            if(argument.Value is string value)
            {
                if (!value.Contains('@')&& argument.Key!="Password")
                {
                    context.ActionArguments[argument.Key] = value.Trim().ToLower();
                }
            }
        }
    }
}
