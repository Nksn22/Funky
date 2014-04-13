﻿using System;
using System.Collections.Generic;
using FunkyBot.Cache.Objects;
using FunkyBot.Player.HotBar.Skills;
using FunkyBot.Cache;
using FunkyBot.Cache.Enums;
using FunkyBot.Movement;
using Zeta.Bot.Settings;
using Zeta.Common;

namespace FunkyBot.Targeting.Behaviors
{
	public class TBUpdateTarget : TargetBehavior
	{
		private DateTime lastAvoidanceConnectSearch = DateTime.Today;
		private bool bStayPutDuringAvoidance = false;
		public TBUpdateTarget() : base() { }


		public override TargetBehavioralTypes TargetBehavioralTypeType
		{
			get
			{
				return TargetBehavioralTypes.Target;
			}
		}
		public override void Initialize()
		{
			base.Test = (ref CacheObject obj) =>
			{
				this.bStayPutDuringAvoidance = false;

				//cluster update
				Bot.Targeting.Cache.Clusters.UpdateTargetClusteringVariables();

				//Standard weighting of valid objects -- updates current target.
				this.WeightEvaluationObjList(ref obj);


				//Final Possible Target Check
				if (obj == null)
				{
					// No valid targets but we were told to stay put?
					if (this.bStayPutDuringAvoidance)
					{
						//Lets check our avoidance object list
						if (Bot.Targeting.Cache.objectsIgnoredDueToAvoidance.Count > 0 && DateTime.Now.Subtract(lastAvoidanceConnectSearch).TotalMilliseconds > 4000)
						{
							Logger.DBLog.InfoFormat("Preforming Avoidance Connection Search on Potential Objects");
							lastAvoidanceConnectSearch = DateTime.Now;


							
							if (Bot.NavigationCache.CurrentGPArea != null &&
								Bot.NavigationCache.CurrentGPArea.LastFoundSafeSpots.Count>0 &&
								DateTime.Now.Subtract(Bot.NavigationCache.CurrentGPArea.DateSearchSafeSpots).TotalMilliseconds<2000 )
							{
								Logger.DBLog.Info("Using exisiting Safe Spots!");
								foreach (var v in Bot.NavigationCache.CurrentGPArea.LastFoundSafeSpots)
								{
									foreach (CacheObject testobj in Bot.Targeting.Cache.objectsIgnoredDueToAvoidance)
									{
										if (Navigation.CheckVectorFlags(v, testobj.Position, PointCheckingFlags.AvoidanceOverlap | PointCheckingFlags.RaycastNavProvider))
										{
											obj = new CacheObject(v, TargetType.Avoidance, 20000, "Avoid Connection", 2.5f, -1);
											return true;
										}
									}
								}
							}
							else
							{
								//Update or Create Bot Postion GPRect
								GPRectangle botrect = new GPRectangle(Bot.Character.Data.Position);
								Vector3 connectVector3;
								foreach (CacheObject testobj in Bot.Targeting.Cache.objectsIgnoredDueToAvoidance)
								{
									if (botrect.TryFindSafeSpot(Bot.Character.Data.Position,out connectVector3, testobj.Position, PointCheckingFlags.AvoidanceOverlap | PointCheckingFlags.BlockedDirection | PointCheckingFlags.MonsterOverlap | PointCheckingFlags.ObstacleOverlap | PointCheckingFlags.RaycastWalkable, new List<GridPoint>(), false))
									{
										obj = new CacheObject(connectVector3, TargetType.Avoidance, 20000, "Avoid Connection", 2.5f, -1);
										return true;
									}
								}
							}
						}

						if (Bot.Targeting.Cache.Environment.TriggeringAvoidances.Count == 0)
						{
							obj = new CacheObject(Bot.Character.Data.Position, TargetType.Avoidance, 20000, "StayPutPoint", 2.5f, -1);
							return true;
						}
					}
				}

				return false;
			};
		}

		///<summary>
		///Iterates through Usable objects and sets the Bot.CurrentTarget to the highest weighted object found inside the given list.
		///</summary>
		private void WeightEvaluationObjList(ref CacheObject CurrentTarget)
		{
			// Store if we are ignoring all units this cycle or not
			bool bIgnoreAllUnits = !Bot.Targeting.Cache.Environment.bAnyChampionsPresent
										&& ((!Bot.Targeting.Cache.Environment.bAnyTreasureGoblinsPresent && Bot.Settings.Targeting.GoblinPriority >= 2) || Bot.Settings.Targeting.GoblinPriority < 2)
										&& Bot.Character.Data.dCurrentHealthPct >= 0.85d;


			//clear our last "avoid" list..
			Bot.Targeting.Cache.objectsIgnoredDueToAvoidance.Clear();

			double iHighestWeightFound = 0;

			foreach (CacheObject thisobj in Bot.Targeting.Cache.ValidObjects)
			{
				thisobj.UpdateWeight();

				if (thisobj.Weight == 1)
				{
					// Force the character to stay where it is if there is nothing available that is out of avoidance stuff and we aren't already in avoidance stuff
					thisobj.Weight = 0;
					if (!Bot.Targeting.Cache.RequiresAvoidance)
						this.bStayPutDuringAvoidance = true;

					continue;
				}

				// Is the weight of this one higher than the current-highest weight? Then make this the new primary target!
				if (thisobj.Weight > iHighestWeightFound && thisobj.Weight > 0)
				{
					//Check combat looting (Demonbuddy Setting)
					if (iHighestWeightFound > 0
										 && thisobj.targetType.Value == TargetType.Item
										 && !CharacterSettings.Instance.CombatLooting
										 && CurrentTarget.targetType.Value == TargetType.Unit) continue;


					//cache RAGUID so we can switch back if we need to
					int CurrentTargetRAGUID = CurrentTarget != null ? CurrentTarget.RAGUID : -1;

					//Set our current target to this object!
					CurrentTarget = ObjectCache.Objects[thisobj.RAGUID];

					bool resetTarget = false;


					if (!Bot.Character.Class.IsMeleeClass && CurrentTarget.targetType.Value == TargetType.Unit && Bot.Targeting.Cache.Environment.NearbyAvoidances.Count > 0)
					{//Ranged Class -- Unit -- with Nearby Avoidances..

						//We are checking if this target is valid and will not cause avoidance triggering due to movement.


						//set unit target (for Ability selector).
						Bot.Targeting.Cache.CurrentUnitTarget = (CacheUnit)CurrentTarget;

						//Generate next Ability..
						Skill nextAbility = Bot.Character.Class.AbilitySelector(Bot.Targeting.Cache.CurrentUnitTarget, Bot.Targeting.Cache.LastCachedTarget.targetType == TargetType.NoMovement);


						if (nextAbility == Bot.Character.Class.DefaultAttack && !Bot.Character.Class.CanUseDefaultAttack)
						{//No valid ability found

							Logger.Write(LogLevel.Target, "Could not find a valid ability for unit {0}", thisobj.InternalName);

							//if (thisobj.ObjectIsSpecial)
							//     ObjectCache.Objects.objectsIgnoredDueToAvoidance.Add(thisobj);
							//else
							resetTarget = true;

						}
						else
						{
							Vector3 destination = nextAbility.DestinationVector;
							if (ObjectCache.Obstacles.TestVectorAgainstAvoidanceZones(Bot.Character.Data.Position, destination))
							{
								//if (!thisobj.ObjectIsSpecial)
								//	resetTarget = true;
								//else
								Bot.Targeting.Cache.objectsIgnoredDueToAvoidance.Add(thisobj);
							}
						}

						//reset unit target
						Bot.Targeting.Cache.CurrentUnitTarget = null;
					}

					//Avoidance Attempt to find a location where we can attack!
					if (Bot.Targeting.Cache.objectsIgnoredDueToAvoidance.Contains(thisobj))
					{
						//Wait if no valid target found yet.. and no avoidance movement required.
						if (!Bot.Targeting.Cache.RequiresAvoidance)
							this.bStayPutDuringAvoidance = true;

						resetTarget = true;
					}

					if (resetTarget)
					{
						CurrentTarget = CurrentTargetRAGUID != -1 ? ObjectCache.Objects[CurrentTargetRAGUID] : null;
						continue;
					}

					iHighestWeightFound = thisobj.Weight;
				}

			} // Loop through all the objects and give them a weight
		}
	}
}
