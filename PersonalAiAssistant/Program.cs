using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Middlewares;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConversationService>();

builder.Services.AddSingleton<PromptBuilder>();

builder.Services.AddSingleton<TextChunker>();

builder.Services.AddScoped<RagService>();

builder.Services.AddScoped<DocumentService>();

builder.Services.AddSingleton<DocumentReaderFactory>();

builder.Services.AddSingleton<IDocumentReader, PdfDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, WordDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, TextDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, MarkdownDocumentReader>();

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

builder.Services.Configure<LmStudioOptions>(builder.Configuration.GetSection("LmStudio"));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();