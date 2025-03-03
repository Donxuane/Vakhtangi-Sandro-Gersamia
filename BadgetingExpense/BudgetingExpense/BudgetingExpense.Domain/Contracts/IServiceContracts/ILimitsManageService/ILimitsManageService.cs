using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService
{
    public interface ILimitsManageService
    {
        Task<bool> SetLimits(Limits limits);
    }
}
