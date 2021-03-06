﻿using System;
using System.Collections.Generic;
using fBaseXtensions.Behaviors;
using Zeta.Bot;
using Zeta.Common;
using Zeta.TreeSharp;
using Decorator = Zeta.TreeSharp.Decorator;
using Action = Zeta.TreeSharp.Action;
using Logger = fBaseXtensions.Helpers.Logger;

namespace fBaseXtensions
{
	public static class HookHandler
	{
		internal static bool initTreeHooks = false;
		private static Guid PrecombatCompositeGUID=Guid.Empty;

		internal static void HookBehaviorTree()
		{
            Logger.DBLog.InfoFormat("[Funky] Treehooks..");
            #region TreeHooks

            #region OutOfGame
            CanRunDecoratorDelegate shouldPreformOutOfGameBehavior = OutOfGame.OutOfGameOverlord;
            ActionDelegate actionDelgateOOGBehavior = OutOfGame.OutOfGameBehavior;
            Sequence sequenceOOG = new Sequence(
                    new Action(actionDelgateOOGBehavior)
            );
            var OutOfGameInsert = new Decorator(shouldPreformOutOfGameBehavior, sequenceOOG);
            SetHookValue(HookType.OutOfGame, 0, OutOfGameInsert, true);
            Logger.DBLog.DebugFormat("Out of game tree hooked");
            #endregion

            #region Death
            Logger.DBLog.DebugFormat("Death...");
            //Insert Death Tally Counter At Beginning!
            CanRunDecoratorDelegate deathTallyDecoratorDelegate = DeathBehavior.TallyDeathCanRunDecorator;
            ActionDelegate actionDelegatedeathTallyAction = DeathBehavior.TallyDeathAction;
            Action deathTallyAction = new Action(actionDelegatedeathTallyAction);
            Decorator deathTallyDecorator = new Decorator(deathTallyDecoratorDelegate, deathTallyAction);

            //Death Wait..
            CanRunDecoratorDelegate deathWaitDecoratorDelegate = DeathBehavior.DeathShouldWait;
            ActionDelegate deathWaitActionDelegate = DeathBehavior.DeathWaitAction;
            Action deathWaitAction = new Action(deathWaitActionDelegate);
            Decorator deathWaitDecorator = new Decorator(deathWaitDecoratorDelegate, deathWaitAction);


            Sequence DeathSequence = new Sequence
            (
                deathTallyDecorator,
                deathWaitDecorator
            );
            SetHookValue(HookType.Death, 0, DeathSequence, true);
            //hook.Value.Add(deathTallyActionReset);
            Logger.DBLog.DebugFormat("Death tree hooked");
            #endregion


            if (RoutineManager.Current.Name == "Funky")
            {
                CanRunDecoratorDelegate canRunDelegateCombatTargetCheck = CombatHandler.GlobalOverlord;
                ActionDelegate actionDelegateCoreTarget = CombatHandler.HandleTarget;
                Sequence sequencecombat = new Sequence
                (
                    new Action(actionDelegateCoreTarget)
                );
                var NewCombatComposite = new Decorator(canRunDelegateCombatTargetCheck, sequencecombat);
                SetHookValue(HookType.Combat, 0, NewCombatComposite);

                CanRunDecoratorDelegate canRunDelegateBlank = BlankDecorator;
                ActionDelegate actionDelegateBlank = BlankAction;
                Sequence sequenceblank = new Sequence(
                        new Action(actionDelegateBlank)
                        );

                var NewLootComposite = new Decorator(canRunDelegateBlank, sequenceblank);
                SetHookValue(HookType.Loot, 0, NewLootComposite);
            }

            #endregion

            initTreeHooks = true;
		}

	    internal static void StoreTreeHooks()
	    {
            StoreHook(HookType.VendorRun);
            StoreHook(HookType.OutOfGame);
            StoreHook(HookType.Death);
            StoreHook(HookType.Combat);
            StoreHook(HookType.Loot);
	    }
		internal static void ResetTreehooks()
		{
		    if (RoutineManager.Current.Name == "Funky")
		    {
                CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();
                LootTargeting.Instance.Provider = new DefaultLootTargetingProvider();
                ObstacleTargeting.Instance.Provider = new DefaultObstacleTargetingProvider();
		    }

			RestoreHook(HookType.VendorRun);
			RestoreHook(HookType.OutOfGame);
			RestoreHook(HookType.Death);
			RestoreHook(HookType.Combat);
			RestoreHook(HookType.Loot);
			initTreeHooks = false;
		}

		private static bool BlankDecorator(object ret) { return false; }
		private static RunStatus BlankAction(object ret) { return RunStatus.Success; }

		internal static bool CheckCombatHook()
		{
			var combatValue = ReturnHookValue(HookType.Combat);
			return combatValue[0].Guid == PrecombatCompositeGUID;
		}
		internal static void HookCombat()
		{
			CanRunDecoratorDelegate canRunDelegateCombatTargetCheck = PreCombat.PreCombatOverlord;
			ActionDelegate actionDelegateCoreTarget = PreCombat.HandleTarget;
			Sequence sequencecombat = new Sequence
			(
				new Zeta.TreeSharp.Action(actionDelegateCoreTarget)
			);
			Decorator Precombat = new Decorator(canRunDelegateCombatTargetCheck, sequencecombat);
			//Record GUID for checking
			PrecombatCompositeGUID = Precombat.Guid;
			//Insert precombat!
			SetHookValue(HookType.Combat, 0, Precombat, true);
		}

		public enum HookType
		{
			Combat = 0,
			VendorRun,
			Loot,
			OutOfGame,
			Death
		}
		private static readonly Dictionary<HookType, List<Composite>> OriginalHooks = new Dictionary<HookType, List<Composite>>();
		public static void StoreHook(HookType type)
		{
			if (!OriginalHooks.ContainsKey(type))
			{
				List<Composite> newList = new List<Composite>();

				var Startcomposite = TreeHooks.Instance.Hooks[type.ToString()][0];
				newList.Add(Startcomposite);

				if (TreeHooks.Instance.Hooks[type.ToString()].Count>1)
				{
					for (int i = 1; i < TreeHooks.Instance.Hooks[type.ToString()].Count; i++)
					{
						var additionalComposite = TreeHooks.Instance.Hooks[type.ToString()][i];
						newList.Add(additionalComposite);
					}
				}
				
				OriginalHooks.Add(type, newList);
				Logger.DBLog.DebugFormat("Stored Hook [{0}] with {1} composite(s)", type.ToString(), newList.Count);
			}
		}
		public static void RestoreHook(HookType type)
		{
			if (OriginalHooks.ContainsKey(type))
			{
				if (!TreeHooks.Instance.Hooks.ContainsKey(type.ToString()))
				{
					Logger.DBLog.DebugFormat("Adding Original Hook [{0}]", type.ToString());
					TreeHooks.Instance.Hooks.Add(type.ToString(), OriginalHooks[type]);
				}
				else
				{
					Logger.DBLog.DebugFormat("Restoring Original Hook [{0}]", type.ToString());
					TreeHooks.Instance.Hooks[type.ToString()].Clear();
					for (int i = 0; i < OriginalHooks[type].Count; i++)
					{
						var composite = OriginalHooks[type][i];
						TreeHooks.Instance.Hooks[type.ToString()].Add(composite);
					}
				}

				OriginalHooks.Remove(type);
			}
		}
		public static List<Composite> ReturnHookValue(HookType type)
		{
			return TreeHooks.Instance.Hooks[type.ToString()];
		}
		public static bool SetHookValue(HookType type, int index, Composite value, bool insert = false)
		{
		    if (!TreeHooks.Instance.Hooks.ContainsKey(type.ToString())) return false;

			if (!insert)
			{
				TreeHooks.Instance.Hooks[type.ToString()][index] = value;
				Logger.DBLog.DebugFormat("Replaced Hook [{0}]", type.ToString());
                return true;
			}
			else
			{
				TreeHooks.Instance.Hooks[type.ToString()].Insert(index, value);
				Logger.DBLog.DebugFormat("Inserted composite for Hook [{0}] at index {1}", type.ToString(), index);
                return true;
			}
		}

		public static void PrintChildrenTypes(List<Composite> composites)
		{
			foreach (var composite in composites)
			{
				Logger.DBLog.DebugFormat(composite.GetType().ToString());
			}
		}
	}
}
