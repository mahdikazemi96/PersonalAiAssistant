using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class AgentService
{
    private const int MaxIterations = 5;

    private readonly PlannerService _plannerService;
    private readonly ToolExecutionService _toolExecutionService;
    private readonly AnswerGenerationService _answerGenerationService;

    public AgentService(
        PlannerService plannerService,
        ToolExecutionService toolExecutionService,
        AnswerGenerationService answerGenerationService)
    {
        _plannerService = plannerService;
        _toolExecutionService = toolExecutionService;
        _answerGenerationService = answerGenerationService;
    }

    public async Task<ChatResponse> AskAsync(
        string question)
    {
        //------------------------------------------
        // Create Context
        //------------------------------------------

        var context = new AgentContext();

        context.Conversation.Add(
            new ChatMessage
            {
                Role = "user",
                Content = question
            });

        //------------------------------------------
        // Agent Loop
        //------------------------------------------

        while (context.Iteration < MaxIterations)
        {
            context.Iteration++;

            //------------------------------------------
            // Planner
            //------------------------------------------

            var action =
                await _plannerService.PlanAsync(
                    context);

            //------------------------------------------
            // Generate Final Answer
            //------------------------------------------

            if (action.Action == AgentActionType.Answer)
            {
                return await _answerGenerationService
                    .GenerateAsync(context);
            }

            //------------------------------------------
            // Execute Tool
            //------------------------------------------

            await _toolExecutionService.ExecuteAsync(
                context,
                action);
        }

        throw new InvalidOperationException(
            "Maximum agent iterations exceeded.");
    }
}