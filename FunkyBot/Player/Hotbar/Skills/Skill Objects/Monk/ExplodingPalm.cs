﻿using System;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Monk
{
	 public class ExplodingPalm : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=5000;
				ExecutionType=AbilityExecuteFlags.Target;
				WaitVars=new WaitLoops(1, 1, true);
				Cost=40;
				Range=14;
				UseageType=AbilityUseage.Combat;
				Priority=AbilityPriority.Medium;
				PreCast=new SkillPreCast((AbilityPreCastFlags.CheckEnergy|AbilityPreCastFlags.CheckCanCast|
				                          AbilityPreCastFlags.CheckPlayerIncapacitated));

				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 14, falseConditionalFlags: TargetProperties.DOTDPS));

				FcriteriaCombat=() => (!Bot.Character.Class.bWaitingForSpecial||Bot.Character.Data.dCurrentEnergy>=Bot.Character.Class.iWaitingReservedAmount);
		  }

		  #region IAbility

		  public override int RuneIndex
		  {
				get { return Bot.Character.Class.HotBar.RuneIndexCache.ContainsKey(Power)?Bot.Character.Class.HotBar.RuneIndexCache[Power]:-1; }
		  }

		  public override int GetHashCode()
		  {
				return (int)Power;
		  }

		  public override bool Equals(object obj)
		  {
				//Check for null and compare run-time types. 
				if (obj==null||GetType()!=obj.GetType())
				{
					 return false;
				}
			  Skill p=(Skill)obj;
			  return Power==p.Power;
		  }

		  #endregion

		  public override SNOPower Power
		  {
				get { return SNOPower.Monk_ExplodingPalm; }
		  }
	 }
}
