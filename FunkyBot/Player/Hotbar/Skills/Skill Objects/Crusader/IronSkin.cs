﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Crusader
{
	public class IronSkin : Skill
	{
		public override SNOPower Power
		{
			get { return SNOPower.X1_Crusader_IronSkin; }
		}

		public override void Initialize()
		{
			Cooldown = 30000;
			Range = 0;
			Priority = SkillPriority.High;
			ExecutionType = SkillExecutionFlags.Buff;

			WaitVars = new WaitLoops(0, 0, true);
			PreCast = new SkillPreCast(SkillPrecastFlags.CheckRecastTimer);
			UseageType = SkillUseage.Combat;

			//Make sure we are targeting something!
			SingleUnitCondition.Add(new UnitTargetConditions());
			FcriteriaCombat = () => Bot.Character.Data.dCurrentHealthPct < 0.5d;
		}
	}
}