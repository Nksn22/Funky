﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Barb
{
	 public class IgnorePain : Skill
	 {
		 public override SNOPower Power
		  {
				get { return SNOPower.Barbarian_IgnorePain; }
		  }

		  public override int RuneIndex { get { return Bot.Character.Class.HotBar.RuneIndexCache.ContainsKey(Power)?Bot.Character.Class.HotBar.RuneIndexCache[Power]:-1; } }

		  public override void Initialize()
		  {
				Cooldown=30200;
				ExecutionType=SkillExecutionFlags.Buff;
				WaitVars=new WaitLoops(0, 0, true);
				Cost=0;
				UseageType=SkillUseage.Anywhere;
				IsSpecialAbility=true;
				Priority=SkillPriority.High;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckRecastTimer|SkillPrecastFlags.CheckCanCast));

				FcriteriaCombat=() => Bot.Character.Data.dCurrentHealthPct<=0.45;
		  }

		  #region IAbility
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
	 }
}
