﻿using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpenses.Service.DtoModels;
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
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogInUserAsync([FromForm] Login user)
    {
        var token = await _auth.LoginUserServiceAsync(user);
        if (token != null)
        {
            return Ok(new { token });
        }
        return BadRequest(new { message = "Password or Email is incorrect!" });
    }
  
    [HttpPost("RegisterWithValidation")]
    public IActionResult GenerateValidationCode([FromForm]Register user)
    {
        var result = _auth.CacheNewUserCredentialsInMemory(user);
        if (result)
        {
            return Ok(new { message = "Check email for verification!" });
        }
        return BadRequest(new { message = "Code can't be generated fro now!\nTry later" });
    }

    [HttpPost("ValidateEmail")]
    public async Task<IActionResult> ValidateAndRegisterUser([FromForm]EmailValidation model)
    {
        var result = await _auth.VerifyUserEmailAsync(model.Email, model.Code);
        if (result)
        {
            return Ok(new { message = "You registered successfully!" });
        }
        return BadRequest(new { message = "Code does not match!\nTry again" });
    }
}
