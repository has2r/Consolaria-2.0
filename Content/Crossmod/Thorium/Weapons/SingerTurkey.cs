using Consolaria.Content.Crossmod.Thorium.Buffs;

using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

using ThoriumMod;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class SingerTurkey : ThoriumItem_BardBase {
    public override BardInstrumentType InstrumentType => BardInstrumentType.Other;

    public override void SetBardDefaults() {
        Item.SetSizeValues(62, 32);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<SingerTurkey_Summon>(0f);

        Item.SetWeaponValues(60, 5f);
        Item.SetDefaultsToUsable(ItemUseStyleID.Swing, 18);

        InspirationCost = 0;
    }

    public override bool? BardUseItem(Player player) {
        if (player.ItemAnimationJustStarted) {
            player.AddBuff(ModContent.BuffType<SingerTurkeyBuff>(), 2, false);
        }

        return base.BardUseItem(player);
    }

    public sealed class SingerTurkey_Summon : ThoriumProjectile_BardBase {
        private Vector2 _velocity;

        public override void SetStaticDefaults() {
            Projectile.SetFrameCount(7);
        }

        public override void SetBardDefaults() {
            Projectile.SetSizeValues(10);

            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.ignoreWater = true;

            Projectile.manualDirectionChange = true;
        }

        public override void AI() {
            if (!Projectile.GetOwnerAsPlayer().GetModPlayer<SingerTurkeyBuff_Handler>().IsEffectActive) {
                Projectile.Kill();
            }

            float num55 = 0.1f;
            Projectile.tileCollide = false;
            int num56 = 200;
            Vector2 vector6 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num57 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector6.X;
            float num58 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector6.Y;
            num58 -= Main.player[Projectile.owner].height / 2;
            num57 -= 40f * Main.player[Projectile.owner].direction;

            float num59 = (float)Math.Sqrt(num57 * num57 + num58 * num58);
            float num60 = 4f;
            float num61 = num59;
            float num62 = 2000f;
            bool num63 = num59 > num62;
            //if (num59 < (float)num56 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
            //    Projectile.ai[0] = 0f;
            //    if (_velocity.Y < -6f)
            //        _velocity.Y = -6f;
            //}

            //if (num59 < 4f) {
            //    _velocity.X = num57;
            //    _velocity.Y = num58;
            //    num55 = 0f;
            //}
            //else {
            //    //if (num59 > 350f) {
            //    //    num55 = 0.2f;
            //    //    num60 = 10f;
            //    //}

            //    //num59 = num60 / num59;
            //    //num57 *= num59;
            //    //num58 *= num59;
            //}

            if (num63) {
                //int num64 = 2;
                //for (int m = 0; m < 12; m++) {
                //    float speedX4 = 1f - Main.rand.NextFloat() * 2f;
                //    float speedY4 = 1f - Main.rand.NextFloat() * 2f;
                //    int num65 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, num64, speedX4, speedY4);
                //    Main.dust[num65].noLightEmittence = true;
                //    Main.dust[num65].noGravity = true;
                //}

                Projectile.Center = Main.player[Projectile.owner].GetPlayerCorePoint();
                _velocity = Vector2.Zero;
                if (Main.myPlayer == Projectile.owner)
                    Projectile.netUpdate = true;
            }

            if (_velocity.X < num57) {
                _velocity.X += num55;
                if (_velocity.X < 0f)
                    _velocity.X += num55;
            }

            if (_velocity.X > num57) {
                _velocity.X -= num55;
                if (_velocity.X > 0f)
                    _velocity.X -= num55;
            }

            if (_velocity.Y < num58) {
                _velocity.Y += num55;
                if (_velocity.Y < 0f)
                    _velocity.Y += num55;
            }

            if (_velocity.Y > num58) {
                _velocity.Y -= num55;
                if (_velocity.Y > 0f)
                    _velocity.Y -= num55;
            }

            Projectile.direction = -Main.player[Projectile.owner].direction;
            Projectile.spriteDirection = -((Main.player[Projectile.owner].Center.X - Projectile.Center.X) > 0).ToDirectionInt();
            Projectile.rotation = _velocity.Y * 0.05f * (float)(-Projectile.direction);

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, _velocity, 0.5f);

            Projectile.position += Projectile.velocity;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4) {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }

            if (Projectile.frame > 5)
                Projectile.frame = 0;

            //if (num61 >= 50f) {
            //    frameCounter++;
            //    if (frameCounter <= 6)
            //        return;

            //    frameCounter = 0;
            //    if (velocity.X < 0f) {
            //        if (frame < 2)
            //            frame++;

            //        if (frame > 2)
            //            frame--;
            //    }
            //    else {
            //        if (frame < 6)
            //            frame++;

            //        if (frame > 6)
            //            frame--;
            //    }
            //}
            //else {
            //    frameCounter++;
            //    if (frameCounter > 6) {
            //        frame += direction;
            //        frameCounter = 0;
            //    }

            //    if (frame > 7)
            //        frame = 0;

            //    if (frame < 0)
            //        frame = 7;
            //}
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool PreDraw(ref Color lightColor) {
            Projectile.QuickDrawAnimated(lightColor);

            return false;
        }

        public override bool? CanCutTiles() => false;
        public override bool? CanDamage() => false;
    }
}
