using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public abstract class TransformationCondition : TransformationPart
	{
		public bool invert;

		public sealed override bool CheckPart(Pawn pawn, object cause) => CheckPartWorker(pawn, cause) ^ invert;
	}
}
