using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace EditorColors;

[BepInPlugin(GUID, "Editor Colors", "1.3.0")]
public class Root : BaseUnityPlugin
{
    private const string GUID = "dev.rats159.tfwr.editor_colors";
    private readonly Harmony harmony = new(GUID);
    public static Root Instance { get; private set; }

    public static ManualLogSource GetLogger() => Instance.Logger;

    public void Awake()
    {
        Instance = this;
        Configuration.Init();
        harmony.PatchAll();
    }

    public void UpdateWindows()
    {
        ThemeManager.UpdateCodeWindows(FindObjectsOfType<CodeWindow>());
        ThemeManager.UpdateDocsWindows(FindObjectsOfType<DocsWindow>());
    }

    public static void SetSkyColor(string value)
    {
        FindObjectOfType<Camera>().backgroundColor = ThemeManager.ToColor(value);
    }

    public static void SetSunColor(string value)
    {
        // This is a bit hacky but i dont know how else to get the sun
        FindObjectOfType<Light>().color = ThemeManager.ToColor(value);
    }
}

