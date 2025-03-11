using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("notifications/[controller]")]
    public class ToggleNotificationsController : ControllerBase
    {
        private readonly IToggleNotificationsService _toggleNotificationsService;

        public ToggleNotificationsController(IToggleNotificationsService toggleNotificationsRepository)
        {
            _toggleNotificationsService = toggleNotificationsRepository;
        }

        [HttpPut("toggleNotifications")]
        public async Task<IActionResult> toggleNotification(bool notification)
        {
            var result =
                 await _toggleNotificationsService.ToggleNotificationsAsync(HttpContext.Items["UserId"].ToString(),
                    notification);
            if (result != null)
            {
                return Ok("notification alert updated");
            }
            return BadRequest();
        }
    }
