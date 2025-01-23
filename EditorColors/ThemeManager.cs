using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;

namespace EditorColors;

public static class ColorGroup
{
    public static readonly Regex
        Comment = new(@"#.*"),
        String = new(@"(['""])(.*?)\1"),
        Keyword = new(@"\b(in|for|while|def|if|else|elif|return|pass|break|continue|and|or|not|import|from)\b"),
        FunctionCall = new(@"\b[a-zA-Z_]\w*(?=\()"),
        Constant = new(@"\b(True|False|None|North|East|South|West|Entities|Items|Grounds|Unlocks|Hats|Leaderboards)|(?<=[a-zA-Z]\.)\w*"),
        Number = new(@"\b(\d*\.)?\d+\b"),
        Brackets = new(@"[\[\]{}()]"),
        Operators = new(@"[+\-*%/=!:><,\.]"), 
        Identifiers = new(@"\b[a-zA-Z]\w*\b");
}

public static class ThemeManager
{
    private static readonly Dictionary<Regex, string> ColorDictionary = new()
    {
        [ColorGroup.Comment] = Configuration.Get("CommentColor"),
        [ColorGroup.String] = Configuration.Get("StringColor"),
        [ColorGroup.Keyword] = Configuration.Get("KeywordColor"),
        [ColorGroup.FunctionCall] = Configuration.Get("FunctionColor"),
        [ColorGroup.Constant] = Configuration.Get("ConstantColor"),
        [ColorGroup.Number] = Configuration.Get("NumberColor"),
        [ColorGroup.Brackets] = Configuration.Get("BracketColor"),
        [ColorGroup.Operators] = Configuration.Get("OperatorColor"),
        [ColorGroup.Identifiers] = Configuration.Get("IdentifierColor")
    };

    public static List<(Regex, string)> Colors()
    {
        return ThemeManager.ColorDictionary.Select(entry=>(entry.Key,entry.Value)).ToList();
    }
    
    public static void SetColor(Regex group, string hexCode)
    {
        ThemeManager.ColorDictionary[group] = hexCode;
    }

    public static Color ToColor(string hex)
    {
        if (hex.Length != 7)
        {
            return new Color32(0, 0, 0, 0);
        }

        byte r = Byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
        byte g = Byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
        byte b = Byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }

    public static void UpdateCodeWindows(CodeWindow[] windows)
    {
        foreach (CodeWindow window in windows)
        {
            ThemeManager.UpdateCodeWindow(window);
        }
    }

    public static void UpdateDocsWindows(DocsWindow[] windows)
    {
        Root.GetLogger().LogInfo(windows);
        foreach (DocsWindow window in windows)
        {
            ThemeManager.UpdateDocsWindow(window);
        }
    }

    public static void UpdateDocsWindow(DocsWindow window)
    {
        window.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("DocsWindowBorderColor"));

        Transform scrollView = window.transform.Find("Scroll View");
        scrollView.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("DocsBackgroundColor"));

        Transform vertical = scrollView.Find("Scrollbar Vertical");
        vertical.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("DocsScrollBackgroundColor"));

        vertical.Find("Sliding Area/Handle").GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("DocsScrollbarColor"));
    }

    public static void UpdateCodeWindow(CodeWindow window)
    {

        window.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("CodeWindowBorderColor"));
        window.transform.Find("InputField").GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("CodeBackgroundColor"));
        window.transform.Find("BreakPointPanel").GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("BreakpointBackgroundColor"));
    }
}
