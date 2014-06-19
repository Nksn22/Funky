﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class Meteor : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=1000;
				ExecutionType=SkillExecutionFlags.ClusterTarget|SkillExecutionFlags.Target;
				WaitVars=new WaitLoops(1, 2, true);
				Cost=50;
				Range=50;
				IsRanged=true;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.Medium;
				PreCast = new SkillPreCast(SkillPrecastFlags.CheckPlayerIncapacitated | SkillPrecastFlags.CheckCanCast);
				ClusterConditions.Add(new SkillClusterConditions(4d, 50f, 2, true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, maxdistance: 45, MinimumHealthPercent: 0.95d, falseConditionalFlags: TargetProperties.Normal|TargetProperties.Fast));
				FcriteriaCombat=() => !Bot.Character.Class.bWaitingForSpecial;
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
				get { return SNOPower.Wizard_Meteor; }
		  }
	 }
}
