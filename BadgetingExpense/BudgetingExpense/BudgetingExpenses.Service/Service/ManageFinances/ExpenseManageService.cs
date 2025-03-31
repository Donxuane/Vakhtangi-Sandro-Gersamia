using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.ManageFinances;

public class ExpenseManageService : IExpenseManageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpenseManageService> _logger;
    private readonly IExpenseAddedNotificationService _expenseNotify;
    private readonly ILimitNotificationService _limitNotificationService;
    public ExpenseManageService(IUnitOfWork unitOfWork, ILogger<ExpenseManageService> logger,
        ILimitNotificationService limitNotificationService, IExpenseAddedNotificationService expenseNotify)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _limitNotificationService = limitNotificationService;
        _expenseNotify = expenseNotify;
    }

    public async Task<int> AddExpenseCategoryAsync(string categoryName)
    {
        try
        {
            var category = new Category { Name = categoryName };
            await _unitOfWork.BeginTransactionAsync();
            var result = await _unitOfWork.ExpenseManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Added Expense Category:{name}", categoryName);
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}",ex.Message);
            return 0;
        }
    }

    public async Task<bool> AddExpenseAsync(Expense model)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ExpenseManage.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            var check = await _expenseNotify.SendEmailWhileExpenseAddedAsync(model);
            if (check)
            {
                _logger.LogInformation("Email send to {user}", model.UserId);
            }
            await _limitNotificationService.NotifyLimitExceededAsync(model.UserId);
            _logger.LogInformation("Added Expense:{id}\nUser:{userId}", model.Id, model.UserId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId, string userId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ExpenseManage.DeleteAsync(expenseId, userId);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Deleted Expense:{id}", expenseId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId)
    {
        try
        {
            var categories = await _unitOfWork.ExpenseManage.GetCategoriesAsync(userId);
                return categories;
            }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }

    public async Task<bool> UpdateExpenseAsync(Update expense)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ExpenseManage.UpdateAsync(expense);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Updated Expense:{id}\nUser:{userId}", expense.Id, expense.UserId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateCategoryAsync(Category Category)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ExpenseManage.UpdateCategoryAsync(Category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Updated Category:{id}",Category.Id);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }

    }

    public async Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId)
    {
        try
        {
            var records = await _unitOfWork.ExpenseManage.GetAllAsync(userId);
                return records;
            }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }
}
