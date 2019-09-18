using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_SkinColor : TransformationAction_Referenceable
   {
      // A color generator used to determine the skin color.
      public ColorGenerator colorGenerator;
      // How much to change with each transformation.
      public float delta = float.MaxValue;
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

         Color target;
         if (tracker.colorTargets.TryGetValue(this, out Color colorTarget))
         {
            target = colorTarget;
         }
         else
         {
            target = colorGenerator.NewRandomizedColor();
            tracker.colorTargets.Add(this, target);
         }

         tracker.skinColor = ColorUtility.MoveTowards(pawn.story.hairColor, target, delta);
         tracker.skinColorPower = power;
         pawn.Drawer.renderer.graphics.ResolveAllGraphics();
         PortraitsCache.SetDirty(pawn);
         yield break;
      }
   }
}
