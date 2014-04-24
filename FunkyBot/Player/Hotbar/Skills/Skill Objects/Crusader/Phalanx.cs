﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Crusader
{
	public class Phalanx : Skill
	{
		public override int RuneIndex { get { return Bot.Character.Class.HotBar.RuneIndexCache.ContainsKey(Power) ? Bot.Character.Class.HotBar.RuneIndexCache[Power] : -1; } }

		public override SNOPower Power
		{
			get { return SNOPower.x1_Crusader_Phalanx3; }
		}

		public override void Initialize()
		{
			Cost = 30;
			Cooldown = 5;
			Range = 20;
			Priority = SkillPriority.Medium;
			ExecutionType = SkillExecutionFlags.Location;

			WaitVars = new WaitLoops(0, 0, true);
			PreCast = new SkillPreCast(SkillPrecastFlags.None);
			UseageType = SkillUseage.Combat;
		}
	}
}
