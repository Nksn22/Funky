﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class ArchonDisintegrationWave : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=5;
				ExecutionType=SkillExecutionFlags.Target;
				WaitVars=new WaitLoops(0, 0, false);
				Range=48;
				IsRanged=true;
				IsChanneling=true;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.Low;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, -1, 10));
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
				get { return SNOPower.Wizard_Archon_DisintegrationWave; }
		  }
	 }

	public class ArchonDisintegrationWaveFire:ArchonDisintegrationWave
	{
		public override SNOPower Power
		{
			get { return SNOPower.Wizard_Archon_DisintegrationWave_Fire; }
		}
	}
	public class ArchonDisintegrationWaveCold : ArchonDisintegrationWave
	{
		public override SNOPower Power
		{
			get { return SNOPower.Wizard_Archon_DisintegrationWave_Cold; }
		}
	}
	public class ArchonDisintegrationWaveLightning : ArchonDisintegrationWave
	{
		public override SNOPower Power
		{
			get { return SNOPower.Wizard_Archon_DisintegrationWave_Lightning; }
		}
	}
}
