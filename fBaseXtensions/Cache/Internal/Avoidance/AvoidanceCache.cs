﻿using System.Collections.Generic;
using fBaseXtensions.Cache.Internal.Enums;
using fBaseXtensions.Game;
using fBaseXtensions.Game.Hero;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace fBaseXtensions.Cache.Internal.Avoidance
{
	public static class AvoidanceCache
	{
		//Static Avoidance Areas in Halls Of Agony:
		//108012 -- FirePit, 34f Radius
		//89578 -- Inferno Wall -- 3f Height, 20f? Width


		internal static readonly AvoidanceValue[] AvoidancesDefault =
		  {
			  new AvoidanceValue(AvoidanceType.AdriaArcanePool, 1, 12,15), 
			  new AvoidanceValue(AvoidanceType.AdriaBlood, 1, 15,15), 
			  new AvoidanceValue(AvoidanceType.ArcaneSentry, 1, 14,15, 10), 
			  new AvoidanceValue(AvoidanceType.AzmodanBodies, 1, 47,5),
			  new AvoidanceValue(AvoidanceType.AzmodanFireball, 1, 16,5),
			  new AvoidanceValue(AvoidanceType.AzmodanPool, 1, 54,5),

			  new AvoidanceValue(AvoidanceType.BeeProjectile, 0.5, 2,5),
			  new AvoidanceValue(AvoidanceType.BelialGround, 1, 25,5),
			  new AvoidanceValue(AvoidanceType.BloodSpringSmall, 0.25, 12,5),
			  new AvoidanceValue(AvoidanceType.BloodSpringMedium, 0.25, 16,5),
			  new AvoidanceValue(AvoidanceType.BloodSpringLarge, 0.25, 20,5),

			  new AvoidanceValue(AvoidanceType.Dececrator, 0.80, 9,12, 10),
			  new AvoidanceValue(AvoidanceType.DemonicForge, 1, 25,12),
			  new AvoidanceValue(AvoidanceType.DestroyerDrop, 0.80, 8, 12),
			  new AvoidanceValue(AvoidanceType.DiabloMetor, 0.80, 28,5),
			  new AvoidanceValue(AvoidanceType.DiabloPrison, 1, 15,5),

			  new AvoidanceValue(AvoidanceType.Frozen, 1, 20,10, 6),
			  new AvoidanceValue(AvoidanceType.FrozenPulse, 0.75, 9,10),

			  new AvoidanceValue(AvoidanceType.GhomGasCloud, 0.4, 25,5),
			  new AvoidanceValue(AvoidanceType.GrotesqueExplosion, 0.50, 20, 5, 3),

			  new AvoidanceValue(AvoidanceType.LacuniBomb, 0.25, 2,5),

			  new AvoidanceValue(AvoidanceType.MageFirePool, 0.85, 10,5),
			  new AvoidanceValue(AvoidanceType.MalletLord, 0.70, 20, 5),

			  new AvoidanceValue(AvoidanceType.MalthaelDeathFog, 1, 20,5),
			  new AvoidanceValue(AvoidanceType.MalthaelDrainSoul, 0.75, 5,5),
			  new AvoidanceValue(AvoidanceType.MalthaelLightning, 0.50, 8,5),
			  new AvoidanceValue(AvoidanceType.MeteorImpact, 1, 18, 5, 23),
			  new AvoidanceValue(AvoidanceType.MoltenCore, 1, 20,5, 5),
			  new AvoidanceValue(AvoidanceType.MoltenTrail, 0.75, 6,5, 10),

			  new AvoidanceValue(AvoidanceType.OrbitFocalPoint, 0.75, 8,5),
			  new AvoidanceValue(AvoidanceType.OrbitProjectile, 0.75, 5,5),

			  new AvoidanceValue(AvoidanceType.PlagueCloud, 0.75, 19,5),
			  new AvoidanceValue(AvoidanceType.PlagueHand, 0.80, 15,5),
			  new AvoidanceValue(AvoidanceType.PoisonGas, 0.5, 9,5, 8),

			  new AvoidanceValue(AvoidanceType.RiftPoison, 0.80, 10, 10),

			  new AvoidanceValue(AvoidanceType.ShamanFireBall, 0.1, 2,5), 
			  new AvoidanceValue(AvoidanceType.SuccubusProjectile, 0.25, 2,5),

			  new AvoidanceValue(AvoidanceType.Teleport, 0,7,0),
			  new AvoidanceValue(AvoidanceType.Thunderstorm, 0.80, 9,10),
			  new AvoidanceValue(AvoidanceType.TreeSpore, 1, 14, 10, 6),
			  //new AvoidanceValue(AvoidanceType.WallOfFire, 0, 0, 0),
			  //?? value never makes it when deseralized, but is seralized.
			  new AvoidanceValue(AvoidanceType.None,0,0,0)
		  };

		// A list of all the SNO's to avoid - you could add 
		public static readonly HashSet<int> hashAvoidanceSNOList = new HashSet<int>
		  {
				  // Arcane        Arcane 2      Desecrator   Poison Tree    Molten Core   Molten Trail   Plague Cloud   Ice Balls     
				  219702,          221225,       84608,       5482,6578,     4803, 4804,   95868,         108869,        402, 223675,             
				  // Bees-Wasps    Plague-Hands  Azmo Pools   Azmo fireball  Azmo bodies   Belial 1       Belial 2      
				  5212,            3865,         123124,      123842,        123839,       161822,        161833, 
				  // Sha-Ball      Mol Ball      Mage Fire    Diablo Prison  Diablo Meteor Ice-trail      PoisonGas
				  4103,            160154,       432,         168031,        214845,       260377,        4176,
				  //lacuni bomb		Succubus Bloodstar	  Halls Of Agony: Inferno Wall
				  4546,			   164829,						  89578,
				  //Lightning Orbiter Projectile		Lightning Orbiter Focal Point
				  343539,								343582,
				  //Frozen Pulse
				  349774,
				  //Thunderstorm
				  341512,
				  //Meteor Impact
				  185366,
				  //Bloodspring (small)
				  332924,
				  //Bloodspring (medium)
				  332922,
				  //Bloodspring (Large)
				  332923,
				  //Teleport
				  337109,
				  
				  335505, // x1_malthael_drainSoul_ghost
				  325136, // x1_Malthael_DeathFogMonster
				  340512, // x1_Malthael_Mephisto_LightningObject

				  360738, // X1_Adria_arcanePool
                  358404, // X1_Adria_blood_large

				  360046, //X1_Unique_Monster_Generic_AOE_DOT_Poison

				  93837, //Ghom's Gluttony_gasCloud

				  134831, //A4 Destroyer Drop Location
			  };

		// A list of SNO's that are projectiles (so constantly look for new locations while avoiding)
		public static readonly HashSet<int> hashAvoidanceSNOProjectiles = new HashSet<int>
		  {
				  // Bees-Wasps  Sha-Ball   Mol Ball   Azmo fireball
				  5212,          4103,      160154,    123842,		164829, 
			  };

		public static bool IsAvoidanceTypeProjectile(AvoidanceType type)
		{
			return (type == AvoidanceType.BeeProjectile ||
					type == AvoidanceType.ShamanFireBall || 
					type == AvoidanceType.AzmodanFireball);
		}
		public static bool IsAvoidanceTypeProjectile(int snoid)
		{
			return hashAvoidanceSNOProjectiles.Contains(snoid);
		}

		internal static AvoidanceType FindAvoidanceUsingName(string Name)
		{
			Name = Name.ToLower();
			if (Name.StartsWith("monsteraffix_"))
			{
				if (Name.Contains("dececrator")) return AvoidanceType.Dececrator;
				if (Name.Contains("frozen")) return AvoidanceType.Frozen;
				if (Name.Contains("molten"))
				{
					if (Name.Contains("trail")) return AvoidanceType.MoltenTrail;
					return AvoidanceType.MoltenCore;
				}
				if (Name.Contains("plagued")) return AvoidanceType.PlagueCloud;
				if (Name.Contains("wall")) return AvoidanceType.Wall;
			}
			else if (Name.Contains("azmodan") || Name.Contains("belial") || Name.Contains("diablo"))
			{
				//Bosses
				if (Name.StartsWith("belial_armslam_projectile")) return AvoidanceType.BelialGround;
				if (Name.StartsWith("belial_groundprojectile")) return AvoidanceType.BelialGround;
			}
			else
			{
				if (Name.StartsWith("skeletonmage_fire_groundpool")) return AvoidanceType.MageFirePool;
				if (Name.StartsWith("fallenshaman_fireball_projectile")) return AvoidanceType.ShamanFireBall;
				if (Name.StartsWith("woodwraith_sporecloud_emitter")) return AvoidanceType.TreeSpore;
			}

			return AvoidanceType.None;
		}

		internal static AvoidanceType FindAvoidanceUsingSNOID(int SNOID)
		{
			switch (SNOID)
			{
				case 219702:
				case 221225:
					return AvoidanceType.ArcaneSentry;
				case 84608:
					return AvoidanceType.Dececrator;
				//case 5482: (Note: This is the Spore when its not "dangerous")
				case 6578:
					return AvoidanceType.TreeSpore;
				case 4803:
				case 4804:
					return AvoidanceType.MoltenCore;
				case 95868:
					return AvoidanceType.MoltenTrail;
				case 108869:
					return AvoidanceType.PlagueCloud;
				case 402:
				case 223675:
					return AvoidanceType.Frozen;
				case 5212:
					return AvoidanceType.BeeProjectile;
				case 3865:
					return AvoidanceType.PlagueHand;
				case 123124:
					return AvoidanceType.AzmodanPool;
				case 123842:
					return AvoidanceType.AzmodanFireball;
				case 123839:
					return AvoidanceType.AzmodanBodies;
				case 161822:
				case 161833:
				case 60108:
					return AvoidanceType.BelialGround;
				case 168031:
					return AvoidanceType.DiabloPrison;
				case 214845:
					return AvoidanceType.DiabloMetor;
				case 432:
					return AvoidanceType.MageFirePool;
				case 4546:
					return AvoidanceType.LacuniBomb;
				case 4176:
					return AvoidanceType.PoisonGas;
				case 164829:
					return AvoidanceType.SuccubusProjectile;
				case 89578:
					return AvoidanceType.WallOfFire;
				case 343539:
					return AvoidanceType.OrbitProjectile;
				case 343582:
					return AvoidanceType.OrbitFocalPoint;
				case 349774:
					return AvoidanceType.FrozenPulse;
				case 341512:
					return AvoidanceType.Thunderstorm;
				case 185366:
					return AvoidanceType.MeteorImpact;
				case 332924:
					return AvoidanceType.BloodSpringSmall;
				case 332922:
					return AvoidanceType.BloodSpringMedium;
				case 332923:
					return AvoidanceType.BloodSpringLarge;
				case 337109:
					return AvoidanceType.Teleport;
				case 335505:
					return AvoidanceType.MalthaelDrainSoul;
				case 325136:
					return AvoidanceType.MalthaelDeathFog;
				case 340512:
					return AvoidanceType.MalthaelLightning;
				case 360738:
					return AvoidanceType.AdriaArcanePool;
				case 358404:
					return AvoidanceType.AdriaBlood;
				case 360046:
					return AvoidanceType.RiftPoison;
				case 93837:
					return AvoidanceType.GhomGasCloud;
				case 134831:
					return AvoidanceType.DestroyerDrop;
			}
			return AvoidanceType.None;
		}

		internal static bool IgnoringAvoidanceType(AvoidanceType thisAvoidance)
		{
			if (!FunkyBaseExtension.Settings.Avoidance.AttemptAvoidanceMovements)
				return true;

			double dThisHealthAvoid = FunkyBaseExtension.Settings.Avoidance.Avoidances[(int)thisAvoidance].Health;
			if (dThisHealthAvoid == 0d)
				return true;

			return false;
		}

		///<summary>
		///Tests the given avoidance type to see if it should be ignored either due to a buff or if health is greater than the avoidance HP.
		///</summary>
		internal static bool IgnoreAvoidance(AvoidanceType thisAvoidance)
		{

			//Pylon Shield?
			if (Hotbar.HasBuff(SNOPower.Pages_Buff_Invulnerable))
				return true;

			//Countess Julias Cameo (Arcane Immunity)
			if (Equipment.ImmuneToArcane && thisAvoidance == AvoidanceType.ArcaneSentry)
				return true;

			//Maras Kaleidoscope (Poison Immunity)
			if (Equipment.ImmuneToPoison && (thisAvoidance == AvoidanceType.PlagueCloud || thisAvoidance == AvoidanceType.PlagueHand || thisAvoidance == AvoidanceType.PoisonGas))
				return true;

			//Talisman of Aranoch
			if (Equipment.ImmuneToCold && (thisAvoidance == AvoidanceType.FrozenPulse || thisAvoidance == AvoidanceType.MalthaelDeathFog))
				return true;

			//Special Blackthorne's Set Bonus Check!
			if (Equipment.ImmuneToDescratorMoltenPlaguedAvoidances)
			{
				switch (thisAvoidance)
				{
					case AvoidanceType.Dececrator:
					case AvoidanceType.MoltenCore:
					case AvoidanceType.MoltenTrail:
					case AvoidanceType.PlagueCloud:
					case AvoidanceType.PlagueHand:
						return true;
				}
			}

			double dThisHealthAvoid = FunkyBaseExtension.Settings.Avoidance.Avoidances[(int)thisAvoidance].Health;

			//if (!FunkyGame.Hero.CriticalAvoidance)
			//{
			//Not Critical Avoidance, should we be in total ignorance because of a buff?

			// Monks with Serenity up ignore all AOE's
			if (FunkyGame.CurrentActorClass == ActorClass.Monk)
			{
				// Monks with serenity are immune
				if (Hotbar.HasPower(SNOPower.Monk_Serenity) && Hotbar.HasBuff(SNOPower.Monk_Serenity))
					return true;

				//Epiphany ignore frozen.
				if (Hotbar.HasPower(SNOPower.X1_Monk_Epiphany) && Hotbar.HasBuff(SNOPower.X1_Monk_Epiphany) && thisAvoidance == AvoidanceType.Frozen)
					return true;
			}

			//Crusader Akarats Champion -- Ignore Frozen!
			if (FunkyGame.CurrentActorClass == ActorClass.Crusader && Hotbar.HasPower(SNOPower.X1_Crusader_AkaratsChampion) && Hotbar.HasBuff(SNOPower.X1_Crusader_AkaratsChampion) && thisAvoidance == AvoidanceType.Frozen)
				return true;

			if (FunkyGame.CurrentActorClass == ActorClass.Barbarian && Hotbar.HasPower(SNOPower.Barbarian_WrathOfTheBerserker) && Hotbar.HasBuff(SNOPower.Barbarian_WrathOfTheBerserker))
			{
				switch (thisAvoidance)
				{
					case AvoidanceType.Frozen:
					case AvoidanceType.ArcaneSentry:
					case AvoidanceType.Dececrator:
					case AvoidanceType.PlagueCloud:
						return true;
				}
			}
			//}

			//Only procedee if health percent is necessary for avoidance!
			return dThisHealthAvoid < FunkyGame.Hero.dCurrentHealthPct;
		}

	}
}