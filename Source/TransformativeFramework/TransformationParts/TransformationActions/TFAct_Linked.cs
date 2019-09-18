using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_Linked : TransformationAction
   {
      public string link;
      public TransformationDef linkDef;

      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         return true;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         foreach (Transformation transformation in linkDef.transformations.Where(tf => tf.anchor == link))
         {
            transformation.Apply(pawn, cause);
         }
         yield break;
      }

      public override IEnumerable<string> ConfigErrors()
      {
         foreach (string error in base.ConfigErrors())
         {
            yield return error;
         }
         if (link.NullOrEmpty())
         {
            yield return "link is empty";
         }
         else if (!linkDef.transformations.Any(tf => tf.anchor == link))
         {
            yield return "link leads nowhere";
         }
         yield break;
      }

      public override void ResolveReferences()
      {
         base.ResolveReferences();

         if (linkDef == null)
         {
            linkDef = Transformation.Def;
         }
      }
   }
}
