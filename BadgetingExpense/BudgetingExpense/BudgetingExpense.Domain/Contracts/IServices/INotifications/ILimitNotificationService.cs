using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifications
{
 public interface ILimitNotificationService
 {
     public Task<bool> NotifyLimitExceededAsync(string userId);
 }
}
