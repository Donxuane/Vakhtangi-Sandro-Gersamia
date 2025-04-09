using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.Limitations;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Limitations
{
    public class LimitationsTest :VerifyLogs
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<LimitsService>> _mockLimitsServiceLogger;
        private readonly Mock<IGetRepository> _mockedGetRepository;
        private readonly LimitsService _limitsService;

        public LimitationsTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLimitsServiceLogger = new Mock<ILogger<LimitsService>>();
            _mockedGetRepository = new Mock<IGetRepository>();
            _limitsService = new LimitsService(_mockUnitOfWork.Object, _mockLimitsServiceLogger.Object
                , _mockedGetRepository.Object);
        }

        [Fact]
        public async Task SetLimitsAsync_ShouldSetLimit_ShouldReturnTrue()
        {
            var limit = new Limits
            {
                Amount = 100, CategoryId = 1, DateAdded = DateTime.UtcNow, PeriodCategory = 1, Id = 1 ,UserId = "user id"
            };
            _mockUnitOfWork.Setup(x => x.LimitsRepository.AddLimitAsync(limit)).Returns(Task.CompletedTask);

            var result = await _limitsService.SetLimitsAsync(limit);
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.LimitsRepository.AddLimitAsync(limit), Times.Once);
            _mockUnitOfWork.Verify(x=>x.SaveChangesAsync(),Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Never);
            VerifyLogInformation(_mockLimitsServiceLogger);
        }

        [Fact]
        public async Task SetLimitsAsync_ShouldNotSetLimits_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.LimitsRepository.AddLimitAsync(It.IsAny<Limits>()))
                .ThrowsAsync(new Exception());
            var result = await _limitsService.SetLimitsAsync(It.IsAny<Limits>());
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.LimitsRepository.AddLimitAsync(It.IsAny<Limits>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
            VerifyLogError(_mockLimitsServiceLogger);
        }

        [Fact]
        public async Task DeleteLimitsAsync_ShouldDeleteLimits_ShouldReturnTrue()
        {
            int limitId = 1;
            _mockUnitOfWork.Setup(x => x.LimitsRepository.DeleteLimitsAsync(limitId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            var result = await _limitsService.DeleteLimitsAsync(limitId, It.IsAny<string>());
            Assert.True(result);
            _mockUnitOfWork.Verify(x=> x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.LimitsRepository.DeleteLimitsAsync(limitId, It.IsAny<string>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            VerifyLogInformation(_mockLimitsServiceLogger);


        }

        [Fact]
        public async Task DeleteLimitsAsync_ShouldNotDeleteLimits_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.LimitsRepository.DeleteLimitsAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            var result = await _limitsService.DeleteLimitsAsync(It.IsAny<int>(), It.IsAny<string>());
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x =>x.LimitsRepository.DeleteLimitsAsync(It.IsAny<int>(),It.IsAny<string>()),Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
            VerifyLogError(_mockLimitsServiceLogger);
        }

        [Fact]
        public async Task UpdateLimitsAsync_ShouldUpdateLimits_ShouldReturnTrue()
        {
            var Limits = new Limits()
            {
                Amount = 100, CategoryId = 1, Currency = Currencies.EUR, DateAdded = DateTime.UtcNow, Id = 1,
                PeriodCategory = 1, UserId = "userId"
            };
            _mockUnitOfWork.Setup(x => x.LimitsRepository.UpdateLimitsAsync(Limits)).Returns(Task.CompletedTask);
            var result = await _limitsService.UpdateLimitsAsync(Limits);
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x=>x.LimitsRepository.UpdateLimitsAsync(Limits),Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            VerifyLogInformation(_mockLimitsServiceLogger);
        }

        [Fact]
        public async Task UpdateLimitsAsync_ShouldNotUpdateLimit_WhileExceptionThrown_ShouldReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.LimitsRepository.UpdateLimitsAsync(It.IsAny<Limits>()))
                .ThrowsAsync(new Exception());
            var result = await _limitsService.UpdateLimitsAsync(It.IsAny<Limits>());
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(x => x.LimitsRepository.UpdateLimitsAsync(It.IsAny<Limits>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
            VerifyLogError(_mockLimitsServiceLogger);
        }

        
    }
}
