using RimWorld;
using Verse;

namespace Mashed_Poogie
{
    [DefOf]
    public static class TerrainAffordanceDefOf
    {
        static TerrainAffordanceDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
        }
        public static TerrainAffordanceDef Diggable;
    }
}