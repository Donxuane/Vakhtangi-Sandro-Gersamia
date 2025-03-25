using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.Limitations;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests.Limitations
{
    public class LimitationsTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<LimitsService>> _mockLimitsServiceLogger;
        private readonly LimitsService _limitsService;

        public LimitationsTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLimitsServiceLogger = new Mock<ILogger<LimitsService>>();
            _limitsService = new LimitsService(_mockUnitOfWork.Object, _mockLimitsServiceLogger.Object);
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
            _mockUnitOfWork.Verify(x=>x.RollBackAsync(),Times.Once);
        }
        
    }
}
