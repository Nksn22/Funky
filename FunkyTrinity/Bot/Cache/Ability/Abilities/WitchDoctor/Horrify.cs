﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.WitchDoctor
{
	public class Horrify : Ability, IAbility
	{
		public Horrify() : base()
		{
		}



		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.Self;
			WaitVars = new WaitLoops(0, 0, true);
			Cost = 37;
			UseFlagsType=AbilityUseFlags.Anywhere;
			Priority = AbilityPriority.Low;
			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated | CastingConditionTypes.CheckCanCast |
			                     CastingConditionTypes.CheckEnergy);

			Fcriteria = new Func<bool>(() =>
			{
				return Bot.Character.dCurrentHealthPct <= 0.60;
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
			get { return SNOPower.Witchdoctor_Horrify; }
		}
	}
}
