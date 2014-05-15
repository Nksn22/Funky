﻿using System;
using System.Globalization;
using System.Linq;
using FunkyBot.Config.Settings;
using Zeta.Bot;
using Zeta.Bot.Settings;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.TreeSharp;

namespace FunkyBot.DBHandlers
{
	public static class OutOfGame
	{

		internal static bool MuleBehavior = false;
		private static bool InitMuleBehavior;
		private static bool CreatedCharacter;
		private static bool RanProfile;
		internal static bool TransferedGear = false;
		internal static bool Finished = false;

		public static bool OutOfGameOverlord(object ret)
		{
			if (MuleBehavior)
			{
				if (!Bot.Settings.Plugin.CreateMuleOnStashFull)
				{
					BotMain.Stop(true, "Cannot stash anymore items!");
					return false;
				}

				//Skip this until we create our new A1 game..
				if (RanProfile && !TransferedGear)
					return false;

				//Now we finish up..
				if (RanProfile && TransferedGear && !Finished)
					return true;
				Logger.Write(LogLevel.OutOfGame, "Starting Mule Behavior");
				CreatedCharacter = false;
				RanProfile = false;
				TransferedGear = false;

				if (ZetaDia.Service.GameAccount.NumEmptyHeroSlots == 0)
				{
					Logger.Write(LogLevel.OutOfGame, "No Empty Hero Slots Remain, and our stash if full.. stopping the bot!");
					BotMain.Stop(true, "Cannot stash anymore items!");
				}
				else
					return true;
			}

			//Change the Monster Power!
			//if (Bot.Settings.Demonbuddy.EnableDemonBuddyCharacterSettings)
			//{
			//	int overridePowerLevel = Bot.Settings.Demonbuddy.MonsterPower;
			//	Logger.DBLog.InfoFormat("[Funky] Overriding Monster Power Level to {0}", overridePowerLevel.ToString(CultureInfo.InvariantCulture));
			//	CharacterSettings.Instance.MonsterPowerLevel = overridePowerLevel;
			//}

			////Disconnect -- Starting Profile Setup.
			//if (FunkyErrorClicker.FunkyErrorClicker.HadDisconnectError)
			//{
			//	 Logger.DBLog.InfoFormat("[Funky] Disconnected Last Game.. Reloading Current Profile.");
			//	 //ReloadStartingProfile();
			//	 ProfileManager.Load(Zeta.CommonBot.ProfileManager.CurrentProfile.Path);
			//	 FunkyErrorClicker.FunkyErrorClicker.HadDisconnectError=false;
			//}


			return false;
		}

		public static RunStatus OutOfGameBehavior(object ret)
		{
			if (MuleBehavior)
			{
				if (!InitMuleBehavior)
				{
					Finished = false;
					InitMuleBehavior = true;
					NewMuleGame.BotHeroName = ZetaDia.Service.Hero.Name;
					NewMuleGame.BotHeroIndex = 0;
					NewMuleGame.LastProfile = ProfileManager.CurrentProfile.Path;
					NewMuleGame.LastHandicap = CharacterSettings.Instance.MonsterPowerLevel;
				}

				if (!CreatedCharacter)
				{
					RunStatus NewHeroStatus = D3Character.CreateNewHero();

					if (NewHeroStatus == RunStatus.Success)
					{
						CreatedCharacter = true;
						//Setup Settings
						Bot.Character.Account.UpdateCurrentAccountDetails();
						Settings_Funky.LoadFunkyConfiguration();
					}
					return RunStatus.Running;
				}

				if (!RanProfile)
				{
					RunStatus NewGameStatus = NewMuleGame.BeginNewGameProfile();
					if (NewGameStatus == RunStatus.Success)
					{
						RanProfile = true;
						return RunStatus.Success;
					}
					return RunStatus.Running;
				}

				RunStatus FinishStatus = NewMuleGame.FinishMuleBehavior();
				if (FinishStatus == RunStatus.Success)
				{
					Finished = true;
					RanProfile = false;
					CreatedCharacter = false;
					InitMuleBehavior = false;
					MuleBehavior = false;
					//Load Settings
					Bot.Character.Account.UpdateCurrentAccountDetails();
					Settings_Funky.LoadFunkyConfiguration();

					return RunStatus.Success;
				}
				return RunStatus.Running;

			}

			return RunStatus.Success;
		}


		public static class D3Character
		{
			private static UIElement SwitchHeroButton_;
			public static UIElement SwitchHeroButton
			{
				get
				{
					if (SwitchHeroButton_ == null || !SwitchHeroButton_.IsValid)
						SwitchHeroButton_ = UIElement.FromHash(0xBE4E61ABD1DCDC79);

					if (SwitchHeroButton_.IsValid && SwitchHeroButton_.IsVisible && SwitchHeroButton_.IsEnabled)
						return SwitchHeroButton_;
					return null;
				}
			}

			private static UIElement CreateHeroButton_;
			public static UIElement CreateHeroButton
			{
				get
				{
					if (CreateHeroButton_ == null || !CreateHeroButton_.IsValid)
						CreateHeroButton_ = UIElement.FromHash(0x744BC83D82918CE2);

					if (CreateHeroButton_.IsValid && CreateHeroButton_.IsVisible && CreateHeroButton_.IsEnabled)
						return CreateHeroButton_;
					else
						return null;
				}
			}

			private static UIElement SelectHeroButton_;
			public static UIElement SelectHeroButton
			{
				get
				{
					if (SelectHeroButton_ == null || !SelectHeroButton_.IsValid)
						SelectHeroButton_ = UIElement.FromHash(0x5D73E830BC87CE66);

					if (SelectHeroButton_.IsValid && SelectHeroButton_.IsVisible && SelectHeroButton_.IsEnabled)
						return CreateHeroButton_;
					else
						return null;
				}
			}

			private static UIElement SelectHeroType(ActorClass type)
			{
				UIElement thisClassButton = null;
				switch (type)
				{
					case ActorClass.Barbarian:
						thisClassButton = UIElement.FromHash(0x98976D3F43BBF74);
						break;
					case ActorClass.DemonHunter:
						thisClassButton = UIElement.FromHash(0x98976D3F43BBF74);
						break;
					case ActorClass.Monk:
						thisClassButton = UIElement.FromHash(0x7733072C07DABF11);
						break;
					case ActorClass.Witchdoctor:
						thisClassButton = UIElement.FromHash(0x1A2DB1F47C26A8C2);
						break;
					case ActorClass.Wizard:
						thisClassButton = UIElement.FromHash(0xBC3AA6A915972065);
						break;
				}

				return thisClassButton;
			}

			private static UIElement HeroNameText_;
			public static UIElement HeroNameText
			{
				get
				{
					if (HeroNameText_ == null || !HeroNameText_.IsValid)
						HeroNameText_ = UIElement.FromHash(0x8D2C771F09BC037F);

					if (HeroNameText_.IsValid && HeroNameText_.IsVisible && HeroNameText_.IsEnabled)
						return HeroNameText_;
					else
						return null;
				}
			}

			private static UIElement CreateNewHeroButton_;
			public static UIElement CreateNewHeroButton
			{
				get
				{
					if (CreateNewHeroButton_ == null || !CreateNewHeroButton_.IsValid)
						CreateNewHeroButton_ = UIElement.FromHash(0x28578F7B0F6384C6);

					if (CreateNewHeroButton_.IsValid && CreateNewHeroButton_.IsVisible && CreateNewHeroButton_.IsEnabled)
						return CreateNewHeroButton_;
					else
						return null;
				}
			}

			private static DateTime LastActionTaken = DateTime.Today;
			private static bool SelectedClass = false;

			public static RunStatus CreateNewHero()
			{
				if (DateTime.Now.Subtract(LastActionTaken).TotalMilliseconds > 1000)
				{
					if (SwitchHeroButton != null && SwitchHeroButton.IsValid && SwitchHeroButton.IsVisible)
					{
						if (NewCharacterName == null)
						{
							SwitchHeroButton.Click();
							SwitchHeroButton_ = null;
						}
						else if (ZetaDia.Service.Hero.Name == NewCharacterName)
						{
							//
							Logger.Write(LogLevel.OutOfGame, "Successfully Created New Character");
							return RunStatus.Success;
						}
					}
					else if (CreateHeroButton != null && CreateHeroButton.IsValid && CreateHeroButton.IsVisible)
					{
						CreateHeroButton.Click();
					}
					else if (HeroNameText != null && HeroNameText.IsValid && HeroNameText.IsVisible)
					{
						if (!SelectedClass)
						{
							UIElement thisClassButton = SelectHeroType(ActorClass.DemonHunter);
							if (thisClassButton != null && thisClassButton.IsValid && thisClassButton.IsEnabled && thisClassButton.IsVisible)
							{
								thisClassButton.Click();
								SelectedClass = true;
							}
						}
						else
						{
							if (NewCharacterName == null)
								NewCharacterName = GenerateRandomText();

							if (HeroNameText.IsValid)
							{
								Logger.Write(LogLevel.OutOfGame, "Valid TextObject for character name UI");
							}

							if (!HeroNameText.HasText)
							{
								HeroNameText.SetText(NewCharacterName.Substring(0, 1));
							}
							else
							{
								if (HeroNameText.Text != NewCharacterName)
								{
									HeroNameText.SetText(NewCharacterName.Substring(0, HeroNameText.Text.Length + 1));
								}
								else if (CreateNewHeroButton != null && CreateNewHeroButton.IsVisible && CreateNewHeroButton.IsEnabled)
								{
									CreateNewHeroButton.Click();
								}
							}
						}
					}

					LastActionTaken = DateTime.Now;
				}
				return RunStatus.Running;
			}


			internal static string NewCharacterName = null;

			private static string GenerateRandomText()
			{
				var chars = "abcdefghijklmnopqrstuvwxyz";
				var random = new Random();
				var result = new string(
					 Enumerable.Repeat(chars, random.Next(5, 8))
					 .Select(s => s[random.Next(s.Length)])
					 .ToArray());

				Logger.DBLog.InfoFormat("Generated Name " + result);
				return result;
			}
		}
	}
}