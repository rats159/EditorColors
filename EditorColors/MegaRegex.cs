using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace EditorColors;

public class MegaRegex
{
    private List<(string, string)> pairs = [];

    public void Add(string name, string pattern)
    {
        this.pairs.Add((name, pattern));
    }

    public string Build()
    {
        string pattern = this.pairs.Select(
            tup => $"(?<{tup.Item1}>(?:{tup.Item2}))"
        ).Join(delimiter:"|");

        return pattern;
    }
}