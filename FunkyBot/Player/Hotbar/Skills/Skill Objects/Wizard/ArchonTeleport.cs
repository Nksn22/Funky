﻿using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Common;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.Wizard
{
	 public class ArchonTeleport : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=10000;
				ExecutionType=AbilityExecuteFlags.ClusterLocation|AbilityExecuteFlags.ZigZagPathing;
				WaitVars=new WaitLoops(1, 1, true);
				Range=48;
				UseageType=AbilityUseage.Anywhere;
				IsASpecialMovementPower = true;
				Priority=AbilityPriority.High;
				PreCast=new SkillPreCast((AbilityPreCastFlags.CheckPlayerIncapacitated|AbilityPreCastFlags.CheckCanCast));

				ClusterConditions.Add(new SkillClusterConditions(5d, 48f, 2, false, minDistance: 15f));

				FcriteriaCombat=() =>
				{
					return ((Bot.Settings.Wizard.bTeleportFleeWhenLowHP&&
					         (Bot.Character.Data.dCurrentHealthPct<0.5d)||
					         (Bot.Targeting.Cache.RequiresAvoidance))
					        ||
					        (Bot.Settings.Wizard.bTeleportIntoGrouping&&LastConditionPassed==ConditionCriteraTypes.Cluster)
					        ||
					        (!Bot.Settings.Wizard.bTeleportFleeWhenLowHP&&!Bot.Settings.Wizard.bTeleportIntoGrouping));

				};
				FCombatMovement=v =>
				{
					float fDistanceFromTarget=Bot.Character.Data.Position.Distance(v);
					if (!Bot.Character.Class.bWaitingForSpecial&&Funky.Difference(Bot.Character.Data.Position.Z, v.Z)<=4&&fDistanceFromTarget>=25f)
					{
						if (fDistanceFromTarget>50f)
							return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, 50f);
						return v;
					}

					return Vector3.Zero;
				};
				FOutOfCombatMovement=v =>
				{
					float fDistanceFromTarget=Bot.Character.Data.Position.Distance(v);
					if (Funky.Difference(Bot.Character.Data.Position.Z, v.Z)<=4&&fDistanceFromTarget>=20f)
					{
						if (fDistanceFromTarget>50f)
							return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, 50f);
						return v;
					}

					return Vector3.Zero;
				};
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
				get { return SNOPower.Wizard_Archon_Teleport; }
		  }
	 }
}
