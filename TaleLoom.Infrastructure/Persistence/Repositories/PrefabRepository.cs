using Microsoft.Data.Sqlite;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Infrastructure.Persistence;

public sealed class PrefabRepository
{
    
    private readonly SqliteDatabase _database;

    public PrefabRepository(SqliteDatabase database)
    {
        _database = database;
    }
    
    public void Save(Prefab prefab)
    {
        using var connection = _database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            if (ExistsPrefab(prefab))
            {
                UpdatePrefab(prefab, connection, transaction);
                UpdateFields(prefab, connection, transaction);
            }
            else
            {
                InsertPrefab(prefab, connection, transaction);
                InsertFields(prefab, connection, transaction);
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

    public bool ExistsPrefab(Prefab prefab)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM prefab WHERE id = @id";
        command.Parameters.AddWithValue("@id", prefab.ID.ToString());

        var reader = command.ExecuteReader();
        reader.Read();
        return reader.GetBoolean(0);
    }
    
    public bool ExistsPrefab(string name)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM prefab WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);

        var reader = command.ExecuteReader();
        reader.Read();
        return reader.GetBoolean(0);
    }
    
    public PrefabID PrefabIdFromName(string name)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT id FROM prefab WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);

        var reader = command.ExecuteReader();
        reader.Read();
        return new PrefabID(Guid.Parse(reader.GetString(0)));
    }

    
    public bool ExistsField(FieldDefinition field)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM field_definition WHERE id = @id";
        command.Parameters.AddWithValue("@id", field.Id.ToString());

        var reader = command.ExecuteReader();
        reader.Read();
        return reader.GetBoolean(0);
    }
    
    private static void UpdatePrefab(Prefab prefab, SqliteConnection con, SqliteTransaction trans)
    {
        using var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText =
            """
            UPDATE prefab
            SET name = @name
            WHERE id = @id
            """;

        command.Parameters.AddWithValue("@id", prefab.ID.ToString());
        command.Parameters.AddWithValue("@name", prefab.Name);

        command.ExecuteNonQuery();
    }

    private void UpdateFields(Prefab prefab, SqliteConnection con, SqliteTransaction trans)
    {
        for (int i = 0; i < 2; ++i)
        {
            foreach (var field in prefab.Fields)
            {
                field.SetOrder(-field.Position);
                Action<Prefab, FieldDefinition, SqliteConnection, SqliteTransaction> execute =
                    ExistsField(field) || i == 0 ? UpdateField : InsertField;

                execute(prefab, field, con, trans);
            }
        }
    }
    
    private static void UpdateField(Prefab prefab, FieldDefinition field, SqliteConnection con, SqliteTransaction trans)
    {
        using var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText =
            """
            UPDATE field_definition
            SET name = @name, position = @position, type = @type, is_required = @is_required, is_active = @is_active
            WHERE id = @id
            """;
        command.Parameters.AddWithValue("@id", field.Id.ToString());
        command.Parameters.AddWithValue("@name", field.Name);
        command.Parameters.AddWithValue("@position", field.Position);
        command.Parameters.AddWithValue("@type", field.Type);
        command.Parameters.AddWithValue("@is_required", field.IsRequired ? 1 : 0);
        command.Parameters.AddWithValue("@is_active", field.IsActive ? 1 : 0);
        
        command.ExecuteNonQuery();
    }

    
    private static void InsertPrefab(Prefab prefab, SqliteConnection con, SqliteTransaction trans)
    {
        using var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText =
            """
            INSERT INTO prefab (id, name)
            VALUES (@id, @name)
            """;

        command.Parameters.AddWithValue("@id", prefab.ID.ToString());
        command.Parameters.AddWithValue("@name", prefab.Name);

        command.ExecuteNonQuery();
    }

    private static void InsertFields(Prefab prefab, SqliteConnection con, SqliteTransaction trans)
    {
        foreach (var field in prefab.Fields)
        {
            InsertField(prefab, field, con, trans);
        }
    }
    
    private static void InsertField(Prefab prefab, FieldDefinition field, SqliteConnection con, SqliteTransaction trans)
    {
        using var command = con.CreateCommand();
        command.Transaction = trans;
        command.CommandText =
            """
            INSERT INTO field_definition( id, prefab_id, name, position, type, is_required, is_active, default_value )
            VALUES ( @id, @prefab_id, @name, @position, @type, @is_required, @is_active, @default_value )
            """;
        command.Parameters.AddWithValue("@id", field.Id.ToString());
        command.Parameters.AddWithValue("@prefab_id", prefab.ID.ToString());
        command.Parameters.AddWithValue("@name", field.Name);
        command.Parameters.AddWithValue("@position", field.Position);
        command.Parameters.AddWithValue("@type", field.Type);
        command.Parameters.AddWithValue("@is_required", field.IsRequired ? 1 : 0);
        command.Parameters.AddWithValue("@is_active", field.IsActive ? 1 : 0);

        var defaultValue = command.CreateParameter();
        defaultValue.ParameterName = "@default_value";
        defaultValue.Value = (object?)field.DefaultValue ?? DBNull.Value;
        defaultValue.IsNullable = true;
        command.Parameters.Add(defaultValue);
        
        command.ExecuteNonQuery();
    }

    public List<Prefab> GetAllPrefabs()
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            """
            SELECT id, name FROM prefab
            """;

        var reader = command.ExecuteReader();

        var prefabs = new List<Prefab>();
        while (reader.Read())
        {
            var idString = reader.GetString(0);
            string name = reader.GetString(1);

            var id = new PrefabID(Guid.Parse(idString));
            
            prefabs.Add(Prefab.Load(id, name));
            
        }

        return prefabs;
    }


    private void LoadPrefabFields(Prefab prefab)
    {
        using var connection = _database.CreateConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
          SELECT id, name, position, type, is_required, is_active, default_value 
          FROM field_definition 
          WHERE prefab_id = @prefab
          ORDER BY position
          """;

        command.Parameters.AddWithValue("@prefab", prefab.ID.ToString());
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var fId = new FieldId(Guid.Parse(reader.GetString(0)));
            var fName = reader.GetString(1); 
            var position = reader.GetInt32(2); 
            var type = (FieldType)reader.GetInt32(3);
            var isRequired = reader.GetInt32(4) != 0;
            var isActive = reader.GetInt32(5) != 0;
            var defaultValue = reader.IsDBNull(6) ? null : reader.GetString(6);
            
            prefab.LoadField(
                fId, fName, position, type, isRequired, isActive, defaultValue
            );
        }
    }

    public Prefab GetPrefabById(PrefabID id, bool getFields = true)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = "SELECT name FROM prefab WHERE id = @id";
        command.Parameters.AddWithValue("@id", id.ToString());
        
        var reader = command.ExecuteReader();

        reader.Read();
        string name = reader.GetString(0);
            
        var prefab = Prefab.Load(id, name);

        if (getFields)
        {
            LoadPrefabFields(prefab);
        }        
        
        return prefab;
    }
    
    
    public Prefab GetPrefabByName(string name, bool getFields = true)
    {
        using var connection = _database.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = "SELECT id FROM prefab WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);
        
        var reader = command.ExecuteReader();

        reader.Read();
        PrefabID id = new PrefabID(Guid.Parse(reader.GetString(0)));
            
        var prefab = Prefab.Load(id, name);

        if (getFields)
        {
            LoadPrefabFields(prefab);
        }        
        
        return prefab;
    }

}