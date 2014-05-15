﻿using System;
using System.Linq;
using FunkyBot.Cache.Enums;
using FunkyBot.DBHandlers;
using FunkyBot.Movement;
using Zeta.Bot.Settings;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Zeta.TreeSharp;

namespace FunkyBot.Cache.Objects
{
	public class CacheInteractable : CacheGizmo
	{
		public CacheInteractable(CacheObject baseobj)
			: base(baseobj)
		{
		}

		public bool IsEventSwitch 
		{ 
			get 
			{ 
				return Gizmotype.HasValue && Gizmotype.Value == GizmoType.Switch && internalNameLower.Contains("event_switch"); 
			}
		}

		public override string DebugString
		{
			get
			{
				return String.Format("{0}\r\n PhysSNO={1} HandleAsObstacle={2} Operated={3}",
					base.DebugString, PhysicsSNO.HasValue ? PhysicsSNO.Value.ToString() : "NULL",
					HandleAsObstacle.HasValue ? HandleAsObstacle.Value.ToString() : "NULL",
					GizmoHasBeenUsed.HasValue ? GizmoHasBeenUsed.Value.ToString() : "NULL");
			}
		}
		private double LootRadius
		{
			get
			{
				if (targetType.Value == TargetType.Shrine)
				{
					if (Gizmotype == GizmoType.PoolOfReflection)
						return Bot.Settings.Ranges.PoolsOfReflectionRange;

					return IsHealthWell ? Bot.Settings.Ranges.ShrineRange * 2 : Bot.Settings.Ranges.ShrineRange;
				}

				if (targetType.Value == TargetType.Container)
				{
					if (IsResplendantChest && Bot.Settings.Targeting.UseExtendedRangeRepChest)
						return Bot.Settings.Ranges.ContainerOpenRange * 2;
					else
						return Bot.Settings.Ranges.ContainerOpenRange;
				}

				if (targetType.Value == TargetType.CursedShrine)
				{
					return Bot.Settings.Ranges.CursedShrineRange;
				}
				if (targetType.Value == TargetType.CursedChest)
					return Bot.Settings.Ranges.CursedChestRange;

				return Bot.Targeting.Cache.iCurrentMaxLootRadius;
			}
		}

		public override bool ObjectIsValidForTargeting
		{
			get
			{
				if (!base.ObjectIsValidForTargeting) return false;
				if (!targetType.HasValue) return false;

				//Disabled by script?
				if (GizmoDisabledByScript.HasValue && GizmoDisabledByScript.Value) return false;


				float centreDistance = CentreDistance;
				float radiusDistance = RadiusDistance;

				// Ignore it if it's not in range yet
				if (centreDistance > LootRadius)
				{
					BlacklistLoops = 10;
					return false;
				}

				if (GizmoHasBeenUsed.HasValue && GizmoHasBeenUsed.Value)
				{
					if ((targetType.Value == TargetType.CursedShrine || targetType.Value == TargetType.CursedChest) && IsStillValid())
					{//Cursed Shrine/Chest that is still valid we reset to keep active!
						Bot.Targeting.Cache.lastSeenCursedShrine = DateTime.Now;
						LoopsUnseen = 0;
					}
					return false;
				}

				if (RequiresLOSCheck && !IgnoresLOSCheck)
				{
					//Get the wait time since last used LOSTest
					double lastLOSCheckMS = base.LineOfSight.LastLOSCheckMS;

					//unless its in front of us.. we wait 500ms mandatory.
					if (lastLOSCheckMS < 500 && centreDistance > 1f)
					{
						if ((IsResplendantChest && Bot.Settings.LOSMovement.AllowRareLootContainer)||
							((IsCursedChest||IsCursedShrine) && Bot.Settings.LOSMovement.AllowCursedChestShrines)||
							(IsEventSwitch && Bot.Settings.LOSMovement.AllowEventSwitches))
						{
							//if (Bot.Settings.Debug.FunkyLogFlags.HasFlag(LogLevel.Target))
							//	Logger.Write(LogLevel.Target, "Adding {0} to LOS Movement Objects", InternalName);
							Bot.Targeting.Cache.Environment.LoSMovementObjects.Add(this);
						}
						return false;
					}
					else
					{
						//Set the maximum wait time
						double ReCheckTime = 3500;

						//Close Range.. we change the recheck timer based on how close
						if (CentreDistance < 25f)
							ReCheckTime = (CentreDistance * 125);
						else if (ObjectIsSpecial)
							ReCheckTime *= 0.25;

						if (lastLOSCheckMS < ReCheckTime)
						{
							if ((IsResplendantChest && Bot.Settings.LOSMovement.AllowRareLootContainer) ||
								((IsCursedChest || IsCursedShrine) && Bot.Settings.LOSMovement.AllowCursedChestShrines) ||
								 (IsEventSwitch && Bot.Settings.LOSMovement.AllowEventSwitches))
							{
								//if (Bot.Settings.Debug.FunkyLogFlags.HasFlag(LogLevel.Target))
								//   Logger.Write(LogLevel.Target, "Adding {0} to LOS Movement Objects", InternalName);
								Bot.Targeting.Cache.Environment.LoSMovementObjects.Add(this);
							}
							return false;
						}
					}

					if (!base.LineOfSight.LOSTest(Bot.Character.Data.Position, true, false))
					{
						if ((IsResplendantChest && Bot.Settings.LOSMovement.AllowRareLootContainer) ||
							((IsCursedChest || IsCursedShrine) && Bot.Settings.LOSMovement.AllowCursedChestShrines) ||
							(IsEventSwitch && Bot.Settings.LOSMovement.AllowEventSwitches))
						{
							//if (Bot.Settings.Debug.FunkyLogFlags.HasFlag(LogLevel.Target))
							//	Logger.Write(LogLevel.Target, "Adding {0} to LOS Movement Objects", InternalName);
							Bot.Targeting.Cache.Environment.LoSMovementObjects.Add(this);
						}
						return false;
					}

					RequiresLOSCheck = false;
				}



				// Now for the specifics
				switch (targetType.Value)
				{
					#region Interactable
					case TargetType.Interactable:
					case TargetType.Door:


						if (targetType.Value == TargetType.Door
							   && PriorityCounter == 0
							   && radiusDistance > 5f)
						{
							Vector3 BarricadeTest = Position;
							if (radiusDistance > 1f)
							{

								BarricadeTest = MathEx.GetPointAt(Position, 10f, Navigation.FindDirection(Bot.Character.Data.Position, Position, true));
								bool intersectionTest = !MathEx.IntersectsPath(Position, CollisionRadius.Value, Bot.Character.Data.Position, BarricadeTest);
								if (!intersectionTest)
								{
									return false;
								}

							}
						}

						if (centreDistance > 30f)
						{
							BlacklistLoops = 3;
							return false;
						}

						return true;
					#endregion
					#region Shrine
					case TargetType.Shrine:


						bool IgnoreThis = false;
						if (IsHealthWell)
						{
							//Health wells..
							if (Bot.Character.Data.dCurrentHealthPct > Bot.Settings.Combat.HealthWellHealthPercent)
							{
								BlacklistLoops = 5;
								return false;
							}
						}
						else if(Gizmotype== GizmoType.PoolOfReflection)
						{

						}
						else
						{
							ShrineTypes shrinetype = CacheIDLookup.FindShrineType(SNOID);

							//Ignore XP Shrines at MAX Paragon Level!
							//if (this.SNOID==176075&&Bot.Character_.Data.iMyParagonLevel==100)
							IgnoreThis = !Bot.Settings.Targeting.UseShrineTypes[(int)shrinetype];
						}

						//Ignoring..?
						if (IgnoreThis)
						{
							NeedsRemoved = true;
							BlacklistFlag = BlacklistType.Permanent;
							return false;
						}

						// Bag it!
						Radius = 5.1f;
						break;
					#endregion
					#region Container
					case TargetType.Container:

						//Vendor Run and DB Settings check
						if (TownRunManager.bWantToTownRun
							 || !IsChestContainer && !CharacterSettings.Instance.OpenLootContainers
							 || IsChestContainer && !CharacterSettings.Instance.OpenChests)
						{
							BlacklistLoops = 25;
							return false;
						}

						if (IsCorpseContainer && Bot.Settings.Targeting.IgnoreCorpses)
						{
							NeedsRemoved = true;
							BlacklistFlag = BlacklistType.Permanent;
							return false;
						}


						// Any physics mesh? Give a minimum distance of 5 feet
						//if (PhysicsSNO.HasValue && PhysicsSNO <= 0)
						//{
						//	NeedsRemoved = true;
						//	return false;
						//}


						// Superlist for rare chests etc.
						if (IsResplendantChest)
						{
							//setup wait time. (Unlike Units, we blacklist right after we interact)
							if (Bot.Targeting.Cache.LastCachedTarget.Equals(this))
							{
								Bot.Targeting.Cache.lastHadContainerAsTarget = DateTime.Now;
								Bot.Targeting.Cache.lastHadRareChestAsTarget = DateTime.Now;
							}
						}

						// Bag it!
						if (IsChestContainer)
							Radius = 5.1f;
						else
							Radius = 4f;

						break;
					#endregion
				} // Object switch on type (to seperate shrines, destructibles, barricades etc.)




				return true;

			}
		}

		public override void UpdateWeight()
		{
			base.UpdateWeight();

			if (CentreDistance >= 4f && Bot.Targeting.Cache.Environment.NearbyAvoidances.Count > 0)
			{
				Vector3 TestPosition = Position;
				if (ObjectCache.Obstacles.IsPositionWithinAvoidanceArea(TestPosition))
					Weight = 1;
				else if (ObjectCache.Obstacles.TestVectorAgainstAvoidanceZones(TestPosition)) //intersecting avoidances..
					Weight = 1;
			}

			if (Weight != 1)
			{
				float centreDistance = CentreDistance;
				Vector3 BotPosition = Bot.Character.Data.Position;
				switch (targetType.Value)
				{
					case TargetType.Shrine:
						Weight = 14500d - (Math.Floor(centreDistance) * 170d);
						// Very close shrines get a weight increase
						if (centreDistance <= 20f)
							Weight += 1000d;

						// health pool
						if (base.IsHealthWell)
						{
							if (Bot.Character.Data.dCurrentHealthPct > 0.75d)
								Weight = 0;
							else
								//Give weight based upon current health percent.
								Weight += 1000d / (Bot.Character.Data.dCurrentHealthPct);
						}
						else if (this.Gizmotype.Value == GizmoType.PoolOfReflection)
						{
							Weight += 1000;
						}

						if (Weight > 0)
						{
							// Was already a target and is still viable, give it some free extra weight, to help stop flip-flopping between two targets
							if (this == Bot.Targeting.Cache.LastCachedTarget)
								Weight += 600;
							// Are we prioritizing close-range stuff atm? If so limit it at a value 3k lower than monster close-range priority
							if (Bot.Character.Data.bIsRooted)
								Weight = 18500d - (Math.Floor(centreDistance) * 200);
							// If there's a monster in the path-line to the item, reduce the weight by 25%
							if (ObjectCache.Obstacles.Monsters.Any(cp => cp.TestIntersection(this, BotPosition)))
								Weight *= 0.75;
						}
						break;
					case TargetType.Interactable:
					case TargetType.Door:
						Weight = 15000d - (Math.Floor(centreDistance) * 170d);
						if (centreDistance <= 20f && RadiusDistance <= 5f)
							Weight += 8000d;
						// Was already a target and is still viable, give it some free extra weight, to help stop flip-flopping between two targets
						if (this == Bot.Targeting.Cache.LastCachedTarget && centreDistance <= 25f)
							Weight += 400;
						// If there's a monster in the path-line to the item, reduce the weight by 50%
						//if (ObjectCache.Obstacles.Monsters.Any(cp => cp.TestIntersection(this, BotPosition)))
						//    this.Weight*=0.5;
						break;
					case TargetType.Container:
						Weight = 11000d - (Math.Floor(centreDistance) * 190d);
						if (centreDistance <= 12f)
							Weight += 600d;
						// Was already a target and is still viable, give it some free extra weight, to help stop flip-flopping between two targets
						if (this == Bot.Targeting.Cache.LastCachedTarget && centreDistance <= 25f)
						{
							Weight += 400;
						}
						// If there's a monster in the path-line to the item, reduce the weight by 50%
						if (ObjectCache.Obstacles.Monsters.Any(cp => cp.TestIntersection(this, BotPosition)))
						{
							Weight *= 0.5;
						}
						if (IsResplendantChest)
							Weight += 1500;

						break;
					case TargetType.CursedShrine:
					case TargetType.CursedChest:
						Weight = 11000d - (Math.Floor(centreDistance) * 190d);
						break;
				}
			}
			else
			{
				Weight = 0;
				BlacklistLoops = 15;
			}
		}

		public override bool IsZDifferenceValid
		{
			get
			{
				float fThisHeightDifference = Funky.Difference(Bot.Character.Data.Position.Z, Position.Z);
				if (fThisHeightDifference >= 10f)
				{
					return false;

				}
				return base.IsZDifferenceValid;
			}
		}

		public override bool ObjectShouldBeRecreated
		{
			get
			{
				return false;
			}
		}

		public override RunStatus Interact()
		{
			// Force waiting for global cooldown timer or long-animation abilities
			if (Bot.Character.Class.PowerPrime.WaitLoopsBefore >= 1)
			{
				//Logger.DBLog.DebugFormat("Debug: Force waiting BEFORE Ability " + powerPrime.powerThis.ToString() + "...");
				Bot.Targeting.Cache.bWaitingForPower = true;
				if (Bot.Character.Class.PowerPrime.WaitLoopsBefore >= 1)
					Bot.Character.Class.PowerPrime.WaitLoopsBefore--;
				return RunStatus.Running;
			}
			Bot.Targeting.Cache.bWaitingForPower = false;


			Bot.Character.Data.WaitWhileAnimating(20);
			ZetaDia.Me.UsePower(SNOPower.Axe_Operate_Gizmo, Position, Bot.Character.Data.iCurrentWorldID, base.AcdGuid.Value);
			InteractionAttempts++;

			if (IsCursedShrine || IsCursedChest)
			{
				Bot.Targeting.Cache.lastSeenCursedShrine = DateTime.Now;
			}

			if (InteractionAttempts == 1)
			{
				Bot.Targeting.Cache.bWaitingAfterPower = true;
			}


			// Wait!!
			if (ObjectCache.CheckTargetTypeFlag(targetType.Value, TargetType.Interactable))
			{
				Bot.Character.Data.WaitWhileAnimating(1500);
			}
			else if(ObjectCache.CheckTargetTypeFlag(targetType.Value,TargetType.Door))
			{
				Bot.Character.Data.WaitWhileAnimating(500);
			}
			else
			{
				Bot.Character.Data.WaitWhileAnimating(75);
			}
			

			// If we've tried interacting too many times, blacklist this for a while
			if (InteractionAttempts > 5 && !IsCursedShrine && !IsCursedChest)
			{
				if (Gizmotype == GizmoType.Door)
				{
					BlacklistLoops = 20;
				}
				else
				{
					BlacklistLoops = 50;
				}
				
				InteractionAttempts = 0;
			}

			if (!Bot.Targeting.Cache.bWaitingAfterPower)
			{
				// Now tell Trinity to get a new target!
				Bot.NavigationCache.lastChangedZigZag = DateTime.Today;
				Bot.NavigationCache.vPositionLastZigZagCheck = Vector3.Zero;
				Bot.Targeting.Cache.bForceTargetUpdate = true;
			}
			return RunStatus.Running;
		}

		public override bool WithinInteractionRange()
		{
			float fRangeRequired = 0f;
			float fDistanceReduction = 0f;

			if (targetType.Value == TargetType.Interactable)
			{
				// Treat the distance as closer based on the radius of the object
				//fDistanceReduction=ObjectData.Radius;
				fRangeRequired = CollisionRadius.Value * 0.75f;

				// Check if it's in our interactable range dictionary or not
				int iTempRange;
				if (CacheIDLookup.dictInteractableRange.TryGetValue(SNOID, out iTempRange))
				{
					fRangeRequired = (float)iTempRange;
				}
				// Treat the distance as closer if the X & Y distance are almost point-blank, for objects
				if (RadiusDistance <= 2f)
					fDistanceReduction += 1f;

				base.DistanceFromTarget = Vector3.Distance(Bot.Character.Data.Position, Position) - fDistanceReduction;

			}
			else
			{
				fDistanceReduction = (Radius * 0.33f);
				fRangeRequired = 8f;


				if (Bot.Character.Data.Position.Distance(Position) <= 1.5f)
					fDistanceReduction += 1f;

				base.DistanceFromTarget = base.RadiusDistance - fDistanceReduction;

			}



			return (fRangeRequired <= 0f || base.DistanceFromTarget <= fRangeRequired);
		}

		public override bool ObjectIsSpecial
		{
			get
			{
				//Rep Chests
				return IsResplendantChest && Bot.Settings.Targeting.UseExtendedRangeRepChest;
			}
		}
	}
}