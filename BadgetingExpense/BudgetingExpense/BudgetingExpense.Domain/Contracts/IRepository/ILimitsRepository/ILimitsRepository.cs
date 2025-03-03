using BudgetingExpense.Domain.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IRepository.ILimitsRepository
{
    public interface ILimitsRepository
    {
        public Task SetLimitAsync(Limits limits);
        public void SetTransaction(DbTransaction transaction);
        public Task DeleteLimitsAsync(int id);
        public Task UpdateLimitsAsync(Limits limits);
    }
}
