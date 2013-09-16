﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.Wizard
{
	public class EnergyArmor : Ability, IAbility
	{
		public EnergyArmor() : base()
		{
		}



		public override void Initialize()
		{
			 ExecutionType=PowerExecutionTypes.Buff;
			 WaitVars=new WaitLoops(1, 2, true);
			 Cost=25;
			 Counter=1;
			 UseFlagsType=AbilityUseFlags.Anywhere;
			 IsBuff=true;
			 Priority=AbilityPriority.High;
			 PreCastConditions=(CastingConditionTypes.CheckPlayerIncapacitated|CastingConditionTypes.CheckEnergy|
														CastingConditionTypes.CheckExisitingBuff);
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
			get { return SNOPower.Wizard_EnergyArmor; }
		}
	}
}
