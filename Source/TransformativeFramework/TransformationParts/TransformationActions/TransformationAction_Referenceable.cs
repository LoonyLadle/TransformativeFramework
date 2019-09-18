using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public abstract class TransformationAction_Referenceable : TransformationAction, IExposable, ILoadReferenceable
   {
      public string refName = "UnnamedRef";

      public string GetUniqueLoadID()
      {
         return "TransformationAction_" + Transformation.Def.defName + "_" + refName;
      }

      public virtual void ExposeData()
      {
         // Nothing to save, but LookMode.Reference doesn't work properly without this.
      }

      public override IEnumerable<string> ConfigErrors()
      {
         foreach (string error in base.ConfigErrors())
         {
            yield return error;
         }
         if (refName == "UnnamedRef")
         {
            yield return "refName not set, this will cause errors on loading";
         }
         yield break;
      }
   }
}
