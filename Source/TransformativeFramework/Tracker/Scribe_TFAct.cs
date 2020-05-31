using System.Collections.Generic;
using System.Xml;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class Scribe_TFAct
	{
		private const string Null = "null";

		public static void Look(ref TransformationAction value, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text = value == null ? Null : value.GetUniqueLoadID();
				Scribe_Values.Look(ref text, label, Null);
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = MyExtractor(Scribe.loader.curXmlParent[label]);
			}
		}
		
		private static TransformationAction MyExtractor(XmlNode subNode)
		{
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == Null)
			{
				return null;
			}
			TransformationAction result = TFActs.Find(act => act.GetUniqueLoadID() == subNode.InnerText);
			if (result == null)
			{
				Log.Error($"Could not load reference to {typeof(TransformationAction)} named {subNode.InnerText}");
			}
			return result;
		}

		private static List<TransformationAction> TFActs
		{
			get
			{
				if (cachedTFActs == null)
				{
					cachedTFActs = new List<TransformationAction>();

					foreach (TransformationDef def in DefDatabase<TransformationDef>.AllDefsListForReading)
					{
						foreach (Transformation tf in def.transformations)
						{
							foreach (TransformationAction act in tf.actions)
							{
								if (!act.refName.NullOrEmpty())
								{
									cachedTFActs.Add(act);
								}
							}
						}
					}
				}
				return cachedTFActs;
			}
		}

		private static List<TransformationAction> cachedTFActs;
	}
}
