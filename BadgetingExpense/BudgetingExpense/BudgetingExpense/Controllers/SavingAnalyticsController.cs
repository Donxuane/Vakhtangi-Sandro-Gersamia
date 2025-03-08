using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;
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
        public async Task<IActionResult> savingAnalyticByPeriod( int month)
        {
           var records = await _analyticsService.FinanceRecords(HttpContext.Items["UserId"].ToString(), month);

           if (records.income >= 0 )
           {
               var savings = await _analyticsService.SavingsAnalyticByPeriodAsync(records.expense, records.income);
               var percentage = await _analyticsService.SavingPercentageAsync(records.expense, records.income);

               return Ok($"Here is your saving : {savings} \n percentage : {Math.Round(percentage,2)} %");

           }

           return BadRequest();
        }
    }
}
