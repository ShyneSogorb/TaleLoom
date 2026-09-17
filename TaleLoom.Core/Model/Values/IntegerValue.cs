
using System.Diagnostics;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public sealed class IntegerValue(int? data) : Value
{

    public int? Data => data;
    public override FieldType Type => FieldType.Integer;
    public override bool IsValid => data != null;
    
    public override bool CanConvertTo<T>()
    {
        if(Data == null)
        {
            return false;
        }

        try
        {
            Convert.ChangeType(Data.Value, typeof(T));
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
        if (Data.Value is T result)
        {
            return result;
        }

        try
        {
            return (T)Convert.ChangeType(Data.Value, typeof(T));
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}", e);
        }
    }
}
    