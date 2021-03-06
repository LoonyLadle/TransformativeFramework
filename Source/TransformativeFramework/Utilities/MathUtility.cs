﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles

namespace LoonyLadle.TFs
{
	public static class MathUtility
	{
		public static bool Between(this float current, float min, float max)
		{
			return (current >= Math.Min(min, max)) && (current <= Math.Max(min, max));
		}

		public static bool Between(this int current, int min, int max)
		{
			return (current >= Math.Min(min, max)) && (current <= Math.Max(min, max));
		}

		public static float MoveTowardsOperationClamped(float current, float target, float maxDelta, Operation operation)
		{
			float min = operation.HasFlag(Operation.Decrease) ? float.MinValue : current;
			float max = operation.HasFlag(Operation.Increase) ? float.MaxValue : current;
			float result = Mathf.MoveTowards(current, target, maxDelta);
			return Mathf.Clamp(result, min, max);
		}
		
		public static int MoveTowardsOperationClamped(int current, int target, int maxDelta, Operation operation)
		{
			int min = operation.HasFlag(Operation.Decrease) ? int.MinValue : current;
			int max = operation.HasFlag(Operation.Increase) ? int.MaxValue : current;
			int result = MathUtility.MoveTowards(current, target, maxDelta);
			return Mathf.Clamp(result, min, max);
		}

		public static float MoveTowardsRepeat(float current, float target, float amount, float length = 1f)
		{
			if (current > (length / 2f))
			{
				current -= length;
			}
			if (target > (length / 2f))
			{
				target -= length;
			}
			return Mathf.Repeat(Mathf.MoveTowards(current, target, amount), length);
		}

		// Int version of Mathf.MoveTowards.
		public static int MoveTowards(int current, int target, int maxDelta)
		{
			return Mathf.Abs(target - current) <= maxDelta ? target : current + (Math.Sign(target - current) * maxDelta);
		}

		public static int Nearest(this IEnumerable<int> values, int nearestTo)
		{
			return values.Aggregate((x, y) => Math.Abs(x - nearestTo) < Math.Abs(y - nearestTo) ? x : y);
		}

		public static int NearestBetween(this IEnumerable<int> values, int nearestTo, int min, int max, int ifNoMatch = 0)
		{
			IEnumerable<int> valuesBetween = values.Where(x => x.Between(min, max));
			return valuesBetween.Any() ? valuesBetween.Nearest(nearestTo) : ifNoMatch;
		}
	}
}
