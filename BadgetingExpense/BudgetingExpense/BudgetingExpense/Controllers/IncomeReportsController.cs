using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[Authorize(Roles ="User")]
[ApiController]
[Route("Reports/[controller]")]
public class IncomeReportsController : ControllerBase
{
    private readonly IIncomeReportsService _service;
    private readonly IIncomeForecastService _forecastService;

    public IncomeReportsController(IIncomeReportsService service,IIncomeForecastService forecastService)
    {
        _service = service;
        _forecastService = forecastService;
    }

    [HttpGet("IncomeRecordsBasedCurrency")]
    public async Task<IActionResult> IncomeRecordsBasedCurrency([FromQuery]GetRecordsCurrencyDto model)
    {
        var record = new GetRecordCurrency
        {
            UserId = HttpContext.Items["UserId"].ToString(),
            Currency = model.Currency,
            Period = model.Period
        };
        var result = await _service.RecordsBasedCurrencyPeriod(record);
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
    public async Task<IActionResult> IncomeRecordsBasedCategory([FromForm] GetRecordsCategoryDto model)
    {
        var record = new GetRecordCategory
        {
            Category = model.CategoryName,
            Period = model.Period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _service.RecordsBasedCategoryPeriod(record);
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
    public async Task<IActionResult> AllIncomeRecords()
    {
        var result = await _service.GetAllRecords(HttpContext.Items["UserId"].ToString());
        if(result != null && result.Any())
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
    [HttpGet("income forecast")]
    public async Task<IActionResult> IncomeForecast()
    {
        var result = await _forecastService.GetForecastCategoriesAsync(HttpContext.Items["UserId"].ToString());
        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }
}
