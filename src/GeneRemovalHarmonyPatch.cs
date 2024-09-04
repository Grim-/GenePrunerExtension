using HarmonyLib;
using Verse;

namespace LichGenePatch
{
    [StaticConstructorOnStartup]
    public static class GeneRemovalHarmonyPatch
    {
        static GeneRemovalHarmonyPatch()
        {
            var harmony = new Harmony("com.yourname.generemoval");
            harmony.PatchAll();
        }
    }
}
