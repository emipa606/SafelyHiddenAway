﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace Safely_Hidden_Away
{
    [HarmonyPatch(typeof(StoryState))]
    [HarmonyPatch("Notify_IncidentFired")]
    internal class CycleDampener
    {
        //StoryState
        public static void Postfix(StoryState __instance, FiringIncident fi, IIncidentTarget ___target,
            ref int ___lastThreatBigTick)
        {
            if (fi.parms.forced || fi.parms.target != ___target)
            {
                return;
            }

            if (!string.IsNullOrEmpty(fi.parms.questTag))
            {
                return;
            }

            if (fi.parms.target is not Map map)
            {
                return;
            }

            var ally = fi.def.category == IncidentCategoryDefOf.FactionArrival;
            var raid = fi.def.category == IncidentCategoryDefOf.ThreatBig;
            if (!ally && !raid)
            {
                return;
            }

            var delayDays = ally ? DelayDays.DelayAllyDays(map) : DelayDays.DelayRaidDays(map);

            var eventDesc = ally ? "visitors" : "threats";

            if (!(delayDays > 0))
            {
                return;
            }

            __instance.lastFireTicks[fi.def] += (int)(delayDays * GenDate.TicksPerDay);

            if (Settings.Get().logResults)
            {
                var date = GenDate.QuadrumDateStringAt(GenTicks.TicksGame, 0);
                Verse.Log.Message(
                    $"On {date}, Safely Hidden Away delayed {eventDesc} to {map.info.parent.LabelShortCap} by {delayDays:0.0} days.");
            }

            if (!raid)
            {
                return;
            }

            var last = ___lastThreatBigTick + (int)(delayDays * GenDate.TicksPerDay);

            ___lastThreatBigTick = last;
        }
    }
}