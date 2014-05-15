﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Demonbuddy;
using FunkyBot.Cache;
using FunkyBot.Config.Settings;
using FunkyBot.Config.UI;
using FunkyBot.Movement;
using System.Collections.Generic;
using FunkyBot.Player.Class;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
using Zeta.Common;
using Zeta.Game.Internals.Actors;
using Zeta.TreeSharp;
using System.Xml;
using System.Windows;
using Decorator = Zeta.TreeSharp.Decorator;
using Action = Zeta.TreeSharp.Action;
using FunkyBot.DBHandlers;

namespace FunkyBot
{
	public partial class Funky
	{
		internal static bool bPluginEnabled;
		private static bool initFunkyButton;
		internal static bool initTreeHooks;
		internal static int iDemonbuddyMonsterPowerLevel = 0;
		internal static SettingsForm FrmSettings;

		// Status text for DB main window status
		internal static string sStatusText = "";
		// Do we need to reset the debug bar after combat handling?
		internal static bool bResetStatusText = false;


		public static void ResetBot()
		{

			Logger.DBLog.InfoFormat("Preforming reset of bot data...");
			BlacklistCache.ClearBlacklistCollections();

			PlayerMover.iTotalAntiStuckAttempts = 1;
			PlayerMover.vSafeMovementLocation = Vector3.Zero;
			PlayerMover.vOldPosition = Vector3.Zero;
			PlayerMover.iTimesReachedStuckPoint = 0;
			PlayerMover.timeLastRecordedPosition = DateTime.Today;
			PlayerMover.timeStartedUnstuckMeasure = DateTime.Today;
			PlayerMover.iTimesReachedMaxUnstucks = 0;
			PlayerMover.iCancelUnstuckerForSeconds = 0;
			PlayerMover.timeCancelledUnstuckerFor = DateTime.Today;

			//Reset all data with bot (Playerdata, Combat Data)
			Bot.Reset();
			PlayerClass.CreateBotClass();
			//Update character info!
			Bot.Character.Data.Update();

			//OOC ID Flags
			Bot.Targeting.Cache.ShouldCheckItemLooted = false;
			Bot.Targeting.Cache.CheckItemLootStackCount = 0;
			ItemIdentifyBehavior.shouldPreformOOCItemIDing = false;

			//TP Behavior Reset
			TownPortalBehavior.ResetTPBehavior();

			//Sno Trim Timer Reset
			ObjectCache.cacheSnoCollection.ResetTrimTimer();
			//clear obstacles
			ObjectCache.Obstacles.Clear();
			ObjectCache.Objects.Clear();
			EventHandlers.DumpedDeathInfo = false;
		}
		public static void ResetGame()
		{
			SkipAheadCache.ClearCache();
			TownRunManager.TalliedTownRun = false;
			TownRunManager.TownrunStartedInTown = true;
			TownRunManager._dictItemStashAttempted = new Dictionary<int, int>();
		}

		private static SplitButton FindFunkyButton()
		{
			Window mainWindow = App.Current.MainWindow;
			var tab = mainWindow.FindName("tabControlMain") as TabControl;
			if (tab == null) return null;
			var infoDumpTab = tab.Items[0] as TabItem;
			if (infoDumpTab == null) return null;
			var grid = infoDumpTab.Content as Grid;
			if (grid == null) return null;
			SplitButton FunkyButton = grid.FindName("Funky") as SplitButton;
			if (FunkyButton != null)
			{
				Logger.DBLog.DebugFormat("Funky Button handler added");
			}
			else
			{
				SplitButton[] splitbuttons = grid.Children.OfType<SplitButton>().ToArray();
				if (splitbuttons.Any())
				{

					foreach (var item in splitbuttons)
					{
						if (item.Name.Contains("Funky"))
						{
							FunkyButton = item;
							break;
						}
					}
				}
			}

			return FunkyButton;
		}
		internal static void buttonFunkySettingDB_Click(object sender, RoutedEventArgs e)
		{
			//Update Account Details when bot is not running!
			if (!BotMain.IsRunning)
				Bot.Character.Account.UpdateCurrentAccountDetails();

			string settingsFolder = FolderPaths.DemonBuddyPath + @"\Settings\FunkyBot\" + Bot.Character.Account.CurrentAccountName;
			if (!Directory.Exists(settingsFolder)) Directory.CreateDirectory(settingsFolder);

			try
			{
				Settings_Funky.LoadFunkyConfiguration();
				FrmSettings = new SettingsForm();
				FrmSettings.Show();
			}
			catch (Exception ex)
			{
				Logger.DBLog.InfoFormat("Failure to initilize Funky Setting Window! \r\n {0} \r\n {1} \r\n {2}", 
					ex.Message, ex.Source, ex.StackTrace);
				if (ex.InnerException!=null)
				{
					Logger.DBLog.InfoFormat("Inner Exception: {0}\r\n{1}\r\n{2}",ex.InnerException.Message,ex.InnerException.Source,ex.InnerException.StackTrace);
				}
			}

			
		}
		internal static void HookBehaviorTree()
		{

			bool townportal = false, idenify = false, unidfystash = false, stash = false, vendor = false, salvage = false, looting = true, combat = true;

			using (XmlReader reader = XmlReader.Create(FolderPaths.PluginPath + "Treehooks.xml"))
			{

				// Parse the XML document.  ReadString is used to 
				// read the text content of the elements.
				reader.Read();
				reader.ReadStartElement("Treehooks");
				reader.Read();
				while (reader.LocalName != "Treehooks")
				{
					switch (reader.LocalName)
					{
						case "Townportal":
							townportal = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "UnidentifiedStash":
							unidfystash = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Identification":
							idenify = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Stash":
							stash = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Vendor":
							vendor = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Salvage":
							salvage = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Looting":
							looting = Convert.ToBoolean(reader.ReadInnerXml());
							break;
						case "Combat":
							combat = Convert.ToBoolean(reader.ReadInnerXml());
							break;
					}

					reader.Read();
				}
			}

			Logger.DBLog.InfoFormat("[Funky] Treehooks..");
			#region TreeHooks
			foreach (var hook in TreeHooks.Instance.Hooks)
			{
				#region Combat

				// Replace the combat behavior tree, as that happens first and so gets done quicker!
				if (hook.Key.Contains("Combat"))
				{
					Logger.DBLog.DebugFormat("Combat...");

					if (combat)
					{
						// Replace the pause just after identify stuff to ensure we wait before trying to run to vendor etc.
						CanRunDecoratorDelegate canRunDelegateCombatTargetCheck = GlobalOverlord;
						ActionDelegate actionDelegateCoreTarget = HandleTarget;
						Sequence sequencecombat = new Sequence(
								new Action(actionDelegateCoreTarget)
								);
						hook.Value[0] = new Decorator(canRunDelegateCombatTargetCheck, sequencecombat);
						Logger.DBLog.DebugFormat("Combat Tree replaced...");
					}

				}
				
				#endregion

				#region VendorRun

				// Wipe the vendorrun and loot behavior trees, since we no longer want them
				if (hook.Key.Contains("VendorRun"))
				{
					Logger.DBLog.DebugFormat("VendorRun...");

					Decorator GilesDecorator = hook.Value[0] as Decorator;
					//PrioritySelector GilesReplacement = GilesDecorator.Children[0] as PrioritySelector;
					PrioritySelector GilesReplacement = GilesDecorator.Children[0] as PrioritySelector;


					#region TownPortal

					//[1] == Return to town
					if (townportal)
					{
						CanRunDecoratorDelegate canRunDelegateReturnToTown = TownPortalBehavior.FunkyTPOverlord;
						ActionDelegate actionDelegateReturnTown = TownPortalBehavior.FunkyTPBehavior;
						ActionDelegate actionDelegateTownPortalFinish = TownPortalBehavior.FunkyTownPortalTownRun;
						Sequence sequenceReturnTown = new Sequence(
							new Action(actionDelegateReturnTown),
							new Action(actionDelegateTownPortalFinish)
							);
						GilesReplacement.Children[1] = new Decorator(canRunDelegateReturnToTown, sequenceReturnTown);
						Logger.DBLog.DebugFormat("Town Run - Town Portal - hooked...");
					}

					
					#endregion

					ActionDelegate actionDelegatePrePause = TownRunManager.GilesStashPrePause;
					ActionDelegate actionDelegatePause = TownRunManager.GilesStashPause;

					#region Idenify


					if (idenify)
					{
						//[2] == IDing items in inventory
						CanRunDecoratorDelegate canRunDelegateFunkyIDBehavior = ItemIdentifyBehavior.FunkyIDOverlord;
						ActionDelegate actionDelegateID = ItemIdentifyBehavior.FunkyIDBehavior;
						Sequence sequenceIDItems = new Sequence(
								new Action(actionDelegateID),
								new Sequence(
								new Action(actionDelegatePrePause),
								new Action(actionDelegatePause)
								)
								);
						GilesReplacement.Children[2] = new Decorator(canRunDelegateFunkyIDBehavior, sequenceIDItems);
						Logger.DBLog.DebugFormat("Town Run - Idenify Items - hooked...");
					}

					
					#endregion

					// Replace the pause just after identify stuff to ensure we wait before trying to run to vendor etc.
					CanRunDecoratorDelegate canRunDelegateStashGilesPreStashPause = TownRunManager.GilesPreStashPauseOverlord;
					Sequence sequencepause = new Sequence(
							new Action(actionDelegatePrePause),
							new Action(actionDelegatePause)
							);

					GilesReplacement.Children[3] = new Decorator(canRunDelegateStashGilesPreStashPause, sequencepause);


					#region Stash
					if (stash)
					{
						// Replace DB stashing behavior tree with my optimized version with loot rule replacement
						CanRunDecoratorDelegate canRunDelegateStashGilesOverlord = TownRunManager.StashOverlord;
						ActionDelegate actionDelegatePreStash = TownRunManager.PreStash;
						ActionDelegate actionDelegatePostStash = TownRunManager.PostStash;

						ActionDelegate actionDelegateStashMovement = TownRunManager.StashMovement;
						ActionDelegate actionDelegateStashUpdate = TownRunManager.StashUpdate;
						ActionDelegate actionDelegateStashItems = TownRunManager.StashItems;

						Sequence sequencestash = new Sequence(
								new Action(actionDelegatePreStash),
								new Action(actionDelegateStashMovement),
								new Action(actionDelegateStashUpdate),
								new Action(actionDelegateStashItems),
								new Action(actionDelegatePostStash),
								new Sequence(
								new Action(actionDelegatePrePause),
								new Action(actionDelegatePause)
								)
								);
						GilesReplacement.Children[4] = new Decorator(canRunDelegateStashGilesOverlord, sequencestash);
						Logger.DBLog.DebugFormat("Town Run - Stash - hooked...");
					}
					#endregion

					#region Vendor
					if (vendor)
					{
						// Replace DB vendoring behavior tree with my optimized & "one-at-a-time" version
						CanRunDecoratorDelegate canRunDelegateSellGilesOverlord = TownRunManager.GilesSellOverlord;
						ActionDelegate actionDelegatePreSell = TownRunManager.GilesOptimisedPreSell;
						ActionDelegate actionDelegateSell = TownRunManager.GilesOptimisedSell;
						ActionDelegate actionDelegatePostSell = TownRunManager.GilesOptimisedPostSell;
						Sequence sequenceSell = new Sequence(
								new Action(actionDelegatePreSell),
								new Action(actionDelegateSell),
								new Action(actionDelegatePostSell),
								new Sequence(
								new Action(actionDelegatePrePause),
								new Action(actionDelegatePause)
								)
								);
						GilesReplacement.Children[5] = new Decorator(canRunDelegateSellGilesOverlord, sequenceSell);
						Logger.DBLog.DebugFormat("Town Run - Vendor - hooked...");
					}
					
					#endregion

					#region Salvage
					if (salvage)
					{
						// Replace DB salvaging behavior tree with my optimized & "one-at-a-time" version
						CanRunDecoratorDelegate canRunDelegateSalvageGilesOverlord = TownRunManager.GilesSalvageOverlord;
						ActionDelegate actionDelegatePreSalvage = TownRunManager.GilesOptimisedPreSalvage;
						ActionDelegate actionDelegateSalvage = TownRunManager.GilesOptimisedSalvage;
						ActionDelegate actionDelegatePostSalvage = TownRunManager.GilesOptimisedPostSalvage;
						Sequence sequenceSalvage = new Sequence(
								new Action(actionDelegatePreSalvage),
								new Action(actionDelegateSalvage),
								new Action(actionDelegatePostSalvage),
								new Sequence(
								new Action(actionDelegatePrePause),
								new Action(actionDelegatePause)
								)
								);
						GilesReplacement.Children[6] = new Decorator(canRunDelegateSalvageGilesOverlord, sequenceSalvage);
						Logger.DBLog.DebugFormat("Town Run - Salvage - hooked...");
					}
					
					#endregion

					#region UnidfyStash

					if (unidfystash)
					{
						CanRunDecoratorDelegate canRunUnidBehavior = TownRunManager.UnidItemOverlord;
						ActionDelegate actionDelegatePreUnidStash = TownRunManager.PreStash;
						ActionDelegate actionDelegatePostUnidStash = TownRunManager.PostStash;

						ActionDelegate actionDelegateStashMovement = TownRunManager.StashMovement;
						ActionDelegate actionDelegateStashUpdate = TownRunManager.StashUpdate;
						ActionDelegate actionDelegateStashItems = TownRunManager.StashItems;

						Sequence sequenceUnidStash = new Sequence(
								new Action(actionDelegatePreUnidStash),
								new Action(actionDelegateStashMovement),
								new Action(actionDelegateStashUpdate),
								new Action(actionDelegateStashItems),
								new Action(actionDelegatePostUnidStash),
								new Sequence(
								new Action(actionDelegatePrePause),
								new Action(actionDelegatePause)
								)
							  );

						//Insert Before Item ID Step
						GilesReplacement.InsertChild(2, new Decorator(canRunUnidBehavior, sequenceUnidStash));
						Logger.DBLog.DebugFormat("Town Run - Undify Stash - inserted...");
					}

					
					#endregion

					#region Interaction Behavior
					CanRunDecoratorDelegate canRunDelegateInteraction = TownRunManager.InteractionOverlord;
					ActionDelegate actionDelegateInteractionMovementhBehavior = TownRunManager.InteractionMovement;
					ActionDelegate actionDelegateInteractionClickBehaviorBehavior = TownRunManager.InteractionClickBehavior;
					ActionDelegate actionDelegateInteractionLootingBehaviorBehavior = TownRunManager.InteractionLootingBehavior;
					ActionDelegate actionDelegateInteractionFinishBehaviorBehavior = TownRunManager.InteractionFinishBehavior;

					Sequence sequenceInteraction = new Sequence(
							new Action(actionDelegateInteractionFinishBehaviorBehavior),
							new Action(actionDelegateInteractionMovementhBehavior),
							new Action(actionDelegateInteractionClickBehaviorBehavior),
							new Action(actionDelegateInteractionLootingBehaviorBehavior),
							new Action(actionDelegateInteractionFinishBehaviorBehavior)
						);
					GilesReplacement.InsertChild(7, new Decorator(canRunDelegateInteraction, sequenceInteraction));
					Logger.DBLog.DebugFormat("Town Run - Interaction Behavior - Inserted...");

					#endregion

					#region Gambling Behavior

					CanRunDecoratorDelegate canRunDelegateGambling = TownRunManager.GamblingRunOverlord;
					ActionDelegate actionDelegateGamblingMovementBehavior = TownRunManager.GamblingMovement;
					ActionDelegate actionDelegateGamblingInteractionBehavior = TownRunManager.GamblingInteraction;
					ActionDelegate actionDelegateGamblingStartBehavior = TownRunManager.GamblingStart;
					ActionDelegate actionDelegateGamblingFinishBehavior = TownRunManager.GamblingFinish;

					Sequence sequenceGambling = new Sequence(
						    new Action(actionDelegateGamblingStartBehavior),
							new Action(actionDelegateGamblingMovementBehavior),
							new Action(actionDelegateGamblingInteractionBehavior),
							new Action(actionDelegateGamblingFinishBehavior)
						);
					GilesReplacement.InsertChild(8, new Decorator(canRunDelegateGambling, sequenceGambling));
					Logger.DBLog.DebugFormat("Town Run - Gambling Behavior - Inserted...");

					#endregion

					#region Finish Behavior

					CanRunDecoratorDelegate canRunDelegateFinish = TownRunManager.FinishOverlord;
					ActionDelegate actionDelegateFinishBehavior = TownRunManager.FinishBehavior;

					Sequence sequenceFinish = new Sequence(
							new Action(actionDelegateFinishBehavior)
						);
					GilesReplacement.InsertChild(9, new Decorator(canRunDelegateFinish, sequenceFinish));
					Logger.DBLog.DebugFormat("Town Run - Finish Behavior - Inserted...");

					#endregion


					CanRunDecoratorDelegate canRunDelegateGilesTownRunCheck = TownRunManager.GilesTownRunCheckOverlord;
					hook.Value[0] = new Decorator(canRunDelegateGilesTownRunCheck, new PrioritySelector(GilesReplacement));

					Logger.DBLog.DebugFormat("Vendor Run tree hooked...");
				} // Vendor run hook

				
				#endregion

				#region Loot

				if (hook.Key.Contains("Loot"))
				{
					Logger.DBLog.DebugFormat("Loot...");

					if (looting)
					{
						// Replace the loot behavior tree with a blank one, as we no longer need it
						CanRunDecoratorDelegate canRunDelegateBlank = BlankDecorator;
						ActionDelegate actionDelegateBlank = BlankAction;
						Sequence sequenceblank = new Sequence(
								new Action(actionDelegateBlank)
								);
						hook.Value[0] = new Decorator(canRunDelegateBlank, sequenceblank);
						Logger.DBLog.DebugFormat("Loot tree replaced...");
					}
					else
					{
						CanRunDecoratorDelegate canRunDelegateBlank = BlankDecorator;
						hook.Value[0] = new Decorator(canRunDelegateBlank, BrainBehavior.CreateLootBehavior());
					}
				}
				
				#endregion

				#region OutOfGame


				if (hook.Key.Contains("OutOfGame"))
				{
					Logger.DBLog.DebugFormat("OutOfGame...");

					PrioritySelector CompositeReplacement = hook.Value[0] as PrioritySelector;

					CanRunDecoratorDelegate shouldPreformOutOfGameBehavior = OutOfGame.OutOfGameOverlord;
					ActionDelegate actionDelgateOOGBehavior = OutOfGame.OutOfGameBehavior;
					Sequence sequenceOOG = new Sequence(
							new Action(actionDelgateOOGBehavior)
					);
					CompositeReplacement.Children.Insert(0, new Decorator(shouldPreformOutOfGameBehavior, sequenceOOG));
					hook.Value[0] = CompositeReplacement;

					Logger.DBLog.DebugFormat("Out of game tree hooked");
				}
				
				#endregion

				#region Death


				if (hook.Key.Contains("Death"))
				{
					Logger.DBLog.DebugFormat("Death...");


					Decorator deathDecorator = hook.Value[0] as Decorator;

					PrioritySelector DeathPrioritySelector = deathDecorator.Children[0] as PrioritySelector;

					//Insert Death Tally Counter At Beginning!
					CanRunDecoratorDelegate deathTallyDecoratorDelegate = EventHandlers.TallyDeathCanRunDecorator;
					ActionDelegate actionDelegatedeathTallyAction = EventHandlers.TallyDeathAction;
					Action deathTallyAction = new Action(actionDelegatedeathTallyAction);
					Decorator deathTallyDecorator = new Decorator(deathTallyDecoratorDelegate, deathTallyAction);
					DeathPrioritySelector.InsertChild(0, deathTallyDecorator);

					//Insert Death Tally Reset at End!
					Action deathTallyActionReset = new Action(ret => EventHandlers.TallyedDeathCounter = false);
					DeathPrioritySelector.InsertChild(7, deathTallyActionReset);

					Logger.DBLog.DebugFormat("Death tree hooked");

					//foreach (var item in DeathPrioritySelector.Children)
					//{
					//	Logger.DBLog.InfoFormat(item.GetType().ToString());
					//}


					/*
					 *  Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Decorator
						Zeta.TreeSharp.Action
					 * 
					*/


				}

				
				#endregion
			}
			#endregion
			initTreeHooks = true;
		}


		private static bool BlankDecorator(object ret)
		{
			return false;
		}
		private static RunStatus BlankAction(object ret)
		{
			return RunStatus.Success;
		}
	}

	public class TrinityStuckHandler : IStuckHandler
	{
		public Vector3 GetUnstuckPos()
		{
			return Vector3.Zero;
		}

		public bool IsStuck
		{
			get
			{
				return false;
			}
		}
	}
	public class TrinityCombatTargetingReplacer : ITargetingProvider
	{
		private static readonly List<DiaObject> listEmptyList = new List<DiaObject>();
		public List<DiaObject> GetObjectsByWeight()
		{
			if (!Bot.Targeting.Cache.DontMove)
				return listEmptyList;
			List<DiaObject> listFakeList = new List<DiaObject>();
			listFakeList.Add(null);
			return listFakeList;
		}
	}
	public class TrinityLootTargetingProvider : ITargetingProvider
	{
		private static readonly List<DiaObject> listEmptyList = new List<DiaObject>();
		public List<DiaObject> GetObjectsByWeight()
		{
			return listEmptyList;
		}
	}
	public class TrinityObstacleTargetingProvider : ITargetingProvider
	{
		private static readonly List<DiaObject> listEmptyList = new List<DiaObject>();
		public List<DiaObject> GetObjectsByWeight()
		{
			return listEmptyList;
		}
	}
}