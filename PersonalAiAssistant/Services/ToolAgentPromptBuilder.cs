using System.Text;

namespace PersonalAiAssistant.Services;

public class ToolAgentPromptBuilder : IPromptBuilder
{
    private readonly IPromptLoader _promptLoader;
    private readonly ToolRouter _toolRouter;

    public ToolAgentPromptBuilder(IPromptLoader promptLoader, ToolRouter toolRouter)
    {
        _promptLoader = promptLoader;
        _toolRouter = toolRouter;
    }

    public async Task<string> BuildAsync()
    {
        //----------------------------------------------------
        // Main Prmpt
        //----------------------------------------------------

        var prompt = _promptLoader.Load("PlannerSystem.txt");

        var builder = new StringBuilder(prompt);

        builder.AppendLine();
        builder.AppendLine();

        //----------------------------------------------------
        // Tools
        //----------------------------------------------------

        builder.AppendLine("Available tools:");
        builder.AppendLine();

        var tools = _toolRouter.GetTools().ToList();

        foreach (var tool in tools)
        {
            builder.AppendLine($"Name: {tool.Name}");
            builder.AppendLine($"Description: {tool.Description}");

            var context =
                await tool.GetContextAsync();

            if (!string.IsNullOrWhiteSpace(context))
            {
                builder.AppendLine();
                builder.AppendLine("Context:");
                builder.AppendLine(context);
            }

            builder.AppendLine();
            builder.AppendLine("----------------------------------------");
            builder.AppendLine();
        }

        //----------------------------------------------------
        // Examples
        //----------------------------------------------------

        var examples = _promptLoader.Load("PlannerSystemExamples.txt");

        builder.AppendLine();
        builder.AppendLine();

        builder.AppendLine(examples);

        return builder.ToString();
    }
}