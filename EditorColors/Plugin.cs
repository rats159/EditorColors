using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace EditorColors;

[BepInPlugin(GUID, "Editor Colors", "1.1.4")]
public class Root : BaseUnityPlugin
{
    private const string GUID = "dev.rats159.tfwr.editor_colors";
    private readonly Harmony harmony = new(GUID);
    public static Root Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        Configuration.Init();
        harmony.PatchAll();
    }

    public void UpdateWindows()
    {
        ThemeManager.UpdateWindows(FindObjectsOfType<CodeWindow>());
    }

    public static void SetSkyColor(string value)
    {
        FindObjectOfType<Camera>().backgroundColor = ThemeManager.ToColor(value);
    }

    public static void SetSunColor(string value)
    {
        FindObjectOfType<Light>().color = ThemeManager.ToColor(value);
    }
}

