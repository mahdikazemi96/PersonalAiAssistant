using System.Text.Json;
using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class ToolAgentService
{
    private readonly ToolAgentPromptBuilder _promptBuilder;
    private readonly ToolRouter _toolRouter;
    private readonly IChatClient _chatClient;

    public ToolAgentService(
        ToolAgentPromptBuilder promptBuilder,
        ToolRouter toolRouter,
        IChatClient chatClient)
    {
        _promptBuilder = promptBuilder;
        _toolRouter = toolRouter;
        _chatClient = chatClient;
    }

    public async Task<ChatResponse?> TryHandleAsync(
        string question)
    {
        //------------------------------------------
        // Step 1
        // Ask AI whether a tool should be used
        //------------------------------------------

        var selectionPrompt =
            _promptBuilder.BuildSelectionPrompt(
                question,
                _toolRouter.GetTools());

        var selectionMessages = new List<ChatMessage>
        {
            new ChatMessage
            {
                Role = "user",
                Content = selectionPrompt
            }
        };

        var selectionResponse =
            await _chatClient.ChatAsync(selectionMessages);

        if (selectionResponse.Trim().Equals(
                "NONE",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        ToolSelectionResult? selection;

        try
        {
            selection =
                JsonSerializer.Deserialize<ToolSelectionResult>(
                    selectionResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }
        catch
        {
            return null;
        }

        if (selection == null)
            return null;

        //------------------------------------------
        // Step 2
        // Execute tool
        //------------------------------------------

        var toolResult =
            await _toolRouter.ExecuteAsync(
                selection.Tool,
                selection.Arguments);

        //------------------------------------------
        // Step 3
        // Ask AI to generate final answer
        //------------------------------------------

        var resultPrompt =
            _promptBuilder.BuildResultPrompt(
                question,
                selection.Tool,
                toolResult);

        var resultMessages = new List<ChatMessage>
        {
            new ChatMessage
            {
                Role = "user",
                Content = resultPrompt
            }
        };

        var finalAnswer =
            await _chatClient.ChatAsync(resultMessages);

        return new ChatResponse
        {
            Answer = finalAnswer
        };
    }
}