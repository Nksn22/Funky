﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using FunkyBot.Cache;
using FunkyBot.Cache.Objects;
using FunkyBot.Config;
using FunkyBot.Settings;
using System.Windows.Controls;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
using RadioButton = System.Windows.Controls.RadioButton;
using UIElement = Zeta.Game.Internals.UIElement;

namespace FunkyBot
{

	internal partial class FunkyWindow
	{
		internal static void buttonFunkySettingDB_Click(object sender, RoutedEventArgs e)
		{
			//Update Account Details when bot is not running!
			if (!BotMain.IsRunning)
				Bot.Character.Account.UpdateCurrentAccountDetails();

			string settingsFolder = FolderPaths.sDemonBuddyPath + @"\Settings\FunkyBot\" + Bot.Character.Account.CurrentAccountName;
			if (!Directory.Exists(settingsFolder)) Directory.CreateDirectory(settingsFolder);

			try
			{
				//var settingsForm = new SettingsForm();
				//settingsForm.ShowDialog();

				funkyConfigWindow = new FunkyWindow();
				funkyConfigWindow.Show();
			}
			catch (Exception ex)
			{
				Logger.DBLog.InfoFormat("Failure to initilize Funky Setting Window! \r\n {0} \r\n {1} \r\n {2}", ex.Message, ex.Source, ex.StackTrace);
			}
		}

		internal static FunkyWindow funkyConfigWindow;

		private void DefaultOpenSettingsFileClicked(object sender, EventArgs e)
		{
			try
			{
				Process.Start(FolderPaths.sFunkySettingsCurrentPath);
			}
			catch
			{

			}
		}
		private void DefaultMenuLevelingClicked(object sender, EventArgs e)
		{
			var confirm = MessageBox.Show(funkyConfigWindow,
				 "Are you sure you want to overwrite settings with default settings?",
				 "Confirm Overwrite", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
			if (confirm == MessageBoxResult.Yes)
			{
				string DefaultLeveling = Path.Combine(FolderPaths.sTrinityPluginPath, "Config", "Defaults", "LowLevel.xml");
				Logger.DBLog.InfoFormat("Creating new settings for {0} -- {1} using file {2}", Bot.Character.Account.CurrentAccountName, Bot.Character.Account.CurrentHeroName, DefaultLeveling);
				Settings_Funky newSettings = Settings_Funky.DeserializeFromXML(DefaultLeveling);
				Bot.Settings = newSettings;
				funkyConfigWindow.Close();
			}
		}
		private void DefaultMenuLoadProfileClicked(object sender, EventArgs e)
		{
			var OFD = new OpenFileDialog
			{
				InitialDirectory = Path.Combine(FolderPaths.sTrinityPluginPath, "Config", "Defaults"),
				RestoreDirectory = false,
				Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
				Title = "Open Settings",
			};
			DialogResult OFD_Result = OFD.ShowDialog();

			if (OFD_Result == System.Windows.Forms.DialogResult.OK)
			{
				try
				{
					MessageBoxResult confirm = MessageBox.Show(funkyConfigWindow, "Are you sure you want to overwrite settings with selected profile?", "Confirm Overwrite", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
					if (confirm == MessageBoxResult.Yes)
					{
						Logger.DBLog.InfoFormat("Creating new settings for {0} -- {1} using file {2}", Bot.Character.Account.CurrentAccountName, Bot.Character.Account.CurrentHeroName, OFD.FileName);
						Settings_Funky newSettings = Settings_Funky.DeserializeFromXML(OFD.FileName);
						Bot.Settings = newSettings;
						funkyConfigWindow.Close();
					}
				}
				catch
				{

				}
			}
		}

		private void FunkyLogLevelChanged(object sender, EventArgs e)
		{
			CheckBox cbSender = (CheckBox)sender;
			LogLevel LogLevelValue = (LogLevel)Enum.Parse(typeof(LogLevel), cbSender.Name);

			if (Bot.Settings.Debug.FunkyLogFlags.HasFlag(LogLevelValue))
				Bot.Settings.Debug.FunkyLogFlags &= ~LogLevelValue;
			else
				Bot.Settings.Debug.FunkyLogFlags |= LogLevelValue;
		}
		private void FunkyLogLevelComboBoxSelected(object sender, EventArgs e)
		{
			//LogLevelNone
			//LogLevelAll
			RadioButton CBsender = (RadioButton)sender;
			if (CBsender.Name == "LogLevelNone")
			{
				CBLogLevels.ForEach(cb => cb.IsChecked = false);
				Bot.Settings.Debug.FunkyLogFlags = LogLevel.None;
			}
			else
			{
				CBLogLevels.ForEach(cb => cb.IsChecked = true);
				Bot.Settings.Debug.FunkyLogFlags = LogLevel.All;
			}
		}
		private void DebugButtonClicked(object sender, EventArgs e)
		{
			LBDebug.Items.Clear();

			Button btnsender = (Button)sender;
			if (btnsender.Name == "Objects")
			{
				string OutPut = ObjectCache.Objects.DumpDebugInfo();

				LBDebug.Items.Add(OutPut);



				#region ColorIndex
				//Create Color Map ID
				StackPanel spColorIndexs = new StackPanel
				{
					Orientation = Orientation.Horizontal,
				};
				CheckBox cbMonsterID = new CheckBox
				{
					Background = Brushes.MediumSeaGreen,
					FontSize = 11,
					Content = "Unit",
					Foreground = Brushes.GhostWhite,
					Margin = new Thickness(5),
				};
				CheckBox cbItemID = new CheckBox
				{
					Background = Brushes.Gold,
					FontSize = 11,
					Content = "Item",
					Foreground = Brushes.GhostWhite,
					Margin = new Thickness(5),
				};
				CheckBox cbDestructibleID = new CheckBox
				{
					Background = Brushes.DarkSlateGray,
					FontSize = 11,
					Content = "Destructible",
					Foreground = Brushes.GhostWhite,
					Margin = new Thickness(5),
				};
				CheckBox cbInteractableID = new CheckBox
				{
					Background = Brushes.DimGray,
					FontSize = 11,
					Content = "Interactable",
					Foreground = Brushes.GhostWhite,
					Margin = new Thickness(5),
				};
				spColorIndexs.Children.Add(cbMonsterID);
				spColorIndexs.Children.Add(cbItemID);
				spColorIndexs.Children.Add(cbDestructibleID);
				spColorIndexs.Children.Add(cbInteractableID);
				LBDebug.Items.Add(spColorIndexs);

				#endregion

				Logger.DBLog.InfoFormat("Dumping Object Cache");

				OutPut += "\r\n";
				try
				{
					var SortedValues = ObjectCache.Objects.Values.OrderBy(obj => obj.targetType.Value).ThenBy(obj => obj.CentreDistance);
					foreach (var item in SortedValues)
					{
						string objDebugStr = item.DebugString;
						OutPut += objDebugStr + "\r\n";

						TextBlock objDebug = new TextBlock
						{
							Text = objDebugStr,
							TextAlignment = TextAlignment.Left,
							FontSize = 11,
							Foreground = (item is CacheItem) ? Brushes.Black : Brushes.GhostWhite,
							Background = (item is CacheDestructable) ? Brushes.DarkSlateGray
							: (item is CacheUnit) ? Brushes.MediumSeaGreen
							: (item is CacheItem) ? Brushes.Gold
							: (item is CacheInteractable) ? Brushes.DimGray
							: Brushes.Gray,
						};
						LBDebug.Items.Add(objDebug);
					}
				}
				catch
				{
					LBDebug.Items.Add("End of Output due to Modification Exception");
					return;
				}

				Logger.DBLog.DebugFormat(OutPut);

			}
			else if (btnsender.Name == "Obstacles")
			{
				LBDebug.Items.Add(ObjectCache.Obstacles.DumpDebugInfo());
				
				Logger.DBLog.InfoFormat("Dumping Obstacle Cache");

				try
				{
					var SortedValues = ObjectCache.Obstacles.Values.OrderBy(obj => obj.Obstacletype.Value).ThenBy(obj => obj.CentreDistance);
					foreach (var item in ObjectCache.Obstacles)
					{
						LBDebug.Items.Add(item.Value.DebugString);
					}
				}
				catch
				{

					LBDebug.Items.Add("End of Output due to Modification Exception");
				}

			}
			else if (btnsender.Name == "SNO")
			{

				LBDebug.Items.Add(ObjectCache.cacheSnoCollection.DumpDebugInfo());

				Logger.DBLog.InfoFormat("Dumping SNO Cache");
				try
				{
					foreach (var item in ObjectCache.cacheSnoCollection)
					{
						LBDebug.Items.Add(item.Value.DebugString);
					}
				}
				catch
				{

					LBDebug.Items.Add("End of Output due to Modification Exception");
				}

			}
			else if (btnsender.Name == "CHARACTER")
			{
				try
				{
					Logger.DBLog.InfoFormat("Dumping Character Cache");

					LBDebug.Items.Add(Bot.Character.Data.DebugString());

				}
				catch (Exception ex)
				{
					Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
				}
			}
			else if (btnsender.Name == "TargetMove")
			{
				try
				{
					Logger.DBLog.InfoFormat("TargetMovement: BlockedCounter{0} -- NonMovementCounter{1}", Bot.Targeting.Movement.BlockedMovementCounter, Bot.Targeting.Movement.NonMovementCounter);
				}
				catch
				{

				}
			}
			else if (btnsender.Name == "CombatCache")
			{
				try
				{
					LBDebug.Items.Add(Bot.Targeting.Cache.DebugString());
				}
				catch (Exception ex)
				{
					Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
				}
			}
			else if (btnsender.Name == "Ability")
			{
				try
				{
					if (Bot.Character.Class == null) return;

					LBDebug.Items.Add("==Current HotBar Abilities==");
					foreach (var item in Bot.Character.Class.Abilities.Values)
					{
						try
						{
							LBDebug.Items.Add(item.DebugString());
						}
						catch (Exception ex)
						{
							Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
						}
					}

					LBDebug.Items.Add("==Cached HotBar Abilities==");
					foreach (var item in Bot.Character.Class.HotBar.CachedPowers)
					{
						try
						{
							LBDebug.Items.Add(item.ToString());
						}
						catch (Exception ex)
						{
							Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
						}
					}

					LBDebug.Items.Add("==Buffs==");
					foreach (var item in Bot.Character.Class.HotBar.CurrentBuffs.Keys)
					{

						string Power = Enum.GetName(typeof(SNOPower), item);
						try
						{
							LBDebug.Items.Add(Power);
						}
						catch (Exception ex)
						{
							Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
						}
					}

				}
				catch (Exception ex)
				{
					Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
				}

			}
			else if (btnsender.Name == "TEST")
			{
				try
				{
					LBDebug.Items.Add(Enum.GetName(typeof(SNOActor), ZetaDia.Me.ActorSNO));
					
				}
				catch(Exception ex)
				{
					Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
				}
			}
			else if (btnsender.Name == "CombatMovement")
			{
				try
				{
					string debugStr = Bot.Targeting.Movement.DebugString();
					LBDebug.Items.Add(debugStr);
					Logger.DBLog.InfoFormat("Movement Info: \r\n {0}", debugStr);
				}
				catch (Exception ex)
				{
					Logger.DBLog.InfoFormat("Safely Handled Exception {0}", ex.Message);
				}
			}
			//
			LBDebug.Items.Refresh();
		}

		#region ClassSettings

		private void bWaitForArchonChecked(object sender, EventArgs e)
		{
			Bot.Settings.Wizard.bWaitForArchon = !Bot.Settings.Wizard.bWaitForArchon;
		}
		private void bKiteOnlyArchonChecked(object sender, EventArgs e)
		{
			Bot.Settings.Wizard.bKiteOnlyArchon = !Bot.Settings.Wizard.bKiteOnlyArchon;
		}
		private void bCancelArchonRebuffChecked(object sender, EventArgs e)
		{
			Bot.Settings.Wizard.bCancelArchonRebuff = !Bot.Settings.Wizard.bCancelArchonRebuff;
		}
		private void bTeleportFleeWhenLowHPChecked(object sender, EventArgs e)
		{
			Bot.Settings.Wizard.bTeleportFleeWhenLowHP = !Bot.Settings.Wizard.bTeleportFleeWhenLowHP;
		}
		private void bTeleportIntoGroupingChecked(object sender, EventArgs e)
		{
			Bot.Settings.Wizard.bTeleportIntoGrouping = !Bot.Settings.Wizard.bTeleportIntoGrouping;
		}
		private void bSelectiveWhirlwindChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bSelectiveWhirlwind = !Bot.Settings.Barbarian.bSelectiveWhirlwind;
		}
		private void bWaitForWrathChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bWaitForWrath = !Bot.Settings.Barbarian.bWaitForWrath;
		}
		private void bGoblinWrathChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bGoblinWrath = !Bot.Settings.Barbarian.bGoblinWrath;
		}
		private void bBarbUseWOTBAlwaysChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bBarbUseWOTBAlways = !Bot.Settings.Barbarian.bBarbUseWOTBAlways;
		}
		private void bFuryDumpWrathChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bFuryDumpWrath = !Bot.Settings.Barbarian.bFuryDumpWrath;
		}
		private void bFuryDumpAlwaysChecked(object sender, EventArgs e)
		{
			Bot.Settings.Barbarian.bFuryDumpAlways = !Bot.Settings.Barbarian.bFuryDumpAlways;
		}
		private void bMonkMaintainSweepingWindChecked(object sender, EventArgs e)
		{
			Bot.Settings.Monk.bMonkMaintainSweepingWind = !Bot.Settings.Monk.bMonkMaintainSweepingWind;
		}
		private void bMonkSpamMantraChecked(object sender, EventArgs e)
		{
			Bot.Settings.Monk.bMonkSpamMantra = !Bot.Settings.Monk.bMonkSpamMantra;
		}
		private void iDHVaultMovementDelaySliderChanged(object sender, EventArgs e)
		{
			Slider slider_sender = (Slider)sender;
			int Value = (int)slider_sender.Value;
			Bot.Settings.DemonHunter.iDHVaultMovementDelay = Value;
			TBiDHVaultMovementDelay.Text = Value.ToString();
		}
		private void DebugStatusBarChecked(object sender, EventArgs e)
		{
			Bot.Settings.Debug.DebugStatusBar = !Bot.Settings.Debug.DebugStatusBar;
		}
		private void SkipAheadChecked(object sender, EventArgs e)
		{
			Bot.Settings.Debug.SkipAhead = !Bot.Settings.Debug.SkipAhead;
		}
		#endregion





		protected override void OnClosed(EventArgs e)
		{
			Settings_Funky.SerializeToXML(Bot.Settings);
			base.OnClosed(e);
		}

	}

}