using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EditorColors;

internal class Configuration
{
    private static readonly Dictionary<string, ConfigEntry<string>> Settings = [];
    public static void Init()
    {
        Settings["CommentColor"] = Bind("Syntax", "CommentColor", "#ffffff", "The color of comments");
        Settings["ImportColor"] = Bind("Syntax", "ImportColor", "#ffffff", "The color of imports");
        Settings["StringColor"] = Bind("Syntax", "StringColor", "#ffffff", "The color of strings");
        Settings["KeywordColor"] = Bind("Syntax", "KeywordColor", "#ffffff", "The color of keywords");
        Settings["FunctionColor"] = Bind("Syntax", "FunctionColor", "#ffffff", "The color of functions");
        Settings["ConstantColor"] = Bind("Syntax", "ConstantColor", "#ffffff", "The color of constants");
        Settings["NumberColor"] = Bind("Syntax", "NumberColor", "#ffffff", "The color of numbers");
        Settings["BracketColor"] = Bind("Syntax", "BracketColor", "#ffffff", "The color of brackets");
        Settings["OperatorColor"] = Bind("Syntax", "OperatorColor", "#ffffff", "The color of operators");
        SetupSyntaxCallbacks();

        Settings["WindowBorderColor"] = Bind("Window", "BorderColor", "#000000", "The color of the window border");
        Settings["CodeBackgroundColor"] = Bind("Window", "CodeBackgroundColor", "#000000", "The color of the code background");
        Settings["BreakpointBackgroundColor"] = Bind("Window", "BreakpointBackgroundColor", "#000000", "The color of the breakpoint background");
        SetupWindowCallbacks();
    }

    public static string Get(string key)
    {
        return Settings[key].Value;
    }

    public static void SetupSyntaxCallbacks()
    {
        MakeSyntaxCallback("CommentColor", ColorGroup.COMMENT);
        MakeSyntaxCallback("ImportColor", ColorGroup.IMPORT);
        MakeSyntaxCallback("StringColor", ColorGroup.STRING);
        MakeSyntaxCallback("KeywordColor", ColorGroup.KEYWORD);
        MakeSyntaxCallback("FunctionColor", ColorGroup.FUNCTION_CALL);
        MakeSyntaxCallback("ConstantColor", ColorGroup.CONSTANT);
        MakeSyntaxCallback("NumberColor", ColorGroup.NUMBER);
        MakeSyntaxCallback("BracketColor", ColorGroup.BRACKETS);
        MakeSyntaxCallback("OperatorColor", ColorGroup.OPERATORS);
    }

    public static void SetupWindowCallbacks()
    {
        MakeWindowCallback("WindowBorderColor");
        MakeWindowCallback("CodeBackgroundColor");
        MakeWindowCallback("BreakpointBackgroundColor");
    }

    private static void MakeWindowCallback(string v)
    {
        Settings[v].SettingChanged += (caller, args) =>
        {
            Root.instance.UpdateWindows();
        };
    }

    private static ConfigEntry<T> Bind<T>(string group, string name, T defaultVal, string desc)
    {
        return Root.instance.Config.Bind(group, name, defaultVal, desc);
    }

    private static void MakeSyntaxCallback(string name, Regex re)
    {
        Settings[name].SettingChanged += (sender, args) =>
        {
            ThemeManager.SetColor(re, Settings[name].Value);
        };
    }
}
