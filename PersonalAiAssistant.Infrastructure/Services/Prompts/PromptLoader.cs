using Microsoft.AspNetCore.Hosting;
using PersonalAiAssistant.Contracts.Interfaces;

namespace PersonalAiAssistant.Infrastructure.Services.Prompts
{
    public sealed class PromptLoader : IPromptLoader
    {
        private readonly IWebHostEnvironment _environment;

        public PromptLoader(
            IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Load(string promptName)
        {
            var path =
                Path.Combine(
                    _environment.ContentRootPath,
                    "Prompts/",
                    promptName);

            return File.ReadAllText(path);
        }
    }
}
