using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets;

public sealed class AlbinoRunt : ModProjectile {
    public override void SetStaticDefaults() {
        Main.projPet[Type] = true;

        Main.projFrames[Type] = 6;
        ProjectileID.Sets.LightPet[Type] = false;

        ProjectileID.Sets.CharacterPreviewAnimations[Type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Type])
            .WithOffset(-12f, -36f)
            .WithSpriteDirection(-1)
            .WithCode(DelegateMethods.CharacterPreview.EtsyPet);
    }

    public override void SetDefaults() {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        Projectile.netImportant = true;
        Projectile.timeLeft *= 5;
        Projectile.friendly = true;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.manualDirectionChange = true;
    }

    public override void AI() {
        Player player = Main.player[Projectile.owner];
        float num = 4f;
        int num2 = 4;
        int num3 = 4;
        int num4 = Main.projFrames[Projectile.type];
        int num5 = 0;
        float num6 = 0.08f;
        bool flag = false;
        float num7 = 0.1f;
        Vector2 vector = new Vector2(player.direction * 15, -40f);
        if (player.dead) {
            Projectile.Kill();
            return;
        }

        bool flag2 = true;

        num7 = 0.5f;
        num6 = 0.1f;

        num7 *= 0.5f;
        num6 *= 0.5f;

        flag = true;
        Projectile.localAI[0] += 1f;
        if (Projectile.localAI[0] > 120f)
            Projectile.localAI[0] = 0f;

        Projectile.localAI[1] += Projectile.velocity.X * 0.01f;
        Projectile.localAI[1] += 1f / 120f;
        if (Projectile.localAI[1] < (float)Math.PI * -2f)
            Projectile.localAI[1] += (float)Math.PI * 2f;

        if (Projectile.localAI[1] > (float)Math.PI * 2f)
            Projectile.localAI[1] -= (float)Math.PI * 2f;

        //if (velocity.Length() < 4f) {
        //    localAI[1] *= 0.9f;
        //    if (velocity.Length() > 0.1f && Main.rand.Next(30) == 0) {
        //        Dust dust = Dust.NewDustDirect(position - velocity, width, height, 292, velocity.X * 0.5f, velocity.Y * 0.5f, 150);
        //        dust.velocity *= 0.3f;
        //        dust.noLightEmittence = true;
        //    }
        //}
        //else {
        //    Vector2 vector2 = new Vector2(Main.screenWidth, Main.screenHeight);
        //    base.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector2 / 2f, vector2 + new Vector2(400f)));
        //    if (Main.rand.Next(15) == 0)
        //        Dust.NewDustDirect(position - velocity, width, height, 292, velocity.X * 0.5f, velocity.Y * 0.5f, 150, default(Color), 0.9f).noLightEmittence = true;
        //}

        float num8 = Projectile.localAI[0] / 120f * 2f;
        if (num8 > 1f)
            num8 = 2f - num8;

        //Projectile.Opacity = MathHelper.Lerp(0.4f, 0.75f, num8);
        Projectile.Opacity = 1f;
        vector.Y += (float)Math.Cos(Projectile.localAI[0] / 120f * ((float)Math.PI * 2f)) * 2f;
        if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.AlbinoRuntBuff>()))
            Projectile.timeLeft = 2;

        //if (flag2 && (player.suspiciouslookingTentacle || player.petFlagDD2Ghost))
        //    vector.X += -player.direction * 64;

        //if (MathF.Abs(Projectile.velocity.X) > 0.1f) {
        //    Projectile.direction = Projectile.spriteDirection = -(Projectile.velocity.X > 0f).ToDirectionInt();
        //}
        Projectile.direction = Projectile.spriteDirection = -player.direction;

        vector.X -= 13f;

        Vector2 vector4 = player.GetPlayerCorePoint() + vector;
        float num9 = Vector2.Distance(Projectile.Center, vector4);
        if (num9 > 1000f)
            Projectile.Center = player.GetPlayerCorePoint() + vector;

        Vector2 vector5 = vector4 - Projectile.Center;
        if (num9 < num)
            Projectile.velocity *= 0.25f;

        if (vector5 != Vector2.Zero) {
            if (vector5.Length() < num * 0.5f)
                Projectile.velocity = vector5;
            else
                Projectile.velocity = vector5 * num7;
        }

        if (Projectile.velocity.Length() > 6f) {
            float num10 = Projectile.velocity.X * num6 + Projectile.velocity.Y * (float)Projectile.spriteDirection * 0.02f;
            if (Math.Abs(Projectile.rotation - num10) >= (float)Math.PI) {
                if (num10 < Projectile.rotation)
                    Projectile.rotation -= (float)Math.PI * 2f;
                else
                    Projectile.rotation += (float)Math.PI * 2f;
            }

            float num11 = 6f;
            //Projectile.rotation = (Projectile.rotation * (num11 - 1f) + num10) / num11;
            if (++Projectile.frameCounter >= num3) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= num4)
                    Projectile.frame = num5;
            }
        }
        else {
            //if (Projectile.rotation > (float)Math.PI)
            //    Projectile.rotation -= (float)Math.PI * 2f;

            //if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
            //    Projectile.rotation = 0f;
            //else
            //    Projectile.rotation *= 0.96f;

            if (++Projectile.frameCounter >= num2) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= num4)
                    Projectile.frame = num5;
            }
        }

        Projectile.rotation = Projectile.velocity.Length() * 0.05f * -Projectile.direction;

        if (!flag) {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 120f)
                Projectile.localAI[0] = 0f;
        }
    }
}