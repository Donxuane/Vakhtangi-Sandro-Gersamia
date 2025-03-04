using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IBudgetPlaningRepository;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.BudgetPlaningRepository
{
    public class BudgetPlaningRepository : IBudgetPlaningRepository
    {
        private readonly DbConnection _connection;
      


        public BudgetPlaningRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<BudgetPlanning>> GetAll(string UserId, int CategoryId)
        {
            var query = "SELECT * FROM BudgetPlaning WHERE UserId = @UserId AND CategoryId = @CategoryId";

            return await _connection.QueryAsync<BudgetPlanning>(query, new {UserId,CategoryId});

        }

        public async Task<string> GetEmail(string UserId)
        {
            var query = "SELECT Email FROM AspNetUsers Where Id =@UserId";
            return await _connection.QuerySingleAsync<string>(query, new {UserId});

        }
    }
}
