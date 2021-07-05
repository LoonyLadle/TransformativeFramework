using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFAct_ChangeTrait : TransformationAction
	{
		// The def of the trait to change.
		public TraitDef trait;
		// The target degree of the trait.
		public int target;
		// How much to move the trait's degree towards the target degree.
		public int delta = int.MaxValue;
		// How to handle conflicts.
		public ConflictResolutionMode conflicts = ConflictResolutionMode.Fail;
		// The intent of changing the trait.
		public Operation operation = Operation.TraitSpectrum;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			if (pawn.story?.traits == null)
			{
				return false;
			}

			Trait realTrait = pawn.story.traits.GetTrait(trait);

			if (realTrait != null)
			{
				if (realTrait.Degree == realTrait.NearestPossibleDegreeTo(target, delta, operation))
				{
					return false;
				}
			}
			else
			{
				if ((conflicts == ConflictResolutionMode.Fail) && pawn.story.traits.allTraits.Any(t => trait.ConflictsWith(t)))
				{
					return false;
				}
			}
			return true;
		}

		protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
		{
			Trait realTrait = pawn.story.traits.GetTrait(trait);

			if (realTrait != null)
			{
				TraitUtility.AdjustTrait(realTrait, pawn, cause, target, delta, operation, out string report);
				yield return report;
			}
			else
			{
				int epsilon = Math.Abs(delta);

				switch (conflicts)
				{
					case ConflictResolutionMode.Fail:
						if (pawn.story.traits.allTraits.Any(t => trait.ConflictsWith(t)))
						{
							Log.Error($"Tried to add trait {trait} to {pawn} but conflicting traits exist (CheckPartWorker should have prevented this).");
							yield break;
						}
						break;
					case ConflictResolutionMode.Ignore:
						break;
					case ConflictResolutionMode.Remove:
						List<Trait> conflictingTraits = pawn.story.traits.allTraits.Where(t => trait.ConflictsWith(t)).ToList();

						while (epsilon > 0 && conflictingTraits.Any())
						{
							Trait randomTrait = conflictingTraits.RandomElement();

							TraitUtility.AdjustTrait(randomTrait, pawn, cause, 0, ref epsilon, Operation.Remove, out string report);
							conflictingTraits.Remove(randomTrait);
							yield return report;
						}
						break;
				}

				epsilon *= Math.Sign(delta);

				if (epsilon != 0)
				{
					int adjustedDegree = TraitUtility.NearestPossibleDegreeTo(trait, 0, target, epsilon, operation);
					
					Trait newTrait = new Trait(trait, adjustedDegree);
					yield return TraitUtility.MessageTraitGained.Translate(pawn.LabelShort, newTrait.Label, StringUtility.ParseCause(cause));
					pawn.story.traits.GainTrait(newTrait);
				}
			}
			// We're done here.
			yield break;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}
			if (trait.degreeDatas.Any(data => data.degree == target))
			{
				yield return $"target is {target} but {trait} has no such data";
			}
		}
	}
}
