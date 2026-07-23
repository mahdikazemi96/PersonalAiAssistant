using PersonalAiAssistant.Models;
using System.Text;

namespace PersonalAiAssistant.Services;

public class SolutionAnalyzerService : ISolutionAnalyzerService
{
    private readonly string _workspacePath;

    public SolutionAnalyzerService(
        IConfiguration configuration)
    {
        _workspacePath =
            configuration["WorkspacePath"]
            ?? throw new InvalidOperationException(
                "WorkspacePath is not configured.");
    }
    public async Task<SolutionInfo> GetSolutionAsync()
    {
        //------------------------------------------
        // Validation
        //------------------------------------------

        if (!Directory.Exists(_workspacePath))
        {
            throw new DirectoryNotFoundException(
                _workspacePath);
        }

        //------------------------------------------
        // Solution
        //------------------------------------------

        var solutionFile =
            FindSolution();

        var solution =
            new SolutionInfo
            {
                Name = Path.GetFileNameWithoutExtension(
                    solutionFile),

                Path = solutionFile
            };

        //------------------------------------------
        // Projects
        //------------------------------------------

        foreach (var projectFile in FindProjects())
        {
            solution.Projects.Add(
                await LoadProjectAsync(projectFile));
        }

        return solution;
    }

    public async Task<string> AnalyzeAsync()
    {
        var solution = await GetSolutionAsync();

        var builder = new StringBuilder();

        //------------------------------------------
        // Solution
        //------------------------------------------

        builder.AppendLine("Solution");

        builder.AppendLine();

        builder.AppendLine(solution.Name);

        builder.AppendLine();

        builder.AppendLine(
            "========================================");

        builder.AppendLine();

        //------------------------------------------
        // Projects
        //------------------------------------------

        builder.AppendLine("Projects");

        builder.AppendLine();

        foreach (var project in solution.Projects)
        {
            AppendProject(
                builder,
                project);
        }

        return builder.ToString();
    }

    private string FindSolution()
    {
        var solution =
            Directory
                .EnumerateFiles(
                    _workspacePath,
                    "*.sln",
                    SearchOption.TopDirectoryOnly)
                .FirstOrDefault();

        if (solution == null)
        {
            throw new InvalidOperationException(
                "Solution file not found.");
        }

        return solution;
    }

    private IReadOnlyList<string> FindProjects()
    {
        return Directory
            .EnumerateFiles(
                _workspacePath,
                "*.csproj",
                SearchOption.AllDirectories)
            .OrderBy(Path.GetFileNameWithoutExtension)
            .ToList();
    }

    private static void AppendProject(
    StringBuilder builder,
    ProjectInfo project)
    {
        builder.AppendLine(
            "----------------------------------------");

        builder.AppendLine();

        builder.AppendLine(project.Name);

        builder.AppendLine();

        //------------------------------------------
        // Folders
        //------------------------------------------

        builder.AppendLine("Folders");

        builder.AppendLine();

        foreach (var folder in project.Folders)
        {
            builder.AppendLine(
                $"- {folder.Name}");
        }

        builder.AppendLine();

        //------------------------------------------
        // Files
        //------------------------------------------

        builder.AppendLine("Files");

        builder.AppendLine();

        foreach (var file in project.Files)
        {
            builder.AppendLine(
                $"- {file.Name}");
        }

        builder.AppendLine();
    }
    private Task<ProjectInfo> LoadProjectAsync(
    string projectFile)
    {
        var projectDirectory =
            Path.GetDirectoryName(projectFile)!;

        var project =
            new ProjectInfo
            {
                Name =
                    Path.GetFileNameWithoutExtension(
                        projectFile),

                Path = projectDirectory
            };

        //------------------------------------------
        // Folders
        //------------------------------------------

        foreach (var directory in Directory.EnumerateDirectories(
                     projectDirectory,
                     "*",
                     SearchOption.TopDirectoryOnly)
                     .OrderBy(Path.GetFileName))
        {
            project.Folders.Add(
                LoadFolder(directory));
        }

        //------------------------------------------
        // Files
        //------------------------------------------

        foreach (var file in Directory.EnumerateFiles(
             projectDirectory,
             "*.*",
             SearchOption.TopDirectoryOnly)
             .OrderBy(Path.GetFileName))
        {
            project.Files.Add(
                new SourceFileInfo
                {
                    Name = Path.GetFileName(file),
                    Path = file
                });
        }

        return Task.FromResult(project);
    }
    private FolderInfo LoadFolder(
    string folderPath)
    {
        var folder =
            new FolderInfo
            {
                Name = Path.GetFileName(folderPath),
                Path = folderPath
            };

        //------------------------------------------
        // Files
        //------------------------------------------

        foreach (var file in Directory.EnumerateFiles(
                     folderPath,
                     "*.*",
                     SearchOption.TopDirectoryOnly)
                     .OrderBy(Path.GetFileName))
        {
            folder.Files.Add(
                new SourceFileInfo
                {
                    Name = Path.GetFileName(file),
                    Path = file
                });
        }

        //------------------------------------------
        // Child Folders
        //------------------------------------------

        foreach (var child in Directory.EnumerateDirectories(
                     folderPath,
                     "*",
                     SearchOption.TopDirectoryOnly)
                     .OrderBy(Path.GetFileName))
        {
            folder.Folders.Add(
                LoadFolder(child));
        }

        return folder;
    }
}