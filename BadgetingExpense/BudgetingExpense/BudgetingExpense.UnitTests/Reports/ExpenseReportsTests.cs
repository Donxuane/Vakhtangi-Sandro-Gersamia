 using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Reports;

public class ExpenseReportsTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<ExpenseReportsService>> _mockedLogger;
    private readonly IExpenseReportsService _expenseReportsService;

    public ExpenseReportsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<ExpenseReportsService>>();
        _expenseReportsService = new ExpenseReportsService(_mockedUnitOfWork.Object,
            _mockedLogger.Object);
    }

    [Fact]
    public async Task BiggestExpensesBasedPeriodAsync_ReturnsDataWhileFetched()
    {
        var modelPass = new RecordsPeriod { Period = 5, UserId = "User Id" };
        var modelReturn = new List<ExpenseRecord> {
            new() { Amount = 100,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1) },
            new() { Amount = 500,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-2) },
            new() { Amount = 300,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1) },
            new() { Amount = 200,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-3) },
            new() { Amount = 700,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1) }
        };
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(modelPass.UserId))
            .ReturnsAsync(modelReturn);

        var result = await _expenseReportsService.BiggestExpensesBasedPeriodAsync(modelPass);
        Assert.NotNull(result);
        Assert.Equal(modelReturn.OrderByDescending(x => x.Amount), result);
        Assert.Equal(5, result.Count());
        Assert.Equal(700, result.First().Amount);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(modelPass.UserId), Times.Once);
        _mockedUnitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllRecordsAsync_ShouldReturnNull_RecordsNotFound()
    {
        string userId = "User Id";
        var list = new List<ExpenseRecord> { };
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(userId))
            .ReturnsAsync(list);
        var result = await _expenseReportsService.GetAllRecordsAsync(userId);

        Assert.Null(result);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(userId), Times.Once);
    }

    [Fact]
    public async Task RecordsBasedCategoryPeriodAsync_ShouldReturnNull_ExceptionThrown()
    {
        var model = new RecordCategory { Category = "Category", Period = 5, UserId = "User Id" };

        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId))
            .ThrowsAsync(new Exception());

        var result = await _expenseReportsService.RecordsBasedCategoryPeriodAsync(model);

        Assert.Null(result);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId), Times.Once);
        _mockedLogger.Verify(x => x.Log(
             LogLevel.Error,
             It.IsAny<EventId>(),
             It.Is<It.IsAnyType>((o, t) => true),
             It.IsAny<Exception>(),
             It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task RecordsBasedCurrencyPeriodAsync_ShouldReturnRecordsBasedPeriod_OrderByDescendingDate()
    {
        var model = new RecordCurrency { Currency = 0, Period = 5, UserId = "User Id" };
        var modelReturn = new List<ExpenseRecord> {
            new() { Amount = 100,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1), Currency = 0 },
            new() { Amount = 500,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-2), Currency = 0 },
            new() { Amount = 300,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1), Currency = 0 },
            new() { Amount = 200,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-3), Currency = 0 },
            new() { Amount = 700,UserId ="User Id", Date = DateTime.UtcNow.AddMonths(-1), Currency = 0 }
        };
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId))
            .ReturnsAsync(modelReturn);

        var result = await _expenseReportsService.RecordsBasedCurrencyPeriodAsync(model);

        Assert.NotNull(result);
        Assert.Equal(modelReturn.OrderByDescending(x => x.Date), result);
        foreach (var record in result)
        {
            Assert.InRange(record.Date, DateTime.UtcNow.AddMonths(-model.Period), DateTime.UtcNow);
        }
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(model.UserId), Times.Once);
        _mockedUnitOfWork.VerifyNoOtherCalls();
    }
}

