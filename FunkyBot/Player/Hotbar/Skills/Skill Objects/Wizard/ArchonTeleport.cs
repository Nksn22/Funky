﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Common;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	public class ArchonTeleport : Skill
	{
		public override double Cooldown { get { return 10000; } }

		public override bool IsMovementSkill { get { return true; } }

		public override SkillExecutionFlags ExecutionType { get { return SkillExecutionFlags.ClusterLocation | SkillExecutionFlags.ZigZagPathing; } }

		public override void Initialize()
		{
			WaitVars = new WaitLoops(1, 1, true);
			Range = 48;


			Priority = SkillPriority.High;
			PreCast = new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated | SkillPrecastFlags.CheckCanCast));

			ClusterConditions.Add(new SkillClusterConditions(5d, 48f, 2, false, minDistance: 15f));

			FcriteriaCombat = () =>
			{
				return ((Bot.Settings.Wizard.bTeleportFleeWhenLowHP &&
						 (Bot.Character.Data.dCurrentHealthPct < 0.5d) ||
						 (Bot.Targeting.Cache.RequiresAvoidance))
						||
						(Bot.Settings.Wizard.bTeleportIntoGrouping && LastConditionPassed == ConditionCriteraTypes.Cluster)
						||
						(!Bot.Settings.Wizard.bTeleportFleeWhenLowHP && !Bot.Settings.Wizard.bTeleportIntoGrouping));

			};
			FCombatMovement = v =>
			{
				float fDistanceFromTarget = Bot.Character.Data.Position.Distance(v);
				if (!Bot.Character.Class.bWaitingForSpecial && Funky.Difference(Bot.Character.Data.Position.Z, v.Z) <= 4 && fDistanceFromTarget >= 25f)
				{
					if (fDistanceFromTarget > 50f)
						return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, 50f);
					return v;
				}

				return Vector3.Zero;
			};
			FOutOfCombatMovement = v =>
			{
				float fDistanceFromTarget = Bot.Character.Data.Position.Distance(v);
				if (Funky.Difference(Bot.Character.Data.Position.Z, v.Z) <= 4 && fDistanceFromTarget >= 20f)
				{
					if (fDistanceFromTarget > 50f)
						return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, 50f);
					return v;
				}

				return Vector3.Zero;
			};
		}


		public override SNOPower Power
		{
			get { return SNOPower.Wizard_Archon_Teleport; }
		}
	}
}
