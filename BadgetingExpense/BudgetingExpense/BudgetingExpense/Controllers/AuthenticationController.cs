using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using BudgetingExpense.Domain.Models.AuthenticationModels;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _auth;
    public AuthenticationController(IAuthenticationService auth)
    {
        _auth = auth;
    }
    [HttpPost("LoginUser")]
    public async Task<IActionResult> LogInUserAync([FromForm] Login user)
    {
        var token = await _auth.LoginUserServiceAsync(user);
        if (token != null)
        {
            return Ok(new { token });
        }
        return BadRequest("Couldn't Process Login");
    }

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUserAsync([FromForm]Register user)
    {
        var register = await _auth.RegisterUserAsync(user);
        if(register == true)
        {
            return Ok("You Registered Successfully");
        }
        return BadRequest();
    }
}
