﻿using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.INotifyUser;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.ManageFinances;

public class IncomeManageService : IIncomeManageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIncomeReceiveNotificationService _notificationService;

    public IncomeManageService(IUnitOfWork unitOfWork, IIncomeReceiveNotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<int> AddIncomeCategoryAsync(string CategoryName)
    {
        try
        {
            var category = new Category { Name = CategoryName };
            await _unitOfWork.BeginTransaction();
            var result = await _unitOfWork.IncomeManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return 0;
        }
    }

    public async Task<bool> AddIncomeAsync(Income model)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.IncomeManage.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            var sendEmail = await _notificationService.NotifyIncomeAsync(model);
            if (sendEmail)
            {
                return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteIncomeAsync(int incomeTypeId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.IncomeManage.DeleteAsync(incomeTypeId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var categories = await _unitOfWork.IncomeManage.GetCategoriesAsync(userId);
            if (categories.Any())
            {
                return categories;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<bool> UpdateIncomeAsync(Income income)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.IncomeManage.UpdateAsync(income);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateIncomeCategoryAsync(Category category)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            await _unitOfWork.IncomeManage.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
    }

    public async Task<IEnumerable<Income>?> GetAllIncomeRecordsAsync(string userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var incomeRecords = await _unitOfWork.IncomeManage.GetAllAsync(userId);
            if (incomeRecords.Any())
            {
                return incomeRecords;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}
