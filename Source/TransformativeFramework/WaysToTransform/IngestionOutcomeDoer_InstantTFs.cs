using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class IngestionOutcomeDoer_InstantTFs : IngestionOutcomeDoer
   {
      public TransformationDef transformation;
      public int number = 1;
      public int doToAddictedMin = 1;
      public int doToAddictedMax = 1;

      protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
      {
         if (ingested == null)
         {
            if (doToGeneratedPawnIfAddicted)
            {
               transformation.DoTransformations(pawn, null, Rand.Range(doToAddictedMin, doToAddictedMax));
            }
         }
         else
         {
            transformation.DoTransformations(pawn, ingested, number);
         }
      }
   }
}
