﻿using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using BudgetingExpenses.Service.IServiceContracts;

namespace BudgetingExpenses.Service.Service;

public class IncomeManageService : IIncomeManageService
{
    private readonly IUnitOfWork _unitOfWork;

    public IncomeManageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> AddIncomeCategoryAsync(string CategoryName)
    {
        try
        {
            var category = new Category { Name = CategoryName };
            var result = await _unitOfWork.IncomeManage.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return result;
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return 0;
    }

    public async Task<bool> AddIncomeAsync(Income model)
    {
        try
        {    
            await _unitOfWork.IncomeManage.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteIncomeAsync(int incomeTypeId)
    {
        try
        {
            await _unitOfWork.IncomeManage.DeleteAsync(incomeTypeId);
            await _unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId)
    {
        try
        {
            var categories = await _unitOfWork.IncomeManage.GetCategoriesAsync(userId);
            if (categories.Any())
            {
                return categories;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<bool> UpdateIncomeAsync(Income income)
    {
        try
        {
            await _unitOfWork.IncomeManage.UpdateAsync(income);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _unitOfWork.RollBackAsync();
          
        }

        return false;
    }

    public async Task<bool> UpdateIncomeCategoryAsync(Category category)
    {
        try
        {
            category.Type = 1;
            await _unitOfWork.IncomeManage.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollBackAsync();
        }
        return false;
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}
