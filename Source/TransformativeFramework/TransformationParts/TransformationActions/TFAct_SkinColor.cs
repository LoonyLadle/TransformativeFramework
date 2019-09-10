using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_SkinColor : TransformationAction
   {
      // A color generator used to determine the skin color.
      public ColorGenerator colorGenerator;
      // How much to change with each transformation.
      public float delta = 1f;
      // Colors with a higher power cannot be overridden by lower ones.
      public float power = 1f;

      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         CompTFTracker tracker = pawn.GetComp<CompTFTracker>();

         if (pawn.story == null)
         {
            return false;
         }
         else if (tracker == null)
         {
            Log.Warning($"Considered setting skin color on pawn {pawn.LabelShort} who lacks a CompTFTracker.");
            return false;
         }
         else if (power < tracker.skinColorPower)
         {
            return false;
         }
         else if (tracker.colorTargets.TryGetValue(this, out Color colorTarget))
         {
            if (pawn.story.SkinColor.IndistinguishableFrom(colorTarget))
            {
               return false;
            }
         }
         return true;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         CompTFTracker tracker = pawn.GetComp<CompTFTracker>();

         Color color;
         if (tracker.colorTargets.TryGetValue(this, out Color colorTarget))
         {
            color = colorTarget;
         }
         else
         {
            color = colorGenerator.NewRandomizedColor();
            tracker.colorTargets.Add(this, color);
         }

         tracker.skinColor = ColorUtility.MoveTowards(pawn.story.hairColor, color, delta);
         tracker.skinColorPower = power;
         pawn.Drawer.renderer.graphics.ResolveAllGraphics();
         PortraitsCache.SetDirty(pawn);
         yield break;
      }
   }
}
