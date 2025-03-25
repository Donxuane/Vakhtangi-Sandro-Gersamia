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
                if (!value.Contains('@')&& !string.Equals(argument.Key,"Password",StringComparison.OrdinalIgnoreCase))
                {
                    context.ActionArguments[argument.Key] = value.Trim().ToLower();
                }
            }
        }
    }
}
