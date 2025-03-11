using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifications
{
  public interface IToggleNotificationsService
    {
        Task<bool> ToggleNotificationsAsync(string userId,bool notification);
    }
}
