using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class SavingsAnalyticsTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<SavingsAnalyticService>> _mockedLogger;
    private readonly Mock<ICurrencyRateService> _mockedCurrencyRateService;
    private readonly ISavingsAnalyticService _savingsAnalyticsService;

    public SavingsAnalyticsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<SavingsAnalyticService>>();
        _mockedCurrencyRateService = new Mock<ICurrencyRateService>();
        _savingsAnalyticsService = new SavingsAnalyticService(_mockedUnitOfWork.Object,_mockedLogger.Object, _mockedCurrencyRateService.Object);
    }

    [Fact]
    public async Task GetSavingsAnalytics_ReturnsDataTest()
    {
        string userId = "userId";
        int month = 1;
        _mockedUnitOfWork.Setup(x=>x.SavingsRepository.GetSavingsAnalyticsAsync(userId,month))
            .ReturnsAsync([]);
        var result = await _savingsAnalyticsService.GetSavingsAnalyticsAsync(userId, month);

        Assert.NotNull(result);
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

        var result = await Assert.ThrowsAsync<Exception>(() => _savingsAnalyticsService.GetSavingsAnalyticsAsync(userId, month));
        Assert.Equal("Exception", result.Message);
        _mockedUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
        _mockedLogger.Verify(
        x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }
}