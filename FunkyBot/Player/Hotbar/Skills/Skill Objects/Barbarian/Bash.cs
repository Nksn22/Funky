﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Barb
{

	 public class Bash : Skill
	 {
		 public override SNOPower Power
		  {
				get { return SNOPower.Barbarian_Bash; }
		  }

		  public override int RuneIndex { get { return Bot.Character.Class.HotBar.RuneIndexCache.ContainsKey(Power)?Bot.Character.Class.HotBar.RuneIndexCache[Power]:-1; } }

		  public override void Initialize()
		  {
				Cooldown=5;
				ExecutionType=SkillExecutionFlags.Target;
				WaitVars=new WaitLoops(0, 1, true);
				Cost=0;
				Range=10;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.Low;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckRecastTimer|SkillPrecastFlags.CheckCanCast|
				                          SkillPrecastFlags.CheckPlayerIncapacitated));
				

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
