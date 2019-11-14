using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public abstract class TransformationAction : TransformationPart
	{
		public string refName;
		public bool ignoreInConditions;
		public MessageTypeDef messageType;

		public sealed override bool CheckPart(Pawn pawn, object cause) => CheckPartWorker(pawn, cause);

		public void ApplyPart(Pawn pawn, object cause, out IEnumerable<string> reports) => reports = ApplyPartWorker(pawn, cause);

		protected abstract IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause);

		public string GetUniqueLoadID()
		{
			return "TransformationAction_" + Transformation.Def.defName + "_" + refName;
		}

		public override void ResolveReferences()
		{
			if (messageType == null)
			{
				messageType = MessageTypeDefOf.PositiveEvent;
			}
		}
	}
}
