﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class MirrorImage : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=5000;
				ExecutionType=SkillExecutionFlags.Buff;
				WaitVars=new WaitLoops(1, 1, true);
				Cost=10;
				Range=48;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.High;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckCanCast));

				FcriteriaCombat=() => (Bot.Character.Data.dCurrentHealthPct<=0.50||
				                       Bot.Targeting.Cache.Environment.iAnythingWithinRange[(int)RangeIntervals.Range_30]>=5||Bot.Character.Data.bIsIncapacitated||
				                       Bot.Character.Data.bIsRooted||Bot.Targeting.Cache.CurrentTarget.ObjectIsSpecial);
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
				get { return SNOPower.Wizard_MirrorImage; }
		  }
	 }
}
