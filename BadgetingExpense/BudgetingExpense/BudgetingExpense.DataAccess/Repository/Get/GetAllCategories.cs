using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Models.MainModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.Get
{
    public class GetAllCategories : IGetAllCategory
    {
        private readonly DbConnection _connection;

        public GetAllCategories(DbConnection connection)
        {
            _connection = connection;
        }
        public  int GetAllCategoryAsync(int categoryId)
        {

            var query = "Select Type FROM Categories WHERE Id = @CategoryId";
           return  _connection.QuerySingle<int>(query, new { categoryId });
             
        }
    }
}
