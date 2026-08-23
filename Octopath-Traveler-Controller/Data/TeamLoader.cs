namespace Octopath_Traveler.Data;

public static class TeamLoader
{
    private const string PlayerTeamHeader = "Player Team";
    private const string EnemyTeamHeader = "Enemy Team";
    private const int NotFound = -1;

    public static ParsedTeamFile Load(string path)
    {
        EnsureFileExists(path);
        string[] rawLines = File.ReadAllLines(path);
        return ParseLines(rawLines);
    }

    private static void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"No se encontró el archivo de equipo: {path}");
    }

    private static ParsedTeamFile ParseLines(string[] rawLines)
    {
        string[] lines = TrimAllLines(rawLines);
        int playerHeaderIndex = Array.IndexOf(lines, PlayerTeamHeader);
        int enemyHeaderIndex = Array.IndexOf(lines, EnemyTeamHeader);
        if (playerHeaderIndex == NotFound || enemyHeaderIndex == NotFound)
            return new ParsedTeamFile(new List<ParsedTraveler>(), new List<string>());

        List<ParsedTraveler> travelers = ParseTravelerLines(lines, playerHeaderIndex + 1, enemyHeaderIndex);
        List<string> beastNames = ParseBeastLines(lines, enemyHeaderIndex + 1, lines.Length);
        return new ParsedTeamFile(travelers, beastNames);
    }

    private static string[] TrimAllLines(string[] lines)
    {
        string[] trimmed = new string[lines.Length];
        for (int i = 0; i < lines.Length; i++)
            trimmed[i] = lines[i].Trim();
        return trimmed;
    }

    private static List<ParsedTraveler> ParseTravelerLines(string[] lines, int start, int end)
    {
        var travelers = new List<ParsedTraveler>();
        for (int i = start; i < end; i++)
            if (!IsBlank(lines[i]))
                travelers.Add(ParseTravelerLine(lines[i]));
        return travelers;
    }

    private static List<string> ParseBeastLines(string[] lines, int start, int end)
    {
        var beastNames = new List<string>();
        for (int i = start; i < end; i++)
            if (!IsBlank(lines[i]))
                beastNames.Add(lines[i]);
        return beastNames;
    }

    private static bool IsBlank(string line)
        => line.Length == 0;

    private static ParsedTraveler ParseTravelerLine(string line)
    {
        string name = ParseName(line);
        List<string> activeSkills = ParseBracketedList(line, '(', ')');
        List<string> passiveSkills = ParseBracketedList(line, '[', ']');
        return new ParsedTraveler(name, activeSkills, passiveSkills);
    }

    private static string ParseName(string line)
    {
        int cutIndex = FindFirstBracketIndex(line);
        string name = cutIndex == NotFound ? line : line.Substring(0, cutIndex);
        return name.Trim();
    }

    private static int FindFirstBracketIndex(string line)
    {
        int parenIndex = line.IndexOf('(');
        int bracketIndex = line.IndexOf('[');
        if (parenIndex == NotFound) return bracketIndex;
        if (bracketIndex == NotFound) return parenIndex;
        return Math.Min(parenIndex, bracketIndex);
    }

    private static List<string> ParseBracketedList(string line, char openChar, char closeChar)
    {
        int openIndex = line.IndexOf(openChar);
        int closeIndex = line.IndexOf(closeChar);
        if (openIndex == NotFound || closeIndex == NotFound) return new List<string>();
        string inside = line.Substring(openIndex + 1, closeIndex - openIndex - 1).Trim();
        return IsBlank(inside) ? new List<string>() : SplitAndTrim(inside);
    }

    private static List<string> SplitAndTrim(string commaSeparated)
    {
        string[] parts = commaSeparated.Split(',');
        var result = new List<string>();
        foreach (string part in parts)
            result.Add(part.Trim());
        return result;
    }
}
