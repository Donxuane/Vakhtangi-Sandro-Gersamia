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
            return Ok("notification alert updated");
        }
        return BadRequest();
    }
}
