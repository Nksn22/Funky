﻿using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.TreeSharp;
namespace FunkyBot
{
	public partial class EventHandlers
	{
		internal static bool TallyedDeathCounter = false;
		internal static bool TallyDeathCanRunDecorator(object ret)
		{
			return !TallyedDeathCounter;
		}
		internal static RunStatus TallyDeathAction(object ret)
		{
			Bot.Game.CurrentGameStats.CurrentProfile.DeathCount++;
			TallyedDeathCounter = true;
			return RunStatus.Success;
		}

		private static bool WaitingForRevive;
		public static RunStatus DeathHandler(object ret)
		{
			UIElement ReviveAtLastCheckpointButton = null;
			try
			{
				ReviveAtLastCheckpointButton = UIElements.ReviveAtLastCheckpointButton;
			}
			catch
			{
				Logger.DBLog.InfoFormat("Revive Button Exception Handled");
			}

			if (ReviveAtLastCheckpointButton != null)
			{
				//TODO:: Add Safety Exit-Game for Broken Equipped Items 
				if (UIElements.ReviveAtLastCheckpointButton.IsVisible && UIElements.ReviveAtLastCheckpointButton.IsEnabled)
				{
					Logger.DBLog.InfoFormat("Clicking Revive Button!");
					UIElements.ReviveAtLastCheckpointButton.Click();
					WaitingForRevive = true;
					Bot.Character.Data.OnHealthChanged += OnHealthChanged;
					return RunStatus.Running;
				}
			}
			else
			{
				//No revive button?? lets check if we are alive..?
				if (!ZetaDia.Me.IsDead)
				{
					//Don't wait for health change event..
					WaitingForRevive = false;
					Revived = false;

					Bot.Game.CurrentGameStats.CurrentProfile.DeathCount++;
					//Bot.BotStatistics.GameStats.CurrentGame.Deaths++;
					//Bot.BotStatistics.ProfileStats.CurrentProfile.DeathCount++;
					Bot.Character.Data.OnHealthChanged -= OnHealthChanged;
					return RunStatus.Success;
				}
			}

			if (WaitingForRevive)
			{
				if (Revived)
				{
					WaitingForRevive = false;
					Revived = false;
					Bot.Game.CurrentGameStats.CurrentProfile.DeathCount++;
					//Bot.BotStatistics.GameStats.CurrentGame.Deaths++;
					//Bot.BotStatistics.ProfileStats.CurrentProfile.DeathCount++;
				}
				else
				{
					Bot.Character.Data.Update();
					return RunStatus.Running;
				}
			}

			Bot.Character.Data.OnHealthChanged -= OnHealthChanged;
			return RunStatus.Success;
		}

		private static bool Revived;
		private static void OnHealthChanged(double oldvalue, double newvalue)
		{
			if (newvalue >= 1d)
				Revived = true;
		}
	}
}
