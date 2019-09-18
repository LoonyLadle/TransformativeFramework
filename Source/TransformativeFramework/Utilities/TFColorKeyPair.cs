using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFColorKeyPair : IExposable
   {
      public TFColorKeyPair(TransformationAction_Referenceable action, Color color)
      {
         actionInt = action;
         colorInt = color;
      }

      public TransformationAction_Referenceable actionInt;
      public Color colorInt;

      public void ExposeData()
      {
         Scribe_TFAct.Look(ref actionInt, nameof(actionInt));
         Scribe_Values.Look(ref colorInt, nameof(colorInt));
      }
   }
}
