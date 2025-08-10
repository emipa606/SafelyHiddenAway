using System.Reflection;
using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace Safely_Hidden_Away;

public class SafelyHiddenAwayMod : Mod
{
    public static Settings Settings;
    public static string CurrentVersion;

    public SafelyHiddenAwayMod(ModContentPack content) : base(content)
    {
        // initialize settings
        Settings = GetSettings<Settings>();

        new Harmony("uuugggg.rimworld.SafelyHiddenAway.main").PatchAll(Assembly.GetExecutingAssembly());
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        Settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "TD.SafelyHiddenAway".Translate();
    }
}