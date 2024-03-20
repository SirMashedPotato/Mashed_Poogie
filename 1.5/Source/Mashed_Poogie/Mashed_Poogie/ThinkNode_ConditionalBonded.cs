using RimWorld;
using Verse;
using Verse.AI;

namespace Mashed_Poogie
{
    public class ThinkNode_ConditionalBonded : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return !TrainableUtility.GetAllColonistBondsFor(pawn).EnumerableNullOrEmpty();
        }
    }
}
