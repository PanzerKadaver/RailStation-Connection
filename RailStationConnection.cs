using System.Collections.Generic;

using HarmonyLib;

namespace RailStationConnection
{
	public class RailStationConnection : VoxelTycoon.Modding.Mod
	{
		public static VoxelTycoon.Logger logger = new VoxelTycoon.Logger<RailStationConnection>();
		public static bool RailStationFlag = false;
		protected override void Initialize()
		{
			Harmony harmony = new Harmony("PZKD.RailStationConnection");
			harmony.PatchAll();

			logger.Log("Initialized !");
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

	[HarmonyPatch(typeof(VoxelTycoon.Buildings.StorageNetworkBuilding), "InvalidateSiblings")]
	internal class StorageBuildingManager_FindSiblings_Postfix
	{
		private static void Postfix(VoxelTycoon.Buildings.StorageNetworkBuilding __instance, List<VoxelTycoon.Buildings.StorageBuildingSibling> ____siblings)
		{
			// RailStationConnection.logger.Log("VoxelTycoon.Buildings.StorageNetworkBuilding.InvalidateSiblings() - Postfix");

			if ((__instance as VoxelTycoon.Tracks.VehicleStation) != null && (__instance as VoxelTycoon.Tracks.VehicleStation).VehicleType == VoxelTycoon.Tracks.VehicleType.Train)
			{
				VoxelTycoon.Cities.City c = null;
				List<VoxelTycoon.Buildings.Building> siblingsList = new List<VoxelTycoon.Buildings.Building>();

				for (int i = 0; i < ____siblings.Count; i++)
				{
					VoxelTycoon.Buildings.Building b = ____siblings[i].Building;
					string cn = (b.City != null) ? b.City.Name : "No City";
					bool hc = (b.City != null) ? true : false;

					// RailStationConnection.logger.Log("[" + i + "] : " + b.SharedData.DisplayName + "/" + Utils.GetBuildingType(b) + "/" + cn);

					if (hc)
					{
						c = b.City;
						break;
					}
				}

				if (c != null)
				{
					foreach(VoxelTycoon.Buildings.StorageBuildingSibling sibling in ____siblings)
						siblingsList.Add(sibling.Building);

					int count = 0;

					foreach(VoxelTycoon.Buildings.Building building in c.GetBuildings().ToList())
					{
						if (!siblingsList.Contains(building) && (building as VoxelTycoon.Buildings.StorageNetworkBuilding) != null)
						{
							____siblings.Add(new VoxelTycoon.Buildings.StorageBuildingSibling
							{
								Building = building as VoxelTycoon.Buildings.StorageNetworkBuilding,
								Distance = VoxelTycoon.Xyz.Distance(building.Position, __instance.Position)
							});
							count++;
						}
					}

					// RailStationConnection.logger.Log("Link [" + count + "] buildings to " + __instance.SharedData.DisplayName);
				}
			}
		}
	}

	public static class Utils
	{
		public static string GetBuildingType(VoxelTycoon.Buildings.Building b)
		{
			if ((b as VoxelTycoon.Buildings.House) != null)
				return "House";
			if ((b as VoxelTycoon.Buildings.Mine) != null)
				return "Mine";
			if ((b as VoxelTycoon.Buildings.Plant) != null)
				return "Plant";
			if ((b as VoxelTycoon.Buildings.Store) != null)
				return "Store";
			if ((b as VoxelTycoon.Buildings.Warehouse) != null)
				return "Warehouse";
			return "Unknow";
		}
	}
}
