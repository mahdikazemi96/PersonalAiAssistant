using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.AgentTool;
using PersonalAiAssistant.AgentTool.Calculator;
using PersonalAiAssistant.AgentTool.ReadFile;
using PersonalAiAssistant.AgentTool.SolutionAnalyzer;
using PersonalAiAssistant.AgentTool.Sql;
using PersonalAiAssistant.AgentTool.Weather;
using PersonalAiAssistant.Engine;
using PersonalAiAssistant.Infrastructure;
using PersonalAiAssistant.Middlewares;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(configuration);
builder.Services.AddEngine(configuration);
builder.Services.AddAgentTool(configuration);
builder.Services.AddCalculatorTool(configuration);
builder.Services.AddReadFileTool(configuration);
builder.Services.AddWeatherTool(configuration);
builder.Services.AddSqlTool(configuration);
builder.Services.AddSolutionAnalyzerTool(configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();