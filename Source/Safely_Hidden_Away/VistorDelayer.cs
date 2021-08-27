using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Safely_Hidden_Away
{
    [StaticConstructorOnStartup]
    internal static class VistorDelayer
    {
        static VistorDelayer()
        {
            foreach (var idef in DefDatabase<IncidentDef>.AllDefs.Where(id =>
                id.category == IncidentCategoryDefOf.FactionArrival))
            {
                if (idef.tags == null)
                {
                    idef.tags = new List<string>();
                }

                idef.tags.Add("VisitorDelayable");
                if (idef.refireCheckTags == null)
                {
                    idef.refireCheckTags = new List<string>();
                }

                idef.refireCheckTags.Add("VisitorDelayable");
            }
        }
    }
}