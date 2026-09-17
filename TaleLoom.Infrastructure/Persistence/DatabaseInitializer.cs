using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Infrastructure.Persistence;

public sealed class DatabaseInitializer
{
    private readonly SqliteDatabase _database;

    public DatabaseInitializer(SqliteDatabase database)
    {
        _database = database;
    }

    public void Initialize()
    {
        using var connection = _database.CreateConnection();
        
        connection.Open();

        var command = connection.CreateCommand();
        DatabaseSchemaConstructor.CreateTable<Prefab>(ref command);

        DatabaseSchemaConstructor.CreateTable<FieldDefinition>(ref command);
        
        command.ExecuteNonQuery();
        
    }
}