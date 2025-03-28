﻿using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Limitations;

public class LimitsService : ILimitsManageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LimitsService> _logger;
    public LimitsService(IUnitOfWork unitOfWork, ILogger<LimitsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<bool> SetLimitsAsync(Limits limits)
    {
        try
        {
           
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.LimitsRepository.AddLimitAsync(limits);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Limit Set To Category:{category}, User:{userId}", limits.CategoryId, limits.UserId);
            
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }

    }

    public async Task<bool> DeleteLimitsAsync(int limitId, string userId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.LimitsRepository.DeleteLimitsAsync(limitId,userId);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Limitation Deletes Id:{id}",limitId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateLimitsAsync(Limits limits)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.LimitsRepository.UpdateLimitsAsync(limits);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("User:{userId} Updated Limit{id}", limits.UserId, limits.Id);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }

    }
}

