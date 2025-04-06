using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
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
    private readonly IncomeReportsService _incomeReportsService;

    public IncomeReportsTests()
    {
        _mockedUnitOfWork = new Mock<IUnitOfWork>();
        _mockedLogger = new Mock<ILogger<IncomeReportsService>>();
        _incomeReportsService = new IncomeReportsService(_mockedUnitOfWork.Object, _mockedLogger.Object);
    }

    [Fact]
    public async Task GetAllRecordsAsync_ShouldReturnAllIncomeRecords_OrderByDescendingDate()
    {
        string userId = "user123";
        int page = 1;
        var incomeReports = new List<IncomeRecord>
        {
            new()
            {
                Amount = 100, CategoryName = "salary", Currency = Currencies.GEL, IncomeDate = DateTime.UtcNow,
                UserId = userId
            },
            new()
            {
                Amount = 100, CategoryName = "business", Currency = Currencies.GEL, IncomeDate = DateTime.UtcNow,
                UserId = userId
            }
        };
        var expectedRecords = (incomeReports, page);
        _mockedUnitOfWork.Setup(x => x.IncomeRecords.AllIncomeRecordsAsync(userId, page)).ReturnsAsync(expectedRecords);
        var result = await _incomeReportsService.GetAllRecordsAsync(userId, page);
        Assert.NotNull(result);
        Assert.Equal(expectedRecords.incomeReports, result?.records); 
        Assert.Equal(expectedRecords.page, result?.pageAmount);
    }

    [Fact]
    public async Task RecordsBasedCategoryPeriodAsync_ShouldReturnNull_NoRecordsFound()
    {
        var model = new RecordCategory { UserId = "User Id" };

        _mockedUnitOfWork.Setup(x => x.IncomeRecords.IncomeRecordsAsync(model.UserId))
            .ReturnsAsync([]);
        var result = await _incomeReportsService.RecordsBasedCategoryPeriodAsync(model);
        Assert.Null(result);
        _mockedUnitOfWork.Verify(x => x.IncomeRecords.IncomeRecordsAsync(model.UserId), Times.Once);
    }

    [Fact]
    public async Task RecordsBasedCurrencyPeriodAsync_ShouldThrow_WhileException()
    {
        _mockedUnitOfWork.Setup(x => x.IncomeRecords.IncomeRecordsAsync(It.IsAny<string>()))
            .ThrowsAsync(new NullReferenceException());

        var result = await Assert.ThrowsAsync<NullReferenceException>(()=>_incomeReportsService.RecordsBasedCurrencyPeriodAsync(It.IsAny<RecordCurrency>()));
        _mockedUnitOfWork.Verify(x => x.IncomeRecords.IncomeRecordsAsync(It.IsAny<string>()), Times.Never);
        _mockedLogger.Verify(x => x.Log(LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once
        );
    }
}
