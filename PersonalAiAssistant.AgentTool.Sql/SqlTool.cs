using PersonalAiAssistant.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public class SqlTool : ITool
    {
        private readonly ISqlToolService _sqlToolService;

        public SqlTool(
            ISqlToolService sqlToolService)
        {
            _sqlToolService = sqlToolService;
        }

        public string Name => "sql";

        public string Description =>
            "Execute read-only SQL queries against SQL Server.";
        public async Task<string?> GetContextAsync()
        {
            var context = new StringBuilder();
            context.Append("A database for an e-commerce shop.");
            context.AppendLine();
            context.Append("This database stroes data about the shop Customers, Products, Orders.");
            context.AppendLine();
            context.Append("Answering to any question about Customers, Products or Orders, will need to this Tool.");
            return context.ToString();
        }
        public async Task<ToolExecutionResult> ExecuteAsync(
            string arguments)
        {
            var result = await _sqlToolService.ExecuteAsync(arguments);

            return new ToolExecutionResult
            {
                Content = result.ToString() ?? string.Empty,
                ContentType = "text/plain"
            };
        }
    }
}
