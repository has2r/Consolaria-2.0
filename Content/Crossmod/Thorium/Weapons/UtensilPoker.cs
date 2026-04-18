using Consolaria.Content.Crossmod.Thorium.Buffs;
using Consolaria.Content.Crossmod.Thorium.Dusts;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class UtensilPoker : ThoriumItem_ThrowerBase {
    private static ushort BASEATTACKSPEED_LEFTCLICK => 20;

    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(44, 36);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.Orange3, Item.sellPrice(gold: 1, silver: 15));
        Item.SetShootableValues<UtensilPoker_Throw>(15f);

        Item.SetWeaponValues(37, 5f, 6);
        Item.SetDefaultsToUsable(ItemUseStyleID.Swing, BASEATTACKSPEED_LEFTCLICK, showItemOnUse: false, autoReuse: true);
    }

    public override bool CanUseItem(Player player) {
        bool rightClick = player.altFunctionUse == 2;
        return !rightClick || (rightClick && !player.GetModPlayer<UtensilPoker_ChargeHandler>().Charged);
    }

    public override bool AltFunctionUse(Player player) {
        return true;
    }

    public override float UseAnimationMultiplier(Player player) {
        bool rightClick = player.altFunctionUse == 2;
        return rightClick ? 0.5f : 1f;
    }

    public override float UseTimeMultiplier(Player player) {
        bool rightClick = player.altFunctionUse == 2;
        return rightClick ? 0.5f : 1f;
    }

    public override bool? UseItem(Player player) {
        bool rightClick = player.altFunctionUse == 2;
        if (player.ItemAnimationJustStarted) {
            UtensilPoker_ChargeHandler handler = player.GetModPlayer<UtensilPoker_ChargeHandler>();
            if (rightClick) {
                player.GetThoriumPlayer().throwerExhaustion -= Item.useTime * 2;

                SoundEngine.PlaySound(SoundID.Item1 with { Pitch = 0.2f * handler.CurrentCharge }, player.Center);

                handler.Charge();
            }
            else {
                SoundEngine.PlaySound(SoundID.Item19 with { Pitch = 1f }, player.Center);
            }
        }

        return base.UseItem(player);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        bool rightClick = player.altFunctionUse == 2;
        if (rightClick) {
            return false;
        }

        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

    public override void SetThrowerValues(ref bool isThrowerNon, ref bool IsThrowerNeedle, ref bool IsThrowerTomahawk, ref bool IsThrowerCaltrop) {
        isThrowerNon = true;
    }

    public sealed class UtensilPoker_ChargeHandler : ModPlayer {
        public byte CurrentCharge { get; private set; }

        public bool Charged => CurrentCharge >= 5;

        public bool CanCharge() {
            Item selectedItem = Player.HeldItem;
            return !selectedItem.IsAir && selectedItem.type == ModContent.ItemType<UtensilPoker>();
        }

        public void Charge() {
            if (!CanCharge()) {
                return;
            }

            CurrentCharge++;
            if (CurrentCharge > 5) {
                CurrentCharge = 5;
            }

            CombatText.NewText(Player.getRect(), new Color(130, 130, 130, 255), CurrentCharge, false, false);
        }

        public void ResetCharge() => CurrentCharge = 0;

        public override void PostUpdateEquips() {
            if (!CanCharge()) {
                CurrentCharge = 0;
            }
        }
    }

    public sealed class UtensilPoker_Throw : ThoriumProjectile_ThrowerBase {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(10);

            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }

        public override bool? CanDamage() => false;
        public override bool? CanCutTiles() => false;

        public override void AI() {
            UtensilPoker_ChargeHandler handler = Projectile.GetOwnerAsPlayer().GetModPlayer<UtensilPoker_ChargeHandler>();

            if (Projectile.IsOwnerLocal()) {
                int smallForkType = ModContent.ProjectileType<UtensilPoker_Fork>(),
                    bigForkType = ModContent.ProjectileType<UtensilPoker_BigFork>();
                void throwFork(int type, Vector2 spawnOffset = default) {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                                                   Projectile.Center + spawnOffset,
                                                   Projectile.velocity,
                                                   type,
                                                   Projectile.damage,
                                                   Projectile.knockBack,
                                                   Projectile.owner,
                                                   0f,
                                                   Projectile.Center.X,
                                                   Projectile.Center.Y);
                }
                float rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                int direction = Projectile.direction;
                switch (handler.CurrentCharge) {
                    case 0:
                        throwFork(smallForkType);
                        break;
                    case 1:
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 8f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 8f);
                        break;
                    case 2:
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 16f);
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation) * 8f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 16f);
                        break;
                    case 3:
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 20f);
                        throwFork(bigForkType, -Vector2.UnitY.RotatedBy(rotation) * 10f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 20f);
                        break;
                    case 4:
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 20f);
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 36f +
                                                 Vector2.UnitY.RotatedBy(rotation) * 12f);
                        throwFork(bigForkType, -Vector2.UnitY.RotatedBy(rotation) * 10f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 20f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 36f +
                                                 Vector2.UnitY.RotatedBy(rotation) * 12f);
                        break;
                    case 5:
                        throwFork(bigForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 26f);
                        throwFork(smallForkType, -Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 46f +
                                                 Vector2.UnitY.RotatedBy(rotation) * 12f);
                        throwFork(bigForkType, -Vector2.UnitY.RotatedBy(rotation) * 10f);
                        throwFork(bigForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 26f);
                        throwFork(smallForkType, Vector2.UnitY.RotatedBy(rotation + MathHelper.PiOver2) * 46f +
                                                 Vector2.UnitY.RotatedBy(rotation) * 12f);
                        break;
                }
            }

            handler.ResetCharge();

            Projectile.Kill();
        }
    }

    public sealed class UtensilPoker_Fork : ThoriumProjectile_ThrowerBase {
        private static Point[] _forkMax8 = new Point[8];

        private static ushort TIMELEFT => Helper.SecondsToFrames(4);
        private static ushort COLLISIONCOPYTIME => Helper.SecondsToFrames(0.5f);

        private float _isStickingToTarget;
        private float _targetWhoAmI;
        private bool _fall;

        public float IsStickingToTarget {
            get => _isStickingToTarget;
            set => _isStickingToTarget = value;
        }

        public float TargetWhoAmI {
            get => _targetWhoAmI;
            set => _targetWhoAmI = value;
        }

        public Vector2 MainCenter {
            get => new Vector2(Projectile.ai[1], Projectile.ai[2]);
            set {
                Projectile.ai[1] = value.X;
                Projectile.ai[2] = value.Y;
            }
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(_isStickingToTarget);
            writer.Write(_targetWhoAmI);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            _isStickingToTarget = reader.ReadSingle();
            _targetWhoAmI = reader.ReadSingle();
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            if (hitbox.Width > 8 && hitbox.Height > 8) {
                hitbox.Inflate(-hitbox.Width / 8, -hitbox.Height / 8);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
            for (int i = 0; i < 4; i++) {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100);
                Main.dust[num].position = Projectile.Center + Main.rand.NextVector2Circular(10f, 10f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity += Projectile.velocity * 0.25f;
            }

            _isStickingToTarget = 1f;
            _targetWhoAmI = target.whoAmI;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;
            Projectile.friendly = false;

            Point[] bufferForScan = _forkMax8;
            KillOldestFork(Projectile.whoAmI, Projectile.type, target.whoAmI, bufferForScan);

            //if (target.ai[1] == (float)target.whoAmI) {
            //    target.AddBuff(BuffID.BoneJavelin, 900);
            //}

            if (_targetWhoAmI == (float)target.whoAmI) {
                target.AddBuff(ModContent.BuffType<UtensilPokerDebuff>(), 900);
            }
        }

        public static void KillOldestFork(int protectedProjectileIndex, int projectileType, int targetNPCIndex, Point[] bufferForScan) {
            int num = 0;
            for (int i = 0; i < 1000; i++) {
                if (i != protectedProjectileIndex && Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projectileType && (Main.projectile[i].ModProjectile as UtensilPoker_Fork)._isStickingToTarget == 1f && (Main.projectile[i].ModProjectile as UtensilPoker_Fork)._targetWhoAmI == (float)targetNPCIndex) {
                    bufferForScan[num++] = new Point(i, Main.projectile[i].timeLeft);
                    if (num >= bufferForScan.Length)
                        break;
                }
            }

            if (num < bufferForScan.Length)
                return;

            int num2 = 0;
            for (int j = 1; j < bufferForScan.Length; j++) {
                if (bufferForScan[j].Y < bufferForScan[num2].Y)
                    num2 = j;
            }

            Main.projectile[bufferForScan[num2].X].Kill();
        }

        public override void Load() {
            On_Projectile.HandleMovement += On_Projectile_HandleMovement;
            On_Projectile.UpdatePosition += On_Projectile_UpdatePosition;
        }

        private static Vector2 _tempPosition, _tempPosition2;

        private void On_Projectile_UpdatePosition(On_Projectile.orig_UpdatePosition orig, Projectile self, Vector2 wetVelocity) {
            if (self.type == ModContent.ProjectileType<UtensilPoker_Fork>() && self.timeLeft > TIMELEFT - COLLISIONCOPYTIME) {
                self.position = _tempPosition;
            }
            if (self.type == ModContent.ProjectileType<UtensilPoker_BigFork>() && self.timeLeft > TIMELEFT - COLLISIONCOPYTIME) {
                self.position = _tempPosition2;
            }
            orig(self, wetVelocity);
        }

        private void On_Projectile_HandleMovement(On_Projectile.orig_HandleMovement orig, Projectile self, Vector2 wetVelocity, out int overrideWidth, out int overrideHeight) {
            if (self.type == ModContent.ProjectileType<UtensilPoker_Fork>() && self.timeLeft > TIMELEFT - COLLISIONCOPYTIME) {
                _tempPosition = self.position;
                self.position = (self.ModProjectile as UtensilPoker_Fork).MainCenter - self.Size / 2;
            }
            if (self.type == ModContent.ProjectileType<UtensilPoker_BigFork>() && self.timeLeft > TIMELEFT - COLLISIONCOPYTIME) {
                _tempPosition2 = self.position;
                self.position = (self.ModProjectile as UtensilPoker_BigFork).MainCenter - self.Size / 2;
            }
            if (self.type == ModContent.ProjectileType<UtensilPoker_Fork>() && !self.active) {
                self.position = _tempPosition;
            }
            if (self.type == ModContent.ProjectileType<UtensilPoker_BigFork>() && !self.active) {
                self.position = _tempPosition2;
            }
            orig(self, wetVelocity, out overrideWidth, out overrideHeight);
        }

        public override void SetStaticDefaults() {
            Projectile.SetTrail(0, 5);
        }

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(30);

            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.penetrate = -1;

            Projectile.aiStyle = -1;

            Projectile.timeLeft = TIMELEFT;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = Projectile.width / 3;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void AI() {
            if (_isStickingToTarget == 0f) {
                if (Projectile.velocity.Length() < 15f && !_fall) {
                    _fall = true;
                }
                if (_fall) {
                    Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
                    if (Projectile.velocity.Y > 16f)
                        Projectile.velocity.Y = 16f;
                }
                else {
                    Projectile.velocity *= 0.98f;
                }
            }

            //Projectile.tileCollide = Projectile.timeLeft < TIMELEFT - 5;

            MainCenter += Projectile.velocity;

            if (_isStickingToTarget == 0f) {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else if (_isStickingToTarget == 1f) {
                Projectile.Opacity *= Utils.GetLerpValue(0f, 10, Projectile.timeLeft, true);

                Vector2 center17 = Projectile.Center;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                int num907 = 15;

                bool flag52 = false;
                bool flag53 = false;
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] % 30f == 0f)
                    flag53 = true;

                int num908 = (int)_targetWhoAmI;
                if (Projectile.localAI[0] >= (float)(60 * num907)) {
                    flag52 = true;
                }
                else if (num908 < 0 || num908 >= 200) {
                    flag52 = true;
                }
                else if (Main.npc[num908].active && !Main.npc[num908].dontTakeDamage) {
                    Projectile.Center = Main.npc[num908].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[num908].gfxOffY;
                    if (flag53)
                        Main.npc[num908].HitEffect(0, 1.0);
                }
                else {
                    flag52 = true;
                }

                if (flag52)
                    Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 vector = value.Bounds.Size() / 2f;

            SpriteEffects effects = Projectile.direction.ToSpriteEffects();

            if (_isStickingToTarget == 0f) {
                for (int i = 1; i < Projectile.oldPos.Length; i++) {
                    Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(value, Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition, null, color * 0.5f, Projectile.rotation, vector, Projectile.scale, effects);
                }
            }

            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, vector, Projectile.scale, effects);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Bleeding, Main.rand.Next(180, 300));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(BuffID.Bleeding, Main.rand.Next(180, 300));
        }

        public override void OnKill(int timeLeft) {
            if (_isStickingToTarget != 0f) {
                return;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Vector2 vector50 = Projectile.position;
            Vector2 vector51 = (Projectile.rotation - (float)Math.PI / 2f).ToRotationVector2();
            vector50 += vector51 * 16f;
            for (int num445 = 0; num445 < 14; num445++) {
                if (Main.rand.NextChance(0.25f)) {
                    continue;
                }
                int num446 = Dust.NewDust(vector50, Projectile.width, Projectile.height, ModContent.DustType<UtensilForkDust>());
                Vector2 velocity = Projectile.velocity * 0.1f;
                Main.dust[num446].velocity *= 0.75f;
                Main.dust[num446].position = (Main.dust[num446].position + Projectile.Center) / 2f - velocity * 20f;
                Main.dust[num446].velocity += velocity;
                Main.dust[num446].scale *= Main.rand.NextFloat(0.9f, 1.1f) * 1f;
                //Main.dust[num446].alpha = 50;
                Dust dust2 = Main.dust[num446];
                dust2.velocity += vector51 * 2f;
                dust2 = Main.dust[num446];
                dust2.velocity *= 0.5f;
                Main.dust[num446].noGravity = true;
                vector50 -= vector51 * 6f;
            }
        }
    }

    public sealed class UtensilPoker_BigFork : ThoriumProjectile_ThrowerBase {
        private static ushort TIMELEFT => Helper.SecondsToFrames(4);

        private bool _fall;

        public Vector2 MainCenter {
            get => new Vector2(Projectile.ai[1], Projectile.ai[2]);
            set {
                Projectile.ai[1] = value.X;
                Projectile.ai[2] = value.Y;
            }
        }

        public override void SetStaticDefaults() {
            Projectile.SetTrail(0, 5);
        }

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(42);

            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.penetrate = -1;

            Projectile.aiStyle = -1;

            Projectile.timeLeft = TIMELEFT;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = Projectile.width / 3;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void AI() {
            Projectile.penetrate = Projectile.maxPenetrate = -1;

            if (Projectile.velocity.Length() < 15f && !_fall) {
                _fall = true;
            }
            if (_fall) {
                Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
                if (Projectile.velocity.Y > 16f)
                    Projectile.velocity.Y = 16f;
            }
            else {
                Projectile.velocity *= 0.98f;
            }

            //Projectile.tileCollide = Projectile.timeLeft < TIMELEFT - 5;

            MainCenter += Projectile.velocity;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 vector = value.Bounds.Size() / 2f;

            SpriteEffects effects = Projectile.direction.ToSpriteEffects();

            for (int i = 1; i < Projectile.oldPos.Length; i++) {
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(value, Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition, null, color * 0.5f, Projectile.rotation, vector, Projectile.scale, effects);
            }

            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, vector, Projectile.scale, effects);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Bleeding, Main.rand.Next(180, 300));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(BuffID.Bleeding, Main.rand.Next(180, 300));
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Vector2 vector50 = Projectile.position;
            Vector2 vector51 = (Projectile.rotation - (float)Math.PI / 2f).ToRotationVector2();
            vector50 += vector51 * 16f;
            for (int num445 = 0; num445 < 14; num445++) {
                if (Main.rand.NextChance(0.25f)) {
                    continue;
                }
                int num446 = Dust.NewDust(vector50, Projectile.width, Projectile.height, DustID.Silver);
                Vector2 velocity = Projectile.velocity * 0.1f;
                Main.dust[num446].velocity *= 0.75f;
                Main.dust[num446].position = (Main.dust[num446].position + Projectile.Center) / 2f - velocity * 20f;
                Main.dust[num446].velocity += velocity;
                Main.dust[num446].scale *= Main.rand.NextFloat(0.9f, 1.1f) * 1f;
                Dust dust2 = Main.dust[num446];
                dust2.velocity += vector51 * 2f;
                dust2 = Main.dust[num446];
                dust2.velocity *= 0.5f;
                Main.dust[num446].noGravity = true;
                vector50 -= vector51 * 6f;
            }
        }
    }
}
