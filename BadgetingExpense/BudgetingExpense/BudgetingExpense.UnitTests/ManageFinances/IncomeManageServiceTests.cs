using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.ManageFinances;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.ManageFinances;

public class IncomeManageServiceTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<IncomeManageService>> _mockedLogger;
    private readonly Mock<IIncomeReceiveNotificationService> _mockedNotificationService;
    private readonly IncomeManageService _service;

    public IncomeManageServiceTests()
    {
        _mockedLogger = new Mock<ILogger<IncomeManageService>>();
        _mockedNotificationService = new Mock<IIncomeReceiveNotificationService>();
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _service = new IncomeManageService(_mockedUnitOfWork.Object, _mockedNotificationService.Object, _mockedLogger.Object);
    }

    [Fact]
    public async Task AddIncomeAsync_ShouldAddIncome_SendEmailToUser()
    {
        var model = new Income { Id = 1 };
        _mockedUnitOfWork.Setup(x => x.IncomeManage.AddAsync(model))
            .GetType();
        _mockedNotificationService.Setup(x => x.NotifyIncomeAsync(model))
            .ReturnsAsync(true);
        var result = await _service.AddIncomeAsync(model);
        Assert.True(result);
        _mockedNotificationService.Verify(x=>x.NotifyIncomeAsync(model),Times.Once);
        _mockedUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockedUnitOfWork.Verify(x => x.IncomeManage.AddAsync(model), Times.Once);
        _mockedUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
