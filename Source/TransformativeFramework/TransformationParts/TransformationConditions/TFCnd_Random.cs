using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFCnd_Random : TransformationCondition
   {
      public float chance;

      // Return true with a probability of chance. This is not the same as giving a transformation a low weight; a low weight reduces the
      // chance that an available TF will be selected, while a random condition adds a chance that the TF won't be available at all.
      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         return Rand.Chance(chance);
      }
   }
}
