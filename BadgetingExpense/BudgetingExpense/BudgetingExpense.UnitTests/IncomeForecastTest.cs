using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.Service.Reports;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests;

public class IncomeForecastTest
{
    private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
    private readonly Mock<ILogger<IncomeForecastService>> _mockedLogger;
    private readonly IForecastService<IncomeRecord> _forecastService;

    public IncomeForecastTest()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<IncomeForecastService>>();
        _forecastService = new IncomeForecastService(_mockedUnitOfWork.Object,_mockedLogger.Object);
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

    }
}
