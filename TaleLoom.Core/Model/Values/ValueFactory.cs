using System.Reflection;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;
public static class ValueFactory
{
    private static readonly Dictionary<FieldType, Func<object?, ValueBase>> _factories = new()
    {
        [FieldType.Integer] = value => new IntegerValue(value),
        [FieldType.Float]   = value => new DoubleValue(value),
        [FieldType.Name]    = value => new NameValue(value),
        //[FieldType.Text]    = value => new TextValue(value),
        //[FieldType.Image]   = value => new ImageValue(value),
        //[FieldType.Url]     = value => new UrlValue(value),
    };

    public static ValueBase Create(FieldType type, object? value)
    {
        if (!_factories.TryGetValue(type, out var factory))
        {
            throw new ArgumentException(
                $"No Value factory exists for FieldType '{type}'.");
        }

        try
        {
            return factory(value);
        }
        catch (InvalidCastException e)
        {
            throw new ArgumentException(e.ToString());
        }
    }


}
