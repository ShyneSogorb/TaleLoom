
using System.Diagnostics;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public sealed class ImageValue(string? _data) : ValueBase
{

    public override FieldType Type => FieldType.Image;
    public override bool IsValid => _data != null;

    protected override object? ParseData(object? value)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvertTo<T>()
    {
        if(_data == null)
        {
            return false;
        }

        try
        {
            Convert.ChangeType(_data, typeof(T));
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    //public ImageValue(object? value = null) : base(value) { }

    protected override T GetImpl<T>()
    {
        Debug.Assert(_data != null, nameof(_data) + " != null");
        if (_data is T result)
        {
            return result;
        }

        try
        {
            return (T)Convert.ChangeType(_data, typeof(T));
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}", e);
        }
    }
}
    