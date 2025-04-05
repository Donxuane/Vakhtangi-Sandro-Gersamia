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
            var assembly = Assembly.GetExecutingAssembly();
            var recourses = $"BudgetingExpense.DataAccess.SqlQueries.{_tableName}.sql";
            using(Stream? stream = assembly.GetManifestResourceStream(recourses))
            {
                if(stream != null)
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        var data = reader.ReadToEnd();
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
