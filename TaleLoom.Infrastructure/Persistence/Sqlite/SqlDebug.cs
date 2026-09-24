using Microsoft.Data.Sqlite;

namespace TaleLoom.Infrastructure.Persistence;

public static class SqlDebug
{
    public static string GetSqlLine(SqliteCommand command)
    {
        var sql = command.CommandText;

        foreach (SqliteParameter parameter in command.Parameters)
        {
            bool isText = parameter.SqliteType == SqliteType.Text;
            var value = parameter.Value?.ToString()?.Replace("'", "''");
            if (value?.Length == 0) value = null;
            
            sql = sql.Replace(
                parameter.ParameterName,
                $"{(isText ? "'" : "")}{value ?? "null"}{(isText ? "'" : "")}");
        }

        return sql;
    }
}