using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[Authorize(Roles = "User")]
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
        return BadRequest("Couldn'd Process Add");
    }

    [HttpPost("AddIncome")]
    public async Task<IActionResult> AddIncome([FromForm] IncomeDto dtoModel)
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
        if (result)
        {
            return Ok("Income Added Successfully");
        }
        return BadRequest("Couldn'd Process Add");
    }

    [HttpDelete("Income")]
    public async Task<IActionResult> DeleteIncome(int incomeTypeId)
    {
        var result = await _incomeService.DeleteIncomeAsync(incomeTypeId);
        if (result == true)
        {
            return Ok("Income Source Deleted Successfully");
        }
        return BadRequest("Could not Process Delete");
    }

    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> UpdateIncome([FromForm]UpdateIncomeViewModel model)
    {
        var income = new Income()
        {
            Id = model.Income.Id,
            Amount = model.Income.Amount,
            CategoryId = model.Income.CategoryId,
            Currency = model.Income.Currency,
            Date = model.Income.Date,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var incomeUpdated = await _incomeService.UpdateIncomeAsync(income);
        if (incomeUpdated && categoryUpdated)
        {
            return Ok("Successfully Updated");
        }
        return BadRequest("Couldn'd Process Update");
    }
   
    [HttpPost("AddExpenses")]
    public async Task<IActionResult> AddExepenses([FromForm]ExpenseDto expenseDto)
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
        if (result)
        {
            return Ok("Added Successfully");
        }
        return BadRequest("Couldn'd Process Add");
    }

    [HttpDelete("Expenses")]
    public async Task<IActionResult> DeleteExpenses(int expenseId)
    {
        var result = await _expenseManageService.DeleteExpenseAsync(expenseId);
        if (result)
        {
            return Ok("deleted successfully");
        }
        return BadRequest("Couldn'd Process Delete");
    }

    [HttpPut("UpdateExpenses")]
    public async Task<IActionResult> UpdateExpenses([FromForm]UpdateExpenseViewModel updateExpenseViewModel)
    {
        var expenses = new Expense()
        {
            Id = updateExpenseViewModel.expenses.Id,
            Currency = updateExpenseViewModel.expenses.Currency,
            Amount = updateExpenseViewModel.expenses.Amount,
            CategoryId = updateExpenseViewModel.expenses.CategoryId,
            Date = updateExpenseViewModel.expenses.Date,
            UserId = HttpContext.Items["UserId"].ToString()
        };
        var result = await _expenseManageService.UpdateExpenseAsync(expenses);
        if (result && category)
        {
            return Ok("Updated Successfully");
        }
        return BadRequest("Couldn'd Process Update");
    }

    [HttpPost("AddExpenseCategory")]
    public async Task<IActionResult> AddExpenseCategory(string category)
    {
        var result = await _expenseManageService.AddExpenseCategoryAsync(category);
        if (result > 0)
        {
            return Ok($"{result} Category Added Successfully");
        }
        return BadRequest("Couldn't Process Add");
    }
}
