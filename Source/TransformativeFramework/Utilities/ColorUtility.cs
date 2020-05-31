using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class ColorUtility
	{
		public static Color MoveTowards(Color current, Color target, float amount)
		{
			// Placeholder implementation.
			return new Color(Mathf.MoveTowards(current.r, target.r, amount), Mathf.MoveTowards(current.g, target.g, amount), Mathf.MoveTowards(current.b, target.b, amount));
		}

		public static Color MoveTowardsHSV(Color current, Color target, float amount, bool moveH = true, bool moveS = true, bool moveV = true)
		{
			Color.RGBToHSV(current, out float aH, out float aS, out float aV);
			Color.RGBToHSV(target,  out float bH, out float bS, out float bV);
			if (moveH) aH = MathUtility.MoveTowardsRepeat(aH, bH, amount);
			if (moveS) aS = Mathf.MoveTowards(aS, bS, amount);
			if (moveV) aV = Mathf.MoveTowards(aV, bV, amount);
			return Color.HSVToRGB(aH, aS, aV);
		}

		public static bool NullOrClear(this Color color) => (color == null) || (color == Color.clear);
	}
}
