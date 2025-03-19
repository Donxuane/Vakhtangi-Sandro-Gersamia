using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true,Inherited = false)]
    public class FinancialValidationAttribute : ValidationAttribute
    {
        private readonly FinancialTypes _financialTypes;
        private readonly IGetAllCategory _getAllCategory;
        
        public FinancialValidationAttribute(FinancialTypes financialTypes)
        {
            _financialTypes = financialTypes;
            
        }

        public FinancialValidationAttribute(IGetAllCategory getAllCategory)
        {
            _getAllCategory = getAllCategory;
        }
        protected override ValidationResult? IsValid(object? value,ValidationContext validationContext)
        {
            if (value is int categoryId)
            {
               var category = _getAllCategory.GetAllCategoryAsync(categoryId);
               if (category ==(int) _financialTypes)
               {
                    return ValidationResult.Success;
               }
            }
           return new ValidationResult("cant match category type");
        }

       
    }
}
