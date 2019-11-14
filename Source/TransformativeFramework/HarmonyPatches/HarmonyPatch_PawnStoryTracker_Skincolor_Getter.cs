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
			CompTFTracker tracker = pawn.GetComp<CompTFTracker>();

			if (tracker?.skinColor.NullOrClear() ?? true)
			{
				// Allow the original method.
				return true;
			}
			else
			{
				__result = tracker.skinColor;
				// Don't allow original to execute.
				return false;
			}
		}
	}
}
