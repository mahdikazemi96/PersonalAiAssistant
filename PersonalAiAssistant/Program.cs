using PersonalAiAssistant.Clients;
using PersonalAiAssistant.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IChatClient, LmStudioChatClient>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:1234");
});

builder.Services.Configure<LmStudioOptions>(builder.Configuration.GetSection("LmStudio"));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();