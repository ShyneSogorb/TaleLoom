using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;

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
    
    public List<Prefab> Populate()
    {
        PrefabRepository repo = new PrefabRepository(_database);

        List<Prefab> prefabs = new List<Prefab>();
        
        var prefab = Prefab.CreateOrGetPrefab("Character");
        prefabs.Add(prefab);

        prefab.AddField("Surname", FieldType.Name, true, true, new NameValue("Ythia"));
        prefab.AddField("Age", FieldType.Integer, true, true, new IntegerValue(28));
        prefab.AddField("Height", FieldType.Float, true, true, new DoubleValue(1.7));

        for (int i = 0; i < prefabs.Count; i++)
        {
            var p = prefabs[i];
            bool exists = repo.ExistsPrefab(p.Name);
            if (!exists)
            {
                repo.Save(p);
            }
            else
            {
                prefabs[i] = repo.GetPrefabByName(p.Name);
            }
        }

        return prefabs;

    }
}