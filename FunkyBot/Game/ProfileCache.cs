﻿using System;
using System.Linq;
using System.Collections.Generic;
using FunkyBot.Cache.Objects;
using FunkyBot.Settings;
using FunkyBot.XMLTags;
using Zeta.Bot;
using Zeta.Bot.Profile;
using Zeta.Bot.Profile.Common;
using Zeta.Common;

namespace FunkyBot.Game
{
	public class ProfileCache
	{
		// A list of "useonceonly" tags that have been triggered this xml profile
		internal static HashSet<int> hashUseOnceID = new HashSet<int>();
		internal static Dictionary<int, int> dictUseOnceID = new Dictionary<int, int>();
		// For the random ID tag
		internal static Dictionary<int, int> dictRandomID = new Dictionary<int, int>();

		internal static HashSet<int> PrioritizedObjects = new HashSet<int>(); 

		private static SettingCluster clusterSettingsTag=new SettingCluster();
		internal static SettingCluster ClusterSettingsTag
		{
			get { return clusterSettingsTag; }
			set { clusterSettingsTag = value; }
		}

		internal bool PreformingInteractiveBehavior
		{
			get
			{
				return IsRunningOOCBehavior && ProfileBehaviorIsOOCInteractive && InteractableCachedObject != null;
			}
		}

		internal Dictionary<int, CacheObject> InteractableObjectCache = new Dictionary<int, CacheObject>();
		internal bool ProfileBehaviorIsOOCInteractive { get; set; }

		internal CacheObject InteractableCachedObject = null;

		private ProfileBehavior currentProfileBehavior;
		internal ProfileBehavior CurrentProfileBehavior
		{
			get { return currentProfileBehavior; }
		}

		private DateTime LastProfileBehaviorCheck = DateTime.Today;
		///<summary>
		///Tracks Current Profile Behavior and sets IsRunningOOCBehavior depending on the current Type of behavior.
		///</summary>
		internal void CheckCurrentProfileBehavior()
		{
			if (DateTime.Now.Subtract(LastProfileBehaviorCheck).TotalMilliseconds > 450)
			{
				LastProfileBehaviorCheck = DateTime.Now;

				if ((currentProfileBehavior == null && ProfileManager.CurrentProfileBehavior != null && ProfileManager.CurrentProfileBehavior.Behavior != null)
					 || (ProfileManager.CurrentProfileBehavior != null && ProfileManager.CurrentProfileBehavior.Behavior != null && currentProfileBehavior != null && currentProfileBehavior.Behavior.Guid != ProfileManager.CurrentProfileBehavior.Behavior.Guid))
				{
					currentProfileBehavior = ProfileManager.CurrentProfileBehavior;
					Logger.Write(LogLevel.Event, "Profile Behavior Changed To {0}", currentProfileBehavior.GetType().ToString());

					Type profileTagType = currentProfileBehavior.GetType();
					string profileTagTypeString = profileTagType.ToString();

					if (oocDBTags.Contains(profileTagType) || profileTagTypeString=="MoveToActor")
					{
						if (InteractiveTags.Contains(profileTagType))
						{
							ProfileBehaviorIsOOCInteractive = true;
							Logger.DBLog.DebugFormat("Interactable Profile Tag!");

							InteractableCachedObject = GetInteractiveCachedObject(currentProfileBehavior);
							if (InteractableCachedObject != null)
								Logger.DBLog.DebugFormat("Found Cached Interactable Server Object");
						}
						else
						{
							ProfileBehaviorIsOOCInteractive = false;
							InteractableCachedObject = null;
						}

						Logger.DBLog.DebugFormat("Current Profile Behavior has enabled OOC Behavior.");
						IsRunningOOCBehavior = true;
					}
					else if(profileTagType == typeof(TrinityExploreDungeon))
					{
						//PrioritizedObjects.Clear();
					}
					else
					{
						ProfileBehaviorIsOOCInteractive = false;
						InteractableCachedObject = null;
						IsRunningOOCBehavior = false;
						PrioritizedObjects.Clear();
					}
				}

			}
		}
		internal bool IsRunningOOCBehavior { get; set; }



		internal static CacheObject GetInteractiveCachedObject(ProfileBehavior tag)
		{
			Type TagType = tag.GetType();
			if (InteractiveTags.Contains(TagType))
			{
				if (TagType == typeof(UseWaypointTag))
				{
					UseWaypointTag tagWP = (UseWaypointTag)tag;
					var WaypointObjects = Bot.Game.Profile.InteractableObjectCache.Values.Where(obj => obj.SNOID == 6442);
					foreach (CacheObject item in WaypointObjects)
					{
						if (item.Position.Distance(tagWP.Position) < 100f)
						{
							//Found matching waypoint object!
							return item;
						}
					}
				}
				else if (TagType == typeof(UseObjectTag))
				{
					UseObjectTag tagUseObj = (UseObjectTag)tag;
					if (tagUseObj.ActorId > 0)
					{//Using SNOID..
						var Objects = Bot.Game.Profile.InteractableObjectCache.Values.Where(obj => obj.SNOID == tagUseObj.ActorId);
						foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(Bot.Character.Data.Position)))
						{
							//Found matching object!
							return item;
						}

					}
					else
					{//use position to match object
						Vector3 tagPosition = tagUseObj.Position;
						var Objects = Bot.Game.Profile.InteractableObjectCache.Values.Where(obj => obj.Position.Distance(tagPosition) <= 100f);
						foreach (CacheObject item in Objects)
						{
							//Found matching object!
							return item;
						}
					}
				}
				else if (TagType == typeof(UsePortalTag))
				{
					UsePortalTag tagUsePortal = (UsePortalTag)tag;
					if (tagUsePortal.ActorId > 0)
					{//Using SNOID..
						var Objects = Bot.Game.Profile.InteractableObjectCache.Values.Where(obj => obj.SNOID == tagUsePortal.ActorId);
						foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(Bot.Character.Data.Position)))
						{
							//Found matching object!
							return item;
						}

					}
					else
					{//use position to match object
						Vector3 tagPosition = tagUsePortal.Position;
						var Objects = Bot.Game.Profile.InteractableObjectCache.Values.Where(obj => obj.Position.Distance(tagPosition) <= 100f);
						foreach (CacheObject item in Objects.OrderBy(obj => obj.Position.Distance(Bot.Character.Data.Position)))
						{
							//Found matching object!
							return item;
						}
					}
				}
			}

			return null;
		}

		//Common Used Profile Tags that should be considered Out-Of-Combat Behavior.
		private static readonly HashSet<Type> oocDBTags = new HashSet<Type> 
																	{ 
																	  typeof(UseWaypointTag), 
																	  typeof(UseObjectTag),
																	  typeof(UseTownPortalTag),
																	  //typeof(Zeta.CommonBot.Profile.Common.WaitTimerTag),
																	  typeof (TrinityTownPortal),
																	  typeof (FunkyWaypoint),
																	};

		//Common Used Profile Tags that requires backtracking during combat sessions.
		private static readonly HashSet<Type> InteractiveTags = new HashSet<Type> 
																	{ 
																	  typeof(UseWaypointTag), 
																	  typeof(UseObjectTag),
																	  //typeof(Zeta.CommonBot.Profile.Common.UseTownPortalTag),
																	  typeof(UsePortalTag),
																	};

		public ProfileCache()
		{
			ProfileBehaviorIsOOCInteractive = false;
		}
	}
}

