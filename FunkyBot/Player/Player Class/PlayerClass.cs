﻿using System;
using System.Globalization;
using System.Linq;
using FunkyBot.Cache.Objects;
using FunkyBot.Player.HotBar.Skills;
using FunkyBot.Player.HotBar.Skills.Conditions;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Common;
using System.Collections.Generic;
using FunkyBot.Player.HotBar;
using Zeta.Game.Internals.SNO;

namespace FunkyBot.Player.Class
{


	///<summary>
	///Used to describe the Current Player -- Class, Hotbar Abilities, Passives, etc..
	///</summary>
	public abstract class PlayerClass
	{
		//Base class for each individual class!
		protected PlayerClass()
		{
			HotBar.RefreshHotbar();
			HotBar.RefreshPassives();
			//HotBar.UpdateRepeatAbilityTimes();

			Skill healthPotionSkill = new DrinkHealthPotion();
			Skill.CreateSkillLogicConditions(ref healthPotionSkill);
			HealthPotionAbility = (DrinkHealthPotion)healthPotionSkill;

			LastUsedAbility = DefaultAttack;
			PowerPrime = DefaultAttack;

			//Gear Checks
			List<CacheACDItem> equippedItems = Bot.Character.Data.BackPack.ReturnCurrentEquippedItems();

			//Repear Wraps
			if (equippedItems.Any(i => i.ThisRealName.Contains("Reaper's Wraps")))
			{
				GlobesRestoreResource = true;
				Logger.DBLog.DebugFormat("Reaper's Wraps is equipped.. globes will be targeted on low resource!");
			}
			else
				GlobesRestoreResource = false;

			//Ring of Royal Grandeur
			bool hasRingOfGrandeur = equippedItems.Any(i => i.ThisRealName.Contains("Ring of Royal Grandeur"));


			//Blackthorns Set
			int BlackThornSetCount = equippedItems.Count(i => i.ThisRealName.Contains("Blackthorne's"));
			if (BlackThornSetCount==4 || BlackThornSetCount==3 && hasRingOfGrandeur)
			{
				ImmuneToDescratorMoltenPlaguedAvoidances = true;
				Logger.DBLog.DebugFormat("Blackthorne's Avoidance Immune is equipped");
			}
			else
				ImmuneToDescratorMoltenPlaguedAvoidances = false;
			

			Logger.DBLog.InfoFormat("[Funky] Finished Creating Player Class");

		}

		public bool GlobesRestoreResource { get; set; }
		public bool ImmuneToDescratorMoltenPlaguedAvoidances { get; set; }

		///<summary>
		///The actor class of this bot.
		///</summary>
		public virtual ActorClass AC { get { return ActorClass.Invalid; } }
		internal Hotbar HotBar = new Hotbar();

		///<summary>
		///This is used to determine things such as how we preform certain checks (I.E. Line Of Sight)
		///</summary>
		internal virtual bool IsMeleeClass
		{
			get { return false; }
		}


		// This is used so we don't use certain skills until we "top up" our primary resource by enough
		internal double iWaitingReservedAmount = 0d;
		internal bool bWaitingForSpecial = false;

		///<summary>
		///Create Ability (Derieved classes override this!)
		///</summary>
		internal virtual Skill CreateAbility(SNOPower P)
		{
			return DefaultAttack;
		}

		internal DrinkHealthPotion HealthPotionAbility = new DrinkHealthPotion();

		internal virtual Skill DefaultAttack
		{
			get { return new WeaponMeleeInsant(); }
		}

		///<summary>
		///
		///</summary>
		internal bool CanUseDefaultAttack { get; set; }

		///<summary>
		///Check if Bot should generate a new ZigZag location.
		///</summary>
		internal virtual bool ShouldGenerateNewZigZagPath()
		{
			return true;
		}

		///<summary>
		///Generates a new ZigZag location.
		///</summary>
		internal virtual void GenerateNewZigZagPath() { }

		///<summary>
		///
		///</summary>
		internal virtual int MainPetCount { get { return 0; } }

		///<summary>
		///Animations that determine if character has been "vortexed"
		///</summary>
		internal virtual HashSet<SNOAnim> KnockbackLandAnims
		{
			get { return null; }
		}



		///<summary>
		///Used to check for a secondary hotbar set. Currently only used for wizards with Archon.
		///</summary>
		internal virtual bool SecondaryHotbarBuffPresent()
		{
			return false;
		}

		public bool ContainsNonRangedCombatSkill { get; set; }
		internal virtual void RecreateAbilities()
		{
			ContainsNonRangedCombatSkill = false;
			Abilities = new Dictionary<SNOPower, Skill>();

			//No default rage generation Ability.. then we add the Instant Melee Ability.
			if (!HotBar.HotbarContainsAPrimarySkill())
			{
				Skill defaultAbility = Bot.Character.Class.DefaultAttack;
				Skill.CreateSkillLogicConditions(ref defaultAbility);
				Abilities.Add(defaultAbility.Power, defaultAbility);
				HotBar.RuneIndexCache.Add(defaultAbility.Power, -1);
				Logger.DBLog.DebugFormat("[Funky] Added Skill {0}", defaultAbility.Power);

				//No Primary Skill.. Check if Default Attack can be used!
				if (!Bot.Settings.Combat.AllowDefaultAttackAlways)
				{
					Logger.DBLog.Warn("**** Warning ****");
					Logger.DBLog.Warn("No Primary Skill Found and Allow Default Attack Always Is Disabled!");
					Logger.DBLog.Warn("This may cause idles to occur.. Enable AllowDefaultAttackAlways setting found under Combat Tab.");
					Logger.DBLog.Warn("**** Warning ****");
				}
			}


			//Create the abilities
			foreach (var item in HotBar.HotbarPowers)
			{
				Skill newAbility = Bot.Character.Class.CreateAbility(item);
				Skill.CreateSkillLogicConditions(ref newAbility);
				newAbility.SuccessfullyUsed += AbilitySuccessfullyUsed;

				//combat ability set property
				if ((SkillExecutionFlags.ClusterLocation | SkillExecutionFlags.ClusterTarget | SkillExecutionFlags.ClusterTargetNearest | SkillExecutionFlags.Location | SkillExecutionFlags.Target).HasFlag(newAbility.ExecutionType))
					newAbility.IsCombat = true;

				Abilities.Add(item, newAbility);

				if (!ContainsNonRangedCombatSkill && !newAbility.IsRanged && !newAbility.IsProjectile && (SkillExecutionFlags.Target | SkillExecutionFlags.ClusterTarget).HasFlag(newAbility.ExecutionType))
					ContainsNonRangedCombatSkill = true;

				Logger.DBLog.DebugFormat("[Funky] Added Skill {0} using RuneIndex {1}", newAbility.Power, newAbility.RuneIndex);
			}

			//Sort Abilities
			SortedAbilities = Abilities.Values.OrderByDescending(a => a.Priority).ThenBy(a => a.Range).ToList();
		}



		///<summary>
		///Sets criteria based on object given.
		///</summary>
		internal virtual Skill AbilitySelector(CacheUnit obj, bool IgnoreOutOfRange = false)
		{
			//Reset default attack can use
			CanUseDefaultAttack = !HotBar.HotbarPowers.Contains(DefaultAttack.Power) ? false : true;
			//Reset waiting for special!
			bWaitingForSpecial = false;

			ConditionCriteraTypes criterias = ConditionCriteraTypes.All;

			//Although the unit is a cluster exception.. we should verify it is not a clustered object.
			if (obj.IsClusterException && obj.BeingIgnoredDueToClusterLogic)
			{
				criterias = ConditionCriteraTypes.SingleTarget;
			}

			return AbilitySelector(criterias, IgnoreOutOfRange);
		}
		///<summary>
		///Selects first Ability that is successful in precast and combat testing.
		///</summary>
		private Skill AbilitySelector(ConditionCriteraTypes criteria = ConditionCriteraTypes.All, bool IgnoreOutOfRange = false)
		{
			Skill returnAbility = DefaultAttack;
			foreach (var item in SortedAbilities)
			{
				//Check precast conditions
				if (!item.CheckPreCastConditionMethod()) continue;

				//Check Combat Conditions!
				if (!item.CheckCombatConditionMethod(criteria))
				{
					continue;
				}

				//Check if we can execute or if it requires movement
				if (IgnoreOutOfRange)
				{
					if (item.DestinationVector != Bot.Character.Data.Position)
						continue;
				}

				returnAbility = item;
				break;
			}

			//Setup Ability (sets vars according to current cache)
			Skill.SetupAbilityForUse(ref returnAbility);
			return returnAbility;
		}

		internal List<Skill> ReturnAllUsableAbilities(CacheUnit obj, bool IgnoreOutOfRange = false)
		{
			//Reset default attack can use
			CanUseDefaultAttack = !Abilities.ContainsKey(DefaultAttack.Power) ? false : true;

			ConditionCriteraTypes criterias = ConditionCriteraTypes.All;

			//Although the unit is a cluster exception.. we should verify it is not a clustered object.
			if (obj.IsClusterException && obj.BeingIgnoredDueToClusterLogic)
			{
				criterias = ConditionCriteraTypes.SingleTarget;
			}

			List<Skill> UsableAbilities = new List<Skill>();
			foreach (var item in SortedAbilities)
			{
				//Check precast conditions
				if (!item.CheckPreCastConditionMethod()) continue;

				//Check Combat Conditions!
				if (!item.CheckCombatConditionMethod(criterias))
				{
					continue;
				}

				//Check if we can execute or if it requires movement
				if (IgnoreOutOfRange)
				{
					if (item.DestinationVector != Bot.Character.Data.Position)
						continue;
				}

				Skill ability = item;
				Skill.SetupAbilityForUse(ref ability);
				UsableAbilities.Add(ability);
			}

			return UsableAbilities;
		}

		///<summary>
		///Returns Ability used for destructibles
		///</summary>
		internal virtual Skill DestructibleAbility()
		{
			Skill returnAbility = Bot.Character.Class.DefaultAttack;
			List<Skill> nonDestructibleAbilities = new List<Skill>();
			foreach (var item in Abilities.Values)
			{
				if (item.IsADestructiblePower)
				{

					//Check LOS -- Projectiles
					if (item.IsRanged && !Bot.Targeting.Cache.CurrentTarget.IgnoresLOSCheck)
					{
						LOSInfo LOSINFO = Bot.Targeting.Cache.CurrentTarget.LineOfSight;
						if (LOSINFO.LastLOSCheckMS > 3000)
						{
							if (!LOSINFO.LOSTest(Bot.Character.Data.Position, true, ServerObjectIntersection: false))
							{
								//Raycast failed.. reset LOS Check -- for valid checking.
								if (!LOSINFO.RayCast.Value) Bot.Targeting.Cache.CurrentTarget.RequiresLOSCheck = true;
								continue;
							}
						}
					}

					if (item.CheckPreCastConditionMethod())
					{
						returnAbility = item;
						Skill.SetupAbilityForUse(ref returnAbility, true);
						return returnAbility;
					}
				}
				else if (item.ExecutionType.HasFlag(SkillExecutionFlags.Target) || item.ExecutionType.HasFlag(SkillExecutionFlags.Location))
				{

					//Check LOS -- Projectiles
					if (item.IsRanged && !Bot.Targeting.Cache.CurrentTarget.IgnoresLOSCheck)
					{
						LOSInfo LOSINFO = Bot.Targeting.Cache.CurrentTarget.LineOfSight;
						if (LOSINFO.LastLOSCheckMS > 3000 || (item.IsProjectile && !LOSINFO.ObjectIntersection.HasValue) || !LOSINFO.NavCellProjectile.HasValue)
						{
							if (!LOSINFO.LOSTest(Bot.Character.Data.Position, NavRayCast: true, ServerObjectIntersection: item.IsProjectile, Flags: NavCellFlags.AllowProjectile))
							{
								//Raycast failed.. reset LOS Check -- for valid checking.
								if (!LOSINFO.RayCast.Value) Bot.Targeting.Cache.CurrentTarget.RequiresLOSCheck = true;
								continue;
							}
						}
						else if ((item.IsProjectile && LOSINFO.ObjectIntersection.Value) || !LOSINFO.NavCellProjectile.Value)
						{
							continue;
						}
					}

					//Add this Ability to our list.. incase we cant find an offical Ability to use.
					if (item.CheckPreCastConditionMethod())
					{
						nonDestructibleAbilities.Add(item);
					}
				}
			}

			//Use non-destructible Ability..
			if (nonDestructibleAbilities.Count > 0)
				returnAbility = nonDestructibleAbilities[0];

			Skill.SetupAbilityForUse(ref returnAbility, true);
			return returnAbility;
		}




		internal Dictionary<SNOPower, Skill> Abilities = new Dictionary<SNOPower, Skill>();
		internal List<Skill> SortedAbilities = new List<Skill>();


		///<summary>
		///Searches for any abilities that have set the OutOfCombat Movement Criteria.
		///</summary>
		internal Vector3 FindOutOfCombatMovementPower(out Skill MovementAbility, Vector3 Destination)
		{
			MovementAbility = null;
			foreach (var item in Abilities.Values.Where(A => A.FOutOfCombatMovement != null))
			{

				if (item.CheckPreCastConditionMethod())
				{
					Vector3 AbilityTargetVector = item.FOutOfCombatMovement.Invoke(Destination);
					if (AbilityTargetVector != Vector3.Zero)
					{
						MovementAbility = item;
						return AbilityTargetVector;
					}
				}
			}
			return Vector3.Zero;
		}
		///<summary>
		///Searches for any abilities that have set the Combat Movement Criteria.
		///</summary>
		internal Vector3 FindCombatMovementPower(out Skill MovementAbility, Vector3 Destination)
		{
			MovementAbility = null;
			foreach (var item in Abilities.Values.Where(A => A.FCombatMovement != null))
			{

				if (item.CheckPreCastConditionMethod())
				{
					Vector3 AbilityTargetVector = item.FCombatMovement.Invoke(Destination);
					if (AbilityTargetVector != Vector3.Zero)
					{
						MovementAbility = item;
						return AbilityTargetVector;
					}
				}
			}
			return Vector3.Zero;
		}

		internal bool HasCastableMovementAbility(bool combatOnly = true)
		{
			foreach (var item in Abilities.Values.Where(A => A.FCombatMovement != null && A.IsASpecialMovementPower))
			{

				if (item.CheckPreCastConditionMethod())
				{
					return true;
				}
			}

			return false;
		}


		///<summary>
		///Returns a power for Buffing.
		///</summary>
		internal bool FindBuffPower(out Skill BuffAbility)
		{
			BuffAbility = null;
			foreach (var item in Abilities.Values.Where(A => A.IsBuff))
			{
				if (item.CheckPreCastConditionMethod())
				{
					if (item.CheckBuffConditionMethod())
					{
						BuffAbility = item;
						Skill.SetupAbilityForUse(ref BuffAbility);
						return true;
					}
				}
			}
			return false;
		}

		///<summary>
		///Returns a power for Combat Buffing.
		///</summary>
		internal bool FindCombatBuffPower(out Skill BuffAbility)
		{
			BuffAbility = null;
			foreach (var item in Abilities.Values.Where(A => A.IsBuff && A.UseageType.HasFlag(SkillUseage.Combat | SkillUseage.Anywhere)))
			{
				if (item.CheckPreCastConditionMethod())
				{
					if (item.CheckCombatConditionMethod())
					{
						BuffAbility = item;
						Skill.SetupAbilityForUse(ref BuffAbility);
						return true;
					}
				}
			}
			return false;
		}


		internal void DebugString()
		{
			Logger.DBLog.InfoFormat("Character Information\r\nRadius {0}\r\nHotBar Abilities [{1}]\r\n", Bot.Character.Data.fCharacterRadius, HotBar.HotbarPowers.Count);

			foreach (SNOPower item in Bot.Character.Class.HotBar.HotbarPowers)
			{
				Logger.DBLog.InfoFormat("{0} with current rune index {1}", item.ToString(), HotBar.RuneIndexCache.ContainsKey(item) ? HotBar.RuneIndexCache[item].ToString(CultureInfo.InvariantCulture) : "none");
			}

			Logger.DBLog.InfoFormat("Created Abilities [{0}]", Abilities.Count);
			foreach (var item in Abilities.Values)
			{
				Logger.DBLog.InfoFormat("Power [{0}] -- Priority {1} --", item.Power.ToString(), item.Priority);
			}

			Logger.DBLog.InfoFormat("Current Buffs");
			foreach (SNOPower item in HotBar.CurrentBuffs.Keys)
			{
				Logger.DBLog.InfoFormat("Buff: {0}", Enum.GetName(typeof(SNOPower), item));
			}
			Logger.DBLog.InfoFormat("Current Debuffs");
			foreach (SNOPower item in HotBar.CurrentDebuffs)
			{
				Logger.DBLog.InfoFormat("Debuff: {0}", Enum.GetName(typeof(SNOPower), item));
			}

		}

		///<summary>
		///Last successful Ability used.
		///</summary>
		internal Skill LastUsedAbility { get; set; }
		internal DateTime LastUsedACombatAbility { get; set; }

		internal void AbilitySuccessfullyUsed(Skill ability, bool reorderAbilities)
		{
			if (ability.IsCombat)
			{
				LastUsedACombatAbility = DateTime.Now;
			}

			LastUsedAbility = ability;


			//Only Sort When Non-Channeling!
			if (reorderAbilities)
				SortedAbilities = Abilities.Values.OrderByDescending(a => a.Priority).ThenByDescending(a => a.LastUsedMilliseconds).ToList();
		}

		internal Skill PowerPrime;

		internal static void CreateBotClass()
		{
			if (Bot.Game != null && Bot.Character.Account.ActorClass != ActorClass.Invalid)
			{
				//Create Specific Player Class
				switch (Bot.Character.Account.ActorClass)
				{
					case ActorClass.Barbarian:
						Bot.Character.Class = new Barbarian();
						break;
					case ActorClass.DemonHunter:
						Bot.Character.Class = new DemonHunter();
						break;
					case ActorClass.Monk:
						Bot.Character.Class = new Monk();
						break;
					case ActorClass.Witchdoctor:
						Bot.Character.Class = new WitchDoctor();
						break;
					case ActorClass.Wizard:
						Bot.Character.Class = new Wizard();
						break;
					case ActorClass.Crusader:
						//Logger.DBLog.Fatal("Crusader Class is not supported yet!");
						//BotMain.Stop(true, "Crusader Class not supported by FunkyBot");
						//return;
						Bot.Character.Class = new Crusader();
						break;
				}

				Bot.Character.Class.RecreateAbilities();
			}
		}
	}

}