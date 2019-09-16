using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_ChangeHediff : TransformationAction
   {
      // The def of the hediff to change.
      public HediffDef hediff;
      // The target severity of the hediff.
      public float target = float.MaxValue;
      // How much to move the hediff's severity towards the target severity.
      public float delta;
      // The intent of changing the hediff.
      // - Valid flags: Increase, Decrease, Remove
      public Operation operation = Operation.Normal;

      private const string MessageHediffChanged = "TFFramework_MessageHediffChanged"; // "{0}'s {1} became {2} because of {3}."
      private const string MessageHediffGained  = "TFFramework_MessageHediffGained";  // "{0}'s gained {1} from {2}."
      private const string MessageHediffLost    = "TFFramework_MessageHediffLost";    // "{0}'s {1} was removed by {2}."

      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         Hediff realHediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediff);

         if (realHediff != null)
         {
            if (realHediff.Severity == target)
            {
               return false;
            }
            if ((operation == Operation.Increase) && (realHediff.Severity == realHediff.def.maxSeverity))
            {
               return false;
            }
            if ((operation == Operation.Decrease) && (realHediff.Severity == realHediff.def.minSeverity))
            {
               return false;
            }
         }
         else
         {
            if ((operation & Operation.Remove) == Operation.Remove)
            {
               return false;
            }
         }
         return true;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         Hediff realHediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediff);

         if (realHediff != null)
         {
            float adjustedSeverity = MathUtility.MoveTowardsOperationClamped(realHediff.Severity, target, delta, operation);
            
            if ((adjustedSeverity <= 0) && ((operation & Operation.Remove) == Operation.Remove))
            {
               yield return MessageHediffLost.Translate(pawn.LabelShort, realHediff.Label, ParseCause(cause));
               pawn.health.RemoveHediff(realHediff);
            }
            else if (realHediff.Severity != adjustedSeverity)
            {
               int oldIndex = realHediff.CurStageIndex;

               realHediff.Severity = adjustedSeverity;

               if (realHediff.ShouldRemove)
               {
                  yield return MessageHediffLost.Translate(pawn.LabelShort, realHediff.LabelBase, ParseCause(cause));
               }
               else if (realHediff.CurStageIndex != oldIndex)
               {
                  yield return MessageHediffChanged.Translate(pawn.LabelShort, realHediff.LabelBase, realHediff.CurStage.label, ParseCause(cause));
               }
            }
         }
         else
         {
            //if (epsilon != 0)
            {
               float adjustedSeverity = MathUtility.MoveTowardsOperationClamped(0, target, delta, operation);
               
               Hediff newHediff = HediffMaker.MakeHediff(hediff, pawn);
               yield return MessageHediffGained.Translate(pawn.LabelShort, newHediff.Label, ParseCause(cause));
               pawn.health.AddHediff(newHediff);
            }
         }
         // We're done here.
         yield break;
      }

      public override IEnumerable<string> ConfigErrors()
      {
         foreach (string error in base.ConfigErrors())
         {
            yield return error;
         }
         if (delta == 0)
         {
            yield return "delta is zero";
         }
         yield break;
      }
   }
}
