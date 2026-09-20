using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;

namespace TaleLoom.Core.Model.Entities;

public class Entity
{
    public class FieldEntity(FieldDefinition definition, ValueBase value)
    {

        public FieldEntity(KeyValuePair<FieldDefinition, ValueBase> fieldValue) : this(fieldValue.Key, fieldValue.Value)
        { }
        public FieldDefinition Definition { get; } = definition;
        public ValueBase Value { get; set; } = value;
    }
    
    public EntityId Id { get; }
    public Prefab Parent { get; }
    
    public string Name { get; private set; }
    
    public string EntityType => Parent.Name;
    
    //Dictionary<FieldDefinition, ValueBase> _fieldValues = new();
    List<FieldEntity> _fieldValues = new();
    public Entity(Prefab parent, Dictionary<FieldDefinition, ValueBase> fieldValues)
    {
        Id = new EntityId(Guid.NewGuid());
        Parent = parent;
        _fieldValues = fieldValues.Select(pair => new FieldEntity(pair.Key, pair.Value)).ToList();
    }
    
    public ValueBase this[FieldDefinition field]
    {
        get => _fieldValues[field.Position - 1].Value;
        set => _fieldValues[field.Position - 1].Value = value;
    }

    public Entity(Prefab prefab, string name)
    {
        Id = new EntityId(Guid.NewGuid());
        Parent = prefab;
        Name = name;
        //Prefab.GetPrefabById(PrefabId).DefaultValues.ToList().ForEach(kv => _fieldValues.Add(kv.Key, kv.Value));
        _fieldValues = prefab.Fields
            .Select(f=> new FieldEntity(f, ValueFactory.Create(f.Type, f.DefaultValue)))
            .ToList();
    }
    

    private Entity(EntityId id, Prefab parent, string name)
    {
        Id = id;
        Parent = parent;
        Name = name;
    }
    
    public IReadOnlyList<FieldDefinition> Fields => _fieldValues.
        Select(entity => entity.Definition).ToList().AsReadOnly();

    public IReadOnlyList<FieldEntity> FieldsData => _fieldValues.AsReadOnly();
    
    public T GetValue<T>(FieldDefinition field)
    {
        var value = _fieldValues[field.Position].Value;
        return value.Get<T>();
    }
    
    public T GetValue<T>(FieldId fieldId)
    {
        var field = _fieldValues.FirstOrDefault(f => f.Definition.Id == fieldId)?.Definition;
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
    
    public void LoadFields( Dictionary<FieldDefinition, ValueBase> values)
    {
        _fieldValues.AddRange(
            values
            .OrderBy(fv => fv.Key.Position)
            .Select(fv => new FieldEntity(fv))
        );
    }

    public void Rename(string name)
    {
        Name = name;
    }
    
}