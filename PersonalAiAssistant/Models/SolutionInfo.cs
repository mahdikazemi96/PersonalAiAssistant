using PersonalAiAssistant.Models;

public class SolutionInfo
{
    public string Name { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public List<ProjectInfo> Projects { get; } =
        new();

    public IReadOnlyList<SourceFileInfo> AllFiles =>
        Projects
            .SelectMany(GetFiles)
            .ToList();

    private static IEnumerable<SourceFileInfo> GetFiles(
        ProjectInfo project)
    {
        foreach (var file in project.Files)
            yield return file;

        foreach (var folder in project.Folders)
        {
            foreach (var file in GetFiles(folder))
                yield return file;
        }
    }

    private static IEnumerable<SourceFileInfo> GetFiles(
        FolderInfo folder)
    {
        foreach (var file in folder.Files)
            yield return file;

        foreach (var child in folder.Folders)
        {
            foreach (var file in GetFiles(child))
                yield return file;
        }
    }
}