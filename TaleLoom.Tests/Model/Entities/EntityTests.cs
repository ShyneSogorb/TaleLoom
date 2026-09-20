using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;

namespace TaleLoom.Tests.Model.Entities;

public class EntityTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var prefab = CreatePrefab(out var name, out var age, out var height, false);
        var entity = CreateEntity(prefab);

        Assert.Equal("Character", entity.EntityType);
        Assert.Equal("John Doe", entity.GetValue<string>(name));
        Assert.Equal(43, entity.GetValue<int>(age));
        Assert.Equal(1.83f, entity.GetValue<float>(height));
        Assert.NotEmpty(entity.Fields);
        Prefab.DeletePrefab(prefab);
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueEntityIds()
    {
        var prefab = CreatePrefab(out _, out _, out _);

        var entity1 = CreateEntity(prefab);
        var entity2 = CreateEntity(prefab);

        Assert.NotEqual(entity1.Id, entity2.Id);
    }

    [Fact]
    public void Constructor_ShouldUseSamePrefab()
    {
        var prefab = CreatePrefab(out _, out _, out _);

        var entity = CreateEntity(prefab);

        Assert.Equal(prefab.ID, entity.Parent.ID);
    }

    [Fact]
    public void GetValue_ShouldReturnCorrectStringValue()
    {
        var prefab = CreatePrefab(out var name, out _, out _);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<string>(name);

        Assert.Equal("John Doe", result);
    }

    [Fact]
    public void GetValue_ShouldReturnCorrectIntegerValue()
    {
        var prefab = CreatePrefab(out _, out var age, out _);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<int>(age);

        Assert.Equal(43, result);
    }

    [Fact]
    public void GetValue_ShouldReturnCorrectFloatValue()
    {
        var prefab = CreatePrefab(out _, out _, out var height);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<float>(height);

        Assert.Equal(1.83f, result);
    }
    
    [Fact]
    public void GetValue_WithUnknownFieldId_ShouldThrow()
    {
        var prefab = CreatePrefab(out _, out _, out _);
        var entity = CreateEntity(prefab);

        var unknownFieldId = new FieldId(Guid.NewGuid());

        Assert.Throws<ArgumentException>(() =>
            entity.GetValue<int>(unknownFieldId));
    }

    [Fact]
    public void GetValue_WithFieldFromAnotherPrefab_ShouldThrow()
    {
        var prefab1 = CreatePrefab(out var age, out _, out _);
        var prefab2 = CreatePrefab(out _, out _, out _);

        var entity = CreateEntity(prefab2);

        Assert.Throws<ArgumentException>(() =>
            entity.GetValue<int>(age));
    }

    [Fact]
    public void Fields_ShouldContainAllPrefabFields()
    {
        var prefab = CreatePrefab(out _, out _, out _);
        var entity = CreateEntity(prefab);

        Assert.Equal(3, entity.Fields.Count);
    }
    
    [Fact]
    public void GetIntegerValueAsFloat_ShouldConvert()
    {
        var prefab = CreatePrefab(out _, out var age, out _);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<float>(age);

        Assert.Equal(43f, result);
    }

    [Fact]
    public void GetIntegerValueAsString_ShouldConvert()
    {
        var prefab = CreatePrefab(out _, out var age, out _);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<string>(age);

        Assert.Equal("43", result);
    }

    [Fact]
    public void GetFloatValueAsInteger_ShouldConvert()
    {
        var prefab = CreatePrefab(out _, out _, out var height);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<int>(height);

        Assert.Equal(2, result);
    }

    [Fact]
    public void GetFloatValueAsString_ShouldConvert()
    {
        var prefab = CreatePrefab(out _, out _, out var height);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<string>(height);

        Assert.Equal("1,83", result);
    }

    [Fact]
    public void GetStringValueAsInteger_ShouldThrow()
    {
        var prefab = CreatePrefab(out var name, out _, out _);
        var entity = CreateEntity(prefab);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            entity.GetValue<int>(name));

        Assert.Contains("Name", exception.Message);
        Assert.Contains("Int32", exception.Message);
    }

    [Fact]
    public void GetStringValueAsFloat_ShouldThrow()
    {
        var prefab = CreatePrefab(out var name, out _, out _);
        var entity = CreateEntity(prefab);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            entity.GetValue<float>(name));

        Assert.Contains("Name", exception.Message);
        Assert.Contains("Single", exception.Message);
    }
    
    [Fact]
    public void GetValue_WithMatchingType_ShouldReturnOriginalValue()
    {
        var prefab = CreatePrefab(out var name, out _, out _);
        var entity = CreateEntity(prefab);

        var result = entity.GetValue<string>(name);

        Assert.Equal("John Doe", result);
    }
    
    [Fact]
    public void GetValue_WithInvalidConversion_ShouldPreserveInnerException()
    {
        var prefab = CreatePrefab(out var name, out _, out _);
        var entity = CreateEntity(prefab);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            entity.GetValue<int>(name));

        Assert.Equal(
            "Cannot convert value of type Name to System.Int32",
            exception.Message);
    }

    private static Prefab CreatePrefab(
        out FieldId name,
        out FieldId age,
        out FieldId height, bool transient = true)
    {
        var prefab = transient ? Prefab.CreateTransientPrefab("Character") : Prefab.CreateOrGetPrefab("Character");

        name = prefab.AddField(
            "Name",
            FieldType.Name);

        age = prefab.AddField(
            "Age",
            FieldType.Integer);

        height = prefab.AddField(
            "Height",
            FieldType.Float);

        return prefab;
    }

    private static Entity CreateEntity(Prefab prefab)
    {
        return new Entity(prefab, "foo");
    }
}