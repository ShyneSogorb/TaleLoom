using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using TaleLoom.Core.Model.Common;
using Guid = System.Guid;

namespace TaleLoom.Infrastructure.Persistence;

public static class DatabaseSchemaConstructor
{
    
    public static string ToSqlName(string name)
    {
        return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2")
            .ToLower();
    }
    
    public static string PropertyName(PropertyInfo propertyInfo)
    {
        return ToSqlName(propertyInfo.GetCustomAttribute<CustomNameAttribute>()?.Name ?? propertyInfo.Name);
    }
    
    public static PropertyInfo PkForReference(PropertyInfo Refence)
    {
        return Refence.PropertyType.GetProperties().First(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null);
    }
    
    public static string GetSqlType(PropertyInfo property)
    {
        if (property.PropertyType == typeof(int))       return "INTEGER";
        if (property.PropertyType == typeof(string))    return "TEXT";
        if (property.PropertyType == typeof(bool))      return "INTEGER";
        
        if (property.PropertyType.GetProperty("Value")?.PropertyType == typeof(Guid)) return "TEXT";
        
        if (property.PropertyType.IsClass)  return GetSqlType(PkForReference(property));
        if (property.PropertyType.IsEnum)   return "INTEGER";

        throw new Exception($"Invalid property type {property.Name} {property.PropertyType}");
    }
    
    static bool AllowsNull(PropertyInfo property)
    {
        var context = new NullabilityInfoContext();
        var nullability = context.Create(property);

        return nullability.ReadState == NullabilityState.Nullable;
    }
    
    public static void CreateTable<T>(SqliteCommand command)
    {
        Type type = typeof(T);
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttributes<SerializedAttribute>().Any())
            .ToList();
        
        var instructions = new List<string>();

        bool IsPk(PropertyInfo property) => property.GetCustomAttribute<PrimaryKeyAttribute>() != null;
        bool IsNn(PropertyInfo property) => !AllowsNull(property);
        
        instructions.AddRange(properties
            .Select(p => $"{PropertyName(p)} {GetSqlType(p)}" +
                         $"{(IsPk(p) ? " PRIMARY KEY" : "")}" +
                         $"{(IsNn(p) ? " NOT NULL" : "")}")
        );

        foreach (var property in properties.Where(p=>p.GetCustomAttribute<ForeignKeyAttribute>() != null) )
        {
            var fk = property.GetCustomAttribute<ForeignKeyAttribute>();

            if (fk == null)
            {
                throw new Exception();
            }

            
            PropertyInfo refenceId = PkForReference(property);
            
            instructions.Add($"FOREIGN KEY ({PropertyName(property)}) REFERENCES {ToSqlName(property.PropertyType.Name)}({PropertyName(refenceId)})");
        }
        
        foreach (var property in properties.Where(p=>p.GetCustomAttribute<UniqueAttribute>() != null) )
        {
            var uq = property.GetCustomAttribute<UniqueAttribute>();

            if (uq == null)
            {
                throw new Exception();
            }

            //var combination = new List<string>(uq.Combination);

            if (uq.Combination.Any(
                    c => properties.FirstOrDefault(
                       t => ToSqlName(PropertyName(t)) == ToSqlName(c)) == null
                ))
            {
                throw new Exception($"Variable {property.Name} of class {type.Name} has combined unique with properties which do not exists");
            }
            
            var combination = uq.Combination.Select(name => PropertyName(properties.First(p => PropertyName(p) == ToSqlName(name)))).ToList();
            combination.Add(PropertyName(property));
            
            instructions.Add($"UNIQUE ({string.Join(", ", combination)})");
        }

        command.CommandText += $"CREATE TABLE IF NOT EXISTS {ToSqlName(type.Name)} (\n" + string.Join(",\n\t", instructions) + "\n);\n";
    }

    public static void InsertToTable<T>(ref SqliteCommand command, T instance)
    {
        Type type = typeof(T);
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttributes<SerializedAttribute>().Any())
            .ToList();

        command.CommandText +=
            $"""
             INSERT INTO {ToSqlName(type.Name)} ( {string.Join(',', properties.Select(PropertyName))} )
             VALUES ({string.Join(',', properties.Select(p => '@' + PropertyName(p)))});\n\n
             """;

        foreach (var prop in properties)
        {
            command.Parameters.AddWithValue(
                '@'+PropertyName(prop),
                prop.GetValue(instance)
            );
        }
    }
    
}