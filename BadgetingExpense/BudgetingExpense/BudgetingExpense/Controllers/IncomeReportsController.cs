using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsService;
using BudgetingExpense.Domain.Models.GetModel;
using BudgetingExpenses.Service.DtoModels.ReportsDtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace BudgetingExpense.api.Controllers;

[Authorize(Roles ="User")]
[ApiController]
[Route("IncomeReports/[controller]")]
public class IncomeReportsController : ControllerBase
{
    private readonly IIncomeReportsService _service;

    public IncomeReportsController(IIncomeReportsService service)
    {
        _service = service;
    }

    [HttpPost("GetIncomeRecords")]
    public async Task<IActionResult> IncomeRecords([FromForm]GetIncomeRecordsDto model)
    {
        var record = new GetIncomeRecord
        {
            UserId = HttpContext.Items["UserId"].ToString(),
            Currency = model.Currency,
            Period = model.Period
        };
        var result = await _service.RecordsBasedCurrecncyPeriod(record);
        if (result != null)
        {
            return Ok(result.ToList());
        }
        return BadRequest("No Records Found");
    }
}
