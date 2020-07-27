using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public class TFAct_HediffChange : TransformationAction
	{
		// The def of the hediff to change.
		public HediffDef hediff;
		// The target severity of the hediff.
		public float target = float.MaxValue;
		// How much to move the hediff's severity towards the target severity.
		public float delta;
		// The intent of changing the hediff.
		// - Valid flags: Increase, Decrease, Remove
		public Operation operation = Operation.Normal;
		// Defs of body parts to affect.
		public List<BodyPartDef> partsToAffect;

		protected override bool CheckPartWorker(Pawn pawn, object cause)
		{
			IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(h => h.def == hediff);
			IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);

			foreach (BodyPartRecord part in parts)
			{
				IEnumerable<Hediff> partHediffs = hediffs.Where(h => h.Part == part);

				if (partHediffs.Any())
				{
					foreach (Hediff realHediff in partHediffs)
					{
						if (realHediff.Severity != target)
						{
							return true;
						}
					}
				}
				else
				{
					return true;
				}
			}
			return false;
		}

		protected override IEnumerable<string> ApplyPartWorker(Pawn pawn, object cause)
		{
			IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(h => h.def == hediff);
			IEnumerable<BodyPartRecord> parts = HediffUtility.GetPartsToAffect(pawn, partsToAffect);
			
			foreach (BodyPartRecord part in parts)
			{
				IEnumerable<Hediff> partHediffs = hediffs.Where(h => h.Part == part);

				if (partHediffs.Any())
				{
					foreach (Hediff realHediff in partHediffs)
					{
						float adjustedSeverity = MathUtility.MoveTowardsOperationClamped(realHediff.Severity, target, delta, operation);

						if (realHediff.Severity != adjustedSeverity)
						{
							int oldIndex = realHediff.CurStageIndex;

							realHediff.Severity = adjustedSeverity;

							if (realHediff.ShouldRemove)
							{
								yield return HediffUtility.MessageHediffLost.Translate(pawn.LabelShort, realHediff.LabelBase, StringUtility.ParseCause(cause));
							}
							else if (realHediff.CurStageIndex != oldIndex)
							{
								yield return HediffUtility.MessageHediffChanged.Translate(pawn.LabelShort, realHediff.LabelBase, realHediff.CurStage.label, StringUtility.ParseCause(cause));
							}
						}
					}
				}
				else
				{
					Hediff newHediff = HediffMaker.MakeHediff(hediff, pawn);

					if (delta != default(float))
					{
						newHediff.Severity = MathUtility.MoveTowardsOperationClamped(0, target, delta, operation);
					}
					if (newHediff.Visible)
					{
						HediffComp_Discoverable discoverable = newHediff.TryGetComp<HediffComp_Discoverable>();
						if (discoverable != null)
						{
							//discoverable.discovered = true;
							typeof(HediffComp_Discoverable).GetField("discovered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(discoverable, true);
						}
						yield return HediffUtility.MessageHediffGained.Translate(pawn.LabelShort, newHediff.Label, StringUtility.ParseCause(cause));
					}
					pawn.health.AddHediff(newHediff);
				}
			}
			// We're done here.
			yield break;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}
			if (hediff == null)
			{
				yield return "no hediff";
			}
			if (delta == 0)
			{
				yield return "delta is zero";
			}
			yield break;
		}
	}
}
