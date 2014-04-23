﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Barb
{
	 public class Warcry : Skill
	 {
		 public override SNOPower Power
		  {
				get { return SNOPower.X1_Barbarian_WarCry_v2; }
		  }

		  public override int RuneIndex { get { return Bot.Character.Class.HotBar.RuneIndexCache.ContainsKey(Power)?Bot.Character.Class.HotBar.RuneIndexCache[Power]:-1; } }

		  public override void Initialize()
		  {
				Cooldown=20500;
				ExecutionType=SkillExecutionFlags.Buff;
				WaitVars=new WaitLoops(1, 1, true);
				Cost=0;
				Range=0;
				IsBuff=true;
				UseageType=SkillUseage.Anywhere;
				Priority=SkillPriority.High;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckCanCast|SkillPrecastFlags.CheckPlayerIncapacitated));
				FcriteriaBuff=() => !Bot.Character.Class.HotBar.HasBuff(SNOPower.X1_Barbarian_WarCry_v2);
				FcriteriaCombat = () => (!Bot.Character.Class.HotBar.HasBuff(SNOPower.X1_Barbarian_WarCry_v2)||
				                       (Bot.Character.Class.HotBar.PassivePowers.Contains(SNOPower.Barbarian_Passive_InspiringPresence)&&LastUsedMilliseconds>59)||
				                        Bot.Character.Data.dCurrentEnergyPct<0.10);

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
