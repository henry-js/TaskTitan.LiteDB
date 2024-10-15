using LiteDB;

namespace TaskTitan.Data;

public class LiteDbContext
{
    public readonly LiteDatabase db;
    public LiteDbContext(string connectionString)
    {
        try
        {
            var db = new LiteDatabase(connectionString);
            if (db != null)
                this.db = db;
        }
        catch (Exception ex)
        {
            throw new Exception("Can find or create LiteDb database.", ex);
        }
    }
}
