using BudgetingExpense.api.ViewModels.IncomeViewModels;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManageUserFinancesController : ControllerBase
{
    private readonly IIncomeManageService _incomeService;
    private readonly IExpenseManageService _expenseManageService;

    public ManageUserFinancesController(IIncomeManageService incomeService, IExpenseManageService expenseManageService)
    {
        _incomeService = incomeService;
        _expenseManageService = expenseManageService;
    }

    [HttpPost("AddIncomeCategory")]
    public async Task<IActionResult> AddIncomeCategory(string category)
    {
        var result = await _incomeService.AddIncomeCategoryAsync(category);
        if (result > 0)
        {
            return Ok($"{result} Category added Successfully");
        }
        return BadRequest();
    }

    [Authorize(Roles = "User")]
    [HttpPost("AddIncomeType")]
    public async Task<IActionResult> AddIncomeType([FromForm] IncomeDto dtoModel)
    {
        string userId = HttpContext.Items["UserId"].ToString();
        var model = new Income
        {
            Amount = dtoModel.Amount,
            CategoryId = dtoModel.CategoryId,
            Currency = dtoModel.Currency,
            Date = dtoModel.Date,
            UserId = userId
        };
        var result = await _incomeService.AddIncomeAsync(model);
        if (result == true)
        {
            return Ok("Income Type Added Successfully");
        }
        return BadRequest();
    }

    [HttpDelete("DeleteIncomeType")]
    public async Task<IActionResult> DeleteIncomeType(int incomeTypeId)
    {
        var result = await _incomeService.DeleteIncomeAsync(incomeTypeId);
        if (result == true)
        {
            return Ok("Income Source Deleted Successfully");
        }

        return BadRequest("Something went wrong contact the site owner");

    }

    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> Update(UpdateIncomeViewModel model)
    {
        var category = new Category
        {
            Id = model.CategoryDto.Id,
            Name = model.CategoryDto.Name,
        };
        var categoryUpdated = await _incomeService.UpdateIncomeCategoryAsync(category);
        var incomeUpdated = await _incomeService.UpdateIncomeAsync(model.Income);
        if (incomeUpdated == true && categoryUpdated == true)
        {
            return Ok("Successfully updated");

        }
        else
        {
            return BadRequest("Could Not Process Update Try Again!");
        }


    }
    [Authorize(Roles ="User")]
    [HttpPost("addExpenses")]
    public async Task<IActionResult> AddExepenses(ExpenseDto expenseDto)
    {
        var expense = new Expense
        {
            Amount = expenseDto.Amount,
            CategoryId = expenseDto.CategoryId,
            Currency = expenseDto.Currency,
            Date = expenseDto.Date,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _expenseManageService.AddExpenseAsync(expense);
        if (result == true)
        {
            return Ok("added successfully");
        }
        else
        {
            return BadRequest("something went wrong");
        }
    }

    [HttpDelete("deleteExpenses")]
    public async Task<IActionResult> deleteExpenses(int expenseId)
    {
        var result = await _expenseManageService.DeleteExpenseAsync(expenseId);
        if (result == true)
        {
            return Ok("deleted successfully");
        }
        else
        {
            return BadRequest("something went wrong");
        }
    }

    [HttpPut("updateExpenses")]
    public async Task<IActionResult> UpdateExpenses(UpdateExpenseDto expenseDto)
    {
        var expense = new Expense
        {
            Id = expenseDto.Id,
            CategoryId = expenseDto.CategoryId,
            Amount = expenseDto.Amount,
            Date = expenseDto.Date,
            Currency = expenseDto.Currency,
            UserId = HttpContext.Items["UserId"].ToString()
        };

        var result = await _expenseManageService.UpdateExpenseAsync(expense);
        if (result == true)
        {
            return Ok("updates successfully");
        }
        else
        {
            return BadRequest("something went wrong");
        }

    }
}
