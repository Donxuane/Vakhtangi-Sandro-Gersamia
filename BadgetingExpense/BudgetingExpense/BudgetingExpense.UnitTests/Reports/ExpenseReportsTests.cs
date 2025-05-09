﻿ using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Reports;

public class ExpenseReportsTests
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<ExpenseReportsService>> _mockedLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ExpenseReportsService _expenseReportsService;

    public ExpenseReportsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<ExpenseReportsService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _expenseReportsService = new ExpenseReportsService(_mockedUnitOfWork.Object,
            _mockedLogger.Object,_mockConfiguration.Object);
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
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.ExpenseRecordsAsync(modelPass.UserId))
            .ReturnsAsync(modelReturn);
        _mockConfiguration.Setup(x => x.GetSection("TopExpenseAmounts")["Amount"]).Returns("10");

        var result = await _expenseReportsService.BiggestExpensesBasedPeriodAsync(modelPass);
        Assert.NotNull(result);
        Assert.Equal(modelReturn.OrderByDescending(x => x.Amount), result);
        Assert.Equal(5, result.Count());
        Assert.Equal(700, result.First().Amount);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.ExpenseRecordsAsync(modelPass.UserId), Times.Once);
        _mockedUnitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecordsBasedCategoryPeriodAsync_ShouldThrow_ExceptionThrown()
    {
        var model = new RecordCategory { Category = "Category", Period = 5, UserId = "User Id" };

        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.ExpenseRecordsAsync(model.UserId))
            .ThrowsAsync(new Exception());

        var result = await Assert.ThrowsAsync<Exception>(()=>_expenseReportsService.RecordsBasedCategoryPeriodAsync(model));

        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.ExpenseRecordsAsync(model.UserId), Times.Once);
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
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.ExpenseRecordsAsync(model.UserId))
            .ReturnsAsync(modelReturn);

        var result = await _expenseReportsService.RecordsBasedCurrencyPeriodAsync(model);

        Assert.NotNull(result);
        Assert.Equal(modelReturn.OrderByDescending(x => x.Date), result);
        foreach (var record in result)
        {
            Assert.InRange(record.Date, DateTime.UtcNow.AddMonths(-model.Period), DateTime.UtcNow);
        }
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.ExpenseRecordsAsync(model.UserId), Times.Once);
        _mockedUnitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllRecordsAsync_ShouldReturnAllRecords_BasedUserIdAndPage()
    {
        var userId = "userId";
        var page = 1;
        var expenseRecord = new List<ExpenseRecord>
        {
            new()
            {
                Amount = 100,CategoryName = "shopping",Currency = Currencies.GEL,Date = DateTime.UtcNow,UserId = userId
            },
            new()
            {
                Amount = 200,CategoryName = "betting",Currency = Currencies.GEL,Date = DateTime.UtcNow,UserId = userId
            }
            
        };
        var expectedRecords = (expenseRecord, page);
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.AllExpenseRecordsAsync(userId, page))
            .ReturnsAsync(expectedRecords);
        var result = await _expenseReportsService.GetAllRecordsAsync(userId, page);
        Assert.NotNull(result);
        Assert.Equal(expectedRecords.expenseRecord,result?.records);
        Assert.Equal(expectedRecords.page,result?.pageAmount);

    }
   
    
}

