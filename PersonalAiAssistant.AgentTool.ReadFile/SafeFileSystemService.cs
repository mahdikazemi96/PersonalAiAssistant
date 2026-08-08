using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAiAssistant.AgentTool.ReadFile
{
    public class SafeFileSystemService
        : ISafeFileSystemService
    {
        private readonly string _rootDirectory;

        public SafeFileSystemService()
        {
            _rootDirectory =
                Path.GetFullPath(
                    @"D:\AIWorkspace");
        }

        public Task<List<string>> GetFilesAsync(
            string relativePath)
        {
            var directory =
                GetSafePath(relativePath);

            if (!Directory.Exists(directory))
            {
                return Task.FromResult(
                    new List<string>());
            }

            var files =
                Directory
                    .GetFiles(directory)
                    .Select(Path.GetFileName)
                    .Where(x => x != null)
                    .Select(x => x!)
                    .ToList();

            return Task.FromResult(files);
        }

        public async Task<string> ReadFileAsync(
            string relativePath)
        {
            var file =
                GetSafePath(relativePath);

            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File '{relativePath}' was not found.");
            }

            return await File.ReadAllTextAsync(
                file,
                Encoding.UTF8);
        }

        private string GetSafePath(
            string relativePath)
        {
            relativePath ??= string.Empty;

            var combinedPath =
                Path.Combine(
                    _rootDirectory,
                    relativePath);

            var fullPath =
                Path.GetFullPath(combinedPath);

            if (!fullPath.StartsWith(
                    _rootDirectory,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }

            return fullPath;
        }
    }
}
