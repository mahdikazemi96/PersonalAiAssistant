using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AssistantService
{
    private const int MaxIterations = 5;
    private static string prompt = null;

    private readonly IPromptBuilder _promptBuilder;
    private readonly PlannerService _plannerService;
    private readonly ToolExecutionService _toolExecutionService;
    private readonly AnswerGenerationService _answerGenerationService;
    private readonly ConversationService _conversationService;

    public AssistantService(
        [FromKeyedServices("ToolAgentPromptBuilder")] IPromptBuilder promptBuilder,
        PlannerService plannerService,
        ToolExecutionService toolExecutionService,
        AnswerGenerationService answerGenerationService,
        ConversationService conversationService)
    {
        _promptBuilder = promptBuilder;
        _plannerService = plannerService;
        _toolExecutionService = toolExecutionService;
        _answerGenerationService = answerGenerationService;
        _conversationService = conversationService;
    }

    public async Task<ChatResponse> AskAsync(
        string question)
    {
        //------------------------------------------
        // Build Prompt
        //------------------------------------------
        if (prompt == null)
            prompt =
               await _promptBuilder.BuildAsync();

        //------------------------------------------
        // Create Context
        //------------------------------------------

        _conversationService.AddSystemMessage(prompt);
        _conversationService.AddUserMessage(question);

        //------------------------------------------
        // Agent Loop
        //------------------------------------------
        int iteration = 0;
        while (iteration < MaxIterations)
        {
            iteration++;

            //------------------------------------------
            // Planner
            //------------------------------------------

            var action =
                await _plannerService.PlanAsync(_conversationService.Conversation);

            //------------------------------------------
            // Generate Final Answer
            //------------------------------------------

            if (action.Action == AgentActionType.Answer)
            {
                return await _answerGenerationService
                    .GenerateAsync(_conversationService.Conversation);
            }

            //------------------------------------------
            // Save Tool Call
            //------------------------------------------

            _conversationService.AddAssistantMessage(AgentActionMessageHelper.BuildToolCallMessage(action));

            //------------------------------------------
            // Execute Tool
            //------------------------------------------

            var toolResult = await _toolExecutionService.ExecuteAsync(
                _conversationService.Conversation,
                action);

            //------------------------------------------
            // Save Tool Result
            //------------------------------------------

            _conversationService.AddToolMessage(AgentActionMessageHelper.BuildToolResultMessage(action.Tool, toolResult));
        }

        throw new InvalidOperationException(
            "Maximum agent iterations exceeded.");
    }


}