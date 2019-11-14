using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class HediffComp_Transformative : HediffComp
	{
		public HediffCompProperties_Transformative Props => (HediffCompProperties_Transformative)props;
		
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Pawn.IsHashIntervalTick(Props.interval) && Rand.MTBEventOccurs(Props.mtbDays, 60000f, Props.interval))
			{
				Props.transformation.DoTransformations(Pawn, parent, Props.number);
			}
		}
	}
}
