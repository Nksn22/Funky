﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class ArcaneOrb : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=100;
				ExecutionType=AbilityExecuteFlags.ClusterTarget|AbilityExecuteFlags.Target;
				WaitVars=new WaitLoops(1, 1, true);
				Cost=35;
				Range=45;
				IsRanged=true;
				IsProjectile=true;
				UseageType=AbilityUseage.Combat;
				Priority=AbilityPriority.Medium;
				PreCast=new SkillPreCast((AbilityPreCastFlags.CheckPlayerIncapacitated|AbilityPreCastFlags.CheckRecastTimer|
				                          AbilityPreCastFlags.CheckEnergy));
				ClusterConditions.Add(new SkillClusterConditions(5d, 40, 3, true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.IsSpecial, 40, 0.5d, TargetProperties.Fast | TargetProperties.MissileDampening));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.Boss, 40));
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
				get { return SNOPower.Wizard_ArcaneOrb; }
		  }
	 }
}
