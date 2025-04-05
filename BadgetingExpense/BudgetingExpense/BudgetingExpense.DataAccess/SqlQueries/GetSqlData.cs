using System.Reflection;

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
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SqlQueries", _tableName+".sql");
            var data = File.ReadAllText(path);
            if (data != null)
                return data;
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
