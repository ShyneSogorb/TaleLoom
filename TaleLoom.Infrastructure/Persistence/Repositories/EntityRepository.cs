using System.Diagnostics;
using Microsoft.Data.Sqlite;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Values;

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

        command.CommandText = $"SELECT * FROM '{prefab.Id.ToString()}'";

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

    public Entity GetEntityById(Prefab prefab, EntityId id, bool getFields = true)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText =
            $"""
             SELECT name 
             FROM {prefab.Id.ToSqlString()}
             WHERE id = @id;
             """;

        command.Parameters.AddWithValue("@id", id.ToString());

        var reader = command.ExecuteReader();
        reader.Read();

        var entity = Entity.Load(id, prefab, reader.GetString(0));

        if (getFields)
        {
            LoadEntityFields(entity);
        }

        return entity;
    }

    private ValueBase CorrectValue(SqliteDataReader reader, FieldDefinition field)
    {
        var ordinal = field.Position - 1;
        return ValueFactory.Create(field.Type, reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal));
    }

    public void LoadEntityFields(Entity entity)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        var command = connection.CreateCommand();

        command.CommandText =
            $"""
             SELECT {string.Join(",", entity.Parent.Fields.Select(f => $"{f.Id.ToSqlField()} as '{f.Name}'"))}
             FROM {entity.Parent.Id.ToSqlString()}
             WHERE id = @id
             """;

        command.Parameters.AddWithValue("@id", entity.Id.ToString());

        var reader = command.ExecuteReader();
        reader.Read();
        Dictionary<FieldDefinition, ValueBase> values = new Dictionary<FieldDefinition, ValueBase>();

        foreach (var field in entity.Parent.Fields)
        {
            values.Add(field, CorrectValue(reader, field));
        }

        entity.LoadFields(values);

    }

    public bool IsEntityRegistered(Entity entity)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(1) FROM {entity.Parent.Id.ToSqlField()} WHERE id = @id";
        command.Parameters.AddWithValue("@id", entity.Id.ToString());

        var reader = command.ExecuteReader();
        reader.Read();
        return reader.GetBoolean(0);
    }

    public void SaveEntity(Entity entity)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            if (IsEntityRegistered(entity))
            {
                UpdateEntity(entity, connection, transaction);
            }
            else
            {
                InsertEntity(entity, connection, transaction);
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    private static string GetFieldParameterName(FieldId id) => $"@field_{id.Value:N}";
    
    private void InsertEntity(Entity entity, SqliteConnection con, SqliteTransaction trans)
    {
        var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText = 
            $"""
            INSERT INTO {entity.Parent.Id.ToSqlField()} 
            (id, name, {string.Join(",", entity.Fields.Select(f=>f.Id.ToSqlField()))})
            VALUES
            (@id, @name, {string.Join(",", entity.Fields.Select(f=> "@" + SQLUtils.ToSqlName(f.Name)))})
            """;

        command.Parameters.AddWithValue("@id", entity.Id.ToString());
        command.Parameters.AddWithValue("@name", entity.Name);

        foreach (var fieldEntity in entity.FieldsData)
        {
            command.Parameters.AddWithValue(
                "@" + SQLUtils.ToSqlName(fieldEntity.Definition.Name),
                fieldEntity.Value.GetData()
            );
        }
        SqlDebug.GetSqlLine(command);
        
        command.ExecuteNonQuery();
    }

    private void UpdateEntity(Entity entity, SqliteConnection con, SqliteTransaction trans)
    {
        var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText = 
            $"""
             UPDATE {entity.Parent.Id.ToSqlField()} SET
             name = @name, {string.Join(",", 
                 entity.Fields.Select(f=> $"{f.Id.ToSqlField()} = {GetFieldParameterName(f.Id)} "))
             }
             WHERE id = @id
             """;

        command.Parameters.AddWithValue("@id", entity.Id.ToString());
        command.Parameters.AddWithValue("@name", entity.Name);

        foreach (var fieldEntity in entity.FieldsData)
        {
            command.Parameters.AddWithValue(
                GetFieldParameterName(fieldEntity.Definition.Id),
                fieldEntity.Value.GetData()
            );
        }

        SqlDebug.GetSqlLine(command);
        
        command.ExecuteNonQuery();
    }

}