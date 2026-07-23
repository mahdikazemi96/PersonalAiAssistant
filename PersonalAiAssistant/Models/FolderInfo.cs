namespace PersonalAiAssistant.Models;

public class FolderInfo
{
    public string Name { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public List<FolderInfo> Folders { get; } =
        new();

    public List<SourceFileInfo> Files { get; } =
        new();

    public string RelativePath { get; set; } = string.Empty;
}