using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Poogie
{
    public class Dialog_OutfitColorPicker : Dialog_ColorPickerBase
    {
        private readonly Comp_SelectableOutfit compOutfit;
        private static readonly List<Color> cachedColorDefList = new List<Color> { };


        public Dialog_OutfitColorPicker(Comp_SelectableOutfit compOutfit, Widgets.ColorComponents visibleTextfields, Widgets.ColorComponents editableTextfields) : base(visibleTextfields, editableTextfields)
        {
            this.compOutfit = compOutfit;
            oldColor = compOutfit.outfitColor;
        }

        public override Vector2 InitialSize => new Vector2(600f, 600f);

        protected override bool ShowDarklight => false;

        protected override Color DefaultColor => Color.white;

        protected override List<Color> PickableColors
        {
            get
            {
                if (cachedColorDefList.NullOrEmpty())
                {
                    foreach (ColorDef def in DefDatabase<ColorDef>.AllDefsListForReading)
                    {
                        if (def.modContentPack.IsCoreMod)
                        {
                            cachedColorDefList.Add(def.color);
                        }
                    }
                }

                return cachedColorDefList;
            }
        }

        protected override float ForcedColorValue => 1f;

        protected override bool ShowColorTemperatureBar => false;


        protected override void SaveColor(Color color)
        {
            compOutfit.outfitColor = color;
            Pawn pawn = compOutfit.parent as Pawn;
            pawn.Drawer.renderer.SetAllGraphicsDirty();
        }
    }
}
