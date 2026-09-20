
using System.Text.RegularExpressions;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public abstract class ValueBase
{
    protected static readonly Regex IntegerRegex = new Regex("^[0-9]+$");
    protected static readonly Regex DecimalRegex = new Regex(@"^[0-9]+[\.,]{0,1}[0-9]+$");
    
    public abstract FieldType Type { get; }
    
    public abstract bool IsValid { get; }

    protected object? Data;

    public ValueBase(object? value = null)
    {
        SetData(value);
    }
        
    public object? GetData() => Data;
    public void SetData(object? valor) => Data = ParseData(valor);

    protected abstract object? ParseData(object? value);
    
    public abstract bool CanConvertTo<T>();
    protected abstract T GetImpl<T>();

    public bool TryGet<T>(out T value)
    {
        if (CanConvertTo<T>())
        {
            value = GetImpl<T>();
            return true;
        }
        value = default!;
        return false;
    }
    
    public T Get<T>()
    {
        if (CanConvertTo<T>())
        {
            return GetImpl<T>();
        }
        throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}");
    }
    
    
}