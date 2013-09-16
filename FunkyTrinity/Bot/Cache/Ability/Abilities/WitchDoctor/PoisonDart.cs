﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.WitchDoctor
{
	public class PoisonDart : Ability, IAbility
	{
		public PoisonDart() : base()
		{
		}



		public override int RuneIndex { get { return Bot.Class.RuneIndexCache.ContainsKey(this.Power)?Bot.Class.RuneIndexCache[this.Power]:-1; } }

		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.Target;
			WaitVars = new WaitLoops(0, 1, true);
			Cost = 10;
			Range = 48;
			IsRanged = true;
			IsProjectile=true;
			IsADestructiblePower=true;
			UseFlagsType=AbilityUseFlags.Combat;
			Priority = AbilityPriority.None;
			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated);

		}

		#region IAbility


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
			get { return SNOPower.Witchdoctor_PoisonDart; }
		}
	}
}
