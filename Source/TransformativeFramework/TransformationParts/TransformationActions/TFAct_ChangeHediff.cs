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
      // The def of the trait to change.
      public HediffDef hediff;
      // The target degree of the trait.
      public float severity;
      // How much to move the trait's degree towards the target degree.
      public float delta = float.MaxValue;
      // The intent of changing the trait.
      // - Valid flags: Increase, Decrease, Remove
      public Operation operation = Operation.Normal;
      /*
      private const string MessageTraitChanged = "TFFramework_MessageTraitChanged";
      private const string MessageTraitGained = "TFFramework_MessageTraitGained";
      private const string MessageTraitLost = "TFFramework_MessageTraitLost";
      */
      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         if (pawn.story?.traits == null)
         {
            return false;
         }
         else if (!pawn.story.traits.HasTrait(hediff))
         {
            if ((operation & Operation.Remove) == Operation.Remove)
            {
               return false;
            }
            if (conflicts == ConflictResolutionMode.Fail && pawn.story.traits.allTraits.Any(t => hediff.ConflictsWith(t)))
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
            float adjustedSeverity = MathUtility.MoveTowardsOperationClamped(realHediff.Severity, severity, delta, operation);

            // If our adjusted degree is zero AND EITHER our operational intent is to remove the trait OR no degree data exists at zero, remove the trait.
            if ((adjustedSeverity == 0) && (((operation & Operation.Remove) == Operation.Remove) || (!realHediff.def.degreeDatas.Any(data => data.degree == adjustedSeverity))))
            {
               yield return MessageTraitLost.Translate(pawn.LabelShort, realHediff.Label, ParseCause(cause));
               pawn.story.traits.LoseTrait(realHediff);
            }
            else if (realHediff.Degree == adjustedSeverity)
            {
               //Log.Message("Existing trait degree equals adjusted degree. No action taken.");
               // WELL IF NO ACTION IS BEING TAKEN WHY ARE WE EVEN HERE? Add a new component to CheckPartWorker before release.
            }
            else
            {
               yield return MessageTraitChanged.Translate(pawn.LabelShort, realHediff.Label, hediff.DataAtDegree(adjustedSeverity).label, ParseCause(cause));
               pawn.story.traits.SetDegreeOfTrait(realHediff, adjustedSeverity);
            }
         }
         else
         {
            int epsilon = Math.Abs(delta);
            
            epsilon *= Math.Sign(delta);

            if (epsilon != 0)
            {
               int adjustedDegree = MathUtility.MoveTowardsOperationClamped(0, severity, epsilon, operation);
               
               Trait newTrait = new Trait(hediff, adjustedDegree);
               yield return MessageTraitGained.Translate(pawn.LabelShort, newTrait.Label, ParseCause(cause));
               pawn.story.traits.GainTrait(newTrait);
            }
         }
         // We're done here.
         yield break;
      }
   }
}
