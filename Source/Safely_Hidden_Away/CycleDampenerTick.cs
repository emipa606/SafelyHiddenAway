using HarmonyLib;
using RimWorld;

namespace Safely_Hidden_Away
{
    [HarmonyPatch(typeof(StoryState), "LastThreatBigTick", MethodType.Getter)]
    internal class CycleDampenerTick
    {
        //public int LastThreatBigTick
        public static bool Prefix(StoryState __instance, ref int __result, int ___lastThreatBigTick)
        {
            __result = ___lastThreatBigTick;
            return false;
        }
    }
}