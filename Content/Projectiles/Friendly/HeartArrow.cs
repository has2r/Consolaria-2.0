using Consolaria.Content.Buffs;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class HeartArrow : ModProjectile {

        public override void SetDefaults() {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;
            Projectile.knockBack = 0f;

            Projectile.friendly = true;
            Projectile.tileCollide = true;


        }

        public override void AI() {
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(3)) {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X + 2f, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.HeartCrystal, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.25f);
                    Main.dust[dust].noGravity = true;
                }
            }

            Projectile.knockBack = 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if ((!target.buffImmune[BuffID.Confused] || target.type == NPCID.Gnome || target.type == NPCID.CyanBeetle || target.type == NPCID.CochinealBeetle || target.type == NPCID.LacBeetle || target.type == NPCID.Crab || target.type == NPCID.SeaSnail || target.type == NPCID.MeteorHead || target.type == NPCID.DarkCaster || target.type == NPCID.FireImp || target.type == NPCID.GoblinSorcerer || target.type == NPCID.Harpy || target.type == NPCID.Demon || target.type == NPCID.VoodooDemon || target.type == NPCID.AnglerFish || target.type == NPCID.Arapaima || target.type == NPCID.BloodFeeder || target.type == NPCID.CorruptGoldfish || target.type == NPCID.CrimsonGoldfish || target.type == NPCID.Piranha || target.type == NPCID.Shark || target.type == NPCID.Raven || target.type == NPCID.Vulture || target.type == NPCID.Squid || target.type == NPCID.BlueJellyfish || target.type == NPCID.GreenJellyfish || target.type == NPCID.PinkJellyfish || target.type == NPCID.BloodJelly || target.type == NPCID.FungoFish || target.type == NPCID.FloatyGross || target.type == NPCID.Gastropod || target.type == NPCID.IceElemental || target.type == NPCID.GraniteFlyer) && Main.rand.NextBool(2)) {
                target.AddBuff(BuffID.Lovestruck, 90);
                target.AddBuff(ModContent.BuffType<Stunned>(), 90);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            if (info.PvP)
                target.AddBuff(BuffID.Lovestruck, 90);
        }

        public override void OnKill(int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(5) && !Projectile.noDropItem) {
                    Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, Projectile.width, Projectile.height, ModContent.ItemType<Items.Weapons.Ammo.HeartArrow>());
                }
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.HeartCrystal, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f, 100, default, 1f);
            }
        }
    }
}