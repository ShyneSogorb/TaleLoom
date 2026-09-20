
using System.Diagnostics;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public sealed class DoubleValue : ValueBase
{
    public double? Value
    {
        get => (double?)Data;
        set => Data = value;
    }

    public override FieldType Type => FieldType.Float;
    public override bool IsValid => Value != null;

    protected override object? ParseData(object? value)
    {
        return value is null ? null : Convert.ToString(value);
    }

    public DoubleValue(double? data)
    {
        Data = data;
    }
    
    public DoubleValue(object? value = null)
    {
        Value = value switch
        {
            null => null,
            double dec => dec,
            string str when DecimalRegex.IsMatch(str) => double.Parse(str),
            string str when IntegerRegex.IsMatch(str) => (double)int.Parse(str),
            _ => null
        };
    }
    
    
    public override bool CanConvertTo<T>()
    {
        if(Data == null)
        {
            return false;
        }

        try
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
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
    