﻿using System;
using System.Linq;
using FunkyBot.Cache.Objects;
using FunkyBot.Player.HotBar.Skills;
using FunkyBot.Player.HotBar.Skills.Wizard;
using Zeta.Game;
using Zeta.Common;
using System.Collections.Generic;
using Zeta.Game.Internals.Actors;
using Logger = FunkyBot.Misc.Logger;

namespace FunkyBot.Player.Class
{

	internal class Wizard : PlayerClass
	{
		public Wizard()
		{

			if (Bot.Character.Data.equipment.EquippedItems.Any(i => i.ThisRealName.Contains("Serpent's Sparker")))
			{
				Bot.Settings.Wizard.SerpentSparker = true;
				Logger.DBLog.DebugFormat("Wizard Can Cast Two Hydras!");
			}
			else
			{
				Bot.Settings.Wizard.SerpentSparker = false;
			}
			Logger.DBLog.DebugFormat("[Funky] Using Wizard Player Class");
		}

		//Base class for each individual class!
		public override ActorClass AC { get { return ActorClass.Wizard; } }

		private readonly HashSet<SNOAnim> knockbackanims_Male = new HashSet<SNOAnim>
		{
					 SNOAnim.Wizard_Male_Archon_knockback_land,
					 SNOAnim.Wizard_Male_Bow_Knockback_End_01,
					 SNOAnim.Wizard_Male_HTH_Orb_Knockback_End_01,
					 SNOAnim.Wizard_Male_1HS_Orb_Knockback_End_01,
					 SNOAnim.Wizard_Male_HTH_Knockback_End_01,
					 SNOAnim.Wizard_Male_STF_Knockback_End_01,
					 SNOAnim.Wizard_Male_1HS_Knockback_End_01,
					 //
		};
		private readonly HashSet<SNOAnim> knockbackanims_Female = new HashSet<SNOAnim>
		{
					 SNOAnim.Wizard_Female_Archon_knockback_land,
					 SNOAnim.Wizard_Female_1HS_Orb_Knockback_Land_01,
					 SNOAnim.Wizard_Female_STF_Knockback_01_Land,
					 SNOAnim.Wizard_Female_1HS_Knockback_End_01,
					 //
		};
		internal override HashSet<SNOAnim> KnockbackLandAnims
		{
			get
			{
				return Bot.Character.Data.SnoActor == SNOActor.Wizard_Female ? knockbackanims_Female : knockbackanims_Male;
			}
		}
		internal override Skill DefaultAttack
		{
			get { return new WeaponRangedWand(); }
		}

		internal override void GenerateNewZigZagPath()
		{
			Vector3 loc;
			//Low HP -- Flee Teleport
			if (Bot.Settings.Wizard.bTeleportFleeWhenLowHP && Bot.Character.Data.dCurrentHealthPct < 0.5d && (Bot.NavigationCache.AttemptFindSafeSpot(out loc, Bot.Targeting.Cache.CurrentTarget.Position, Bot.Settings.Plugin.FleeingFlags)))
				Bot.NavigationCache.vSideToSideTarget = loc;
			else
				Bot.NavigationCache.vSideToSideTarget = Bot.NavigationCache.FindZigZagTargetLocation(Bot.Targeting.Cache.CurrentTarget.Position, Bot.Targeting.Cache.CurrentTarget.CentreDistance, true);
		}
		internal override int MainPetCount
		{
			get
			{
				return Bot.Character.Data.PetData.WizardHydra;
			}
		}
		internal override bool IsMeleeClass
		{
			get
			{
				return false;
			}
		}

		///<summary>
		///Used to check for a secondary hotbar set. Currently only used for wizards with Archon.
		///</summary>
		internal override bool SecondaryHotbarBuffPresent()
		{

			bool ArchonBuffPresent = HotBar.HasBuff(SNOPower.Wizard_Archon);

			//Confirm we don't have archon Ability without archon buff.
			bool RefreshNeeded = ((!ArchonBuffPresent && (Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Fire) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Cold) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Lightning)))
									   || (ArchonBuffPresent && (!Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast) && !Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Fire) && !Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Cold) && !Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Lightning))));

			if (RefreshNeeded)
			{
				Logger.DBLog.InfoFormat("Updating Hotbar abilities!");
				HotBar.RefreshHotbar();
				RecreateAbilities();
				return true;
			}

			return false;
		}

		internal override void RecreateAbilities()
		{
			base.RecreateAbilities();

			//Check for buff Archon -- and if we should add Cancel to abilities.
			if (Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Fire) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Cold) || Abilities.ContainsKey(SNOPower.Wizard_Archon_ArcaneBlast_Lightning))
			{
				Abilities.Add(SNOPower.Wizard_Archon_Cancel, new CancelArchonBuff());
			}
		}
		internal override Skill CreateAbility(SNOPower Power)
		{
			WizardActiveSkills power = (WizardActiveSkills)Enum.ToObject(typeof(WizardActiveSkills), (int)Power);
			switch (power)
			{
				case WizardActiveSkills.Wizard_Electrocute:
					return new Electrocute();
				case WizardActiveSkills.Wizard_SlowTime:
					return new SlowTime();
				case WizardActiveSkills.Wizard_ArcaneOrb:
					return new ArcaneOrb();
				case WizardActiveSkills.Wizard_Blizzard:
					return new Blizzard();
				case WizardActiveSkills.Wizard_FrostNova:
					return new FrostNova();
				case WizardActiveSkills.Wizard_Hydra:
					return new Hydra();
				case WizardActiveSkills.Wizard_MagicMissile:
					return new MagicMissile();
				case WizardActiveSkills.Wizard_ShockPulse:
					return new ShockPulse();
				case WizardActiveSkills.Wizard_WaveOfForce:
					return new WaveOfForce();
				case WizardActiveSkills.Wizard_Meteor:
					return new Meteor();
				case WizardActiveSkills.Wizard_SpectralBlade:
					return new SpectralBlade();
				case WizardActiveSkills.Wizard_IceArmor:
					return new IceArmor();
				case WizardActiveSkills.Wizard_StormArmor:
					return new StormArmor();
				case WizardActiveSkills.Wizard_DiamondSkin:
					return new DiamondSkin();
				case WizardActiveSkills.Wizard_MagicWeapon:
					return new MagicWeapon();
				case WizardActiveSkills.Wizard_EnergyTwister:
					return new EnergyTwister();
				case WizardActiveSkills.Wizard_EnergyArmor:
					return new EnergyArmor();
				case WizardActiveSkills.Wizard_ExplosiveBlast:
					return new ExplosiveBlast();
				case WizardActiveSkills.Wizard_Disintegrate:
					return new Disintegrate();
				case WizardActiveSkills.Wizard_RayOfFrost:
					return new RayOfFrost();
				case WizardActiveSkills.Wizard_MirrorImage:
					return new MirrorImage();
				case WizardActiveSkills.Wizard_Familiar:
					return new Familiar();
				case WizardActiveSkills.Wizard_ArcaneTorrent:
					return new ArcaneTorrent();
				case WizardActiveSkills.Wizard_Archon:
					return new Archon();
				case WizardActiveSkills.Wizard_Archon_ArcaneStrike:
					return new ArchonArcaneStrike();
				case WizardActiveSkills.Wizard_Archon_DisintegrationWave:
					return new ArchonDisintegrationWave();
				case WizardActiveSkills.Wizard_Archon_SlowTime:
					return new ArchonSlowTime();
				case WizardActiveSkills.Wizard_Archon_ArcaneBlast:
					return new ArchonArcaneBlast();
				case WizardActiveSkills.Wizard_Archon_Teleport:
					return new ArchonTeleport();
				case WizardActiveSkills.Wizard_Teleport:
					return new Teleport();
				case WizardActiveSkills.Wizard_BlackHole:
					return new BlackHole();

				case WizardActiveSkills.Wizard_Archon_ArcaneBlast_Cold:
					return new ArchonArcaneBlastCold();
				case WizardActiveSkills.Wizard_Archon_ArcaneBlast_Fire:
					return new ArchonArcaneBlastFire();
				case WizardActiveSkills.Wizard_Archon_ArcaneBlast_Lightning:
					return new ArchonArcaneBlastLightning();

				case WizardActiveSkills.Wizard_Archon_ArcaneStrike_Cold:
					return new ArchonArcaneStrikeCold();
				case WizardActiveSkills.Wizard_Archon_ArcaneStrike_Fire:
					return new ArchonArcaneStrikeFire();
				case WizardActiveSkills.Wizard_Archon_ArcaneStrike_Lightning:
					return new ArchonArcaneStrikeLightning();

				case WizardActiveSkills.Wizard_Archon_DisintegrationWave_Cold:
					return new ArchonDisintegrationWaveCold();
				case WizardActiveSkills.Wizard_Archon_DisintegrationWave_Fire:
					return new ArchonDisintegrationWaveFire();
				case WizardActiveSkills.Wizard_Archon_DisintegrationWave_Lightning:
					return new ArchonDisintegrationWaveLightning();

				default:
					return DefaultAttack;
			}

		}


		enum WizardActiveSkills
		{
			Wizard_Electrocute = 1765,
			Wizard_SlowTime = 1769,
			Wizard_ArcaneOrb = 30668,
			Wizard_Blizzard = 30680,
			/*
								 Wizard_EnergyShield=30708,
			*/
			Wizard_FrostNova = 30718,
			Wizard_Hydra = 30725,
			Wizard_MagicMissile = 30744,
			Wizard_ShockPulse = 30783,
			Wizard_WaveOfForce = 30796,
			Wizard_Meteor = 69190,
			Wizard_SpectralBlade = 71548,
			Wizard_IceArmor = 73223,
			Wizard_StormArmor = 74499,
			Wizard_DiamondSkin = 75599,
			Wizard_MagicWeapon = 76108,
			Wizard_EnergyTwister = 77113,
			Wizard_EnergyArmor = 86991,
			Wizard_ExplosiveBlast = 87525,
			Wizard_Disintegrate = 91549,
			Wizard_RayOfFrost = 93395,
			Wizard_MirrorImage = 98027,
			Wizard_Familiar = 99120,
			Wizard_ArcaneTorrent = 134456,
			Wizard_Teleport = 168344,
			Wizard_BlackHole = 243141,

			Wizard_Archon = 134872,
			Wizard_Archon_ArcaneStrike = 135166,
			Wizard_Archon_DisintegrationWave = 135238,
			Wizard_Archon_SlowTime = 135663,
			Wizard_Archon_ArcaneBlast = 167355,
			Wizard_Archon_Teleport = 167648,

			Wizard_Archon_ArcaneBlast_Fire = 392691,
			Wizard_Archon_ArcaneBlast_Cold = 392692,
			Wizard_Archon_ArcaneBlast_Lightning = 392693,

			Wizard_Archon_ArcaneStrike_Cold = 392694,
			Wizard_Archon_ArcaneStrike_Fire = 392695,
			Wizard_Archon_ArcaneStrike_Lightning = 392696,

			Wizard_Archon_DisintegrationWave_Cold = 392697,
			Wizard_Archon_DisintegrationWave_Fire = 392698,
			Wizard_Archon_DisintegrationWave_Lightning = 392699,

		}
		/*
						enum WizardPassiveSkills
						{
							 Wizard_Passive_ColdBlooded=226301,
							 Wizard_Passive_Paralysis=226348,
							 Wizard_Passive_Conflagration=218044,
							 Wizard_Passive_CriticalMass=218153,
							 Wizard_Passive_ArcaneDynamo=208823,
							 Wizard_Passive_GalvanizingWard=208541,
							 Wizard_Passive_Illusionist=208547,
							 Wizard_Passive_Blur=208468,
							 Wizard_Passive_GlassCannon=208471,
							 Wizard_Passive_AstralPresence=208472,
							 Wizard_Passive_Evocation=208473,
							 Wizard_Passive_UnstableAnomaly=208474,
							 Wizard_Passive_TemporalFlux=208477,
							 Wizard_Passive_PowerHungry=208478,
							 Wizard_Passive_Prodigy=208493,

						}
		*/


	}

}