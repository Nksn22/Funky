﻿using System;
using fBaseXtensions.Cache.Internal;
using fBaseXtensions.Cache.Internal.Enums;
using fBaseXtensions.Cache.Internal.Objects;
using fBaseXtensions.Game;
using fBaseXtensions.Game.Hero;
using fBaseXtensions.Game.Hero.Skills;
using fBaseXtensions.Game.Hero.Skills.Conditions;
using fBaseXtensions.Navigation;
using Zeta.Bot;
using Zeta.Bot.Navigation;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Zeta.TreeSharp;
using Logger = fBaseXtensions.Helpers.Logger;
using LogLevel = fBaseXtensions.Helpers.LogLevel;

namespace fBaseXtensions.Targeting
{

	public class TargetMovement
	{
		//TargetMovement -- Used during Target Handling to move the bot into Interaction Range.

		public TargetMovement()
		{
			LastPlayerLocation = Vector3.Zero;
			BlockedMovementCounter = 0;
			NonMovementCounter = 0;
			LastMovementDuringCombat = DateTime.Today;
			LastMovementAttempted = DateTime.Today;
			LastMovementCommand = DateTime.Today;
			IsAlreadyMoving = false;
			LastTargetLocation = Vector3.Zero;
			CurrentTargetLocation = Vector3.Zero;

			//Add handler for position update
			FunkyGame.Hero.OnPositionChanged += positionChangedHandler;
		}

		internal string DebugString()
		{
			return String.Format("== Movement ==\r\n" +
			                     "LastPlayerPOS: {0} \r\n " +
			                     "LastTargetPOS: {1} \r\n " +
			                     "CurrentTargetPOS: {2} \r\n" +
								 "BlockedMovementCount: {3} --- NonMovementCount: {4} \r\n" +
								 "IsAlreadyMoving: {5} \r\n" +
								 "LastMovementDuringCombat: {6} \r\n" +
								 "LastMovementAttempted: {7} \r\n" +
								 "LastMovementCommand: {8}",
								 LastPlayerLocation.ToString(), 
								 LastTargetLocation.ToString(), 
								 CurrentTargetLocation.ToString(), 
								 BlockedMovementCounter, 
								 NonMovementCounter,
								 IsAlreadyMoving,
								 LastMovementDuringCombat.ToString(),
								 LastMovementAttempted.ToString(),
								 LastMovementCommand.ToString());
		}

		private void positionChangedHandler(Vector3 position)
		{
			lastPositionChange = DateTime.Now;
		}
		internal void OnTargetChanged(object cacheobj, TargetingCache.TargetChangedArgs args)
		{
			NewTargetResetVars();

		}

		internal int BlockedMovementCounter = 0;
		internal int NonMovementCounter = 0;
		internal DateTime LastMovementDuringCombat = DateTime.Today;
		internal Vector3 CurrentTargetLocation = Vector3.Zero;
		internal DateTime LastMovementAttempted = DateTime.Today;

		private DateTime LastMovementCommand = DateTime.Today;
		private Vector3 LastTargetLocation = Vector3.Zero;
		private DateTime lastPositionChange = DateTime.Today;
		private Vector3 LastPlayerLocation = Vector3.Zero;
		private bool IsAlreadyMoving;


		internal RunStatus TargetMoveTo(CacheObject obj)
		{

			#region DebugInfo
			if (FunkyBaseExtension.Settings.Debugging.DebugStatusBar)
			{
				string Action = "[Move-";
				switch (obj.targetType.Value)
				{
					case TargetType.Avoidance:
						Action += "Avoid] ";
						break;
					case TargetType.Fleeing:
						Action += "Flee] ";
						break;

					case TargetType.Backtrack:
						Action += "BackTrack] ";
						break;

					case TargetType.LineOfSight:
						Action += "LOS] ";
						break;

					case TargetType.Unit:
						if (FunkyGame.Navigation.groupRunningBehavior && FunkyGame.Navigation.groupingCurrentUnit != null && FunkyGame.Navigation.groupingCurrentUnit == obj)
							Action += "Grouping] ";
						else
							Action += "Combat] ";

						break;
					case TargetType.Item:
					case TargetType.Gold:
					case TargetType.Globe:
						Action += "Pickup] ";
						break;
					case TargetType.Interactable:
						Action += "Interact] ";
						break;
					case TargetType.Container:
						Action += "Open] ";
						break;
					case TargetType.Destructible:
					case TargetType.Barricade:
						Action += "Destroy] ";
						break;
					case TargetType.Shrine:
						Action += "Click] ";
						break;
				}
				FunkyGame.sStatusText = Action + " ";

				FunkyGame.sStatusText += "Target=" + FunkyGame.Targeting.Cache.CurrentTarget.InternalName + " C-Dist=" + Math.Round(FunkyGame.Targeting.Cache.CurrentTarget.CentreDistance, 2) + ". " +
					 "R-Dist=" + Math.Round(FunkyGame.Targeting.Cache.CurrentTarget.RadiusDistance, 2) + ". ";

				if (FunkyGame.Targeting.Cache.CurrentTarget.targetType.Value == TargetType.Unit && FunkyGame.Hero.Class.PowerPrime.Power != SNOPower.None)
					FunkyGame.sStatusText += "Power=" + FunkyGame.Hero.Class.PowerPrime.Power + " (range " + FunkyGame.Hero.Class.PowerPrime.MinimumRange + ") ";

				FunkyGame.sStatusText += "Weight=" + FunkyGame.Targeting.Cache.CurrentTarget.Weight;
				BotMain.StatusText = FunkyGame.sStatusText;
				FunkyGame.bResetStatusText = true;
			}
			#endregion

			// Are we currently incapacitated? If so then wait...
			if (FunkyGame.Hero.bIsIncapacitated || FunkyGame.Hero.bIsRooted)
				return RunStatus.Running;

			//Ignore skip ahead cache for LOS movements..
			if (FunkyBaseExtension.Settings.Debugging.SkipAhead && obj.targetType.Value != TargetType.LineOfSight)
				SkipAheadCache.RecordSkipAheadCachePoint();
			
			// Some stuff to avoid spamming usepower EVERY loop, and also to detect stucks/staying in one place for too long
			bool bForceNewMovement = false;

			//Herbfunk: Added this to prevent stucks attempting to move to a target blocked. (Case: 3 champs behind a wall, within range but could not engage due to being on the other side.)
			#region Non Movement Counter Reached
			if (NonMovementCounter > FunkyBaseExtension.Settings.Plugin.MovementNonMovementCount)
			{
				Logger.Write(LogLevel.Movement, "non movement counter reached {0}", NonMovementCounter);

				if (obj.Actortype.HasValue && obj.Actortype.Value.HasFlag(ActorType.Item))
				{
					if (NonMovementCounter > 250)
					{
						//Are we stuck?
						if (!Navigation.Navigation.MGP.CanStandAt(FunkyGame.Hero.Position))
						{
							Logger.DBLog.InfoFormat("Character is stuck inside non-standable location.. attempting townportal cast..");
							ZetaDia.Me.UseTownPortal();
							NonMovementCounter = 0;
							return RunStatus.Running;
						}
					}


					//Check if we can walk to this location from current location..
					if (!Navigation.Navigation.CanRayCast(FunkyGame.Hero.Position, CurrentTargetLocation, UseSearchGridProvider: true))
					{
						obj.RequiresLOSCheck = true;
						obj.BlacklistLoops = 50;

						Logger.Write(LogLevel.Movement, "Ignoring Item {0} -- due to RayCast Failure!", obj.InternalName);

						FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
						return RunStatus.Running;
					}
				}
				else if (ObjectCache.CheckFlag(obj.targetType.Value, TargetType.LineOfSight | TargetType.Backtrack))
				{

					Logger.Write(LogLevel.LineOfSight, "Line of Sight Movement Stalled!");

					//FunkyGame.Navigation.LOSmovementObject = null;
					Navigation.Navigation.NP.Clear();
					FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
					NonMovementCounter = 0;
					// Reset the emergency loop counter and return success
					return RunStatus.Running;
				}
				else
				{
					Logger.Write(LogLevel.Movement, "Ignoring obj {0} ", obj.InternalName + " _ SNO:" + obj.SNOID);
					obj.BlacklistLoops = 50;
					obj.RequiresLOSCheck = true;
					FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
					NonMovementCounter = 0;

					// Reset the emergency loop counter and return success
					return RunStatus.Running;
				}
			}
			#endregion

			//Do a priority check for nearby obstacle objects.
			FunkyGame.Navigation.ObstaclePrioritizeCheck(15f);

			#region Evaluate Last Action

			// Position didn't change last update.. check if we are stuck!
			if (DateTime.Now.Subtract(lastPositionChange).TotalMilliseconds > 150 &&
				(!FunkyGame.Hero.IsMoving || FunkyGame.Hero.currentMovementState == MovementState.WalkingInPlace || FunkyGame.Hero.currentMovementState.Equals(MovementState.None)))
			{
				bForceNewMovement = true;
				if (DateTime.Now.Subtract(LastMovementDuringCombat).TotalMilliseconds >= 250)
				{
					LastMovementDuringCombat = DateTime.Now;
					BlockedMovementCounter++;

					// Tell target finder to prioritize close-combat targets incase we were bodyblocked
					#region TargetingPriortize
					switch (BlockedMovementCounter)
					{
						case 2:
						case 3:
							if (FunkyGame.Navigation.groupRunningBehavior)
							{
								Logger.Write(LogLevel.Movement, "Grouping Behavior stopped due to blocking counter");

								FunkyGame.Navigation.GroupingFinishBehavior();
								FunkyGame.Navigation.groupingSuspendedDate = DateTime.Now.AddMilliseconds(4000);
								FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
								return RunStatus.Running;
							}

							if (!ObjectCache.CheckFlag(obj.targetType.Value, TargetType.AvoidanceMovements))
							{
								//Finally try raycasting to see if navigation is possible..
								if (obj.Actortype.HasValue &&
									 (obj.Actortype.Value == ActorType.Gizmo || obj.Actortype.Value == ActorType.Monster))
								{
									Vector3 hitTest;
									// No raycast available, try and force-ignore this for a little while, and blacklist for a few seconds
									if (Navigator.Raycast(FunkyGame.Hero.Position, obj.Position, out hitTest))
									{
										if (hitTest != Vector3.Zero)
										{
											obj.RequiresLOSCheck = true;
											obj.BlacklistLoops = 10;
											Logger.Write(LogLevel.Movement, "Ignoring object " + obj.InternalName + " due to not moving and raycast failure!", true);

											FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
											return RunStatus.Running;
										}
									}
								}
								else if (obj.targetType.Value == TargetType.Item)
								{
									obj.BlacklistLoops = 10;
									FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
								}
							}
							else
							{
								if (!Navigation.Navigation.CanRayCast(FunkyGame.Hero.Position, CurrentTargetLocation, NavCellFlags.AllowWalk))
								{
									Logger.Write(LogLevel.Movement, "Cannot continue with avoidance movement due to raycast failure!");
									BlockedMovementCounter = 0;

									FunkyGame.Navigation.CurrentGPArea.BlacklistLastSafespot();
									FunkyGame.Navigation.vlastSafeSpot = Vector3.Zero;
									FunkyGame.Targeting.Cache.bForceTargetUpdate = true;
									return RunStatus.Running;
								}
							}
							break;
					}
					#endregion

					return RunStatus.Running;
				}
			}
			else
			{
				// Movement has been made, so count the time last moved!
				LastMovementDuringCombat = DateTime.Now;
			}
			#endregion

			// See if we want to ACTUALLY move, or are just waiting for the last move command...
			if (!bForceNewMovement && IsAlreadyMoving && CurrentTargetLocation == LastTargetLocation && DateTime.Now.Subtract(LastMovementCommand).TotalMilliseconds <= 100)
			{
				return RunStatus.Running;
			}

			// If we're doing avoidance, globes or backtracking, try to use special abilities to move quicker
			#region SpecialMovementChecks
			if (ObjectCache.CheckFlag(obj.targetType.Value, FunkyBaseExtension.Settings.Combat.CombatMovementTargetTypes))
			{
				Skill MovementPower;
				Vector3 MovementVector = FunkyGame.Hero.Class.FindCombatMovementPower(out MovementPower, obj.Position);
				if (MovementVector != Vector3.Zero)
				{
					ZetaDia.Me.UsePower(MovementPower.Power, MovementVector, FunkyGame.Hero.CurrentWorldDynamicID, -1);
					MovementPower.OnSuccessfullyUsed();

					// Store the current destination for comparison incase of changes next loop
					LastTargetLocation = CurrentTargetLocation;
					// Reset total body-block count, since we should have moved
					//if (DateTime.Now.Subtract(FunkyGame.Targeting.Cache.Environment.lastForcedKeepCloseRange).TotalMilliseconds>=2000)
					BlockedMovementCounter = 0;

					FunkyGame.Hero.Class.PowerPrime.WaitLoopsBefore = 5;

					return RunStatus.Running;
				}

				//Special Whirlwind Code
				if (FunkyGame.Hero.Class.AC == ActorClass.Barbarian && Hotbar.HasPower(SNOPower.Barbarian_Whirlwind))
				{
					// Whirlwind against everything within range (except backtrack points)
					if (FunkyGame.Hero.dCurrentEnergy >= 10
						 && FunkyGame.Targeting.Cache.Environment.iAnythingWithinRange[(int)RangeIntervals.Range_20] >= 1
						 && obj.DistanceFromTarget <= 12f
						 && (!Hotbar.HasPower(SNOPower.Barbarian_Sprint) || Hotbar.HasBuff(SNOPower.Barbarian_Sprint))
						 && (ObjectCache.CheckFlag(obj.targetType.Value, TargetType.AvoidanceMovements | TargetType.Gold | TargetType.Globe) == false)
						 && (obj.targetType.Value != TargetType.Unit
						 || (obj.targetType.Value == TargetType.Unit && !obj.IsTreasureGoblin
							  && (!FunkyBaseExtension.Settings.Barbarian.bSelectiveWhirlwind
									|| FunkyGame.Targeting.Cache.Environment.bAnyNonWWIgnoreMobsInRange
									|| !CacheIDLookup.hashActorSNOWhirlwindIgnore.Contains(obj.SNOID)))))
					{
						// Special code to prevent whirlwind double-spam, this helps save fury
						bool bUseThisLoop = SNOPower.Barbarian_Whirlwind != FunkyGame.Hero.Class.LastUsedAbility.Power;
						if (!bUseThisLoop)
						{
							if (FunkyGame.Hero.Class.Abilities[SNOPower.Barbarian_Whirlwind].LastUsedMilliseconds >= 200)
								bUseThisLoop = true;
						}
						if (bUseThisLoop)
						{
							ZetaDia.Me.UsePower(SNOPower.Barbarian_Whirlwind, CurrentTargetLocation, FunkyGame.Hero.CurrentWorldDynamicID);
							FunkyGame.Hero.Class.Abilities[SNOPower.Barbarian_Whirlwind].OnSuccessfullyUsed();
						}
						// Store the current destination for comparison incase of changes next loop
						LastTargetLocation = CurrentTargetLocation;
						BlockedMovementCounter = 0;
						return RunStatus.Running;
					}
				}
			}
			#endregion


			//Special Movement Check for Steed Charge and Spirit Walk
			if (FunkyGame.Hero.Class.LastUsedAbility.IsSpecialMovementSkill && FunkyGame.Hero.Class.HasSpecialMovementBuff() && ObjectCache.CheckFlag(obj.targetType.Value, TargetType.Unit))
			{
				//Logger.DBLog.DebugFormat("Preforming ZigZag for special movement skill activation!");
				FunkyGame.Navigation.vPositionLastZigZagCheck = FunkyGame.Hero.Position;
				if (FunkyGame.Hero.Class.ShouldGenerateNewZigZagPath()) FunkyGame.Hero.Class.GenerateNewZigZagPath();
				CurrentTargetLocation = FunkyGame.Navigation.vSideToSideTarget;
			}


			// Now for the actual movement request stuff
			IsAlreadyMoving = true;
			UseTargetMovement(obj, bForceNewMovement);

			// Store the current destination for comparison incase of changes next loop
			LastMovementAttempted = DateTime.Now;
			LastTargetLocation = CurrentTargetLocation;

			//Check if we moved at least 5f..
			if (LastPlayerLocation.Distance(FunkyGame.Hero.Position) <= 5f)
				NonMovementCounter++;
			else
			{
				NonMovementCounter = 0;
				BlockedMovementCounter = 0;
			}

			//store player location
			LastPlayerLocation = FunkyGame.Hero.Position;

			return RunStatus.Running;
		}

		private void UseTargetMovement(CacheObject obj, bool bForceNewMovement = false)
		{
			float currentDistance = Vector3.Distance(LastTargetLocation, CurrentTargetLocation);
			if (DateTime.Now.Subtract(LastMovementAttempted).TotalMilliseconds >= 250 || (currentDistance >= 2f && !FunkyGame.Hero.IsMoving) || bForceNewMovement)
			{
				bool UsePowerMovement = true;

				//Check for any circumstances where we use actor movement instead of power. (click or click-hold)
				if (ObjectCache.CheckFlag(obj.targetType.Value, TargetType.AvoidanceMovements))
				{
					if (NonMovementCounter < 10 || currentDistance > 50f)
						UsePowerMovement = false;
				}
				else if (ObjectCache.CheckFlag(obj.targetType.Value, TargetType.LineOfSight | TargetType.Backtrack))
				{
					UsePowerMovement = true;
				}
				else if (ObjectCache.CheckFlag(obj.targetType.Value, TargetType.LineOfSight))
				{
					Navigator.MoveTo(CurrentTargetLocation, "LOS");
					LastMovementCommand = DateTime.Now;
					return;
				}
				else
				{
					//Use Walk Power when not using LOS Movement, target is not an item and target does not ignore LOS.
					if (!(NonMovementCounter < 10 &&
						!obj.UsingLOSV3 &&
						!obj.IgnoresLOSCheck &&
						 (obj.targetType.Value != TargetType.Item) &&
						 obj.targetType.Value != TargetType.Interactable))
					{
						UsePowerMovement = true;
					}
				}

				//Preform Movement!
				if (!UsePowerMovement)
					ZetaDia.Me.Movement.MoveActor(CurrentTargetLocation);
				else
					ZetaDia.Me.UsePower(SNOPower.Walk, CurrentTargetLocation, FunkyGame.Hero.CurrentWorldDynamicID);

				

				//and record when we sent the movement..
				LastMovementCommand = DateTime.Now;
			}
		}

		internal void RestartTracking()
		{
			FunkyGame.Hero.OnPositionChanged += positionChangedHandler;
			lastPositionChange = DateTime.Now;
		}
		internal void ResetTargetMovementVars()
		{
			FunkyGame.Hero.OnPositionChanged -= positionChangedHandler;
			BlockedMovementCounter = 0;
			NonMovementCounter = 0;
			NewTargetResetVars();
		}
		internal void NewTargetResetVars()
		{
			IsAlreadyMoving = false;
			LastMovementCommand = DateTime.Today;
		}
	}

}