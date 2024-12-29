using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace EditorColors;

[BepInPlugin(Root.GUID, "Editor Colors", "1.5.0")]
public class Root : BaseUnityPlugin
{
    private const string GUID = "dev.rats159.tfwr.editor_colors";
    private readonly Harmony _harmony = new(Root.GUID);
    public static Root Instance { get; private set; }

    public static ManualLogSource GetLogger() => Root.Instance.Logger;

    public void Awake()
    {
        Root.Instance = this;
        Configuration.Init();
        this._harmony.PatchAll();
    }

    public void UpdateWindows()
    {
        ThemeManager.UpdateCodeWindows(UnityEngine.Object.FindObjectsOfType<CodeWindow>());
        ThemeManager.UpdateDocsWindows(UnityEngine.Object.FindObjectsOfType<DocsWindow>());
    }

    public static void SetSkyColor(string value)
    {
        UnityEngine.Object.FindObjectOfType<Camera>().backgroundColor = ThemeManager.ToColor(value);
    }

    public static void SetSunColor(string value)
    {
        // This is a bit hacky but i dont know how else to get the sun
        UnityEngine.Object.FindObjectOfType<Light>().color = ThemeManager.ToColor(value);
    }
}

