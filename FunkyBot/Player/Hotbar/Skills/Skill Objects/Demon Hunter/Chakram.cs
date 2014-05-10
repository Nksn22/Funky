﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.DemonHunter
{
	 public class Chakram : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=5;
				ExecutionType=SkillExecutionFlags.ClusterTarget|SkillExecutionFlags.Target;
				WaitVars=new WaitLoops(0, 1, true);
				Cost=10;
				Range=50;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.Medium;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated|SkillPrecastFlags.CheckEnergy));

				ClusterConditions.Add(new SkillClusterConditions(4d, 40, 2, true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 50, 0.95d, TargetProperties.Normal));

				FcriteriaCombat=() =>
				{
					return ((!Bot.Character.Class.HotBar.HotbarPowers.Contains(SNOPower.DemonHunter_ClusterArrow))||
					        LastUsedMilliseconds>=110000);
				};
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
				get { return SNOPower.DemonHunter_Chakram; }
		  }
	 }
}
