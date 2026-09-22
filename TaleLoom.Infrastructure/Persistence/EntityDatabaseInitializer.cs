using System.Text.RegularExpressions;
using TaleLoom.Core.Model.Entities;
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


    
    public void Initialize(Prefab prefab)
    {
        using var connection = _database.CreateConnection();
        
        connection.Open();

        var command = connection.CreateCommand();

        List<String> instructions = new List<string>();
        instructions.AddRange(new string[]{
            "id TEXT PRIMARY KEY",
            "name TEXT NOT NULL"
        });
        instructions.AddRange(
            prefab.Fields
            .Select(f => $"'{f.Id.ToString()}' {SQLUtils.ToSqlType(f.Type)}")
            .ToList()
        );
        

        command.CommandText = $"CREATE TABLE IF NOT EXISTS '{prefab.Id}' ( {string.Join(",\n", instructions)} )";
        
        command.ExecuteNonQuery();
        
    }

    private static Regex SpaceRemove = new Regex(@"\s");
    public string ToSqlName(string name)
    {
        return SpaceRemove.Replace(name, "_");
    }

    public void Populate(Entity entity)
    {
        using var connection = _database.CreateConnection();
        
        connection.Open();

        var command = connection.CreateCommand();
        
        command.CommandText +=
            $"""
             INSERT OR IGNORE INTO '{entity.Parent.Id.ToString()}' ( id, name, {string.Join(",\n", entity.Parent.Fields.Select(f => $"'{f.Id.ToString()}'"))} )
             VALUES ( @id, @name, {string.Join(",\n", entity.Fields.Select(f => '@' + ToSqlName(f.Name)))});
             """;

        foreach (var field in entity.Fields)
        {
            var param = command.CreateParameter();
            param.ParameterName = '@' + ToSqlName(field.Name);
            param.Value = entity[field]?.GetData() ?? DBNull.Value;
            param.IsNullable = true;
            command.Parameters.Add(param);
        }
        command.Parameters.AddWithValue("@id", entity.Id.ToString());
        command.Parameters.AddWithValue("@name", entity.Name);
                
        command.ExecuteNonQuery();

    }
    
}