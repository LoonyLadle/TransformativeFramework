using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_HediffRemove : TransformationAction
   {
      // The def of the hediff to change.
      public HediffDef hediff;
      // Defs of body parts to affect.
      public List<BodyPartDef> partsToAffect;

      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(h => h.def == hediff);
         IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);

         foreach (BodyPartRecord part in parts)
         {
            if (hediffs.Any(h => h.Part == part))
            {
               return true;
            }
         }
         return false;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);
         
         foreach (Hediff realHediff in pawn.health.hediffSet.hediffs.Where(h => h.def == hediff && parts.Contains(h.Part)))
         {
            yield return HediffUtility.MessageHediffLost.Translate(pawn.LabelShort, realHediff.Label, ParseCause(cause));
            pawn.health.RemoveHediff(realHediff);
         }
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
