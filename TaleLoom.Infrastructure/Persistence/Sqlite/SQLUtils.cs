using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.Infrastructure.Persistence;

public static class SQLUtils
{
    public static string ToSqlName(string name)
    {
        return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2")
            .Replace(' ', '_')
            .ToLower();
    }
    
    public static string ToSqlTypeName(FieldType fieldType)
    {
        if (fieldType == FieldType.Integer) return "INTEGER";
        if (fieldType == FieldType.Float) return "REAL";
        return "TEXT";
    }
    
    public static SqliteType ToSqlType(FieldType fieldType)
    {
        if (fieldType == FieldType.Integer) return SqliteType.Integer;
        if (fieldType == FieldType.Float) return SqliteType.Real;
        return SqliteType.Text;
    }
}