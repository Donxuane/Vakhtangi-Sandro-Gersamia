using BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController] 
    [Route("api/[controller]")]
    public class LimitsController : ControllerBase
    {
        private readonly ILimitsManageService _limitsManageService;

        public LimitsController(ILimitsManageService limitsManageService)
        {
            _limitsManageService = limitsManageService;
        }
        [HttpPost("AddLimit")]
        public async Task<IActionResult> AddLimit(LimitsDto limitDto)
        {
            var limits = new Limits()
            {
                CategoryId = limitDto.CategoryDtoId,
                Amount = limitDto.Amount,
                PeriodCategory = limitDto.PeriodCategory,
                DateAdded = limitDto.DateAdded,
                UserId = HttpContext.Items["UserId"].ToString()
            };
            var result = await _limitsManageService.SetLimits(limits);
            if (result == true)
            {
                return Ok("Limit added successfully");
            }
            return BadRequest();
        }

        [HttpDelete("deleteLimit")]
        public async Task<IActionResult> deleteLimit(int limitId)
        {
            var result = await _limitsManageService.DeleteLimits(limitId);
            if (result == true)
            {
                return Ok("Limit deleted succesfully");
            }

            return BadRequest("something went wrong");
        }

        [HttpPut("UpdateLimits")]
        public async Task<IActionResult> UpdateLimit(UpdateLimitDto updateLimitDto)
        {
            var updateLimits = new Limits()
            {
                Id = updateLimitDto.Id,
                Amount = updateLimitDto.Amount,
                CategoryId = updateLimitDto.CategoryId,
                PeriodCategory = updateLimitDto.PeriodCategory,
             
                UserId = HttpContext.Items["UserId"].ToString()
                
            };
            var result = await _limitsManageService.UpdateLimits(updateLimits);
            if (result == true)
            {
                return Ok("updated");
            }

            return BadRequest();
        }
    }
}
