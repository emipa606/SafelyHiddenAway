using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Safely_Hidden_Away
{
    internal class DelayDays
    {
        public static float DelayAllyDays(Map map)
        {
            return DaysTo(map, f => !f.IsPlayer && !f.HostileTo(Faction.OfPlayer));
        }

        public static float DelayRaidDays(Map map)
        {
            return DaysTo(map, f => f.HostileTo(Faction.OfPlayer));
        }

        public static float DaysTo(Map map, Func<Faction, bool> factionValidator)
        {
            var tile = map.Tile;

            bool WoValidator(WorldObject wo)
            {
                return (wo is Settlement || wo is Site s && s.parts.Any(part => part.def == SitePartDefOf.Outpost)) &&
                       wo.Faction != null && factionValidator(wo.Faction);
            }

            bool Validator(int t)
            {
                return Find.World.worldObjects.ObjectsAt(t).Any(WoValidator);
            }

            bool WaterValidator(int t)
            {
                return Find.World.grid[t].WaterCovered;
            }

            //bool factionBase = false, water = false; 
            if (!TryFindClosestTile(tile, t => !Find.World.Impassable(t), Validator, out var foundTile))
            {
                TryFindClosestTile(tile, t => !Find.World.Impassable(t) || WaterValidator(t), WaterValidator,
                    out foundTile);
            }

            Log.Message($"Closest tile to {map} is {foundTile}:{Find.World.grid[foundTile]}");
            var path = Find.WorldPathFinder.FindPath(tile, foundTile, null);
            float cost;
            if (path.Found)
            {
                cost = path.TotalCost;
                Log.Message($"Path cost is {cost}");
                path.ReleaseToPool();
            }
            else
            {
                var neighborTiles = new List<int>();
                Find.World.grid.GetTileNeighbors(foundTile, neighborTiles);
                var bestCost = float.MaxValue;
                foreach (var nTile in neighborTiles)
                {
                    Log.Message($"Looking at neighbor tile {nTile}:{Find.World.grid[nTile]}");
                    path = Find.WorldPathFinder.FindPath(tile, nTile, null);
                    if (path.Found)
                    {
                        bestCost = Math.Min(bestCost, path.TotalCost);
                    }

                    Log.Message($"best cost is {bestCost}");
                    path.ReleaseToPool();
                }

                if (bestCost == float.MaxValue)
                {
                    bestCost = 0; //paranoid?
                }

                cost = bestCost + (Settings.Get().islandAddedDays * 40000);
                Log.Message($"cost after added island days: {cost}");
            }

            cost /= 40000; //Cost to days-ish

            var wealth = map.wealthWatcher.WealthTotal;
            return AddedDays(cost) * WealthReduction(wealth);
        }


        public static bool TryFindClosestTile(int rootTile, Predicate<int> searchThrough, Predicate<int> predicate,
            out int foundTile, int maxTilesToScan = 2147483647)
        {
            var foundTileLocal = -1;
            Find.WorldFloodFiller.FloodFill(rootTile, searchThrough, delegate(int t)
            {
                if (predicate(t))
                {
                    foundTileLocal = t;
                }

                return predicate(t);
            }, maxTilesToScan);
            foundTile = foundTileLocal;
            return foundTileLocal >= 0;
        }

        public static float AddedDays(float daysTravel)
        {
            //x-x/2^(.2x)^4 , .2 configurable
            double calc = daysTravel;
            calc *= Settings.Get().distanceFactor; //*.2 x
            calc *= calc;
            calc *= calc; //^4
            return Settings.Get().visitDiminishingFactor * (float)(daysTravel - (daysTravel / Math.Pow(2, calc)));
        }

        public static float WealthReduction(float w)
        {
            double l = Settings.Get().wealthLimit;
            if (w <= l)
            {
                return 1.0f;
            }

            double m = Settings.Get().wealthMax;
            var f = Settings.Get().wealthFactor;
            if (w >= m)
            {
                return 1 - f;
            }

            double q = Settings.Get().wealthCurvy;

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
}