using System.Text.RegularExpressions;

namespace TaleLoom.Infrastructure.Persistence;

public static class SQLUtils
{
    public static string ToSqlName(string name)
    {
        return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2")
            .ToLower();
    }
}