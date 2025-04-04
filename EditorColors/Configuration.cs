﻿using BepInEx.Configuration;
using System.Collections.Generic;

namespace EditorColors;

internal static class Configuration
{
    private static readonly Dictionary<string, ConfigEntry<string>> Settings = [];
    public static void Init()
    {
        Configuration.Settings["CommentColor"] = Configuration.Bind("Syntax", "CommentColor", "#999999", "The color of comments");
        Configuration.Settings["StringColor"] = Configuration.Bind("Syntax", "StringColor", "#cc8a43", "The color of strings");
        Configuration.Settings["KeywordColor"] = Configuration.Bind("Syntax", "KeywordColor", "#e3a63d", "The color of keywords");
        Configuration.Settings["FunctionColor"] = Configuration.Bind("Syntax", "FunctionColor", "#ffecbd", "The color of functions");
        Configuration.Settings["ConstantColor"] = Configuration.Bind("Syntax", "ConstantColor", "#efc678", "The color of constants");
        Configuration.Settings["NumberColor"] = Configuration.Bind("Syntax", "NumberColor", "#efc678", "The color of numbers");
        Configuration.Settings["BracketColor"] = Configuration.Bind("Syntax", "BracketColor", "#ffffff", "The color of brackets");
        Configuration.Settings["OperatorColor"] = Configuration.Bind("Syntax", "OperatorColor", "#ffffff", "The color of operators");
        Configuration.Settings["IdentifierColor"] = Configuration.Bind("Syntax", "IdentifierColor", "#ffffff", "The color of identifiers. This has the lowest precedence.");
        Configuration.SetupSyntaxCallbacks();

        Configuration.Settings["BorderColor"] = Configuration.Bind("Window", "BorderColor", "#565656", "The color of the code window border");
        Configuration.Settings["BackgroundColor"] = Configuration.Bind("Window", "BackgroundColor", "#2E2E2E", "The color of the code background");
        Configuration.Settings["BreakpointBackgroundColor"] = Configuration.Bind("Window", "BreakpointBackgroundColor", "#2E2E2E", "The color of the breakpoint background");
        Configuration.Settings["ScrollbarColor"] = Configuration.Bind("Window", "ScrollbarColor", "#414141", "The color of the window scrollbar");
        Configuration.Settings["ScrollBackgroundColor"] = Configuration.Bind("Window", "ScrollBackgroundColor", "#2B2B2B", "The color of the window scroll background");
        Configuration.SetupCodeWindowCallbacks();

        Configuration.Settings["SkyColor"] = Configuration.Bind("Environment", "SkyColor", "#85AFDB", "The color of the sky");
        Configuration.Settings["SunColor"] = Configuration.Bind("Environment", "SunColor", "#FFE292", "The color of the sunlight");
        Configuration.SetupEnvironmentCallbacks();
    }

    public static string Get(string key)
    {
        return Configuration.Settings[key].Value;
    }

    private static void SetupSyntaxCallbacks()
    {
        Configuration.MakeSyntaxCallback("CommentColor", "comment");
        Configuration.MakeSyntaxCallback("StringColor", "string");
        Configuration.MakeSyntaxCallback("KeywordColor", "keyword");
        Configuration.MakeSyntaxCallback("FunctionColor", "function");
        Configuration.MakeSyntaxCallback("ConstantColor", "constant");
        Configuration.MakeSyntaxCallback("NumberColor", "number");
        Configuration.MakeSyntaxCallback("BracketColor", "brackets");
        Configuration.MakeSyntaxCallback("OperatorColor", "operators");
        Configuration.MakeSyntaxCallback("IdentifierColor", "identifiers");
    }

    private static void SetupEnvironmentCallbacks()
    {
        Configuration.Settings["SkyColor"].SettingChanged += (_, _) =>
        {
            Root.SetSkyColor(Configuration.Settings["SkyColor"].Value);
        };

        Configuration.Settings["SunColor"].SettingChanged += (_, _) =>
        {
            Root.SetSunColor(Configuration.Settings["SunColor"].Value);
        };
    }

    private static void SetupCodeWindowCallbacks()
    {
        Configuration.MakeWindowCallback("BorderColor");
        Configuration.MakeWindowCallback("BackgroundColor");
        Configuration.MakeWindowCallback("BreakpointBackgroundColor");
        Configuration.MakeWindowCallback("ScrollbarColor");
        Configuration.MakeWindowCallback("ScrollBackgroundColor");
    }

    private static void MakeWindowCallback(string v)
    {
        Configuration.Settings[v].SettingChanged += (_,_) => Root.Instance.UpdateWindows();
        
    }

    private static ConfigEntry<T> Bind<T>(string group, string name, T defaultVal, string desc)
    {
        return Root.Instance.Config.Bind(group, name, defaultVal, desc);
    }

    private static void MakeSyntaxCallback(string name, string re)
    {
        Configuration.Settings[name].SettingChanged += (_, _) =>
        {
            ThemeManager.SetColor(re, Configuration.Settings[name].Value);
        };
    }
}
