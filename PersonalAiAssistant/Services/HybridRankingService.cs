using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class HybridRankingService
{
    public List<SearchResult> Rank(
        string question,
        List<SearchResult> documents)
    {
        if (documents.Count == 0)
            return documents;

        var keywords = ExtractKeywords(question);

        foreach (var document in documents)
        {
            foreach (var keyword in keywords)
            {
                if (document.Text.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase))
                {
                    document.Score += 0.10f;
                }
            }
        }

        return documents
            .OrderByDescending(x => x.Score)
            .ToList();
    }

    private static List<string> ExtractKeywords(string question)
    {
        return question
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.Length > 2)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}