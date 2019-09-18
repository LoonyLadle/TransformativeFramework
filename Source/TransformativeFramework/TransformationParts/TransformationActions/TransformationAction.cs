using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public abstract class TransformationAction : TransformationPart, IExposable, ILoadReferenceable
   {
      public bool ignoreInConditions;
      public MessageTypeDef messageType;

      public sealed override bool CheckPart(Pawn pawn, object cause) => CheckPartWorker(pawn, cause);

      public void ApplyPart(Pawn pawn, object cause, out IEnumerable<string> reports) => reports = ApplyPartWorker(pawn, cause);

      protected abstract IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause);

      public override void ResolveReferences()
      {
         if (messageType == null)
         {
            messageType = MessageTypeDefOf.PositiveEvent;
         }
      }

      public string GetUniqueLoadID()
      {
         // Placeholder. REPLACE THIS BEFORE RELEASE!
         return GetType().ToString() + Transformation.actions.IndexOf(this) + "_" + Transformation.Def.defName + Transformation.Def.transformations.IndexOf(Transformation);
      }

      public virtual void ExposeData()
      {
         // Nothing to save, but LookMode.Reference doesn't work properly without this.
      }
   }
}
