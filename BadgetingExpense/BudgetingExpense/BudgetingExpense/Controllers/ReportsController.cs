using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
using BudgetingExpenses.Service.MapService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("Api/[controller]")]
public class ReportsController : BaseControllerExstention
{
    private readonly IIncomeReportsService _service;
    private readonly IForecastService<IncomeRecord> _forecastService;
    private readonly IExpenseReportsService _expenseRecordsService;
    private readonly IForecastService<ExpenseRecord> _expenseForecastService;
    private readonly ISavingsAnalyticService _analyticsService;

    public ReportsController(IIncomeReportsService service, IExpenseReportsService expenseReportsService,
        IForecastService<IncomeRecord> forecastService, IForecastService<ExpenseRecord> expenseForecastService
        , ISavingsAnalyticService analyticsService)
    {
        _service = service;
        _forecastService = forecastService;
        _expenseForecastService = expenseForecastService;
        _expenseRecordsService = expenseReportsService;
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Income Reports And Analitycs
    /// </summary>
    [HttpGet("IncomeRecordsBasedCurrency")]
    public async Task<IActionResult> IncomeRecordsBasedCurrencyAsync([FromQuery] GetRecordsCurrencyDto model)
    {
        var result = await _service.RecordsBasedCurrencyPeriodAsync(model.Map(UserId));
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => x.Map());
            return Ok(finalRecords);
        }
        return BadRequest(new { message = "Records not found" });
    }

    [HttpGet("IncomeRecordsBasedCategory")]
    public async Task<IActionResult> IncomeRecordsBasedCategoryAsync([FromQuery]GetRecordsCategoryDto model)
    {
        var result = await _service.RecordsBasedCategoryPeriodAsync(model.Map(UserId));
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => x.Map());
            return Ok(finalRecords);
        }
        return BadRequest(new { message = "Records not found" });
    }

    [HttpGet("IncomeRecords")]
    public async Task<IActionResult> IncomeRecordsAsync(int page)
    {
        var result = await _service.GetAllRecordsAsync(UserId, page);
        if (result != null && result.Value.records.Any())
        {
            var records = result.Value.records.Select(x => x.Map());
            return Ok(new { result.Value.pageAmount, currentPage = page, records });
        }
        return BadRequest(new { message = "Records not found" });
    }
    [HttpGet("IncomeForecast")]
    public async Task<IActionResult> IncomeForecastAsync()
    {
        var result = await _forecastService.GetForecastCategoriesAsync(UserId);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest(new { message = "Unfortunatelly theres nothing to predict" });
    }

    /// <summary>
    /// Expense Reports And Analitycs
    /// </summary>
    [HttpGet("TopExpenses")]
    public async Task<IActionResult> GetMostExpenseRecordsAsync([FromQuery]TopExpenseDto model)
    {
        var result = await _expenseRecordsService.BiggestExpensesBasedPeriodAsync(model.Map(UserId));
        if (result != null && result.Any())
        {
            var finalResult = result.Select(x => x.Map());
            return Ok(finalResult);
        }
        return BadRequest(new { message = "Records not found" });
    }
    [HttpGet("ExpensesBasedCategoryPeriod")]
    public async Task<IActionResult> GetExpensesBasedCategoryPeriodAsync([FromQuery] GetRecordsCategoryDto model)
    {
        var records = await _expenseRecordsService.RecordsBasedCategoryPeriodAsync(
            model.Map(UserId));
        if (records != null && records.Any())
        {
            var finalRecord = records.Select(x => x.Map());
            return Ok(finalRecord);
        }
        return BadRequest(new { message = "Records not found" });
    }
    [HttpGet("ExpensesBasedCurrencyPeriod")]
    public async Task<IActionResult> GetExpensesBasedCurrencyPeriodAsync([FromQuery] GetRecordsCurrencyDto model)
    {
        var records = await _expenseRecordsService.RecordsBasedCurrencyPeriodAsync(
            model.Map(UserId));
        if (records != null && records.Any())
        {
            var final = records.Select(x => x.Map());
            return Ok(final);
        }
        return BadRequest(new { message = "Records not found" });
    }
    [HttpGet("ExpenseRecords")]
    public async Task<IActionResult> GetAllExpenseRecordsAsync(int page)
    {
        var records = await _expenseRecordsService.GetAllRecordsAsync(UserId, page);
        if (records != null && records.Value.records.Any())
        {
            var final = records.Value.records.Select(x => x.Map());
            return Ok(new { PageAmount = records.Value.pageAmount, final });
        }
        return BadRequest(new { message = "Records not found" });
    }

    [HttpGet("ExpenseForecast")]
    public async Task<IActionResult> ExpenseForecastAsync()
    {
        var result = await _expenseForecastService.GetForecastCategoriesAsync(UserId);
        if (result.Any())
        {
            return Ok(result);
        }
        return BadRequest(new { message = "Records not found" });
    }

    /// <summary>
    /// Savings 
    /// </summary>
    [HttpGet("SavingAnalyticByPeriod")]
    public async Task<IActionResult> SavingAnalyticByPeriodAsync(int month)
    {
        var value = await _analyticsService.GetSavingsAnalyticsAsync(UserId, month);
        if (value.Item1 != null)
        {
            return Ok(new{value.Item1,value.Item2});
        }
        return BadRequest(new { message = "Records not found" });
    }
}

