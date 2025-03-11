using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Notifications;

public class ToggleNotificationService : IToggleNotificationsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ToggleNotificationService> _logger;

    public ToggleNotificationService(IUnitOfWork unitOfWork,ILogger<ToggleNotificationService> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> ToggleNotificationsAsync(string userId, bool status)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ToggleNotificationsRepository.ToggleNotification(userId, status);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Email Status Updated\nUser:{id}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }
}
