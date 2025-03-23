using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true,Inherited = false)]
    public class CategoryTypeValidationAttribute : Attribute
    {
        public FinancialTypes FinancialTypes { get; }
        
        public CategoryTypeValidationAttribute(FinancialTypes financialTypes)
        {
            FinancialTypes = financialTypes;
        }    
    }
}
