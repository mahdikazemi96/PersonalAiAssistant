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
    private readonly HybridRankingService _hybridRankingService;

    public RagService(
        IEmbeddingClient embeddingClient,
        QdrantService qdrantService,
        ConversationService conversationService,
        PromptBuilder promptBuilder,
        IChatClient chatClient,
        HybridRankingService hybridRankingService)
    {
        _embeddingClient = embeddingClient;
        _qdrantService = qdrantService;
        _conversationService = conversationService;
        _promptBuilder = promptBuilder;
        _chatClient = chatClient;
        _hybridRankingService = hybridRankingService;
    }

    public async Task<ChatResponse> AskAsync(string question)
    {
        
        //-------------------------------------------------
        // 2) Conversation
        //-------------------------------------------------

        _conversationService.AddUserMessage(question);

        //-------------------------------------------------
        // 3) Embedding
        //-------------------------------------------------

        var embedding =
            await _embeddingClient.CreateEmbeddingAsync(question);

        //-------------------------------------------------
        // 4) Vector Search
        //-------------------------------------------------

        var documents =
            await _qdrantService.SearchAsync(embedding);

        //-------------------------------------------------
        // 5) Hybrid Ranking
        //-------------------------------------------------

        documents =
            _hybridRankingService.Rank(
                question,
                documents);

        //-------------------------------------------------
        // 6) Prompt
        //-------------------------------------------------

        var history =
            _conversationService.GetMessages();

        var messages =
            _promptBuilder.Build(
                history,
                documents);

        //-------------------------------------------------
        // 7) LLM
        //-------------------------------------------------

        var answer =
            await _chatClient.ChatAsync(messages);

        _conversationService.AddAssistantMessage(answer);

        //-------------------------------------------------
        // 8) Response
        //-------------------------------------------------

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