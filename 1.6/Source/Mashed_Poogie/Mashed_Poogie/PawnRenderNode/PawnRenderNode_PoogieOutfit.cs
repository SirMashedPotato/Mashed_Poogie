using UnityEngine;
using Verse;

namespace Mashed_Poogie
{
    public class PawnRenderNode_PoogieOutfit : PawnRenderNode
    {
        public PawnRenderNode_PoogieOutfit(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Color ColorFor(Pawn pawn)
        {
            Comp_SelectableOutfit comp = pawn.TryGetComp<Comp_SelectableOutfit>();
            return comp != null ? comp.outfitColor : Color.white;
        }
    }
}
