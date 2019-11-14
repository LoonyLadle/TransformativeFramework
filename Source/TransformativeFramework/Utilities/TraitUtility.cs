using RimWorld;
using System.Reflection;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class TraitUtility
	{
		public static void LoseTrait(this TraitSet traitSet, Trait trait, bool forceUpdate = true)
		{
			Pawn pawn = (Pawn)typeof(TraitSet).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(traitSet);
			
			if (!traitSet.HasTrait(trait.def))
			{
				Log.Warning(pawn + " does not have trait " + trait.def);
				return;
			}
			traitSet.allTraits.Remove(trait);

			if (forceUpdate)
			{
				traitSet.ForceUpdate();
			}
		}
		
		public static void SetDegreeOfTrait(this TraitSet traitSet, Trait trait, int degree, bool forceUpdate = true)
		{
			typeof(Trait).GetField("degree", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(trait, degree);

			if (forceUpdate)
			{
				traitSet.ForceUpdate();
			}
		}

		public static void ForceUpdate(this TraitSet traitSet)
		{
			Pawn pawn = (Pawn)typeof(TraitSet).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(traitSet);

			if (pawn.workSettings != null)
			{
				pawn.workSettings.Notify_GainedTrait();
			}
			//pawn.story.Notify_TraitChanged();
			typeof(Pawn_StoryTracker).GetMethod("Notify_TraitChanged", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(pawn.story, null);

			if (pawn.skills != null)
			{
				pawn.skills.Notify_SkillDisablesChanged();
			}
			if (!pawn.Dead && pawn.RaceProps.Humanlike)
			{
				pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}
	}
}
