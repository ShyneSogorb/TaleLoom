namespace TaleLoom.Core.Model.Common;

[AttributeUsage(AttributeTargets.Property)]
public class SerializedAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : SerializedAttribute { }

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : SerializedAttribute { }

[AttributeUsage(AttributeTargets.Property)]
public class UniqueAttribute : SerializedAttribute
{
    public UniqueAttribute(string[] combination)
    {
        Combination = combination;
    }
    
    public UniqueAttribute() : this(new string[]{}) { }

    public readonly string[] Combination;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class CustomNameAttribute : SerializedAttribute
{
    
    public CustomNameAttribute(string name)
    {
        Name = name;
    }

public readonly string Name;
}
