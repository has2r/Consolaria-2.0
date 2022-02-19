﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class AlbinoMandible : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.IceBoomerang);

            int width = 30; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;

            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720;
        }

        public override void Kill(int timeLeft) {
            if (Projectile.owner == Main.myPlayer) SoundEngine.PlaySound(0, Projectile.Center, 0);   
        }
    }
}