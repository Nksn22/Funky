﻿using System;
using FunkyTrinity.Enums;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.DemonHunter
{
	public class EvasiveFire : Ability, IAbility
	{
		public EvasiveFire() : base()
		{
		}



		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.Target;
			WaitVars = new WaitLoops(1, 1, true);
			Cost = 0;
			UseFlagsType=AbilityUseFlags.Anywhere;
			Priority = AbilityPriority.Low;

			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated | CastingConditionTypes.CheckRecastTimer);
			UnitsWithinRangeConditions = new Tuple<RangeIntervals, int>(RangeIntervals.Range_15, 1);
		}

		#region IAbility

		public override int RuneIndex
		{
			get { return Bot.Class.RuneIndexCache.ContainsKey(this.Power) ? Bot.Class.RuneIndexCache[this.Power] : -1; }
		}

		public override int GetHashCode()
		{
			return (int) this.Power;
		}

		public override bool Equals(object obj)
		{
			//Check for null and compare run-time types. 
			if (obj == null || this.GetType() != obj.GetType())
			{
				return false;
			}
			else
			{
				Ability p = (Ability) obj;
				return this.Power == p.Power;
			}
		}

		#endregion

		public override SNOPower Power
		{
			get { return SNOPower.DemonHunter_EvasiveFire; }
		}
	}
}
