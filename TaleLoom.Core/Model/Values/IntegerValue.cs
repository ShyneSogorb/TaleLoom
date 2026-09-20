
using System.Diagnostics;
using System.Text.RegularExpressions;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public sealed class IntegerValue : ValueBase
{
    
    public int? Value
    {
        get => (int?)Data;
        set => Data = value;
    }
    protected override object? ParseData(object? value)
    {
        return value is null ? null : Convert.ToString(value);
    }
    public override FieldType Type => FieldType.Integer;
    public override bool IsValid => Value != null;

    public IntegerValue(object? value = null)
    {
        Value = value switch
        {
            null => null,
            int integer => integer,
            long lng => (int)lng,
            string str when IntegerRegex.IsMatch(str) => int.Parse(str),
            string str when DecimalRegex.IsMatch(str) => (int)double.Parse(str),
            _ => null
        };
    }

    public IntegerValue(int? data)
    {
        Data = data;
    }

    public override bool CanConvertTo<T>()
    {
        if(Data == null)
        {
            return false;
        }

        try
        {
            Convert.ChangeType(Value, typeof(T));
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
        if (Value is T result)
        {
            return result;
        }

        try
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}", e);
        }
    }
}
    