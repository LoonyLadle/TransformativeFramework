using RimWorld;
using System.Collections.Generic;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
   public class TFAct_SkillLearn : TransformationAction
   {
      // The def of the skill to learn.
      public SkillDef skill;
      // How much experience in the skill to learn.
      public byte delta;

      private const string MessageSkillLeveled = "TFFramework_MessageSkillLeveled"; // "{0}'s skill in {1} is now {2} because of {3}."

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
         else return true;
      }

      protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
      {
         SkillRecord realSkill = pawn.skills.GetSkill(skill);
         int oldLevel = realSkill.Level;
         realSkill.Learn(delta, true);
         if (realSkill.Level != oldLevel)
         {
            yield return MessageSkillLeveled.Translate(pawn.LabelShort, skill.label, realSkill.Level, ParseCause(cause));
         }
         yield break;
      }

      public override IEnumerable<string> ConfigErrors()
      {
         foreach (string error in base.ConfigErrors())
         {
            yield return error;
         }
         if (skill == null)
         {
            yield return "skill is null";
         }
         if (delta == 0)
         {
            yield return "delta is zero";
         }
      }
   }
}
