namespace Octopath_Traveler.Utils;

public static class FileGuard
{
    public static void EnsureExists(string path, string missingFileMessage)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"{missingFileMessage}: {path}");
    }
}
