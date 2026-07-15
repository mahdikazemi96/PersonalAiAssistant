using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class RagService
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly QdrantService _qdrantService;
    private readonly ConversationService _conversationService;
    private readonly PromptBuilder _promptBuilder;
    private readonly IChatClient _chatClient;
    private readonly HybridRankingService _hybridSearchService;

    public RagService(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService,
        ConversationService conversationService,
        PromptBuilder promptBuilder,
        IChatClient chatClient,
        HybridRankingService hybridSearchService)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
        _conversationService = conversationService;
        _promptBuilder = promptBuilder;
        _chatClient = chatClient;
        _hybridSearchService = hybridSearchService;
    }

    public async Task<ChatResponse> AskAsync(string question)
    {
        _conversationService.AddUserMessage(question);

        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(question);

        var documents =
            await _qdrantService.SearchAsync(embedding);

        documents =
            _hybridSearchService.Rank(
                question,
                documents);

        var history =
            _conversationService.GetMessages();

        var messages =
            _promptBuilder.Build(
                history,
                documents);

        var answer =
            await _chatClient.ChatAsync(messages);

        _conversationService.AddAssistantMessage(answer);

        var response = new ChatResponse
        {
            Answer = answer
        };

        foreach (var source in documents)
        {
            var item =
                $"{source.FileName} (Chunk {source.ChunkNumber})";

            if (!response.Sources.Contains(item))
            {
                response.Sources.Add(item);
            }
        }

        return response;
    }
}