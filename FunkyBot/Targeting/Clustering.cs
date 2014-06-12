﻿using FunkyBot.Cache.Objects;
using FunkyBot.Game;
using FunkyBot.Misc;
using FunkyBot.Movement.Clustering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunkyBot.Targeting
{
	public class Clustering
	{
		public Clustering()
		{
			TargetClusterCollection = new ClusterTargetCollection(TargetClusterConditions);
		}


		private readonly ClusterConditions TargetClusterConditions = new ClusterConditions(ProfileCache.ClusterSettingsTag.ClusterDistance, ProfileCache.ClusterSettingsTag.ClusterMaxDistance, ProfileCache.ClusterSettingsTag.ClusterMinimumUnitCount, false);
		private readonly ClusterTargetCollection TargetClusterCollection;
		internal List<int> ValidClusterUnits = new List<int>();
		internal List<UnitCluster> CurrentGroupClusters = new List<UnitCluster>();




		private DateTime LastClusterGroupingLogicRefresh = DateTime.MinValue;

		private Dictionary<ClusterConditions, ClusterCollection> AbilityClusters = new Dictionary<ClusterConditions, ClusterCollection>();
		///<summary>
		///Tracks Lists of Clusters for specific Conditions used during Ability Clustering.
		///</summary>
		internal List<UnitCluster> AbilityClusterCache(ClusterConditions CC)
		{
			if (!AbilityClusters.ContainsKey(CC))
			{
				Logger.Write(LogLevel.Cluster, "Creating new entry for ClusterConditions -- {0}", CC.ToString());
				AbilityClusters.Add(CC, new ClusterCollection(CC, 400, 200));
			}

			if (AbilityClusters[CC].ShouldUpdateClusters)
				AbilityClusters[CC].UpdateClusters();
			else
				AbilityClusters[CC].RefreshClusters();

			//Logger.DBLog.InfoFormat("Ability Clusters Found {0}", AbilityClusters[CC].Count.ToString());
			return AbilityClusters[CC].CurrentClusters;
		}


		internal void UpdateGroupClusteringVariables()
		{
			//grouping clustering
			if (Bot.Settings.Grouping.AttemptGroupingMovements)
			{
				if ((CurrentGroupClusters.Count == 0 && DateTime.Compare(LastClusterGroupingLogicRefresh, DateTime.Now) < 0)
					 || (CurrentGroupClusters.Count > 0 && DateTime.Now.Subtract(LastClusterGroupingLogicRefresh).TotalMilliseconds > 1250))
				{
					//Clear Clusters and Unit collection
					CurrentGroupClusters = new List<UnitCluster>();

					//Check if there are enough units present currently..
					if (Bot.Targeting.Cache.Environment.DistantUnits.Count > Bot.Settings.Grouping.GroupingMinimumUnitsInCluster)
					{

						//Update UnitCluster Collection
						CurrentGroupClusters = UnitCluster.RunKmeans(Bot.Targeting.Cache.Environment.DistantUnits, Bot.Settings.Grouping.GroupingClusterRadiusDistance)
							 .Where(cluster => cluster.ListUnits.Count >= Bot.Settings.Grouping.GroupingMinimumUnitsInCluster && cluster.NearestMonsterDistance <= Bot.Settings.Grouping.GroupingMaximumDistanceAllowed)
							 .OrderByDescending(cluster => cluster.NearestMonsterDistance).ToList();

						//if (Bot.SettingsFunky.LogGroupingOutput)
						//    Logger.Write(LogLevel.Cluster, "Updated Group Clusters. Count={0}", CurrentGroupClusters.Count.ToString());

						LastClusterGroupingLogicRefresh = DateTime.Now;
					}

				}
			}
		}

		///<summary>
		///The main cluster refresh -- creates general clustering to be used in more specific clustering checks.
		///</summary>
		internal void UpdateTargetClusteringVariables()
		{

			//normal clustering
			if (Bot.Settings.Cluster.EnableClusteringTargetLogic &&
				 (Bot.Character.Data.dCurrentHealthPct > Bot.Settings.Cluster.IgnoreClusterLowHPValue))
			{
				if (Bot.Targeting.Cache.Environment.UnitRAGUIDs.Count >= Bot.Settings.Cluster.ClusterMinimumUnitCount)
				{
					if (TargetClusterCollection.ShouldUpdateClusters)
						TargetClusterCollection.UpdateClusters();
				}
				else
					TargetClusterCollection.RefreshClusters();

				List<CacheUnit> units = TargetClusterCollection.RetrieveAllUnits();

				if (units.Count > 0)
					ValidClusterUnits = units.Select(u => u.RAGUID).ToList();
			}
		}


	}
}
