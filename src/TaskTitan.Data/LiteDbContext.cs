using System.Reflection.Metadata;
using LiteDB;
using TaskTitan.Configuration;

namespace TaskTitan.Data;

public class LiteDbContext
{
    public const string FILE_NAME = "tasktitan.db";
    public readonly LiteDatabase db;
    public LiteDbContext(string connectionString)
    {
        connectionString = $@"Filename={Global.DataDirectoryPath}\tasktitan.db";
        try
        {
            var db = new LiteDatabase(connectionString);
            if (db != null)
                this.db = db;
        }
        catch (Exception ex)
        {
            throw new Exception("Can't find or create LiteDb database.", ex);
        }
    }

    public static string CreateConnectionStringFrom(string dataDirectoryPath)
    {
        return $@"Filename={Path.Combine(dataDirectoryPath, FILE_NAME)}";
    }
}
