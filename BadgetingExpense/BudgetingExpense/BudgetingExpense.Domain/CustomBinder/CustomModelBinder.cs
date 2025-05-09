﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BudgetingExpense.Domain.CustomBinder;

public class CustomModelBinder<T> : IModelBinder where T : class, new()
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var model = new T();

        foreach (var property in typeof(T).GetProperties())
        { 
            var propertyResult = bindingContext.ValueProvider.GetValue(property.Name);
            var result = propertyResult.FirstOrDefault();
            object? valueToSet;
            if (property.PropertyType == typeof(string) && property.CanWrite &&
                !string.IsNullOrEmpty(result))
            {
                if (!result.Contains('@') && !property.Name.Equals("Password", StringComparison.OrdinalIgnoreCase)
                   && !property.Name.Equals("RepeatPassword", StringComparison.OrdinalIgnoreCase))
                {
                    valueToSet = result.Trim().ToLower();
                }
                else
                {
                    valueToSet = result;
                }
            }
            else
            {
                valueToSet = Convert.ChangeType(result, property.PropertyType);
            }
            property.SetValue(model, valueToSet);
        }
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}
