using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.Tests.Model.Prefabs;

public class PrefabTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var prefab = Prefab.CreateTransientPrefab("Character");
        var id = prefab.Id;

        Assert.Equal(id, prefab.Id);
        Assert.Equal("Character", prefab.Name);
        Assert.Empty(prefab.Fields);
    }
    
    [Fact]
    public void Rename_ShouldChangeName()
    {
        var prefab = CreatePrefab();

        prefab.Rename("NPC");

        Assert.Equal("NPC", prefab.Name);
    }
    
    [Fact]
    public void Rename_ShouldNotChangeIdentity()
    {
        var prefab = CreatePrefab();
        var id = prefab.Id;
        
        prefab.Rename("NPC");
        Assert.Equal(id, prefab.Id);
    }
    
    [Fact]
    public void AddField_ShouldAddFieldToPrefab()
    {
        var prefab = CreatePrefab();
        var field = CreateField(prefab, "Age", FieldType.Integer);

        Assert.Single(prefab.Fields);
        Assert.Equal(field, prefab.Fields[0]);
    }
    
    [Fact]
    public void AddField_ShouldPreserveInsertionOrder()
    {
        var prefab = CreatePrefab();
        var name = CreateField(prefab, "Age", FieldType.Integer);
        var age = CreateField(prefab, "Name", FieldType.Text);
        var bio = CreateField(prefab, "Biography", FieldType.Text);

        Assert.Equal(
            [name, age, bio],
            prefab.Fields
        );
    }
    
    private static Prefab CreatePrefab()
    {
        return Prefab.CreateTransientPrefab("Character");
    }

    private static FieldDefinition CreateField(
        Prefab prefab,
        string name,
        FieldType type)
    {
        var newElement = prefab.AddField(name, type);
        return prefab.Fields.First(f => f.Id == newElement);
    }
    
}