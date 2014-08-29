﻿using System.Linq;
using fBaseXtensions.Behaviors;
using fBaseXtensions.Cache.Internal;
using fBaseXtensions.Cache.Internal.Objects;
using fBaseXtensions.Settings;
using Zeta.Bot.Profile.Common;
using Zeta.Common;
using Zeta.Game;
using Logger = fBaseXtensions.Helpers.Logger;
using LogLevel = fBaseXtensions.Helpers.LogLevel;

namespace fBaseXtensions.Game
{
	//Tracks Stats, Profile related properties, and general in-game info
	public class GameCache
	{
		public GameCache()
		{
			QuestMode = false;
			ShouldNavigateMinimapPoints = false;
			AllowAnyUnitForLOSMovement = false;
		}

		internal bool QuestMode { get; set; }
		internal bool ShouldNavigateMinimapPoints { get; set; }
		internal bool AllowAnyUnitForLOSMovement { get; set; }
		public void ResetCombatModifiers()
		{
			SettingCluster.ClusterSettingsTag = FunkyBaseExtension.Settings.Cluster;
			SettingLOSMovement.LOSSettingsTag = FunkyBaseExtension.Settings.LOSMovement;
			QuestMode = false;
			AllowAnyUnitForLOSMovement = false;
			ShouldNavigateMinimapPoints = false;
		}
		public bool ShouldNavigatePointsOfInterest
		{
			get
			{

				return FunkyGame.AdventureMode && (FunkyBaseExtension.Settings.AdventureMode.NavigatePointsOfInterest || ShouldNavigateMinimapPoints) && FunkyGame.Profile.CurrentProfileBehaviorType == Profile.ProfileBehaviorTypes.ExploreDungeon;
			}
		}


		internal static void GoldInactivityTimerTrippedHandler()
		{
			Logger.DBLog.Info("[Funky] Gold Timeout Breached.. enabling exit behavior!");
			ExitGame.ShouldExitGame = true;
		}

		
		internal CacheObject InteractableCachedObject { get; set; }
		internal void OnProfileBehaviorChanged(Profile.ProfileBehaviorTypes type)
		{
			if (type == Profile.ProfileBehaviorTypes.Unknown)
			{
				InteractableCachedObject = null;

				return;
			}

			if (type == Profile.ProfileBehaviorTypes.SetQuestMode)
				QuestMode = true;
			else if(type.HasFlag(Profile.ProfileBehaviorTypes.Interactive))
			{
				Logger.DBLog.DebugFormat("Interactable Profile Tag!");

				InteractableCachedObject = GetInteractiveCachedObject(type);
				if (InteractableCachedObject != null)
					Logger.DBLog.DebugFormat("Found Cached Interactable Server Object");
			}
		}

		internal CacheObject GetInteractiveCachedObject(Profile.ProfileBehaviorTypes type)
		{

			if (type == Profile.ProfileBehaviorTypes.UseWaypoint)
			{
				UseWaypointTag tagWP = (UseWaypointTag)FunkyGame.Profile.CurrentProfileBehavior;
				var WaypointObjects = ObjectCache.InteractableObjectCache.Values.Where(obj => obj.SNOID == 6442);
				foreach (CacheObject item in WaypointObjects)
				{
					if (item.Position.Distance(tagWP.Position) < 100f)
					{
						//Found matching waypoint object!
						return item;
					}
				}
			}
			else if (type == Profile.ProfileBehaviorTypes.UseObject)
			{
				UseObjectTag tagUseObj = (UseObjectTag)FunkyGame.Profile.CurrentProfileBehavior;
				if (tagUseObj.ActorId > 0)
				{//Using SNOID..
					var Objects = ObjectCache.InteractableObjectCache.Values.Where(obj => obj.SNOID == tagUseObj.ActorId);
					foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(FunkyGame.Hero.Position)))
					{
						//Found matching object!
						return item;
					}

				}
				else
				{//use position to match object
					Vector3 tagPosition = tagUseObj.Position;
					var Objects = ObjectCache.InteractableObjectCache.Values.Where(obj => obj.Position.Distance(tagPosition) <= 100f);
					foreach (CacheObject item in Objects)
					{
						//Found matching object!
						return item;
					}
				}
			}
			else if (type == Profile.ProfileBehaviorTypes.UsePortal)
			{
				UsePortalTag tagUsePortal = (UsePortalTag)FunkyGame.Profile.CurrentProfileBehavior;
				if (tagUsePortal.ActorId > 0)
				{//Using SNOID..
					var Objects = ObjectCache.InteractableObjectCache.Values.Where(obj => obj.SNOID == tagUsePortal.ActorId);
					foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(FunkyGame.Hero.Position)))
					{
						//Found matching object!
						return item;
					}

				}
				else
				{//use position to match object
					Vector3 tagPosition = tagUsePortal.Position;
					var Objects = ObjectCache.InteractableObjectCache.Values.Where(obj => obj.Position.Distance(tagPosition) <= 100f);
					foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(FunkyGame.Hero.Position)))
					{
						//Found matching object!
						return item;
					}
				}
			}


			return null;
		}


		internal void OnGameIDChangedHandler()
		{
			Logger.Write(LogLevel.OutOfCombat, "New Game Started");


			if (FunkyGame.AdventureMode && SettingAdventureMode.AdventureModeSettingsTag.EnableAdventuringMode)
			{
				ResetCombatModifiers();
			}


			//Clear Interactable Cache
			ObjectCache.InteractableObjectCache.Clear();

			//Clear Health Average
			ObjectCache.Objects.ClearHealthAverageStats();

			//Renew bot
			FunkyGame.ResetBot();


		}

		public static SNOLevelArea GetSNOLevelAreaByWaypointID(int ID)
		{
			switch (ID)
			{
				case 0: return SNOLevelArea.A1_Tristram_Adventure_Mode_Hub;
				case 1: return SNOLevelArea.A1_trOut_Old_Tristram_Road_Cath;
					
				case 2: return SNOLevelArea.A1_trDun_Level01;
				
				case 3: return SNOLevelArea.A1_trDun_Level04;
		
				case 4: return SNOLevelArea.A1_trDun_Level06;
				
				case 5: return SNOLevelArea.A1_trDun_Level07B;
				
				case 6: return SNOLevelArea.A1_trOut_TristramWilderness;
				
				case 7: return SNOLevelArea.A1_trOut_Wilderness_BurialGrounds;
				
				case 9: return SNOLevelArea.A1_trOut_TristramFields_A;
					
				case 10: return SNOLevelArea.A1_trOut_TristramFields_B;
				
				case 11: return SNOLevelArea.A1_trOut_TownAttack_ChapelCellar;
				
				case 12: return SNOLevelArea.A1_C6_SpiderCave_01_Main;
					
				case 13: return SNOLevelArea.A1_trOUT_Highlands_Bridge;
					
				case 14: return SNOLevelArea.A1_trOUT_Highlands2;
				
				case 15: return SNOLevelArea.A1_trDun_Leoric01;
					
				case 16: return SNOLevelArea.A1_trDun_Leoric02;
					
				case 17: return SNOLevelArea.A1_trDun_Leoric03;

				case 18: return SNOLevelArea.A2_caOut_CT_RefugeeCamp_Hub;
				case 19: return SNOLevelArea.A2_caOUT_StingingWinds_Canyon;
		
				case 20: return SNOLevelArea.A2_Caldeum_Uprising;
				
				case 21: return SNOLevelArea.A2_caOUT_BorderlandsKhamsin;
				
				case 22: return SNOLevelArea.A2_caOUT_StingingWinds;
					
				case 23: return SNOLevelArea.A2_caOut_Oasis;
					
				case 24: return SNOLevelArea.A2_caOUT_Boneyard_01;
				
				case 25: return SNOLevelArea.A2_Dun_Zolt_Lobby;
				case 26: return SNOLevelArea.A3_Dun_Keep_Hub;
				case 27: return SNOLevelArea.A3_dun_rmpt_Level02;
				
				case 28: return SNOLevelArea.A3_Dun_Keep_Level03;
				
				case 29: return SNOLevelArea.A3_Dun_Keep_Level04;
				
				case 30: return SNOLevelArea.A3_Dun_Keep_Level05;
				
				case 31: return SNOLevelArea.A3_Dun_Battlefield_Gate;
				
				case 32: return SNOLevelArea.A3_Bridge_Choke_A;
			
				case 33: return SNOLevelArea.A3_Battlefield_B;
			
				case 34: return SNOLevelArea.A3_Dun_Crater_Level_01;
				
				case 35: return SNOLevelArea.A3_dun_Crater_ST_Level01;
			
				case 36: return SNOLevelArea.A3_Dun_Crater_Level_02;
			
				case 37: return SNOLevelArea.A3_dun_Crater_ST_Level01B;
			
				case 38: return SNOLevelArea.A3_Dun_Crater_Level_03;

				case 39: return SNOLevelArea.A3_Dun_Keep_Hub;

				case 40: return SNOLevelArea.A4_dun_Heaven_1000_Monsters_Fight_Entrance;
				
				case 41: return SNOLevelArea.A4_dun_Garden_of_Hope_01;
				
				case 42: return SNOLevelArea.A4_dun_Garden_of_Hope_02;
				
				case 43: return SNOLevelArea.A4_dun_Hell_Portal_01;
				
				case 44: return SNOLevelArea.A4_dun_Spire_01;
				
				case 45: return SNOLevelArea.A4_dun_Spire_02;

				case 46: return SNOLevelArea.x1_Westm_Hub;
				case 47: return SNOLevelArea.X1_WESTM_ZONE_01;
				
				case 48: return SNOLevelArea.X1_Westm_Graveyard_DeathOrb;
				
				case 49: return SNOLevelArea.X1_WESTM_ZONE_03;
				
				case 50: return SNOLevelArea.x1_Bog_01_Part2;
				case 51: return SNOLevelArea.x1_Catacombs_Level01;
				case 52: return SNOLevelArea.x1_Catacombs_Level02;
				case 53: return SNOLevelArea.x1_fortress_level_01;
				case 54: return SNOLevelArea.x1_fortress_level_02_Intro;
				case 55: return SNOLevelArea.X1_Pand_Ext_2_Battlefields;
			}

			return SNOLevelArea.Limbo;
		}
		public static int GetWaypointBySNOLevelID(SNOLevelArea id)
		{
			switch (id)
			{
				case SNOLevelArea.A1_Tristram_Adventure_Mode_Hub:
					return 0;
				case SNOLevelArea.A1_trOut_Old_Tristram_Road_Cath:
					return 1;
				case SNOLevelArea.A1_trDun_Level01:
					return 2;
				case SNOLevelArea.A1_trDun_Level04:
					return 3;
				case SNOLevelArea.A1_trDun_Level06:
					return 4;
				case SNOLevelArea.A1_trDun_Level07B:
					return 5;
				case SNOLevelArea.A1_trOut_TristramWilderness:
					return 6;
				case SNOLevelArea.A1_trOut_Wilderness_BurialGrounds:
					return 7;
				case SNOLevelArea.A1_trOut_TristramFields_A:
					return 9;
				case SNOLevelArea.A1_trOut_TristramFields_B:
					return 10;
				case SNOLevelArea.A1_trOut_TownAttack_ChapelCellar:
					return 11;
				case SNOLevelArea.A1_C6_SpiderCave_01_Main:
					return 12;
				case SNOLevelArea.A1_trOUT_Highlands_Bridge:
					return 13;
				case SNOLevelArea.A1_trOUT_Highlands2:
					return 14;
				case SNOLevelArea.A1_trDun_Leoric01:
					return 15;
				case SNOLevelArea.A1_trDun_Leoric02:
					return 16;
				case SNOLevelArea.A1_trDun_Leoric03:
					return 17;
				case SNOLevelArea.A3_Dun_Keep_Hub:
					return 26;
				case SNOLevelArea.A2_caOUT_StingingWinds_Canyon:
					return 19;
				case SNOLevelArea.A2_Caldeum_Uprising:
					return 20;
				case SNOLevelArea.A2_caOUT_BorderlandsKhamsin:
					return 21;
				case SNOLevelArea.A2_caOUT_StingingWinds:
					return 22;
				case SNOLevelArea.A2_caOut_Oasis:
					return 23;
				case SNOLevelArea.A2_caOUT_Boneyard_01:
					return 24;
				case SNOLevelArea.A2_Dun_Zolt_Lobby:
					return 25;

				case SNOLevelArea.A3_dun_rmpt_Level02:
					return 27;
				case SNOLevelArea.A3_Dun_Keep_Level03:
					return 28;
				case SNOLevelArea.A3_Dun_Keep_Level04:
					return 29;
				case SNOLevelArea.A3_Dun_Keep_Level05:
					return 30;
				case SNOLevelArea.A3_Dun_Battlefield_Gate:
					return 31;
				case SNOLevelArea.A3_Bridge_Choke_A:
					return 32;
				case SNOLevelArea.A3_Battlefield_B:
					return 33;
				case SNOLevelArea.A3_Dun_Crater_Level_01:
					return 34;
				case SNOLevelArea.A3_dun_Crater_ST_Level01:
					return 35;
				case SNOLevelArea.A3_Dun_Crater_Level_02:
					return 36;
				case SNOLevelArea.A3_dun_Crater_ST_Level01B:
					return 37;
				case SNOLevelArea.A3_Dun_Crater_Level_03:
					return 38;

				case SNOLevelArea.A4_dun_Heaven_1000_Monsters_Fight_Entrance:
					return 40;
				case SNOLevelArea.A4_dun_Garden_of_Hope_01:
					return 41;
				case SNOLevelArea.A4_dun_Garden_of_Hope_02:
					return 42;
				case SNOLevelArea.A4_dun_Hell_Portal_01:
					return 43;
				case SNOLevelArea.A4_dun_Spire_01:
					return 44;
				case SNOLevelArea.A4_dun_Spire_02:
					return 45;
				case SNOLevelArea.x1_Westm_Hub:
					return 46;
				case SNOLevelArea.X1_WESTM_ZONE_01:
					return 47;
				case SNOLevelArea.X1_Westm_Graveyard_DeathOrb:
					return 48;
				case SNOLevelArea.X1_WESTM_ZONE_03:
					return 49;
				case SNOLevelArea.x1_Bog_01_Part2:
					return 50;
				case SNOLevelArea.x1_Catacombs_Level01:
					return 51;
				case SNOLevelArea.x1_Catacombs_Level02:
					return 52;
				case SNOLevelArea.x1_fortress_level_01:
					return 53;
				case SNOLevelArea.x1_fortress_level_02_Intro:
					return 54;
				case SNOLevelArea.X1_Pand_Ext_2_Battlefields:
					return 55;
			}

			return -1;
		}
		///<summary>
		///To Find Town Areas
		///</summary>
		public static Act FindActByTownLevelAreaID(int ID)
		{
			switch (ID)
			{
				case 332339:
				case 19947:
					return Act.A1;
				case 168314:
					return Act.A2;
				case 92945:
					return Act.A3;
				case 270011:
					return Act.A5;
			}

			return Act.Invalid;
		}
		public enum TownRunBehavior
		{
			Stash,
			Sell,
			Salvage,
			Gamble,
			Interaction,
			Idenify,
			NephalemObelisk
		}
		public static Vector3 ReturnTownRunMovementVector(TownRunBehavior type, Act act)
		{
			switch (type)
			{
				case TownRunBehavior.Salvage:
					switch (act)
					{
						case Act.A1:
							if (!FunkyGame.AdventureMode)
								return new Vector3(2958.418f, 2823.037f, 24.04533f);
							else
								return new Vector3(375.5075f, 563.1337f, 24.04533f);
						case Act.A2:
							return new Vector3(289.6358f, 232.1146f, 0.1f);
						case Act.A3:
						case Act.A4://x="332.819" y="423.1313" z="0.4986931" (new Vector3(332.819f,423.1313f,0.4986931f))
							return new Vector3(379.6096f, 415.6198f, 0.3321424f);
						case Act.A5://x="538.4665" y="479.0026" z="2.620764" (new Vector3())
							return new Vector3(560.1434f, 501.5706f, 2.685907f);
					}
					break;
				case TownRunBehavior.Sell:
					switch (act)
					{
						case Act.A1:
							if (!FunkyGame.AdventureMode)
								return new Vector3(2901.399f, 2809.826f, 24.04533f);
							else
								return new Vector3(320.8555f, 524.6776f, 24.04532f);
						case Act.A2:
							return new Vector3(295.2101f, 265.1436f, 0.1000002f);
						case Act.A3:
						case Act.A4:
							return new Vector3(418.9743f, 351.0592f, 0.1000005f);
						case Act.A5:
							return new Vector3(560.1434f, 501.5706f, 2.685907f);
					}
					break;
				case TownRunBehavior.Stash:
					switch (act)
					{
						case Act.A1:
							if (!FunkyGame.AdventureMode)
								return new Vector3(2967.146f, 2799.459f, 24.04533f);
							else
								return new Vector3(386.8494f, 524.2585f, 24.04533f);
						case Act.A2:
							return new Vector3(323.4543f, 228.5806f, 0.1f);
						case Act.A3:
						case Act.A4:
							return new Vector3(389.3798f, 390.7143f, 0.3321428f);
						case Act.A5:
							return new Vector3(510.6552f, 502.1889f, 2.620764f);
					}
					break;
				case TownRunBehavior.Gamble:
					switch (act)
					{
						case Act.A1:
							return new Vector3(376.3878f, 561.3141f, 24.04533f);
						case Act.A2:
							return new Vector3(334.8506f, 267.2392f, 0.1000038f);
						case Act.A3:
						case Act.A4:
							return new Vector3(458.5429f, 416.3311f, 0.2663189f);
						case Act.A5:
							return new Vector3(592.5067f, 535.6719f, 2.74532f);
					}
					break;
				case TownRunBehavior.Interaction:
					switch (act)
					{
						case Act.A1:
							if (!FunkyGame.AdventureMode)
								return new Vector3(2959.277f, 2811.887f, 24.04533f);
							else
								return new Vector3(386.6582f, 534.2561f, 24.04533f);
						case Act.A2:
							return new Vector3(299.5841f, 250.1721f, 0.1000036f);
						case Act.A3:
						case Act.A4:
							return new Vector3(403.7034f, 395.9311f, 0.5069602f);
						case Act.A5:
							return new Vector3(532.3179f, 521.8536f, 2.662077f);
					}
					break;
				case TownRunBehavior.Idenify:
					switch (act)
					{
						case Act.A1:
							if (!FunkyGame.AdventureMode)
								return new Vector3(2955.026f, 2817.4f, 24.04533f);
							else
								return new Vector3(372.3016f, 532.6918f, 24.04532f);
						case Act.A2:
							return new Vector3(326.9954f, 250.1623f, -0.3242276f);
						case Act.A3:
						case Act.A4:
							return new Vector3(398.9163f, 393.4324f, 0.3577437f);
						case Act.A5:
							return new Vector3(523.6658f, 525.9195f, 2.662077f);
					}
					break;
				case TownRunBehavior.NephalemObelisk:
					switch (act)
					{
						case Act.A1:
							return new Vector3(374.0686f,585.5703f,24.04528f);
						case Act.A2:
							return new Vector3(349.6098f,264.5385f,0.09394981f);
						case Act.A3:
						case Act.A4:
							return new Vector3(459.3317f,391.8568f,0.392121f);
						case Act.A5:
							return new Vector3(601.1368f, 754.1678f, 2.703856f);
					}
					break;
			}

			return Vector3.Zero;
		}
	}
}
