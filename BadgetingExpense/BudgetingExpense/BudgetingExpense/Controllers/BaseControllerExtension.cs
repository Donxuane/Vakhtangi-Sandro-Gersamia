﻿using Microsoft.AspNetCore.Mvc;

namespace BudgetingExpense.Api.Controllers;

public class BaseControllerExtension : ControllerBase
{
    protected string? UserId
    {
        get
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userId))
            {
                return Convert.ToString(userId);
            }
            return null;
        }
    }
}
