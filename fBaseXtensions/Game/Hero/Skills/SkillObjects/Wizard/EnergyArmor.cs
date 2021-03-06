﻿using fBaseXtensions.Game.Hero.Skills.Conditions;
using Zeta.Game.Internals.Actors;

namespace fBaseXtensions.Game.Hero.Skills.SkillObjects.Wizard
{
	public class EnergyArmor : Skill
	{
		public override double Cooldown { get { return 115000; } }

		public override bool IsBuff { get { return true; } }

		public override SkillExecutionFlags ExecutionType { get { return SkillExecutionFlags.Buff; } }

		public override void Initialize()
		{
			WaitVars = new WaitLoops(1, 2, true);
			Cost = 25;
			Counter = 1;


			Priority = SkillPriority.High;
			PreCast = new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated | SkillPrecastFlags.CheckEnergy |
									  SkillPrecastFlags.CheckExisitingBuff));
		}

		public override void OnSuccessfullyUsed(bool reorderAbilities = true)
		{
			//Reset Character Max Energy!
			FunkyGame.Hero.MaxEnergy = 0;

			base.OnSuccessfullyUsed(reorderAbilities);
		}

		public override SNOPower Power
		{
			get { return SNOPower.Wizard_EnergyArmor; }
		}
	}
}
