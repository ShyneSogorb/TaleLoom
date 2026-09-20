using Microsoft.Data.Sqlite;

namespace TaleLoom.Infrastructure.Persistence;

public static class SqlDebug
{
    public static string GetSqlLine(SqliteCommand command)
    {
        var sql = command.CommandText;

        foreach (SqliteParameter parameter in command.Parameters)
        {
            var value = parameter.Value?.ToString()?.Replace("'", "''");
            sql = sql.Replace(
                parameter.ParameterName,
                $"'{value}'");
        }

        return sql;
    }
}