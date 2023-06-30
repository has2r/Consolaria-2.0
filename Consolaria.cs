using Consolaria.Content.Projectiles;
using Consolaria.Content.Projectiles.Friendly.Pets;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Weapons.Melee;
using static Terraria.Graphics.FinalFractalHelper;
using System.Reflection;
using System.Collections.Generic;

namespace Consolaria {
	public class Consolaria : Mod {
<<<<<<< HEAD
        public override void Load() {
            if (Main.dedServ) {
                return;
            }
            On_Player.DropTombstone += On_Player_DropTombstone;
        }

        private void On_Player_DropTombstone(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                return;
            }
            if (self.GetModPlayer<WormData>().IsWormPetActive) {
                Vector2 GetRandomTombstoneVelocity(int hitDirection) {
                    float num;
                    for (num = Main.rand.Next(-35, 36) * 0.1f; num < 2f && num > -2f; num += Main.rand.Next(-30, 31) * 0.1f)  {
                    }
                    return new Vector2(Main.rand.Next(10, 30) * 0.1f * hitDirection + num,
                                       Main.rand.Next(-40, -20) * 0.1f);
                }

                int projectile = Projectile.NewProjectile(new EntitySource_Death(self), self.Center, GetRandomTombstoneVelocity(hitDirection), ModContent.ProjectileType<WormTombstone>(), 0, 0f, Main.myPlayer);
                DateTime now = DateTime.Now;
                string str = now.ToString("D");
                if (GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) {
                    str = now.ToString("MMMM d, yyy");
                }
                string miscText = deathText.ToString() + "\n" + str;
                Main.projectile[projectile].miscText = miscText;
                return;
            }
            orig(self, coinsOwned, deathText, hitDirection);
        }
    }
=======
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
>>>>>>> main
}