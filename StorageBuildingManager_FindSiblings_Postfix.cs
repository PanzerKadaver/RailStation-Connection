using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Text;

using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Cities;
using VoxelTycoon.Tracks;

namespace RailStationConnection
{
	[HarmonyPatch(typeof(StorageNetworkBuilding), "InvalidateSiblings")]
	internal class StorageBuildingManager_FindSiblings_Postfix
	{
		private static void Postfix(StorageNetworkBuilding __instance, List<StorageBuildingSibling> ____siblings)
		{
			// RailStationConnection.logger.Log("VoxelTycoon.Buildings.StorageNetworkBuilding.InvalidateSiblings() - Postfix");

			if ((__instance as VehicleStation) != null && (__instance as VehicleStation).VehicleType == VehicleType.Train)
			{
				if ((__instance as VehicleStation).SharedData.Freight && !RailStationConnection.FreightConnection)
					return;
				if ((__instance as VehicleStation).SharedData.Passenger && !RailStationConnection.PassengerConnection)
					return;

				City c = null;
				List<Building> siblingsList = new List<Building>();

				for (int i = 0; i < ____siblings.Count; i++)
				{
					Building b = ____siblings[i].Building;
					//string cn = (b.City != null) ? b.City.Name : "No City";
					bool hc = (b.City != null);

					// RailStationConnection.logger.Log("[" + i + "] : " + b.SharedData.DisplayName + "/" + Utils.GetBuildingType(b) + "/" + cn);

					if (hc)
					{
						c = b.City;
						break;
					}
				}

				if (c != null)
				{
					foreach (StorageBuildingSibling sibling in ____siblings)
						siblingsList.Add(sibling.Building);

					int count = 0;

					foreach (Building building in c.GetBuildings().ToList())
					{
						if (!siblingsList.Contains(building) && (building as StorageNetworkBuilding) != null)
						{
							____siblings.Add(new StorageBuildingSibling
							{
								Building = building as StorageNetworkBuilding,
								Distance = Xyz.Distance(building.Position, __instance.Position)
							});
							count++;
						}
					}

					// RailStationConnection.logger.Log("Link [" + count + "] buildings to " + __instance.SharedData.DisplayName);
				}
			}
		}
	}
}
