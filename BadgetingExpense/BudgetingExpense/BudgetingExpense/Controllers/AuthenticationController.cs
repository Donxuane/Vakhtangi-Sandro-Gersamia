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
        var logedUser = await _auth.LoginUserServiceAsync(user);
        if(logedUser)
        {
            var loggedUser = await _auth.GetUserAsync(user.Email);
            var roles = await _auth.GetRoleAsync(user.Email);
            var generateToken = await _auth.GenerateJwtTokenAsync(loggedUser.Id, roles.FirstOrDefault());
            return Ok(new {token = generateToken});
        }
        return BadRequest("Couldn't Process Login");
    }

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUserAsync([FromForm]Register user)
    {
        var register = await _auth.RegisterUserServiceAsync(user);
        if(register == true)
        {
            await _auth.AddUserRolesAsync(user.Email, "User");
            return Ok("You Registered Successfully");
        }
        return BadRequest();
    }
}
