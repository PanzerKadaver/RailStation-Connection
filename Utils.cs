using System;
using System.Collections.Generic;
using System.Text;

using VoxelTycoon.Buildings;

namespace RailStationConnection
{
	public static class Utils
	{
		public static string GetBuildingType(Building b)
		{
			if ((b as House) != null)
				return "House";
			if ((b as Mine) != null)
				return "Mine";
			if ((b as Plant) != null)
				return "Plant";
			if ((b as Store) != null)
				return "Store";
			if ((b as Warehouse) != null)
				return "Warehouse";
			return "Unknow";
		}
	}
}
