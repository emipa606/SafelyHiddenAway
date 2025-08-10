using HarmonyLib;
using RimWorld;
using TD.Utilities;
using UnityEngine;
using Verse;

namespace Safely_Hidden_Away;

public class Settings : ModSettings
{
    private static readonly AccessTools.FieldRef<StoryState, int> lastThreatBigTick =
        AccessTools.FieldRefAccess<StoryState, int>("lastThreatBigTick");

    public float DistanceFactor = 0.2f;
    public float IslandAddedDays = 5.0f;

    public float IsolatedMountainValleyDays = 10.0f;

    //TODO: save per map
    public bool LOGResults;
    private float scrollHeight;

    private Vector2 scrollPos;
    public float VisitDiminishingFactor = 2.0f;
    public float WealthCurvy = 10f;
    public float WealthFactor = 1f;

    public float WealthLimit = 200000;
    public float WealthMax = 600000;

    public void DoWindowContents(Rect rect)
    {
        var viewRect = rect.AtZero();
        viewRect.height = scrollHeight;
        viewRect.width -= 16;

        Widgets.BeginScrollView(rect, ref scrollPos, viewRect);


        var options = new Listing_Standard { maxOneColumn = true };
        options.Begin(viewRect);

        options.Label("TD.SettingIslandDays".Translate() + $"{IslandAddedDays:0.0}");
        IslandAddedDays = options.Slider(IslandAddedDays, 0f, 20f);

        options.Label("TD.SettingMountainDays".Translate() + $"{IsolatedMountainValleyDays:0.0}");
        IsolatedMountainValleyDays = options.Slider(IsolatedMountainValleyDays, 0f, 30f);

        options.Label("TD.SettingRemotenessSpeed".Translate());
        DistanceFactor = options.Slider(DistanceFactor, .05f, .5f);

        options.Label("TD.SettingRemotenessFactor".Translate() + $"{VisitDiminishingFactor:0.0}x");
        VisitDiminishingFactor = options.Slider(VisitDiminishingFactor, 0.1f, 5);

        var graphLine = options.GetRect(Text.LineHeight * 12);
        var graphRect = graphLine.LeftPartPixels(graphLine.height);

        TDWidgets.DrawGraph(graphRect, "TD.DaysTravel".Translate(), "TD.DaysAdded".Translate(), "{0:0}", "{0:0}", 0, 10,
            DelayDays.AddedDays, null, null, 5);

        var map = Find.CurrentMap;
        if (map != null && (Prefs.DevMode || Harmony.DEBUG)) //That's one roundabout way to check DEBUG
        {
            var gameTicks = GenTicks.TicksGame;
            options.Label("TD.GameTicks".Translate() + gameTicks);

            var lastTick = lastThreatBigTick(map.storyState);
            options.Label("TD.ForMap".Translate() + map.info.parent.LabelShortCap);
            options.Label("TD.BigThreatsDelayed".Translate() + lastTick);

            var days = (lastTick - gameTicks).TicksToDays();
            if (days >= 0)
            {
                options.Label("TD.XDaysInFuture".Translate(days));
            }

            if (options.ButtonText("TD.ResetToNOW".Translate()))
            {
                lastThreatBigTick(map.storyState) = GenTicks.TicksGame;
            }

            options.Label(string.Format("TD.ThreatWillDelay".Translate(), DelayDays.DelayRaidDays(map)));
            options.Label(string.Format("TD.GuestWillDelay".Translate(), DelayDays.DelayAllyDays(map)));
        }

        options.CheckboxLabeled("TD.WriteLogs".Translate(), ref LOGResults);

        options.Label(string.Format("TD.SettingMinimumWealth".Translate(), WealthLimit));
        WealthLimit = options.Slider(WealthLimit, 1, 1000000);

        options.Label(string.Format("TD.SettingMaximumWealth".Translate(), WealthMax));
        WealthMax = options.Slider(WealthMax, 1, 1000000);

        options.Label(string.Format("TD.SettingWealthFactor".Translate(), WealthFactor));
        WealthFactor = options.Slider(WealthFactor, 0f, 1f);

        options.Label("TD.SettingCurvinessFactor".Translate());
        WealthCurvy = options.Slider(WealthCurvy, 0f, 50f);


        graphLine = options.GetRect(Text.LineHeight * 16);
        graphRect = graphLine.LeftPartPixels(graphLine.height);
        TDWidgets.DrawGraph(graphRect, "TD.ColonyWealth".Translate(), "TD.PercentDelayReduced".Translate(), "{0:0}",
            "{0:P0}", 0, 1000000, DelayDays.WealthReduction, 0, 1f);

        if (SafelyHiddenAwayMod.CurrentVersion != null)
        {
            options.Gap();
            GUI.contentColor = Color.gray;
            options.Label("TD.CurrentModVersion".Translate(SafelyHiddenAwayMod.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        scrollHeight = options.CurHeight;
        options.End();
        Widgets.EndScrollView();
    }


    public override void ExposeData()
    {
        Scribe_Values.Look(ref LOGResults, "logResults", true);
        Scribe_Values.Look(ref IslandAddedDays, "islandAddedDays", 5f);
        Scribe_Values.Look(ref IsolatedMountainValleyDays, "isolatedMountainValleyDays", 10f);
        Scribe_Values.Look(ref DistanceFactor, "distanceFactor", 0.2f);
        Scribe_Values.Look(ref VisitDiminishingFactor, "threatDiminshingFactor", 2.0f);

        Scribe_Values.Look(ref WealthLimit, "wealthLimit", 200000);
        Scribe_Values.Look(ref WealthMax, "wealthMax", 600000);
        Scribe_Values.Look(ref WealthFactor, "wealthSpeed", 1f);
        Scribe_Values.Look(ref WealthCurvy, "wealthCurvy", 10f);
    }
}