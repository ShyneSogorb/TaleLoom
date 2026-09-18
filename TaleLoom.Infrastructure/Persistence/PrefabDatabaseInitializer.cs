using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Infrastructure.Persistence;

public sealed class PrefabDatabaseInitializer
{
    private readonly SqliteDatabase _database;

    public PrefabDatabaseInitializer(SqliteDatabase database)
    {
        _database = database;
    }

    public void Initialize()
    {
        using var connection = _database.CreateConnection();
        
        connection.Open();

        var command = connection.CreateCommand();
        DatabaseSchemaConstructor.CreateTable<Prefab>(command);

        DatabaseSchemaConstructor.CreateTable<FieldDefinition>(command);
        
        command.ExecuteNonQuery();
        
    }
    
    public void Populate()
    {
        PrefabRepository repo = new PrefabRepository(_database);

        List<Prefab> prefabs = new List<Prefab>();
        
        var prefab = Prefab.CreateOrGetPrefab("Character");
        prefabs.Add(prefab);

        prefab.AddField("Name", FieldType.Name);
        prefab.AddField("Age", FieldType.Integer);
        prefab.AddField("Height", FieldType.Float);

        foreach (var p in prefabs)
        {
            if (!repo.ExistsField(p.Name))
            {
                repo.Save(p);
            }
        }

    }
}