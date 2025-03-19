using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;

namespace EditorColors;

public static class ColorGroup
{
    public static readonly string Pattern;

    // Association list because order is important
    public static readonly AssociationList<string, string> Groups = new()
    {
        ["comment"] = "#.*",
        ["string"] = @"'[^']*'|""[^""]*""",
        ["keyword"] = @"\b(?:in|for|while|def|if|else|elif|return|pass|break|continue|and|or|not|global|import|from)\b",
        ["function"] = @"\b[a-zA-Z_]\w*(?=\()",
        ["constant"] =
            @"\b(?:True|False|None|North|East|South|West|Entities|Items|Grounds|Unlocks|Leaderboards|Hats)\b|(?<=[a-zA-Z_]\.)\w+",
        ["number"] = @"\b(?:\d*\.)?\d+\b",
        ["brackets"] = @"[\[\]{}()]",
        ["operators"] = @"[+\-*%/=!:><,\.]",
        ["identifiers"] = @"\b[a-zA-Z]\w*\b"
    };

    static ColorGroup()
    {
        MegaRegex group = new();

        foreach ((string name, string pattern) in ColorGroup.Groups)
        {
            group.Add(name, pattern);
        }

        ColorGroup.Pattern = group.Build();
    }
}

public static class ThemeManager
{
    public static List<(string, string)> Colors()
    {
        return ThemeManager.ColorDictionary.Select(entry => (entry.Key, entry.Value)).ToList();
    }

    private static readonly Dictionary<string, string> ColorDictionary = new()
    {
        ["comment"] = Configuration.Get("CommentColor"),
        ["string"] = Configuration.Get("StringColor"),
        ["keyword"] = Configuration.Get("KeywordColor"),
        ["function"] = Configuration.Get("FunctionColor"),
        ["constant"] = Configuration.Get("ConstantColor"),
        ["number"] = Configuration.Get("NumberColor"),
        ["brackets"] = Configuration.Get("BracketColor"),
        ["operators"] = Configuration.Get("OperatorColor"),
        ["identifiers"] = Configuration.Get("IdentifierColor")
    };

    public static void SetColor(string name, string hexCode)
    {
        ThemeManager.ColorDictionary[name] = hexCode;
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
        foreach (DocsWindow window in windows)
        {
            ThemeManager.UpdateDocsWindow(window);
        }
    }

    public static void UpdateDocsWindow(DocsWindow window)
    {
        window.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("BorderColor"));

        Transform scrollView = window.transform.Find("Scroll View");
        scrollView.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("BackgroundColor"));

        Transform vertical = scrollView.Find("Scrollbar Vertical");
        vertical.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("ScrollBackgroundColor"));

        vertical.Find("Sliding Area/Handle").GetComponent<Image>().color =
            ThemeManager.ToColor(Configuration.Get("ScrollbarColor"));
    }

    public static void UpdateCodeWindow(CodeWindow window)
    {
        if (window == null) return;

        window.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("BorderColor"));
        
        Transform scrollView = window.transform.Find("Scroll View");
        scrollView.GetComponent<Image>().color =  ThemeManager.ToColor(Configuration.Get("BackgroundColor"));
        
        Transform inputField = scrollView.Find("Viewport/InputField");
        Transform breakpoint = scrollView.Find("Viewport/InputField/BreakPointPanel");


        Transform vertical = scrollView.Find("Scrollbar Vertical");
        vertical.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("ScrollBackgroundColor"));
        
        vertical.Find("Sliding Area/Handle").GetComponent<Image>().color =
            ThemeManager.ToColor(Configuration.Get("ScrollbarColor"));
        
        inputField.GetComponent<Image>().color = ThemeManager.ToColor(Configuration.Get("BackgroundColor"));
        breakpoint.GetComponent<Image>().color =
            ThemeManager.ToColor(Configuration.Get("BreakpointBackgroundColor"));
    }
}