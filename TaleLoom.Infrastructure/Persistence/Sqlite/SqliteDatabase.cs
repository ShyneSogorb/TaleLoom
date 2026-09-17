namespace TaleLoom.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;


public sealed class SqliteDatabase
{

    private readonly string _connectionString;

    public SqliteDatabase(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
    
}