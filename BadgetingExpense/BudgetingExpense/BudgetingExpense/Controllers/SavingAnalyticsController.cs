using BudgetingExpense.Domain.Contracts.IServices.IReposrts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers
{
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
        public async Task<IActionResult> savingAnalyticByPeriod(int month)
        {
           var (expense, income) = await _analyticsService.FinanceRecords(HttpContext.Items["UserId"].ToString(), month);
           if (income >= 0 )
           {
               var savings = await _analyticsService.SavingsAnalyticByPeriodAsync(expense, income);
               var percentage = await _analyticsService.SavingPercentageAsync(expense, income);
               return Ok($"Income - {income}\n" +
                   $"Expense - {expense}\nYou Saved : {savings} \n percentage : {percentage}%");
           }

           return BadRequest();
        }
    }
}
