﻿using System;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Common;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.HotBar.Skills.DemonHunter
{
	 public class Vault : Skill
	 {
		 public override void Initialize()
		  {
				Cooldown=400;
				ExecutionType=SkillExecutionFlags.ZigZagPathing;
				WaitVars=new WaitLoops(1, 2, true);
				Cost=8;
				SecondaryEnergy=true;
				Range=20;
				UseageType=SkillUseage.Combat;
				Priority=SkillPriority.Medium;
				IsMovementSkill = true;
				PreCast=new SkillPreCast((SkillPrecastFlags.CheckPlayerIncapacitated|SkillPrecastFlags.CheckCanCast|
				                          SkillPrecastFlags.CheckEnergy|SkillPrecastFlags.CheckRecastTimer));
				//SingleUnitCondition.Add(new UnitTargetConditions(TargetProperties.None, 10);
				//UnitsWithinRangeConditions=new Tuple<RangeIntervals, int>(RangeIntervals.Range_6, 1);

				FCombatMovement=v =>
				{
					float fDistanceFromTarget=Bot.Character.Data.Position.Distance(v);
					if (!Bot.Character.Class.bWaitingForSpecial&&Funky.Difference(Bot.Character.Data.Position.Z, v.Z)<=4&&fDistanceFromTarget>=18f&&
					    (LastUsedMilliseconds>=Bot.Settings.DemonHunter.iDHVaultMovementDelay))
					{
						return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, Math.Max(fDistanceFromTarget, 35f));
					}

					return Vector3.Zero;
				};
				FOutOfCombatMovement=v =>
				{
					float fDistanceFromTarget=Bot.Character.Data.Position.Distance(v);
					if (Funky.Difference(Bot.Character.Data.Position.Z, v.Z)<=4&&fDistanceFromTarget>=18f&&
					    (LastUsedMilliseconds>=Bot.Settings.DemonHunter.iDHVaultMovementDelay))
					{
						return MathEx.CalculatePointFrom(v, Bot.Character.Data.Position, Math.Max(fDistanceFromTarget, 35f));
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
				get { return SNOPower.DemonHunter_Vault; }
		  }
	 }
}
