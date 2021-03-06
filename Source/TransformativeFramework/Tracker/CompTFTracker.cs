﻿using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class CompTFTracker : ThingComp
	{
		public void ClearData(TransformationAction owner, string key)
		{
			savedData.Find(sd => sd.owner == owner)?.data?.Remove(key);
		}

		public T LoadData<T>(TransformationAction owner, string key)
		{
			Dictionary<string, object> data = savedData.Find(sd => sd.owner == owner)?.data;
			return (data != null) && data.TryGetValue(key, out object value) ? (T)value : default(T);
		}

		public void SaveData(TransformationAction owner, string key, object value)
		{
			if (owner.refName.NullOrEmpty())
			{
				string def = owner.Transformation.Def.defName;
				string tf  = owner.Transformation.Def.transformations.IndexOf(owner.Transformation).ToString();
				string act = owner.Transformation.actions.IndexOf(owner).ToString();
				Log.Warning($"TransformationAction of type {owner.GetType()} at position {def}.transformations[{tf}].actions[{act}] is saving data but has no refName. This will cause errors during loading.");
			}

			TFDataObject dataObject = savedData.Find(sd => sd.owner == owner);

			if (dataObject == null)
			{
				dataObject = new TFDataObject(owner);
			}
			dataObject.data.Add(key, value);
		}

		public override void PostExposeData()
		{
			Scribe_Collections.Look(ref savedData, nameof(savedData), LookMode.Deep);

			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (savedData == null)
				{
					savedData = new List<TFDataObject>();
				}
			}
		}

		private List<TFDataObject> savedData = new List<TFDataObject>();
	}
}
