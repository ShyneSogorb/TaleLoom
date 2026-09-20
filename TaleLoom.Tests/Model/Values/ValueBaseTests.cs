
using TaleLoom.Core.Model.Common;
//using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Values;

namespace TaleLoom.Tests.Model.Values;

public class ValueBaseTests
{
    [Fact]
    public void FromInt_ShouldCreateIntValue()
    {
        var value = new IntegerValue(42);

        Assert.Equal(FieldType.Integer, value.Type);
        Assert.Equal(42, value.Value);
    }

    [Fact]
    public void FromFloat_ShouldCreateFloatValue()
    {
        var value = new DoubleValue(3.14f);

        Assert.Equal(FieldType.Float, value.Type);
        Assert.Equal(3.14f, value.Value);
    }

    [Theory]
    [InlineData(FieldType.Name)]
    [InlineData(FieldType.Text)]
    [InlineData(FieldType.Image)]
    [InlineData(FieldType.Url)]
    public void FromString_ShouldCreateStringValue(FieldType type)
    {
        const string data = "Test value";

        var value = ValueFactory.Create(type, data);

        Assert.Equal(type, value.Type);
        Assert.Equal(data, value.Get<string>());
    }

    [Theory]
    [InlineData(FieldType.Integer)]
    [InlineData(FieldType.Float)]
    public void FromString_ShouldRejectNonStringTypes(FieldType type)
    {
        Assert.Throws<ArgumentException>(() =>
            ValueFactory.Create(type, "Invalid"));
    }

    [Fact]
    public void Get_ShouldReturnStoredValue()
    {
        var value = new IntegerValue(123);

        var result = value.Get<int>();

        Assert.Equal(123, result);
    }

    [Fact]
    public void Get_ShouldThrowWhenRequestedTypeIsWrong()
    {
        var value = new NameValue("Some text");

        Assert.Throws<InvalidOperationException>(() =>
            value.Get<int>());
    }
}