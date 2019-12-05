using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFCnd_HasTrait : TransformationCondition
	{
		public TraitDef trait;
		public int degreeMin = int.MinValue;
		public int degreeMax = int.MaxValue;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			Trait realTrait = pawn.story?.traits?.GetTrait(trait);

			return realTrait != null 
				? realTrait.Degree.Between(degreeMin, degreeMax)
				: false;
		}
	}
}
