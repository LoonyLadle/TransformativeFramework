using RimWorld;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFCnd_HasSkill : TransformationCondition
   {
      public SkillDef skill;
      public int min = int.MinValue;
      public int max = int.MaxValue;

      // Return true if pawn has a trait of def traitDef between degreeMin and degreeMax.
      protected override bool CheckPartWorker(Pawn pawn, object cause)
      {
         SkillRecord realSkill = pawn.skills?.GetSkill(skill);
         
         if (realSkill == null)
         {
            return false;
         }
         else if (realSkill.TotallyDisabled)
         {
            return false;
         }
         else if (realSkill.Level < min || realSkill.Level > max)
         {
            return false;
         }
         else return true;
      }
   }
}
