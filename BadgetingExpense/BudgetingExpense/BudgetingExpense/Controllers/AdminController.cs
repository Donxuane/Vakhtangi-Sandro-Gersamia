using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : BaseControllerExtension
{

    /// <summary>
    /// Returns Link For Logs Page
    /// </summary>
    /// <returns>Link</returns>
    [HttpGet("LogsPageUrl")]
    public IActionResult UrlToLogs()
    {
        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        if (baseUrl != null)
            return Ok(new { url = baseUrl + "/AdminLogs" });
        return BadRequest(new {message = "Page is not available!"});  
    }
}
