﻿using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;

namespace BudgetingExpenses.Service.Service.LimitsService
{
    public class LimitsService : ILimitsManageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LimitsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> SetLimits(Limits limits)
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

        public async Task<bool> DeleteLimits(int LimitId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.LimitsRepository.DeleteLimitsAsync(LimitId);
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

        public async Task<bool> UpdateLimits(Limits limits)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.LimitsRepository.UpdateLimitsAsync(limits);
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
