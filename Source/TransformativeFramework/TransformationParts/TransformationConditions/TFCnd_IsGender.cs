﻿using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFCnd_IsGender : TransformationCondition
	{
		public Gender gender;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			return pawn.gender == gender;
		}
	}
}
