using System;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	[Flags]
	public enum Operation : byte
	{
		None = 0,
		Increase = 1,
		Decrease = 2,
		IncreaseOrDecrease = 3
	}
}
