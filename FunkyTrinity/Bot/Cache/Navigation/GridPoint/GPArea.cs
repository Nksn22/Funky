﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zeta;
using Zeta.Common;

namespace FunkyTrinity.Movement
{

		  //Redesign how we use the GPCache by creating it into a instance which contains the entrie area we conduct a search in.


		  public class GPArea
		  {
				//ToDo: Track object IDs so we can add them to the appropriate GPRect to use for updating.

				//GPArea -- a collection of GPRects which are connected, describes the entire location, holds each point indexed for easier access
				private List<GPRectangle> gridpointrectangles_;
				internal bool AllGPRectsFailed=false;
				internal GPRectangle centerGPRect;
				internal GPRectangle lastUsedGPRect=null;

				public GPArea(Vector3 startingLocation)
				{
					 //Creation and Cache base
					 centerGPRect=new GPRectangle(startingLocation, 3);
					 GPRectangle centerClone=centerGPRect.Clone();

					 //Get all valid points (besides current point) from our current location GPR
					 GridPoint[] SearchPoints=centerGPRect.Points.Keys.Where(gp => !gp.Ignored).ToArray();
					 gridpointrectangles_=new List<GPRectangle>();
					 if (SearchPoints.Length>1)
					 {								//we should check our surrounding points to see if we can even move into any of them first!
						  for (int i=1; i<SearchPoints.Length-1; i++)
						  {
								GridPoint curGP=SearchPoints[i];
								Vector3 thisV3=(Vector3)curGP;
								thisV3.Z+=(Bot.Character.fCharacterRadius/2f);

								//Its a valid point for direction testing!
								float DirectionDegrees=Navigation.FindDirection(Bot.NavigationCache.LastSearchVector, thisV3, false);
								DirectionPoint P=new DirectionPoint((Vector3)curGP, DirectionDegrees, 125f);

								if (P.Range>5f)
								{
									 gridpointrectangles_.Add(new GPRectangle(P, centerGPRect));
								}
						  }
						  gridpointrectangles_.Add(centerClone);
						  gridpointrectangles_=gridpointrectangles_.OrderByDescending(gpr => gpr.Count).ToList();
					 }
				}

				public bool GridPointContained(GridPoint point)
				{
					 return centerGPRect.Contains(point);
				}

				///<summary>
				///Searches for a safespot!
				///</summary>
				public Vector3 AttemptFindSafeSpot(Vector3 CurrentPosition, Vector3 LOS, bool kiting=false)
				{
					 if (AllGPRectsFailed&&Bot.NavigationCache.BlacklistedGridpoints.Count>0)
					 {
						  //Reset all blacklist to retry again.
						  AllGPRectsFailed=false;
						  //Clear Blacklisted
						  Bot.NavigationCache.BlacklistedGridpoints.Clear();
					 }

					 Vector3 safespot=Vector3.Zero;
					 //Check if we actually created any surrounding GPCs..
					 if (gridpointrectangles_.Count>0)
					 {
						  iterateGPRectsSafeSpot(CurrentPosition, out safespot, LOS, kiting);
						  //If still failed to find a safe spot.. set the timer before we try again.
						  if (safespot==Vector3.Zero)
						  {
								if (Bot.SettingsFunky.LogSafeMovementOutput)
									 Logging.WriteVerbose("All GPCs failed to find a valid location to move!");

								AllGPRectsFailed=true;

								//Set timer here
								if (!kiting)
								{
									 Bot.Combat.iMillisecondsCancelledEmergencyMoveFor=(int)(Bot.Character.dCurrentHealthPct*Bot.SettingsFunky.AvoidanceRecheckMinimumRate)+1000;
									 Bot.Combat.timeCancelledEmergencyMove=DateTime.Now;
								}
								else
								{
									 Bot.Combat.iMillisecondsCancelledFleeMoveFor=(int)(Bot.Character.dCurrentHealthPct*Bot.SettingsFunky.KitingRecheckMinimumRate)+1000;
									 Bot.Combat.timeCancelledFleeMove=DateTime.Now;
								}
								return safespot;
						  }
						  else
						  {
								//Cache it and set timer
								Bot.NavigationCache.lastFoundSafeSpot=DateTime.Now;
								Bot.NavigationCache.vlastSafeSpot=safespot;
						  }
					 }
					 //Logging.WriteVerbose("Safespot location {0} distance from {1} is {2}", safespot.ToString(), LastSearchVector.ToString(), safespot.Distance2D(LastSearchVector));
					 return Bot.NavigationCache.vlastSafeSpot;
				}

				private int lastGPRectIndexUsed=0;
				private void iterateGPRectsSafeSpot(Vector3 CurrentPosition, out Vector3 safespot, Vector3 LOS, bool kiting=false)
				{
					 safespot=Vector3.Zero;
					 for (int i=lastGPRectIndexUsed; i<gridpointrectangles_.Count-1; i++)
					 {
						  GPRectangle item=gridpointrectangles_[i];
						  item.UpdateObjectCount(AllGPRectsFailed);
						  if (item.Weight>Bot.NavigationCache.CurrentLocationGPRect.Weight) continue;

						  if (item.TryFindSafeSpot(CurrentPosition, out safespot, LOS, kiting, false, AllGPRectsFailed))
						  {
								this.lastUsedGPRect=gridpointrectangles_[i];
								return;
						  }
					 }
					 lastGPRectIndexUsed=0;
				}
		  }

	 
}