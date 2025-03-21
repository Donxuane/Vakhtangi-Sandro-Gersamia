﻿using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class ForecastsServiceTest
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<IncomeForecastService>> _mockedIncomeForecastLogger;
    private readonly Mock<ILogger<ExpenseForecastService>> _mockedExpenseForcastLogger;
    private readonly IForecastService<IncomeRecord> _incomeForecastService;
    private readonly IForecastService<ExpenseRecord> _expenseForcastService;

    public ForecastsServiceTest()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedIncomeForecastLogger = new Mock<ILogger<IncomeForecastService>>();
        _mockedExpenseForcastLogger = new Mock<ILogger<ExpenseForecastService>>();
        _incomeForecastService = new IncomeForecastService(_mockedUnitOfWork.Object,_mockedIncomeForecastLogger.Object);
        _expenseForcastService = new ExpenseForecastService(_mockedUnitOfWork.Object, _mockedExpenseForcastLogger.Object);
    }

    [Fact]
    public async Task GetForecastCategoriesAsync_ShouldReturnIncomeForecast_BasedCategoriesAndCurrencies()
    {
        string userId = "User Id";
        var repositoryModelCollection = new List<IncomeRecord> {
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-1),Currency = Currencies.GEL,UserId = userId},
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-2),Currency = Currencies.GEL,UserId = userId},
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-3),Currency = Currencies.GEL,UserId = userId}
        };
        var expectedResult = new List<ForecastCategory> { 
            new() {CategoryName = "salary",Currency = Currencies.GEL.ToString(),Expected = 500} 
        };

        _mockedUnitOfWork.Setup(x => x.IncomeRecords.GetUserIncomeRecordsAsync(userId))
            .ReturnsAsync(repositoryModelCollection);

        var result = await _incomeForecastService.GetForecastCategoriesAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.Count,result.Count());
        Assert.All(expectedResult, expected =>
        {
            Assert.Contains(result, actual => actual.CategoryName == expected.CategoryName &&
            actual.Currency == expected.Currency && actual.Expected == expected.Expected);
        });
        _mockedUnitOfWork.Verify(x=>x.IncomeRecords.GetUserIncomeRecordsAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetForecastCategoriesAsync_ShouldReturnNull_WhileException()
    {
        string userId = "User Id";
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(userId))
            .ThrowsAsync(new Exception());

        var result = await _expenseForcastService.GetForecastCategoriesAsync(userId);

        Assert.Null(result);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.GetUserExpenseRecordsAsync(userId), Times.Once);
        _mockedExpenseForcastLogger.Verify(x=>x.Log(LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
