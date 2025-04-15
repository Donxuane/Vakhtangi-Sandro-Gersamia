using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.CustomValidationAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public class CategoryTypeAttribute : Attribute
{
    public FinancialTypes FinancialTypes { get; }

    public CategoryTypeAttribute(FinancialTypes financialTypes)
    {
        FinancialTypes = financialTypes;
    }
}
