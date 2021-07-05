using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class StringUtility
	{
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
				string causeAsString = cause.ToString();
				Log.Warning($"[TransformationFramework] cause \"{causeAsString}\" is not a known type.");
				return causeAsString;
			}
		}
	}
}
