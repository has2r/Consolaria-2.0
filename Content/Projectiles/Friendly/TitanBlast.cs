﻿using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class TitanBlast : ModProjectile {
        private object info;

        public override string Texture => "Consolaria/Assets/Textures/Projectiles/TitanBlast";

        public override void SetStaticDefaults()
            => Main.projFrames[Projectile.type] = 4;


        public override void SetDefaults() {
            int width = 92; int height = 164;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;

            Projectile.tileCollide = false;

            Projectile.netImportant = true;
        }

        public override bool ShouldUpdatePosition()
            => false;

        public override bool? CanCutTiles()
            => false;

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (Main.myPlayer == Projectile.owner) {
                Vector2 position = Vector2.Subtract(Main.MouseWorld, player.RotatedRelativePoint(player.MountedCenter, true));
                position.Normalize();
                if (!Utils.HasNaNs(position)) {
                    Projectile.velocity = position;
                    Projectile.netUpdate = true;
                }
                Projectile.Center = player.MountedCenter + Projectile.velocity * 100f + new Vector2(0f, -4f);
            }
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + 1.57f;
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            if (!player.dead && player.active) {
                if (Projectile.ai[0] == 0f) {
                    Projectile.ai[0] = 1f;
                    Vector2 center = Vector2.Add(Projectile.Center, Projectile.velocity * 30f);
                    Vector2 direction = Vector2.Subtract(player.Center, center);
                    Vector2 velocity = Vector2.Normalize(direction);
                    direction = Utils.HasNaNs(direction) ? Vector2.Zero : velocity;
                    if (player.velocity.Y == 0) player.velocity.X += direction.X * 12f;
                    else player.velocity.X += direction.X * 20f;
                    player.velocity.Y = player.velocity.Y * 0.2f + direction.Y * 20f;
                    if (Utils.HasNaNs(player.velocity)) {
                        player.velocity = Vector2.Zero;
                    }
                    Projectile.netUpdate = true;
                }
            }
            Lighting.AddLight(Projectile.Center, new Vector3(1.9f, 1.4f, 0.8f) * (4 - Projectile.frame) / 4);
            if (Projectile.frame >= 4) {
                Projectile.Kill();
                return;
            }
            else {
                if (++Projectile.frameCounter >= Main.projFrames[Projectile.type]) {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
            if (modifiers.PvP)
                modifiers.SetMaxDamage(350);
        }

        public override Color? GetAlpha(Color lightColor)
            => new Color(200, 200, 200, 75);
    }
}