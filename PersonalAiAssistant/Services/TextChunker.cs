namespace PersonalAiAssistant.Services;

public class TextChunker
{
    public List<string> Split(
        string text,
        int chunkSize = 500,
        int overlap = 100)
    {
        var chunks = new List<string>();

        if (string.IsNullOrWhiteSpace(text))
            return chunks;

        int start = 0;

        while (start < text.Length)
        {
            int end = Math.Min(start + chunkSize, text.Length);

            // اگر آخر متن نیست، نزدیک‌ترین Space یا Enter را پیدا کن
            if (end < text.Length)
            {
                while (end > start && text[end] != ' ' && text[end] != '\n')
                {
                    end--;
                }

                // اگر هیچ Space پیدا نشد همان اندازه اولیه را استفاده کن
                if (end == start)
                {
                    end = Math.Min(start + chunkSize, text.Length);
                }
            }

            var chunk = text.Substring(start, end - start).Trim();

            if (!string.IsNullOrWhiteSpace(chunk))
            {
                chunks.Add(chunk);
            }

            if (end >= text.Length)
                break;

            start = Math.Max(end - overlap, 0);
        }

        return chunks;
    }
}