﻿using fBaseXtensions.Game.Hero.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace fBaseXtensions.Game.Hero.Skills.SkillObjects.Demonhunter
{
	public class Chakram : Skill
	{
		public override double Cooldown { get { return 5; } }

		public override SkillUseage UseageType { get { return SkillUseage.Combat; } }

		private SkillExecutionFlags _executiontype = SkillExecutionFlags.ClusterTarget | SkillExecutionFlags.Target;
		public override SkillExecutionFlags ExecutionType { get { return _executiontype; } }

		public override void Initialize()
		{
			WaitVars = new WaitLoops(0, 1, true);
			Cost = 10;

			if (RuneIndex != 4)
			{
				IsDestructiblePower = true;
				Range = 50;
				Priority = SkillPriority.Medium;
				PreCast = new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated | SkillPrecastFlags.CheckEnergy));

				ClusterConditions.Add(new SkillClusterConditions(4d, 40, 2, true));
				SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, maxdistance: 50, MinimumHealthPercent: 0.95d, falseConditionalFlags: TargetProperties.Normal));

				FcriteriaCombat = (u) => !FunkyGame.Hero.Class.bWaitingForSpecial && ((!Hotbar.HasPower(SNOPower.DemonHunter_ClusterArrow)) ||
																				  LastUsedMilliseconds >= 110000);
			}
			else
			{//Shuriken Cloud
				_executiontype = SkillExecutionFlags.Buff;
				PreCast = new SkillPreCast(SkillPrecastFlags.CheckCanCast);
				Priority=SkillPriority.High;
				IsBuff = true;
				FcriteriaBuff = () => !Hotbar.HasBuff(SNOPower.DemonHunter_Chakram);
				FcriteriaCombat = (u) => !Hotbar.HasBuff(SNOPower.DemonHunter_Chakram);
			}
		}


		public override SNOPower Power
		{
			get { return SNOPower.DemonHunter_Chakram; }
		}
	}
}
