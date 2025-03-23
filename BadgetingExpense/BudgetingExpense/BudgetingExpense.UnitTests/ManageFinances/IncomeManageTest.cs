using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.ManageFinances;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.ManageFinances
{
    
    public class IncomeManageTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<IncomeManageService>> _mockIncomeManageServiceLogger;
        private readonly Mock<IIncomeReceiveNotificationService> _mockIncomeReceiveNotificationService;
        private readonly IncomeManageService _IncomeManageService;
        public IncomeManageTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockIncomeManageServiceLogger = new Mock<ILogger<IncomeManageService>>();
            _mockIncomeReceiveNotificationService = new Mock<IIncomeReceiveNotificationService>();
            _IncomeManageService = new IncomeManageService(_mockUnitOfWork.Object,
                _mockIncomeReceiveNotificationService.Object, _mockIncomeManageServiceLogger.Object);

        }

        [Fact]
        public async Task AddIncomeCategoryAsync_ShouldAddCategory_ShouldReturnAddedCategoryId()
        {
            string categoryName = "Salary";
            int categoryId = 1;
            _mockUnitOfWork.Setup(x => x.IncomeManage.AddCategoryAsync(It.IsAny<Category>())).ReturnsAsync(categoryId);

            var result = await _IncomeManageService.AddIncomeCategoryAsync(categoryName);

            Assert.NotNull(result);
            Assert.Equal(categoryId,result);
            _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
            _mockUnitOfWork.Verify(x=>x.IncomeManage.AddCategoryAsync(It.IsAny<Category>()),Times.Once);
            _mockUnitOfWork.Verify(x=>x.SaveChangesAsync(),Times.Once);
        }

        [Fact]
        public async Task AddIncomeCategoryAsync_ShouldNotAddCategory_WhileExceptionThrown_ShouldReturnZero()
        {
            _mockUnitOfWork.Setup(x => x.IncomeManage.AddCategoryAsync(It.IsAny<Category>())).ThrowsAsync(new Exception());
            var result = _IncomeManageService.AddIncomeCategoryAsync(It.IsAny<string>());

            Assert.NotNull(result);
           
            _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync());
            _mockIncomeManageServiceLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);


        }
    }
}
