using Verse;
using Verse.AI;
using RimWorld;

namespace Mashed_Poogie
{
    public class JobGiver_SniffOutItem : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.Downed)
            {
                return null;
            }
			IntVec3 c = TryFindDigCell(pawn);
			if (!c.IsValid)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Mashed_Poogie_SniffOutItem, c);
		}

        private static IntVec3 TryFindDigCell(Pawn pawn)
        {
            if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), (Region r) => r.Room.PsychologicallyOutdoors, 100, out Region rootReg, RegionType.Set_Passable))
            {
                return IntVec3.Invalid;
            }
            IntVec3 result = IntVec3.Invalid;
            RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.District == rootReg.District, delegate (Region r)
            {
                for (int i = 0; i < 5; i++)
                {
                    IntVec3 randomCell = r.RandomCell;
                    if (IsGoodDigCell(randomCell, pawn))
                    {
                        result = randomCell;
                        return true;
                    }
                }
                return false;
            }, 30, RegionType.Set_Passable);
            return result;
        }

		private static bool IsGoodDigCell(IntVec3 c, Pawn pawn)
		{
			if (!c.GetTerrain(pawn.Map).affordances.Contains(TerrainAffordanceDefOf.Diggable))
			{
				return false;
			}
			if (!c.GetThingList(pawn.Map).NullOrEmpty())
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (c.GetEdifice(pawn.Map) != null)
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (!c2.InBounds(pawn.Map))
				{
					return false;
				}
				if (!c2.Standable(pawn.Map))
				{
					return false;
				}
				if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
				{
					return false;
				}
			}
			
			return true;
		}
    }
}
