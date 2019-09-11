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
            
            if ((adjustedSeverity == 0) && ((operation & Operation.Remove) == Operation.Remove))
            {
               //yield return MessageTraitLost.Translate(pawn.LabelShort, realHediff.Label, ParseCause(cause));
               pawn.health.RemoveHediff(realHediff);
            }
            else if (realHediff.Severity != adjustedSeverity)
            {
               //yield return MessageTraitChanged.Translate(pawn.LabelShort, realHediff.Label, hediff.DataAtDegree(adjustedSeverity).label, ParseCause(cause));
               realHediff.Severity = adjustedSeverity;
            }
         }
         else
         {
            //if (epsilon != 0)
            {
               float adjustedSeverity = MathUtility.MoveTowardsOperationClamped(0, target, delta, operation);
               
               Hediff newHediff = HediffMaker.MakeHediff(hediff, pawn);
               //yield return MessageTraitGained.Translate(pawn.LabelShort, newTrait.Label, ParseCause(cause));
               pawn.health.AddHediff(newHediff);
            }
         }
         // We're done here.
         yield break;
      }
   }
}
