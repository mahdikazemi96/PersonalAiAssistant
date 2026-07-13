using PersonalAiAssistant.Clients;

namespace PersonalAiAssistant.Services;

public class RagService
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;
    private readonly ConversationService _conversationService;
    private readonly PromptBuilder _promptBuilder;
    private readonly IChatClient _chatClient;

    public RagService(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService,
        ConversationService conversationService,
        PromptBuilder promptBuilder,
        IChatClient chatClient)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
        _conversationService = conversationService;
        _promptBuilder = promptBuilder;
        _chatClient = chatClient;
    }

    public async Task<string> AskAsync(string question)
    {
        // ذخیره سؤال
        _conversationService.AddUserMessage(question);

        // ساخت Embedding
        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(question);

        // جستجوی اسناد
        var documents =
            await _qdrantService.SearchAsync(embedding);

        // گرفتن History
        var history =
            _conversationService.GetMessages();

        // ساخت Prompt
        var messages =
            _promptBuilder.Build(history, documents);

        // گرفتن پاسخ
        var answer =
            await _chatClient.ChatAsync(messages);

        // ذخیره پاسخ
        _conversationService.AddAssistantMessage(answer);

        return answer;
    }
}