using RimWorld;
using Verse;

namespace Mashed_Poogie
{
    [DefOf]
    public static class JobDefOf
    {
        static JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
        }
        public static JobDef Mashed_Poogie_SniffOutItem;
    }
}