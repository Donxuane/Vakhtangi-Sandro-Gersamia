using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BudgetingExpense.Domain.CustomValidationAttributes;
[AttributeUsage(AttributeTargets.Property, AllowMultiple =true, Inherited =false)]
public class NormalizeInputAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is string input)
        {
            var checkNumber = new Regex(@"\d");
            if(checkNumber.IsMatch(input))
            {
                return new ValidationResult("Avoid numbers!");
            }
            return ValidationResult.Success;
        }
        return new ValidationResult("Input value does not match!");
    }
}
