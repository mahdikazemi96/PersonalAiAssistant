using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConversationService>();

builder.Services.AddHttpClient<IChatClient, LmStudioChatClient>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:1234");
});

builder.Services.AddHttpClient<IEmbeddingClient, LmStudioEmbeddingClient>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:1234");
});

builder.Services.AddHttpClient<QdrantService>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:6333");
});

builder.Services.AddScoped<RagService>();

builder.Services.AddSingleton<PromptBuilder>();

builder.Services.AddSingleton<IDocumentReader, PdfDocumentReader>();

builder.Services.AddSingleton<TextChunker>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();