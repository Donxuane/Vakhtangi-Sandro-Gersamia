using Moq;
using Microsoft.Extensions.Logging;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.Service.ManageFinances;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.UnitTests.ManageFinances;
public class ExpenseManageServiceTests :VerifyLogs
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<ExpenseManageService>> _mockLogger;
    private readonly Mock<ILimitNotificationService> _mockLimitNotificationService;
    private readonly Mock<IExpenseAddedNotificationService> _mockExpenseAddedNotificationService;
    private readonly ExpenseManageService _service;

    public ExpenseManageServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<ExpenseManageService>>();
        _mockExpenseAddedNotificationService = new Mock<IExpenseAddedNotificationService>();
        _mockLimitNotificationService = new Mock<ILimitNotificationService>();
        _service = new ExpenseManageService(
            _mockUnitOfWork.Object,
            _mockLogger.Object,
            _mockLimitNotificationService.Object,
            _mockExpenseAddedNotificationService.Object );
    }

    [Fact]
    public async Task AddExpenseCategoryAsync_ShouldAddCategory_ShouldReturnAddedCategoryId()
    {
        string categoryName = "shopping";
        int expectedCategoryId = 1;
        _mockUnitOfWork.Setup(x => x.ExpenseManage.AddCategoryAsync(It.IsAny<Category>()))
            .ReturnsAsync(expectedCategoryId);

        var result = await _service.AddExpenseCategoryAsync(categoryName);

        Assert.Equal(expectedCategoryId, result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x => x.ExpenseManage.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        VerifyLogInformation(_mockLogger);
    }

    [Fact]
    public async Task AddExpenseAsync_ShouldAddExpenseInDatabase_ShouldReturnTrue()
    {
        var model = new Expense { Amount = 21, Date = DateTime.UtcNow, CategoryId = 1, Currency = Currencies.GEL, UserId = "User Id" };
        _mockUnitOfWork.Setup(x => x.ExpenseManage.AddAsync(model))
            .GetType();
        var result = await _service.AddExpenseAsync(model);
        Assert.True(result);
        _mockUnitOfWork.Verify(x=>x.RollBackAsync(),Times.Never);
        _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
        _mockUnitOfWork.Verify(x=>x.ExpenseManage.AddAsync(model), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        VerifyLogInformation(_mockLogger);
    }

    [Fact]
    public async Task DeleteExpenseAsync_ShouldDelete_Record()
    {
        int expenseId = 12;
        string userId = "user id";
        _mockUnitOfWork.Setup(x => x.ExpenseManage.DeleteAsync(expenseId, userId))
            .GetType();
        var result = await _service.DeleteExpenseAsync(expenseId,userId);
        Assert.True(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x => x.ExpenseManage.DeleteAsync(expenseId, userId), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        VerifyLogInformation(_mockLogger);
    }

    [Fact]
    public async Task GetAllExpenseCategoryRecordsAsync_ShouldReturnAllExpenseCategoryRecords_BasedUserId()
    {
        var userId = "User Id";
        var list = new List<Category>() { 
            new() { Id = 1, Name = "shopping", Type = 0 },
            new() { Id = 2, Name = "shopping1", Type = 0 },
            new() { Id = 3, Name = "shopping2", Type = 0 }
        };
        _mockUnitOfWork.Setup(x => x.ExpenseManage.GetCategoriesAsync(userId))
            .ReturnsAsync(list);
        var result = await _service.GetAllExpenseCategoryRecordsAsync(userId);
        Assert.NotNull(result);
        Assert.All(list, item =>
        Assert.Contains(result, category => 
        category.Id == item.Id && category.Name == item.Name && category.Type == item.Type
        ));
        Assert.Equal(list, result);
      
    }

    [Fact]
    public async Task UpdateExpenseAsync_ShouldNotProceedUpdate_WhileExceptionThrown()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.UpdateAsync(It.IsAny<Update>()))
            .ThrowsAsync(new Exception());

        var result = await _service.UpdateExpenseAsync(It.IsAny<Update>());
        Assert.False(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x => x.ExpenseManage.UpdateAsync(It.IsAny<Update>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task GetAllExpenseRecordsAsync_ShouldReturnAllExpenseRecords_BasedUserId()
    {
        string userId = "User Id";
        var returnCollection = new List<Expense>() { 
            new() { Id = 1,Amount = 10, CategoryId = 1,Currency = Currencies.GEL,Date = DateTime.UtcNow.AddDays(-3),UserId = userId},
            new() { Id = 2,Amount = 15, CategoryId = 1,Currency = Currencies.GEL,Date = DateTime.UtcNow.AddDays(-5),UserId = userId}
        };
        _mockUnitOfWork.Setup(x => x.ExpenseManage.GetAllAsync(userId))
            .ReturnsAsync(returnCollection);

        var result = await _service.GetAllExpenseRecordsAsync(userId);
        Assert.Equal(returnCollection, result);
        Assert.NotNull(result);
          Assert.All(returnCollection, record =>
          Assert.Contains(result, rec =>
            rec.Id == record.Id
        ));
        _mockUnitOfWork.Verify(x => x.ExpenseManage.GetAllAsync(userId), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldUpdateCategory_ShouldReturnTrue()
    {
        var category = new Category()
        {
            Id = 1,
            Name = "name",
            Type = 1
        };
        _mockUnitOfWork.Setup(x => x.ExpenseManage.UpdateCategoryAsync(category)).Returns(Task.CompletedTask);
        var result = await _service.UpdateCategoryAsync(category);
        Assert.True(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x=>x.ExpenseManage.UpdateCategoryAsync(category),Times.Once);
        _mockUnitOfWork.Verify(x=>x.SaveChangesAsync(),Times.Once);
        _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Never);
        VerifyLogInformation(_mockLogger);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldNotUpdateCategory_WhileExceptionThrown_ShouldReturnFalse()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.UpdateCategoryAsync(It.IsAny<Category>())).Throws(new Exception());
        var result = await _service.UpdateCategoryAsync(It.IsAny<Category>());
        Assert.False(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(),Times.Once);
        _mockUnitOfWork.Verify(x => x.ExpenseManage.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
        _mockUnitOfWork.Verify(x=>x.RollBackAsync(), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task addExpenseCategoryAsync_ShouldNotAddExpenseCategory_WhileExceptionThrown()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.AddCategoryAsync(It.IsAny<Category>()))
            .ThrowsAsync(new Exception());
        var result = await Assert.ThrowsAsync<Exception>(() => _service.AddExpenseCategoryAsync(It.IsAny<string>()));
        _mockUnitOfWork.Verify(x=>x.ExpenseManage.AddCategoryAsync(It.IsAny<Category>()),Times.Once);
        _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task addExpenseAsync_ShouldNotAddExpense_WhileExceptionThrown_ShouldReturnFalse()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.AddAsync(It.IsAny<Expense>())).ThrowsAsync(new Exception());
        var result = await _service.AddExpenseAsync(It.IsAny<Expense>());
        Assert.False(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x => x.ExpenseManage.AddAsync(It.IsAny<Expense>()));
        _mockUnitOfWork.Verify(x => x.RollBackAsync(), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task DeleteExpenseAsync_ShouldNotDeleteExpense_WhileExceptionThrown_ShouldReturnFalse()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.DeleteAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        var result = await _service.DeleteExpenseAsync(It.IsAny<int>(), It.IsAny<string>());
        Assert.False(result);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(x=>x.ExpenseManage.DeleteAsync(It.IsAny<int>(),It.IsAny<string>()),Times.Once);
        _mockUnitOfWork.Verify(x=>x.RollBackAsync(),Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task GetAllExpenseRecords_ShouldNotReturnAllExpenseRecords_WhileExceptionThrown_BaseUserId()
    {
        _mockUnitOfWork
            .Setup(x => x.ExpenseManage.GetAllAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

       
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllExpenseRecordsAsync("testUser"));

        _mockUnitOfWork.Verify(x => x.ExpenseManage.GetAllAsync(It.IsAny<string>()), Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task UpdateExpenseAsync_ShouldNotUpdateExpense_WhileExceptionThrown_ShouldReturnFalse()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.UpdateAsync(It.IsAny<Update>())).ThrowsAsync(new Exception());
        var result = await _service.UpdateExpenseAsync(It.IsAny<Update>());
        Assert.False(result);
        _mockUnitOfWork.Verify(x=>x.BeginTransactionAsync(),Times.Once);
        _mockUnitOfWork.Verify(x=>x.ExpenseManage.UpdateAsync(It.IsAny<Update>()));
        _mockUnitOfWork.Verify(x=>x.RollBackAsync(),Times.Once);
        VerifyLogError(_mockLogger);
    }

    [Fact]
    public async Task GetAllExpenseCategoryRecordsAsync_ShouldNotGetAllExpenseCategoryRecords_WhileExceptionThrown()
    {
        _mockUnitOfWork.Setup(x => x.ExpenseManage.GetCategoriesAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllExpenseCategoryRecordsAsync("UserId"));
        _mockUnitOfWork.Verify(x => x.ExpenseManage.GetCategoriesAsync(It.IsAny<string>()), Times.Once);
        VerifyLogError(_mockLogger);
    }
}