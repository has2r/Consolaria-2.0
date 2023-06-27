using Terraria.ModLoader;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Weapons.Melee;
using static Terraria.Graphics.FinalFractalHelper;
using System.Reflection;
using System.Collections.Generic;

namespace Consolaria {
	public class Consolaria : Mod {
		public override void Load()
		{
			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			fractalProfiles.Add(ModContent.ItemType<Tizona>(), new FinalFractalProfile(70f, new Color(132, 122, 224))); //Color up for debate
		}

		public override void Unload()
		{
			var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

			fractalProfiles.Remove(ModContent.ItemType<Tizona>());
		}

		//Basically whats happening here is that we are reflecting the '_fractalProfiles' dictionary in Terraria.Graphics.FinalFractalHelper
		//Then we are inserting our own dictionary list that states the Tizona item ID, its size (we stick with 70f in this case) and select a color for its trail to be based off
		//we then load and the unload it to not have either an unloaded item or amisplaced item from another mod being used on the zenith when consolaria is unloaded/loaded
		//more can be added to this list by adding another fractalProfiles.Add but make sure there is a fractalProfiles.Remove as well to make sure its getting unloaded

		//This has to be loaded an unloaded here or else it will never load and the Tizona will never show up in the zenith. Trust me I have already tried
	}
}