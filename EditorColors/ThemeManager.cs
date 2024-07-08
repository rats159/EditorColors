using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EditorColors;

public class ColorGroup
{
    public static Regex
        COMMENT = new("#.*"),
        IMPORT = new("^(import|(from.*import)).*"),
        STRING = new("(['\"])(.*?)\\1"),
        KEYWORD = new("\\b(in|for|while|def|if|else|elif|return|pass|break|continue|and|or|not)\\b"),
        FUNCTION_CALL = new("\\b[a-z,A-Z]\\w*(?=\\()"),
        CONSTANT = new("\\b(True|False|None|North|East|South|West|Entities|Items|Grounds|Unlocks)|(?<=[a-zA-Z]\\.)\\w*"),
        NUMBER = new("\\b(\\d*\\.)?\\d+\\b"),
        BRACKETS = new("[\\[\\]{}()]"),
        OPERATORS = new("[+\\-*%/=!:><]");
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
        [ColorGroup.OPERATORS] = Configuration.Get("OperatorColor")
    };

    public static List<KeyValuePair<Regex, string>> Colors()
    {
        return colors.ToList();
    }
    
    public static void SetColor(Regex group, string hexCode)
    {
        colors[group] = hexCode;
    }

    public static Color ToColor(string hex)
    {
        if (hex.Length != 7)
        {
            return new Color(0, 0, 0);
        }

        byte r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 256f, g / 256f, b / 256f, 1f);
    }

    public static void UpdateWindows(CodeWindow[] windows)
    {
        foreach (CodeWindow window in windows)
        {
            UpdateWindow(window);
        }
    }

    public static void UpdateWindow(CodeWindow window)
    {

        window.GetComponent<Image>().color = ToColor(Configuration.Get("WindowBorderColor"));
        window.transform.Find("InputField").GetComponent<Image>().color = ToColor(Configuration.Get("CodeBackgroundColor"));
        window.transform.Find("BreakPointPanel").GetComponent<Image>().color = ToColor(Configuration.Get("BreakpointBackgroundColor"));
    }
}
