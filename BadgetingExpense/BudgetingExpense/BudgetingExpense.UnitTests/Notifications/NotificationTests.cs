using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpenses.Service.Service.Notifications;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Notifications;

public class NotificationTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<ToggleNotificationService>> _mockedToggleNotificationServiceLogger;
    private readonly IToggleNotificationsService _toggleNotificationsService;

    public NotificationTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedToggleNotificationServiceLogger = new Mock<ILogger<ToggleNotificationService>>();
        _toggleNotificationsService = new ToggleNotificationService(_mockedUnitOfWork.Object, _mockedToggleNotificationServiceLogger.Object);

    }

    [Fact]
    public async Task ToggleNotificationsAsync_ShouldSetNotificationsColumn_True()
    {
        string userId = "User Id";
        bool status = true;
        _mockedUnitOfWork.Setup(x => x.ToggleNotificationsRepository.ToggleNotification(userId, status))
            .Verifiable("Did Not Execute");

        var result = await _toggleNotificationsService.ToggleNotificationsAsync(userId,status);
        Assert.True(result);
        _mockedUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
        _mockedUnitOfWork.Verify(x=>x.ToggleNotificationsRepository.ToggleNotification(userId,status), Times.Once);
        _mockedUnitOfWork.Verify(x=>x.SaveChangesAsync(),Times.Once);
    }
}
