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

      private List<TFColorKeyPair> colorTargets = new List<TFColorKeyPair>();

      public Color GetColorTarget(TransformationAction_Referenceable action)
      {
         return colorTargets.Find(tfcp => tfcp.actionInt == action)?.colorInt ?? Color.clear;
      }

      public void SetColorTarget(TransformationAction_Referenceable action, Color color)
      {
         var colorTarget = colorTargets.Find(tfcp => tfcp.actionInt == action);

         if (colorTarget == null)
         {
            colorTargets.Add(new TFColorKeyPair(action, color));
         }
         else
         {
            colorTarget.colorInt = color;
         }
      }

      public override void PostExposeData()
      {
         Scribe_Values.Look(ref skinColor, nameof(skinColor));
         Scribe_Values.Look(ref skinColorPower, nameof(skinColorPower));
         Scribe_Values.Look(ref hairColorOriginal, nameof(hairColorOriginal));
         Scribe_Values.Look(ref hairColorPower, nameof(hairColorPower));
         Scribe_Collections.Look(ref colorTargets, nameof(colorTargets), LookMode.Deep);

         if (Scribe.mode == LoadSaveMode.LoadingVars)
         {
            if (colorTargets == null)
            {
               colorTargets = new List<TFColorKeyPair>();
            }
         }
      }
   }
}
