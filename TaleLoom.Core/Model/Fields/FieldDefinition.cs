
using System.Diagnostics;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Core.Model.Fields;

public sealed class FieldDefinition
{
    [PrimaryKey]
    public FieldId Id { get; }
    
    [CustomName("PrefabId")]
    [ForeignKey]
    public Prefab Owner { get; }
    
    [Unique(["PrefabId"])]
    public string Name { get; private set; }
    
    [Unique(["PrefabId"])]
    public int Position { get; private set; }
    
    [Serialized]
    public FieldType Type { get; private set; }
    
    [Serialized]
    public bool IsRequired { get; private set; }
    
    [Serialized]
    public bool IsActive { get; private set; }

    [Serialized]
    public string? DefaultValue { get; private set; }

    private FieldDefinition(FieldId id, Prefab owner, string name, int position, FieldType type, bool isRequired, bool isActive, string? defaultValue)
    {
        Id = id;
        Owner = owner;
        
        Name = name;
        Debug.Assert(position>0);
        Position = position;
        Type = type;
        IsRequired = isRequired;
        IsActive = isActive;
        DefaultValue = defaultValue;
    }
    
    public FieldDefinition(
        Prefab owner, string name, int position, FieldType type, bool isRequired, bool isActive, string? defaultValue
        ) : this(new FieldId(Guid.NewGuid()), owner, name, position, type, isRequired, isActive, defaultValue)
    { }
 
    public void Rename(string newName)
    {
        Name = newName;
    }

    public void SetOrder(int order)
    {
        Debug.Assert(order!=0);
        Position = order;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Restore()
    {
        IsActive = true;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void ChangeType(FieldType newType)
    {
        Type = newType;
    }

    public static FieldDefinition Load(FieldId id, Prefab owner, string name, int position, FieldType type, bool isRequired, bool isActive, string? defaultValue)
    {
        return new FieldDefinition(id, owner, name, position, type, isRequired, isActive, defaultValue);
    }
}