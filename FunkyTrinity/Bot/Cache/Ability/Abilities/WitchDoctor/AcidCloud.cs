﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.WitchDoctor
{
	public class AcidCloud : Ability, IAbility
	{
		public AcidCloud() : base()
		{
		}



		public override void Initialize()
		{
			ExecutionType = PowerExecutionTypes.ClusterTarget | PowerExecutionTypes.Target;

			WaitVars = new WaitLoops(1, 1, true);
			Cost = 250;
			Range = Bot.Class.RuneIndexCache[Power] == 4 ? 20 : 40;
			IsRanged = true;
			UseFlagsType=AbilityUseFlags.Combat;
			Priority = AbilityPriority.Low;
			PreCastConditions = (CastingConditionTypes.CheckPlayerIncapacitated | CastingConditionTypes.CheckEnergy |
			                     CastingConditionTypes.CheckCanCast | CastingConditionTypes.CheckRecastTimer);
			 
			 Fprecast = new Func<bool>(()=>{return !Bot.Class.HasDebuff(SNOPower.Succubus_BloodStar);});

			TargetUnitConditionFlags = new UnitTargetConditions(TargetProperties.IsSpecial,
				falseConditionalFlags: TargetProperties.Fast);
			ClusterConditions=new ClusterConditions(4d, Bot.Class.RuneIndexCache[Power]==4?20f:40f, 2, true);

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
			get { return SNOPower.Witchdoctor_AcidCloud; }
		}
	}
}
