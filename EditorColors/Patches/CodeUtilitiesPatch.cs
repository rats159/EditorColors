using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(CodeUtilities), nameof(CodeUtilities.SyntaxColor))]
internal class CodeUtitlitiesPatch
{
    public static bool Prefix(ref string __result, string code)
    {
        string text = code;
        List<ValueTuple<Capture, string>> captures = [];

        foreach (KeyValuePair<Regex, string> valueTuple in ThemeManager.Colors())
        {
            foreach (Capture item in valueTuple.Key.Matches(text))
            {
                captures.Add(new(item, valueTuple.Value));
            }
        }
        List<ValueTuple<Capture, string>> list2 = [];
        RangeSet rangeSet = new();
        foreach (ValueTuple<Capture, string> valueTuple2 in from m in captures
                                                            orderby m.Item1.Index
                                                            select m)
        {
            if (rangeSet.AddRange(valueTuple2.Item1.Index, valueTuple2.Item1.Length))
            {
                list2.Add(new(valueTuple2.Item1, valueTuple2.Item2));
            }
        }
        list2.Reverse();
        foreach (ValueTuple<Capture, string> valueTuple3 in list2)
        {
            text = text.Insert(valueTuple3.Item1.Index + valueTuple3.Item1.Length, "</color>");
            text = text.Insert(valueTuple3.Item1.Index, string.Format("<color={0}>", valueTuple3.Item2));
        }
        __result = text;
        return false;
    }
}