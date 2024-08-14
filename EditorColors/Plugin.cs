using BepInEx;
using HarmonyLib;


namespace EditorColors;

[BepInPlugin(GUID, "Editor Colors", "1.0.4")]
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
}

