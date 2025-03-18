﻿using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
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
        var record = new GetRecordCurrency
        {
            UserId = HttpContext.Items["UserId"].ToString(),
            Currency = model.Currency,
            Period = model.Period
        };
        var result = await _service.RecordsBasedCurrencyPeriodAsync(record);
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.IncomeDate
            });
            return Ok(finalRecords.ToList());
        }
        return BadRequest("Records Not Found");
    }

    [HttpPost("IncomeRecordsBasedCategory")]
    public async Task<IActionResult> IncomeRecordsBasedCategoryAsync([FromForm] GetRecordsCategoryDto model)
    {
        var record = new GetRecordCategory
        {
            Category = model.CategoryName,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _service.RecordsBasedCategoryPeriodAsync(record);
        if (result != null && result.Any())
        {
            var finalRecords = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.IncomeDate
            });
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
            var records = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.IncomeDate
            });
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
        var model = new GetRecordsPeriod
        {
            Period = period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _expenseRecordsService.BiggestExpensesBasedPeriodAsync(model);
        if (result != null && result.Any())
        {
            var finalResult = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.Date
            });
            return Ok(finalResult.ToList());
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCategoryPeriod")]
    public async Task<IActionResult> GetExpensesBasedCategoryPeriodAsync([FromQuery] GetRecordsCategoryDto model)
    {
        var categoryModel = new GetRecordCategory
        {
            Category = model.CategoryName,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var records = await _expenseRecordsService.RecordsBasedCategoryPeriodAsync(categoryModel);
        if (records != null && records.Any())
        {
            var finalRecord = records.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.Date
            });
            return Ok(finalRecord);
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCurrencyPeriod")]
    public async Task<IActionResult> GetExpensesBasedCurrencyPeriodAsync([FromQuery] GetRecordsCurrencyDto model)
    {
        var currencyModel = new GetRecordCurrency
        {
            Currency = model.Currency,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var records = await _expenseRecordsService.RecordsBasedCurrencyPeriodAsync(currencyModel);
        if (records != null && records.Any())
        {
            var final = records.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.Date
            });
            return Ok(final);
        }
        return BadRequest("No Records Found");
    }
    [HttpGet("AllExpenseRecords")]
    public async Task<IActionResult> GetAllExpenseRecordsAsync()
    {
        var records = await _expenseRecordsService.GetAllRecordsAsync(HttpContext.Items["UserId"].ToString());
        if (records != null && records.Any())
        {
            var final = records.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.Date
            });
            return Ok(final);
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

        return BadRequest();
    }

    /// <summary>
    /// Savings 
    /// </summary>
    [HttpGet("savingAnalyticByPeriod")]
    public async Task<IActionResult> SavingAnalyticByPeriodAsync(int month)
    {
        var value = await _analyticsService.SavingsAnalyticsAsync(HttpContext.Items["UserId"].ToString(), month);
        if (value != null && value.Income > 0)
        {
            return Ok(value);
        }
        return BadRequest();
    }
}

