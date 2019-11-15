using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class Transformation
	{
		public List<TransformationCondition> conditions = new List<TransformationCondition>();
		public List<TransformationAction> actions = new List<TransformationAction>();
		public float weight = 1.0f;
		public string anchor;

		public TransformationDef Def { get; private set; }

		// Returns true if the TF should be applied.
		public bool Check(Pawn pawn, object cause)
		{
			// First, check our conditions. All conditions must be true for a TF to run.
			foreach (TransformationCondition condition in conditions)
			{
				if (!condition.CheckPart(pawn, cause))
				{
					return false;
				}
			}
			// Next, check our actions. Unlike conditions, only one action needs to be true.
			foreach (TransformationAction action in actions)
			{
				if (action.ignoreInConditions || action.CheckPart(pawn, cause))
				{
					return true;
				}
			}
			// If we got this far, no actions returned true. This means there's nothing for the TF to do!
			return false;
		}

		// Applies the TF.
		public void Apply(Pawn pawn, object cause)
		{
			foreach (TransformationAction action in actions)
			{
				if (action.CheckPart(pawn, cause))
				{
					action.ApplyPart(pawn, cause, out IEnumerable<string> reports);
					
					if (PawnUtility.ShouldSendNotificationAbout(pawn))
					{
						foreach (string report in reports)
						{
							Messages.Message(report, pawn, action.messageType);
						}
					}
				}
			}
		}

		public IEnumerable<string> ConfigErrors()
		{
			for (int i = 0; i < conditions.Count; i++)
			{
				foreach (string error in conditions[i].ConfigErrors())
				{
					yield return $".conditions[{i}] {error}";
				}
			}
			if (actions.NullOrEmpty())
			{
				yield return " has no actions";
			}
			else
			{
				for (int i = 0; i < actions.Count; i++)
				{
					foreach (string error in actions[i].ConfigErrors())
					{
						yield return $".actions[{i}] {error}";
					}
				}
			}
			yield break;
		}

		public void ResolveReferences(TransformationDef parentDef)
		{
			Def = parentDef;

			foreach (TransformationCondition condition in conditions)
			{
				condition.ResolveReferences(this);
			}

			foreach (TransformationAction action in actions)
			{
				action.ResolveReferences(this);
			}
		}
	}
}
