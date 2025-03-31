using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace BudgetingExpense.Domain.CustomBinder;

public class CustomModelBinder<T> : IModelBinder where T : class, new()
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var model = new T();
        bool check = false;

        foreach (var property in typeof(T).GetProperties())
        { 
            var propertyResult = bindingContext.ValueProvider.GetValue(property.Name);
            var result = propertyResult.FirstOrDefault();
            object? valueToSet = null;
            if (property.PropertyType == typeof(string) && property.CanWrite)
            {
                if (propertyResult.Length != 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (!result.Contains('@') && !property.Name.Equals("Password", StringComparison.OrdinalIgnoreCase)
                           && !property.Name.Equals("RepeatPassword", StringComparison.OrdinalIgnoreCase))
                        {
                            valueToSet = result.Trim().ToLower();
                            check = true;
                        }
                        
                    }
                }
            }
            else
            {
                valueToSet = Convert.ChangeType(result, property.PropertyType, CultureInfo.InvariantCulture);
            }
            property.SetValue(model, valueToSet);
        }
        if (check == true)
        {
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        return Task.CompletedTask;
    }
}
