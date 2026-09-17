using Microsoft.Data.Sqlite;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Infrastructure.Persistence;

public sealed class EntityRepository
{
    
    private readonly SqliteDatabase _database;

    public EntityRepository(SqliteDatabase database)
    {
        _database = database;
    }

    public List<Entity> GetAllEntities(Prefab prefab)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText = $"SELECT * FROM {prefab.ID.ToString()}";

        var reader = command.ExecuteReader();
        List<Entity> entities = new List<Entity>();

        while (reader.Read())
        {
            var id = new EntityId(Guid.Parse(reader.GetString(0)));
            
            entities.Add(Entity.Load(
                id,
                prefab,
                reader.GetString(1)
                ));
        }

        return entities;
    }

}