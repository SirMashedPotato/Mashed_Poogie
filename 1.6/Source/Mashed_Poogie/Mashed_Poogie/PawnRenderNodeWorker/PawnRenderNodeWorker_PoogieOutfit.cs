using UnityEngine;
using Verse;

namespace Mashed_Poogie
{
    public class PawnRenderNodeWorker_PoogieOutfit : PawnRenderNodeWorker
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            Comp_SelectableOutfit comp = parms.pawn.TryGetComp<Comp_SelectableOutfit>();
            return comp != null;
        }
        public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
        {
            return base.ScaleFor(node, parms) * 0.8f;
        }
    }
}
