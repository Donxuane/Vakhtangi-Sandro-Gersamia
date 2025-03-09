using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;
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
    private readonly IExpenseReportsService _service;
    public ExpenseReportsController(IExpenseReportsService service)
    {
        _service = service;
    }

    [HttpGet("TopExpenses")]
    public async Task<IActionResult> GetMostExpenseRecords(int period)
    {
        var model = new GetRecordsPeriod
        {
            Period = period,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _service.BiggestExpensesBasedPeriod(model);
        if (result != null && result.Any())
        {
            var finalResult = result.Select(x => new RecordsDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName,
                Currency = x.Currency,
                Date =x.Date
            });
            return Ok(finalResult.ToList());
        }
        return BadRequest("Records Not Found");
    }
}
