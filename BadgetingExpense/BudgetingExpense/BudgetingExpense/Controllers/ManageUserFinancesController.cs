using BudgetingExpense.Api.Controllers;
using BudgetingExpense.Api.CustomFilters;
using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.ILimitations;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.MapService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("api/[controller]")]
public class ManageUserFinancesController : BaseControllerExstention
{
    private readonly IIncomeManageService _incomeService;
    private readonly IExpenseManageService _expenseManageService;
    private readonly ILimitsManageService _limitsManageService;
    public ManageUserFinancesController(IIncomeManageService incomeService, 
        IExpenseManageService expenseManageService, ILimitsManageService limitsManageService, CurrencyRateService currencyRateService)
    {
        _incomeService = incomeService;
        _expenseManageService = expenseManageService;
        _limitsManageService = limitsManageService;
    }

    /// <summary>
    /// Manage Income
    /// </summary>
    [ServiceFilter(typeof(PropertyNormalizationFilter))]
    [HttpPost("AddIncomeCategory")]
    public async Task<IActionResult> AddIncomeCategoryAsync(string category)
    {
        var result = await _incomeService.AddIncomeCategoryAsync(category);
        if (result > 0)
        {
            return Ok($"ID: {result}; {category} Category added Successfully");
        }
        return BadRequest("Couldn't Process Add");
    }

    [HttpGet("IncomeCategories")]
    public async Task<IActionResult> GetAllIncomeCategories()
    {
        var result = await _incomeService.GetAllIncomeCategoryRecordsAsync(UserId);
        if(result!=null && result.Any())
        {
            return Ok(result);
        }
        return BadRequest("Records not found!");
    }

    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPost("AddIncome")]
    public async Task<IActionResult> AddIncomeAsync([FromForm] IncomeDto dtoModel)
    {
        var result = await _incomeService.AddIncomeAsync(dtoModel.Map(UserId));
        if (result)
        {
            return Ok("Income Added Successfully");
        }
        return BadRequest("Couldn't Process Add");
    }

    [HttpDelete("Income")]
    public async Task<IActionResult> DeleteIncomeAsync(int incomeTypeId)
    {
        var result = await _incomeService.DeleteIncomeAsync(incomeTypeId, UserId);
        if (result)
        {
            return Ok("Income Source Deleted Successfully");
        }
        return BadRequest("Could not Process Delete");
    }
    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> UpdateIncomeAsync([FromForm] UpdateIncomeDto model)
    {

        var incomeUpdated = await _incomeService.UpdateIncomeAsync(
            model.Map(UserId));
        if (incomeUpdated)
        {
            return Ok("Successfully Updated");
        }
        return BadRequest("Couldn't Process Update");
    }
    /// <summary>
    /// Manage Expenses
    /// </summary>

    [ServiceFilter(typeof(PropertyNormalizationFilter))]
    [HttpPost("AddExpenseCategory")]
    public async Task<IActionResult> AddExpenseCategoryAsync(string category)
    {
        var result = await _expenseManageService.AddExpenseCategoryAsync(category);
        if (result > 0)
        {
            return Ok($"{result} Category Added Successfully");
        }
        return BadRequest("Couldn't Process Add");
    }

    [HttpGet("AllExpenseCategories")]
    public async Task<IActionResult> GetAllExpenseCategories()
    {
        var result = await _expenseManageService.GetAllExpenseCategoryRecordsAsync(UserId);
        if(result!=null && result.Any())
        {
            return Ok(result);
        }
        return BadRequest("Records not found!");
    }

    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPost("AddExpenses")]
    public async Task<IActionResult> AddExpensesAsync([FromForm] ExpenseDto expenseDto)
    {
        var result = await _expenseManageService.AddExpenseAsync(
            expenseDto.Map(UserId));
        if (result)
        {
            return Ok("Added Successfully");
        }
        return BadRequest("Couldn't Process Add");
    }

    [HttpDelete("Expenses")]
    public async Task<IActionResult> DeleteExpensesAsync(int expenseId)
    {
        var result = await _expenseManageService.DeleteExpenseAsync(expenseId, UserId);
        if (result)
        {
            return Ok("deleted successfully");
        }
        return BadRequest("Couldn't Process Delete");
    }
    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPut("UpdateExpenses")]
    public async Task<IActionResult> UpdateExpensesAsync([FromForm] UpdateExpenseDto model)
    {
        var result = await _expenseManageService.UpdateExpenseAsync(
            model.Map(UserId));
        if (result)
        {
            return Ok("Updated Successfully");
        }
        return BadRequest("Couldn't Process Update");
    }

    /// <summary>
    /// Manage Limits On Expenses
    /// </summary>

    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPost("AddLimit")]
    public async Task<IActionResult> AddLimitAsync([FromForm] LimitsDto limitDto)
    {
        var result = await _limitsManageService.SetLimitsAsync(
            limitDto.Map(UserId));
        if (result)
        {
            return Ok("Limit added successfully");
        }
        return BadRequest();
    }

    [HttpDelete("DeleteLimit")]
    public async Task<IActionResult> DeleteLimitAsync(int limitId)
    {
        var result = await _limitsManageService.DeleteLimitsAsync(limitId, UserId);
        if (result)
        {
            return Ok("Limit deleted successfully");
        }
        return BadRequest("something went wrong");
    }
    
    [ServiceFilter(typeof(CategoryValidationFilter))]
    [HttpPut("UpdateLimitsAsync")]
    public async Task<IActionResult> UpdateLimitAsync([FromForm] UpdateLimitDto updateLimitDto)
    {
        var result = await _limitsManageService.UpdateLimitsAsync(
            updateLimitDto.Map(UserId));
        if (result)
        {
            return Ok(" limit updated successfully");
        }
        return BadRequest();
    }

    [HttpGet("ActiveLimitsDetails")]
    public async Task<IActionResult> GetLimitsDetails()
    {
        var result = await _limitsManageService.GetLimitsDetails(UserId);
        var model = result.Select(x => x.Item1.Map(x.Item2));
        if (result.Any())
        {
            return Ok(model);
        }
        return BadRequest("Records not found");
    }
}