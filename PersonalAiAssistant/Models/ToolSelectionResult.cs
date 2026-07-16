using PersonalAiAssistant.Models.Llm;

namespace PersonalAiAssistant.Models;

public class ToolSelectionResult
{
    public string? Tool { get; set; }

    public string? Arguments { get; set; }

    public static ResponseFormat ResponseFormat { get; } =
        new ResponseFormat
        {
            JsonSchema = new JsonSchemaDefinition
            {
                Name = "tool_selection",
                Strict = true,
                Schema = new
                {
                    type = "object",

                    properties = new
                    {
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
                        "tool",
                        "arguments"
                    },

                    additionalProperties = false
                }
            }
        };
}