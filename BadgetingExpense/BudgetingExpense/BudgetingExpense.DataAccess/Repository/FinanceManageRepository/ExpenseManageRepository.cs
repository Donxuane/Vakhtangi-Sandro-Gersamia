﻿using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Models;
using Dapper;
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
            Date = model.Date = DateTime.Now,
            model.UserId
            
        }, _transaction);
    }

    public async Task DeleteAsync(int Id)
    {
        var query = "DELETE FROM Expenses WHERE Id = @Id";
        await _connection.ExecuteAsync(query, new { Id }, _transaction);
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(string UserId)
    {
        var query = "SELECT FROM Expenses WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<Expense>(query, new { UserId }, _transaction);
        return collection;
    }

    public Task<int> AddCategoryAsync(Category category)
    {
        if (category.Name == null)
        {
            category.Name = "etc";
        }
        var query = "INSERT INTO Categories(Name, Type) OUTPUT INSERTED.Id VALUES(@Name,@Type)";
        var id = _connection.QuerySingleAsync<int>(query, new { category.Name, Type = category.Type = 0 }, _transaction);
        
        return id;
    }

    public Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
    {
        var query = @"SELECT DISTINCT c.Id,c.Name,c.Type                       
                      FROM Categories c
                      JOIN Expenses ex ON c.Id = ex.CategoryId WHERE ex.UserId = @userId";
        var expenseCategories = _connection.QueryAsync<Category>(query, new { userId }, _transaction);
        return expenseCategories;
    }



    public async Task UpdateCategoryAsync(Category category)
    {
        var query = "UPDATE Categories SET Name = @Name ,Type = @Type WHERE Id = @Id ";
        await _connection.ExecuteAsync(query, new { category.Name, Type = category.Type = 0, Id = category.Id }, _transaction);

    }

    public async Task UpdateAsync(Expense model)
    {
        var query = "Update Expenses SET Currency = @Currency,Amount =@Amount,Date=@Date WHERE UserId = @Id AND Id = @ExpenseId";
        await _connection.ExecuteAsync(query, new { model.Currency, model.Amount, model.Date, UserId = model.UserId,model.Id },
            _transaction);
    }
}
