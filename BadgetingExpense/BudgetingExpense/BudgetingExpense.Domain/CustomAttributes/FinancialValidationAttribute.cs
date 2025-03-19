using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true,Inherited = false)]
    public class FinancialValidationAttribute : ValidationAttribute
    {
        private readonly FinancialTypes _financialTypes;
        
        public FinancialValidationAttribute(FinancialTypes financialTypes)
        {
            _financialTypes = financialTypes;
               }
            }
}
