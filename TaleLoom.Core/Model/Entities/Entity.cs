using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;

namespace TaleLoom.Core.Model.Entities;

public class Entity
{
    public EntityId Id { get; }
    public Prefab Parent { get; }
    
    public string Name { get; }
    
    public string EntityType => Parent.Name;
    
    Dictionary<FieldDefinition, Value> _fieldValues = new();
    public Entity(Prefab parent, Dictionary<FieldDefinition, Value> fieldValues)
    {
        Id = new EntityId(Guid.NewGuid());
        Parent = parent;
        _fieldValues = fieldValues;
    }

    public Entity(Prefab prefab)
    {
        Id = new EntityId(Guid.NewGuid());
        Parent = prefab;
        //Prefab.GetPrefabById(PrefabId).DefaultValues.ToList().ForEach(kv => _fieldValues.Add(kv.Key, kv.Value));
    }

    private Entity(EntityId id, Prefab parent, string name)
    {
        Id = id;
        Parent = parent;
        Name = name;
    }
    
    public IReadOnlyList<FieldDefinition> Fields => _fieldValues.Keys.ToList().AsReadOnly();
    
    public T GetValue<T>(FieldDefinition field)
    {
        if (!_fieldValues.TryGetValue(field, out var value))
        {
            throw new ArgumentException($"Field '{field.Name}' does not exist in the entity.");
        }
        
        return value.Get<T>();
    }
    
    public T GetValue<T>(FieldId fieldId)
    {
        var field = _fieldValues.Keys.FirstOrDefault(f => f.Id == fieldId);
        if (field == null)
        {
            throw new ArgumentException($"Field with ID '{fieldId}' does not exist in the entity.");
        }
        
        return GetValue<T>(field);
    }
    
    public static Entity Load(EntityId id, Prefab parent, string name)
    {
        return new Entity(id, parent, name);
    }

    
}