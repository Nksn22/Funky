﻿using System;
using System.Linq;
using FunkyBot.Cache;
using FunkyBot.Cache.Enums;
using FunkyBot.Cache.Objects;
using Zeta.Bot.Dungeons;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
using Zeta.Bot.Pathfinding;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Zeta.Common;
using System.Collections.Generic;
using System.Globalization;
using FunkyBot.Movement.Clustering;

namespace FunkyBot.Movement
{


	///<summary>
	///Cache of all values Navigation related
	///</summary>
	public class Navigation
	{
		#region Bot (Character) Properties
		private bool isMoving;
		///<summary>
		///Cached Value of ZetaDia.Me.IsMoving
		///</summary>
		public bool IsMoving
		{
			get
			{
				RefreshMovementCache();
				return isMoving;
			}
		}

		private MovementState curMoveState = MovementState.None;
		public MovementState currentMovementState
		{
			get
			{
				RefreshMovementCache();
				return curMoveState;
			}
		}

		private float curRotation;
		public float currentRotation
		{
			get
			{
				return curRotation;
			}
		}

		private double curSpeedXY;
		public double currentSpeedXY
		{
			get
			{
				RefreshMovementCache();
				return curSpeedXY;
			}
		}

		private StuckFlags stuckflags;
		public StuckFlags Stuckflags
		{
			get
			{
				RefreshMovementCache();
				return stuckflags;
			}
		}

		private DateTime LastUpdatedMovementData = DateTime.Today;
		internal void RefreshMovementCache()
		{
			if (DateTime.Now.Subtract(LastUpdatedMovementData).TotalMilliseconds < 150)
				return;

			LastUpdatedMovementData = DateTime.Now;

			//These vars are used to accuratly predict what the bot is doing (Melee Movement/Combat)
			using (ZetaDia.Memory.AcquireFrame())
			{
				// If we aren't in the game of a world is loading, don't do anything yet
				if (!ZetaDia.IsInGame || ZetaDia.IsLoadingWorld)
					return;
				try
				{
					ActorMovement botMovement = ZetaDia.Me.Movement;
					isMoving = botMovement.IsMoving;
					curSpeedXY = botMovement.SpeedXY;
					curRotation = botMovement.Rotation;
					curMoveState = botMovement.MovementState;
					stuckflags = botMovement.StuckFlags;
				}
				catch
				{

				}
			}

		}

		#endregion




		//GPCache

		private GPRectangle CurrentLocationGPrect;
		///<summary>
		///Current Bots Position Rectangle
		///</summary>
		public GPRectangle CurrentLocationGPRect
		{
			get { return CurrentLocationGPrect; }
			set { CurrentLocationGPrect = value; }
		}

		private AreaBoundary currentLocationBoundary;
		public AreaBoundary CurrentLocationBoundary
		{
			get { return currentLocationBoundary; }
			set { currentLocationBoundary = value; }
		}








		///<summary>
		///Bots Position is blocked from adjacent movement -- Updated whenever searching for a safe location!
		///</summary>
		internal void RefreshNavigationBlocked()
		{
			//Check if bot is navigationally blocked.. (if bot pos changed or 500ms elapsed then we refresh!)
			if (LastBotPositionChecked != Bot.Character.Data.Position || DateTime.Now.Subtract(LastNavigationBlockCheck).TotalMilliseconds > 500)
			{
				LastNavigationBlockCheck = DateTime.Now;
				LastBotPositionChecked = Bot.Character.Data.Position;

				if (IsVectorBlocked(Bot.Character.Data.Position))
				{
					//Logger.DBLog.InfoFormat("[Funky] Current Position is Navigationally Blocked");
					BotIsNavigationallyBlocked = true;
				}
				else
					BotIsNavigationallyBlocked = false;
			}
		}
		internal bool BotIsNavigationallyBlocked = false;
		private DateTime LastNavigationBlockCheck = DateTime.Today;
		private Vector3 LastBotPositionChecked = Vector3.Zero;

		#region Vetor Blocking
		//Tracks Objects that occupy surrounding navigation grid points
		private Dictionary<int, int> LastObjectblockCounter = new Dictionary<int, int>();
		private Dictionary<int, GridPoint[]> LastObjectOccupiedGridPoints = new Dictionary<int, GridPoint[]>();
		private GPRectangle LastUsedBlockCheckGPRect = null;
		private List<GridPoint> LastNavigationBlockedPoints = new List<GridPoint>();
		///<summary>
		///Checks if the position is total blocked from adjacent movements either by objects or non navigation
		///</summary>
		private bool IsVectorBlocked(Vector3 location)
		{
			//Reset Navigationally Blocked GPs
			LastNavigationBlockedPoints = new List<GridPoint>();

			//Create Local GPRect!
			if (LastUsedBlockCheckGPRect == null || LastUsedBlockCheckGPRect.centerpoint != (GridPoint)location)
			{
				//Clear lists
				LastObjectblockCounter.Clear();
				LastObjectOccupiedGridPoints.Clear();
				LastUsedBlockCheckGPRect = new GPRectangle(location);
			}

			if (LastUsedBlockCheckGPRect.Count == 0)
			{
				//Logger.DBLog.DebugFormat("Current Location GP Rect has no valid Grid Points!");
				return false;
			}

			GridPoint[] CurrentLocationGridPoints = LastUsedBlockCheckGPRect.Keys.ToArray();
			List<GridPoint> SurroundingPoints = new List<GridPoint>();
			int SurroundingMaxCount = LastUsedBlockCheckGPRect.Count >= 8 ? 8 : LastUsedBlockCheckGPRect.Count;
			for (int i = 0; i < SurroundingMaxCount; i++)
			{
				GridPoint gp = CurrentLocationGridPoints[i];
				if (!gp.Ignored)
					SurroundingPoints.Add(gp);
				else
					LastNavigationBlockedPoints.Add(gp);
			}

			List<int> NearbyObjectRAGUIDs = new List<int>();
			List<CacheServerObject> NearbyObjects = Bot.Targeting.Cache.Environment.NearbyObstacleObjects.Where(obj => obj.RadiusDistance <= 6f).ToList();//ObjectCache.Obstacles.Navigations.Where(obj => obj.RadiusDistance<=5f).ToList();

			//no nearby objects passed distance check..
			if (NearbyObjects.Count == 0)
			{
				//Clear list, and return pure navigational check (Zero means we are completely stuck in a non-navigable location?)
				LastObjectblockCounter.Clear();
				LastObjectOccupiedGridPoints.Clear();

				//Logger.DBLog.InfoFormat("Current Location Point has {0} usable points (NoNewObjs)", SurroundingPoints.Count);

				return (SurroundingPoints.Count == 0);
			}

			//Update ObjectBlockCounter Collection
			if (LastObjectblockCounter.Count > 0)
			{
				//Add current nearby object RAGUIDs to collection
				NearbyObjectRAGUIDs.AddRange((from objs in NearbyObjects
											  select objs.RAGUID).ToArray());

				//Generate Removal List for ObjectBlockCounter Collections
				List<int> RemovalRAGUIDList = (from raguids in LastObjectblockCounter.Keys
											   where !NearbyObjectRAGUIDs.Contains(raguids)
											   select raguids).ToList();

				//Removal
				foreach (var item in RemovalRAGUIDList)
				{
					LastObjectblockCounter.Remove(item);
					LastObjectOccupiedGridPoints.Remove(item);
				}
			}

			//Check any exisiting block entries
			if (LastObjectblockCounter.Count > 0)
			{
				foreach (var item in LastObjectOccupiedGridPoints.Values)
				{
					LastNavigationBlockedPoints.AddRange(item);
				}

				//Update Surrounding Points
				SurroundingPoints = SurroundingPoints.Except(LastNavigationBlockedPoints).ToList();

				if (SurroundingPoints.Count == 0)
				{
					//Logger.DBLog.InfoFormat("NavBlocked -- No available surrounding points.");

					return true;
				}
			}

			//Generate new object list that contains objects that are not already accounted for
			List<CacheServerObject> NewObjects = NearbyObjects.Where(obj => !LastObjectblockCounter.ContainsKey(obj.RAGUID) || LastObjectblockCounter[obj.RAGUID] < 4).ToList();

			//No new objects to test..
			if (NewObjects.Count == 0)
			{
				//Logger.DBLog.InfoFormat("No new Objects Unaccounted");

				return (SurroundingPoints.Count == 0);
			}


			foreach (GridPoint item in SurroundingPoints)
			{
				//Find any objects that contain this GP
				CacheServerObject[] ContainedObjs = NewObjects.Where(Obj => Obj.PointInside(item)			   //only objects that have hit there maximum block count.
																				&& (!LastObjectblockCounter.ContainsKey(Obj.RAGUID) || Math.Round(Obj.PointRadius) < LastObjectblockCounter[Obj.RAGUID])).ToArray();
				if (ContainedObjs.Length > 0)
				{
					//if (ContainedObjs.Length > 1 && Bot.Settings.Debug.FunkyLogFlags.HasFlag(LogLevel.Movement))
						//Logger.DBLog.InfoFormat("Multiple Objects Found Occuping Grid Point!");

					CacheServerObject ThisObjBlocking = ContainedObjs[0];
					int ObjRAGUID = ThisObjBlocking.RAGUID;

					if (LastObjectblockCounter.ContainsKey(ObjRAGUID))
					{
						int GPCount = LastObjectOccupiedGridPoints[ObjRAGUID].Length;
						LastObjectblockCounter[ObjRAGUID]++;
						GridPoint[] newArrayGPs = new GridPoint[GPCount];
						LastObjectOccupiedGridPoints[ObjRAGUID].CopyTo(newArrayGPs, 0);
						newArrayGPs[GPCount - 1] = item.Clone();
						LastObjectOccupiedGridPoints[ObjRAGUID] = newArrayGPs;
					}
					else
					{
						LastObjectblockCounter.Add(ObjRAGUID, 1);
						GridPoint[] NewArrayGP = new GridPoint[1] { item.Clone() };
						LastObjectOccupiedGridPoints.Add(ObjRAGUID, NewArrayGP);
					}

					LastNavigationBlockedPoints.Add(item);
				}
			}

			//Update Surrounding Points
			SurroundingPoints = SurroundingPoints.Except(LastNavigationBlockedPoints).ToList();

			//Logger.DBLog.InfoFormat("Current Location Point has {0} usable points", SurroundingPoints.Count);


			return (SurroundingPoints.Count == 0);
		}

		#endregion



		private GPArea CurrentGParea;
		///<summary>
		///Area used to search for movement locations.
		///</summary>
		public GPArea CurrentGPArea
		{
			get { return CurrentGParea; }
			set { CurrentGParea = value; }
		}
		// When we last FOUND a safe spot
		public DateTime lastFoundSafeSpot = DateTime.Today;
		public Vector3 vlastSafeSpot = Vector3.Zero;
		///<summary>
		///Last successful GP Rectangle used during search method.
		///</summary>
		public GPRectangle LastUsedRect
		{
			get { return CurrentGParea != null ? CurrentGParea.lastUsedGPRect : null; }
		}
		private Vector3 lastsearchvector = Vector3.Zero;
		public Vector3 LastSearchVector
		{
			get { return lastsearchvector; }
			set { lastsearchvector = value; }
		}

		public bool CurrentAreaValid()
		{
			return (Bot.NavigationCache.CurrentGPArea != null && !Bot.NavigationCache.CurrentGPArea.AllGPRectsFailed
				&& (Bot.NavigationCache.CurrentGPArea.StartingLocation == Bot.Character.Data.Position || Bot.NavigationCache.CurrentGPArea.centerGPRect.Contains(Bot.Character.Data.Position)));
		}

		public Vector3 AttemptToReuseLastLocationFound()
		{
			if (vlastSafeSpot != Vector3.Zero)
			{
				//Check how close we are..
				if (Bot.Character.Data.Position.Distance2D(vlastSafeSpot) < 2.5f)
				{
					vlastSafeSpot = Vector3.Zero;

				}
			}
			return vlastSafeSpot;
		}
		///<summary>
		///Searches for a safespot
		///</summary>
		public bool AttemptFindSafeSpot(out Vector3 safespot, Vector3 LOS, PointCheckingFlags flags)
		{
			safespot = vlastSafeSpot;

			Vector3 BotPosition = Bot.Character.Data.Position;

			//Recreate the entire area?
			if (!CurrentAreaValid())
				CurrentGPArea = new GPArea(BotPosition);



			//Check Bot Navigationally blocked
			RefreshNavigationBlocked();
			if (BotIsNavigationallyBlocked)
			{
				return false;
			}

			//Recreate Bot Current rect?
			//if (CurrentLocationGPrect == null || CurrentLocationGPrect.centerpoint != Bot.Character.Data.PointPosition)
			//{
			//	Bot.NavigationCache.CurrentLocationGPrect = new GPRectangle(BotPosition);
			//	//Refresh boundary (blocked directions)
			//	currentLocationBoundary = new AreaBoundary(BotPosition);
			//	UpdateLocationsBlocked();
			//}

			// Bot.NavigationCache.CurrentLocationGPRect.UpdateObjectCount();

			safespot = CurrentGPArea.AttemptFindSafeSpot(BotPosition, LOS, flags);
			return (safespot != Vector3.Zero);
		}


		private List<LocationFlags> blockedLocationDirections = new List<LocationFlags>();
		public bool CheckPointAgainstBlockedDirection(GridPoint point)
		{
			if (blockedLocationDirections.Count > 0)
			{
				LocationFlags pointFlags = CurrentLocationBoundary.ComputeOutCode(point.X, point.Y);
				return blockedLocationDirections.Contains(pointFlags);
			}
			return false;
		}
		private void UpdateLocationsBlocked()
		{
			blockedLocationDirections.Clear();

			if (LastNavigationBlockedPoints.Count > 0)
			{
				var Locations = LastNavigationBlockedPoints.Select(item => CurrentLocationBoundary.ComputeOutCode(item.X, item.Y)).ToList();

				if (Locations.Count > 2)
				{
					//TOP
					if (Locations.Count(p => p.HasFlag(LocationFlags.Top)) > 2)
					{
						blockedLocationDirections.Add(LocationFlags.Top);
					}
					//BOTTOM
					if (Locations.Count(p => p.HasFlag(LocationFlags.Bottom)) > 2)
					{
						blockedLocationDirections.Add(LocationFlags.Bottom);
					}
					//LEFT
					if (Locations.Count(p => p.HasFlag(LocationFlags.Left)) > 2)
					{
						blockedLocationDirections.Add(LocationFlags.Left);
					}
					//RIGHT
					if (Locations.Count(p => p.HasFlag(LocationFlags.Right)) > 2)
					{
						blockedLocationDirections.Add(LocationFlags.Right);
					}
				}

				//CHECK CORNERS
				if (!blockedLocationDirections.Contains(LocationFlags.Bottom))
				{
					if (!blockedLocationDirections.Contains(LocationFlags.Left) && Locations.Count(p => p.HasFlag(LocationFlags.BottomLeft)) > 0)
					{
						blockedLocationDirections.Add(LocationFlags.BottomLeft);
					}
					if (!blockedLocationDirections.Contains(LocationFlags.Right) && Locations.Count(p => p.HasFlag(LocationFlags.BottomRight)) > 0)
					{
						blockedLocationDirections.Add(LocationFlags.BottomRight);
					}
				}
				if (!blockedLocationDirections.Contains(LocationFlags.Top))
				{
					if (!blockedLocationDirections.Contains(LocationFlags.Left) && Locations.Count(p => p.HasFlag(LocationFlags.TopLeft)) > 0)
					{
						blockedLocationDirections.Add(LocationFlags.TopLeft);
					}
					if (!blockedLocationDirections.Contains(LocationFlags.Right) && Locations.Count(p => p.HasFlag(LocationFlags.TopRight)) > 0)
					{
						blockedLocationDirections.Add(LocationFlags.TopRight);
					}
				}

				//Logger.DBLog.DebugFormat("Blocked Directions Count == [{0}]", blockedLocationDirections.Count.ToString());

			}
		}

		internal int iACDGUIDLastWhirlwind = 0;
		internal Vector3 vSideToSideTarget = Vector3.Zero;
		internal DateTime lastChangedZigZag = DateTime.Today;
		internal Vector3 vPositionLastZigZagCheck = Vector3.Zero;

		//Special Movements
		public Vector3 FindZigZagTargetLocation(Vector3 vTargetLocation, float fDistanceOutreach, bool bRandomizeDistance = false, bool bRandomizeStart = false)
		{
			var rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), NumberStyles.HexNumber));
			float iFakeStart = 0;
			//K: bRandomizeStart is for boss and elite, they usually jump around, make obstacles, let you Incapacitated. 
			//   you usually have to move back and forth to hit them
			if (bRandomizeStart)
				iFakeStart = rndNum.Next(18) * 5;
			if (bRandomizeDistance)
				fDistanceOutreach += rndNum.Next(18);
			float fDirectionToTarget = FindDirection(Bot.Character.Data.Position, vTargetLocation);

			float fHighestWeight = float.NegativeInfinity;
			Vector3 vBestLocation = Vector3.Zero;

			bool bFoundSafeSpotsFirstLoop = false;
			float fAdditionalRange = 0f;
			//K: Direction is more important than distance

			for (int iMultiplier = 1; iMultiplier <= 2; iMultiplier++)
			{
				if (iMultiplier == 2)
				{
					if (bFoundSafeSpotsFirstLoop)
						break;
					fAdditionalRange = 150f;
					if (bRandomizeStart)
						iFakeStart = 30f + (rndNum.Next(16) * 5);
					else
						iFakeStart = (rndNum.Next(17) * 5);
				}
				float fRunDistance = fDistanceOutreach;
				for (float iDegreeChange = iFakeStart; iDegreeChange <= 30f + fAdditionalRange; iDegreeChange += 5)
				{
					float iPosition = iDegreeChange;
					//point to target is better, otherwise we have to avoid obstacle first 
					if (iPosition > 105f)
						iPosition = 90f - iPosition;
					else if (iPosition > 30f)
						iPosition -= 15f;
					else
						iPosition = 15f - iPosition;
					float fPointToTarget = iPosition;

					iPosition += fDirectionToTarget;
					if (iPosition < 0)
						iPosition = 360f + iPosition;
					if (iPosition >= 360f)
						iPosition = iPosition - 360f;

					Vector3 vThisZigZag = MathEx.GetPointAt(Bot.Character.Data.Position, fRunDistance, MathEx.ToRadians(iPosition));

					if (fPointToTarget <= 30f || fPointToTarget >= 330f)
					{
						vThisZigZag.Z = vTargetLocation.Z;
					}
					else if (fPointToTarget <= 60f || fPointToTarget >= 300f)
					{
						//K: we are trying to find position that we can circle around the target
						//   but we shouldn't run too far away from target
						vThisZigZag.Z = (vTargetLocation.Z + Bot.Character.Data.Position.Z) / 2;
						fRunDistance = fDistanceOutreach - 5f;
					}
					else
					{
						//K: don't move too far if we are not point to target, we just try to move
						//   this can help a lot when we are near stairs
						fRunDistance = 8f;
					}

					bool bCanRayCast = MGP.CanStandAt(vThisZigZag);



					// Give weight to each zigzag point, so we can find the best one to aim for
					if (bCanRayCast)
					{
						const bool bAnyAvoidance = false;

						// Starting weight is 1000f
						float fThisWeight = 1000f;
						if (iMultiplier == 2)
							fThisWeight -= 80f;

						if (Bot.Character.Data.ShouldFlee && ObjectCache.Objects.IsPointNearbyMonsters(vThisZigZag, Bot.Settings.Fleeing.FleeMaxMonsterDistance))
							continue;

						if (ObjectCache.Obstacles.Navigations.Any(obj => obj.Obstacletype.Value != ObstacleType.Monster && obj.TestIntersection(Bot.Character.Data.Position, vThisZigZag, false)))
							continue;

						float distanceToTarget = vTargetLocation.Distance2D(Bot.Character.Data.Position);

						fThisWeight += (distanceToTarget * 10f);

						// Use this one if it's more weight, or we haven't even found one yet, or if same weight as another with a random chance
						if (fThisWeight > fHighestWeight)
						{
							fHighestWeight = fThisWeight;


							vBestLocation = new Vector3(vThisZigZag.X, vThisZigZag.Y, MGP.GetHeight(vThisZigZag.ToVector2()));


							if (!bAnyAvoidance)
								bFoundSafeSpotsFirstLoop = true;
						}
					}
					// Can we raycast to the point at minimum?
				}
				// Loop through degrees
			}
			// Loop through multiplier
			return vBestLocation;
		}

		#region Grouping
		internal bool groupRunningBehavior = false;
		internal bool groupReturningToOrgin = false;
		internal CacheUnit groupingCurrentUnit = null;
		internal CacheUnit groupingOrginUnit = null;
		internal DateTime groupingSuspendedDate = DateTime.MinValue;
		internal UnitCluster groupingUnitCluster = null;

		internal void GroupingFinishBehavior()
		{
			Bot.NavigationCache.groupRunningBehavior = false;
			Bot.NavigationCache.groupReturningToOrgin = false;
			Bot.NavigationCache.groupingCurrentUnit = null;
			Bot.NavigationCache.groupingOrginUnit = null;
			Bot.NavigationCache.groupingUnitCluster = null;
			Bot.NavigationCache.groupingSuspendedDate = DateTime.Now.AddMilliseconds(2500);
		}
		#endregion

		#region Line of Sight Movement
		internal CacheLineOfSight LOSmovementObject = null;
		internal List<int> LOSBlacklistedRAGUIDs = new List<int>();
		#endregion

		// For "position-shifting" to navigate around obstacle SNO's
		private DateTime LastObstacleIntersectionTest = DateTime.Today;
		//Prioritized IDs due to blocking
		internal List<int> PrioritizedRAGUIDs = new List<int>();
		///<summary>
		///Checks bots movement flags then prioritizes all objects that are considered to be blocking.
		///</summary>
		public void ObstaclePrioritizeCheck(float range = 20f)
		{

			if (DateTime.Now.Subtract(LastObstacleIntersectionTest).TotalMilliseconds > 1500)
			{
				RefreshMovementCache();

				if (!IsMoving || currentMovementState.Equals(MovementState.WalkingInPlace) || currentMovementState.Equals(MovementState.None) || Bot.Targeting.Movement.BlockedMovementCounter > 0)
				{
					LastObstacleIntersectionTest = DateTime.Now;

					//modify range based upon # of stucks
					if (Funky.PlayerMover.iTotalAntiStuckAttempts > 0) range += (Funky.PlayerMover.iTotalAntiStuckAttempts * 5f);

					//get collection of objects that pass the tests.
					var intersectingObstacles = Bot.Targeting.Cache.Environment.NearbyObstacleObjects //ObjectCache.Obstacles.Values.OfType<CacheServerObject>()
																			  .Where(obstacle =>
																					!PrioritizedRAGUIDs.Contains(obstacle.RAGUID)//Only objects not already prioritized
																					&& obstacle.Obstacletype.HasValue
																					&& ObstacleType.Navigation.HasFlag(obstacle.Obstacletype.Value)//only navigation/intersection blocking objects!
																					&& obstacle.RadiusDistance <= range //Only within range..
																					&& obstacle.BotIsFacing() || obstacle.RadiusDistance <= 0f);
					//&&obstacle.TestIntersection(BotGridPoint, IntersectionDestinationPoint));



					if (intersectingObstacles.Any())
					{
						var intersectingObjectRAGUIDs = (from objs in intersectingObstacles
														 select objs.RAGUID);

						PrioritizedRAGUIDs.AddRange(intersectingObjectRAGUIDs);
					}
				}
			}
		}




		//Static Methods
		#region Static Movement Methods
		public static bool CheckVectorFlags(Vector3 currentPos, Vector3 targetPos, PointCheckingFlags flags)
		{

			//Avoidance Check (Any Avoidance)
			if (flags.HasFlag(PointCheckingFlags.AvoidanceOverlap))
			{
				if (ObjectCache.Obstacles.IsPositionWithinAvoidanceArea(targetPos)) return false;
			}


			//Kiting Check
			if (flags.HasFlag(PointCheckingFlags.MonsterOverlap))
			{
				if (ObjectCache.Objects.OfType<CacheUnit>().Any(m => m.ShouldFlee && m.IsPositionWithinRange(targetPos, Bot.Settings.Fleeing.FleeMaxMonsterDistance))) return false;
			}


			//Avoidance Intersection Check
			if (flags.HasFlag(PointCheckingFlags.AvoidanceIntersection))
			{
				if (ObjectCache.Obstacles.TestVectorAgainstAvoidanceZones(currentPos, targetPos)) return false;
			}

			Vector3 botcurpos = Bot.Character.Data.Position;
			if (flags.HasFlag(PointCheckingFlags.Raycast))
			{
				if (!Navigation.CanRayCast(currentPos, targetPos)) return false;
			}
			if (flags.HasFlag(PointCheckingFlags.RaycastWalkable))
			{
				if (!Navigation.CanRayCast(currentPos, targetPos, NavCellFlags.AllowWalk)) return false;
			}
			if (flags.HasFlag(PointCheckingFlags.RaycastNavProvider))
			{
				if (!Navigation.CanRayCast(currentPos, targetPos, UseSearchGridProvider: true)) return false;
			}

			return true;
		}
		///<summary>
		///Ray Cast -- if no navcellflags parameter is given then it will use Navigator.Raycast -- else it uses ZetaDia.Physics.Raycast to test navcellflags
		///</summary>
		public static bool CanRayCast(Vector3 vStartLocation, Vector3 vDestination, NavCellFlags NavType = NavCellFlags.None, bool UseSearchGridProvider = false)
		{
			if (!NavType.Equals(NavCellFlags.None))
				return ZetaDia.Physics.Raycast(vStartLocation, vDestination, NavType); //False means nothing hit
			if (UseSearchGridProvider)
			{
				Vector2 hitVector;
				if (vStartLocation.Z >= vDestination.Z)
					return !Navigator.SearchGridProvider.Raycast(vStartLocation.ToVector2(), vDestination.ToVector2(), out hitVector);
				return !Navigator.SearchGridProvider.Raycast(vDestination.ToVector2(), vStartLocation.ToVector2(), out hitVector);
			}
			return !Navigator.Raycast(vStartLocation, vDestination); //True means nothing hit
		}

		// Quickly calculate the direction a vector is from ourselves, and return it as a float
		public static float FindDirection(Vector3 vStartLocation, Vector3 vTargetLocation, bool inRadian = false)
		{
			Vector3 startNormalized = vStartLocation;
			startNormalized.Normalize();
			Vector3 targetNormalized = vTargetLocation;
			targetNormalized.Normalize();

			float angle = NormalizeRadian((float)Math.Atan2((vTargetLocation.Y - vStartLocation.Y), (vTargetLocation.X - vStartLocation.X)));
			//MathEx.ToDegrees(NormalizeRadian((float)Math.Atan2(vTargetLocation.Y-vStartLocation.Y, vTargetLocation.X-vStartLocation.X)));



			if (inRadian)
				return angle;
			return MathEx.ToDegrees(angle);
		}
		public static float NormalizeRadian(float radian)
		{
			if (radian < 0)
			{
				double mod = -radian;
				mod %= Math.PI * 2d;
				mod = -mod + Math.PI * 2d;
				return (float)mod;
			}
			return (float)(radian % (Math.PI * 2d));
		}
		#endregion


		//Static Encapsulated Navigator Properties
		#region Static Navigator Properties

		/// <summary>
		/// PathFinder
		/// </summary>
		public static PathFinder PathFinder
		{
			get
			{
				return (Navigator.SearchGridProvider as PathFinder);
			}
		}
		/// <summary>
		/// MainGridProvider
		/// </summary>
		public static MainGridProvider MGP
		{
			get
			{
				return (Navigator.SearchGridProvider as MainGridProvider);
			}
		}
		/// <summary>
		/// DungeonExplorer
		/// </summary>
		public static DungeonExplorer DE
		{
			get
			{
				return (BrainBehavior.DungeonExplorer);
			}
		}
		///<summary>
		///Returns GetNavigationProviderAs as DefaultNavigationProvider (Pathing)
		///</summary>
		public static DefaultNavigationProvider NP
		{
			get
			{
				return Navigator.GetNavigationProviderAs<DefaultNavigationProvider>();
			}
		}

		#endregion


		//Town Scence IDs -- to verify in town!
		private static readonly HashSet<int> TownSceneIDs = new HashSet<int>
				{
					 //a1
					 33348,
					 //a2
					 161513,31413,
					 //a3
					 172876,180638,204708,
				};

		public static bool IsInTown()
		{
			bool isInTown;

			using (ZetaDia.Memory.AcquireFrame())
			{
				isInTown = ZetaDia.IsInTown;

			}

			//If true.. lets verify it with scene check!
			if (isInTown)
			{
				int curSceneID;
				using (ZetaDia.Memory.AcquireFrame())
				{
					curSceneID = ZetaDia.Me.CurrentScene.SceneGuid;
				}

				//Check the scene ID.. no match means FALSE!
				if (!TownSceneIDs.Contains(curSceneID))
					isInTown = false;
			}

			return isInTown;
		}
	}


}