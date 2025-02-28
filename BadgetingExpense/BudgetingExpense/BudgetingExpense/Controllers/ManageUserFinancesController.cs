using BudgetingExpense.api.ViewModels.ExpenseViewModel;
using BudgetingExpense.api.ViewModels.IncomeViewModels;
using BudgetingExpense.Domain.Models.Models.MainModels;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;
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
        return BadRequest();
    }

    [Authorize(Roles = "User")]
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
        if (result == true)
        {
            return Ok("Income Type Added Successfully");
        }
        return BadRequest();
    }

    [HttpDelete("DeleteIncome")]
    public async Task<IActionResult> DeleteIncome(int incomeTypeId)
    {
        var result = await _incomeService.DeleteIncomeAsync(incomeTypeId);
        if (result == true)
        {
            return Ok("Income Source Deleted Successfully");
        }

        return BadRequest("Something went wrong contact the site owner");

    }

    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> UpdateIncome(UpdateIncomeViewModel model)
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
        var categoryUpdated = await _incomeService.UpdateIncomeCategoryAsync(model.Category);
        var incomeUpdated = await _incomeService.UpdateIncomeAsync(income);
        if (incomeUpdated == true && categoryUpdated == true)
        {
            return Ok("Successfully updated");

        }
        else
        {
            return BadRequest("Could Not Process Update Try Again!");
        }


    }
   
    [HttpPost("AddExpenses")]
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

    [HttpDelete("DeleteExpenses")]
    public async Task<IActionResult> DeleteExpenses(int expenseId)
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

    [HttpPut("UpdateExpenses")]
    public async Task<IActionResult> UpdateExpenses(UpdateExpenseViewModel updateExpenseViewModel)
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
        var category = await _expenseManageService.UpdateCategoryAsync(updateExpenseViewModel.category);
        var result = await _expenseManageService.UpdateExpenseAsync(expenses);
        if (result == true && category == true )
        {
            return Ok("updates successfully");
        }
        else
        {
            return BadRequest("something went wrong");
        }

    }

    [HttpPost("AddExpenseCategory")]
    public async Task<IActionResult> AddExpenseCategory(string category)
    {
        var result = await _expenseManageService.AddExpenseCategoryAsync(category);
        if (result > 0)
        {
            return Ok($"{result} Category added Successfully");
        }
        return BadRequest();

    }
}
