using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.CustomValidationAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public class CategoryTypeValidationAttribute : Attribute
{
    public FinancialTypes FinancialTypes { get; }

    public CategoryTypeValidationAttribute(FinancialTypes financialTypes)
    {
        FinancialTypes = financialTypes;
    }
}
