using Harmony;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   [HarmonyPriority(Priority.High)]
   [HarmonyPatch(typeof(Pawn_StoryTracker), nameof(Pawn_StoryTracker.SkinColor), MethodType.Getter)]
   public static class HarmonyPatch_PawnStoryTracker_Skincolor_Getter
   {
      public static bool Prefix(Pawn_StoryTracker __instance, ref Color __result)
      {
         Pawn pawn = (Pawn)typeof(Pawn_StoryTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
         CompTFTracker tfData = pawn.GetComp<CompTFTracker>();
         
         // Is a TF skin color defined?
         if (tfData?.skinColor != null)
         {
            // Set our result to the transformed color.
            __result = tfData.skinColor;
            // Don't allow original to execute.
            return false;
         }
         // Allow the original method.
         else return true;
      }
   }
}
