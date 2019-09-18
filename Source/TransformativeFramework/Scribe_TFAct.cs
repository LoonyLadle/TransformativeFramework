using System.Collections.Generic;
using System.Xml;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public static class Scribe_TFAct
   {
      public static void Look(ref TransformationAction_Referenceable value, string label)
      {
         if (Scribe.mode == LoadSaveMode.Saving)
         {
            string text = value == null ? "null" : value.GetUniqueLoadID();
            Scribe_Values.Look(ref text, label, "null", false);
         }
         else if (Scribe.mode == LoadSaveMode.LoadingVars)
         {
            value = MyExtractor(Scribe.loader.curXmlParent[label]);
         }
      }
      
      private static TransformationAction_Referenceable MyExtractor(XmlNode subNode)
      {
         if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
         {
            return null;
         }
         TransformationAction_Referenceable result = GetTFActs().Find(act => act.GetUniqueLoadID() == subNode.InnerText);
         if (result == null)
         {
            Log.Error($"Could not load reference to {typeof(TransformationAction_Referenceable)} named {subNode.InnerText}");
         }
         return result;
      }

      private static List<TransformationAction_Referenceable>GetTFActs()
      {
         if (cachedTFActs == null)
         {
            cachedTFActs = new List<TransformationAction_Referenceable>();

            foreach (TransformationDef def in DefDatabase<TransformationDef>.AllDefs)
            {
               foreach (Transformation tf in def.transformations)
               {
                  foreach (TransformationAction act in tf.actions)
                  {
                     if (act is TransformationAction_Referenceable)
                     {
                        cachedTFActs.Add(act as TransformationAction_Referenceable);
                     }
                  }
               }
            }
         }
         return cachedTFActs;
      }

      private static List<TransformationAction_Referenceable> cachedTFActs;
   }
}
