using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;

namespace BudgetingExpenses.Service.Service
{
  public class LimitsService : ILimitsManageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LimitsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  async Task<bool> SetLimits(Limits limits)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.LimitsRepository.SetLimitAsync(limits);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RollBackAsync();
            }
            return false;
        }
    }
}
