using System;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public abstract class TransformationPart
	{
		public Transformation Transformation { get; private set; }

		public abstract bool CheckPart(Pawn pawn, object cause);

		protected abstract bool CheckPartWorker(Pawn pawn, object cause);

		public virtual IEnumerable<string> ConfigErrors()
		{
			// Empty method for overriding.
			yield break;
		}

		public virtual void PostLoadSpecial(Transformation parent)
		{
			// Empty method for overriding.
		}

		public virtual void ResolveReferencesSpecial()
		{
			// Empty method for overriding.
		}

		public void ResolveReferencesSpecial(Transformation parent)
		{
			Transformation = parent;
			ResolveReferencesSpecial();
		}

		public static string ParseCause(object cause)
		{
			if (cause is Thing thing)
			{
				return thing.LabelShort;
			}
			else if (cause is Hediff hediff)
			{
				return hediff.LabelBase;
			}
			else
			{
				throw new ArgumentException("cause is not a known type.");
			}
		}
	}
}
