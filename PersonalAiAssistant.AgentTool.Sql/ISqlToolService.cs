using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public interface ISqlToolService
    {
        Task<string> ExecuteAsync(string userQuestion);
    }
}
