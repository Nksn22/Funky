﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Monk
{
	 public class LashingTailKick : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=250;
				ExecutionType=AbilityExecuteFlags.Target;
				WaitVars=new WaitLoops(1, 1, true);
				Cost=30;
				Range=10;
				Priority=AbilityPriority.Medium;
				UseageType=AbilityUseage.Combat;
				PreCast=new SkillPreCast((AbilityPreCastFlags.CheckEnergy|AbilityPreCastFlags.CheckCanCast|
				                          AbilityPreCastFlags.CheckRecastTimer|AbilityPreCastFlags.CheckPlayerIncapacitated));
				ClusterConditions.Add(new SkillClusterConditions(4d, 18f, 3, true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 10, falseConditionalFlags: TargetProperties.Normal));


				FcriteriaCombat=() => (!Bot.Character.Class.HotBar.HotbarPowers.Contains(SNOPower.Monk_SweepingWind)||
				                       (Bot.Character.Class.HotBar.HotbarPowers.Contains(SNOPower.Monk_SweepingWind)&&Bot.Character.Class.HotBar.HasBuff(SNOPower.Monk_SweepingWind)))&&
				                      (!Bot.Character.Class.bWaitingForSpecial||Bot.Character.Data.dCurrentEnergy>=Bot.Character.Class.iWaitingReservedAmount);
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
				get { return SNOPower.Monk_LashingTailKick; }
		  }
	 }
}
