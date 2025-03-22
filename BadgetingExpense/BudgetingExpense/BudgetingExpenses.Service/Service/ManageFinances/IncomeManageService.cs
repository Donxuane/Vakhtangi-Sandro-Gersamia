using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.ManageFinances;

public class IncomeManageService : IIncomeManageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIncomeReceiveNotificationService _notificationService;
    private readonly ILogger<IncomeManageService> _logger;

    public IncomeManageService(IUnitOfWork unitOfWork, 
        IIncomeReceiveNotificationService notificationService, ILogger<IncomeManageService> logger)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<int> AddIncomeCategoryAsync(string CategoryName)
    {
        try
        {
            var category = new Category { Name = CategoryName };
            await _unitOfWork.BeginTransactionAsync();
            var result = await _unitOfWork.IncomeManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Added Income Category:{id}", CategoryName);
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return 0;
        }
    }

    public async Task<bool> AddIncomeAsync(Income model)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.IncomeManage.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Added Income:{id}\nUser:{userId}", model.Id, model.UserId);
            var sendEmail = await _notificationService.NotifyIncomeAsync(model);
            if (sendEmail)
            {
                _logger.LogInformation("Email Sent To User:{id}",model.Id);
                return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteIncomeAsync(int incomeTypeId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.IncomeManage.DeleteAsync(incomeTypeId);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Deleted Income:{id}", incomeTypeId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var categories = await _unitOfWork.IncomeManage.GetCategoriesAsync(userId);
            if (categories.Any())
            {
                return categories;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
        return null;
    }

    public async Task<bool> UpdateIncomeAsync(Income income)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.IncomeManage.UpdateAsync(income);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Updated Income:{id}\nUser:{userId}",income.Id,income.UserId);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateIncomeCategoryAsync(Category category)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.IncomeManage.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Updated Income Category:{id}", category.Id);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync();
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Income>?> GetAllIncomeRecordsAsync(string userId)
    {
        try
        {
            var incomeRecords = await _unitOfWork.IncomeManage.GetAllAsync(userId);
            if (incomeRecords.Any())
            {
                return incomeRecords;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
        return null;
    }
}
