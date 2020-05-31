using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFCnd_HasSkill : TransformationCondition
	{
		public SkillDef skill;
		public int levelMin = int.MinValue;
		public int levelMax = int.MaxValue;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			SkillRecord realSkill = pawn.skills?.GetSkill(skill);

			return realSkill != null 
				? !realSkill.TotallyDisabled && realSkill.Level.Between(levelMin, levelMax) 
				: false;
		}
	}
}
