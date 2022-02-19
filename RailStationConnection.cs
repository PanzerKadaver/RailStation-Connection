using System.Collections.Generic;

using HarmonyLib;

using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Cities;
using VoxelTycoon.Modding;
using VoxelTycoon.Tracks;

namespace RailStationConnection
{
	public class RailStationConnection : Mod
	{
		public static Logger logger = new Logger<RailStationConnection>();
		public static bool RailStationFlag = false;
		public static bool FreightConnection = true;
		public static bool PassengerConnection = true;


		protected override void Initialize()
		{
			Harmony harmony = new Harmony("PZKD.RailStationConnection");
			harmony.PatchAll();

			logger.Log("Initialized !");
		}

		protected override void OnGameStarted()
		{
			FreightConnection = WorldSettings.Current.GetBool<RailStationConnectionSettings>(RailStationConnectionSettings.FreightConnectionKey);
			PassengerConnection = WorldSettings.Current.GetBool<RailStationConnectionSettings>(RailStationConnectionSettings.PassengerConnectionKey);
		}
	}

	/** DEBUG
	[HarmonyPatch(typeof(VoxelTycoon.Tracks.VehicleStation), "OnBuilt")]
	internal class VehicleStation_OnBuilt_Prefix
	{
		private static void Prefix(VoxelTycoon.Tracks.VehicleStation __instance)
		{
			//RailStationConnection.logger.Log("VoxelTycoon.Tracks.VehicleStation.OnBuilt() - Prefix");
			//RailStationConnection.logger.Log(__instance.VehicleType.ToString());

			if (__instance.VehicleType == VoxelTycoon.Tracks.VehicleType.Train)
			{
				RailStationConnection.logger.Log("RailStation flag ON");
				RailStationConnection.RailStationFlag = true;
			}
		}
	}
	**/

	/** DEBUG
	[HarmonyPatch(typeof(VoxelTycoon.Tracks.VehicleStation), "OnBuilt")]
	internal class VehicleStation_OnBuilt_Postfix
	{
		private static void Postfix(VoxelTycoon.Tracks.VehicleStation __instance)
		{
			RailStationConnection.logger.Log("VoxelTycoon.Tracks.VehicleStation.OnBuilt() - Postfix");

			if (RailStationConnection.RailStationFlag)
			{
				RailStationConnection.logger.Log("RailStation flag OFF");
				RailStationConnection.RailStationFlag = false;

				RailStationConnection.logger.Log("Children count : " + __instance.ChildrenCount); // Subpart of the building
				
				
				for (int i = 0; i < __instance.ChildrenCount; i++)
				{
					RailStationConnection.logger.Log("[" + i + "] : " + __instance.GetChild(i).SharedData.DisplayName);
				}
				
			}
		}
	}
	**/

	/** DEBUG
	[HarmonyPatch(typeof(VoxelTycoon.Buildings.StorageNetworkBuilding), "OnBuilt")]
	internal class StorageNetworkBuilding_OnBuilt_Postfix
	{
		private static void Postfix(VoxelTycoon.Buildings.StorageNetworkBuilding __instance, List<VoxelTycoon.Buildings.StorageBuildingSibling> ____siblings)
		{
			RailStationConnection.logger.Log("VoxelTycoon.Buildings.StorageNetworkBuilding.OnBuilt() - Postfix");

			if (RailStationConnection.RailStationFlag)
			{
				int sc = ____siblings.Count;

				RailStationConnection.logger.Log("Siblings count : " + sc); // Other buildings inside the building range
				for (int i = 0; i < sc; i++)
				{
					VoxelTycoon.Buildings.Building b = ____siblings[i].Building;
					string cn = (b.City != null) ? b.City.Name : "No City";

					RailStationConnection.logger.Log("[" + i + "] : " + b.SharedData.DisplayName + "/" + Utils.GetBuildingType(b) + "/" + cn);
				}
			}
		}
	}
	**/
}
