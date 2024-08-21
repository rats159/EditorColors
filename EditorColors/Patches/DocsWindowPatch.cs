using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(DocsWindow), nameof(DocsWindow.LoadDoc))]
internal class DocsWindowPatch
{
    public static void Postfix(DocsWindow __instance)
    {
        ThemeManager.UpdateDocsWindow(__instance);
    }
}
