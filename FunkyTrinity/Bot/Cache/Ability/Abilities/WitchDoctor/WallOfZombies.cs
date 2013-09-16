﻿using System;
using FunkyTrinity.Enums;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.WitchDoctor
{
	public class WallOfZombies : Ability, IAbility
	{
		public WallOfZombies() : base()
		{
		}



		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.Location;
			WaitVars = new WaitLoops(1, 1, true);
			Cost = 103;
			Range = 25;
			UseFlagsType=AbilityUseFlags.Combat;
			Priority = AbilityPriority.Low;
			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated | CastingConditionTypes.CheckEnergy |
			                     CastingConditionTypes.CheckCanCast);
			UnitsWithinRangeConditions = new Tuple<RangeIntervals, int>(RangeIntervals.Range_15, 3);
			ElitesWithinRangeConditions = new Tuple<RangeIntervals, int>(RangeIntervals.Range_15, 1);
			TargetUnitConditionFlags = new UnitTargetConditions(TargetProperties.IsSpecial, 25);
			Fcriteria = new Func<bool>(() =>
			{
				return !Bot.Class.bWaitingForSpecial;
			});
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
			get { return SNOPower.Witchdoctor_WallOfZombies; }
		}
	}
}
