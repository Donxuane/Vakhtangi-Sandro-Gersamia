using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.ManageFinances;
using BudgetingExpenses.Service.Service.Notifications;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.ManageFinances
{
    
    public class IncomeManageTest :VerifyLogs
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
            var result =await _IncomeManageService.AddIncomeCategoryAsync(It.IsAny<string>());

            Assert.NotNull(result);
           
            _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(),Times.Once);
            _mockIncomeManageServiceLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);


        }

        [Fact]
        public async Task addIncomeAsync_ShouldAddIncome_ShouldReturnTrue()
        {
            var model = new Income
                { Amount = 100, CategoryId = 1, Currency = Currencies.GEL, Date = DateTime.UtcNow, UserId = "User Id" };
            _mockUnitOfWork.Setup(x => x.IncomeManage.AddAsync(model)).Returns(Task.CompletedTask);
            var result = await _IncomeManageService.AddIncomeAsync(model);
            Assert.True(result);
            
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.AddAsync(model), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Never);

             VerifyLogInformation(_mockIncomeManageServiceLogger);

        }

        [Fact] 
        public async Task addIncomeAsync_ShouldNotAddIncome_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.IncomeManage.AddAsync(It.IsAny<Income>())).ThrowsAsync(new Exception());
            var result = _IncomeManageService.AddIncomeAsync(It.IsAny<Income>());

            Assert.NotNull(result);
            _mockUnitOfWork.Verify(x=> x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.AddAsync(It.IsAny<Income>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);

            VerifyLogError(_mockIncomeManageServiceLogger);


        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldDeleteIncome_ShouldReturnTrue()
        {
            int expenceId = 1;
            _mockUnitOfWork.Setup(x => x.IncomeManage.DeleteAsync(expenceId)).Returns(Task.CompletedTask);
            var result = await _IncomeManageService.DeleteIncomeAsync(expenceId);
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.DeleteAsync(expenceId), Times.Once);
            _mockUnitOfWork.Verify(x=>x.SaveChangesAsync(),Times.Once);

            VerifyLogInformation(_mockIncomeManageServiceLogger);
        }

        [Fact]
        public async Task aDeleteIncomeAsync_ShouldNot_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.IncomeManage.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception());
            var result = _IncomeManageService.DeleteIncomeAsync(It.IsAny<int>());
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.DeleteAsync(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync());
            VerifyLogError(_mockIncomeManageServiceLogger);
        }

        [Fact]
        public async Task GetAllIncomeRecordsAsync_ShouldReturnAllIncomeCategoryRecords_BasedUserId()
        {
            string userId = "user Id";
            var returnCollection = new List<Income>()
            {
                new()
                {
                    Id = 1, Amount = 10, CategoryId = 1, Currency = Currencies.GEL, Date = DateTime.UtcNow.AddDays(-3),
                    UserId = userId
                },
                new()
                {
                    Id = 2, Amount = 20, CategoryId = 1, Currency = Currencies.GEL, Date = DateTime.UtcNow.AddDays(-5),
                    UserId = userId
                },
            };
            _mockUnitOfWork.Setup(x => x.IncomeManage.GetAllAsync(userId)).ReturnsAsync(returnCollection);
            var result = await _IncomeManageService.GetAllIncomeRecordsAsync(userId);
            Assert.Equal(returnCollection,result);
            Assert.NotNull(result);
            Assert.All(returnCollection,record =>
                Assert.Contains(result,rec => rec.Id ==  record.Id));
            _mockUnitOfWork.Verify(x=>x.IncomeManage.GetAllAsync(userId),Times.Once);
            
        }

        [Fact]

        public async Task GetAllIncomeRecordsAsync_ShouldNotReturnAllIncomeCategoryRecords_WhileExceptionThrown_BasedUserId()
        {
            _mockUnitOfWork.Setup(x => x.IncomeManage.GetAllAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
            var result = await _IncomeManageService.GetAllIncomeRecordsAsync(It.IsAny<string>());
            Assert.Null(result);
            _mockUnitOfWork.Verify(x => x.IncomeManage.GetAllAsync(It.IsAny<string>()), Times.Once);
            VerifyLogError(_mockIncomeManageServiceLogger);


        }

        [Fact]
        public async Task GetAllIncomeCategoryRecordsAsync_ShouldReturnAllIncomeCategoryRecords_BasedOnUserId()
        {
            var userId = "User Id";
            var list = new List<Category>()
            {
                new() { Id = 1, Name = "Salary", Type = 1 },
                new() { Id = 2, Name = "Business", Type = 1 },
                new() { Id = 3, Name = "Salary", Type = 1 }
            };
            _mockUnitOfWork.Setup(x => x.IncomeManage.GetCategoriesAsync(userId)).ReturnsAsync(list);
            var result = await _IncomeManageService.GetAllIncomeCategoryRecordsAsync(userId);
            Assert.NotNull(result);
            Assert.All(list, item =>
            Assert.Contains(result, category =>
                category.Id == item.Id && category.Name == item.Name && category.Type == item.Type
            ));
            Assert.Equal(list, result);
        }
        [Fact]
        public async Task UpdateIncomeAsync_ShouldUpdateIncome_ShouldReturnTrue()
        {
            var updateIncome = new Update
            {
                Amount = 100, CategoryId = 1, Currency = Currencies.EUR, Date = DateTime.UtcNow, Id = 1,
                UserId = "userId"
            };
            _mockUnitOfWork.Setup(x => x.IncomeManage.UpdateAsync(updateIncome)).Returns(Task.CompletedTask);
            var result = await _IncomeManageService.UpdateIncomeAsync(updateIncome);
            Assert.True(result);
            _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
            _mockUnitOfWork.Verify(x=>x.IncomeManage.UpdateAsync(updateIncome),Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(),Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Never);
            VerifyLogInformation(_mockIncomeManageServiceLogger);
        }
        [Fact]
        public async Task UpdateIncomeAsync_ShouldNotUpdateIncome_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.IncomeManage.UpdateAsync(It.IsAny<Update>())).ThrowsAsync(new Exception());
            var result = await _IncomeManageService.UpdateIncomeAsync(It.IsAny<Update>());
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.IncomeManage.UpdateAsync(It.IsAny<Update>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
            VerifyLogError(_mockIncomeManageServiceLogger);

        }

        

        }



    }


