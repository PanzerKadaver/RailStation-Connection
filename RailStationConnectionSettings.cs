using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VoxelTycoon;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Modding;

namespace RailStationConnection
{
	public class RailStationConnectionSettings : SettingsMod
	{
		public const string FreightConnectionKey = "GlobalFreightConnection";
		public const string PassengerConnectionKey = "GlobalPassengerConnection";

		protected override void SetDefaults(WorldSettings worldSettings)
		{
			worldSettings.SetBool<RailStationConnectionSettings>(FreightConnectionKey, true);
			worldSettings.SetBool<RailStationConnectionSettings>(PassengerConnectionKey, true);
		}

		protected override void SetupSettingsControl(SettingsControl settingsControl, WorldSettings worldSettings)
		{
			settingsControl.AddToggle("Allow global Stores connection", null,
				worldSettings.GetBool<RailStationConnectionSettings>(FreightConnectionKey));
			settingsControl.AddToggle("Allow global Passengers connection", null,
				worldSettings.GetBool<RailStationConnectionSettings>(PassengerConnectionKey));
		}
	}
}
