using System.Collections.Generic;
using Verse;

namespace Mashed_Poogie
{
    public class CompProperties_SelectableOutfit : CompProperties
    {
        public CompProperties_SelectableOutfit() => compClass = typeof(Comp_SelectableOutfit);

        public List<HediffDef> outfitDefs;
    }
}
