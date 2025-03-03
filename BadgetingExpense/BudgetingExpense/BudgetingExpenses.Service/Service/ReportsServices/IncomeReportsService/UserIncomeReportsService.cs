﻿using BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices.IIncomeReportsService;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.ReportsServices.IncomeReportsService;

public class UserIncomeReportsService : IIncomeReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserIncomeReportsService(IUnitOfWork unitOfWork)
    { 
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<IncomeRecord>?> GetAllRecords(string userId)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            if (records != null)
            {
                return records;
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category);
                }
                return records.Where(x => x.CategoryName == model.Category && x.IncomeDate >= period);
            }
            return null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrecncyPeriod(GetRecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Currency > 0)
                {
                    return records.Where(x => x.Currency == model.Currency);
                }
                return records.Where(x => x.Currency == model.Currency && x.IncomeDate >= period);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
