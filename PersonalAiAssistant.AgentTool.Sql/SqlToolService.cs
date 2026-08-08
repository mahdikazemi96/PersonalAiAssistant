using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAiAssistant.Contracts.Interfaces;
using PersonalAiAssistant.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.Sql
{
    public class SqlToolService : ISqlToolService
    {
        private readonly IChatClient _chatClient;
        private readonly IPromptBuilder _promptBuilder;
        private readonly SqlValidator _validator;
        private readonly string _connectionString;

        public SqlToolService(
            IChatClient chatClient,
            [FromKeyedServices("SqlPromptBuilder")] IPromptBuilder promptBuilder,
            SqlValidator validator,
            IConfiguration configuration)
        {
            _chatClient = chatClient;
            _promptBuilder = promptBuilder;
            _validator = validator;

            _connectionString =
                configuration.GetConnectionString("SqlServer")
                ?? throw new InvalidOperationException(
                    "Connection string 'SqlServer' not found.");
        }

        public async Task<string> ExecuteAsync(
            string userQuestion)
        {
            //-------------------------------------------------
            // Build prompt
            //-------------------------------------------------

            var prompt =
                await _promptBuilder.BuildAsync();

            //------------------------------------
            // User Question
            //------------------------------------

            prompt = string.Join(Environment.NewLine, prompt, "User Question:", Environment.NewLine, userQuestion);

            //-------------------------------------------------
            // Ask LLM
            //-------------------------------------------------

            var messages = new List<ChatMessage>
        {
            new ChatMessage
            {
                Role = "user",
                Content = prompt
            }
        };

            var response =
                await _chatClient.ChatAsync(
                    messages,
                    SqlGenerationResult.ResponseFormat);

            //-------------------------------------------------
            // Deserialize
            //-------------------------------------------------

            var sqlGeneration =
                JsonSerializer.Deserialize<SqlGenerationResult>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (sqlGeneration == null)
            {
                throw new InvalidOperationException(
                    "Unable to generate SQL.");
            }

            //-------------------------------------------------
            // Validate SQL
            //-------------------------------------------------

            if (!_validator.IsSafe(sqlGeneration.Sql))
            {
                throw new InvalidOperationException(
                    "Unsafe SQL generated.");
            }

            //-------------------------------------------------
            // Execute SQL
            //-------------------------------------------------

            return await ExecuteQueryAsync(sqlGeneration.Sql);
        }

        private async Task<string> ExecuteQueryAsync(
            string sql)
        {
            await using var connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var command =
                new SqlCommand(sql, connection);

            await using var reader =
                await command.ExecuteReaderAsync();

            return await ToJsonAsync(reader);
        }

        private static async Task<string> ToJsonAsync(
            SqlDataReader reader)
        {
            var rows =
                new List<Dictionary<string, object?>>();

            while (await reader.ReadAsync())
            {
                var row =
                    new Dictionary<string, object?>();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] =
                        reader.IsDBNull(i)
                            ? null
                            : reader.GetValue(i);
                }

                rows.Add(row);
            }

            return JsonSerializer.Serialize(
                rows,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }
    }
}
