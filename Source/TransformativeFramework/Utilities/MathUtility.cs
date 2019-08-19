using System;
using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public static class MathUtility
   {
      public static float MoveTowardsOperationClamped(float current, float target, float maxDelta, TraitOperation operation)
      {
         float min = (operation & TraitOperation.Decrease) == TraitOperation.Decrease ? float.MinValue : current;
         float max = (operation & TraitOperation.Increase) == TraitOperation.Increase ? float.MaxValue : current;
         float result = Mathf.MoveTowards(current, target, maxDelta);
         return Mathf.Clamp(result, min, max);
      }
      
      public static int MoveTowardsOperationClamped(int current, int target, int maxDelta, TraitOperation operation)
      {
         int min = (operation & TraitOperation.Decrease) == TraitOperation.Decrease ? int.MinValue : current;
         int max = (operation & TraitOperation.Increase) == TraitOperation.Increase ? int.MaxValue : current;
         int result = MathUtility.MoveTowards(current, target, maxDelta);
         return Mathf.Clamp(result, min, max);
      }

      public static float MoveTowardsRepeat(float current, float target, float amount, float length = 1f)
      {
         if (current > (length / 2f))
         {
            current -= length;
         }
         if (target > (length / 2f))
         {
            target -= length;
         }
         return Mathf.Repeat(Mathf.MoveTowards(current, target, amount), length);
      }

      // Int version of Mathf.MoveTowards.
      public static int MoveTowards(int current, int target, int maxDelta)
      {
         return Mathf.Abs(target - current) <= maxDelta ? target : current + (Math.Sign(target - current) * maxDelta);
      }
   }
}
