namespace TaleLoom.Core.Model.Common;

public readonly record struct PrefabID(Guid Value)
{
    public PrefabID() : this(Guid.NewGuid()){}
    public override string ToString() => Value.ToString();
}
public readonly record struct FieldId(Guid Value)
{
    public FieldId() : this(Guid.NewGuid()){}
    public override string ToString() => Value.ToString();
}
public readonly record struct EntityId(Guid Value)
{
    public EntityId() : this(Guid.NewGuid()){}
    public override string ToString() => Value.ToString();
}