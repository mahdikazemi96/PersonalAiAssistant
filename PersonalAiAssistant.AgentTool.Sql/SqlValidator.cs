namespace PersonalAiAssistant.AgentTool.Sql
{
    public class SqlValidator
    {
        private static readonly string[] ForbiddenKeywords =
        {
        "INSERT",
        "UPDATE",
        "DELETE",
        "DROP",
        "ALTER",
        "TRUNCATE",
        "CREATE",
        "EXEC",
        "MERGE"
    };

        public bool IsSafe(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return false;

            var normalized = sql.ToUpperInvariant();

            if (!normalized.TrimStart().StartsWith("SELECT"))
                return false;

            foreach (var keyword in ForbiddenKeywords)
            {
                if (normalized.Contains(keyword))
                    return false;
            }

            return true;
        }
    }
}
