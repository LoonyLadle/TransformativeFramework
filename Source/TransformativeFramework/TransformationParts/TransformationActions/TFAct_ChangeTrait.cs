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
		public Operation operation = Operation.IncreaseOrDecrease;

		private const string MessageTraitChanged = "TFFramework_MessageTraitChanged";
		private const string MessageTraitGained = "TFFramework_MessageTraitGained";
		private const string MessageTraitLost = "TFFramework_MessageTraitLost";

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			if (pawn.story?.traits == null)
			{
				return false;
			}

			Trait realTrait = pawn.story.traits.GetTrait(trait);

			if (realTrait != null)
			{
				if (realTrait.Degree == MathUtility.MoveTowardsOperationClamped(realTrait.Degree, target, delta, operation))
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
				int adjustedDegree = MathUtility.MoveTowardsOperationClamped(realTrait.Degree, target, delta, operation);

				// If our adjusted degree is zero and no degree data exists at zero, remove the trait.
				if ((adjustedDegree == 0) && !realTrait.def.degreeDatas.Any(data => data.degree == adjustedDegree))
				{
					yield return MessageTraitLost.Translate(pawn.LabelShort, realTrait.Label, ParseCause(cause));
					pawn.story.traits.LoseTrait(realTrait);
				}
				else
				{
					yield return MessageTraitChanged.Translate(pawn.LabelShort, realTrait.Label, trait.DataAtDegree(adjustedDegree).label, ParseCause(cause));
					pawn.story.traits.SetDegreeOfTrait(realTrait, adjustedDegree);
				}
			}
			else
			{
				int epsilon = Math.Abs(delta);

				switch (conflicts)
				{
					case ConflictResolutionMode.Fail:
						if (pawn.story.traits.allTraits.Any(t => trait.ConflictsWith(t)))
						{
							// CheckPartWorker should have prevented this. If this error happens then something is VERY WRONG.
							Log.Error($"Tried to add trait {trait} to {pawn} but conflicting traits exist (how did this even happen?).");
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
							yield return MessageTraitLost.Translate(pawn.LabelShort, randomTrait.Label, ParseCause(cause));
							// Do not force update until we are done.
							pawn.story.traits.LoseTrait(randomTrait, false);
							conflictingTraits.Remove(randomTrait);
							epsilon--;
						}
						// Force update now that all conflicting traits are removed.
						pawn.story.traits.ForceUpdate();
						break;
					default:
						throw new InvalidOperationException("Unhandled conflictResolution value: " + conflicts.ToString());
				}

				epsilon *= Math.Sign(delta);

				if (epsilon != 0)
				{
					int adjustedDegree = MathUtility.MoveTowardsOperationClamped(0, target, epsilon, operation);
					
					Trait newTrait = new Trait(trait, adjustedDegree);
					yield return MessageTraitGained.Translate(pawn.LabelShort, newTrait.Label, ParseCause(cause));
					pawn.story.traits.GainTrait(newTrait);
				}
			}
			// We're done here.
			yield break;
		}
	}
}
