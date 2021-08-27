using HarmonyLib;
using UnityEngine;
using Verse;

namespace Safely_Hidden_Away
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            // initialize settings
            GetSettings<Settings>();
#if DEBUG
            Harmony.DEBUG = true;
#endif
            var harmony = new Harmony("uuugggg.rimworld.SafelyHiddenAway.main");

            harmony.PatchAll();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<Settings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "TD.SafelyHiddenAway".Translate();
        }
    }
}