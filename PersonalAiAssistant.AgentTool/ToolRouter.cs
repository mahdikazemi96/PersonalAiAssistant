using PersonalAiAssistant.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool
{
    public class ToolRouter
    {
        private readonly Dictionary<string, ITool> _tools;

        public ToolRouter(
            IEnumerable<ITool> tools)
        {
            _tools =
                tools.ToDictionary(
                    x => x.Name,
                    StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<ITool> GetTools()
        {
            return _tools.Values.ToList();
        }

        public async Task<ToolExecutionResult> ExecuteAsync(
            string toolName,
            string arguments)
        {
            if (!_tools.TryGetValue(toolName, out var tool))
            {
                throw new InvalidOperationException(
                    $"Tool '{toolName}' not found.");
            }

            return await tool.ExecuteAsync(arguments);
        }
    }
}
