using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
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
        
    }
}
