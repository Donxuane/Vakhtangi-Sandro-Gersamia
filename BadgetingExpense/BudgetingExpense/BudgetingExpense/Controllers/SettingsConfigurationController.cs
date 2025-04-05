using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("notifications/[controller]")]
public class SettingsConfigurationController : BaseControllerExstention
{
    private readonly IToggleNotificationsService _toggleNotificationsService;

    public SettingsConfigurationController(IToggleNotificationsService toggleNotificationsRepository)
    {
        _toggleNotificationsService = toggleNotificationsRepository;
    }

    [HttpPut("toggleNotifications")]
    public async Task<IActionResult> ToggleNotificationAsync(bool notification)
    {
        var result = await _toggleNotificationsService.ToggleNotificationsAsync(UserId,
            notification);
        if (result)
        {
            return Ok(new { message = $"Notification set to {notification}" });
        }
        return BadRequest(new { message = "Couldn't set" });
    }

    [HttpDelete("LogOut")]
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("refreshToken");
        HttpContext.Response.Headers.Remove("Authorization");
        return Ok(new { message = "Logged out" });
    }
}
