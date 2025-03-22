using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.MainModels;

   public interface IGetCategory
   {
       public int GetCategoryTypeAsync( int categoryId);
   }
}
