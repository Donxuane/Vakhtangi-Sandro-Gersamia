using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class ForecastsServiceTest
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<IncomeForecastService>> _mockedIncomeForecastLogger;
    private readonly Mock<ILogger<ExpenseForecastService>> _mockedExpenseForecastLogger;
    private readonly IncomeForecastService _incomeForecastService;
    private readonly ExpenseForecastService _expenseForecastService;
    private readonly Mock<IConfiguration> _configuration;

    public ForecastsServiceTest()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedIncomeForecastLogger = new Mock<ILogger<IncomeForecastService>>();
        _mockedExpenseForecastLogger = new Mock<ILogger<ExpenseForecastService>>();
        _configuration = new Mock<IConfiguration>();
        _incomeForecastService = new IncomeForecastService(_mockedUnitOfWork.Object,_mockedIncomeForecastLogger.Object,_configuration.Object);
        _expenseForecastService = new ExpenseForecastService(_mockedUnitOfWork.Object, _mockedExpenseForecastLogger.Object,_configuration.Object);
    }

    [Fact]
    public async Task GetForecastCategoriesAsync_ShouldReturnIncomeForecast_BasedCategoriesAndCurrencies()
    {
        string userId = "User Id";
        _configuration.Setup(x => x.GetSection("ConfigureForcastCounts")["IncomeForecastCount"])
            .Returns("2");
        var repositoryModelCollection = new List<IncomeRecord> {
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-1),Currency = Currencies.GEL,UserId = userId},
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-2),Currency = Currencies.GEL,UserId = userId},
            new() { Amount = 500,CategoryName = "salary",IncomeDate = DateTime.UtcNow.AddMonths(-3),Currency = Currencies.GEL,UserId = userId}
        };
        var expectedResult = new List<ForecastCategory> { 
            new() {CategoryName = "salary",Currency = Currencies.GEL.ToString(),Expected = 500} 
        };

        _mockedUnitOfWork.Setup(x => x.IncomeRecords.IncomeRecordsAsync(userId))
            .ReturnsAsync(repositoryModelCollection);

        var result = await _incomeForecastService.GetForecastCategoriesAsync(userId);

        
        Assert.Equal(expectedResult.Count,result.Count());
        Assert.All(expectedResult, expected =>
        {
            Assert.Contains(result, actual => actual.CategoryName == expected.CategoryName &&
            actual.Currency == expected.Currency && actual.Expected == expected.Expected);
        });
        _mockedUnitOfWork.Verify(x=>x.IncomeRecords.IncomeRecordsAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetForecastCategoriesAsync_Throws_WhileException()
    {
        string userId = "User Id";
        _mockedUnitOfWork.Setup(x => x.ExpenseRecords.ExpenseRecordsAsync(userId))
            .ThrowsAsync(new Exception("Database exception"));
        _configuration.Setup(x => x.GetSection("ConfigureForcastCounts")["ExpenseForecastCount"])
            .Returns("2");
        var result = await Assert.ThrowsAsync<Exception>(() => _expenseForecastService.GetForecastCategoriesAsync(userId));
        Assert.Equal("Database exception", result.Message);
        _mockedUnitOfWork.Verify(x => x.ExpenseRecords.ExpenseRecordsAsync(userId), Times.Once);
        _mockedExpenseForecastLogger.Verify(x => x.Log(LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once
        );
    }
    
}
