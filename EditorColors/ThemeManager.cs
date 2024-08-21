using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;

namespace EditorColors;

public class ColorGroup
{
    public static Regex
        COMMENT = new("#.*"),
        IMPORT = new("^(import|(from.*import)).*"),
        STRING = new("(['\"])(.*?)\\1"),
        KEYWORD = new("\\b(in|for|while|def|if|else|elif|return|pass|break|continue|and|or|not)\\b"),
        FUNCTION_CALL = new("\\b[a-zA-Z]\\w*(?=\\()"),
        CONSTANT = new("\\b(True|False|None|North|East|South|West|Entities|Items|Grounds|Unlocks)|(?<=[a-zA-Z]\\.)\\w*"),
        NUMBER = new("\\b(\\d*\\.)?\\d+\\b"),
        BRACKETS = new("[\\[\\]{}()]"),
        OPERATORS = new("[+\\-*%/=!:><,\\.]"), 
        IDENTIFIERS = new("\\b[a-zA-Z]\\w*\\b");
}

internal class ThemeManager
{
    private readonly static Dictionary<Regex, string> colors = new()
    {
        [ColorGroup.COMMENT] = Configuration.Get("CommentColor"),
        [ColorGroup.IMPORT] = Configuration.Get("ImportColor"),
        [ColorGroup.STRING] = Configuration.Get("StringColor"),
        [ColorGroup.KEYWORD] = Configuration.Get("KeywordColor"),
        [ColorGroup.FUNCTION_CALL] = Configuration.Get("FunctionColor"),
        [ColorGroup.CONSTANT] = Configuration.Get("ConstantColor"),
        [ColorGroup.NUMBER] = Configuration.Get("NumberColor"),
        [ColorGroup.BRACKETS] = Configuration.Get("BracketColor"),
        [ColorGroup.OPERATORS] = Configuration.Get("OperatorColor"),
        [ColorGroup.IDENTIFIERS] = Configuration.Get("IdentifierColor")
    };

    public static List<(Regex, string)> Colors()
    {
        return colors.Select(entry=>(entry.Key,entry.Value)).ToList();
    }
    
    public static void SetColor(Regex group, string hexCode)
    {
        colors[group] = hexCode;
    }

    public static Color ToColor(string hex)
    {
        if (hex.Length != 7)
        {
            return new Color32(0, 0, 0, 0);
        }

        byte r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }

    public static void UpdateCodeWindows(CodeWindow[] windows)
    {
        foreach (CodeWindow window in windows)
        {
            UpdateCodeWindow(window);
        }
    }

    public static void UpdateDocsWindows(DocsWindow[] windows)
    {
        Root.GetLogger().LogInfo(windows);
        foreach (DocsWindow window in windows)
        {
            UpdateDocsWindow(window);
        }
    }

    public static void UpdateDocsWindow(DocsWindow window)
    {
        window.GetComponent<Image>().color = ToColor(Configuration.Get("DocsWindowBorderColor"));

        var scrollView = window.transform.Find("Scroll View");
        scrollView.GetComponent<Image>().color = ToColor(Configuration.Get("DocsBackgroundColor"));

        var vertical = scrollView.Find("Scrollbar Vertical");
        vertical.GetComponent<Image>().color = ToColor(Configuration.Get("DocsScrollBackgroundColor"));

        vertical.Find("Sliding Area/Handle").GetComponent<Image>().color = ToColor(Configuration.Get("DocsScrollbarColor"));
    }

    public static void UpdateCodeWindow(CodeWindow window)
    {

        window.GetComponent<Image>().color = ToColor(Configuration.Get("CodeWindowBorderColor"));
        window.transform.Find("InputField").GetComponent<Image>().color = ToColor(Configuration.Get("CodeBackgroundColor"));
        window.transform.Find("BreakPointPanel").GetComponent<Image>().color = ToColor(Configuration.Get("BreakpointBackgroundColor"));
    }
}
