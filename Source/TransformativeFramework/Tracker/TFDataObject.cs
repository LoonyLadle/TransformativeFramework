using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFDataObject : IExposable
   {
      public TFDataObject()
      {
         // nothing, just needs to exist
      }

      public TFDataObject(TransformationAction owner)
      {
         this.owner = owner;
      }

      public void ExposeData()
      {
         Scribe_TFAct.Look(ref owner, nameof(owner));
         Scribe_Collections.Look(ref data, nameof(data), LookMode.Value, LookMode.Undefined);
      }

      public TransformationAction owner;
      public Dictionary<string, object> data = new Dictionary<string, object>();
   }
}
