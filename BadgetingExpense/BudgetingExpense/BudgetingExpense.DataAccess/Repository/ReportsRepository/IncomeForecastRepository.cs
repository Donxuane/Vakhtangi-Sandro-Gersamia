using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.MainModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository
{
    public class IncomeForecastRepository : IForecastRepository<IncomeRecord>
    {
        private readonly DbConnection _connection;

        public IncomeForecastRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<IncomeRecord>> GetAll(string userId)
        {
            var query = "SELECT Amount,Currency FROM Incomes WHERE UserId = @UserId";
            var report = await _connection.QueryAsync<IncomeRecord>(query ,new {userId});
            return report;
        }
    }
}
