using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Tests.Model.Fields;

public class FieldDefinitionTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var _ = Prefab.CreateTransientPrefab("");
        var field = _.GetField(_.AddField("Age", FieldType.Integer, false, true));
        var id = field.Id;
        
        Assert.Equal(id, field.Id);
        Assert.Equal("Age", field.Name);
        Assert.Equal(FieldType.Integer, field.Type);
        Assert.False(field.IsRequired);
        Assert.True(field.IsActive);
    }

    [Fact]
    public void Rename_ShouldChangeName()
    {
        var field = CreateField();

        field.Rename("Character Age");

        Assert.Equal("Character Age", field.Name);
    }

    [Fact]
    public void Rename_ShouldNotChangeIdentity()
    {
        var field = CreateField();
        var id = field.Id;
        
        field.Rename("Character Age");
        Assert.Equal(id, field.Id);
    }
    
    
    [Fact]
    public void Deactivate_ShouldSetFieldInactive()
    {
        var field = CreateField();

        field.Deactivate();

        Assert.False(field.IsActive);
    }

    [Fact]
    public void Restore_ShouldSetFieldActive()
    {
        var field = CreateField();

        field.Deactivate();
        field.Restore();

        Assert.True(field.IsActive);
    }
    
    private static FieldDefinition CreateField()
    {
        return new FieldDefinition(
            Prefab.CreateTransientPrefab(""),
            "Age",
            0,
            FieldType.Integer,
            false, 
            true,
            null
            );
    }
}