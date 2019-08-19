using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class IngestionOutcomeDoer_InstantTFs : IngestionOutcomeDoer
   {
      public TransformationDef transformation;
      public int number = 1;

      protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
      {
         transformation.DoTransformations(pawn, ingested, number);
      }
   }
}
