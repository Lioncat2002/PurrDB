using Dapper;
using Microsoft.Data.Sqlite;


namespace GameServer.Config;

public class DB
{
    private static DB _instance;
    public static DB Instance => _instance ??= new DB();
    
    public SqliteConnection Ctx;
    public DB()
    {
        Ctx = new SqliteConnection(Config.CONNECTION_STRING);
        Ctx.Open();
    }

    public void Migrate()
    {
        Console.WriteLine("Migrating database...");
        Ctx.Execute("CREATE TABLE Player (Id INTEGER PRIMARY KEY,X REAL,Y REAL);");
        Console.WriteLine("Database migrated.");
    }
}