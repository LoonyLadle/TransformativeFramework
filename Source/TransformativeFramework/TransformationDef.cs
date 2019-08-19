using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TransformationDef : Def
   {
      public List<Transformation> transformations = new List<Transformation>();

      private const string StatLabelDef = "TFFramework_StatLabelDef";

      public void DoTransformations(Pawn pawn, object cause, int number)
      {
         for (int i = 0; i < number; i++)
         {
            // Get a weighted random transformation that passes check on the pawn.
            if (transformations.Where(tf => tf.Check(pawn, cause)).TryRandomElementByWeight(tf => tf.weight, out Transformation transformation))
            {
               transformation.Apply(pawn, cause);
            }
            else
            {
               // There are no valid transformations remaining; stop applying.
               break;
            }
         }
      }

      public override IEnumerable<string> ConfigErrors()
      {
         foreach (string error in base.ConfigErrors())
         {
            yield return error;
         }

         if (label.NullOrEmpty())
         {
            yield return "no label";
         }

         if (transformations.NullOrEmpty())
         {
            yield return "has no transformations";
         }
         else
         {
            for (int i = 0; i < transformations.Count; i++)
            {
               foreach (string error in transformations[i].ConfigErrors())
               {
                  yield return $"transformations[{i}]{error}";
               }
            }
         }
         yield break;
      }

      public override void ResolveReferences()
      {
         base.ResolveReferences();

         foreach (Transformation transformation in transformations)
         {
            transformation.ResolveReferences(this);
         }
      }
   }
}
