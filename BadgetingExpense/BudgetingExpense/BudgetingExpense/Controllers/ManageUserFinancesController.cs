using BudgetingExpense.api.ViewModels;
using BudgetingExpense.Domain.Models;
using BudgetingExpense.Domain.Models.DtoModels;
using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManageUserFinancesController : ControllerBase
{
    private readonly IIncomeManageService _incomeService;
    
    public ManageUserFinancesController(IIncomeManageService incomeService)
    {
        _incomeService = incomeService;
    }

    [HttpPost("AddIncomeCategory")]
    public async Task<IActionResult> AddIncomeCategory(string category)
    {
        var result = await _incomeService.AddIncomeCategoryAsync(category);
        if(result >0)
        {
            return Ok($"{result} Category added Successfully");
        }
        return BadRequest();
    }

    [HttpPost("AddIncomeType")]
    public async Task<IActionResult> AddIncomeType([FromForm]IncomeDto model)
    {
        var result = await _incomeService.AddIncomeAsync(model);
        if(result == true)
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
    public async Task<IActionResult> Update(UpdateIncomeViewModel updateIncomeViewModel)
    {
        var category = await _incomeService.UpdateIncomeCategoryAsync(updateIncomeViewModel.CategoryDto);
         var incomeResult = await _incomeService.UpdateIncomeAsync(updateIncomeViewModel.Income);
         if ( incomeResult ==true)
         {
             return Ok("Successfully updated");

        }
         else
         {
             return BadRequest("something went wrong");
         }

       


    }


}
