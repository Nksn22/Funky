﻿using FunkyBot.Cache;
using FunkyBot.Cache.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeta.Internals.Actors;
using FunkyBot.Game;

namespace FunkyBot.Game
{
	///<summary>
	///Loot Stat Actions
	///</summary>
	public enum LootStatTypes
	{
		Looted,
		Stashed,
		Salvaged,
		Vendored,
		Dropped
	}
	///<summary>
	///Tracking of Loot Specific actions taken
	///</summary>
    public class LootStats
    {
        public int Looted { get; set; }
        public int Stashed { get; set; }
        public int Salvaged { get; set; }
        public int Vendored { get; set; }
        public int Dropped { get; set; }

        public LootStats()
        {
            Looted = 0;
            Stashed = 0;
            Salvaged = 0;
            Vendored = 0;
            Dropped = 0;
        }

        public void Merge(LootStats other)
        {
            this.Looted += other.Looted;
            this.Stashed += other.Stashed;
            this.Salvaged += other.Salvaged;
            this.Vendored += other.Vendored;
            this.Dropped += other.Dropped;
        }

        public override string ToString()
		{
			return String.Format("{0} \t {1} \t {2} \t {3} \t {4}", 
                this.Dropped, this.Looted, this.Stashed, this.Vendored, this.Salvaged);
        }
    }

	///<summary>
	///Container for multiple individual LootStats (Summary)
	///</summary>
    public class LootTracking
    {
        public LootStats Magical { get; set; }
        public LootStats Rare { get; set; }
        public LootStats Legendary { get; set; }
        public LootStats Gems { get; set; }
        public LootStats Crafting { get; set; }
        public LootStats Keys { get; set; }

        public LootTracking()
        {
            Magical = new LootStats();
            Rare = new LootStats();
            Legendary = new LootStats();
            Gems = new LootStats();
            Crafting = new LootStats();
            Keys = new LootStats();
        }

        public void Merge(LootTracking other)
        {
            Magical.Merge(other.Magical);
            Rare.Merge(other.Rare);
            Legendary.Merge(other.Legendary);
            Gems.Merge(other.Gems);
            Crafting.Merge(other.Crafting);
            Keys.Merge(other.Keys);
        }

		public int GetTotalLootStatCount(LootStatTypes statType)
		{
			switch (statType)
			{
				case LootStatTypes.Looted:
					return this.Magical.Looted + this.Rare.Looted + this.Legendary.Looted + this.Crafting.Looted + this.Keys.Looted + this.Gems.Looted;
				case LootStatTypes.Stashed:
					return this.Magical.Stashed + this.Rare.Stashed + this.Legendary.Stashed + this.Crafting.Stashed + this.Keys.Stashed + this.Gems.Stashed;
				case LootStatTypes.Salvaged:
					return this.Magical.Salvaged + this.Rare.Salvaged + this.Legendary.Salvaged + this.Crafting.Salvaged + this.Keys.Salvaged + this.Gems.Salvaged;
				case LootStatTypes.Vendored:
					return this.Magical.Vendored + this.Rare.Vendored + this.Legendary.Vendored + this.Crafting.Vendored + this.Keys.Vendored + this.Gems.Vendored;
				case LootStatTypes.Dropped:
					return this.Magical.Dropped + this.Rare.Dropped + this.Legendary.Dropped + this.Crafting.Dropped + this.Keys.Dropped + this.Gems.Dropped;
			}

			return 0;
		}

        public void LootedItemLog(GilesItemType thisgilesitemtype, GilesBaseItemType thisgilesbasetype, ItemQuality itemQuality)
        {
            if (thisgilesitemtype == GilesItemType.HealthPotion)
                return;

            //No profile set.. because new game?
            //if (Bot.BotStatistics.ProfileStats.CurrentProfile == null)
            //    return;

            switch (thisgilesbasetype)
            {
                case GilesBaseItemType.WeaponOneHand:
                case GilesBaseItemType.WeaponTwoHand:
                case GilesBaseItemType.WeaponRange:
                case GilesBaseItemType.Offhand:
                case GilesBaseItemType.Armor:
                case GilesBaseItemType.Jewelry:
                case GilesBaseItemType.FollowerItem:
                    if (itemQuality > ItemQuality.Rare6)
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[3]++;
                        //Statistics.ItemStats.CurrentGame.lootedItemTotals[3]++;
						Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Legendary.Looted++;
                    }
                    else if (itemQuality > ItemQuality.Magic3)
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[2]++;
						Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Rare.Looted++;
                    }
                    else
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[1]++;
						Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Magical.Looted++;
                    }
                    break;

                case GilesBaseItemType.Unknown:
                case GilesBaseItemType.Misc:
                    if (thisgilesitemtype == GilesItemType.CraftingMaterial || thisgilesitemtype == GilesItemType.CraftingPlan || thisgilesitemtype == GilesItemType.CraftTome)
                    {
                        //   Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[(int)LootIndex.Crafting]++;
						Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Crafting.Looted++;
                    }
                    else if (thisgilesitemtype == GilesItemType.InfernalKey)
                    {
                        // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[(int)LootIndex.Key]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Keys.Looted++;
                    }
                    else
                    {
                        // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[0]++;
                    }
                    break;
                case GilesBaseItemType.Gem:
                    // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.lootedItemTotals[(int)LootIndex.Gem]++;
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Gems.Looted++;
                    break;
            }


        }

        public void StashedItemLog(CacheACDItem i)
        {
            //if (Bot.BotStatistics.ProfileStats.CurrentProfile==null)
            //return;

            GilesItemType thisGilesItemType = Funky.DetermineItemType(i.ThisInternalName, i.ThisDBItemType, i.ThisFollowerType);
            if (thisGilesItemType == GilesItemType.InfernalKey)
            {
                Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Keys.Stashed++;
                //	 Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Key]++;
                return;
            }


            switch (i.ACDItem.ItemType)
            {
                case ItemType.CraftingPage:
                case ItemType.CraftingPlan:
                case ItemType.CraftingReagent:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Crafting.Stashed++;
                    //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Crafting]++;
                    break;
                case ItemType.Gem:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Gems.Stashed++;
                    // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Gem]++;
                    break;
                case ItemType.Amulet:
                case ItemType.Axe:
                case ItemType.Belt:
                case ItemType.Boots:
                case ItemType.Bow:
                case ItemType.Bracer:
                case ItemType.CeremonialDagger:
                case ItemType.Chest:
                case ItemType.Cloak:
                case ItemType.Crossbow:
                case ItemType.Dagger:
                case ItemType.Daibo:
                case ItemType.FistWeapon:
                case ItemType.FollowerSpecial:
                case ItemType.Gloves:
                case ItemType.HandCrossbow:
                case ItemType.Helm:
                case ItemType.Legs:
                case ItemType.Mace:
                case ItemType.MightyBelt:
                case ItemType.MightyWeapon:
                case ItemType.Mojo:
                case ItemType.Orb:
                case ItemType.Polearm:
                case ItemType.Quiver:
                case ItemType.Ring:
                case ItemType.Shield:
                case ItemType.Shoulder:
                case ItemType.Spear:
                case ItemType.SpiritStone:
                case ItemType.Staff:
                case ItemType.Sword:
                case ItemType.VoodooMask:
                case ItemType.Wand:
                case ItemType.WizardHat:
                    if (i.ThisQuality == ItemQuality.Legendary)
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[3]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Legendary.Stashed++;
                    }
                    else if (i.ThisQuality > ItemQuality.Magic3)
                    {
                        // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[2]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Rare.Stashed++;
                    }
                    else
                    {
                        //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[1]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Magical.Stashed++;
                    }
                    break;
            }
        }

        public void SalvagedItemLog(CacheACDItem i)
        {
            if (i.ThisQuality == ItemQuality.Legendary)
            {
                //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[3]++;
                Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Legendary.Salvaged++;
            }
            else if (i.ThisQuality > ItemQuality.Magic3)
            {
                // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[2]++;
                Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Rare.Salvaged++;
            }
            else
            {
                //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[1]++;
                Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Magical.Salvaged++;
            }
            return;
        }

        public void VendoredItemLog(CacheACDItem i)
        {
            //if (Bot.BotStatistics.ProfileStats.CurrentProfile==null)
            //return;

            switch (i.ACDItem.ItemType)
            {
                case ItemType.CraftingPage:
                case ItemType.CraftingPlan:
                case ItemType.CraftingReagent:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Crafting.Vendored++;
                    //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Crafting]++;
                    break;
                case ItemType.Gem:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Gems.Vendored++;
                    // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Gem]++;
                    break;
                case ItemType.Amulet:
                case ItemType.Axe:
                case ItemType.Belt:
                case ItemType.Boots:
                case ItemType.Bow:
                case ItemType.Bracer:
                case ItemType.CeremonialDagger:
                case ItemType.Chest:
                case ItemType.Cloak:
                case ItemType.Crossbow:
                case ItemType.Dagger:
                case ItemType.Daibo:
                case ItemType.FistWeapon:
                case ItemType.FollowerSpecial:
                case ItemType.Gloves:
                case ItemType.HandCrossbow:
                case ItemType.Helm:
                case ItemType.Legs:
                case ItemType.Mace:
                case ItemType.MightyBelt:
                case ItemType.MightyWeapon:
                case ItemType.Mojo:
                case ItemType.Orb:
                case ItemType.Polearm:
                case ItemType.Quiver:
                case ItemType.Ring:
                case ItemType.Shield:
                case ItemType.Shoulder:
                case ItemType.Spear:
                case ItemType.SpiritStone:
                case ItemType.Staff:
                case ItemType.Sword:
                case ItemType.VoodooMask:
                case ItemType.Wand:
                case ItemType.WizardHat:
                    if (i.ThisQuality == ItemQuality.Legendary)
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[3]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Legendary.Vendored++;
                    }
                    else if (i.ThisQuality > ItemQuality.Magic3)
                    {
                        // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[2]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Rare.Vendored++;
                    }
                    else
                    {
                        //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[1]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Magical.Vendored++;
                    }
                    break;
            }
        }

        public void DroppedItemLog(CacheItem i)
        {
            CacheBalance thisBalanceData = i.BalanceData;
            GilesItemType thisGilesItemType = Funky.DetermineItemType(i.InternalName, thisBalanceData.thisItemType, thisBalanceData.thisFollowerType);
            if (thisGilesItemType == GilesItemType.InfernalKey)
            {
                Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Keys.Dropped++;
                //	 Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Key]++;
                return;
            }

            switch (thisBalanceData.thisItemType)
            {
                case ItemType.CraftingPage:
                case ItemType.CraftingPlan:
                case ItemType.CraftingReagent:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Crafting.Dropped++;
                    //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Crafting]++;
                    break;
                case ItemType.Gem:
                    Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Gems.Dropped++;
                    // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[(int)LootIndex.Gem]++;
                    break;
                case ItemType.Amulet:
                case ItemType.Axe:
                case ItemType.Belt:
                case ItemType.Boots:
                case ItemType.Bow:
                case ItemType.Bracer:
                case ItemType.CeremonialDagger:
                case ItemType.Chest:
                case ItemType.Cloak:
                case ItemType.Crossbow:
                case ItemType.Dagger:
                case ItemType.Daibo:
                case ItemType.FistWeapon:
                case ItemType.FollowerSpecial:
                case ItemType.Gloves:
                case ItemType.HandCrossbow:
                case ItemType.Helm:
                case ItemType.Legs:
                case ItemType.Mace:
                case ItemType.MightyBelt:
                case ItemType.MightyWeapon:
                case ItemType.Mojo:
                case ItemType.Orb:
                case ItemType.Polearm:
                case ItemType.Quiver:
                case ItemType.Ring:
                case ItemType.Shield:
                case ItemType.Shoulder:
                case ItemType.Spear:
                case ItemType.SpiritStone:
                case ItemType.Staff:
                case ItemType.Sword:
                case ItemType.VoodooMask:
                case ItemType.Wand:
                case ItemType.WizardHat:
                    if (i.Itemquality.Value == ItemQuality.Legendary)
                    {
                        //Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[3]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Legendary.Dropped++;
                    }
                    else if (i.Itemquality.Value > ItemQuality.Magic3)
                    {
                        // Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[2]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Rare.Dropped++;
                    }
                    else
                    {
                        //  Bot.BotStatistics.ProfileStats.CurrentProfile.ItemStats.stashedItemTotals[1]++;
                        Bot.Game.CurrentGameStats.CurrentProfile.LootTracker.Magical.Dropped++;
                    }
                    break;
            }
        }

        public override string ToString()
        {
			return String.Format("Type       \t Drop \t Loot \t Stash \t Sold \t Salvaged\r\n" +
								 "Magical:    \t {0} \r\n" +
                                 "Rare:      \t {1} \r\n" +
                                 "Legendary: \t {2} \r\n" +
                                 "Gems:      \t {3} \r\n" +
                                 "Crafting: \t {4} \r\n" +
                                 "Keys:      \t {5} \r\n",
                                 this.Magical.ToString(), this.Rare.ToString(), this.Legendary.ToString(), this.Gems.ToString(), this.Crafting.ToString(), this.Keys.ToString());
        }
    }
}
