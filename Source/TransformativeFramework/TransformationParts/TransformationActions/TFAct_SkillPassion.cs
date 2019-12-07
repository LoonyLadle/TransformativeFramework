using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFAct_SkillPassion : TransformationAction
	{
		// The def of the skill to change.
		public SkillDef skill;
		// The target degree of the skill passion.
		public Passion target;
		// How much to move the skill's passion towards the target passion.
		public int delta = byte.MaxValue;
		// The intent of changing the passion.
		// - Valid flags: Increase, Decrease, Remove
		public Operation operation = Operation.IncreaseOrDecrease;

		private const string MessageSkillPassionChanged = "TFFramework_MessageSkillPassionChanged"; // {0}'s passion for {1} is now {2} because of {3}.

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			SkillRecord realSkill = pawn.skills?.GetSkill(skill);

			if (realSkill == null)
			{
				return false;
			}
			else if (realSkill.TotallyDisabled)
			{
				return false;
			}
			else if (realSkill.passion == target)
			{
				return false;
			}
			else return true;
		}

		protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
		{
			SkillRecord realSkill = pawn.skills.GetSkill(skill);
			
			Passion adjustedPassion = (Passion)MathUtility.MoveTowardsOperationClamped((byte)realSkill.passion, (byte)target, delta, operation);
			yield return MessageSkillPassionChanged.Translate(pawn.LabelShort, skill.label, adjustedPassion.ToString().ToLower(), ParseCause(cause));
			realSkill.passion = adjustedPassion;
			// We're done here.
			yield break;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}
			if (skill == null)
			{
				yield return "skill is null";
			}
		}
	}
}
