
using System.Diagnostics;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public sealed class UrlValue : ValueBase
{
    public override FieldType Type => FieldType.Url;
    public override bool IsValid => Data != null;
    
    public string? Value
    {
        get => (string?)Data;
        set => Data = value;
    }
    
    public UrlValue(object? value = null) : base(value) { }
    
    protected override object? ParseData(object? value)
    {
        return value is null ? null : Convert.ToString(value);
    }
    
    //public UrlValue(object? value = null) : base(value) { }

    public override bool CanConvertTo<T>()
    {
        if(Data == null)
        {
            return false;
        }

        try
        {
            Convert.ChangeType(Data, typeof(T));
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    protected override T GetImpl<T>()
    {
        Debug.Assert(Data != null, nameof(Data) + " != null");
        if (Data is T result)
        {
            return result;
        }

        try
        {
            return (T)Convert.ChangeType(Data, typeof(T));
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}", e);
        }
    }
}
    