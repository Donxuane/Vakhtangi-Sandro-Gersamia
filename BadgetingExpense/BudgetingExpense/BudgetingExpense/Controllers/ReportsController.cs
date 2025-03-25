﻿using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
using BudgetingExpenses.Service.MapService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("Api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IIncomeReportsService _service;
    private readonly IForecastService<IncomeRecord> _forecastService;
    private readonly IExpenseReportsService _expenseRecordsService;
    private readonly IForecastService<ExpenseRecord> _expenseForecastService;
    private readonly ISavingsAnalyticService _analyticsService;

    public ReportsController(IIncomeReportsService service, IExpenseReportsService expenseReportsService,
        IForecastService<IncomeRecord> forecastService, IForecastService<ExpenseRecord> expenseForcastService
        , ISavingsAnalyticService analyticsService)
    {
        _service = service;
        _forecastService = forecastService;
        _expenseForecastService = expenseForcastService;
        _expenseRecordsService = expenseReportsService;
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Income Reports And Analitycs
    /// </summary>
    [HttpGet("IncomeRecordsBasedCurrency")]
    public async Task<IActionResult> IncomeRecordsBasedCurrencyAsync([FromQuery] GetRecordsCurrencyDto model)
    {
        var result = await _service.RecordsBasedCurrencyPeriodAsync(
            model.Map(HttpContext.Items["UserId"].ToString()));
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => x.Map());
            return Ok(finalRecords.ToList());
        }
        return BadRequest("Records Not Found");
    }

    [HttpGet("IncomeRecordsBasedCategory")]
    public async Task<IActionResult> IncomeRecordsBasedCategoryAsync([FromQuery]GetRecordsCategoryDto model)
    {
        var result = await _service.RecordsBasedCategoryPeriodAsync(model.Map(HttpContext.Items["UserId"].ToString()));
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => x.Map());
            return Ok(finalRecords);
        }
        return BadRequest("Records Not Found");
    }

    [HttpGet("AllIncomeRecords")]
    public async Task<IActionResult> IncomeRecordsAsync()
    {
        var result = await _service.GetAllRecordsAsync(HttpContext.Items["UserId"].ToString());
        if (result != null && result.Any())
        {
            var records = result.Select(x => x.Map());
            return Ok(records);
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("IncomeForecast")]
    public async Task<IActionResult> IncomeForecastAsync()
    {
        var result = await _forecastService.GetForecastCategoriesAsync(HttpContext.Items["UserId"].ToString());
        if (result != null)
        {
            return Ok(new { result });
        }
        return BadRequest();
    }

    /// <summary>
    /// Expense Reports And Analitycs
    /// </summary>
    [HttpGet("TopExpenses")]
    public async Task<IActionResult> GetMostExpenseRecordsAsync(int period)
    {
        var result = await _expenseRecordsService.BiggestExpensesBasedPeriodAsync(
            new RecordsPeriod { Period = period, UserId = HttpContext.Items["UserId"].ToString() });
        if (result != null && result.Any())
        {
            var finalResult = result.Select(x => x.Map());
            return Ok(finalResult.ToList());
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCategoryPeriod")]
    public async Task<IActionResult> GetExpensesBasedCategoryPeriodAsync([FromQuery] GetRecordsCategoryDto model)
    {
        var records = await _expenseRecordsService.RecordsBasedCategoryPeriodAsync(
            model.Map(HttpContext.Items["UserId"].ToString()));
        if (records != null && records.Any())
        {
            var finalRecord = records.Select(x => x.Map());
            return Ok(finalRecord);
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCurrencyPeriod")]
    public async Task<IActionResult> GetExpensesBasedCurrencyPeriodAsync([FromQuery] GetRecordsCurrencyDto model)
    {
        var records = await _expenseRecordsService.RecordsBasedCurrencyPeriodAsync(
            model.Map(HttpContext.Items["UserId"].ToString()));
        if (records != null && records.Any())
        {
            var final = records.Select(x => x.Map());
            return Ok(final);
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("AllExpenseRecords")]
    public async Task<IActionResult> GetAllExpenseRecordsAsync(int page)
    {
        var records = await _expenseRecordsService.GetAllRecordsAsync(HttpContext.Items["UserId"].ToString(), page);
        if (records != null && records.Value.records.Any())
        {
            var final = records.Value.records.Select(x => x.Map());
            return Ok(new { PageAmount = records.Value.pageAmount, final });
        }
        return BadRequest("Records Not Found");
    }

    [HttpGet("expenseForecast")]
    public async Task<IActionResult> ExpenseForecastAsync()
    {
        var result = await _expenseForecastService.GetForecastCategoriesAsync(HttpContext.Items["UserId"].ToString());
        if (result.Any())
        {
            return Ok(result);
        }
        return BadRequest("Records Not Found");
    }

    /// <summary>
    /// Savings 
    /// </summary>
    [HttpGet("savingAnalyticByPeriod")]
    public async Task<IActionResult> SavingAnalyticByPeriodAsync(int month)
    {
        var value = await _analyticsService.GetSavingsAnalyticsAsync(HttpContext.Items["UserId"].ToString(), month);
        if (value != null)
        {
            return Ok(value);
        }
        return BadRequest("Records Not Found");
    }
}

