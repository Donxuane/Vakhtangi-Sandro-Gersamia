using System.Reflection;
using Microsoft.Extensions.Logging;

namespace BudgetingExpense.DataAccess.SqlQueries;

public class GetSqlData
{
    private readonly string _tableName;
  
    public GetSqlData(string tableName)
    {
        _tableName = tableName;
       
    }
    public string? GetData()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SqlQueries", _tableName+".sql");
        var data = File.ReadAllText(path);
        if (data != null)
            return data;
        return null;
    }
}
