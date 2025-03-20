using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class SavingsAnalyticService : ISavingsAnalyticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SavingsAnalyticService> _logger;

    public SavingsAnalyticService(IUnitOfWork unitOfWork, ILogger<SavingsAnalyticService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<IEnumerable<SavingsPeriod>?> GetSavingsAnalyticsAsync(string userId, int? month)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var records = await _unitOfWork.SavingsRepository.GetSavingsAnalyticsAsync(userId, month);
            await _unitOfWork.SaveChangesAsync();
            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            await _unitOfWork.RollBackAsync();
            return null;
        }
    }
}
