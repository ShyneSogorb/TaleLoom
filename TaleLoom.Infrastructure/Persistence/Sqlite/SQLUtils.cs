using System.Text.RegularExpressions;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Infrastructure.Persistence;

public static class SQLUtils
{
    public static string ToSqlName(string name)
    {
        return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2")
            .ToLower();
    }
    
    public static string ToSqlType(FieldType fieldType)
    {
        if (fieldType == FieldType.Integer) return "INTEGER";
        if (fieldType == FieldType.Float) return "REAL";
        return "TEXT";
    }
}