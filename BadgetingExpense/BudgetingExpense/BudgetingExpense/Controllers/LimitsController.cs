﻿using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
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
    private readonly IBudgetPlanningService _budgetPlanningService;

    public LimitsController(ILimitsManageService limitsManageService,
        IBudgetPlanningService budgetPlanningService)
    {
        _limitsManageService = limitsManageService;
        _budgetPlanningService = budgetPlanningService;
    }
    [HttpPost("AddLimit")]
    public async Task<IActionResult> AddLimit([FromForm]LimitsDto limitDto)
    {
        var limits = new Limits()
        {
            CategoryId = limitDto.CategoryId,
            Amount = limitDto.Amount,
            PeriodCategory = limitDto.Period,
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

    [HttpDelete("DeleteLimit")]
    public async Task<IActionResult> DeleteLimit(int limitId)
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
            DateAdded = DateTime.UtcNow,
            UserId = HttpContext.Items["UserId"].ToString()
            
        };
        var result = await _limitsManageService.UpdateLimits(updateLimits);
        if (result == true)
        {
            return Ok("updated");
        }

        return BadRequest();
    }

    [HttpPost("SendEmail")]
    public async Task<IActionResult> SendEmail(int CategoryId)
    {
        await _budgetPlanningService.AllExpenses(HttpContext.Items["UserId"].ToString(), CategoryId);
        return Ok("email was sent");
    }
}
