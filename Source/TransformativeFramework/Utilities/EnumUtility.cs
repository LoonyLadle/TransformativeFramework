#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class EnumUtility
	{
		public static bool HasFlag(this Operation operation, Operation flag) => (operation & flag) == flag;
	}
}
