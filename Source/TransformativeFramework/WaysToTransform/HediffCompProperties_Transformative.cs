using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class HediffCompProperties_Transformative : HediffCompProperties
   {
      public HediffCompProperties_Transformative() => compClass = typeof(HediffComp_Transformative);

      // List of possible transformations.
      public TransformationDef transformation;
      // Mean time between TFs.
      public float mtbDays;
      // Number of transformations to do.
      public int number = 1;
      // Hash update interval (advanced users only).
      public int interval = 60;
   }
}
