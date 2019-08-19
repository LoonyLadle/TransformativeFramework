using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public static class ColorUtility
   {
      public static Color MoveTowards(Color current, Color target, float amount)
      {
         // Placeholder implementation.
         return new Color(Mathf.MoveTowards(current.r, target.r, amount), Mathf.MoveTowards(current.g, target.g, amount), Mathf.MoveTowards(current.b, target.b, amount));
      }
   }
}
