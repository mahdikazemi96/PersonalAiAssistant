using System;

namespace PersonalAiAssistant.Models;

public class EmbeddingResultResponse
{
    public int Dimension { get; set; }

    public float[] Vector { get; set; } = Array.Empty<float>();
}