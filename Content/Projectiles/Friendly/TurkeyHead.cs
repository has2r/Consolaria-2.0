using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class TurkeyHead : ModProjectile {
        public override void SetStaticDefaults() {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;

            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(317);
            Projectile.aiStyle = 54;

            int width = 26; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI() {
            if (Projectile.frame > 3) Projectile.frame = 1;
            Player player = Main.player[Projectile.owner];
            if (!CheckActive(player)) return;
        }

        private bool CheckActive(Player player) {
            if (player.dead || !player.active) {
                player.ClearBuff(ModContent.BuffType<Buffs.WeirdTurkey>());
                return false;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.WeirdTurkey>())) Projectile.timeLeft = 2;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.tileCollide = false;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.tileCollide = false;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => OnDamage();

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => OnDamage();

        private void OnDamage() {
            Projectile.ai[1] = -1f;
            Projectile.netUpdate = true;
        }

        public override bool? CanCutTiles()
            => false;

        public override bool MinionContactDamage()
            => true;

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/TurkeyHeadNeck");

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 vector2_4 = mountedCenter - position;

            float height = texture.Height;
            bool flag = true;

            if (float.IsNaN(position.X) && float.IsNaN(position.Y)) flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y)) flag = false;
            while (flag) {
                if (vector2_4.Length() < height + 1.0) flag = false;
                else {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * height;
                    vector2_4 = mountedCenter - position;
                    Vector2 vector2_5 = position - Main.screenPosition;
                    Color neckColor = Lighting.GetColor((int)(position.X / 16), (int)(position.Y / 16f));
                    spriteBatch.Draw(texture, vector2_5, null, neckColor, 0, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
            return true;
        }
    }
}