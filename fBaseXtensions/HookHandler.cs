﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fBaseXtensions.Behaviors;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Bot.Navigation;
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

		internal static void HookBehaviorTree()
		{
			StoreHook(HookType.VendorRun);
			StoreHook(HookType.OutOfGame);
			StoreHook(HookType.Death);
			StoreHook(HookType.Combat);
			StoreHook(HookType.Loot);

			Logger.DBLog.InfoFormat("[Funky] Treehooks..");
			#region TreeHooks
			foreach (var hook in TreeHooks.Instance.Hooks)
			{

				#region OutOfGame

				if (hook.Key.Contains("OutOfGame"))
				{
					Logger.DBLog.DebugFormat("OutOfGame...");
					var outofgameHookValue = hook.Value[0];
					Logger.DBLog.InfoFormat(outofgameHookValue.GetType().ToString());

					PrioritySelector CompositeReplacement = hook.Value[0] as PrioritySelector;
					//PrintChildrenTypes(CompositeReplacement.Children);

					CanRunDecoratorDelegate shouldPreformOutOfGameBehavior = OutOfGame.OutOfGameOverlord;
					ActionDelegate actionDelgateOOGBehavior = OutOfGame.OutOfGameBehavior;
					Sequence sequenceOOG = new Sequence(
							new Zeta.TreeSharp.Action(actionDelgateOOGBehavior)
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

					//Insert Death Tally Reset at End!
					Action deathTallyActionReset = new Action(ret => DeathBehavior.TallyedDeathCounter = false);

					//Default Hook Value
					var deathValue = hook.Value[0];

					Sequence DeathSequence = new Sequence
					(
						deathTallyDecorator,
						deathWaitDecorator,
						deathValue,
						deathTallyActionReset
					);

					hook.Value[0] = DeathSequence;
					Logger.DBLog.DebugFormat("Death tree hooked");
				}


				#endregion
			}
			#endregion

			initTreeHooks = true;
		}

		internal static void ResetTreehooks()
		{
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

			if (combatValue[0] is PrioritySelector)
				return true;

			return false;
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

			var CombatValue = ReturnHookValue(HookType.Combat)[0];
			PrioritySelector hookedCombatSequence = new PrioritySelector
			(
				Precombat,
				CombatValue
			);

			//var coroutineComposite = new ActionRunCoroutine(ctx => PreCombat._PreCombatOverlord());
			SetHookValue(HookType.Combat, 0, hookedCombatSequence);
		}

		public enum HookType
		{
			Combat = 0,
			VendorRun,
			Loot,
			OutOfGame,
			Death
		}
		private static readonly Dictionary<HookType, Composite> OriginalHooks = new Dictionary<HookType, Composite>();
		public static void StoreHook(HookType type)
		{
			if (!OriginalHooks.ContainsKey(type))
			{
				var composite = TreeHooks.Instance.Hooks[type.ToString()][0];
				OriginalHooks.Add(type, composite);
				Logger.DBLog.DebugFormat("Stored Hook [{0}]", type.ToString());
			}
		}
		public static void RestoreHook(HookType type)
		{
			if (OriginalHooks.ContainsKey(type))
			{
				Logger.DBLog.DebugFormat("Restoring Hook [{0}]", type.ToString());
				TreeHooks.Instance.ReplaceHook(type.ToString(), OriginalHooks[type]);
				OriginalHooks.Remove(type);
			}
		}
		public static List<Composite> ReturnHookValue(HookType type)
		{
			return TreeHooks.Instance.Hooks[type.ToString()];
		}
		public static void SetHookValue(HookType type, int index, Composite value, bool insert = false)
		{
			if (!insert)
			{
				TreeHooks.Instance.Hooks[type.ToString()][index] = value;
				Logger.DBLog.DebugFormat("Replaced Hook [{0}]", type.ToString());
			}
			else
			{
				TreeHooks.Instance.Hooks[type.ToString()].Insert(index, value);
				Logger.DBLog.DebugFormat("Inserted composite for Hook [{0}] at index {1}", type.ToString(), index);
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