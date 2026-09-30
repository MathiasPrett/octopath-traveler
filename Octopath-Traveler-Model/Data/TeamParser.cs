namespace Octopath_Traveler.Data;

public static class TeamParser
{
    private const string PlayerTeamHeader = "Player Team";
    private const string EnemyTeamHeader = "Enemy Team";
    private const int NotFound = -1;

    public static ParsedTeamFile Parse(string[] rawLines)
    {
        string[] lines = TrimAllLines(rawLines);
        int playerHeaderIndex = Array.IndexOf(lines, PlayerTeamHeader);
        int enemyHeaderIndex = Array.IndexOf(lines, EnemyTeamHeader);
        return AreBothFound(playerHeaderIndex, enemyHeaderIndex)
            ? ParseSections(lines, playerHeaderIndex, enemyHeaderIndex)
            : CreateEmptyTeam();
    }

    private static ParsedTeamFile ParseSections(string[] lines, int playerHeaderIndex, int enemyHeaderIndex)
        => new ParsedTeamFile(
            ParseTravelerLines(lines, playerHeaderIndex + 1, enemyHeaderIndex),
            ParseBeastLines(lines, enemyHeaderIndex + 1, lines.Length));

    private static ParsedTeamFile CreateEmptyTeam()
        => new ParsedTeamFile(new List<ParsedTraveler>(), new List<string>());

    private static bool AreBothFound(int firstIndex, int secondIndex)
        => firstIndex != NotFound && secondIndex != NotFound;

    private static string[] TrimAllLines(string[] lines)
        => lines.Select(line => line.Trim()).ToArray();

    private static List<ParsedTraveler> ParseTravelerLines(string[] lines, int start, int end)
        => SelectNonBlankLines(lines, start, end).Select(ParseTravelerLine).ToList();

    private static List<string> ParseBeastLines(string[] lines, int start, int end)
        => SelectNonBlankLines(lines, start, end).ToList();

    private static IEnumerable<string> SelectNonBlankLines(string[] lines, int start, int end)
        => lines.Skip(start).Take(end - start).Where(line => !IsBlank(line));

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
        if (!AreBothFound(openIndex, closeIndex)) return new List<string>();
        string inside = line.Substring(openIndex + 1, closeIndex - openIndex - 1).Trim();
        return IsBlank(inside) ? new List<string>() : SplitAndTrim(inside);
    }

    private static List<string> SplitAndTrim(string commaSeparated)
        => commaSeparated.Split(',').Select(part => part.Trim()).ToList();
}
