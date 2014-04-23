﻿using System;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.DemonHunter
{
	 public class Sentry : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=12000;
				ExecutionType=SkillExecutionFlags.Self;
				WaitVars=new WaitLoops(0, 0, true);
				Cost=30;
				UseageType=SkillUseage.Anywhere;
				Priority=SkillPriority.High;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckEnergy|SkillPrecastFlags.CheckRecastTimer|
				                          SkillPrecastFlags.CheckPlayerIncapacitated));

				FcriteriaCombat=() => Bot.Character.Class.LastUsedAbility.Power!=SNOPower.DemonHunter_Sentry&&
				                      (Bot.Targeting.Cache.FleeingLastTarget||DateTime.Now.Subtract(Bot.Targeting.Cache.LastFleeAction).TotalMilliseconds<1000)||
				                      (Bot.Targeting.Cache.Environment.iElitesWithinRange[(int)RangeIntervals.Range_40]>=1||
				                       Bot.Targeting.Cache.Environment.iAnythingWithinRange[(int)RangeIntervals.Range_40]>=2);
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
				else
				{
					 Skill p=(Skill)obj;
					 return Power==p.Power;
				}
		  }

		  #endregion

		  public override SNOPower Power
		  {
				get { return SNOPower.DemonHunter_Sentry; }
		  }
	 }
}
