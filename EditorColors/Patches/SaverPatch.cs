using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorColors.Patches;

[HarmonyPatch(typeof(Saver),nameof(Saver.Load))]
internal class SaverPatch
{
    public static void Postfix()
    {
        Root.SetSkyColor(Configuration.Get("SkyColor"));
        Root.SetSunColor(Configuration.Get("SunColor"));
    }
}
