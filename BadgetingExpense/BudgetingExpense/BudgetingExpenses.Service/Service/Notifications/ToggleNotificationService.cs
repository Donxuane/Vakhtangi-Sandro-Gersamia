using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;

namespace BudgetingExpenses.Service.Service.Notifications
{
    public class ToggleNotificationService : IToggleNotificationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleNotificationService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ToggleNotificationsAsync(string userId, bool notification)
        {
            try
            {
             await _unitOfWork.ToggleNotificationsRepository.ToggleNotification(userId);
             return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
          
          
        }
    }
}
