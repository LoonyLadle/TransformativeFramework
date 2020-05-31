using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFCnd_HasHediff : TransformationCondition
	{
		public HediffDef hediff;
		public bool mustBeVisible;
		public float severityMin = float.MinValue;
		public float severityMax = float.MaxValue;
		public int stageMin = int.MinValue;
		public int stageMax = int.MaxValue;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			Hediff realHediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediff, mustBeVisible);

			return realHediff != null
				? realHediff.CurStageIndex.Between(stageMin, stageMax) && realHediff.Severity.Between(severityMin, severityMax)
				: false;
		}
	}
}
