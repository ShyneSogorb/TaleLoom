
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Values;

namespace TaleLoom.Core.Model.Prefabs;

public sealed class Prefab
{

    // static List<Prefab> _prefabs = new();
    //
    // public static Prefab GetPrefabById(PrefabID id)
    // {
    //     var result = _prefabs.FirstOrDefault(p => p.ID == id);
    //     if (result == null)
    //     {
    //         throw new ArgumentException($"Prefab with ID '{id}' does not exist.");
    //     }
    //
    //     return result;
    // }

    private List<FieldDefinition> _fields = [];

    [PrimaryKey] public PrefabID ID { get; private set; }

    [Unique] public string Name { get; private set; }

    public IReadOnlyList<FieldDefinition> Fields => _fields.AsReadOnly();
    private Prefab(string name, bool transient)
    {
        ID = new PrefabID();
        Name = name;
        if (!transient)
        {
            //_prefabs.Add(this);
        }
    }

    private Prefab(PrefabID id, string name)
    {
        ID = id;
        Name = name;
    }

    public static Prefab Load(PrefabID id, string name)
    {
        return new Prefab(id, name);
    }

    public FieldDefinition LoadField(FieldId id, string name, int position, FieldType type, bool isRequired, bool isActive, string? defaultValue)
    {
        var newField = FieldDefinition.Load(id, this, name, position, type, isRequired, isActive, defaultValue);
        AddField(newField);
        _fields = _fields.OrderBy(f => f.Position).ToList();
        return newField;
    }

    public static void DeletePrefab(Prefab prefab)
    {
        //_prefabs.Remove(prefab);
    }
    
    public static Prefab CreateTransientPrefab(string name)
    {
        return new Prefab(name, true);
    }

    public void FixId(PrefabID id)
    {
        ID = id;
    }
    
    public static Prefab CreateOrGetPrefab(string name)
    {
        // var existingPrefab = _prefabs.FirstOrDefault(p => p.Name == name);
        // if (existingPrefab != null)
        // {
        //     return existingPrefab;
        // }
        return new Prefab(name, false);
    }
    
    public void Rename(string newName)
    {
        Name = newName;
    }
    
    public void AddField(FieldDefinition field)
    {
        //field.SetOrder(_fields.Count+1);
        _fields.Add(field);
    }
    
    public FieldId AddField(string name, FieldType type, bool isRequired, bool isActive, ValueBase defaultValueBase)
    {
        
        var newElement = new FieldDefinition(this, name, _fields.Count+1, type, isRequired, isActive, 
            defaultValueBase.IsValid ? defaultValueBase.Get<string>() : null);
        
        _fields.Add(newElement);
        return newElement.Id;
    }

    public FieldId AddField(string name, FieldType type, bool isRequired = true,
        bool isActive = true)
    {
        return AddField(name, type, isRequired, isActive, ValueFactory.Create(type, null));
    }
    
    public FieldDefinition GetField(FieldId fieldId)
    {
        return _fields.First(f => f.Id == fieldId);
    }
    
    public void DisableField(FieldId fieldId)
    {
        FieldDefinition? field = _fields.FirstOrDefault(f => f.Id == fieldId);
        if (field == null)
            return;
        
        field.Deactivate();
    }
    
    public void EnableField(FieldId fieldId)
    {
        FieldDefinition? field = _fields.FirstOrDefault(f => f.Id == fieldId);
        if (field == null)
            return;
        
        field.Restore();
    }
    
    public void RemoveField(FieldId fieldId)
    {
        FieldDefinition? field = _fields.FirstOrDefault(f => f.Id == fieldId);
        if (field == null)
            return;
        
        _fields.Remove(field);
    }

    
    public void RenameField(FieldId fieldId, string newName)
    {
        FieldDefinition? field = _fields.FirstOrDefault(f => f.Id == fieldId);
        if (field == null)
        {
            return;
        }

        field.Rename(newName);
    }
    
}