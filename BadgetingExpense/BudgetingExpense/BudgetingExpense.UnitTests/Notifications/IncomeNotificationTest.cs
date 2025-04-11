using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Notifications
{
    public class IncomeNotificationTest : VerifyLogs
    {
        private readonly Mock<IEmailService> _mailServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<IncomeReceiveNotificationService>>_loggerMock;
        private readonly IncomeReceiveNotificationService _incomeReceiveNotificationService;

        public IncomeNotificationTest()
        {
            _mailServiceMock = new Mock<IEmailService>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<IncomeReceiveNotificationService>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _incomeReceiveNotificationService = new IncomeReceiveNotificationService(_mailServiceMock.Object,
                _unitOfWorkMock.Object, _configurationMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task NotifyIncomeAsync_ShouldSendEmailWhenIncomeAdded_ShouldReturnTrue()
        {
            var userId = "userid";
            var email = "123@Gmail.com";
            var income = new Income
            {
                Id = 1,
                UserId = "userid",
                CategoryId = 10,
                Amount = 100,
                Date = DateTime.Now
            };

            _unitOfWorkMock.Setup(x => x.GetRepository.GetEmailAsync(userId)).ReturnsAsync(email);
            _unitOfWorkMock.Setup(x => x.GetRepository.GetCategoryNameAsync(It.IsAny<int>())).ReturnsAsync("salary");
            _configurationMock.Setup(x => x.GetSection("IncomeEmail")["Subject"]).Returns("Subject");
            _configurationMock.Setup(x => x.GetSection("IncomeEmail")["Message"]).Returns("Message");
            _mailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<EmailModel>())).Returns(Task.CompletedTask);

            var result = await _incomeReceiveNotificationService.NotifyIncomeAsync(income);
            Assert.True(result);
            
            _unitOfWorkMock.Verify(x => x.GetRepository.GetEmailAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(x => x.GetRepository.GetCategoryNameAsync(It.IsAny<int>()), Times.Once);
            _configurationMock.Verify(x => x.GetSection("IncomeEmail")["Subject"], Times.Once);
            _configurationMock.Verify(x => x.GetSection("IncomeEmail")["Message"], Times.Once);
            _mailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<EmailModel>()), Times.Once);
        }

        [Fact]
        public async Task SendEmailWhileExpenseAdded_WhenGetEmailFails_ShouldThrowException_ShouldReturnFalse()
        {
            var userId  = Guid.NewGuid().ToString();
            var income = new Income
            {
                Amount = 20,
                CategoryId = 2,
                Currency = Currencies.USD,
                Date = DateTime.UtcNow,
                Id = 1,
                UserId = userId
            };
            _unitOfWorkMock.Setup(x => x.GetRepository.GetEmailAsync(userId)).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() =>
                _incomeReceiveNotificationService.NotifyIncomeAsync(income));
        }
    }
}
