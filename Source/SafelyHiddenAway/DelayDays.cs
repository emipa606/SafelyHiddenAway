using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Safely_Hidden_Away;

internal class DelayDays
{
    private static readonly List<PlanetTile> neighborTiles = [];

    public static float DelayAllyDays(Map map)
    {
        return daysTo(map, f => !f.IsPlayer && !f.HostileTo(Faction.OfPlayer));
    }

    public static float DelayRaidDays(Map map)
    {
        return daysTo(map, f => f.HostileTo(Faction.OfPlayer));
    }

    private static float daysTo(Map map, Func<Faction, bool> factionValidator)
    {
        var tile = map.Tile;
        Func<WorldObject, bool> woValidator = wo =>
            (wo is Settlement || wo is Site s && s.parts.Any(part => part.def == SitePartDefOf.Outpost))
            && wo.Faction != null && factionValidator(wo.Faction);

        //bool factionBase = false, water = false; 
        if (!tryFindClosestTile(tile, t => !Find.World.Impassable(t), worldValidator, out var foundTile))
        {
            tryFindClosestTile(tile, t => !Find.World.Impassable(t) || waterValidator(t), waterValidator,
                out foundTile);
        }

        float daysTravel;
        if (foundTile < 0)
        {
            // separated from all enemy outposts and water: we're isolated in a mountain valley
            daysTravel = SafelyHiddenAwayMod.Settings.IsolatedMountainValleyDays;
        }
        else
        {
            var path = tile.Tile.Layer.Pather.FindPath(tile, foundTile, null);
            if (path.Found)
            {
                daysTravel = path.TotalCost / 40000; // Cost to days-ish
                path.ReleaseToPool();
            }
            else
            {
                // Probably ended up in water, so find adjacent land
                neighborTiles.Clear();
                Find.World.grid.GetTileNeighbors(foundTile, neighborTiles);
                var bestCost = float.MaxValue;
                foreach (var nTile in neighborTiles)
                {
                    if (nTile.Layer != tile.Layer)
                    {
                        continue;
                    }

                    path = tile.Tile.Layer.Pather.FindPath(tile, nTile, null);
                    if (path.Found)
                    {
                        bestCost = Math.Min(bestCost, path.TotalCost);
                    }

                    path.ReleaseToPool();
                }

                if (bestCost == float.MaxValue)
                {
                    bestCost = 0; //paranoid?
                }

                daysTravel = (bestCost / 40000) + SafelyHiddenAwayMod.Settings.IslandAddedDays;
            }
        }


        var wealth = map.wealthWatcher.WealthTotal;
        return AddedDays(daysTravel) * WealthReduction(wealth);

        static bool waterValidator(PlanetTile t)
        {
            return Find.World.grid[t].WaterCovered;
        }

        bool worldValidator(PlanetTile t)
        {
            return Find.World.worldObjects.ObjectsAt(t).Any(woValidator);
        }
    }


    private static bool tryFindClosestTile(PlanetTile rootTile, Predicate<PlanetTile> searchThrough,
        Predicate<PlanetTile> predicate, out PlanetTile foundTile, int maxTilesToScan = 2147483647)
    {
        PlanetTile foundTileLocal = -1;
        rootTile.Layer.Filler.FloodFill(rootTile, searchThrough, delegate(PlanetTile t)
        {
            var tileFound = predicate(t);
            if (tileFound)
            {
                foundTileLocal = t;
            }

            return tileFound;
        }, maxTilesToScan);
        foundTile = foundTileLocal;
        return foundTileLocal >= 0;
    }

    public static float AddedDays(float daysTravel)
    {
        //x-x/2^(.2x)^4 , .2 configurable
        double calc = daysTravel;
        calc *= SafelyHiddenAwayMod.Settings.DistanceFactor; //*.2 x
        calc *= calc;
        calc *= calc; //^4
        return SafelyHiddenAwayMod.Settings.VisitDiminishingFactor *
               (float)(daysTravel - (daysTravel / Math.Pow(2, calc)));
    }

    public static float WealthReduction(float w)
    {
        double l = SafelyHiddenAwayMod.Settings.WealthLimit;
        if (w <= l)
        {
            return 1.0f;
        }

        double m = SafelyHiddenAwayMod.Settings.WealthMax;
        var f = SafelyHiddenAwayMod.Settings.WealthFactor;
        if (w >= m)
        {
            return 1 - f;
        }

        double q = SafelyHiddenAwayMod.Settings.WealthCurvy;

        //don't ask what all this does.
        var h = (m - l) / 2; //halfway point
        var x1 = q * (((w - l) / h) - 1);
        var a = q == 0
            ? (w - l) / (2 * h)
            : 0.5 + (0.5 * x1 /
                     (q / (1 + q) * (1 + Math.Abs(x1))));

        return (float)(1 - (f * a));
    }
}