using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(CodeUtilities), nameof(CodeUtilities.SyntaxColor2))]
internal class CodeUtilitiesPatch
{
    public static bool Prefix(ref string __result, string code, string searchWord = "", int searchIndex = -1)
    {
        string regex = String.IsNullOrEmpty(searchWord) ? ColorGroup.Pattern : "(?<search>(?i:" + Regex.Escape(searchWord) + ")(?-i))|" + ColorGroup.Pattern;

        __result = Regex.Replace(
            code,
            regex,
            match =>
            {
                if (!String.IsNullOrEmpty(searchWord) && match.Groups["search"].Success)
                {
                    if (searchIndex >= 0 && match.Index == searchIndex)
                    {
                        return "<mark=#ffffff22>" + match.Value + "</mark>";
                    }
                    return "<mark=#ffffff11>" + match.Value + "</mark>";
                }
                
                // there was some extra search check here, i don't think it does anything though so i removed it :p
                // if search is broken, it's this thing's fault

                foreach ((string name, string color) in ThemeManager.Colors())
                {
                    if (match.Groups[name].Success)
                    {
                        return $"<color={color}>{match.Value}</color>";
                    }
                }
                
                return match.Value; // No highlight
            }
        );
        return false;
    }
}