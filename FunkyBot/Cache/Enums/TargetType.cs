﻿using System;

namespace FunkyBot.Cache.Enums
{
	 ///<summary>
	 ///Used to describe the object type for target handling.
	 ///</summary>
	 [Flags]
	 public enum TargetType
	 {
		 None = 0,
		 Unit = 1,
		 Shrine = 2,
		 Interactable = 4,
		 Destructible = 8,
		 Barricade = 16,
		 Container = 32,
		 Item = 64,
		 Gold = 128,
		 Globe = 256,
		 Avoidance = 512,
		 Door = 1024,
		 LineOfSight = 2048,
		 NoMovement = 4096,
		 Fleeing = 8192,
		 ServerInteractable = 16384,
		 Backtrack = 32768,
		 PowerGlobe = 65536,
		 CursedShrine = 131072,
		 CursedChest = 262144,
		 NavigationPoint = 524288,

		 All = Unit | Shrine | Interactable | Destructible | Barricade | Container | Item | Gold | Globe | Avoidance | Door | PowerGlobe | CursedShrine | CursedChest | NavigationPoint,
		 Gizmo = Shrine | Interactable | Destructible | Barricade | Container | Door | CursedShrine | CursedChest,
		 Interactables = Shrine | Interactable | Door | Container | CursedShrine | CursedChest,
		 ServerObjects = Unit | Interactables | Destructible | Barricade,
		 AvoidanceMovements = Avoidance | Fleeing,
	 }
}