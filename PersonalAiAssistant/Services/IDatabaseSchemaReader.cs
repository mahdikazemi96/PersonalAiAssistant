using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public interface IDatabaseSchemaReader
{
    Task<DatabaseSchema> ReadAsync();
}