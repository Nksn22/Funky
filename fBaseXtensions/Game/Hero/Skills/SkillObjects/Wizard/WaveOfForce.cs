﻿using fBaseXtensions.Game.Hero.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace fBaseXtensions.Game.Hero.Skills.SkillObjects.Wizard
{
	 public class WaveOfForce : Skill
	 {
		 public override SkillExecutionFlags ExecutionType { get { return SkillExecutionFlags.Buff; } }

		 public override void Initialize()
		  {
				Cooldown=2000;
				
				WaitVars=new WaitLoops(1, 2, true);
				Cost=25;
				Range = 25;
				
				Priority=SkillPriority.Medium;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated|SkillPrecastFlags.CheckEnergy|
				                          SkillPrecastFlags.CheckCanCast));
				ClusterConditions.Add(new SkillClusterConditions(15d, 20f, 2, false, useRadiusDistance: true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.CloseDistance | TargetProperties.Weak));
				FcriteriaCombat = (u) => !FunkyGame.Hero.Class.bWaitingForSpecial;
		  }

		  public override SNOPower Power
		  {
				get { return SNOPower.Wizard_WaveOfForce; }
		  }
	 }
}
