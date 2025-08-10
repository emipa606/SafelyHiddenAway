using System.Linq;
using RimWorld;
using Verse;

namespace Safely_Hidden_Away;

[StaticConstructorOnStartup]
internal static class VistorDelayer
{
    static VistorDelayer()
    {
        foreach (var incidentDef in DefDatabase<IncidentDef>.AllDefs.Where(id =>
                     id.category == IncidentCategoryDefOf.FactionArrival))
        {
            incidentDef.tags ??= [];

            incidentDef.tags.Add("VisitorDelayable");
            incidentDef.refireCheckTags ??= [];

            incidentDef.refireCheckTags.Add("VisitorDelayable");
        }
    }
}