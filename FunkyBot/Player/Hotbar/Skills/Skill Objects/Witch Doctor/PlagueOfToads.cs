﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.WitchDoctor
{
	public class PlagueOfToads : Skill
	{
		public override double Cooldown { get { return 5; } }

		public override bool IsDestructiblePower { get { return true; } }
		public override bool IsPrimarySkill { get { return true; } }
		public override bool IsRanged { get { return true; } }
		public override bool IsProjectile { get { return true; } }

		public override SkillUseage UseageType { get { return SkillUseage.Combat; } }

		public override SkillExecutionFlags ExecutionType { get { return SkillExecutionFlags.Target; } }

		public override void Initialize()
		{
			Cost = 10;
			Range = 30;
			Priority = SkillPriority.Low;
			PreCast = new SkillPreCast((SkillPrecastFlags.CheckCanCast));

		}

		public override SNOPower Power
		{
			get { return SNOPower.Witchdoctor_PlagueOfToads; }
		}
	}
}
