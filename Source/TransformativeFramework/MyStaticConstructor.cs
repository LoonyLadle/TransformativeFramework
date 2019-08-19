using Harmony;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   [StaticConstructorOnStartup]
   public static class MyStaticConstructor
   {
      static MyStaticConstructor()
      {
         HarmonyInstance harmony = HarmonyInstance.Create("rimworld.loonyladle.tfs");
         harmony.PatchAll();
      }
   }
}
