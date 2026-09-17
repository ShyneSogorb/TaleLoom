using System.Reflection;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;
public static class ValueFactory
{
    private static readonly Dictionary<FieldType, Func<object?, Value>> _factories = new()
    {
        [FieldType.Integer] = value => new IntegerValue((int?)value),
        [FieldType.Float]   = value => new FloatValue((float?)value),
        [FieldType.Name]    = value => new NameValue((string?)value),
        [FieldType.Text]    = value => new TextValue((string?)value),
        [FieldType.Image]   = value => new ImageValue((string?)value),
        [FieldType.Url]     = value => new UrlValue((string?)value),
    };

    public static Value Create(FieldType type, object? value)
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
