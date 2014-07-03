﻿using System;
using System.Linq;
using FunkyBot.Cache.Objects;
using FunkyBot.Player.HotBar.Skills;
using FunkyBot.Player.HotBar.Skills.Monk;
using Zeta.Common;
using System.Collections.Generic;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Logger = FunkyBot.Misc.Logger;

namespace FunkyBot.Player.Class
{

	internal class Monk : PlayerClass
	{
		//Base class for each individual class!
		public Monk()
		{
			//Monk Inna Full Set
			int InnaSetItemCount = Bot.Character.Data.equipment.EquippedItems.Count(i => i.ThisRealName.Contains("Inna"));
			if (InnaSetItemCount > 3 || InnaSetItemCount > 2 && Bot.Character.Data.equipment.RingOfGrandeur)
			{
				Logger.DBLog.InfoFormat("Monk has full inna set!");
				Bot.Settings.Monk.bMonkInnaSet = true;
			}
			else
				Bot.Settings.Monk.bMonkInnaSet = false;


			//Combo Strike???
			if (HotBar.PassivePowers.Contains(SNOPower.Monk_Passive_CombinationStrike))
			{
				Logger.DBLog.InfoFormat("Combination Strike Found!");
				Bot.Settings.Monk.bMonkComboStrike = true;
				int TotalAbilities = HotBar.HotbarSkills.Count(Skill => SpiritGeneratingAbilities.Contains(Skill.Power));
				Bot.Settings.Monk.iMonkComboStrikeAbilities = TotalAbilities;
			}
			else
			{
				Bot.Settings.Monk.bMonkComboStrike = false;
				Bot.Settings.Monk.iMonkComboStrikeAbilities = 0;
			}

			Logger.DBLog.DebugFormat("[Funky] Using Monk Player Class");

		}
		public override ActorClass AC { get { return ActorClass.Monk; } }
		private readonly HashSet<SNOPower> SpiritGeneratingAbilities = new HashSet<SNOPower>
				{
					 SNOPower.Monk_FistsofThunder,
					 SNOPower.Monk_WayOfTheHundredFists,
					 SNOPower.Monk_DeadlyReach,
					 SNOPower.Monk_CripplingWave,
				};
		private readonly HashSet<SNOAnim> knockbackanims_Male = new HashSet<SNOAnim>
		{
					 SNOAnim.Monk_Male_HTH_knockback_land_01,
					 SNOAnim.Monk_Male_1HF_knockback_land_01,
					 SNOAnim.Monk_Male_DW_SF_knockback_land_01,
					 SNOAnim.Monk_Male_DW_FF_knockback_land_01,
					 SNOAnim.Monk_Male_1HS_knockback_land,
					 SNOAnim.Monk_Male_STF_knockback_land,
					 SNOAnim.Monk_Male_2HT_knockback_land,
		};
		private readonly HashSet<SNOAnim> knockbackanims_Female = new HashSet<SNOAnim>
		{
					 SNOAnim.Monk_Female_STF_knockback_land,
					 SNOAnim.Monk_Female_1HS_knockback_land,
					 SNOAnim.Monk_Female_2HT_knockback_land,
					 SNOAnim.Monk_Female_HTH_knockback_land_01,
					 SNOAnim.Monk_Female_DW_SS_knockback_land_01,
					 SNOAnim.Monk_Female_DW_SF_knockback_land_01,
					 SNOAnim.Monk_Female_DW_FF_knockback_land_01,
					 SNOAnim.Monk_Female_1HF_knockback_land_01,
		};

		internal override HashSet<SNOAnim> KnockbackLandAnims
		{
			get
			{
				return Bot.Character.Data.SnoActor == SNOActor.Monk_Female ? knockbackanims_Female : knockbackanims_Male;
			}
		}
		internal override Skill DefaultAttack
		{
			get { return new WeaponMeleeInsant(); }
		}

		internal override int MainPetCount
		{
			get
			{
				return Bot.Character.Data.PetData.MysticAlly;
			}
		}
		internal override bool IsMeleeClass
		{
			get
			{
				return true;
			}
		}
		internal override bool ShouldGenerateNewZigZagPath()
		{
			return (DateTime.Now.Subtract(Bot.NavigationCache.lastChangedZigZag).TotalMilliseconds >= 1500 ||
					 (Bot.NavigationCache.vPositionLastZigZagCheck != Vector3.Zero && Bot.Character.Data.Position == Bot.NavigationCache.vPositionLastZigZagCheck && DateTime.Now.Subtract(Bot.NavigationCache.lastChangedZigZag).TotalMilliseconds >= 200) ||
					 Vector3.Distance(Bot.Character.Data.Position, Bot.NavigationCache.vSideToSideTarget) <= 4f ||
					 Bot.Targeting.Cache.CurrentTarget != null && Bot.Targeting.Cache.CurrentTarget.AcdGuid.HasValue && Bot.Targeting.Cache.CurrentTarget.AcdGuid.Value != Bot.NavigationCache.iACDGUIDLastWhirlwind);
		}
		internal override void GenerateNewZigZagPath()
		{
			float fExtraDistance = Bot.Targeting.Cache.CurrentTarget.CentreDistance <= 20f ? 5f : 1f;
			Bot.NavigationCache.vSideToSideTarget = Bot.NavigationCache.FindZigZagTargetLocation(Bot.Targeting.Cache.CurrentTarget.Position, Bot.Targeting.Cache.CurrentTarget.CentreDistance + fExtraDistance);
			// Resetting this to ensure the "no-spam" is reset since we changed our target location

			Bot.NavigationCache.iACDGUIDLastWhirlwind = Bot.Targeting.Cache.CurrentTarget.AcdGuid.HasValue ? Bot.Targeting.Cache.CurrentTarget.AcdGuid.Value : -1;
			Bot.NavigationCache.lastChangedZigZag = DateTime.Now;
		}


		internal override Skill CreateAbility(SNOPower Power)
		{
			MonkActiveSkills power = (MonkActiveSkills)Enum.ToObject(typeof(MonkActiveSkills), (int)Power);

			switch (power)
			{
				case MonkActiveSkills.Monk_BreathOfHeaven:
					return new BreathofHeaven();
				case MonkActiveSkills.Monk_MantraOfRetribution:
					return new MantraOfRetribution();
				case MonkActiveSkills.Monk_MantraOfHealing:
					return new MantraOfHealing();
				case MonkActiveSkills.Monk_MantraOfConviction:
					return new MantraOfConviction();
				case MonkActiveSkills.Monk_FistsofThunder:
					return new FistsofThunder();
				case MonkActiveSkills.Monk_DeadlyReach:
					return new DeadlyReach();
				case MonkActiveSkills.Monk_WaveOfLight:
					return new WaveOfLight();
				case MonkActiveSkills.Monk_SweepingWind:
					return new SweepingWind();
				case MonkActiveSkills.Monk_DashingStrike:
					return new DashingStrike();
				case MonkActiveSkills.Monk_Serenity:
					return new Serenity();
				case MonkActiveSkills.Monk_CripplingWave:
					return new CripplingWave();
				case MonkActiveSkills.Monk_SevenSidedStrike:
					return new SevenSidedStrike();
				case MonkActiveSkills.Monk_WayOfTheHundredFists:
					return new WayOfTheHundredFists();
				case MonkActiveSkills.Monk_InnerSanctuary:
					return new InnerSanctuary();
				case MonkActiveSkills.Monk_ExplodingPalm:
					return new ExplodingPalm();
				case MonkActiveSkills.Monk_LashingTailKick:
					return new LashingTailKick();
				case MonkActiveSkills.Monk_TempestRush:
					return new TempestRush();
				case MonkActiveSkills.Monk_MysticAlly:
					return new MysticAlly();
				case MonkActiveSkills.Monk_BlindingFlash:
					return new BlindingFlash();
				case MonkActiveSkills.Monk_MantraOfEvasion:
					return new MantraOfEvasion();
				case MonkActiveSkills.Monk_CycloneStrike:
					return new CycloneStrike();
				case MonkActiveSkills.Monk_Epiphany:
					return new Epiphany();
				default:
					return DefaultAttack;
			}
		}

		enum MonkActiveSkills
		{
			Monk_BreathOfHeaven = 69130,
			Monk_MantraOfRetribution = 375082,
			Monk_MantraOfHealing = 373143,
			Monk_MantraOfConviction = 375088,
			Monk_FistsofThunder = 95940,
			Monk_DeadlyReach = 96019,
			Monk_WaveOfLight = 96033,
			Monk_SweepingWind = 96090,
			Monk_DashingStrike = 312736,
			Monk_Serenity = 96215,
			Monk_CripplingWave = 96311,
			Monk_SevenSidedStrike = 96694,
			Monk_WayOfTheHundredFists = 97110,
			Monk_InnerSanctuary = 317076,
			Monk_ExplodingPalm = 97328,
			Monk_LashingTailKick = 111676,
			Monk_TempestRush = 121442,
			Monk_MysticAlly = 362102,
			Monk_BlindingFlash = 136954,
			Monk_MantraOfEvasion = 375049,
			Monk_CycloneStrike = 223473,
			Monk_Epiphany = 312307,
		}
		/*
						enum MonkPassiveSkills
						{
							 Monk_Passive_CombinationStrike=218415,
							 Monk_Passive_Resolve=211581,
							 Monk_Passive_TheGuardiansPath=209812,
							 Monk_Passive_Pacifism=209813,
							 Monk_Passive_SixthSense=209622,
							 Monk_Passive_SeizeTheInitiative=209628,
							 Monk_Passive_OneWithEverything=209656,
							 Monk_Passive_Transcendence=209250,
							 Monk_Passive_BeaconOfYtar=209104,
							 Monk_Passive_ExaltedSoul=209027,
							 Monk_Passive_FleetFooted=209029,
							 Monk_Passive_ChantOfResonance=156467,
							 Monk_Passive_NearDeathExperience=156484,
							 Monk_Passive_GuidingLight=156492,

						}
		*/

	}

}