﻿using System;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.WitchDoctor
{
	 public class BigBadVoodoo : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=120000;
				ExecutionType=SkillExecutionFlags.Self;
				WaitVars=new WaitLoops(0, 0, true);
				UseageType=SkillUseage.Anywhere;
				Priority=SkillPriority.Medium;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated|SkillPrecastFlags.CheckCanCast));

				ElitesWithinRangeConditions=new Tuple<RangeIntervals, int>(RangeIntervals.Range_6, 1);
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.IsSpecial, 12));
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
				get { return SNOPower.Witchdoctor_BigBadVoodoo; }
		  }
	 }
}
