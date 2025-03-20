using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class SavingsAnalyticsTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<SavingsAnalyticService>> _mockedLogger;
    private readonly ISavingsAnalyticService _savingsAnalyticsService;

    public SavingsAnalyticsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<SavingsAnalyticService>>();
        _savingsAnalyticsService = new SavingsAnalyticService(_mockedUnitOfWork.Object,_mockedLogger.Object);
    }

    [Fact]
    public async Task GetSavingsAnalytics_ReturnsDataTest()
    {
        string userId = "userId";
        int month = 1;

        var expectedValues = new List<SavingsPeriod>
        {
            new() {},
            new() {},
            new() {}
        };
        _mockedUnitOfWork.Setup(x=>x.SavingsRepository.GetSavingsAnalyticsAsync(userId,month))
            .ReturnsAsync(expectedValues);
        var result = await _savingsAnalyticsService.GetSavingsAnalyticsAsync(userId, month);

        Assert.NotNull(result);
        Assert.Equal(expectedValues, result);
        _mockedUnitOfWork.Verify(x=>x.BeginTransactionAsync(), Times.Once);
        _mockedUnitOfWork.Verify(x=>x.SavingsRepository.GetSavingsAnalyticsAsync(userId,month), Times.Once);
        _mockedUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        _mockedUnitOfWork.VerifyNoOtherCalls();
    }
    [Fact]
    public async Task GetSavingsAnalyticsAsync_ReturnsNullTest_WhileThrowingException()
    {
        string userId = "User Id";
        int month = 1;

        _mockedUnitOfWork.Setup(x => x.SavingsRepository.GetSavingsAnalyticsAsync(userId, month))
            .ThrowsAsync(new Exception("Exception"));

        var result = await _savingsAnalyticsService.GetSavingsAnalyticsAsync(userId, month);
        Assert.Null(result);
        _mockedLogger.Verify(x=>x.LogError(It.IsAny<string>(),It.IsAny<object>()),Times.Once);

    }
}