﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Crusader
{
	public class SweepAttack : Skill
	{
		public override SNOPower Power
		{
			get { return SNOPower.X1_Crusader_SweepAttack; }
		}


		public override double Cooldown { get { return 5; } }

		public override WaitLoops WaitVars { get { return WaitLoops.Default; } }

		public override SkillExecutionFlags ExecutionType { get { return SkillExecutionFlags.Target | SkillExecutionFlags.ClusterTargetNearest; } }

		public override SkillUseage UseageType { get { return SkillUseage.Combat; } }

		public override void Initialize()
		{
			Range = 18;
			Cost = 20;
			Priority = SkillPriority.Medium;
			PreCast = new SkillPreCast(SkillPrecastFlags.CheckCanCast);

			ClusterConditions.Add(new SkillClusterConditions(10d, 25f, 3, true));
			SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, maxdistance: 20, MinimumHealthPercent: 0.95d, falseConditionalFlags: TargetProperties.Normal));
			SingleUnitCondition.Add(new UnitTargetConditions
			{
				TrueConditionFlags = TargetProperties.None,
				Criteria = () => Bot.Character.Data.dCurrentEnergyPct > 0.5d,
				FalseConditionFlags = TargetProperties.LowHealth,
			});
		}
	}
}