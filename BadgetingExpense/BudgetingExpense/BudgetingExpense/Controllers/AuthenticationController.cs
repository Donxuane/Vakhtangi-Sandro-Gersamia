using BudgetingExpenses.Service.DtoModels;
using BudgetingExpenses.Service.IServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[ApiController]
[Route("Auth/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _auth;
    public AuthenticationController(IAuthenticationService auth)
    {
        _auth = auth;
    }
    [HttpPost(Name ="LoginUser")]
    public async Task<IActionResult> LogInUserAync([FromForm] LoginDto user)
    {
        var logedUser = await _auth.LoginUserServiceAsync(user);
        if(logedUser == true)
        {
            await _auth.GenerateJwtTokenAsync()
        }
    }

}
