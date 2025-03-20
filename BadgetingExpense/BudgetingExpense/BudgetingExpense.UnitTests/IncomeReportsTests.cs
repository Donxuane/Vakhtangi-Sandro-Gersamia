using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class IncomeReportsTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<IncomeReportsService>> _mockedLogger;
    private readonly IIncomeReportsService _incomeReportsService;

    public IncomeReportsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<IncomeReportsService>>();
        _incomeReportsService = new IncomeReportsService(_mockedUnitOfWork.Object, _mockedLogger.Object);
    }

    [Fact]
    public async Task GetAllRecordsAsync_ShouldReturnAllIncomeRecords_OrderByDescendingDate()
    {
        string userId = "User Id";
        var returnResult = new List<IncomeRecord> {
            new() { UserId = userId, IncomeDate = DateTime.UtcNow.AddMonths(-1)},
            new() { UserId = userId, IncomeDate = DateTime.UtcNow.AddMonths(-2)},
            new() { UserId = userId, IncomeDate = DateTime.UtcNow.AddMonths(-3)},
            new() { UserId = userId, IncomeDate = DateTime.UtcNow.AddMonths(-4)}
        };
        _mockedUnitOfWork.Setup(x => x.IncomeRecords.GetUserIncomeRecordsAsync(userId))
            .ReturnsAsync(returnResult);

        var result = await _incomeReportsService.GetAllRecordsAsync(userId);
        Assert.NotNull(result);
        Assert.Equal(returnResult.OrderByDescending(x => x.IncomeDate), result);

        _mockedUnitOfWork.Verify(x => x.IncomeRecords.GetUserIncomeRecordsAsync(userId), Times.Once);
    }

    [Fact]
    public async Task RecordsBasedCategoryPeriodAsync_ShouldReturnNull_NoRecordsFound()
    {
        var model = new RecordCategory { UserId = "User Id" };

        _mockedUnitOfWork.Setup(x => x.IncomeRecords.GetUserIncomeRecordsAsync(model.UserId))
            .ReturnsAsync([]);
        var result = await _incomeReportsService.RecordsBasedCategoryPeriodAsync(model);
        Assert.Null(result);
        _mockedUnitOfWork.Verify(x => x.IncomeRecords.GetUserIncomeRecordsAsync(model.UserId), Times.Once);
    }

    [Fact]
    public async Task RecordsBasedCurrencyPeriodAsync_ShouldReturnNull_WhileException()
    {
        var model = new RecordCurrency { UserId = "User Id" };
        _mockedUnitOfWork.Setup(x => x.IncomeRecords.GetUserIncomeRecordsAsync(model.UserId))
            .ThrowsAsync(new Exception());

        var result = await _incomeReportsService.RecordsBasedCurrencyPeriodAsync(model);
        Assert.Null(result);
        _mockedLogger.Verify(x=>x.Log(LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),Times.Once);
    }
}
