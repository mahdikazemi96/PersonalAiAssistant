using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Contracts.Models;
using PersonalAiAssistant.Infrastructure.Services.VectorDb;
using System.Text;

namespace PersonalAiAssistant.Engine
{
    public class RagService
    {
        private static string prompt = null;

        private readonly IEmbeddingClient _embeddingClient;
        private readonly QdrantService _qdrantService;
        private readonly HybridRankingService _hybridRankingService;
        private readonly ConversationService _conversationService;
        private readonly IPromptBuilder _promptBuilder;
        private readonly IChatClient _chatClient;

        public RagService(
            IEmbeddingClient embeddingClient,
            QdrantService qdrantService,
            HybridRankingService hybridRankingService,
            ConversationService conversationService,
            [FromKeyedServices("RagPromptBuilder")] IPromptBuilder promptBuilder,
            IChatClient chatClient)
        {
            _embeddingClient = embeddingClient;
            _qdrantService = qdrantService;
            _hybridRankingService = hybridRankingService;
            _conversationService = conversationService;
            _promptBuilder = promptBuilder;
            _chatClient = chatClient;
        }

        public async Task<ChatResponse> AskAsync(string question)
        {
            //-------------------------------------------------
            // Embedding
            //-------------------------------------------------

            var embedding =
                await _embeddingClient.CreateEmbeddingAsync(question);

            //-------------------------------------------------
            // Vector Search
            //-------------------------------------------------

            var documents =
                await _qdrantService.SearchAsync(embedding);

            documents = _hybridRankingService
                .Rank(question, documents);

            if (documents.Count == 0)
                return null;

            //------------------------------------------
            // Build Prompt
            //------------------------------------------
            if (prompt == null)
                prompt =
                   await _promptBuilder.BuildAsync();

            var builder = new StringBuilder(prompt);

            for (var i = 0; i < documents.Count; i++)
            {
                builder.AppendLine(
                    $"========== Document {i + 1} ==========");

                builder.AppendLine();

                builder.AppendLine(documents[i].Text);

                builder.AppendLine();
            }

            var finalPrompt = builder.ToString();

            //-------------------------------------------------
            // Conversation
            //-------------------------------------------------

            _conversationService.AddSystemMessage(finalPrompt);
            _conversationService.AddUserMessage(question);

            //-------------------------------------------------
            // LLM
            //-------------------------------------------------

            var answer =
                await _chatClient.ChatAsync(_conversationService.Conversation);

            _conversationService.AddAssistantMessage(answer);

            //-------------------------------------------------
            // Response
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
}
