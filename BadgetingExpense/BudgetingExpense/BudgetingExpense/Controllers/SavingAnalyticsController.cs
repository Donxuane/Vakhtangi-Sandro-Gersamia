using BudgetingExpense.Domain.Contracts.IServices.IReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SavingAnalyticsController :ControllerBase

{
    private readonly ISavingsAnalyticService _analyticsService;

    public SavingAnalyticsController(ISavingsAnalyticService analyticsService)
    {
        _analyticsService = analyticsService;
    }
    [HttpGet("savingAnalyticByPeriod")]
    public async Task<IActionResult> SavingAnalyticByPeriod(int month)
    {
        var value = await _analyticsService.SavingsAnalyticsAsync(HttpContext.Items["UserId"].ToString(), month);
        if (value != null && value.Income > 0)
        {
            return Ok(value);
        }
       return BadRequest();
    }
}