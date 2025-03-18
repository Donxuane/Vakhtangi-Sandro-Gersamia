using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

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
    public async Task<IActionResult> AddLimitAsync([FromForm]LimitsDto limitDto)
    {
        var limits = new Limits()
        {
            CategoryId = limitDto.CategoryId,
            Amount = limitDto.Amount,
            PeriodCategory = limitDto.Period,
            DateAdded = limitDto.DateAdded,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _limitsManageService.SetLimitsAsync(limits);
        if (result == true)
        {
            return Ok("Limit added successfully");
        }
        return BadRequest();
    }

    [HttpDelete("DeleteLimit")]
    public async Task<IActionResult> DeleteLimitAsync(int limitId)
    {
        var result = await _limitsManageService.DeleteLimitsAsync(limitId);
        if (result == true)
        {
            return Ok("Limit deleted succesfully");
        }

        return BadRequest("something went wrong");
    }

    [HttpPut("UpdateLimitsAsync")]
    public async Task<IActionResult> UpdateLimitAsync([FromForm]UpdateLimitDto updateLimitDto)
    {
        var updateLimits = new Limits()
        {
            Id = updateLimitDto.Id,
            Amount = updateLimitDto.Amount,
            CategoryId = updateLimitDto.CategoryId,
            PeriodCategory = updateLimitDto.PeriodCategory,
            DateAdded = updateLimitDto.StartupDate,
            UserId = HttpContext.Items["UserId"].ToString()
            
        };
        var result = await _limitsManageService.UpdateLimitsAsync(updateLimits);
        if (result == true)
        {
            return Ok("updated");
        }

        return BadRequest();
    }
}
