using Consolaria.Content.Projectiles;
using Consolaria.Content.Projectiles.Friendly.Pets;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria {
	public class Consolaria : Mod {
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
}