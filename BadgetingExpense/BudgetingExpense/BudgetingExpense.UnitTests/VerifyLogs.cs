using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetingExpense.UnitTests
{
    public class VerifyLogs
    {
        public void VerifyLogInformation<T>(Mock<ILogger<T>> loggerMock)
        {
            loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
        }

        public void VerifyLogError<T>(Mock<ILogger<T>> loggerMock)
        {
            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()

            ), Times.Once);
        }
    }
}
