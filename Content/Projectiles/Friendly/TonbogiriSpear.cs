using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class TonbogiriSpear : ModProjectile
    {
        public override void SetStaticDefaults() 
            => DisplayName.SetDefault("Tonbogiri");
        
        public override void SetDefaults() {
            int width = 22; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hide = true;

            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.ignoreWater = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;

            Projectile.alpha = 255;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = vector;
            if (player.dead) {
                Projectile.Kill();
                return;
            }
            Projectile.ai[0]++;
            if (!player.frozen) {
                Projectile.spriteDirection = Projectile.direction = player.direction;
                Vector2 vector2 = vector;
                Projectile.alpha -= 127;
                if (Projectile.alpha < 0) Projectile.alpha = 0;      
                if (Projectile.localAI[0] > 0f) Projectile.localAI[0] -= 1f;
                
                float num = (float)player.itemAnimation / (float)player.itemAnimationMax;
                float num2 = 1f - num;
                float num3 = Projectile.velocity.ToRotation();
                float num4 = Projectile.velocity.Length();
                float num5 = 24f;
                Vector2 spinningpoint = new Vector2(1.5f, 0f).RotatedBy((float)Math.PI + num2 * ((float)Math.PI * 3f)) * new Vector2(num4, Projectile.ai[0]);
                Projectile.position += spinningpoint.RotatedBy(num3) + new Vector2(num4 + num5, 0f).RotatedBy(num3);
                Vector2 target = vector2 + spinningpoint.RotatedBy(num3) + new Vector2(num4 + num5 + 40f, 0f).RotatedBy(num3);
                Projectile.rotation = vector2.AngleTo(target) + (float)Math.PI / 4f * (float)player.direction;
                if (Projectile.spriteDirection == -1) Projectile.rotation += (float)Math.PI;
                
                vector2.DirectionTo(Projectile.Center);
                Vector2 value = vector2.DirectionTo(target);
                Vector2 vector3 = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                float num6 = 2f;
                for (int i = 0; (float)i < num6; i++) {
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, 171, 0f, 0f, 120);
                    dust.velocity = vector2.DirectionTo(dust.position) * 2f;
                    dust.position = Projectile.Center + vector3.RotatedBy(num2 * ((float)Math.PI * 2f) * 2f + (float)i / num6 * ((float)Math.PI * 2f)) * 8f;
                    dust.scale = 1f + 0.5f * Main.rand.NextFloat();
                    dust.velocity += vector3 * 3f;
                    dust.noGravity = true;
                }    
            }
            if (player.itemAnimation == 2) {
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            float f5 = Projectile.rotation - (float)Math.PI / 4f * (float)Math.Sign(Projectile.velocity.X) + ((Projectile.spriteDirection == -1) ? ((float)Math.PI) : 0f);
            float collisionPoint = 0f;
            float scaleFactor = -95f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + f5.ToRotationVector2() * scaleFactor, 23f * Projectile.scale, ref collisionPoint))
                return true;     
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Projectile.direction = ((Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1));
            target.AddBuff(BuffID.Venom, 90);
        }

        public override void OnHitPvp(Player target, int damage, bool crit) {
            Projectile.direction = ((Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1));
            target.AddBuff(BuffID.Venom, 90);
        }

        public override bool PreDraw(ref Color lightColor) {
            Player player = Main.player[Projectile.owner];
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2((Projectile.spriteDirection == 1) ? ((float)texture.Width + 8f) : (-8f), (player.gravDir == 1f) ? (-8f) : ((float)texture.Height + 8f));
            Vector2 position = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            float rotation = Projectile.rotation;
            var spriteEffects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (player.gravDir == -1f) {
                spriteEffects = SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * (float)(Projectile.spriteDirection);
            }
            spriteBatch.Draw(texture, position, null, lightColor, rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}