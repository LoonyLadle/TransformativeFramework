using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class TraitUtility
	{
		public static void LoseTrait(this TraitSet traitSet, Trait trait)
		{
			Pawn pawn = (Pawn)typeof(TraitSet).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(traitSet);
			
			if (!traitSet.HasTrait(trait.def))
			{
				Log.Warning(pawn + " does not have trait " + trait.def);
				return;
			}
			traitSet.allTraits.Remove(trait);
			traitSet.ForceUpdate();
		}
		
		public static void SetDegreeOfTrait(this TraitSet traitSet, Trait trait, int degree)
		{
			typeof(Trait).GetField("degree", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(trait, degree);
			traitSet.ForceUpdate();
		}

		public static void ForceUpdate(this TraitSet traitSet)
		{
			Pawn pawn = (Pawn)typeof(TraitSet).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(traitSet);

			pawn.Notify_DisabledWorkTypesChanged();

			if (pawn.skills != null)
			{
				pawn.skills.Notify_SkillDisablesChanged();
			}
			if (!pawn.Dead && pawn.RaceProps.Humanlike && pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
		}

		public static int NearestPossibleDegreeTo(TraitDef trait, int current, int target, int delta, Operation operation)
		{
			int adjustedDegree = MathUtility.MoveTowardsOperationClamped(current, target, delta, operation);

			if ((adjustedDegree == 0) || trait.degreeDatas.Any(data => data.degree == adjustedDegree))
			{
				return adjustedDegree;
			}
			else
			{
				return MathUtility.NearestBetween(trait.degreeDatas.Select(data => data.degree), adjustedDegree, current, adjustedDegree);
			}
		}

		public static int NearestPossibleDegreeTo(this Trait realTrait, int target, int delta, Operation operation)
		{
			return NearestPossibleDegreeTo(realTrait.def, realTrait.Degree, target, delta, operation);
		}
	}
}
