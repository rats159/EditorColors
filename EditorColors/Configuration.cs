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
        Settings["CommentColor"] = Bind("Syntax", "CommentColor", "#999999", "The color of comments");
        Settings["ImportColor"] = Bind("Syntax", "ImportColor", "#999999", "The color of imports");
        Settings["StringColor"] = Bind("Syntax", "StringColor", "#cc8a43", "The color of strings");
        Settings["KeywordColor"] = Bind("Syntax", "KeywordColor", "#e3a63d", "The color of keywords");
        Settings["FunctionColor"] = Bind("Syntax", "FunctionColor", "#ffecbd", "The color of functions");
        Settings["ConstantColor"] = Bind("Syntax", "ConstantColor", "#efc678", "The color of constants");
        Settings["NumberColor"] = Bind("Syntax", "NumberColor", "#efc678", "The color of numbers");
        Settings["BracketColor"] = Bind("Syntax", "BracketColor", "#ffffff", "The color of brackets");
        Settings["OperatorColor"] = Bind("Syntax", "OperatorColor", "#ffffff", "The color of operators");
        SetupSyntaxCallbacks();

        Settings["WindowBorderColor"] = Bind("Window", "BorderColor", "#565656", "The color of the window border");
        Settings["CodeBackgroundColor"] = Bind("Window", "CodeBackgroundColor", "#2E2E2E", "The color of the code background");
        Settings["BreakpointBackgroundColor"] = Bind("Window", "BreakpointBackgroundColor", "#2E2E2E", "The color of the breakpoint background");
        SetupWindowCallbacks();

        Settings["SkyColor"] = Bind("Environment", "SkyColor", "#85AFDB", "The color of the sky");
        Settings["SunColor"] = Bind("Environment", "SunColor", "#FFE292", "The color of the sunlight");
        SetupEnvironmentCallbacks();
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

    public static void SetupEnvironmentCallbacks()
    {
        Settings["SkyColor"].SettingChanged += (caller, args) =>
        {
            Root.SetSkyColor(Settings["SkyColor"].Value);
        };

        Settings["SunColor"].SettingChanged += (caller, args) =>
        {
            Root.SetSunColor(Settings["SunColor"].Value);
        };
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
            Root.Instance.UpdateWindows();
        };
    }

    private static ConfigEntry<T> Bind<T>(string group, string name, T defaultVal, string desc)
    {
        return Root.Instance.Config.Bind(group, name, defaultVal, desc);
    }

    private static void MakeSyntaxCallback(string name, Regex re)
    {
        Settings[name].SettingChanged += (sender, args) =>
        {
            ThemeManager.SetColor(re, Settings[name].Value);
        };
    }
}
