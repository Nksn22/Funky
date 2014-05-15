using System.IO;
using System.Xml.Serialization;
using FunkyBot.Movement.Clustering;

namespace FunkyBot.Config.Settings
{
	public class SettingGrouping
	{
		public bool AttemptGroupingMovements { get; set; }
		public double GroupingClusterRadiusDistance { get; set; }
		public int GroupingMinimumUnitDistance { get; set; }
		public int GroupingMaximumDistanceAllowed { get; set; }
		public int GroupingMinimumClusterCount { get; set; }
		public int GroupingMinimumUnitsInCluster { get; set; }
		public double GroupingMinimumBotHealth { get; set; }

        public ClusterProperties GroupingClusterProperties { get; set; }

		public SettingGrouping(bool enabled=true)
		{
			AttemptGroupingMovements=enabled;
			GroupingClusterRadiusDistance=10d;
			GroupingMinimumUnitDistance=35;
			GroupingMaximumDistanceAllowed=110;
			GroupingMinimumClusterCount=1;
			GroupingMinimumUnitsInCluster=3;
			GroupingMinimumBotHealth=0d;
            GroupingClusterProperties = ClusterProperties.Boss | ClusterProperties.Elites | ClusterProperties.Large | ClusterProperties.Strong | ClusterProperties.Fast;
		}
		public SettingGrouping()
		{
			AttemptGroupingMovements=false;
			GroupingClusterRadiusDistance=10d;
			GroupingMinimumUnitDistance=35;
			GroupingMaximumDistanceAllowed=110;
			GroupingMinimumClusterCount=1;
			GroupingMinimumUnitsInCluster=3;
			GroupingMinimumBotHealth=0d;
            GroupingClusterProperties = ClusterProperties.Boss | ClusterProperties.Elites | ClusterProperties.Large | ClusterProperties.Strong | ClusterProperties.Fast;
		}

		private static string DefaultFilePath=Path.Combine(FolderPaths.SettingsDefaultPath, "Specific", "Grouping_Default.xml");
		public static SettingGrouping DeserializeFromXML()
		{
			 XmlSerializer deserializer=new XmlSerializer(typeof(SettingGrouping));
			 TextReader textReader=new StreamReader(DefaultFilePath);
			 SettingGrouping settings;
			 settings=(SettingGrouping)deserializer.Deserialize(textReader);
			 textReader.Close();
			 return settings;
		}
		public static SettingGrouping DeserializeFromXML(string Path)
		{
			 XmlSerializer deserializer=new XmlSerializer(typeof(SettingGrouping));
			 TextReader textReader=new StreamReader(Path);
			 SettingGrouping settings;
			 settings=(SettingGrouping)deserializer.Deserialize(textReader);
			 textReader.Close();
			 return settings;
		}
	}
}