﻿using BudgetingExpense.Domain.Contracts.IRepository.IFinance;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.MainModels;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.FinanceManageRepository;

public class ExpenseManageRepository : IManageFinancesRepository<Expense>
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public ExpenseManageRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public void SetTransaction(DbTransaction transaction)
    {
        _transaction ??= transaction;
    }

    public async Task AddAsync(Expense model)
    {
        var query = "INSERT INTO Expenses(Currency,Amount,CategoryId,[Date],UserId)" +
            "Values(@Currency,@Amount,@CategoryId,@Date,@UserId)";
        await _connection.ExecuteAsync(query, new
        {
            model.Currency,
            model.Amount,
            model.CategoryId,
            model.Date,
            model.UserId
        }, _transaction);
    }

    public async Task DeleteAsync(int Id, string userId)
    {
        var query = "DELETE FROM Expenses WHERE Id = @Id AND UserId = @userId";
        await _connection.ExecuteAsync(query, new { Id, userId }, _transaction);
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(string UserId)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<Expense>(query, new { UserId }, _transaction);
        return collection;
    }

    public Task<int> AddCategoryAsync(Category category)
    {
        var query = "INSERT INTO Categories(Name, Type) OUTPUT INSERTED.Id VALUES(@Name,@Type)";
        var id = _connection.QuerySingleAsync<int>(query, new { category.Name, Type = FinancialTypes.Expense }, _transaction);
        return id;
    }

    public Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
    {
        var query = @"SELECT * FROM Categories WHERE Type = @Type";
        var expenseCategories = _connection.QueryAsync<Category>(query, new { Type = FinancialTypes.Expense }, _transaction);
        return expenseCategories;
    }



    public async Task UpdateCategoryAsync(Category category)
    {
        var query = "UPDATE Categories SET Name = @Name ,Type = @Type WHERE Id = @Id ";
        await _connection.ExecuteAsync(query, new { category.Name, Type = 0, category.Id }, _transaction);

    }

    public async Task UpdateAsync(Update model)
    {
        await _connection.ExecuteAsync("UpdateProcedure", new { TableName = "Expenses",model.Currency,
            model.Amount,model.CategoryId, model.Date, Id = model.Id, UserId = model.UserId},
            _transaction,commandType: System.Data.CommandType.StoredProcedure);
    }
}
