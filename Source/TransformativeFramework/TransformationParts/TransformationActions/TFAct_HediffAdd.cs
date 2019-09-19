using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_HediffAdd : TransformationAction
   {
      // The def of the hediff to change.
      public HediffDef hediff;
      // The target severity of the hediff.
      public float severity;
      // Defs of body parts to affect.
      public List<BodyPartDef> partsToAffect;

      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(h => h.def == hediff);
         IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);

         foreach (BodyPartRecord part in parts)
         {
            if (!hediffs.Any(h => h.Part == part))
            {
               return true;
            }
         }
         return false;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(h => h.def == hediff);
         IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);

         foreach (BodyPartRecord part in parts)
         {
            // Skip this part if it already has our hediff.
            if (hediffs.Any(h => h.Part == part)) continue;

            Hediff newHediff = HediffMaker.MakeHediff(hediff, pawn);

            if (severity != default(int))
            {
               newHediff.Severity = severity;
            }
            yield return HediffUtility.MessageHediffGained.Translate(pawn.LabelShort, newHediff.Label, ParseCause(cause));
            pawn.health.AddHediff(newHediff);
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
         if (hediff == null)
         {
            yield return "no hediff";
         }
         yield break;
      }
   }
}
