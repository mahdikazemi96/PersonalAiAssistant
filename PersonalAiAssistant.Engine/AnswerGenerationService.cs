using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Contracts.Models;

namespace PersonalAiAssistant.Engine
{
    public class AnswerGenerationService
    {
        private readonly IChatClient _chatClient;

        public AnswerGenerationService(
            IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<ChatResponse> GenerateAsync(
            IEnumerable<ChatMessage> conversation)
        {
            //------------------------------------------
            // Prepare Context To LLM Generate Just Answer
            //------------------------------------------

            var requestContext = new List<ChatMessage>();
            requestContext = conversation.Where(c => c.Role != "system").ToList();

            //------------------------------------------
            // Ask LLM
            //------------------------------------------

            var answer =
                await _chatClient.ChatAsync(
                    requestContext);

            //------------------------------------------
            // Response
            //------------------------------------------

            return new ChatResponse
            {
                Answer = answer
            };
        }
    }
}
