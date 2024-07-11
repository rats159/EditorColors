using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorColors;

[BepInPlugin(GUID, "Editor Colors", "1.0.1")]
public class Root : BaseUnityPlugin
{
    private const string GUID = "editor_colors";

    private readonly Harmony harmony = new(GUID);

    public static Root instance;

    public ManualLogSource mls;

    void Awake()
    {
        instance = this;
        mls = BepInEx.Logging.Logger.CreateLogSource(GUID);
        Configuration.Init();
        harmony.PatchAll();
    }

    public void UpdateWindows()
    {
        ThemeManager.UpdateWindows(FindObjectsOfType<CodeWindow>());
    }
}

