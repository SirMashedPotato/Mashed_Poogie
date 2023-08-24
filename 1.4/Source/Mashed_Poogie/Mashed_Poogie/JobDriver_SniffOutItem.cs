using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using System.Linq;

namespace Mashed_Poogie
{
    public class JobDriver_SniffOutItem : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected IntVec3 Cell => job.targetA.Cell;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
            Toil doWork = new Toil
            {
                initAction = delegate ()
                {
                    workLeft = baseWork / pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
                },
                tickAction = delegate ()
                {
                    workLeft--;
                    if (pawn.IsHashIntervalTick(10))
                    {
                        FleckMaker.ThrowDustPuff(Cell, pawn.Map, 1f);
                    }
                    if (workLeft <= 0f)
                    {
                        SpawnItem();
                        EndJobWith(JobCondition.Succeeded);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return doWork;
            yield break;
        }

        private void SpawnItem()
        {
            ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.category == ThingCategory.Item && !x.IsCorpse && !x.Minifiable && x.BaseMarketValue > 1).RandomElementByWeight(y => 1 / y.BaseMarketValue);
            Thing thing = ThingMaker.MakeThing(thingDef, null);

            if (thingDef.stackLimit > 1)
            {
                thing.stackCount = Rand.RangeInclusive(1, thingDef.stackLimit / 5);
            }

            if (!thingDef.MadeFromStuff && !thingDef.stuffCategories.NullOrEmpty())
            {
                thing.SetStuffDirect(DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.stuffCategories != null && x.stuffCategories.Any(y => thingDef.stuffCategories.Contains(y))).RandomElement());
            }

            GenSpawn.Spawn(thing, TargetLocA, Map, WipeMode.Vanish);
            Messages.Message("Mashed_Poogie_DugUpItem".Translate(pawn.Name, thing.Label), thing, MessageTypeDefOf.PositiveEvent);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f, false);
        }

        private float workLeft = 0f;
        private const int baseWork = 100;
	}
}

