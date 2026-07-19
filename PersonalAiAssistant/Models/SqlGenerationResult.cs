using PersonalAiAssistant.Models.Llm;

namespace PersonalAiAssistant.Models;

public class SqlGenerationResult
{
    public string Sql { get; set; } = string.Empty;

    public static ResponseFormat ResponseFormat { get; set; } =
        new ResponseFormat
        {
            JsonSchema = new JsonSchemaDefinition
            {
                Name = "sql_generation",

                Strict = true,

                Schema = new
                {
                    type = "object",

                    properties = new
                    {
                        sql = new
                        {
                            type = "string"
                        }
                    },

                    required = new[]
                    {
                        "sql"
                    },

                    additionalProperties = false
                }
            }
        };
}