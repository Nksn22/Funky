﻿using System;
using Zeta;
using Zeta.Common;
using Zeta.CommonBot;
using Zeta.Internals.Actors;

namespace FunkyTrinity.ability.Abilities.Barb
{
	public class Cleave : Ability, IAbility
	{
		public Cleave() : base()
		{
		}

		public override SNOPower Power
		{
			get { return SNOPower.Barbarian_Cleave; }
		}

		public override int RuneIndex { get { return Bot.Class.RuneIndexCache.ContainsKey(this.Power)?Bot.Class.RuneIndexCache[this.Power]:-1; } }

		public override void Initialize()
		{
			ExecutionType = AbilityUseType.Target | AbilityUseType.ClusterTargetNearest;
			WaitVars = new WaitLoops(0, 2, true);
			Cost = 0;
			Range = 10;
			UseageType=AbilityUseage.Combat;
			Priority = AbilityPriority.None;
			PreCastConditions = (AbilityConditions.CheckPlayerIncapacitated);
			ClusterConditions = new ClusterConditions(4d, 10f, 2, true);
			TargetUnitConditionFlags = new UnitTargetConditions(TargetProperties.None);
		}
		public override void InitCriteria()
		{
			 base.AbilityTestConditions=new AbilityUsablityTests(this);
		}
		#region IAbility
		public override int GetHashCode()
		{
			 return (int)this.Power;
		}
		public override bool Equals(object obj)
		{
			 //Check for null and compare run-time types. 
			 if (obj==null||this.GetType()!=obj.GetType())
			 {
				  return false;
			 }
			 else
			 {
				  Ability p=(Ability)obj;
				  return this.Power==p.Power;
			 }
		}

	
		#endregion
	}
}