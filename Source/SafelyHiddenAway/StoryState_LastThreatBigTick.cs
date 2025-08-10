using HarmonyLib;
using RimWorld;

namespace Safely_Hidden_Away;

[HarmonyPatch(typeof(StoryState), nameof(StoryState.LastThreatBigTick), MethodType.Getter)]
internal class StoryState_LastThreatBigTick
{
    //public int LastThreatBigTick
    public static bool Prefix(ref int __result, int ___lastThreatBigTick)
    {
        __result = ___lastThreatBigTick;
        return false;
    }
}