﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using fBaseXtensions.Helpers;
using fBaseXtensions.Items.Enums;

namespace fBaseXtensions.Cache.External.Objects
{
	public class ItemDataCollection
	{
		//TODO:: Create Classic and ROS Lists -- based on sno>300k? or name containing x1_

		public HashSet<DroppedItemEntry> DroppedItemCache { get; set; }
		public List<ItemStringEntry> DroppedItemInternalNames { get; set; }
		public HashSet<ItemDataEntry> ItemDataCache { get; set; } 
		public ItemDataCollection()
		{
			DroppedItemInternalNames = new List<ItemStringEntry>
			{
				new ItemStringEntry("ruby_", PluginDroppedItemTypes.Ruby),
				new ItemStringEntry("emerald_", PluginDroppedItemTypes.Emerald),
				new ItemStringEntry("topaz_", PluginDroppedItemTypes.Topaz),
				new ItemStringEntry("amethyst", PluginDroppedItemTypes.Amethyst),
				new ItemStringEntry("diamond_", PluginDroppedItemTypes.Diamond),
				new ItemStringEntry("horadric", PluginDroppedItemTypes.BloodShard),
				new ItemStringEntry("infernalmachine", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("demonkey_", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("demontrebuchetkey", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("craftingreagent", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("lootrunkey", PluginDroppedItemTypes.KeyFragment),
				new ItemStringEntry("crafting_", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("craftingmaterials_", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("flail1h_", PluginDroppedItemTypes.Flail),
				new ItemStringEntry("flail", PluginDroppedItemTypes.FlailTwoHanded),
				new ItemStringEntry("twohandedaxe_", PluginDroppedItemTypes.AxeTwoHanded),
				new ItemStringEntry("axe_", PluginDroppedItemTypes.Axe),
				new ItemStringEntry("ceremonialdagger_", PluginDroppedItemTypes.CeremonialKnife),
				new ItemStringEntry("handxbow_", PluginDroppedItemTypes.HandCrossbow),
				new ItemStringEntry("dagger_", PluginDroppedItemTypes.Dagger),
				new ItemStringEntry("fistweapon_", PluginDroppedItemTypes.FistWeapon),
				new ItemStringEntry("twohandedmace_", PluginDroppedItemTypes.MaceTwoHanded),
				new ItemStringEntry("mace_", PluginDroppedItemTypes.Mace),
				new ItemStringEntry("mightyweapon_1h_", PluginDroppedItemTypes.MightyWeapon),
				new ItemStringEntry("spear_", PluginDroppedItemTypes.Spear),
				new ItemStringEntry("twohandedsword_", PluginDroppedItemTypes.SwordTwoHanded),
				new ItemStringEntry("sword_", PluginDroppedItemTypes.Sword),
				new ItemStringEntry("wand_", PluginDroppedItemTypes.Wand),
				new ItemStringEntry("xbow_", PluginDroppedItemTypes.Crossbow),
				new ItemStringEntry("bow_", PluginDroppedItemTypes.Bow),
				new ItemStringEntry("combatstaff_", PluginDroppedItemTypes.Daibo),
				new ItemStringEntry("mightyweapon_2h_", PluginDroppedItemTypes.MightyWeaponTwoHanded),
				new ItemStringEntry("polearm_", PluginDroppedItemTypes.Polearm),
				new ItemStringEntry("staff_", PluginDroppedItemTypes.Staff),
				new ItemStringEntry("mojo_", PluginDroppedItemTypes.Mojo),
				new ItemStringEntry("orb_", PluginDroppedItemTypes.Source),
				new ItemStringEntry("quiver_", PluginDroppedItemTypes.Quiver),
				new ItemStringEntry("crushield_", PluginDroppedItemTypes.CrusaderShield),
				new ItemStringEntry("shield_", PluginDroppedItemTypes.Shield),
				new ItemStringEntry("amulet_", PluginDroppedItemTypes.Amulet),
				new ItemStringEntry("ring_", PluginDroppedItemTypes.Ring),
				new ItemStringEntry("boots_", PluginDroppedItemTypes.Boots),
				new ItemStringEntry("bracers_", PluginDroppedItemTypes.Bracers),
				new ItemStringEntry("cloak_", PluginDroppedItemTypes.Chest),
				new ItemStringEntry("gloves_", PluginDroppedItemTypes.Gloves),
				new ItemStringEntry("pants_", PluginDroppedItemTypes.Pants),
				new ItemStringEntry("barbbelt_", PluginDroppedItemTypes.Belt),
				new ItemStringEntry("shoulderpads_", PluginDroppedItemTypes.Shoulders),
				new ItemStringEntry("spiritstone_", PluginDroppedItemTypes.Helm),
				new ItemStringEntry("voodoomask_", PluginDroppedItemTypes.Helm),
				new ItemStringEntry("wizardhat_", PluginDroppedItemTypes.Helm),
				new ItemStringEntry("lore_book_", PluginDroppedItemTypes.LoreBook),
				new ItemStringEntry("page_of_", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("blacksmithstome", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("healthpotion", PluginDroppedItemTypes.Potion),
				new ItemStringEntry("followeritem_enchantress_", PluginDroppedItemTypes.FollowerTrinket),
				new ItemStringEntry("followeritem_scoundrel_", PluginDroppedItemTypes.FollowerTrinket),
				new ItemStringEntry("followeritem_templar_", PluginDroppedItemTypes.FollowerTrinket),
				new ItemStringEntry("jewelbox_", PluginDroppedItemTypes.FollowerTrinket),
				new ItemStringEntry("craftingplan_", PluginDroppedItemTypes.CraftingMaterial),
				new ItemStringEntry("healthglobe", PluginDroppedItemTypes.HealthGlobe),
				new ItemStringEntry("chestarmor_", PluginDroppedItemTypes.Chest),
				new ItemStringEntry("helm_", PluginDroppedItemTypes.Helm),
				new ItemStringEntry("helmcloth_", PluginDroppedItemTypes.Helm),
				new ItemStringEntry("belt_", PluginDroppedItemTypes.Belt),
			};

			#region Dropped Items
			DroppedItemCache = new HashSet<DroppedItemEntry>
			{
				//============================= ARMOR =============================
				//
				new DroppedItemEntry(3353, PluginDroppedItemTypes.Belt,"Belt_norm_base_04"),
				new DroppedItemEntry(3352, PluginDroppedItemTypes.Belt,"Belt_norm_base_03"),
				new DroppedItemEntry(3358, PluginDroppedItemTypes.Belt,"Belt_norm_base_flippy"),
				new DroppedItemEntry(3351, PluginDroppedItemTypes.Belt),
				new DroppedItemEntry(3354, PluginDroppedItemTypes.Belt, "Belt_norm_base_05-10802"),
				new DroppedItemEntry(3355, PluginDroppedItemTypes.Belt, "Belt_norm_base_06-5983"),

				//
				new DroppedItemEntry(205620, PluginDroppedItemTypes.Boots,"Boots_norm_unique_075"),
				new DroppedItemEntry(3433, PluginDroppedItemTypes.Boots),
				new DroppedItemEntry(3434, PluginDroppedItemTypes.Boots,"Boots_norm_base_03"),
				new DroppedItemEntry(3439, PluginDroppedItemTypes.Boots),
				new DroppedItemEntry(330, PluginDroppedItemTypes.Boots, "Boots_norm_base_04-11248"),
				new DroppedItemEntry(3436, PluginDroppedItemTypes.Boots, "Boots_norm_base_06-3778"),
				new DroppedItemEntry(3435, PluginDroppedItemTypes.Boots, "Boots_norm_base_05-4805"),
				new DroppedItemEntry(222463, PluginDroppedItemTypes.Boots, "Boots_norm_unique_050-13353"),
				new DroppedItemEntry(339125, PluginDroppedItemTypes.Boots, "x1_Boots_norm_unique_04-11551"),

				//
				new DroppedItemEntry(56323, PluginDroppedItemTypes.Bracers,"Bracers_norm_base_01"),
				new DroppedItemEntry(56324, PluginDroppedItemTypes.Bracers),
				new DroppedItemEntry(56325, PluginDroppedItemTypes.Bracers,"Bracers_norm_base_03"),
				new DroppedItemEntry(56327, PluginDroppedItemTypes.Bracers, "Bracers_norm_base_05-15573"),
				new DroppedItemEntry(56326, PluginDroppedItemTypes.Bracers, "Bracers_norm_base_04-6794"),
				new DroppedItemEntry(56328, PluginDroppedItemTypes.Bracers, "Bracers_norm_base_06-12613"),

				//
				new DroppedItemEntry(3802, PluginDroppedItemTypes.Chest,"chestArmor_norm_base_04"),
				new DroppedItemEntry(3799, PluginDroppedItemTypes.Chest,"chestArmor_norm_base_01"),
				new DroppedItemEntry(3800, PluginDroppedItemTypes.Chest,"chestArmor_norm_base_02"),
				new DroppedItemEntry(3801, PluginDroppedItemTypes.Chest),
				new DroppedItemEntry(3813, PluginDroppedItemTypes.Chest,"chestArmor_norm_base_flippy"),
				new DroppedItemEntry(3803, PluginDroppedItemTypes.Chest, "chestArmor_norm_base_05-7757"),
				new DroppedItemEntry(3804, PluginDroppedItemTypes.Chest, "chestArmor_norm_base_06-7127"),

				//
				new DroppedItemEntry(4264, PluginDroppedItemTypes.Gloves,"Gloves_norm_base_flippy"),

				//
				new DroppedItemEntry(205482, PluginDroppedItemTypes.Helm,"HelmCloth_norm_base_flippy"),
				new DroppedItemEntry(4463, PluginDroppedItemTypes.Helm,"Helm_norm_base_flippy"),
				new DroppedItemEntry(299444, PluginDroppedItemTypes.Helm, "x1_voodooMask_wdm_norm_unique_07-3884"),
				new DroppedItemEntry(369025, PluginDroppedItemTypes.Helm, "x1_helm_WDM_norm_set_16-2783"),

				//
				new DroppedItemEntry(4833, PluginDroppedItemTypes.Pants,"pants_norm_base_flippy"),

				//
				new DroppedItemEntry(5288, PluginDroppedItemTypes.Shoulders,"shoulderPads_norm_base_flippy"),

				//============================= WEAPONS =============================
				//
				new DroppedItemEntry(3256, PluginDroppedItemTypes.Axe, "Axe_norm_base_flippy_04"),
				new DroppedItemEntry(3254, PluginDroppedItemTypes.Axe,"Axe_norm_base_flippy_02"),
				new DroppedItemEntry(3255, PluginDroppedItemTypes.Axe),
				new DroppedItemEntry(367144, PluginDroppedItemTypes.Axe),
				new DroppedItemEntry(3258, PluginDroppedItemTypes.Axe, "Axe_norm_base_flippy_06"),
				new DroppedItemEntry(3257, PluginDroppedItemTypes.Axe, "Axe_norm_base_flippy_05"),
				new DroppedItemEntry(271592, PluginDroppedItemTypes.Axe, "x1_Axe_norm_unique_flippy_09"),
				new DroppedItemEntry(3253, PluginDroppedItemTypes.Axe, "Axe_norm_base_flippy_01-24475"),
				new DroppedItemEntry(335157, PluginDroppedItemTypes.Axe, "x1_Axe_norm_base_flippy_01-15153"),
				new DroppedItemEntry(190006, PluginDroppedItemTypes.Axe, "Axe_norm_unique_flippy_06-15977"),
				new DroppedItemEntry(3259, PluginDroppedItemTypes.Axe, "Axe_norm_base_flippy_07-6732"),

				//
				new DroppedItemEntry(6326, PluginDroppedItemTypes.AxeTwoHanded, "twoHandedAxe_norm_base_flippy_03"),
				new DroppedItemEntry(482, PluginDroppedItemTypes.AxeTwoHanded,"twoHandedAxe_norm_base_flippy_02"),
				new DroppedItemEntry(6325, PluginDroppedItemTypes.AxeTwoHanded,"twoHandedAxe_norm_base_flippy_01"),
				new DroppedItemEntry(186497, PluginDroppedItemTypes.AxeTwoHanded,"twoHandedAxe_norm_unique_flippy_03"),
				new DroppedItemEntry(367146, PluginDroppedItemTypes.AxeTwoHanded,"x1_twoHandedAxe_norm_base_flippy_02"),
				new DroppedItemEntry(6327, PluginDroppedItemTypes.AxeTwoHanded, "twoHandedAxe_norm_base_flippy_04-8339"),
				new DroppedItemEntry(6328, PluginDroppedItemTypes.AxeTwoHanded, "twoHandedAxe_norm_base_flippy_05-24277"),
				new DroppedItemEntry(186576, PluginDroppedItemTypes.AxeTwoHanded, "twoHandedAxe_norm_unique_flippy_02-23923"),
				new DroppedItemEntry(191065, PluginDroppedItemTypes.AxeTwoHanded, "twoHandedAxe_norm_unique_04-9710"),
				new DroppedItemEntry(335161, PluginDroppedItemTypes.AxeTwoHanded, "x1_twoHandedAxe_norm_base_flippy_01-20274"),

				//
				new DroppedItemEntry(3457, PluginDroppedItemTypes.Bow, "Bow_norm_base_flippy_05-5931"),
				new DroppedItemEntry(331, PluginDroppedItemTypes.Bow,"Bow_norm_base_flippy_01"),
				new DroppedItemEntry(367158, PluginDroppedItemTypes.Bow,"x1_bow_norm_base_flippy_02"),
				new DroppedItemEntry(3456, PluginDroppedItemTypes.Bow, "Bow_norm_base_flippy_04-11164"),
				new DroppedItemEntry(3454, PluginDroppedItemTypes.Bow, "Bow_norm_base_flippy_02-13808"),
				new DroppedItemEntry(335188, PluginDroppedItemTypes.Bow, "x1_bow_norm_base_flippy_01-5473"),
				new DroppedItemEntry(3455, PluginDroppedItemTypes.Bow, "Bow_norm_base_flippy_03-1103"),

				//
				new DroppedItemEntry(145927, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_base_flippy_03-7239"),
				new DroppedItemEntry(195821, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_unique_flippy_03-12833"),
				new DroppedItemEntry(367199, PluginDroppedItemTypes.CeremonialKnife),
				new DroppedItemEntry(139104, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_base_flippy_01-1837"),
				new DroppedItemEntry(145928, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_base_flippy_04-13805"),
				new DroppedItemEntry(145926, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_base_flippy_02-12711"),
				new DroppedItemEntry(326816, PluginDroppedItemTypes.CeremonialKnife, "x1_ceremonialDagger_norm_base_flippy_01-12060"),
				new DroppedItemEntry(271738, PluginDroppedItemTypes.CeremonialKnife, "x1_ceremonialDagger_norm_unique_10-9703"),
				new DroppedItemEntry(223035, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_unique_flippy_05-26194"),
				new DroppedItemEntry(193440, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_unique_flippy_02-115480"),
				new DroppedItemEntry(223386, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_unique_flippy_06-181600"),
				new DroppedItemEntry(195443, PluginDroppedItemTypes.CeremonialKnife, "ceremonialDagger_norm_unique_03_bladeEnergy-13546"),

				//
				new DroppedItemEntry(6622, PluginDroppedItemTypes.Crossbow, "XBow_norm_base_flippy_06-10533"),
				new DroppedItemEntry(6618, PluginDroppedItemTypes.Crossbow, "XBow_norm_base_flippy_02"),
				new DroppedItemEntry(6619, PluginDroppedItemTypes.Crossbow,"XBow_norm_base_flippy_03"),
				new DroppedItemEntry(6620, PluginDroppedItemTypes.Crossbow),
				new DroppedItemEntry(367161, PluginDroppedItemTypes.Crossbow,"x1_xbow_norm_base_flippy_02"),
				new DroppedItemEntry(6621, PluginDroppedItemTypes.Crossbow, "XBow_norm_base_flippy_05-10406"),
				new DroppedItemEntry(98162, PluginDroppedItemTypes.Crossbow, "XBow_norm_base_flippy_08-26299"),
				new DroppedItemEntry(6623, PluginDroppedItemTypes.Crossbow, "XBow_norm_base_flippy_07-1603"),
				new DroppedItemEntry(335191, PluginDroppedItemTypes.Crossbow, "x1_xbow_norm_base_flippy_01-13054"),
				new DroppedItemEntry(221763, PluginDroppedItemTypes.Crossbow, "XBow_norm_unique_flippy_06-30319"),

				//
				new DroppedItemEntry(3843, PluginDroppedItemTypes.Daibo, "combatStaff_norm_base_flippy_03-15187"),
				new DroppedItemEntry(367192, PluginDroppedItemTypes.Daibo, "x1_combatStaff_norm_base_flippy_02-23248"),

				//
				new DroppedItemEntry(3913, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_04-7545"),
				new DroppedItemEntry(3911, PluginDroppedItemTypes.Dagger),
				new DroppedItemEntry(367137, PluginDroppedItemTypes.Dagger),
				new DroppedItemEntry(3914, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_05-11593"),
				new DroppedItemEntry(3916, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_07-17646"),
				new DroppedItemEntry(3915, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_06-17642"),
				new DroppedItemEntry(3910, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_01-20297"),
				new DroppedItemEntry(335132, PluginDroppedItemTypes.Dagger, "x1_dagger_norm_base_flippy_01-16158"),
				new DroppedItemEntry(3912, PluginDroppedItemTypes.Dagger, "Dagger_norm_base_flippy_03-13694"),
				new DroppedItemEntry(195400, PluginDroppedItemTypes.Dagger, "Dagger_norm_unique_flippy_03-3435"),
				new DroppedItemEntry(192638, PluginDroppedItemTypes.Dagger, "Dagger_norm_unique_flippy_02-11188"),

				//
				new DroppedItemEntry(4140, PluginDroppedItemTypes.FistWeapon, "fistWeapon_norm_base_flippy_03-20926"),
				new DroppedItemEntry(367194, PluginDroppedItemTypes.FistWeapon, "x1_fistWeapon_norm_base_flippy_02-2721"),
				new DroppedItemEntry(335312, PluginDroppedItemTypes.FistWeapon, "x1_fistWeapon_norm_base_flippy_01-5293"),

				//
				new DroppedItemEntry(247389, PluginDroppedItemTypes.Flail,"x1_flail1H_norm_base_flippy_02"),
				new DroppedItemEntry(247392, PluginDroppedItemTypes.Flail,"x1_flail1H_norm_base_flippy_05"),
				new DroppedItemEntry(247390, PluginDroppedItemTypes.Flail, "x1_flail1H_norm_base_flippy_03-8053"),
				new DroppedItemEntry(247381, PluginDroppedItemTypes.Flail, "x1_flail1H_norm_base_05-5702"),
				new DroppedItemEntry(247391, PluginDroppedItemTypes.Flail, "x1_flail1H_norm_base_flippy_04-21586"),
				new DroppedItemEntry(247354, PluginDroppedItemTypes.Flail, "x1_flail1H_norm_base_flippy_01-8978"),
				new DroppedItemEntry(367103, PluginDroppedItemTypes.Flail, "x1_flail1H_norm_unique_flippy_07-1326"),

				//
				new DroppedItemEntry(247357, PluginDroppedItemTypes.FlailTwoHanded,"x1_flail2H_norm_base_flippy_01"),
				new DroppedItemEntry(247395, PluginDroppedItemTypes.FlailTwoHanded,"x1_flail2H_norm_base_flippy_02"),
				new DroppedItemEntry(247397, PluginDroppedItemTypes.FlailTwoHanded, "x1_flail2H_norm_base_flippy_04-17083"),
				new DroppedItemEntry(247398, PluginDroppedItemTypes.FlailTwoHanded, "x1_flail2H_norm_base_flippy_05-24496"),
				new DroppedItemEntry(247396, PluginDroppedItemTypes.FlailTwoHanded, "x1_flail2H_norm_base_flippy_03-10085"),

				//
				new DroppedItemEntry(145120, PluginDroppedItemTypes.HandCrossbow, "handXBow_norm_base_flippy_06-8485"),
				new DroppedItemEntry(145121, PluginDroppedItemTypes.HandCrossbow, "handXBow_norm_base_flippy_07-5677"),
				new DroppedItemEntry(145119, PluginDroppedItemTypes.HandCrossbow,"handXBow_norm_base_flippy_05"),
				new DroppedItemEntry(145093, PluginDroppedItemTypes.HandCrossbow),
				new DroppedItemEntry(367186, PluginDroppedItemTypes.HandCrossbow),
				new DroppedItemEntry(145122, PluginDroppedItemTypes.HandCrossbow, "handXBow_norm_base_flippy_08-2907"),
				new DroppedItemEntry(82630, PluginDroppedItemTypes.HandCrossbow, "handXbow_norm_base_flippy_01-7406"),
				new DroppedItemEntry(145118, PluginDroppedItemTypes.HandCrossbow, "handXBow_norm_base_flippy_04-5972"),
				new DroppedItemEntry(335371, PluginDroppedItemTypes.HandCrossbow, "x1_handXbow_norm_base_flippy_01-4978"),
				new DroppedItemEntry(192478, PluginDroppedItemTypes.HandCrossbow, "handXBow_norm_unique_flippy_01-14894"),

				//
				new DroppedItemEntry(4653, PluginDroppedItemTypes.Mace,"Mace_norm_base_flippy_01"),
				new DroppedItemEntry(367148, PluginDroppedItemTypes.Mace,"x1_Mace_norm_base_flippy_02"),
				new DroppedItemEntry(4657, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_05-14517"),
				new DroppedItemEntry(4656, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_04-11695"),
				new DroppedItemEntry(4658, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_06-21266"),
				new DroppedItemEntry(4659, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_07-22022"),
				new DroppedItemEntry(4655, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_03-957"),
				new DroppedItemEntry(335168, PluginDroppedItemTypes.Mace, "x1_Mace_norm_base_flippy_01-6790"),
				new DroppedItemEntry(188181, PluginDroppedItemTypes.Mace, "Mace_norm_unique_07-12305"),
				new DroppedItemEntry(188212, PluginDroppedItemTypes.Mace, "Mace_norm_unique_flippy_07-7852"),
				new DroppedItemEntry(188204, PluginDroppedItemTypes.Mace, "Mace_norm_unique_flippy_04-6001"),
				new DroppedItemEntry(188161, PluginDroppedItemTypes.Mace, "Mace_norm_unique_flippy_02-19179"),
				new DroppedItemEntry(188161, PluginDroppedItemTypes.Mace, "Mace_norm_unique_flippy_02-19179"),
				new DroppedItemEntry(188217, PluginDroppedItemTypes.Mace, "Mace_norm_unique_flippy_09-8942"),
				new DroppedItemEntry(271664, PluginDroppedItemTypes.Mace, "x1_Mace_norm_unique_flippy_11-9228"),
				new DroppedItemEntry(271649, PluginDroppedItemTypes.Mace, "x1_Mace_norm_unique_flippy_10-12436"),
				new DroppedItemEntry(4654, PluginDroppedItemTypes.Mace, "Mace_norm_base_flippy_02-3002"),

				//
				new DroppedItemEntry(6337, PluginDroppedItemTypes.MaceTwoHanded),
				new DroppedItemEntry(367152, PluginDroppedItemTypes.MaceTwoHanded),
				new DroppedItemEntry(6338, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_base_flippy_03-3310"),
				new DroppedItemEntry(6340, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_base_flippy_05-4080"),
				new DroppedItemEntry(6341, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_base_flippy_06-13839"),
				new DroppedItemEntry(6339, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_base_flippy_04-21580"),
				new DroppedItemEntry(335171, PluginDroppedItemTypes.MaceTwoHanded, "x1_twoHandedMace_norm_base_flippy_01-13662"),
				new DroppedItemEntry(99228, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_unique_flippy_02-3137"),
				new DroppedItemEntry(190885, PluginDroppedItemTypes.MaceTwoHanded, "twoHandedMace_norm_unique_flippy_04-656"),

				//
				new DroppedItemEntry(144192, PluginDroppedItemTypes.MightyWeapon, "mightyWeapon_1H_norm_base_flippy_02-1494"),
				new DroppedItemEntry(367169, PluginDroppedItemTypes.MightyWeapon, "x1_mightyWeapon_1H_norm_base_flippy_02-1846"),
				new DroppedItemEntry(144194, PluginDroppedItemTypes.MightyWeapon, "mightyWeapon_1H_norm_base_flippy_04-1864"),
				new DroppedItemEntry(144193, PluginDroppedItemTypes.MightyWeapon, "mightyWeapon_1H_norm_base_flippy_03-12964"),

				//
				new DroppedItemEntry(367171, PluginDroppedItemTypes.MightyWeaponTwoHanded,"x1_mightyWeapon_2H_norm_base_flippy_02"),
				new DroppedItemEntry(335395, PluginDroppedItemTypes.MightyWeaponTwoHanded, "x1_mightyWeapon_2H_norm_base_flippy_01-3363"),

				//
				new DroppedItemEntry(4878, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_06-7575"),
				new DroppedItemEntry(4874, PluginDroppedItemTypes.Polearm,"Polearm_norm_base_flippy_02"),
				new DroppedItemEntry(4875, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_03-970"),
				new DroppedItemEntry(406, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_07-18433"),
				new DroppedItemEntry(367154, PluginDroppedItemTypes.Polearm, "x1_polearm_norm_base_flippy_02-1120"),
				new DroppedItemEntry(196616, PluginDroppedItemTypes.Polearm, "Polearm_norm_unique_flippy_04-5870"),
				new DroppedItemEntry(4877, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_05-8434"),
				new DroppedItemEntry(335178, PluginDroppedItemTypes.Polearm, "x1_polearm_norm_base_flippy_01-2166"),
				new DroppedItemEntry(4876, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_04-5937"),
				new DroppedItemEntry(4879, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_08-774"),
				new DroppedItemEntry(191574, PluginDroppedItemTypes.Polearm, "Polearm_norm_unique_flippy_01-5224"),
				new DroppedItemEntry(4873, PluginDroppedItemTypes.Polearm, "Polearm_norm_base_flippy_01-14641"),

				//
				new DroppedItemEntry(5457, PluginDroppedItemTypes.Spear,"Spear_norm_base_flippy_01"),
				new DroppedItemEntry(5458, PluginDroppedItemTypes.Spear),
				new DroppedItemEntry(367156, PluginDroppedItemTypes.Spear),
				new DroppedItemEntry(5460, PluginDroppedItemTypes.Spear, "Spear_norm_base_flippy_04-19280"),
				new DroppedItemEntry(5459, PluginDroppedItemTypes.Spear, "Spear_norm_base_flippy_03-2545"),
				new DroppedItemEntry(197098, PluginDroppedItemTypes.Spear, "Spear_norm_unique_flippy_04-18555"),
				new DroppedItemEntry(5461, PluginDroppedItemTypes.Spear, "Spear_norm_base_flippy_05-19801"),
				new DroppedItemEntry(335181, PluginDroppedItemTypes.Spear, "x1_spear_norm_base_flippy_01-13786"),

				//
				new DroppedItemEntry(5491, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_02"),
				new DroppedItemEntry(5493, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_04-16574"),
				new DroppedItemEntry(5494, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_05-8358"),
				new DroppedItemEntry(5496, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_07-22734"),
				new DroppedItemEntry(367162, PluginDroppedItemTypes.Staff, "x1_Staff_norm_base_02-6191"),
				new DroppedItemEntry(59610, PluginDroppedItemTypes.Staff, "Staff_norm_unique_flippy_01-25475"),
				new DroppedItemEntry(328170, PluginDroppedItemTypes.Staff, "x1_Staff_norm_base_flippy_01-1922"),
				new DroppedItemEntry(184232, PluginDroppedItemTypes.Staff, "Staff_norm_unique_flippy_03-2202"),
				new DroppedItemEntry(194007, PluginDroppedItemTypes.Staff, "Staff_norm_unique_flippy_06-1279"),
				new DroppedItemEntry(193257, PluginDroppedItemTypes.Staff, "Staff_norm_unique_flippy_05-13224"),
				new DroppedItemEntry(195671, PluginDroppedItemTypes.Staff, "Staff_norm_unique_flippy_07-11602"),
				new DroppedItemEntry(5492, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_03-6947"),
				new DroppedItemEntry(5490, PluginDroppedItemTypes.Staff, "Staff_norm_base_flippy_01-14597"),

				//
				new DroppedItemEntry(5530, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_04-7841"),
				new DroppedItemEntry(5529, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_03"),
				new DroppedItemEntry(367140, PluginDroppedItemTypes.Sword),
				new DroppedItemEntry(5534, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_08-2609"),
				new DroppedItemEntry(5528, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_02-6635"),
				new DroppedItemEntry(5527, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_01-14837"),
				new DroppedItemEntry(5531, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_05-21488"),
				new DroppedItemEntry(335137, PluginDroppedItemTypes.Sword, "x1_Sword_norm_base_flippy_01-15995"),
				new DroppedItemEntry(5532, PluginDroppedItemTypes.Sword, "Sword_norm_base_flippy_06-9809"),
				new DroppedItemEntry(115146, PluginDroppedItemTypes.Sword, "Sword_norm_unique_flippy_02-23452"),
				new DroppedItemEntry(271622, PluginDroppedItemTypes.Sword, "x1_Sword_norm_unique_flippy_14-6575"),
				new DroppedItemEntry(115145, PluginDroppedItemTypes.Sword, "Sword_norm_unique_flippy_01-7363"),
				new DroppedItemEntry(220015, PluginDroppedItemTypes.Sword, "Sword_norm_unique_flippy_07-578"),
				new DroppedItemEntry(271620, PluginDroppedItemTypes.Sword, "x1_Sword_norm_unique_flippy_13-2812"),

				//
				new DroppedItemEntry(6348, PluginDroppedItemTypes.SwordTwoHanded,"twoHandedSword_norm_base_flippy_02"),
				new DroppedItemEntry(6349, PluginDroppedItemTypes.SwordTwoHanded,"twoHandedSword_norm_base_flippy_03"),
				new DroppedItemEntry(367142, PluginDroppedItemTypes.SwordTwoHanded,"x1_twoHandedSword_norm_base_flippy_02"),
				new DroppedItemEntry(6351, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_base_flippy_05-13187"),
				new DroppedItemEntry(6350, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_base_flippy_04-21068"),
				new DroppedItemEntry(6352, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_base_flippy_06-19494"),
				new DroppedItemEntry(102425, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_base_flippy_07-12859"),
				new DroppedItemEntry(6347, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_base_flippy_01-3028"),
				new DroppedItemEntry(335148, PluginDroppedItemTypes.SwordTwoHanded, "x1_twoHandedSword_norm_base_flippy_01-13325"),
				new DroppedItemEntry(59666, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_unique_flippy_01-3225"),
				new DroppedItemEntry(190362, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_unique_flippy_08-8966"),
				new DroppedItemEntry(198981, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_unique_flippy_09-8791"),
				new DroppedItemEntry(184189, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_unique_flippy_04-16946"),
				new DroppedItemEntry(184195, PluginDroppedItemTypes.SwordTwoHanded, "twoHandedSword_norm_unique_flippy_05-18881"),

				//
				new DroppedItemEntry(6431, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_05-11528"),
				new DroppedItemEntry(6429, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_03-2186"),
				new DroppedItemEntry(6432, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_06-15603"),
				new DroppedItemEntry(367203, PluginDroppedItemTypes.Wand,"x1_Wand_norm_base_flippy_02"),
				new DroppedItemEntry(6430, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_04-12251"),
				new DroppedItemEntry(6433, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_07-24809"),
				new DroppedItemEntry(335375, PluginDroppedItemTypes.Wand, "x1_Wand_norm_base_flippy_01-15541"),
				new DroppedItemEntry(6428, PluginDroppedItemTypes.Wand, "Wand_norm_base_flippy_02-5412"),

				//============================= JEWELERY =============================
				new DroppedItemEntry(3188, PluginDroppedItemTypes.Amulet),
				new DroppedItemEntry(63985, PluginDroppedItemTypes.Ring),
				new DroppedItemEntry(193056, PluginDroppedItemTypes.FollowerTrinket),

				//============================= OFFHAND =============================

				//
				new DroppedItemEntry(335033, PluginDroppedItemTypes.CrusaderShield,"x1_CruShield_norm_base_flippy_03"),
				new DroppedItemEntry(335038, PluginDroppedItemTypes.CrusaderShield),
				new DroppedItemEntry(367176, PluginDroppedItemTypes.CrusaderShield,"x1_CruShield_norm_base_flippy_08"),
				new DroppedItemEntry(335039, PluginDroppedItemTypes.CrusaderShield, "x1_CruShield_norm_base_flippy_05-3185"),
				new DroppedItemEntry(335040, PluginDroppedItemTypes.CrusaderShield, "x1_CruShield_norm_base_flippy_06-9340"),
				new DroppedItemEntry(335041, PluginDroppedItemTypes.CrusaderShield, "x1_CruShield_norm_base_flippy_07-14146"),
				new DroppedItemEntry(316612, PluginDroppedItemTypes.CrusaderShield, "x1_CruShield_norm_base_flippy_02-3319"),

				//
				new DroppedItemEntry(139096, PluginDroppedItemTypes.Mojo, "Mojo_norm_base_02-10188"),
				new DroppedItemEntry(194999, PluginDroppedItemTypes.Mojo, "Mojo_norm_unique_flippy_04-12327"),
				new DroppedItemEntry(146943, PluginDroppedItemTypes.Mojo, "Mojo_norm_base_flippy_03-2351"),
				new DroppedItemEntry(146942, PluginDroppedItemTypes.Mojo,"Mojo_norm_base_flippy_02"),
				new DroppedItemEntry(216525, PluginDroppedItemTypes.Mojo),
				new DroppedItemEntry(367196, PluginDroppedItemTypes.Mojo),
				new DroppedItemEntry(367195, PluginDroppedItemTypes.Mojo, "x1_Mojo_norm_base_02-17539"),
				new DroppedItemEntry(146944, PluginDroppedItemTypes.Mojo, "Mojo_norm_base_flippy_04-24863"),
				new DroppedItemEntry(146941, PluginDroppedItemTypes.Mojo, "Mojo_norm_base_flippy_01-9263"),
				new DroppedItemEntry(335260, PluginDroppedItemTypes.Mojo, "x1_Mojo_norm_base_flippy_01-20262"),
				new DroppedItemEntry(139095, PluginDroppedItemTypes.Mojo, "Mojo_norm_base_01-11670"),
				new DroppedItemEntry(195001, PluginDroppedItemTypes.Mojo, "Mojo_norm_unique_flippy_05-16239"),
				new DroppedItemEntry(211673, PluginDroppedItemTypes.Mojo, "Mojo_norm_unique_flippy_07-22764"),
				new DroppedItemEntry(335259, PluginDroppedItemTypes.Mojo, "x1_Mojo_norm_base_01-15051"),
				new DroppedItemEntry(367195, PluginDroppedItemTypes.Mojo, "x1_Mojo_norm_base_02-17539"),

				//
				new DroppedItemEntry(218695, PluginDroppedItemTypes.Quiver),

				//
				new DroppedItemEntry(5270, PluginDroppedItemTypes.Shield, "Shield_norm_base_flippy_04-2681"),
				new DroppedItemEntry(5271, PluginDroppedItemTypes.Shield, "Shield_norm_base_flippy_05-7566"),
				new DroppedItemEntry(5268, PluginDroppedItemTypes.Shield,"Shield_norm_base_flippy_02"),
				new DroppedItemEntry(5269, PluginDroppedItemTypes.Shield,"Shield_norm_base_flippy_03"),
				new DroppedItemEntry(367165, PluginDroppedItemTypes.Shield,"x1_Shield_norm_base_flippy_02"),
				new DroppedItemEntry(152660, PluginDroppedItemTypes.Shield, "Shield_norm_unique_flippy_03-18333"),
				new DroppedItemEntry(152666, PluginDroppedItemTypes.Shield, "Shield_norm_unique_03-16014"),
				new DroppedItemEntry(5267, PluginDroppedItemTypes.Shield, "Shield_norm_base_flippy_01-19143"),
				new DroppedItemEntry(5273, PluginDroppedItemTypes.Shield, "Shield_norm_base_flippy_07-20794"),
				new DroppedItemEntry(5272, PluginDroppedItemTypes.Shield, "Shield_norm_base_flippy_06-1082"),
				new DroppedItemEntry(195391, PluginDroppedItemTypes.Shield, "Shield_norm_unique_flippy_07-1304"),
				new DroppedItemEntry(335210, PluginDroppedItemTypes.Shield, "x1_Shield_norm_base_flippy_01-14924"),

				//
				new DroppedItemEntry(4817, PluginDroppedItemTypes.Source, "orb_norm_base_flippy_03-9464"),
				new DroppedItemEntry(4815, PluginDroppedItemTypes.Source, "orb_norm_base_flippy_01-10108"),
				new DroppedItemEntry(335314, PluginDroppedItemTypes.Source, "x1_orb_norm_base_flippy_01-10695"),
				new DroppedItemEntry(367205, PluginDroppedItemTypes.Source, "x1_orb_norm_base_flippy_02-2520"),

				//============================= CRAFTING =============================
				new DroppedItemEntry(137958, PluginDroppedItemTypes.CraftingMaterial,"CraftingMaterials_Flippy_Global"),
				new DroppedItemEntry(192598, PluginDroppedItemTypes.CraftingMaterial, "CraftingPlan_Smith_Drop"),
				new DroppedItemEntry(192600, PluginDroppedItemTypes.CraftingMaterial, "CraftingPlan_Jeweler_Drop"),
				new DroppedItemEntry(371083, PluginDroppedItemTypes.CraftingMaterial, "CraftingReagent_Legendary_Unique_Gloves_001_x1-8035"),

				new DroppedItemEntry(364695,PluginDroppedItemTypes.InfernalKey), 
				new DroppedItemEntry(364697, PluginDroppedItemTypes.InfernalKey),
				new DroppedItemEntry(364694,PluginDroppedItemTypes.InfernalKey),
				new DroppedItemEntry(364696, PluginDroppedItemTypes.InfernalKey),
				new DroppedItemEntry(255880, PluginDroppedItemTypes.InfernalKey),
				new DroppedItemEntry(255881, PluginDroppedItemTypes.InfernalKey),
				new DroppedItemEntry(255882,PluginDroppedItemTypes.InfernalKey),

				new DroppedItemEntry(323722, PluginDroppedItemTypes.KeyFragment,"LootRunKey"),

				//============================= MISC =============================
				new DroppedItemEntry(301283, PluginDroppedItemTypes.PowerGlobe,"Console_PowerGlobe"),
				new DroppedItemEntry(4267, PluginDroppedItemTypes.HealthGlobe,"HealthGlobe"),
				new DroppedItemEntry(85798, PluginDroppedItemTypes.HealthGlobe,"HealthGlobe_02"),
				new DroppedItemEntry(366139, PluginDroppedItemTypes.HealthGlobe, "x1_healthGlobe-2662"),

				//
				new DroppedItemEntry(376, PluginDroppedItemTypes.Gold,"GoldCoin"),
				new DroppedItemEntry(209200, PluginDroppedItemTypes.Gold),
				new DroppedItemEntry(4311, PluginDroppedItemTypes.Gold,"GoldLarge"),
				new DroppedItemEntry(4312, PluginDroppedItemTypes.Gold,"GoldMedium"),
				new DroppedItemEntry(4313, PluginDroppedItemTypes.Gold,"GoldSmall"),

				new DroppedItemEntry(304319, PluginDroppedItemTypes.Potion,"healthPotion_Console"),
				new DroppedItemEntry(341333, PluginDroppedItemTypes.Potion, "healthPotion_Legendary_01_x1"),

				new DroppedItemEntry(192866, PluginDroppedItemTypes.LoreBook),
				new DroppedItemEntry(218853, PluginDroppedItemTypes.LoreBook, "Lore_Book_Flippy"),

				new DroppedItemEntry(359504, PluginDroppedItemTypes.BloodShard),
				//============================= GEMS =============================
				new DroppedItemEntry(56860, PluginDroppedItemTypes.Amethyst, "Amethyst_01"),
				new DroppedItemEntry(56861, PluginDroppedItemTypes.Amethyst, "Amethyst_02"),
				new DroppedItemEntry(56862, PluginDroppedItemTypes.Amethyst, "Amethyst_03"),
				new DroppedItemEntry(56863, PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(56864, PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(56865, PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(56866,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(56867,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(283116,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(361564,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(361565,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(361566,PluginDroppedItemTypes.Amethyst), 
				new DroppedItemEntry(361567,PluginDroppedItemTypes.Amethyst), 

				new DroppedItemEntry(56874, PluginDroppedItemTypes.Diamond, "Diamond_01"),
				new DroppedItemEntry(56875, PluginDroppedItemTypes.Diamond, "Diamond_02"),
				new DroppedItemEntry(56876, PluginDroppedItemTypes.Diamond, "Diamond_03"),
				new DroppedItemEntry(56877, PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(56878,PluginDroppedItemTypes.Diamond),  
				new DroppedItemEntry(56879, PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(56880,PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(56881, PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(361559,PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(361560,PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(361561, PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(361562, PluginDroppedItemTypes.Diamond), 
				new DroppedItemEntry(361563,PluginDroppedItemTypes.Diamond),

				new DroppedItemEntry(56888, PluginDroppedItemTypes.Emerald, "Emerald_01"),
				new DroppedItemEntry(56889, PluginDroppedItemTypes.Emerald, "Emerald_02"),
				new DroppedItemEntry(56890, PluginDroppedItemTypes.Emerald, "Emerald_03"),
				new DroppedItemEntry(56891,PluginDroppedItemTypes.Emerald), 
				new DroppedItemEntry(56892, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(56893, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(56894, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(56895, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(283117,PluginDroppedItemTypes.Emerald), 
				new DroppedItemEntry(361492, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(361493, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(361494, PluginDroppedItemTypes.Emerald),
				new DroppedItemEntry(361495,PluginDroppedItemTypes.Emerald),

				new DroppedItemEntry(56846, PluginDroppedItemTypes.Ruby, "Ruby_01"),
				new DroppedItemEntry(56847, PluginDroppedItemTypes.Ruby, "Ruby_02"),
				new DroppedItemEntry(56848, PluginDroppedItemTypes.Ruby, "Ruby_03"),
				new DroppedItemEntry(56849, PluginDroppedItemTypes.Ruby), 
				new DroppedItemEntry(56850,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(56851,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(56852,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(56853,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(283118,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(361568,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(361569,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(361570,  PluginDroppedItemTypes.Ruby),
				new DroppedItemEntry(361571, PluginDroppedItemTypes.Ruby),

				new DroppedItemEntry(5625, PluginDroppedItemTypes.Topaz, "Topaz_normal-3048"),
				new DroppedItemEntry(56916, PluginDroppedItemTypes.Topaz, "Topaz_01"),
				new DroppedItemEntry(56917, PluginDroppedItemTypes.Topaz, "Topaz_02"),
				new DroppedItemEntry(56918, PluginDroppedItemTypes.Topaz, "Topaz_03"),
				new DroppedItemEntry(56919, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(56920, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(56921, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(56922,PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(56923, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(283119, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(361572, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(361573, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(361574, PluginDroppedItemTypes.Topaz),
				new DroppedItemEntry(361575,PluginDroppedItemTypes.Topaz)
			};
			
			#endregion

			#region Item Data
			ItemDataCache = new HashSet<ItemDataEntry>
			{
				//BASE ITEMS

				//========= JEWELERY =========//
				new ItemDataEntry(325062, PluginItemTypes.Amulet),new ItemDataEntry(325061, PluginItemTypes.Amulet),
				new ItemDataEntry(3179, PluginItemTypes.Amulet),
				new ItemDataEntry(3182, PluginItemTypes.Amulet),
				new ItemDataEntry(3181, PluginItemTypes.Amulet),
				new ItemDataEntry(3184, PluginItemTypes.Amulet),

				new ItemDataEntry(5044, PluginItemTypes.Ring),new ItemDataEntry(5043, PluginItemTypes.Ring),
				new ItemDataEntry(5030, PluginItemTypes.Ring),
				new ItemDataEntry(5035, PluginItemTypes.Ring),
				new ItemDataEntry(5036, PluginItemTypes.Ring),
				new ItemDataEntry(5034, PluginItemTypes.Ring),
				new ItemDataEntry(5038, PluginItemTypes.Ring),
				new ItemDataEntry(5039, PluginItemTypes.Ring),
				new ItemDataEntry(5037, PluginItemTypes.Ring),

				//========= ARMOR =========//
				new ItemDataEntry(253996, PluginItemTypes.Belt),new ItemDataEntry(253987, PluginItemTypes.Belt),
				new ItemDataEntry(139133, PluginItemTypes.Belt),
				new ItemDataEntry(139134, PluginItemTypes.Belt),
				new ItemDataEntry(139135, PluginItemTypes.Belt),
				new ItemDataEntry(139136, PluginItemTypes.Belt),
				new ItemDataEntry(139137, PluginItemTypes.Belt),

				new ItemDataEntry(253997, PluginItemTypes.Boots),new ItemDataEntry(253986, PluginItemTypes.Boots),
				new ItemDataEntry(58878, PluginItemTypes.Boots),
				new ItemDataEntry(58877, PluginItemTypes.Boots),
				new ItemDataEntry(58879, PluginItemTypes.Boots),
				new ItemDataEntry(58880, PluginItemTypes.Boots),
				new ItemDataEntry(58881, PluginItemTypes.Boots),
				new ItemDataEntry(58904, PluginItemTypes.Boots),

				new ItemDataEntry(253988, PluginItemTypes.Bracers),new ItemDataEntry(253995, PluginItemTypes.Bracers),
				new ItemDataEntry(56324, PluginItemTypes.Bracers),
				new ItemDataEntry(56328, PluginItemTypes.Bracers),
				new ItemDataEntry(56325, PluginItemTypes.Bracers),
				new ItemDataEntry(56326, PluginItemTypes.Bracers),
				new ItemDataEntry(56323, PluginItemTypes.Bracers),
				new ItemDataEntry(56327, PluginItemTypes.Bracers),

				new ItemDataEntry(253994, PluginItemTypes.Chest),new ItemDataEntry(253983, PluginItemTypes.Chest),
				new ItemDataEntry(3804, PluginItemTypes.Chest),
				new ItemDataEntry(58887, PluginItemTypes.Chest),
				new ItemDataEntry(58889, PluginItemTypes.Chest),
				new ItemDataEntry(58888, PluginItemTypes.Chest),
				new ItemDataEntry(58890, PluginItemTypes.Chest),

				new ItemDataEntry(367188, PluginItemTypes.Cloak),new ItemDataEntry(335376, PluginItemTypes.Cloak),
				new ItemDataEntry(139089, PluginItemTypes.Cloak),

				new ItemDataEntry(253993, PluginItemTypes.Gloves),new ItemDataEntry(253985, PluginItemTypes.Gloves),
				new ItemDataEntry(58917, PluginItemTypes.Gloves),
				new ItemDataEntry(58919, PluginItemTypes.Gloves),
				new ItemDataEntry(70412, PluginItemTypes.Gloves),
				new ItemDataEntry(70411, PluginItemTypes.Gloves),
				new ItemDataEntry(58921, PluginItemTypes.Gloves),

				new ItemDataEntry(253992, PluginItemTypes.Helm),new ItemDataEntry(239290, PluginItemTypes.Helm),
				new ItemDataEntry(57779, PluginItemTypes.Helm),
				new ItemDataEntry(57778, PluginItemTypes.Helm),
				new ItemDataEntry(57780, PluginItemTypes.Helm),
				new ItemDataEntry(57781, PluginItemTypes.Helm),
				new ItemDataEntry(57784, PluginItemTypes.Helm),
				new ItemDataEntry(57782, PluginItemTypes.Helm),

				new ItemDataEntry(253984, PluginItemTypes.Pants),new ItemDataEntry(253991, PluginItemTypes.Pants),
				new ItemDataEntry(58932, PluginItemTypes.Pants),
				new ItemDataEntry(58933, PluginItemTypes.Pants),
				new ItemDataEntry(58935, PluginItemTypes.Pants),
				new ItemDataEntry(58934, PluginItemTypes.Pants),
				new ItemDataEntry(58937, PluginItemTypes.Pants),

				new ItemDataEntry(367173, PluginItemTypes.MightyBelt),new ItemDataEntry(367172, PluginItemTypes.MightyBelt),
				
				new ItemDataEntry(253990, PluginItemTypes.Shoulders),new ItemDataEntry(239289, PluginItemTypes.Shoulders),
				new ItemDataEntry(58692, PluginItemTypes.Shoulders),
				new ItemDataEntry(5285, PluginItemTypes.Shoulders),
				new ItemDataEntry(58694, PluginItemTypes.Shoulders),
				new ItemDataEntry(58693, PluginItemTypes.Shoulders),
				new ItemDataEntry(58697, PluginItemTypes.Shoulders),
				new ItemDataEntry(58695, PluginItemTypes.Shoulders),

				new ItemDataEntry(335392, PluginItemTypes.SpiritStone),new ItemDataEntry(364156, PluginItemTypes.SpiritStone),
				new ItemDataEntry(139114, PluginItemTypes.VoodooMask),

				new ItemDataEntry(367197, PluginItemTypes.VoodooMask),new ItemDataEntry(335387, PluginItemTypes.VoodooMask),
				new ItemDataEntry(139115, PluginItemTypes.VoodooMask),

				new ItemDataEntry(367201, PluginItemTypes.WizardHat),new ItemDataEntry(335378, PluginItemTypes.WizardHat),
				new ItemDataEntry(139126, PluginItemTypes.WizardHat),
				new ItemDataEntry(139125, PluginItemTypes.WizardHat),

				


				//========= WEAPONS =========//
				new ItemDataEntry(367143, PluginItemTypes.Axe),
				new ItemDataEntry(335155, PluginItemTypes.Axe),
				new ItemDataEntry(3247, PluginItemTypes.Axe),
				new ItemDataEntry(3249, PluginItemTypes.Axe),
				new ItemDataEntry(3248, PluginItemTypes.Axe),
				new ItemDataEntry(3246, PluginItemTypes.Axe),
				new ItemDataEntry(3251, PluginItemTypes.Axe),

				new ItemDataEntry(326811, PluginItemTypes.CeremonialKnife),
				new ItemDataEntry(367198, PluginItemTypes.CeremonialKnife),
				
				new ItemDataEntry(367136, PluginItemTypes.Dagger),
				new ItemDataEntry(335128, PluginItemTypes.Dagger),
				new ItemDataEntry(3908, PluginItemTypes.Dagger),
				new ItemDataEntry(3905, PluginItemTypes.Dagger),
				new ItemDataEntry(3903, PluginItemTypes.Dagger),

				new ItemDataEntry(327966, PluginItemTypes.TwoHandDaibo),
				new ItemDataEntry(367191, PluginItemTypes.TwoHandDaibo),
				
				new ItemDataEntry(247381, PluginItemTypes.Flail),
				new ItemDataEntry(247380, PluginItemTypes.Flail),
				new ItemDataEntry(247379, PluginItemTypes.Flail),

				new ItemDataEntry(367193, PluginItemTypes.FistWeapon),
				new ItemDataEntry(328572, PluginItemTypes.FistWeapon),
				
				new ItemDataEntry(335369, PluginItemTypes.HandCrossbow),
				new ItemDataEntry(367185, PluginItemTypes.HandCrossbow),
				new ItemDataEntry(145086, PluginItemTypes.HandCrossbow),
				new ItemDataEntry(145084, PluginItemTypes.HandCrossbow),

				new ItemDataEntry(367147, PluginItemTypes.Mace),
				new ItemDataEntry(335166, PluginItemTypes.Mace),
				new ItemDataEntry(4647, PluginItemTypes.Mace),
				new ItemDataEntry(4648, PluginItemTypes.Mace),
				new ItemDataEntry(4650, PluginItemTypes.Mace),
				new ItemDataEntry(4646, PluginItemTypes.Mace),

				new ItemDataEntry(367168, PluginItemTypes.MightyWeapon),
				new ItemDataEntry(335340, PluginItemTypes.MightyWeapon),
				
				new ItemDataEntry(367155, PluginItemTypes.Spear),
				new ItemDataEntry(335179, PluginItemTypes.Spear),
				new ItemDataEntry(5455, PluginItemTypes.Spear),

				new ItemDataEntry(335133, PluginItemTypes.Sword),
				new ItemDataEntry(367139, PluginItemTypes.Sword),
				new ItemDataEntry(5521, PluginItemTypes.Sword),
				new ItemDataEntry(5524, PluginItemTypes.Sword),

				new ItemDataEntry(335159, PluginItemTypes.TwoHandAxe),
				new ItemDataEntry(367145, PluginItemTypes.TwoHandAxe),
				new ItemDataEntry(6319, PluginItemTypes.TwoHandAxe),
				new ItemDataEntry(6322, PluginItemTypes.TwoHandAxe),

				new ItemDataEntry(367157, PluginItemTypes.TwoHandBow),
				new ItemDataEntry(335186, PluginItemTypes.TwoHandBow),
				new ItemDataEntry(3445, PluginItemTypes.TwoHandBow),
				new ItemDataEntry(3447, PluginItemTypes.TwoHandBow),

				new ItemDataEntry(335189, PluginItemTypes.TwoHandCrossbow),
				new ItemDataEntry(367159, PluginItemTypes.TwoHandCrossbow),
				new ItemDataEntry(6614, PluginItemTypes.TwoHandCrossbow),
				new ItemDataEntry(6613, PluginItemTypes.TwoHandCrossbow),

				new ItemDataEntry(247387, PluginItemTypes.TwoHandFlail),
				new ItemDataEntry(247386, PluginItemTypes.TwoHandFlail),
				new ItemDataEntry(247384, PluginItemTypes.TwoHandFlail),

				new ItemDataEntry(335169, PluginItemTypes.TwoHandMace),
				new ItemDataEntry(367151, PluginItemTypes.TwoHandMace),
				new ItemDataEntry(6331, PluginItemTypes.TwoHandMace),
				new ItemDataEntry(6332, PluginItemTypes.TwoHandMace),
				new ItemDataEntry(6335, PluginItemTypes.TwoHandMace),

				new ItemDataEntry(367170, PluginItemTypes.TwoHandMighty),
				new ItemDataEntry(221451, PluginItemTypes.TwoHandMighty),
				
				new ItemDataEntry(335176, PluginItemTypes.TwoHandPolearm),
				new ItemDataEntry(367153, PluginItemTypes.TwoHandPolearm),
				new ItemDataEntry(4865, PluginItemTypes.TwoHandPolearm),
				new ItemDataEntry(4864, PluginItemTypes.TwoHandPolearm),
				new ItemDataEntry(4871, PluginItemTypes.TwoHandPolearm),

				new ItemDataEntry(367162, PluginItemTypes.TwoHandStaff),
				new ItemDataEntry(328169, PluginItemTypes.TwoHandStaff),
				new ItemDataEntry(5487, PluginItemTypes.TwoHandStaff),
				new ItemDataEntry(5485, PluginItemTypes.TwoHandStaff),
				new ItemDataEntry(5483, PluginItemTypes.TwoHandStaff),

				new ItemDataEntry(335139, PluginItemTypes.TwoHandSword),
				new ItemDataEntry(367141, PluginItemTypes.TwoHandSword),
				new ItemDataEntry(6345, PluginItemTypes.TwoHandSword),

				new ItemDataEntry(367202, PluginItemTypes.Wand),
				new ItemDataEntry(335373, PluginItemTypes.Wand),
				new ItemDataEntry(6421, PluginItemTypes.Wand),
				new ItemDataEntry(6422, PluginItemTypes.Wand),
				new ItemDataEntry(6426, PluginItemTypes.Wand),

				new ItemDataEntry(367175, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(335037, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(314462, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(335031, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(335035, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(335034, PluginItemTypes.CrusaderShield),
				new ItemDataEntry(335036, PluginItemTypes.CrusaderShield),

				new ItemDataEntry(335259, PluginItemTypes.Mojo),
				new ItemDataEntry(367195, PluginItemTypes.Mojo),
				
				new ItemDataEntry(367204, PluginItemTypes.Source),
				new ItemDataEntry(327063, PluginItemTypes.Source),
				
				new ItemDataEntry(367184, PluginItemTypes.Quiver),
				new ItemDataEntry(367183, PluginItemTypes.Quiver),
				
				new ItemDataEntry(367164, PluginItemTypes.Shield),
				new ItemDataEntry(335208, PluginItemTypes.Shield),
				new ItemDataEntry(5262, PluginItemTypes.Shield),
				new ItemDataEntry(5261, PluginItemTypes.Shield),
				new ItemDataEntry(5260, PluginItemTypes.Shield),
				new ItemDataEntry(5264, PluginItemTypes.Shield),
				new ItemDataEntry(5263, PluginItemTypes.Shield),
				new ItemDataEntry(426, PluginItemTypes.Shield),

				new ItemDataEntry(190635, PluginItemTypes.FollowerEnchantress),
				new ItemDataEntry(190632, PluginItemTypes.FollowerEnchantress),
				new ItemDataEntry(190636, PluginItemTypes.FollowerEnchantress),

				new ItemDataEntry(190639, PluginItemTypes.FollowerScoundrel),
				new ItemDataEntry(190638, PluginItemTypes.FollowerScoundrel),
				new ItemDataEntry(190641, PluginItemTypes.FollowerScoundrel),

				new ItemDataEntry(190628, PluginItemTypes.FollowerTemplar),
				new ItemDataEntry(190629, PluginItemTypes.FollowerTemplar),


				//MISC ITEMS
				new ItemDataEntry(192598, PluginItemTypes.CraftingPlan),
				new ItemDataEntry(192600, PluginItemTypes.CraftingPlan),

				new ItemDataEntry(304319, PluginItemTypes.HealthPotion),
				new ItemDataEntry(344093, PluginItemTypes.HealthPotion),
				new ItemDataEntry(342824, PluginItemTypes.HealthPotion),
				new ItemDataEntry(341343, PluginItemTypes.HealthPotion),
				new ItemDataEntry(341342, PluginItemTypes.HealthPotion),
				new ItemDataEntry(342823, PluginItemTypes.HealthPotion),
				new ItemDataEntry(341333, PluginItemTypes.HealthPotion),

				//Crafting Materials
				new ItemDataEntry(189860, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(361984, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(189861, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(361985, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(189862, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(361986, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(189863, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(361988, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(283101, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(361989, PluginItemTypes.CraftingMaterial),

				//Staff Of Herding Materials
				new ItemDataEntry(214604, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(214603, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(180697, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(214605, PluginItemTypes.CraftingMaterial),
				

				//Uber Materials (Organs)
				new ItemDataEntry(364722, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(364723, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(364724, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(364725, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(257736, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(257739, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(257738, PluginItemTypes.CraftingMaterial),
				
				//Infernal Machines
				new ItemDataEntry(257737, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(366946, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(366949, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(366947, PluginItemTypes.CraftingMaterial),
				new ItemDataEntry(366948, PluginItemTypes.CraftingMaterial),
				

				//Infernal Keys
				new ItemDataEntry(364695, PluginItemTypes.InfernalKey),
				new ItemDataEntry(364697, PluginItemTypes.InfernalKey),
				new ItemDataEntry(364694, PluginItemTypes.InfernalKey),
				new ItemDataEntry(364696, PluginItemTypes.InfernalKey),
				new ItemDataEntry(255880, PluginItemTypes.InfernalKey),
				new ItemDataEntry(255881, PluginItemTypes.InfernalKey),
				new ItemDataEntry(255882, PluginItemTypes.InfernalKey),
				
				//Horadric Cache
				new ItemDataEntry(364329, PluginItemTypes.HoradricCache),
				new ItemDataEntry(360166, PluginItemTypes.HoradricCache),

				new ItemDataEntry(323722, PluginItemTypes.KeyStone),

				//Gems
				new ItemDataEntry(56888, PluginItemTypes.Emerald),
				new ItemDataEntry(56889, PluginItemTypes.Emerald),
				new ItemDataEntry(56890, PluginItemTypes.Emerald),
				new ItemDataEntry(56891, PluginItemTypes.Emerald),
				new ItemDataEntry(56892, PluginItemTypes.Emerald),
				new ItemDataEntry(56893, PluginItemTypes.Emerald),
				new ItemDataEntry(56894, PluginItemTypes.Emerald),
				new ItemDataEntry(56895, PluginItemTypes.Emerald),
				new ItemDataEntry(283117, PluginItemTypes.Emerald),
				new ItemDataEntry(361492, PluginItemTypes.Emerald),
				new ItemDataEntry(361493, PluginItemTypes.Emerald),
				new ItemDataEntry(361494, PluginItemTypes.Emerald),
				new ItemDataEntry(361495, PluginItemTypes.Emerald),
				//
				new ItemDataEntry(56874, PluginItemTypes.Diamond),
				new ItemDataEntry(56875, PluginItemTypes.Diamond),
				new ItemDataEntry(56876, PluginItemTypes.Diamond),
				new ItemDataEntry(56877, PluginItemTypes.Diamond),
				new ItemDataEntry(56878, PluginItemTypes.Diamond),
				new ItemDataEntry(56879, PluginItemTypes.Diamond),
				new ItemDataEntry(56880, PluginItemTypes.Diamond),
				new ItemDataEntry(56881, PluginItemTypes.Diamond),
				new ItemDataEntry(361559, PluginItemTypes.Diamond),
				new ItemDataEntry(361560, PluginItemTypes.Diamond),
				new ItemDataEntry(361561, PluginItemTypes.Diamond),
				new ItemDataEntry(361562, PluginItemTypes.Diamond),
				new ItemDataEntry(361563, PluginItemTypes.Diamond),
				//
				new ItemDataEntry(56916, PluginItemTypes.Topaz),
				new ItemDataEntry(56917, PluginItemTypes.Topaz),
				new ItemDataEntry(56918, PluginItemTypes.Topaz),
				new ItemDataEntry(56919, PluginItemTypes.Topaz),
				new ItemDataEntry(56920, PluginItemTypes.Topaz),
				new ItemDataEntry(56921, PluginItemTypes.Topaz),
				new ItemDataEntry(56922, PluginItemTypes.Topaz),
				new ItemDataEntry(56923, PluginItemTypes.Topaz),
				new ItemDataEntry(283119, PluginItemTypes.Topaz),
				new ItemDataEntry(361572, PluginItemTypes.Topaz),
				new ItemDataEntry(361573, PluginItemTypes.Topaz),
				new ItemDataEntry(361574, PluginItemTypes.Topaz),
				new ItemDataEntry(361575, PluginItemTypes.Topaz),
				//
				new ItemDataEntry(56846, PluginItemTypes.Ruby),
				new ItemDataEntry(56847, PluginItemTypes.Ruby),
				new ItemDataEntry(56848, PluginItemTypes.Ruby),
				new ItemDataEntry(56849, PluginItemTypes.Ruby),
				new ItemDataEntry(56850, PluginItemTypes.Ruby),
				new ItemDataEntry(56851, PluginItemTypes.Ruby),
				new ItemDataEntry(56852, PluginItemTypes.Ruby),
				new ItemDataEntry(56853, PluginItemTypes.Ruby),
				new ItemDataEntry(283118, PluginItemTypes.Ruby),
				new ItemDataEntry(361568, PluginItemTypes.Ruby),
				new ItemDataEntry(361569, PluginItemTypes.Ruby),
				new ItemDataEntry(361570, PluginItemTypes.Ruby),
				new ItemDataEntry(361571, PluginItemTypes.Ruby),
				//
				new ItemDataEntry(56860, PluginItemTypes.Amethyst),
				new ItemDataEntry(56861, PluginItemTypes.Amethyst),
				new ItemDataEntry(56862, PluginItemTypes.Amethyst),
				new ItemDataEntry(56863, PluginItemTypes.Amethyst),
				new ItemDataEntry(56864, PluginItemTypes.Amethyst),
				new ItemDataEntry(56865, PluginItemTypes.Amethyst),
				new ItemDataEntry(56866, PluginItemTypes.Amethyst),
				new ItemDataEntry(56867, PluginItemTypes.Amethyst),
				new ItemDataEntry(283116, PluginItemTypes.Amethyst),
				new ItemDataEntry(361564, PluginItemTypes.Amethyst),
				new ItemDataEntry(361565, PluginItemTypes.Amethyst),
				new ItemDataEntry(361566, PluginItemTypes.Amethyst),
				new ItemDataEntry(361567, PluginItemTypes.Amethyst),

				//,,,,,,,,,,,,,,
				new ItemDataEntry(148299, PluginItemTypes.Dye),
				new ItemDataEntry(148311, PluginItemTypes.Dye),
				new ItemDataEntry(54505, PluginItemTypes.Dye),
				new ItemDataEntry(148304, PluginItemTypes.Dye),
				new ItemDataEntry(148298, PluginItemTypes.Dye),
				new ItemDataEntry(148303, PluginItemTypes.Dye),
				new ItemDataEntry(148296, PluginItemTypes.Dye),
				new ItemDataEntry(148309, PluginItemTypes.Dye),
				new ItemDataEntry(212182, PluginItemTypes.Dye),
				new ItemDataEntry(212183, PluginItemTypes.Dye),
				new ItemDataEntry(148305, PluginItemTypes.Dye),
				new ItemDataEntry(148307, PluginItemTypes.Dye),
				new ItemDataEntry(148308, PluginItemTypes.Dye),
				new ItemDataEntry(148310, PluginItemTypes.Dye),
				new ItemDataEntry(148288, PluginItemTypes.Dye),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),
				//new ItemDataEntry(000000, PluginItemTypes.Unknown),


				//LEGENDARY ITEMS

				

				//Single Items
				new ItemDataEntry(298050, PluginItemTypes.Amulet, LegendaryItemTypes.CountessJuliasCameo),
				new ItemDataEntry(298054, PluginItemTypes.Amulet, LegendaryItemTypes.DovuEnergyTrap),
				//new ItemDataEntry(341342, PluginItemTypes.Amulet, LegendaryItemTypes.None), //ExecutionersMedal
				new ItemDataEntry(197823, PluginItemTypes.Amulet, LegendaryItemTypes.EyeofEtlich),
				new ItemDataEntry(298052, PluginItemTypes.Amulet, LegendaryItemTypes.GoldenGorgetofLeoric), //GoldenGorgetofLeoric
				new ItemDataEntry(298056, PluginItemTypes.Amulet, LegendaryItemTypes.HalcyonsAscent),
				new ItemDataEntry(297806, PluginItemTypes.Amulet, LegendaryItemTypes.HauntofVaxo),
				new ItemDataEntry(197822, PluginItemTypes.Amulet, LegendaryItemTypes.HolyBeacon),
				new ItemDataEntry(197812, PluginItemTypes.Amulet, LegendaryItemTypes.KymbosGold),
				new ItemDataEntry(197824, PluginItemTypes.Amulet, LegendaryItemTypes.MarasKaleidoscope),
				new ItemDataEntry(197813, PluginItemTypes.Amulet, LegendaryItemTypes.MoonlightWard),
				new ItemDataEntry(197815, PluginItemTypes.Amulet, LegendaryItemTypes.Ouroboros),
				new ItemDataEntry(298053, PluginItemTypes.Amulet, LegendaryItemTypes.OverwhelmingDesire), //OverwhelmingDesire
				new ItemDataEntry(298055, PluginItemTypes.Amulet, LegendaryItemTypes.RakoffsGlassofLife),
				new ItemDataEntry(197818, PluginItemTypes.Amulet, LegendaryItemTypes.RondalsLocket),
				new ItemDataEntry(197819, PluginItemTypes.Amulet, LegendaryItemTypes.SquirtsNecklace),
				new ItemDataEntry(197821, PluginItemTypes.Amulet, LegendaryItemTypes.TalismanofAranoch),
				new ItemDataEntry(298051, PluginItemTypes.Amulet, LegendaryItemTypes.TheEssofJohan),
				new ItemDataEntry(193659, PluginItemTypes.Amulet, LegendaryItemTypes.TheFlavorofTime),
				new ItemDataEntry(197817, PluginItemTypes.Amulet, LegendaryItemTypes.TheStarofAzkaranth),
				new ItemDataEntry(197814, PluginItemTypes.Amulet, LegendaryItemTypes.XephirianAmulet),

				new ItemDataEntry(298095, PluginItemTypes.Ring, LegendaryItemTypes.AvariceBand), //AvariceBand
				new ItemDataEntry(197834, PluginItemTypes.Ring, LegendaryItemTypes.BandofHollowWhispers),
				new ItemDataEntry(298093, PluginItemTypes.Ring, LegendaryItemTypes.BandoftheRueChambers),
				new ItemDataEntry(212602, PluginItemTypes.Ring, LegendaryItemTypes.BandofUntoldSecrets),
				new ItemDataEntry(212589, PluginItemTypes.Ring, LegendaryItemTypes.BrokenPromises),
				new ItemDataEntry(212603, PluginItemTypes.Ring, LegendaryItemTypes.BulKathossWeddingBand),
				new ItemDataEntry(212601, PluginItemTypes.Ring, LegendaryItemTypes.EternalUnion),
				new ItemDataEntry(260327, PluginItemTypes.Ring, LegendaryItemTypes.HellfireRing),
				new ItemDataEntry(212590, PluginItemTypes.Ring, LegendaryItemTypes.JusticeLantern),
				new ItemDataEntry(197835, PluginItemTypes.Ring, LegendaryItemTypes.LeoricsSignet),
				new ItemDataEntry(212546, PluginItemTypes.Ring, LegendaryItemTypes.ManaldHeal),
				new ItemDataEntry(212586, PluginItemTypes.Ring, LegendaryItemTypes.Nagelring),
				new ItemDataEntry(212588, PluginItemTypes.Ring, LegendaryItemTypes.ObsidianRingoftheZodiac),
				new ItemDataEntry(212648, PluginItemTypes.Ring, LegendaryItemTypes.OculusRing),
				new ItemDataEntry(298096, PluginItemTypes.Ring, LegendaryItemTypes.PandemoniumLoop), //PandemoniumLoop
				new ItemDataEntry(298091, PluginItemTypes.Ring, LegendaryItemTypes.RechelsRingofLarceny),
				new ItemDataEntry(298090, PluginItemTypes.Ring, LegendaryItemTypes.RogarsHugeStone),
				new ItemDataEntry(212618, PluginItemTypes.Ring, LegendaryItemTypes.SkullGrasp),
				new ItemDataEntry(197839, PluginItemTypes.Ring, LegendaryItemTypes.StolenRing),
				new ItemDataEntry(212582, PluginItemTypes.Ring, LegendaryItemTypes.StoneofJordan),
				new ItemDataEntry(197837, PluginItemTypes.Ring, LegendaryItemTypes.PuzzleRing),
				new ItemDataEntry(298088, PluginItemTypes.Ring,  LegendaryItemTypes.TheTallMansFinger),
				new ItemDataEntry(298094, PluginItemTypes.Ring, LegendaryItemTypes.RingofRoyalGrandeur),
				new ItemDataEntry(212581, PluginItemTypes.Ring, LegendaryItemTypes.Unity),
				new ItemDataEntry(298089, PluginItemTypes.Ring, LegendaryItemTypes.Wyrdward),

				new ItemDataEntry(193666, PluginItemTypes.Belt, LegendaryItemTypes.AngelHairBraid),
				new ItemDataEntry(298127, PluginItemTypes.Belt, LegendaryItemTypes.CordoftheSherma),
				new ItemDataEntry(193667, PluginItemTypes.Belt, LegendaryItemTypes.FleetingStrap),
				new ItemDataEntry(193671, PluginItemTypes.Belt, LegendaryItemTypes.Goldwrap),
				new ItemDataEntry(298129, PluginItemTypes.Belt, LegendaryItemTypes.HarringtonWaistguard),
				new ItemDataEntry(193668, PluginItemTypes.Belt, LegendaryItemTypes.HellcatWaistguard),
				new ItemDataEntry(298131, PluginItemTypes.Belt, LegendaryItemTypes.HwojWrap),
				new ItemDataEntry(298126, PluginItemTypes.Belt, LegendaryItemTypes.InsatiableBelt), //InsatiableBelt
				new ItemDataEntry(298130, PluginItemTypes.Belt, LegendaryItemTypes.JangsEnvelopment),
				new ItemDataEntry(197836, PluginItemTypes.Belt, LegendaryItemTypes.KredesFlame),
				new ItemDataEntry(298124, PluginItemTypes.Belt, LegendaryItemTypes.RazorStrop),
				new ItemDataEntry(193664, PluginItemTypes.Belt, LegendaryItemTypes.SaffronWrap),
				new ItemDataEntry(298125, PluginItemTypes.Belt, LegendaryItemTypes.SashofKnives),
				new ItemDataEntry(299381, PluginItemTypes.Belt, LegendaryItemTypes.SeborsNightmare),
				new ItemDataEntry(193669, PluginItemTypes.Belt, LegendaryItemTypes.StringofEars),
				new ItemDataEntry(193670, PluginItemTypes.Belt, LegendaryItemTypes.TheWitchingHour),
				new ItemDataEntry(212230, PluginItemTypes.Belt, LegendaryItemTypes.ThundergodsVigor),
				new ItemDataEntry(193665, PluginItemTypes.Belt, LegendaryItemTypes.VigilanteBelt),

				new ItemDataEntry(205621, PluginItemTypes.Boots, LegendaryItemTypes.BoardWalkers),
				new ItemDataEntry(197224, PluginItemTypes.Boots, LegendaryItemTypes.BojAnglers),
				new ItemDataEntry(322905, PluginItemTypes.Boots, LegendaryItemTypes.BootsofDisregard), //BootsofDisregard
				new ItemDataEntry(205624, PluginItemTypes.Boots, LegendaryItemTypes.FireWalkers),
				new ItemDataEntry(339125, PluginItemTypes.Boots, LegendaryItemTypes.IrontoeMudsputters),
				new ItemDataEntry(205622, PluginItemTypes.Boots, LegendaryItemTypes.LutSocks),
				new ItemDataEntry(205620, PluginItemTypes.Boots, LegendaryItemTypes.TheCrudestBoots),
				new ItemDataEntry(222464, PluginItemTypes.Boots, LegendaryItemTypes.IceClimbers),
				new ItemDataEntry(332342, PluginItemTypes.Boots, LegendaryItemTypes.IllusionaryBoots),

				new ItemDataEntry(298118, PluginItemTypes.Bracers, LegendaryItemTypes.ReapersWraps),
				new ItemDataEntry(298116, PluginItemTypes.Bracers, LegendaryItemTypes.AncientParthanDefenders),
				new ItemDataEntry(298122, PluginItemTypes.Bracers, LegendaryItemTypes.CusterianWristguards),
				new ItemDataEntry(193688, PluginItemTypes.Bracers, LegendaryItemTypes.GungdoGear),
				new ItemDataEntry(193683, PluginItemTypes.Bracers, LegendaryItemTypes.KethryesSplint),
				new ItemDataEntry(193687, PluginItemTypes.Bracers, LegendaryItemTypes.LacuniProwlers),
				new ItemDataEntry(298121, PluginItemTypes.Bracers, LegendaryItemTypes.NemesisBracers),
				new ItemDataEntry(193684, PluginItemTypes.Bracers, LegendaryItemTypes.PromiseofGlory),
				new ItemDataEntry(298120, PluginItemTypes.Bracers, LegendaryItemTypes.SanguinaryVambraces), //SanguinaryVambraces
				new ItemDataEntry(193685, PluginItemTypes.Bracers, LegendaryItemTypes.SlaveBonds),
				new ItemDataEntry(193686, PluginItemTypes.Bracers, LegendaryItemTypes.SteadyStrikers),
				new ItemDataEntry(193692, PluginItemTypes.Bracers, LegendaryItemTypes.StrongarmBracers),
				new ItemDataEntry(298119, PluginItemTypes.Bracers, LegendaryItemTypes.TragOulCoils),
				new ItemDataEntry(298115, PluginItemTypes.Bracers, LegendaryItemTypes.WarzechianArmguards),


				new ItemDataEntry(197203, PluginItemTypes.Chest, LegendaryItemTypes.AquilaCuirass),
				new ItemDataEntry(332202, PluginItemTypes.Chest, LegendaryItemTypes.ArmoroftheKindRegent),
				new ItemDataEntry(197204, PluginItemTypes.Chest, LegendaryItemTypes.Chaingmail),
				new ItemDataEntry(222455, PluginItemTypes.Chest, LegendaryItemTypes.Cindercoat),
				new ItemDataEntry(205616, PluginItemTypes.Chest, LegendaryItemTypes.Goldskin),
				new ItemDataEntry(205607, PluginItemTypes.Chest, LegendaryItemTypes.HeartofIron),
				new ItemDataEntry(205609, PluginItemTypes.Chest, LegendaryItemTypes.MantleoftheRydraelm),
				new ItemDataEntry(332200, PluginItemTypes.Chest, LegendaryItemTypes.ShiMizusHaori),
				new ItemDataEntry(205608, PluginItemTypes.Chest, LegendaryItemTypes.TyraelsMight),

				new ItemDataEntry(223150, PluginItemTypes.Cloak, LegendaryItemTypes.BeckonSail),
				new ItemDataEntry(332206, PluginItemTypes.Cloak, LegendaryItemTypes.Blackfeather),
				new ItemDataEntry(223149, PluginItemTypes.Cloak, LegendaryItemTypes.CapeoftheDarkNight),
				new ItemDataEntry(332208, PluginItemTypes.Cloak, LegendaryItemTypes.CloakofDeception), //CloakofDeception
				new ItemDataEntry(223151, PluginItemTypes.Cloak, LegendaryItemTypes.TheCloakoftheGarwulf),

				//new ItemDataEntry(341342, PluginItemTypes.Gloves, LegendaryItemTypes.None), //ForgeHarm
				//new ItemDataEntry(224051, PluginItemTypes.Gloves, LegendaryItemTypes.None), //ForgeHarm
				//new ItemDataEntry(197193, PluginItemTypes.Gloves, LegendaryItemTypes.None), //ForgeHarm
				new ItemDataEntry(197205, PluginItemTypes.Gloves, LegendaryItemTypes.Frostburn),
				new ItemDataEntry(205635, PluginItemTypes.Gloves, LegendaryItemTypes.GladiatorGauntlets),
				new ItemDataEntry(332344, PluginItemTypes.Gloves, LegendaryItemTypes.GlovesofWorship), //GlovesofWorship
				new ItemDataEntry(197206, PluginItemTypes.Gloves, LegendaryItemTypes.Magefist),
				new ItemDataEntry(197207, PluginItemTypes.Gloves, LegendaryItemTypes.PendersPurchase),
				new ItemDataEntry(332172, PluginItemTypes.Gloves, LegendaryItemTypes.StArchewsGage),
				new ItemDataEntry(205640, PluginItemTypes.Gloves, LegendaryItemTypes.StoneGauntlets),
				new ItemDataEntry(205642, PluginItemTypes.Gloves, LegendaryItemTypes.TaskerandTheo),

				new ItemDataEntry(198014, PluginItemTypes.Helm, LegendaryItemTypes.AndarielsVisage),
				new ItemDataEntry(197037, PluginItemTypes.Helm, LegendaryItemTypes.BlindFaith),
				new ItemDataEntry(220630, PluginItemTypes.Helm, LegendaryItemTypes.BrokenCrown),
				new ItemDataEntry(298146, PluginItemTypes.Helm, LegendaryItemTypes.DeathseersCowl),
				new ItemDataEntry(196024, PluginItemTypes.Helm, LegendaryItemTypes.LeoricsCrown),
				new ItemDataEntry(369016, PluginItemTypes.Helm, LegendaryItemTypes.MaskofJeram),
				new ItemDataEntry(223577, PluginItemTypes.Helm, LegendaryItemTypes.MempoofTwilight),
				new ItemDataEntry(298147, PluginItemTypes.Helm, LegendaryItemTypes.PridesFall), //PridesFall
				new ItemDataEntry(220549, PluginItemTypes.Helm, LegendaryItemTypes.SkullofResonance),

				new ItemDataEntry(193675, PluginItemTypes.MightyBelt, LegendaryItemTypes.AgelessMight),
				new ItemDataEntry(298133, PluginItemTypes.MightyBelt, LegendaryItemTypes.ChilaniksChain),
				new ItemDataEntry(193672, PluginItemTypes.MightyBelt, LegendaryItemTypes.DreadIron),
				new ItemDataEntry(212232, PluginItemTypes.MightyBelt, LegendaryItemTypes.GirdleofGiants),
				new ItemDataEntry(193674, PluginItemTypes.MightyBelt, LegendaryItemTypes.KotuursBrace),
				new ItemDataEntry(212234, PluginItemTypes.MightyBelt, LegendaryItemTypes.Lamentation),
				new ItemDataEntry(193673, PluginItemTypes.MightyBelt, LegendaryItemTypes.PrideofCassius),
				new ItemDataEntry(193676, PluginItemTypes.MightyBelt, LegendaryItemTypes.TheUndisputedChampion),

				new ItemDataEntry(332205, PluginItemTypes.Pants, LegendaryItemTypes.DeathsBargain), //DeathsBargain
				new ItemDataEntry(197216, PluginItemTypes.Pants, LegendaryItemTypes.DepthDiggers),
				new ItemDataEntry(209059, PluginItemTypes.Pants, LegendaryItemTypes.HammerJammers),
				new ItemDataEntry(332204, PluginItemTypes.Pants, LegendaryItemTypes.HexingPantsofMrYan),
				new ItemDataEntry(197220, PluginItemTypes.Pants, LegendaryItemTypes.PoxFaulds),
				new ItemDataEntry(209057, PluginItemTypes.Pants, LegendaryItemTypes.SwampLandWaders),

				new ItemDataEntry(200310, PluginItemTypes.Shoulders, LegendaryItemTypes.DeathWatchMantle),
				new ItemDataEntry(198573, PluginItemTypes.Shoulders, LegendaryItemTypes.HomingPads),
				new ItemDataEntry(298164, PluginItemTypes.Shoulders, LegendaryItemTypes.PauldronsoftheSkeletonKing), //PauldronsoftheSkeletonKing
				new ItemDataEntry(298158, PluginItemTypes.Shoulders, LegendaryItemTypes.ProfanePauldrons),
				new ItemDataEntry(298163, PluginItemTypes.Shoulders, LegendaryItemTypes.SpauldersofZakara),
				new ItemDataEntry(201325, PluginItemTypes.Shoulders, LegendaryItemTypes.VileWard),

				new ItemDataEntry(299464, PluginItemTypes.SpiritStone, LegendaryItemTypes.EyeofPeshkov),
				new ItemDataEntry(222169, PluginItemTypes.SpiritStone, LegendaryItemTypes.GyanaNaKashu),
				new ItemDataEntry(299461, PluginItemTypes.SpiritStone, LegendaryItemTypes.KekegisUnbreakableSpirit),
				new ItemDataEntry(222171, PluginItemTypes.SpiritStone, LegendaryItemTypes.SeeNoEvil),
				new ItemDataEntry(222170, PluginItemTypes.SpiritStone, LegendaryItemTypes.TheEyeoftheStorm),
				new ItemDataEntry(299454, PluginItemTypes.SpiritStone, LegendaryItemTypes.TheLawsofSeph),
				new ItemDataEntry(222172, PluginItemTypes.SpiritStone, LegendaryItemTypes.TheMindsEye),
				new ItemDataEntry(222305, PluginItemTypes.SpiritStone, LegendaryItemTypes.TzoKrinsGaze),

				new ItemDataEntry(299442, PluginItemTypes.VoodooMask, LegendaryItemTypes.Carnevil),
				new ItemDataEntry(299443, PluginItemTypes.VoodooMask, LegendaryItemTypes.MaskofJeram),
				new ItemDataEntry(204136, PluginItemTypes.VoodooMask, LegendaryItemTypes.Quetzalcoatl),
				new ItemDataEntry(221167, PluginItemTypes.VoodooMask, LegendaryItemTypes.SplitTusk),
				new ItemDataEntry(221166, PluginItemTypes.VoodooMask, LegendaryItemTypes.TheGrinReaper),
				new ItemDataEntry(221382, PluginItemTypes.VoodooMask, LegendaryItemTypes.TiklandianVisage),
				new ItemDataEntry(221168, PluginItemTypes.VoodooMask, LegendaryItemTypes.VisageofGiyua),

				new ItemDataEntry(299471, PluginItemTypes.WizardHat, LegendaryItemTypes.ArchmagesVicalyke),
				new ItemDataEntry(220694, PluginItemTypes.WizardHat, LegendaryItemTypes.StormCrow),
				new ItemDataEntry(325579, PluginItemTypes.WizardHat, LegendaryItemTypes.TheMagistrate),
				new ItemDataEntry(218681, PluginItemTypes.WizardHat, LegendaryItemTypes.TheSwami),
				new ItemDataEntry(299472, PluginItemTypes.WizardHat, LegendaryItemTypes.VelvetCamaral),


				
				new ItemDataEntry(116388, PluginItemTypes.Axe, LegendaryItemTypes.FleshTearer),
				new ItemDataEntry(116386, PluginItemTypes.Axe, LegendaryItemTypes.Genzaniku),
				new ItemDataEntry(271598, PluginItemTypes.Axe, LegendaryItemTypes.Hack),
				new ItemDataEntry(116389, PluginItemTypes.Axe, LegendaryItemTypes.SkySplitter),
				new ItemDataEntry(181484, PluginItemTypes.Axe, LegendaryItemTypes.TheBurningAxeofSankis),
				new ItemDataEntry(189973, PluginItemTypes.Axe, LegendaryItemTypes.TheButchersSickle),
				new ItemDataEntry(186560, PluginItemTypes.Axe, LegendaryItemTypes.TheExecutioner),
				new ItemDataEntry(116387, PluginItemTypes.Axe, LegendaryItemTypes.UtarsRoar),

				new ItemDataEntry(196250, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.AnessaziEdge),
				new ItemDataEntry(193433, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.DeadlyRebirth),
				new ItemDataEntry(195370, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.LastBreath),
				new ItemDataEntry(192540, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.LivingUmbralOath),
				new ItemDataEntry(271745, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.RhenhoFlayer),
				new ItemDataEntry(271738, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.StarmetalKukri),
				new ItemDataEntry(209246, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.TheGidbinn),
				new ItemDataEntry(222978, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.TheSpiderQueensGrasp),

				new ItemDataEntry(195655, PluginItemTypes.Dagger, LegendaryItemTypes.BloodMagicEdge), //BloodMagicEdge
				new ItemDataEntry(271732, PluginItemTypes.Dagger, LegendaryItemTypes.EnviousBlade), //EnviousBlade
				new ItemDataEntry(192579, PluginItemTypes.Dagger, LegendaryItemTypes.Kill),
				new ItemDataEntry(221313, PluginItemTypes.Dagger, LegendaryItemTypes.PigSticker),
				new ItemDataEntry(195174, PluginItemTypes.Dagger, LegendaryItemTypes.TheBarber),
				new ItemDataEntry(219329, PluginItemTypes.Dagger, LegendaryItemTypes.Wizardspike),

				new ItemDataEntry(175939, PluginItemTypes.FistWeapon, LegendaryItemTypes.CrystalFist),
				new ItemDataEntry(193459, PluginItemTypes.FistWeapon, LegendaryItemTypes.DemonClaw),
				new ItemDataEntry(145850, PluginItemTypes.FistWeapon, LegendaryItemTypes.Fleshrake),
				new ItemDataEntry(145849, PluginItemTypes.FistWeapon, LegendaryItemTypes.LogansClaw),
				new ItemDataEntry(196472, PluginItemTypes.FistWeapon, LegendaryItemTypes.RabidStrike),
				new ItemDataEntry(175938, PluginItemTypes.FistWeapon, LegendaryItemTypes.SledgeFist), //SledgeFist
				new ItemDataEntry(175937, PluginItemTypes.FistWeapon, LegendaryItemTypes.TheFistofAzTurrasq),

				new ItemDataEntry(299435, PluginItemTypes.Flail, LegendaryItemTypes.BalefulRemnant),
				new ItemDataEntry(299428, PluginItemTypes.Flail, LegendaryItemTypes.Darklight),
				new ItemDataEntry(299436, PluginItemTypes.Flail, LegendaryItemTypes.FateoftheFell),
				new ItemDataEntry(299437, PluginItemTypes.Flail, LegendaryItemTypes.GoldenFlense),
				new ItemDataEntry(299419, PluginItemTypes.Flail, LegendaryItemTypes.GoldenScourge),
				new ItemDataEntry(299429, PluginItemTypes.Flail, LegendaryItemTypes.InviolableFaith),
				new ItemDataEntry(299424, PluginItemTypes.Flail, LegendaryItemTypes.JustiniansMercy),
				new ItemDataEntry(299426, PluginItemTypes.Flail, LegendaryItemTypes.KassarsRetribution),
				new ItemDataEntry(299425, PluginItemTypes.Flail, LegendaryItemTypes.Swiftmount),
				new ItemDataEntry(299431, PluginItemTypes.Flail, LegendaryItemTypes.TheMortalDrama),
				new ItemDataEntry(299427, PluginItemTypes.Flail, LegendaryItemTypes.GyrfalconsFoote),

				new ItemDataEntry(192528, PluginItemTypes.HandCrossbow, LegendaryItemTypes.BalefireCaster),
				new ItemDataEntry(195078, PluginItemTypes.HandCrossbow, LegendaryItemTypes.Blitzbolter),
				new ItemDataEntry(196409, PluginItemTypes.HandCrossbow, LegendaryItemTypes.Dawn),
				new ItemDataEntry(271914, PluginItemTypes.HandCrossbow, LegendaryItemTypes.Helltrapper), //Helltrapper
				new ItemDataEntry(192467, PluginItemTypes.HandCrossbow, LegendaryItemTypes.Izzuccob),

				new ItemDataEntry(188177, PluginItemTypes.Mace, LegendaryItemTypes.Devastator),
				new ItemDataEntry(188181, PluginItemTypes.Mace, LegendaryItemTypes.EchoingFury),
				new ItemDataEntry(271648, PluginItemTypes.Mace, LegendaryItemTypes.JacesHammerofVigilance),
				new ItemDataEntry(271663, PluginItemTypes.Mace, LegendaryItemTypes.MadMonarchsScepter), //MadMonarchsScepter
				new ItemDataEntry(188158, PluginItemTypes.Mace, LegendaryItemTypes.Nailbiter),
				new ItemDataEntry(102665, PluginItemTypes.Mace, LegendaryItemTypes.Neanderthal),
				new ItemDataEntry(188169, PluginItemTypes.Mace, LegendaryItemTypes.Nutcracker),
				new ItemDataEntry(188185, PluginItemTypes.Mace, LegendaryItemTypes.OdynSon),
				new ItemDataEntry(271662, PluginItemTypes.Mace, LegendaryItemTypes.Solanium),
				new ItemDataEntry(188173, PluginItemTypes.Mace, LegendaryItemTypes.SunKeeper),
				//new ItemDataEntry(191584, PluginItemTypes.Mace, LegendaryItemTypes.None), //SupremacyNexus
				new ItemDataEntry(188189, PluginItemTypes.Mace, LegendaryItemTypes.TelrandensHand),

				new ItemDataEntry(193486, PluginItemTypes.MightyWeapon, LegendaryItemTypes.AmbosPride),
				new ItemDataEntry(193611, PluginItemTypes.MightyWeapon, LegendaryItemTypes.BladeoftheWarlord),
				new ItemDataEntry(192105, PluginItemTypes.MightyWeapon, LegendaryItemTypes.FjordCutter),
				new ItemDataEntry(192705, PluginItemTypes.MightyWeapon, LegendaryItemTypes.NightsReaping),

				new ItemDataEntry(191446, PluginItemTypes.Spear, LegendaryItemTypes.ArreatsLaw),
				new ItemDataEntry(194241, PluginItemTypes.Spear, LegendaryItemTypes.EmpyreanMessenger),
				new ItemDataEntry(197095, PluginItemTypes.Spear, LegendaryItemTypes.Scrimshaw),
				new ItemDataEntry(196638, PluginItemTypes.Spear, LegendaryItemTypes.TheThreeHundredthSpear),
				new ItemDataEntry(192511, PluginItemTypes.Sword, LegendaryItemTypes.Azurewrath),
				new ItemDataEntry(189552, PluginItemTypes.Sword, LegendaryItemTypes.DevilTongue),
				new ItemDataEntry(185397, PluginItemTypes.Sword, LegendaryItemTypes.Doombringer),
				new ItemDataEntry(271617, PluginItemTypes.Sword, LegendaryItemTypes.Exarian),
				new ItemDataEntry(271631, PluginItemTypes.Sword, LegendaryItemTypes.Fulminator),
				new ItemDataEntry(271630, PluginItemTypes.Sword, LegendaryItemTypes.GiftofSilaria),
				new ItemDataEntry(270977, PluginItemTypes.Sword, LegendaryItemTypes.GriswoldsPerfection),
				new ItemDataEntry(115140, PluginItemTypes.Sword, LegendaryItemTypes.MonsterHunter),
				new ItemDataEntry(271636, PluginItemTypes.Sword, LegendaryItemTypes.Rimeheart),
				new ItemDataEntry(115141, PluginItemTypes.Sword, LegendaryItemTypes.Sever),
				new ItemDataEntry(376463, PluginItemTypes.Sword, LegendaryItemTypes.ShardofHate),
				new ItemDataEntry(182347, PluginItemTypes.Sword, LegendaryItemTypes.Skycutter),
				new ItemDataEntry(194481, PluginItemTypes.Sword, LegendaryItemTypes.TheAncientBonesaberofZumakalis),
				new ItemDataEntry(270978, PluginItemTypes.Sword, LegendaryItemTypes.Wildwood),

				
				new ItemDataEntry(182081, PluginItemTypes.Wand, LegendaryItemTypes.Atrophy),
				new ItemDataEntry(193355, PluginItemTypes.Wand, LegendaryItemTypes.BlackhandKey),
				new ItemDataEntry(181995, PluginItemTypes.Wand, LegendaryItemTypes.FragmentofDestiny),
				new ItemDataEntry(182071, PluginItemTypes.Wand, LegendaryItemTypes.GestureofOrpheus),
				new ItemDataEntry(272084, PluginItemTypes.Wand, LegendaryItemTypes.SerpentSparker),
				new ItemDataEntry(181982, PluginItemTypes.Wand, LegendaryItemTypes.SloraksMadness),
				new ItemDataEntry(182074, PluginItemTypes.Wand, LegendaryItemTypes.Starfire),
				new ItemDataEntry(272086, PluginItemTypes.Wand, LegendaryItemTypes.WandofWoh),

				new ItemDataEntry(191065, PluginItemTypes.TwoHandAxe, LegendaryItemTypes.MesserschmidtsReaver),
				new ItemDataEntry(192887, PluginItemTypes.TwoHandAxe, LegendaryItemTypes.Skorn),
				new ItemDataEntry(186494, PluginItemTypes.TwoHandAxe, LegendaryItemTypes.ButchersCarver),
				new ItemDataEntry(6329, PluginItemTypes.TwoHandAxe, LegendaryItemTypes.CinderSwitch),

				new ItemDataEntry(175582, PluginItemTypes.TwoHandBow, LegendaryItemTypes.Cluckeye),
				new ItemDataEntry(221893, PluginItemTypes.TwoHandBow, LegendaryItemTypes.SydyruCrust),
				new ItemDataEntry(221938, PluginItemTypes.TwoHandBow, LegendaryItemTypes.TheRavensWing),
				new ItemDataEntry(220654, PluginItemTypes.TwoHandBow, LegendaryItemTypes.UnboundBolt),
				new ItemDataEntry(175580, PluginItemTypes.TwoHandBow, LegendaryItemTypes.Uskang),

				new ItemDataEntry(194957, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.ArcaneBarb),
				new ItemDataEntry(98163, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.BakkanCaster),
				new ItemDataEntry(194219, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.BurizaDoKyanon),
				new ItemDataEntry(222286, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.DemonMachine),
				new ItemDataEntry(192836, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.Hellrack),
				new ItemDataEntry(204874, PluginItemTypes.TwoHandCrossbow, LegendaryItemTypes.PusSpitter),

				new ItemDataEntry(195145, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.Balance),
				new ItemDataEntry(197065, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.FlyingDragon),
				new ItemDataEntry(192342, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.IncenseTorchoftheGrandTemple),
				new ItemDataEntry(209214, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.LaiYuisPersuader),
				new ItemDataEntry(196880, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.RozpedinsForce), //RozpedinsForce
				new ItemDataEntry(197072, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.TheFlowofEternity),
				new ItemDataEntry(197068, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.ThePaddle),

				new ItemDataEntry(59633, PluginItemTypes.TwoHandMace, LegendaryItemTypes.ArthefsSparkofLife),
				new ItemDataEntry(99227, PluginItemTypes.TwoHandMace, LegendaryItemTypes.Crushbane),
				new ItemDataEntry(271666, PluginItemTypes.TwoHandMace, LegendaryItemTypes.TheFurnace),
				new ItemDataEntry(191584, PluginItemTypes.TwoHandMace, LegendaryItemTypes.WrathoftheBoneKing),
				new ItemDataEntry(190868, PluginItemTypes.TwoHandMace, LegendaryItemTypes.Sunder),
				new ItemDataEntry(197717, PluginItemTypes.TwoHandMace, LegendaryItemTypes.SchaeferssHammer), //SchaefersHammer
				new ItemDataEntry(190840, PluginItemTypes.TwoHandMace, LegendaryItemTypes.Skywarden),
				new ItemDataEntry(190866, PluginItemTypes.TwoHandMace, LegendaryItemTypes.SledgeofAthskeleng),
				new ItemDataEntry(271671, PluginItemTypes.TwoHandMace, LegendaryItemTypes.Soulsmasher), //Soulsmasher

				new ItemDataEntry(195690, PluginItemTypes.TwoHandMighty, LegendaryItemTypes.BastionsRevered),
				new ItemDataEntry(193657, PluginItemTypes.TwoHandMighty, LegendaryItemTypes.TheGavelofJudgment),
				new ItemDataEntry(196308, PluginItemTypes.TwoHandMighty, LegendaryItemTypes.WaroftheDead),
				new ItemDataEntry(272012, PluginItemTypes.TwoHandMighty, LegendaryItemTypes.MadawcsSorrow),

				new ItemDataEntry(272056, PluginItemTypes.TwoHandPolearm, LegendaryItemTypes.BovineBardiche),
				new ItemDataEntry(192569, PluginItemTypes.TwoHandPolearm, LegendaryItemTypes.HeartSlaughter),
				new ItemDataEntry(196570, PluginItemTypes.TwoHandPolearm, LegendaryItemTypes.PledgeofCaldeum),
				new ItemDataEntry(191570, PluginItemTypes.TwoHandPolearm, LegendaryItemTypes.Standoff),
				new ItemDataEntry(195491, PluginItemTypes.TwoHandPolearm, LegendaryItemTypes.Vigilance),

				new ItemDataEntry(184228, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.AutumnsCall),
				new ItemDataEntry(193832, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.MalothsFocus),
				new ItemDataEntry(59612, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.MarkofTheMagi), //MarkofTheMagi
				new ItemDataEntry(59601, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.TheBrokenStaff),
				new ItemDataEntry(192167, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.TheGrandVizier),
				new ItemDataEntry(193066, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.TheTormentor),
				new ItemDataEntry(271773, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.ValtheksRebuke),
				new ItemDataEntry(195407, PluginItemTypes.TwoHandStaff, LegendaryItemTypes.Wormwood),

				new ItemDataEntry(198960, PluginItemTypes.TwoHandSword, LegendaryItemTypes.FaithfulMemory),
				new ItemDataEntry(184187, PluginItemTypes.TwoHandSword, LegendaryItemTypes.Maximus),
				new ItemDataEntry(181511, PluginItemTypes.TwoHandSword, LegendaryItemTypes.Scourge),
				new ItemDataEntry(271639, PluginItemTypes.TwoHandSword, LegendaryItemTypes.StalgardsDecimator),
				new ItemDataEntry(190360, PluginItemTypes.TwoHandSword, LegendaryItemTypes.TheGrandfather),
				new ItemDataEntry(184190, PluginItemTypes.TwoHandSword, LegendaryItemTypes.TheSultanofBlindingSand),
				new ItemDataEntry(59665, PluginItemTypes.TwoHandSword, LegendaryItemTypes.TheZweihander),
				new ItemDataEntry(181495, PluginItemTypes.TwoHandSword, LegendaryItemTypes.Warmonger),
				new ItemDataEntry(184184, PluginItemTypes.TwoHandSword, LegendaryItemTypes.BladeofProphecy),
				new ItemDataEntry(271644, PluginItemTypes.TwoHandSword, LegendaryItemTypes.CamsRebuttal),

				

				new ItemDataEntry(299413, PluginItemTypes.CrusaderShield, LegendaryItemTypes.HallowedBulwark),
				new ItemDataEntry(299415, PluginItemTypes.CrusaderShield, LegendaryItemTypes.Hellskull),
				new ItemDataEntry(299412, PluginItemTypes.CrusaderShield, LegendaryItemTypes.Jekangbord),
				new ItemDataEntry(299418, PluginItemTypes.CrusaderShield, LegendaryItemTypes.Salvation), //Salvation
				new ItemDataEntry(299416, PluginItemTypes.CrusaderShield, LegendaryItemTypes.SublimeConviction),
				new ItemDataEntry(299417, PluginItemTypes.CrusaderShield, LegendaryItemTypes.TheFinalWitness),
				new ItemDataEntry(299411, PluginItemTypes.CrusaderShield, LegendaryItemTypes.PiroMarella),

				new ItemDataEntry(194995, PluginItemTypes.Mojo, LegendaryItemTypes.GazingDemise),
				new ItemDataEntry(194991, PluginItemTypes.Mojo, LegendaryItemTypes.Homunculus),
				new ItemDataEntry(272070, PluginItemTypes.Mojo, LegendaryItemTypes.ShukranisTriumph),
				new ItemDataEntry(194988, PluginItemTypes.Mojo, LegendaryItemTypes.Spite),
				new ItemDataEntry(192468, PluginItemTypes.Mojo, LegendaryItemTypes.ThingoftheDeep),
				new ItemDataEntry(191278, PluginItemTypes.Mojo, LegendaryItemTypes.UhkapianSerpent),

				new ItemDataEntry(197626, PluginItemTypes.Quiver, LegendaryItemTypes.ArchfiendArrows),
				new ItemDataEntry(298171, PluginItemTypes.Quiver, LegendaryItemTypes.BombadiersRucksack),
				new ItemDataEntry(197630, PluginItemTypes.Quiver, LegendaryItemTypes.DeadMansLegacy),
				new ItemDataEntry(298172, PluginItemTypes.Quiver, LegendaryItemTypes.EmimeisDuffel),
				new ItemDataEntry(197629, PluginItemTypes.Quiver, LegendaryItemTypes.FletchersPride),
				new ItemDataEntry(197628, PluginItemTypes.Quiver, LegendaryItemTypes.SilverStarPiercers),
				new ItemDataEntry(197625, PluginItemTypes.Quiver, LegendaryItemTypes.SinSeekers),
				new ItemDataEntry(298170, PluginItemTypes.Quiver, LegendaryItemTypes.TheNinthCirriSatchel),

				new ItemDataEntry(298191, PluginItemTypes.Shield, LegendaryItemTypes.CovensCriterion), //CovensCriterion
				new ItemDataEntry(298182, PluginItemTypes.Shield, LegendaryItemTypes.DefenderofWestmarch),
				new ItemDataEntry(152666, PluginItemTypes.Shield, LegendaryItemTypes.Denial),
				new ItemDataEntry(298186, PluginItemTypes.Shield, LegendaryItemTypes.EberliCharo),
				new ItemDataEntry(61550, PluginItemTypes.Shield, LegendaryItemTypes.FreezeofDeflection),
				new ItemDataEntry(197478, PluginItemTypes.Shield, LegendaryItemTypes.IvoryTower),
				new ItemDataEntry(195389, PluginItemTypes.Shield, LegendaryItemTypes.LidlessWall),
				new ItemDataEntry(192484, PluginItemTypes.Shield, LegendaryItemTypes.Stormshield),
				new ItemDataEntry(298188, PluginItemTypes.Shield, LegendaryItemTypes.VoToyiasSpiker),

				new ItemDataEntry(195127, PluginItemTypes.Source, LegendaryItemTypes.CosmicStrand),
				new ItemDataEntry(272038, PluginItemTypes.Source, LegendaryItemTypes.LightofGrace),
				new ItemDataEntry(272022, PluginItemTypes.Source, LegendaryItemTypes.Mirrorball),
				new ItemDataEntry(272037, PluginItemTypes.Source, LegendaryItemTypes.MykensBallofHate),
				new ItemDataEntry(192320, PluginItemTypes.Source, LegendaryItemTypes.TheOculus),
				new ItemDataEntry(195325, PluginItemTypes.Source, LegendaryItemTypes.Triumvirate),
				new ItemDataEntry(184199, PluginItemTypes.Source, LegendaryItemTypes.WinterFlurry),
				
				

				//BLACKTHORNE'S
				new ItemDataEntry(222477, PluginItemTypes.Pants, LegendaryItemTypes.BlackthornesBattlegear), //Jousting Mail
				new ItemDataEntry(224191, PluginItemTypes.Belt, LegendaryItemTypes.BlackthornesBattlegear), //Notched Belt
				new ItemDataEntry(222456, PluginItemTypes.Chest, LegendaryItemTypes.BlackthornesBattlegear), //Surcoat
				new ItemDataEntry(222463, PluginItemTypes.Boots, LegendaryItemTypes.BlackthornesBattlegear), //Spurs
				new ItemDataEntry(224189, PluginItemTypes.Amulet, LegendaryItemTypes.BlackthornesBattlegear), //Duncraig Cross

				//Zunimassa's
				new ItemDataEntry(205615, PluginItemTypes.Chest, LegendaryItemTypes.ZunimassasHaunt), //Marrow
				new ItemDataEntry(216525, PluginItemTypes.Mojo, LegendaryItemTypes.ZunimassasHaunt), //String of Skulls
				new ItemDataEntry(221202, PluginItemTypes.Helm, LegendaryItemTypes.ZunimassasHaunt), //Vision
				new ItemDataEntry(205627, PluginItemTypes.Boots, LegendaryItemTypes.ZunimassasHaunt), //Trail
				new ItemDataEntry(212579, PluginItemTypes.Ring, LegendaryItemTypes.ZunimassasHaunt), //Pox
			

				//Akkhan's
				new ItemDataEntry(358799, PluginItemTypes.Helm, LegendaryItemTypes.ArmorofAkkhan),//Helm
				new ItemDataEntry(358795, PluginItemTypes.Boots, LegendaryItemTypes.ArmorofAkkhan),//Sabatons
				new ItemDataEntry(358801, PluginItemTypes.Shoulders, LegendaryItemTypes.ArmorofAkkhan),//Pauldrons
				new ItemDataEntry(358800, PluginItemTypes.Pants, LegendaryItemTypes.ArmorofAkkhan),//Cuisses
				new ItemDataEntry(358796, PluginItemTypes.Chest, LegendaryItemTypes.ArmorofAkkhan),//Breastplate
				new ItemDataEntry(358798, PluginItemTypes.Gloves, LegendaryItemTypes.ArmorofAkkhan),//Gauntlets

				//Raiment of a ThousandStorms
				new ItemDataEntry(338035, PluginItemTypes.Pants, LegendaryItemTypes.RaimentofaThousandStorms),//Pants
				new ItemDataEntry(338031, PluginItemTypes.Boots, LegendaryItemTypes.RaimentofaThousandStorms),//Boots
				new ItemDataEntry(338036, PluginItemTypes.Shoulders, LegendaryItemTypes.RaimentofaThousandStorms),//Shoulders
				new ItemDataEntry(338034, PluginItemTypes.Helm, LegendaryItemTypes.RaimentofaThousandStorms),//Helm
				new ItemDataEntry(338032, PluginItemTypes.Chest, LegendaryItemTypes.RaimentofaThousandStorms),//Chest
				new ItemDataEntry(338033, PluginItemTypes.Gloves, LegendaryItemTypes.RaimentofaThousandStorms),//Gloves

				//Inna's Mantra
				new ItemDataEntry(205646, PluginItemTypes.Pants, LegendaryItemTypes.InnasMantra),//Pants
				new ItemDataEntry(222487, PluginItemTypes.Belt, LegendaryItemTypes.InnasMantra),//Belt
				new ItemDataEntry(222307, PluginItemTypes.Helm, LegendaryItemTypes.InnasMantra),//Helm
				new ItemDataEntry(205614, PluginItemTypes.Chest, LegendaryItemTypes.InnasMantra),//Chest
				new ItemDataEntry(212208, PluginItemTypes.TwoHandDaibo, LegendaryItemTypes.InnasMantra),//Daibo


				//Raiment of the Jade Harvester
				new ItemDataEntry(338041, PluginItemTypes.Pants, LegendaryItemTypes.RaimentoftheJadeHarvester),//Pants
				new ItemDataEntry(338037, PluginItemTypes.Boots, LegendaryItemTypes.RaimentoftheJadeHarvester),//Boots
				new ItemDataEntry(338042, PluginItemTypes.Shoulders, LegendaryItemTypes.RaimentoftheJadeHarvester),//Shoulders
				new ItemDataEntry(338040, PluginItemTypes.Helm, LegendaryItemTypes.RaimentoftheJadeHarvester),//Helm
				new ItemDataEntry(338038, PluginItemTypes.Chest, LegendaryItemTypes.RaimentoftheJadeHarvester),//Chest
				new ItemDataEntry(338039, PluginItemTypes.Gloves, LegendaryItemTypes.RaimentoftheJadeHarvester),//Gloves

				//Vyr's Amazing Arcana
				new ItemDataEntry(332360, PluginItemTypes.Pants, LegendaryItemTypes.VyrsAmazingArcana),//Pants
				new ItemDataEntry(346210, PluginItemTypes.Gloves, LegendaryItemTypes.VyrsAmazingArcana),//Gloves
				new ItemDataEntry(332363, PluginItemTypes.Boots, LegendaryItemTypes.VyrsAmazingArcana),//Boots
				new ItemDataEntry(332357, PluginItemTypes.Chest, LegendaryItemTypes.VyrsAmazingArcana),//Chest
			
				//Might of the Earth
				new ItemDataEntry(340521, PluginItemTypes.Pants, LegendaryItemTypes.MightOfTheEarth),//Pants
				new ItemDataEntry(340523, PluginItemTypes.Gloves, LegendaryItemTypes.MightOfTheEarth),//Gloves
				new ItemDataEntry(340528, PluginItemTypes.Helm, LegendaryItemTypes.MightOfTheEarth),//Helm
				new ItemDataEntry(340526, PluginItemTypes.Shoulders, LegendaryItemTypes.MightOfTheEarth),//Shoulders

				//Embodiment of the Marauder
				new ItemDataEntry(336993, PluginItemTypes.Pants, LegendaryItemTypes.EmbodimentoftheMarauder),//Pants
				new ItemDataEntry(336995, PluginItemTypes.Boots, LegendaryItemTypes.EmbodimentoftheMarauder),//Boots
				new ItemDataEntry(336996, PluginItemTypes.Shoulders, LegendaryItemTypes.EmbodimentoftheMarauder),//Shoulders
				new ItemDataEntry(336994, PluginItemTypes.Helm, LegendaryItemTypes.EmbodimentoftheMarauder),//Helm
				new ItemDataEntry(363803, PluginItemTypes.Chest, LegendaryItemTypes.EmbodimentoftheMarauder),//Chest
				new ItemDataEntry(336992, PluginItemTypes.Gloves, LegendaryItemTypes.EmbodimentoftheMarauder),//Gloves

				//Helltooth Harness
				new ItemDataEntry(369016, PluginItemTypes.Helm, LegendaryItemTypes.HelltoothHarness),
				new ItemDataEntry(340524, PluginItemTypes.Boots, LegendaryItemTypes.HelltoothHarness), //HelltoothGreaves
				new ItemDataEntry(363088, PluginItemTypes.Chest, LegendaryItemTypes.HelltoothHarness), //HelltoothTunic
				new ItemDataEntry(363094, PluginItemTypes.Gloves, LegendaryItemTypes.HelltoothHarness), //HelltoothGauntlets
				new ItemDataEntry(340522, PluginItemTypes.Pants, LegendaryItemTypes.HelltoothHarness), //HelltoothLegGuards
				new ItemDataEntry(340525, PluginItemTypes.Shoulders, LegendaryItemTypes.HelltoothHarness), //HelltoothMantle

				//Immortal Kings
				new ItemDataEntry(205625, PluginItemTypes.Boots, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsStride
				new ItemDataEntry(205613, PluginItemTypes.Chest, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsEternalReign
				new ItemDataEntry(205631, PluginItemTypes.Gloves, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsIrons
				new ItemDataEntry(210265, PluginItemTypes.Helm, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsTriumph
				new ItemDataEntry(212235, PluginItemTypes.MightyBelt, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsTribalBinding
				new ItemDataEntry(210678, PluginItemTypes.MightyWeapon, LegendaryItemTypes.ImmortalKingsCall), //ImmortalKingsBoulderBreaker

				//ManajumasWay
				new ItemDataEntry(223365, PluginItemTypes.CeremonialKnife, LegendaryItemTypes.ManajumasWay), //ManajumasCarvingKnife
				new ItemDataEntry(210993, PluginItemTypes.Mojo, LegendaryItemTypes.ManajumasWay), //ManajumasGoryFetch

				//Captain Crimsons Trimmings
				new ItemDataEntry(197221, PluginItemTypes.Boots, LegendaryItemTypes.CaptainCrimsonsTrimmings), //CaptainCrimsonsWaders
				new ItemDataEntry(197214, PluginItemTypes.Pants, LegendaryItemTypes.CaptainCrimsonsTrimmings), //CaptainCrimsonsThrust
				new ItemDataEntry(222974, PluginItemTypes.Belt, LegendaryItemTypes.CaptainCrimsonsTrimmings), //CaptainCrimsonsSilkGirdle
				
				//EndlessWalk
				new ItemDataEntry(222490, PluginItemTypes.Amulet, LegendaryItemTypes.EndlessWalk), //TheTravelersPledge
				new ItemDataEntry(212587, PluginItemTypes.Ring, LegendaryItemTypes.EndlessWalk), //TheCompassRose

				//BastionsofWill
				new ItemDataEntry(332210, PluginItemTypes.Ring, LegendaryItemTypes.BastionsofWill), //Restraint
				new ItemDataEntry(332209, PluginItemTypes.Ring, LegendaryItemTypes.BastionsofWill), //Focus

				//LegacyofNightmares
				new ItemDataEntry(212651, PluginItemTypes.Ring, LegendaryItemTypes.LegacyofNightmares), //LitanyoftheUndaunted
				new ItemDataEntry(212650, PluginItemTypes.Ring, LegendaryItemTypes.LegacyofNightmares), //TheWailingHost

				

				//Sunwukos
				new ItemDataEntry(336174, PluginItemTypes.Amulet, LegendaryItemTypes.MonkeyKingsGarb), //SunwukosShines
				new ItemDataEntry(336172, PluginItemTypes.Gloves, LegendaryItemTypes.MonkeyKingsGarb), //SunwukosPaws
				new ItemDataEntry(336173, PluginItemTypes.Helm, LegendaryItemTypes.MonkeyKingsGarb), //SunwukosCrown
				new ItemDataEntry(336175, PluginItemTypes.Shoulders, LegendaryItemTypes.MonkeyKingsGarb), //SunwukosBalance

				//ThornsoftheInvoker
				new ItemDataEntry(335030, PluginItemTypes.Bracers, LegendaryItemTypes.ThornsoftheInvoker), //ShacklesoftheInvoker
				new ItemDataEntry(335027, PluginItemTypes.Gloves, LegendaryItemTypes.ThornsoftheInvoker), //PrideoftheInvoker
				new ItemDataEntry(335028, PluginItemTypes.Helm, LegendaryItemTypes.ThornsoftheInvoker), //CrownoftheInvoker

				//Ashearas Vestments
				new ItemDataEntry(205618, PluginItemTypes.Boots, LegendaryItemTypes.AshearasVestments), //AshearasFinders
				new ItemDataEntry(205636, PluginItemTypes.Gloves, LegendaryItemTypes.AshearasVestments), //AshearasWard
				new ItemDataEntry(209054, PluginItemTypes.Pants, LegendaryItemTypes.AshearasVestments), //AshearasPace
				new ItemDataEntry(225132, PluginItemTypes.Shoulders, LegendaryItemTypes.AshearasVestments), //AshearasCustodian

				//DemonsHide
				new ItemDataEntry(222741, PluginItemTypes.Bracers, LegendaryItemTypes.DemonsHide), //DemonsAnimus
				new ItemDataEntry(205612, PluginItemTypes.Chest, LegendaryItemTypes.DemonsHide), //DemonsMarrow
				new ItemDataEntry(224397, PluginItemTypes.Shoulders, LegendaryItemTypes.DemonsHide), //DemonsAileron
				new ItemDataEntry(222740, PluginItemTypes.Belt, LegendaryItemTypes.DemonsHide), //DemonsRestraint

				//Cain sDestiny
				new ItemDataEntry(197210, PluginItemTypes.Gloves, LegendaryItemTypes.CainsDestiny), //CainsScribe
				new ItemDataEntry(197218, PluginItemTypes.Pants, LegendaryItemTypes.CainsDestiny), //CainsHabit
				new ItemDataEntry(197225, PluginItemTypes.Boots, LegendaryItemTypes.CainsDestiny), //CainsTravelers
				
				//BornsDefiance
				new ItemDataEntry(222948, PluginItemTypes.Shoulders, LegendaryItemTypes.BornsDefiance), //BornsPrivilege
				new ItemDataEntry(197199, PluginItemTypes.Chest, LegendaryItemTypes.BornsDefiance), //BornsFrozenSoul

				//AughildsAuthority
				new ItemDataEntry(222972, PluginItemTypes.Bracers, LegendaryItemTypes.AughildsAuthority), //AughildsSearch
				new ItemDataEntry(197193, PluginItemTypes.Chest, LegendaryItemTypes.AughildsAuthority), //AughildsRule
				new ItemDataEntry(224051, PluginItemTypes.Shoulders, LegendaryItemTypes.AughildsAuthority), //AughildsPower

				//Tal Rashas
				new ItemDataEntry(222486, PluginItemTypes.Amulet, LegendaryItemTypes.TalRashasElements), //TalRashasAllegiance
				new ItemDataEntry(212657, PluginItemTypes.Belt, LegendaryItemTypes.TalRashasElements), //TalRashasBrace
				new ItemDataEntry(211626, PluginItemTypes.Chest, LegendaryItemTypes.TalRashasElements), //TalRashasRelentlessPursuit
				new ItemDataEntry(211531, PluginItemTypes.Helm, LegendaryItemTypes.TalRashasElements), //TalRashasGuiseofWisdom
				new ItemDataEntry(212780, PluginItemTypes.Source, LegendaryItemTypes.TalRashasElements), //TalRashasUnwaveringGlare

				//Firebirds Finery
				new ItemDataEntry(358793, PluginItemTypes.Boots, LegendaryItemTypes.FirebirdsFinery), //FirebirdsTarsi
				new ItemDataEntry(358788, PluginItemTypes.Chest, LegendaryItemTypes.FirebirdsFinery), //FirebirdsBreast
				new ItemDataEntry(358789, PluginItemTypes.Gloves, LegendaryItemTypes.FirebirdsFinery), //FirebirdsTalons
				new ItemDataEntry(358791, PluginItemTypes.Helm, LegendaryItemTypes.FirebirdsFinery), //FirebirdsPlume
				new ItemDataEntry(358790, PluginItemTypes.Pants, LegendaryItemTypes.FirebirdsFinery), //FirebirdsDown
				new ItemDataEntry(358819, PluginItemTypes.Source, LegendaryItemTypes.FirebirdsFinery), //FirebirdsEye
				new ItemDataEntry(358792, PluginItemTypes.Shoulders, LegendaryItemTypes.FirebirdsFinery), //FirebirdsPinions

				//NatalyasVengeance
				new ItemDataEntry(197223, PluginItemTypes.Boots, LegendaryItemTypes.NatalyasVengeance), //NatalyasBloodyFootprints
				new ItemDataEntry(208934, PluginItemTypes.Cloak, LegendaryItemTypes.NatalyasVengeance), //NatalyasEmbrace
				new ItemDataEntry(210851, PluginItemTypes.Helm, LegendaryItemTypes.NatalyasVengeance), //NatalyasSight
				new ItemDataEntry(212545, PluginItemTypes.Ring, LegendaryItemTypes.NatalyasVengeance), //NatalyasReflection

				//TheShadowsMantle
				new ItemDataEntry(332364, PluginItemTypes.Boots, LegendaryItemTypes.TheShadowsMantle), //TheShadowsHeels
				new ItemDataEntry(332359, PluginItemTypes.Chest, LegendaryItemTypes.TheShadowsMantle), //TheShadowsBane
				new ItemDataEntry(332361, PluginItemTypes.Pants, LegendaryItemTypes.TheShadowsMantle), //TheShadowsCoil

				//TheLegacyofRaekor
				new ItemDataEntry(336987, PluginItemTypes.Boots, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsStriders
				new ItemDataEntry(336984, PluginItemTypes.Chest, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsHeart
				new ItemDataEntry(336985, PluginItemTypes.Gloves, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsWraps
				new ItemDataEntry(336988, PluginItemTypes.Helm, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsWill
				new ItemDataEntry(336986, PluginItemTypes.Pants, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsBreeches
				new ItemDataEntry(336989, PluginItemTypes.Shoulders, LegendaryItemTypes.TheLegacyofRaekor), //RaekorsBurden

				//ShenlongsSpirit
				new ItemDataEntry(208996, PluginItemTypes.FistWeapon, LegendaryItemTypes.ShenlongsSpirit), //ShenlongsFistofLegend
				new ItemDataEntry(208898, PluginItemTypes.FistWeapon, LegendaryItemTypes.ShenlongsSpirit), //ShenlongsRelentlessAssault

				//BulKathossOath
				new ItemDataEntry(208771, PluginItemTypes.MightyWeapon, LegendaryItemTypes.BulKathossOath), //BulKathossSolemnVow
				new ItemDataEntry(208775, PluginItemTypes.MightyWeapon, LegendaryItemTypes.BulKathossOath), //BulKathossWarriorBlood

				//ChantodosResolve
				new ItemDataEntry(212277, PluginItemTypes.Source, LegendaryItemTypes.ChantodosResolve), //ChantodosForce
				new ItemDataEntry(210479, PluginItemTypes.Wand, LegendaryItemTypes.ChantodosResolve), //ChantodosWill
			
				//Istvans Paired Blades
				new ItemDataEntry(313290, PluginItemTypes.Sword, LegendaryItemTypes.IstvansPairedBlades), //TheSlanderer
				new ItemDataEntry(313291, PluginItemTypes.Sword, LegendaryItemTypes.IstvansPairedBlades), //LittleRogue

			};
			#endregion

		}

		private static readonly string DefaultFilePath = Path.Combine(FolderPaths.PluginPath, "Cache", "External", "Dictionaries", "Cache_Items.xml");
		internal static ItemDataCollection DeserializeFromXML()
		{
			var deserializer = new XmlSerializer(typeof(ItemDataCollection));
			TextReader textReader = new StreamReader(DefaultFilePath);
			var settings = (ItemDataCollection)deserializer.Deserialize(textReader);
			textReader.Close();
			return settings;
		}
		internal static void SerializeToXML(ItemDataCollection settings)
		{
			var serializer = new XmlSerializer(typeof(ItemDataCollection));
			var textWriter = new StreamWriter(DefaultFilePath);
			serializer.Serialize(textWriter, settings);
			textWriter.Close();
		}
	}
}