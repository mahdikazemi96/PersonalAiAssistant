using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public interface IDatabaseSchemaReader
    {
        Task<DatabaseSchema> ReadAsync();
    }
}
