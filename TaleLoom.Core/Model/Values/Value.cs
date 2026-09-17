
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Core.Model.Values;

public abstract class Value
{
    
    public abstract FieldType Type { get; }
    
    public abstract bool IsValid { get; } 
    
    public abstract bool CanConvertTo<T>();
    protected abstract T GetImpl<T>();

    public T Get<T>()
    {
        if (CanConvertTo<T>())
        {
            return GetImpl<T>();
        }
        throw new InvalidOperationException($"Cannot convert value of type {Type} to {typeof(T)}");
    }
    
    
}