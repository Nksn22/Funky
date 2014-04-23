﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.WitchDoctor
{
	 public class LocustSwarm : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=8000;
				ExecutionType=SkillExecutionFlags.ClusterLocation|SkillExecutionFlags.Location;
				ClusterConditions.Add(new SkillClusterConditions(5d, 20f, 1, true, 0.25d));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 21, 0.5d, TargetProperties.DOTDPS));
				WaitVars=new WaitLoops(1, 1, true);
				Cost=196;
				Range=21;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.High;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated|SkillPrecastFlags.CheckCanCast|
				                          SkillPrecastFlags.CheckEnergy|SkillPrecastFlags.CheckRecastTimer));

				PreCast.Criteria += (s) => !Bot.Character.Class.HotBar.HasDebuff(SNOPower.Succubus_BloodStar);

				IsSpecialAbility=true;
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
				get { return SNOPower.Witchdoctor_Locust_Swarm; }
		  }
	 }
}
