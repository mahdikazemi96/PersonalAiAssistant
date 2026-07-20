using PersonalAiAssistant.Models.Llm;
using PersonalAiAssistant.Tools;

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