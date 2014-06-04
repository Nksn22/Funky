﻿using System;
using System.Linq;
using FunkyBot.Player.HotBar.Skills;
using FunkyBot.Player.HotBar.Skills.WitchDoctor;
using Zeta.Game;
using System.Collections.Generic;
using Zeta.Game.Internals.Actors;

namespace FunkyBot.Player.Class
{

	internal class WitchDoctor : PlayerClass
	{
		public WitchDoctor()
		{
			int ZunimassaSetItemCount = Bot.Character.Data.equipment.EquippedItems.Count(i => i.ThisRealName.Contains("Zunimassa"));
			if (ZunimassaSetItemCount > 3 || ZunimassaSetItemCount > 2 && Bot.Character.Data.equipment.RingOfGrandeur)
			{
				Logger.DBLog.DebugFormat("[Funky] Zunimassa Five Set Bounus Found!");
				Bot.Settings.WitchDoctor.ZunimassaFullSet = true;
			}
			else
				Bot.Settings.WitchDoctor.ZunimassaFullSet = false;

			Logger.DBLog.DebugFormat("[Funky] Using WitchDoctor Player Class");
		}
		//Base class for each individual class!
		public override ActorClass AC { get { return ActorClass.Witchdoctor; } }

		private readonly HashSet<SNOAnim> knockbackanims_Female = new HashSet<SNOAnim>
		{
					 SNOAnim.WitchDoctor_Female_1HT_MOJO_knockback_land,
					 SNOAnim.WitchDoctor_Female_HTH_knockback_land,
					 SNOAnim.WitchDoctor_Female_1HT_knockback_land,
					 SNOAnim.WitchDoctor_Female_2HT_knockback_land,
					 SNOAnim.WitchDoctor_Female_STF_knockback_land,
					 SNOAnim.WitchDoctor_Female_XBOW_knockback_land,
					 SNOAnim.WitchDoctor_Female_HTH_MOJO_knockback_land,
					 SNOAnim.WitchDoctor_Female_BOW_knockback_land,
					 SNOAnim.WitchDoctor_Female_2HS_knockback_land,
					 SNOAnim.WitchDoctor_Female_1HS_MOJO_knockback_land,
					 SNOAnim.WitchDoctor_Female_1HS_Knockback_land,
		};
		private readonly HashSet<SNOAnim> knockbackanims_Male = new HashSet<SNOAnim>
		{
					 SNOAnim.WitchDoctor_Male_1HS_Knockback_land,
					 SNOAnim.WitchDoctor_Male_2HS_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_2HT_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_HTH_MOJO_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_1HS_MOJO_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_BOW_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_STF_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_XBOW_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_HTH_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_1HT_Knockback_Land,
					 SNOAnim.WitchDoctor_Male_1HT_MOJO_knockback_land,
		};
		internal override HashSet<SNOAnim> KnockbackLandAnims
		{
			get
			{
				return Bot.Character.Data.SnoActor == SNOActor.WitchDoctor_Female ? knockbackanims_Female : knockbackanims_Male;
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
				return Bot.Character.Data.PetData.Gargantuan;
			}
		}
		internal override bool IsMeleeClass
		{
			get
			{
				return false;
			}
		}

		internal override Skill CreateAbility(SNOPower Power)
		{
			WitchDoctorActiveSkills power = (WitchDoctorActiveSkills)Enum.ToObject(typeof(WitchDoctorActiveSkills), (int)Power);

			switch (power)
			{
				case WitchDoctorActiveSkills.Witchdoctor_Gargantuan:
					return new Gargantuan();
				case WitchDoctorActiveSkills.Witchdoctor_Hex:
					return new Hex();
				case WitchDoctorActiveSkills.Witchdoctor_Firebomb:
					return new Firebomb();
				case WitchDoctorActiveSkills.Witchdoctor_MassConfusion:
					return new MassConfusion();
				case WitchDoctorActiveSkills.Witchdoctor_SoulHarvest:
					return new SoulHarvest();
				case WitchDoctorActiveSkills.Witchdoctor_Horrify:
					return new Horrify();
				case WitchDoctorActiveSkills.Witchdoctor_GraspOfTheDead:
					return new GraspOfTheDead();
				case WitchDoctorActiveSkills.Witchdoctor_CorpseSpider:
					return new CorpseSpider();
				case WitchDoctorActiveSkills.Witchdoctor_Locust_Swarm:
					return new LocustSwarm();
				case WitchDoctorActiveSkills.Witchdoctor_AcidCloud:
					return new AcidCloud();
				case WitchDoctorActiveSkills.Witchdoctor_FetishArmy:
					return new FetishArmy();
				case WitchDoctorActiveSkills.Witchdoctor_ZombieCharger:
					return new ZombieCharger();
				case WitchDoctorActiveSkills.Witchdoctor_Haunt:
					return new Haunt();
				case WitchDoctorActiveSkills.Witchdoctor_Sacrifice:
					return new Sacrifice();
				case WitchDoctorActiveSkills.Witchdoctor_SummonZombieDog:
					return new SummonZombieDogs();
				case WitchDoctorActiveSkills.Witchdoctor_Firebats:
					return new Firebats();
				case WitchDoctorActiveSkills.Witchdoctor_SpiritWalk:
					return new SpiritWalk();
				case WitchDoctorActiveSkills.Witchdoctor_PlagueOfToads:
					return new PlagueOfToads();
				case WitchDoctorActiveSkills.Witchdoctor_SpiritBarrage:
					return new SpiritBarrage();
				case WitchDoctorActiveSkills.Witchdoctor_BigBadVoodoo:
					return new BigBadVoodoo();
				case WitchDoctorActiveSkills.Witchdoctor_WallOfZombies:
					return new WallOfZombies();
				case WitchDoctorActiveSkills.Witchdoctor_PoisonDart:
					return new PoisonDart();
				case WitchDoctorActiveSkills.Witchdcotor_Piranhas:
					return new Piranhas();
				default:
					return DefaultAttack;
			}
		}

		enum WitchDoctorActiveSkills
		{
			Witchdoctor_Gargantuan = 30624,
			Witchdoctor_Hex = 30631,
			Witchdoctor_Firebomb = 67567,
			Witchdoctor_MassConfusion = 67600,
			Witchdoctor_SoulHarvest = 67616,
			Witchdoctor_Horrify = 67668,
			Witchdoctor_GraspOfTheDead = 69182,
			Witchdoctor_CorpseSpider = 69866,
			Witchdoctor_Locust_Swarm = 69867,
			Witchdoctor_AcidCloud = 70455,
			Witchdoctor_FetishArmy = 72785,
			Witchdoctor_ZombieCharger = 74003,
			Witchdoctor_Haunt = 83602,
			Witchdoctor_Sacrifice = 102572,
			Witchdoctor_SummonZombieDog = 102573,
			Witchdoctor_Firebats = 105963,
			Witchdoctor_SpiritWalk = 106237,
			Witchdoctor_PlagueOfToads = 106465,
			Witchdoctor_SpiritBarrage = 108506,
			Witchdoctor_BigBadVoodoo = 117402,
			Witchdoctor_WallOfZombies = 134837,
			Witchdoctor_PoisonDart = 103181,
			Witchdcotor_Piranhas = 347265,
		}
		//enum WitchDoctorPassiveSkills
		//{
		//	 Witchdoctor_Passive_SpiritVessel=218501,
		//	 Witchdoctor_Passive_FetishSycophants=218588,
		//	 Witchdoctor_Passive_GraveInjustice=218191,
		//	 Witchdoctor_Passive_BadMedicine=217826,
		//	 Witchdoctor_Passive_JungleFortitude=217968,
		//	 Witchdoctor_Passive_VisionQuest=209041,
		//	 Witchdoctor_Passive_PierceTheVeil=208628,
		//	 Witchdoctor_Passive_FierceLoyalty=208639,
		//	 Witchdoctor_Passive_ZombieHandler=208563,
		//	 Witchdoctor_Passive_RushOfEssence=208565,
		//	 Witchdoctor_Passive_BloodRitual=208568,
		//	 Witchdoctor_Passive_SpiritualAttunement=208569,
		//	 Witchdoctor_Passive_CircleOfLife=208571,
		//	 Witchdoctor_Passive_GruesomeFeast=208594,
		//	 Witchdoctor_Passive_TribalRites=208601,

		//}

	}

}