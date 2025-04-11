using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Notifications
{
    public class ExpenseAddedNotificationTest :VerifyLogs
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailService> _mailServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<ExpenseAddedNotificationService>> _loggerMock;
        private readonly ExpenseAddedNotificationService _expenseAddedNotificationService;

        public ExpenseAddedNotificationTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mailServiceMock = new Mock<IEmailService>();
            _loggerMock = new Mock<ILogger<ExpenseAddedNotificationService>>();
            _expenseAddedNotificationService = new ExpenseAddedNotificationService(_configurationMock.Object,_mailServiceMock.Object,_loggerMock.Object,_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task SendEmailWhileExpenseAddedAsync_ShouldSendEmail_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid().ToString();
            var categoryId = 1;
          
            var expense = new Expense
            {
                Amount = 11,
                CategoryId = 2,
                Currency = Currencies.EUR,
                Date = DateTime.UtcNow,
                Id = 1,
                UserId = userId
            };
            _unitOfWorkMock.Setup(x => x.GetRepository.GetEmailAsync(userId)).ReturnsAsync("email");
            _unitOfWorkMock.Setup(x => x.GetRepository.GetCategoryNameAsync(It.IsAny<int>())).ReturnsAsync("categoryId");
         
            _configurationMock.Setup(x => x.GetSection("ExpenseEmail")["Subject"]).Returns("Subject");
            _configurationMock.Setup(x => x.GetSection("ExpenseEmail")["Message"]).Returns("Message");
   _mailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<EmailModel>())).Returns(Task.CompletedTask);
            var result = await _expenseAddedNotificationService.SendEmailWhileExpenseAddedAsync(expense);

            Assert.True(result);

            _unitOfWorkMock.Verify(x => x.GetRepository.GetEmailAsync(userId),Times.Once);
            _unitOfWorkMock.Verify(x => x.GetRepository.GetCategoryNameAsync(It.IsAny<int>()), Times.Once);
            _configurationMock.Verify(x => x.GetSection("ExpenseEmail")["Subject"],Times.Once);
            _configurationMock.Verify(x => x.GetSection("ExpenseEmail")["Message"],Times.Once);
            _mailServiceMock.Verify(x=>x.SendEmailAsync(It.IsAny<EmailModel>()),Times.Once);

        }
        [Fact]
        public async Task SendEmailWhileExpenseAddedAsync_WhenGetEmailFails_ShouldThrowException_ShouldReturnFalse()
        {
            
            var userId = Guid.NewGuid().ToString();
            var expense = new Expense
            {
                Amount = 20,
                CategoryId = 2,
                Currency = Currencies.USD,
                Date = DateTime.UtcNow,
                Id = 1,
                UserId = userId
            };

            
            _unitOfWorkMock.Setup(x => x.GetRepository.GetEmailAsync(userId))
                .ThrowsAsync(new Exception());

         
            await Assert.ThrowsAsync<Exception>(() =>
                _expenseAddedNotificationService.SendEmailWhileExpenseAddedAsync(expense));
        }


    }
}
