using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

[Authorize(Roles = "User,Admin")]
[ApiController]
[Route("notifications/[controller]")]
public class SettingsConfigurationController : BaseControllerExstension
{
    private readonly IToggleNotificationsService _toggleNotificationsService;

    public SettingsConfigurationController(IToggleNotificationsService toggleNotificationsRepository)
    {
        _toggleNotificationsService = toggleNotificationsRepository;
    }

    /// <summary>
    /// Updates Notification Status (true/false)
    /// </summary>
    /// <param name="notification"></param>
    /// <returns>message</returns>
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

    /// <summary>
    /// Delete Cookies and Headers
    /// </summary>
    /// <returns>message</returns>
    [HttpDelete("LogOut")]
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("refreshToken");
        HttpContext.Response.Headers.Remove("Authorization");
        return Ok(new { message = "Logged out" });
    }
}
