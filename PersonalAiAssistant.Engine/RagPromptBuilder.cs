using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Contracts.Models;
using System.Text;

namespace PersonalAiAssistant.Engine
{
    public class RagPromptBuilder : IPromptBuilder
    {
        private readonly IPromptLoader _promptLoader;

        public RagPromptBuilder(
            IPromptLoader promptLoader)
        {
            _promptLoader = promptLoader;
        }
        public async Task<string> BuildAsync()
        {
            var prompt = _promptLoader.Load("Rag.txt");

            var builder = new StringBuilder(prompt);

            builder.AppendLine();
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
