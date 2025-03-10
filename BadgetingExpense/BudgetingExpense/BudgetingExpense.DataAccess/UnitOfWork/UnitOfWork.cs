﻿using BudgetingExpense.Domain.Contracts.IRepository.IFinance;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthentication _authentication;
    private readonly IManageFinancesRepository<Expense> _expenseManage;
    private readonly IManageFinancesRepository<Income> _incomeManage;
    private readonly IIncomeRecordsRepository _incomeRecords;
    private readonly IExpenseRecordsRepository _expenseRecords;
    private readonly IBudgetLimitsRepository _limits;
    private readonly IBudgetPlaningRepository _budgetPlaning;
    private readonly IGetRepository _getRepository;
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public UnitOfWork(IAuthentication authentication, DbConnection connection,
        IManageFinancesRepository<Expense> expenseManage, IManageFinancesRepository<Income> incomeManage,
        IIncomeRecordsRepository incomeRecords, IBudgetLimitsRepository limits,
        IExpenseRecordsRepository expenseRecords, IBudgetPlaningRepository budgetPlaning,
        IGetRepository getRepository)
    {
        _expenseManage = expenseManage;
        _incomeManage = incomeManage;
        _authentication = authentication;
        _incomeRecords = incomeRecords;
        _limits = limits;
        _expenseRecords = expenseRecords;
        _connection = connection;
        _budgetPlaning = budgetPlaning;
        _getRepository = getRepository;
    }

    public IAuthentication Authentication => _authentication;

    public IManageFinancesRepository<Expense> ExpenseManage 
    {
        get 
        {
            if (_transaction != null)
            {
                _expenseManage.SetTransaction(_transaction);
            }
            return _expenseManage;
        } 
    }
    public IManageFinancesRepository<Income> IncomeManage 
    {
        get
        {
            if (_transaction != null)
            {
                _incomeManage.SetTransaction(_transaction);
            }
            return _incomeManage;
        } 
    }

    public IIncomeRecordsRepository IncomeRecords => _incomeRecords;

    public IBudgetLimitsRepository LimitsRepository { get
        {
            if (_transaction != null)
            {
                _limits.SetTransaction(_transaction);
            }
            return _limits;
        }
    }

    public IExpenseRecordsRepository ExpenseRecords => _expenseRecords;
    public IBudgetPlaningRepository BudgetPlanning => _budgetPlaning;

    public IGetRepository GetRepository => _getRepository;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task RollBackAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task BeginTransaction()
    {
        try
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
            _transaction = await _connection.BeginTransactionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new InvalidOperationException("Database Connection Lost");
        }
    }
}
