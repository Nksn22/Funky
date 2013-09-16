﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.Monk
{
	public class FistsofThunder : Ability, IAbility
	{
		public FistsofThunder() : base()
		{
		}



		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.ClusterTargetNearest | PowerExecutionTypes.Target;
			WaitVars = new WaitLoops(0, 1, false);
			UseFlagsType=AbilityUseFlags.Combat;
			Priority = AbilityPriority.None;
			IsADestructiblePower=true;
			Range = Bot.Class.RuneIndexCache[SNOPower.Monk_FistsofThunder] == 0 ? 25 : 12;

			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated);
			ClusterConditions = new ClusterConditions(5d, 20f, 1, true);
			TargetUnitConditionFlags = new UnitTargetConditions(TargetProperties.None);

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
			get { return SNOPower.Monk_FistsofThunder; }
		}
	}
}
