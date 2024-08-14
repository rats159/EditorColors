using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(CodeUtilities), nameof(CodeUtilities.SyntaxColor))]
internal class CodeUtitlitiesPatch
{
    public static bool Prefix(ref string __result, string code)
    {
        string text = code;
        List<(Capture, string)> captures = [];

        foreach ((Regex regex, string color) in ThemeManager.Colors())
        {
            foreach (Match item in regex.Matches(text))
            {
                captures.Add((item, color));
            }
        }
        List<(Capture, string)> list2 = [];
        RangeSet rangeSet = new();

        foreach ((Capture cap, string str) in captures.OrderBy(m=>m.Item1.Index))
        {
            if (rangeSet.AddRange(cap.Index, cap.Length))
            {
                list2.Add((cap, str));
            }
        }

        list2.Reverse();

        foreach ((Capture cap, string color) in list2)
        {
            text = text.Insert(cap.Index + cap.Length, "</color>");
            text = text.Insert(cap.Index, string.Format("<color={0}>", color));
        }

        __result = text;
        return false;
    }
}