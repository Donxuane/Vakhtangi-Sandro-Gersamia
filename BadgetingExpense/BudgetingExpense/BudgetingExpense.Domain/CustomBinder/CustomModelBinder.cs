using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BudgetingExpense.Domain.CustomBinder;

public class CustomModelBinder<T> : IModelBinder where T : class, new()
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var model = new T();

        foreach (var property in typeof(T).GetProperties())
        {
            if (property.PropertyType == typeof(string) && property.CanWrite)
            {
                var propertyResult = bindingContext.ValueProvider.GetValue(property.Name);
                if (propertyResult.Length != 0)
                {
                    var result = propertyResult.FirstOrDefault();

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (!result.Contains('@') && !property.Name.Equals("Password", StringComparison.OrdinalIgnoreCase)
                           && !property.Name.Equals("RepeatPassword", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.Trim().ToLower();
                        }
                        property.SetValue(model, result);
                    }
                }
            }
        }
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}
