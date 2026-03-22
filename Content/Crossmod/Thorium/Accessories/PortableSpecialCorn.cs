using Consolaria.Content.Crossmod.Thorium.Buffs;

using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using System;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Items;

namespace Consolaria.Content.Crossmod.Thorium.Accessories;

public sealed class PortableSpecialCorn : ThoriumItem_BardBase {
    public override BardInstrumentType InstrumentType => BardInstrumentType.Other;

    public override void SetBardDefaults() {
        Item.DefaultToAccessory();

        Item.SetSizeValues(36, 34);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
    }

    public override void UpdateEquip(Player player) {
        player.AddBuff(ModContent.BuffType<SingerTurkeyBuff>(), 2);
    }

    public sealed class PortableSpecialCorn_Summon : ThoriumProjectile_BardBase {
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "ItemCheck_Shoot")]
        public extern static void Player_ItemCheck_Shoot(Player player, int i, Item sItem, int weaponDamage);

        private Vector2 _velocity;

        public ref float ShotValue => ref Projectile.ai[0];

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
                return;
            }

            bool flag = false;
            if (Projectile.localAI[2] == 0f) {
                Projectile.localAI[2] = 1f;

                SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble") with { Pitch = 0.5f }, Projectile.Center);

                flag = true;

                Projectile.direction = Projectile.spriteDirection = -(((Main.player[Projectile.owner].Center.X - 10f * Main.player[Projectile.owner].direction) - Projectile.Center.X) > 0).ToDirectionInt();
            }

            Projectile.timeLeft = 2;

            Player player = Projectile.GetOwnerAsPlayer();
            Item selectedItem = player.HeldItem;
            int animationTime = 30;
            if (Projectile.IsOwnerLocal() && SingerTurkeyBuff_Handler.JustUsedBardWeapon(player) && player.GetModPlayer<SingerTurkeyBuff_Handler>().BardAttackCount >= 4) {
                Vector2 position = player.Center;
                player.Center = Projectile.Center;
                Player_ItemCheck_Shoot(player, player.whoAmI, player.HeldItem, (int)(player.GetWeaponDamage(player.HeldItem) * 0.45f));
                player.Center = position;

                ShotValue = animationTime;
                Projectile.netUpdate = true;

                player.GetModPlayer<SingerTurkeyBuff_Handler>().BardAttackCount = 0;
            }

            if (ShotValue == animationTime) {
                SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble") with { Pitch = 0.5f }, Projectile.Center);
            }

            if (ShotValue > 0f) {
                ShotValue--;
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

            if (!flag) {
                Projectile.direction = -Main.player[Projectile.owner].direction;
                if (MathF.Abs(Main.player[Projectile.owner].Center.X - Projectile.Center.X) > 10f) 
                {
                    Projectile.spriteDirection = -(((Main.player[Projectile.owner].Center.X - 10f * Main.player[Projectile.owner].direction) - Projectile.Center.X) > 0).ToDirectionInt();
                }
            }
            Projectile.rotation = _velocity.Y * 0.05f * (float)(-Projectile.direction);

            Projectile.position.Y += Helper.Wave(-1f, 1f, 0.1f, Projectile.identity);

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, _velocity, 0.5f);

            Projectile.position += Projectile.velocity;

            if (ShotValue > 0f) {
                Projectile.frameCounter = 0;
                Projectile.frame = 6;

                return;
            }

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
