using System.Collections.Generic;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public class DatabaseSchema
    {
        public List<TableSchema> Tables { get; set; } = new();
    }
}
