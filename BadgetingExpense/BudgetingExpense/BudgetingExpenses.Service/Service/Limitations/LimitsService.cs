using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using Microsoft.Extensions.Logging;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;

namespace BudgetingExpenses.Service.Service.Limitations;

public class LimitsService : ILimitsManageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LimitsService> _logger;
    private readonly IGetRepository _getRepository;
    public LimitsService(IUnitOfWork unitOfWork, ILogger<LimitsService> logger, IGetRepository getRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _getRepository = getRepository;
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
            _logger.LogInformation("Limitation deleted Id:{id}",limitId);
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

    public async Task<IEnumerable<(LimitationsView,string)>> GetLimitsDetails(string userId)
    {
        try
        {
            List<(LimitationsView, string)> list = [];
            var result = await _unitOfWork.BudgetPlaningRepository.GetLimitsInfo(userId);
            foreach(var record in result)
            {
                list.Add((record, await _getRepository.GetCategoryNameAsync(record.CategoryId)));
            }
            return list;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
}

