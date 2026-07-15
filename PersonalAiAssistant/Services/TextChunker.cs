namespace PersonalAiAssistant.Services;

public class TextChunker
{
    private const int MaxChunkLength = 1000;

    public List<string> Split(string text)
    {
        var chunks = new List<string>();

        if (string.IsNullOrWhiteSpace(text))
            return chunks;

        var paragraphs = text.Split(
            new[] { "\r\n\r\n", "\n\n" },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var paragraph in paragraphs)
        {
            var value = paragraph.Trim();

            if (string.IsNullOrWhiteSpace(value))
                continue;

            if (value.Length <= MaxChunkLength)
            {
                chunks.Add(value);
                continue;
            }

            SplitLargeParagraph(value, chunks);
        }

        return chunks;
    }

    private static void SplitLargeParagraph(
     string paragraph,
     List<string> chunks)
    {
        const int overlapWords = 30;

        var words = paragraph.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        var currentWords = new List<string>();

        foreach (var word in words)
        {
            var currentText =
                string.Join(" ", currentWords);

            if (currentText.Length + word.Length + 1 > MaxChunkLength)
            {
                chunks.Add(string.Join(" ", currentWords));

                currentWords =
                    currentWords
                        .Skip(Math.Max(0, currentWords.Count - overlapWords))
                        .ToList();
            }

            currentWords.Add(word);
        }

        if (currentWords.Count > 0)
        {
            chunks.Add(string.Join(" ", currentWords));
        }
    }
}