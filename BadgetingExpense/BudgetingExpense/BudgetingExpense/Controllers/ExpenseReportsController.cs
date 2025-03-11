using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("Reports/[controller]")]
public class ExpenseReportsController : ControllerBase
{
    private readonly IExpenseReportsService _expenseRecordsService;
    private readonly IForecastService<ExpenseRecord> _expenseForecastService;
    public ExpenseReportsController(IExpenseReportsService service, IForecastService<ExpenseRecord> expenseForecastService)
    {
        _expenseRecordsService = service;
        _expenseForecastService = expenseForecastService;
    }

    [HttpGet("TopExpenses")]
    public async Task<IActionResult> GetMostExpenseRecords(int period)
    {
        var model = new GetRecordsPeriod
        {
            Period = period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _expenseRecordsService.BiggestExpensesBasedPeriod(model);
        if (result != null && result.Any())
        {
            var finalResult = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date =x.Date
            });
            return Ok(finalResult.ToList());
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCategoryPeriod")]
    public async Task<IActionResult> GetExpensesBasedCategoryPeriod([FromQuery]GetRecordsCategoryDto model)
    {
        var categoryModel = new GetRecordCategory
        {
            Category = model.CategoryName,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var records = await _expenseRecordsService.RecordsBasedCategoryPeriod(categoryModel);
        if (records != null && records.Any())
        {
            var finalRecord = records.Select(x => new RecordsDto
            {
                Amount= x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency.ToString(),
                Date = x.Date
            });
            return Ok(finalRecord);
        }
        return BadRequest("Records Not Found");
    }
    [HttpGet("ExpensesBasedCurrencyPeriod")]
    public async Task<IActionResult> GetExpensesBasedCurrencyPeriod([FromQuery]GetRecordsCurrencyDto model)
    {
        var currencyModel = new GetRecordCurrency
        {
            Currency = model.Currency,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var records = await _expenseRecordsService.RecordsBasedCurrencyPeriod(currencyModel);
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
    public async Task<IActionResult> GetAllExpenseRecords()
    {
        var records = await _expenseRecordsService.GetAllRecords(HttpContext.Items["UserId"].ToString());
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
    public async Task<IActionResult> ExpenseForecast()
    {
        var result = await _expenseForecastService.GetForecastCategoriesAsync(HttpContext.Items["UserId"].ToString());
        if (result.Any())
        {
            return Ok(result);
        }

        return BadRequest();
    }
}
