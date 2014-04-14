﻿using System;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class FrostNova : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=9000;
				ExecutionType=AbilityExecuteFlags.Target;
				WaitVars=new WaitLoops(0, 0, true);
				Range=14;
				UseageType=AbilityUseage.Anywhere;
				Priority=AbilityPriority.Medium;
				PreCast=new SkillPreCast((AbilityPreCastFlags.CheckPlayerIncapacitated|AbilityPreCastFlags.CheckCanCast));

				ClusterConditions.Add(new SkillClusterConditions(7d, 20f, 2, false));
				UnitsWithinRangeConditions=new Tuple<RangeIntervals, int>(RangeIntervals.Range_20, 1);
				ElitesWithinRangeConditions = new Tuple<RangeIntervals, int>(RangeIntervals.Range_20, 1);
				//SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 12);
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
				get { return SNOPower.Wizard_FrostNova; }
		  }
	 }
}
