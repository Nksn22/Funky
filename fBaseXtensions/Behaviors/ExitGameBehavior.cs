﻿using System;
using fBaseXtensions.Helpers;
using Zeta.Game;
using Zeta.TreeSharp;

namespace fBaseXtensions.Behaviors
{
	public static class ExitGameBehavior
	{
		public static bool ShouldExitGame = false;

		private static bool _behaviorEngaged;
		internal static bool BehaviorEngaged
		{
			get { return _behaviorEngaged; }
			set
			{
				_behaviorEngaged = value;
				//true we reset behavior starting time
				BehaviorEngagedTime = value ? DateTime.Now : DateTime.MaxValue;
			}
		}

        internal static DateTime BehaviorEngagedTime = DateTime.MaxValue;

		///<summary>
		///Exiting Game Behavior
		///</summary>
        internal static RunStatus Behavior()
		{

			//Run Town Portal Behavior.. 
			if (TownPortalBehavior.FunkyTPOverlord(null))
			{
				TownPortalBehavior.FunkyTPBehavior(null);
				return RunStatus.Running;
			}

			//Loading World?
			if (ZetaDia.IsLoadingWorld)
				return RunStatus.Running;

			//Exit Game..
			if (ZetaDia.IsInGame)
			{
				if (DateTime.Now.Subtract(_lastExitAttempt).TotalSeconds > 4)
				{
					Logger.DBLog.InfoFormat("[Funky] Exiting game..");
					ZetaDia.Service.Party.LeaveGame();
					_lastExitAttempt = DateTime.Now;
				}
				return RunStatus.Running;
			}

			return RunStatus.Success;
		}
		private static DateTime _lastExitAttempt = DateTime.MinValue;
	}
}
