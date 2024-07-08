using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(CodeWindow), nameof(CodeWindow.Load))]
internal class CodeWindowPatch
{
    public static void Postfix(ref CodeWindow __instance)
    {
        ThemeManager.UpdateWindow(__instance);
    }
}
