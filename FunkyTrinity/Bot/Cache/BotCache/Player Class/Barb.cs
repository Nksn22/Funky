﻿using System;
using System.Linq;
using Zeta;
using Zeta.Internals.Actors;
using Zeta.Common;
using System.Collections.Generic;
using Zeta.CommonBot;
using Zeta.Internals.SNO;
using FunkyTrinity.Enums;
using FunkyTrinity.ability;
using FunkyTrinity.Cache;
using FunkyTrinity.ability.Abilities;

namespace FunkyTrinity
{

		  internal class Barbarian : Player
		  {
				 enum BarbarianActiveSkills
				 {
						Barbarian_AncientSpear=69979,
						Barbarian_Rend=70472,
						Barbarian_Frenzy=78548,
						Barbarian_Sprint=78551,
						Barbarian_BattleRage=79076,
						Barbarian_ThreateningShout=79077,
						Barbarian_Bash=79242,
						Barbarian_GroundStomp=79446,
						Barbarian_IgnorePain=79528,
						Barbarian_WrathOfTheBerserker=79607,
						Barbarian_HammerOfTheAncients=80028,
						Barbarian_CallOfTheAncients=80049,
						Barbarian_Cleave=80263,
						Barbarian_WarCry=81612,
						Barbarian_SeismicSlam=86989,
						Barbarian_Leap=93409,
						Barbarian_WeaponThrow=93885,
						Barbarian_Whirlwind=96296,
						Barbarian_FuriousCharge=97435,
						Barbarian_Earthquake=98878,
						Barbarian_Revenge=109342,
						Barbarian_Overpower=159169,
				 }
				 enum BarbarianPassiveSkills
				 {
						Barbarian_Passive_BoonOfBulKathos=204603,
						Barbarian_Passive_NoEscape=204725,
						Barbarian_Passive_Brawler=205133,
						Barbarian_Passive_Ruthless=205175,
						Barbarian_Passive_BerserkerRage=205187,
						Barbarian_Passive_PoundOfFlesh=205205,
						Barbarian_Passive_Bloodthirst=205217,
						Barbarian_Passive_Animosity=205228,
						Barbarian_Passive_Unforgiving=205300,
						Barbarian_Passive_Relentless=205398,
						Barbarian_Passive_Superstition=205491,
						Barbarian_Passive_InspiringPresence=205546,
						Barbarian_Passive_Juggernaut=205707,
						Barbarian_Passive_ToughAsNails=205848,
						Barbarian_Passive_WeaponsMaster=206147,
				 }
				//Base class for each individual class!
				public Barbarian(ActorClass a)
					 : base(a)
				{
				}
				public override Ability DefaultAttack
				{
					 get { return new WeaponMeleeInsant(); }
				}

				private bool UsingWeaponThrowAbility=false;
				public override bool IsMeleeClass
				{
					 get
					 {
						  return !UsingWeaponThrowAbility;
					 }
				}


				public override bool ShouldGenerateNewZigZagPath()
				{
					 return (DateTime.Now.Subtract(Bot.Combat.lastChangedZigZag).TotalMilliseconds>=2000f||
								(Bot.Combat.vPositionLastZigZagCheck!=Vector3.Zero&&Bot.Character.Position==Bot.Combat.vPositionLastZigZagCheck&&DateTime.Now.Subtract(Bot.Combat.lastChangedZigZag).TotalMilliseconds>=1200)||
								Vector3.Distance(Bot.Character.Position, Bot.Combat.vSideToSideTarget)<=5f||
								Bot.Target.CurrentTarget!=null&&Bot.Target.CurrentTarget.AcdGuid.HasValue&&Bot.Target.CurrentTarget.AcdGuid.Value!=Bot.Combat.iACDGUIDLastWhirlwind);
				}
				public override void GenerateNewZigZagPath()
				{
					 if (Bot.Combat.bCheckGround)
						  Bot.Combat.vSideToSideTarget=Bot.NavigationCache.FindZigZagTargetLocation(Bot.Target.CurrentTarget.Position, 25f, false, true, true);
					 else if (Bot.Combat.iAnythingWithinRange[(int)RangeIntervals.Range_30]>=6||Bot.Combat.iElitesWithinRange[(int)RangeIntervals.Range_30]>=3)
						  Bot.Combat.vSideToSideTarget=Bot.NavigationCache.FindZigZagTargetLocation(Bot.Target.CurrentTarget.Position, 25f, false, true);
					 else
						  Bot.Combat.vSideToSideTarget=Bot.NavigationCache.FindZigZagTargetLocation(Bot.Target.CurrentTarget.Position, 25f);
					 Bot.Combat.powerLastSnoPowerUsed=SNOPower.None;
					 Bot.Combat.iACDGUIDLastWhirlwind=Bot.Target.CurrentTarget.AcdGuid.HasValue?Bot.Target.CurrentTarget.AcdGuid.Value:-1;
					 Bot.Combat.lastChangedZigZag=DateTime.Now;
				}


				//TODO:: Add settings to enable skills to be used to fury dump.
				//private bool FuryDumping=false;


				public override void RecreateAbilities()
				{
					 Abilities=new Dictionary<SNOPower, Ability>();

					 //Create the abilities
					 foreach (var item in HotbarPowers)
					 {
						  Abilities.Add(item, this.CreateAbility(item));
					 }

					 //No default rage generation ability.. then we add the Instant Melee Ability.
					 if (!HotbarContainsAPrimaryAbility())
					 {
						  Ability defaultAbility=this.DefaultAttack;
						  Abilities.Add(defaultAbility.Power, defaultAbility);
						  RuneIndexCache.Add(defaultAbility.Power, -1);
					 }

					 //Sort Abilities
					 SortedAbilities=Abilities.Values.OrderByDescending(a => a.Priority).ThenBy(a => a.Range).ToList();
				}

				public override Ability CreateAbility(SNOPower Power)
				{
					 BarbarianActiveSkills power=(BarbarianActiveSkills)Enum.ToObject(typeof(BarbarianActiveSkills), (int)Power);
					

					switch (power)
					{
						 case BarbarianActiveSkills.Barbarian_AncientSpear:
								return new ability.Abilities.Barb.AncientSpear();
						 case BarbarianActiveSkills.Barbarian_Rend:
							return new ability.Abilities.Barb.Rend();
						 case BarbarianActiveSkills.Barbarian_Frenzy:
								return new ability.Abilities.Barb.Frenzy();
						 case BarbarianActiveSkills.Barbarian_Sprint:
								return new ability.Abilities.Barb.Sprint();
						 case BarbarianActiveSkills.Barbarian_BattleRage:
								return new ability.Abilities.Barb.Battlerage();
						 case BarbarianActiveSkills.Barbarian_ThreateningShout:
								return new ability.Abilities.Barb.ThreateningShout();
						 case BarbarianActiveSkills.Barbarian_Bash:
								return new ability.Abilities.Barb.Bash();
						 case BarbarianActiveSkills.Barbarian_GroundStomp:
								return new ability.Abilities.Barb.GroundStomp();
						 case BarbarianActiveSkills.Barbarian_IgnorePain:
								return new ability.Abilities.Barb.IgnorePain();
						 case BarbarianActiveSkills.Barbarian_WrathOfTheBerserker:
								return new ability.Abilities.Barb.WrathOfTheBerserker();
						 case BarbarianActiveSkills.Barbarian_HammerOfTheAncients:
								return new ability.Abilities.Barb.HammeroftheAncients();
						 case BarbarianActiveSkills.Barbarian_CallOfTheAncients:
								return new ability.Abilities.Barb.CalloftheAncients();
						 case BarbarianActiveSkills.Barbarian_Cleave:
								return new ability.Abilities.Barb.Cleave();
						 case BarbarianActiveSkills.Barbarian_WarCry:
								return new ability.Abilities.Barb.Warcry();
						 case BarbarianActiveSkills.Barbarian_SeismicSlam:
								return new ability.Abilities.Barb.SeismicSlam();
						 case BarbarianActiveSkills.Barbarian_Leap:
								return new ability.Abilities.Barb.Leap();
						 case BarbarianActiveSkills.Barbarian_WeaponThrow:
								return new ability.Abilities.Barb.WeaponThrow();
						 case BarbarianActiveSkills.Barbarian_Whirlwind:
								return new ability.Abilities.Barb.Whirlwind();
						 case BarbarianActiveSkills.Barbarian_FuriousCharge:
								return new ability.Abilities.Barb.FuriousCharge();
						 case BarbarianActiveSkills.Barbarian_Earthquake:
								return new ability.Abilities.Barb.Earthquake();
						 case BarbarianActiveSkills.Barbarian_Revenge:
								return new ability.Abilities.Barb.Revenge();
						 case BarbarianActiveSkills.Barbarian_Overpower:
								return new ability.Abilities.Barb.Overpower();
						 default:
								return new Ability();
					}


				}
		  }
	 
}