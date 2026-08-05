using PersonalAiAssistant.Models.Llm;
using System.Text;

namespace PersonalAiAssistant.Models;

public enum AgentActionType
{
    Answer,
    Tool
}

public class AgentAction
{
    public AgentActionType Action { get; set; }

    public string? Tool { get; set; }

    public string? Arguments { get; set; }

    public static ResponseFormat ResponseFormat { get; } =
        new ResponseFormat
        {
            JsonSchema = new JsonSchemaDefinition
            {
                Name = "planner",

                Strict = true,

                Schema = new
                {
                    type = "object",

                    properties = new
                    {
                        action = new
                        {
                            type = "string",

                            @enum = new[]
                            {
                                "Answer",
                                "Tool"
                            }
                        },

                        tool = new
                        {
                            type = new[]
                            {
                                "string",
                                "null"
                            }
                        },

                        arguments = new
                        {
                            type = new[]
                            {
                                "string",
                                "null"
                            }
                        }
                    },

                    required = new[]
                    {
                        "action",
                        "tool",
                        "arguments"
                    },

                    additionalProperties = false
                }
            }
        };
}

public class AgentActionMessageHelper
{
    public static string BuildToolCallMessage(AgentAction action)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Tool Executed");

        builder.AppendLine();

        builder.AppendLine($"Tool: {action.Tool}");

        if (!string.IsNullOrWhiteSpace(action.Arguments))
        {
            builder.AppendLine();

            builder.AppendLine("Arguments:");

            builder.AppendLine(action.Arguments);
        }

        return builder.ToString();
    }

    public static string BuildToolResultMessage(
        string tool,
        string result)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Tool Result");

        builder.AppendLine();

        builder.AppendLine($"Tool: {tool}");

        builder.AppendLine();

        builder.AppendLine(result);

        return builder.ToString();
    }
}