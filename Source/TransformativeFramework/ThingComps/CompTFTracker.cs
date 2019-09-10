using System.Collections.Generic;
using UnityEngine;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class CompTFTracker : ThingComp
   {
      public CompProperties_TFTracker Props => (CompProperties_TFTracker)props;

      public Color hairColorOriginal;
      public float hairColorPower;

      public Color skinColor;
      public float skinColorPower;

      public Dictionary<TransformationAction, Color> colorTargets = new Dictionary<TransformationAction, Color>();

      public override void PostExposeData()
      {
         List<TransformationAction> keyList = new List<TransformationAction>();
         List<Color> valList = new List<Color>();

         Scribe_Values.Look(ref skinColor, nameof(skinColor));
         Scribe_Values.Look(ref skinColorPower, nameof(skinColorPower));
         Scribe_Values.Look(ref hairColorOriginal, nameof(hairColorOriginal));
         Scribe_Values.Look(ref hairColorPower, nameof(hairColorPower));
         Scribe_Collections.Look(ref colorTargets, nameof(colorTargets), LookMode.Reference, LookMode.Value, ref keyList, ref valList);

         if (Scribe.mode == LoadSaveMode.LoadingVars)
         {
            if (colorTargets == null)
            {
               Log.Message("Creating new colorTargets because existing one is null.");
               colorTargets = new Dictionary<TransformationAction, Color>();
            }
         }
      }
   }
}
