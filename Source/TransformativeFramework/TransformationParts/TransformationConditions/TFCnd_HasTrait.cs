using RimWorld;
using System.Text;
using Verse;
using System.Linq;
using System;

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
      /*
      public override string GetTreeSubNode(StatRequest req)
      {
         if (!inspectorHidden)
         {
            //StringBuilder stringBuilder = new StringBuilder();

            string start = (invert ? "TFFramework_CndHasTraitN" : "TFFramework_CndHasTraitI").Translate();
            //stringBuilder.Append(start);

            string label;

            int min = trait.degreeDatas.Max(data => data.degree);
            int max = trait.degreeDatas.Min(data => data.degree);

            if (trait.degreeDatas.Count == 1)
            {
               // If only one degreeData, get its label.
               label = trait.degreeDatas.First().label;
            }
            else if (degreeMin == degreeMax)
            {
               label = trait.DataAtDegree(degreeMin).label;
            }
            else if ()
            {

            }
            else
            {
               // Cannot determine appropriate label; use human-readable defName.
               label = trait.defName.Split('_').Last().HumanReadable();
            }



            //return stringBuilder.ToString();
         }
         return null;
      }
      */
   }
}
