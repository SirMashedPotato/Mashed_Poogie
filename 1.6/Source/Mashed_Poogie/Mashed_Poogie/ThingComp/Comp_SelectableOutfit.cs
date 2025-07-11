using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Poogie
{
    public class Comp_SelectableOutfit : ThingComp
    {
        public CompProperties_SelectableOutfit Props => (CompProperties_SelectableOutfit)props;

        public Hediff currentOutfit;
        public Color outfitColor = Color.white;

        public static Widgets.ColorComponents visibleColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;
        public static Widgets.ColorComponents editableColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

        List<FloatMenuOption> outfitGizmoOptions;

        private List<FloatMenuOption> OutfitGizmoOptions
        {
            get
            {
                if (outfitGizmoOptions.NullOrEmpty())
                {
                    outfitGizmoOptions = new List<FloatMenuOption>();

                    //changing the colour at the top
                    FloatMenuOption changeColour = new FloatMenuOption("Mashed_Poogie_ChangeColour".Translate(), delegate
                    {
                        Widgets.ColorComponents visibleTextfields = (visibleColorTextfields);
                        Widgets.ColorComponents editableTextfields = (editableColorTextfields);
                        Dialog_OutfitColorPicker window = new Dialog_OutfitColorPicker(this, visibleTextfields, editableTextfields);
                        Find.WindowStack.Add(window);
                    });
                    outfitGizmoOptions.Add(changeColour);

                    //changing the colour at the top
                    FloatMenuOption noOutfit = new FloatMenuOption("Mashed_Poogie_NoOutfit".Translate(), delegate
                    {
                        if (currentOutfit != null)
                        {
                            Pawn pawn = parent as Pawn;
                            pawn.health.RemoveHediff(currentOutfit);
                            currentOutfit = null;
                        }
                    });
                    outfitGizmoOptions.Add(noOutfit);

                    foreach (HediffDef hediffDef in Props.outfitDefs)
                    {
                        FloatMenuOption outfit = new FloatMenuOption(hediffDef.label.CapitalizeFirst(), delegate
                        {
                            Pawn pawn = parent as Pawn;
                            if (currentOutfit != null)
                            {
                                pawn.health.RemoveHediff(currentOutfit);
                                currentOutfit = null;
                            }

                            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
                            pawn.health.AddHediff(hediff);
                            currentOutfit = hediff;

                        });
                        outfitGizmoOptions.Add(outfit);
                    }
                }
                return outfitGizmoOptions;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (parent.Faction != null && parent.Faction == Faction.OfPlayer &&
                parent is Pawn p && p.ageTracker.Adult)
            {
                yield return new Command_Action
                {
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Mashed_Poogie_ChangeOutfit", true),
                    defaultLabel = "Mashed_Poogie_ChangeOutfit_Label".Translate(),
                    defaultDesc = "Mashed_Poogie_ChangeOutfit_Desc".Translate(parent),
                    action = delegate
                    {
                        Find.WindowStack.Add(new FloatMenu(OutfitGizmoOptions));
                    }
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_References.Look(ref currentOutfit, "currentOutfit");
            Scribe_Values.Look(ref outfitColor, "outfitColor", Color.white);
            base.PostExposeData();
        }
    }
}
