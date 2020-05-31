using System.Collections.Generic;
using System.Linq;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class HediffUtility
	{
		public const string MessageHediffChanged = "TFFramework_MessageHediffChanged"; // "{0}'s {1} became {2} because of {3}."
		public const string MessageHediffGained  = "TFFramework_MessageHediffGained";  // "{0}'s gained {1} from {2}."
		public const string MessageHediffLost	 = "TFFramework_MessageHediffLost";	 // "{0}'s {1} was removed by {2}."

		public static IEnumerable<BodyPartRecord> GetPartsToAffect(Pawn pawn, List<BodyPartDef> partsToAffect)
		{
			if (partsToAffect.NullOrEmpty())
			{
				return new List<BodyPartRecord> { null };
			}
			else
			{
				return pawn.health.hediffSet.GetNotMissingParts().Where(p => partsToAffect.Contains(p.def));
			}
		}
	}
}
