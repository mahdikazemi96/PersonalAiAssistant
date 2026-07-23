using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Middlewares;
using PersonalAiAssistant.Models;
using PersonalAiAssistant.Services;
using PersonalAiAssistant.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConversationService>();

builder.Services.AddSingleton<PromptBuilder>();

builder.Services.AddSingleton<HybridRankingService>();

builder.Services.AddSingleton<TextChunker>();

builder.Services.AddScoped<AnswerGenerationService>();

builder.Services.AddScoped<DocumentService>();

builder.Services.AddSingleton<DocumentReaderFactory>();

builder.Services.AddSingleton<IDocumentReader, PdfDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, WordDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, TextDocumentReader>();

builder.Services.AddSingleton<IDocumentReader, MarkdownDocumentReader>();

builder.Services.AddSingleton<IDatabaseSchemaReader, SqlServerSchemaReader>();

builder.Services.AddSingleton<ISqlToolService, SqlToolService>();

builder.Services.AddSingleton<SqlValidator>();

builder.Services.AddSingleton<ITool, CalculatorTool>();

builder.Services.AddSingleton<ToolRouter>();

builder.Services.AddScoped<AssistantService>();

builder.Services.AddScoped<PlannerService>();

builder.Services.AddScoped<AgentService>();

builder.Services.AddScoped<ToolExecutionService>();

builder.Services.AddSingleton<ToolAgentPromptBuilder>();

builder.Services.AddSingleton<SqlPromptBuilder>();

builder.Services.AddSingleton<ISafeFileSystemService, SafeFileSystemService>();

builder.Services.AddSingleton<ITool, FileSystemTool>();

builder.Services.AddSingleton<ITool, WeatherTool>();

builder.Services.AddSingleton<ITool, SqlTool>();

builder.Services.AddSingleton<ISolutionAnalyzerService, SolutionAnalyzerService>();

builder.Services.AddSingleton<ITool, SolutionAnalyzerTool>();

builder.Services.AddHttpClient<IChatClient, LmStudioChatClient>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:1234");
    client.Timeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddHttpClient<IEmbeddingClient, LmStudioEmbeddingClient>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:1234");
});

builder.Services.AddHttpClient<QdrantService>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:6333");
});

builder.Services.AddHttpClient<IWeatherService, OpenWeatherService>(client =>
{
    client.BaseAddress =
        new Uri("https://api.openweathermap.org/data/2.5/");
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