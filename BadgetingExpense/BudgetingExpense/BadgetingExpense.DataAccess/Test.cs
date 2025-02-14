using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models;
using Dapper;

namespace BudgetingExpense.DataAccess
{
    public class Test
    {
        private readonly DbConnection _connection;

        public Test(DbConnection connection)
        {
            _connection = connection;
        }

        public void AddUser(User user)
        {
            string query =
                "INSERT INTO [User] (UserName,UserSurname,Email,Password,RegisterDate,Notifications) VALUES(@UserName,@UserSurname,@Email,@Password,@RegisterDate,@Notifications)";
            _connection.Execute(query, user);
        }
    }
}
