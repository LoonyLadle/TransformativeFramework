using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFCnd_HasTrait : TransformationCondition
   {
      public TraitDef trait;
      public int degreeMin = int.MinValue;
      public int degreeMax = int.MaxValue;

      // Return true if pawn has a trait of def traitDef between degreeMin and degreeMax.
      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         Trait realTrait = pawn.story?.traits?.GetTrait(trait);

         return realTrait != null 
            ? realTrait.Degree >= degreeMin && realTrait.Degree <= degreeMax 
            : false;
      }
   }
}
