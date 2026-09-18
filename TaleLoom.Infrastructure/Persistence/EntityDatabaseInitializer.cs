using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Infrastructure.Persistence;

public sealed class EntityDatabaseInitializer
{
    private readonly SqliteDatabase _database;

    public EntityDatabaseInitializer(SqliteDatabase database)
    {
        _database = database;
    }

    public void InitializeMany(List<Prefab> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            Initialize(prefab);
        }
        
    }

    private string ToSqlType(FieldType fieldType)
    {
        if (fieldType == FieldType.Integer) return "INTEGER";
        if (fieldType == FieldType.Float) return "REAL";
        return "TEXT";
    }
    
    public void Initialize(Prefab prefab)
    {
        using var connection = _database.CreateConnection();
        
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            $"""
            CREATE TABLE IF NOT EXIST '{prefab.ID}'
            """;

        List<String> instructions = prefab.Fields
            .Select(f => $"'{}'");

        command.ExecuteNonQuery();
        
    }
}