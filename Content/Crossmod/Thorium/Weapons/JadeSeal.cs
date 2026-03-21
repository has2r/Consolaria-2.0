using Consolaria.Content.Crossmod.Thorium.Buffs;
using Consolaria.Content.Crossmod.Thorium.Dusts;

using Microsoft.Xna.Framework;

using System;
using System.IO;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class JadeSeal : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.staff[Type] = true;
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(54, 54);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<JadeSeal_Lamp>();

        Item.SetDefaultsToUsable(ItemUseStyleID.Shoot, 50, useSound: SoundID.Item34);

        Item.mana = 100;
    }

    public override bool AltFunctionUse(Player player) => true;

    public override void SetHealerValues(ref bool IsDarkHealer, ref HealType healType, ref int healAmount, ref bool healDisplay, ref bool isAHealerTool) {
        healType = HealType.AllyAndPlayer;
        healAmount = 1;
        healDisplay = true;
        isAHealerTool = true;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        position = player.GetViableMousePosition();
    }

    public override void ModifyManaCost(Player player, ref float reduce, ref float mult) {
        if (player.altFunctionUse == 2) {
            mult *= 0f;
        }
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        velocity = Vector2.Zero;

        int num = ModContent.ProjectileType<JadeSeal_Lamp>();
        int num2 = ModContent.ProjectileType<JadeSeal_GoldenLamp>();
        bool rightClicking = player.altFunctionUse == 2;
        for (int i = 0; i < Main.maxProjectiles; i++) {
            Projectile projectile = Main.projectile[i];
            if (projectile.active && projectile.owner == player.whoAmI && ((rightClicking && projectile.type == num) || (!rightClicking && projectile.type == num2))) {
                projectile.Kill();
            }
        }
        Vector2 position2 = position;
        position2.X -= 46 / 2;
        bool hasMainLamp = player.ownedProjectileCounts[num] > 0;
        if (rightClicking || !hasMainLamp) {
            Projectile.NewProjectileDirect(source, position2, velocity, num, 0, 0f, player.whoAmI);
        }
        position2.X += 46;
        if (!rightClicking) {
            Projectile.NewProjectileDirect(source, hasMainLamp ? position : position2, velocity, num2, 0, 0f, player.whoAmI);
        }

        return false;
    }

    public sealed class JadeSeal_Lamp : ThoriumProjectile_HealerBase {
        public override void SetStaticDefaults() {
            Projectile.SetFrameCount(4);
        }

        public override void SetHealerDefaults() {
            Projectile.SetSizeValues(46, 52);

            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, new Color(23, 246, 160).ToVector3());

            MakeJadeDusts(Projectile);

            Projectile.rotation = Projectile.velocity.X * 0.025f;

            int radius = 125;
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner) {
                IEntitySource source_FromThis = Projectile.GetSource_FromThis();
                foreach (Player player2 in Main.ActivePlayers) {
                    if (player2.active && !player2.InOpposingTeam(player) && !player2.dead && Projectile.Distance(player2.Center) < radius) {
                        player.AddBuff(ModContent.BuffType<JadeBuff>(), 2);
                    }
                }
            }

            Projectile.Animate(4);
        }

        public static void MakeJadeDusts(Projectile projectile, bool yellow = false) {
            int num4 = 300;
            Vector2 vector2 = new Vector2(projectile.Top.X, projectile.position.Y + (float)num4);
            for (int j = 0; j < 4; j++) {
                Vector2 vector3 = Main.rand.NextVector2Unit();
                if (!(Math.Abs(vector3.X) < 0.12f)) {
                    Vector2 targetPosition = projectile.Center + vector3 * new Vector2((projectile.height - num4) / 2);
                    Dust dust = Dust.NewDustDirect(targetPosition, 0, 0, yellow ? ModContent.DustType<JadeDust2>() : ModContent.DustType<JadeDust>(), 0f, 0f, 100);
                    dust.position = targetPosition;
                    dust.velocity = (vector2 - dust.position).SafeNormalize(Vector2.Zero);
                    dust.velocity += dust.position.DirectionTo(projectile.Center) * Main.rand.NextFloat(2.5f, 5f);
                    dust.scale = 0.7f + 0.7f * Main.rand.NextFloatDirection();
                    dust.fadeIn = 1f;
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.customData = projectile;
                }
            }
        }
    }

    public sealed class JadeSeal_GoldenLamp : ThoriumProjectile_HealerBase {
        private Vector2 _mousePosition;

        public ref float HealTime => ref Projectile.localAI[0];

        public override void SetStaticDefaults() {
            Projectile.SetFrameCount(4);
        }

        public override void SetHealerDefaults() {
            Projectile.SetSizeValues(46, 52);

            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, new Color(241, 206, 77).ToVector3());

            float healTime = 30f;
            if (++HealTime > healTime) {
                int radius = 125;
                Projectile.ThoriumHeal(1, radius, onHealEffects: true, bonusHealing: true, delegate {
                    SoundEngine.PlaySound(in SoundID.Item85, Projectile.position);
                }, null, -1, ignoreHealer: false);
                HealTime = 0f;
            }

            JadeSeal_Lamp.MakeJadeDusts(Projectile, true);

            if (Projectile.IsOwnerLocal()) {
                _mousePosition = Projectile.GetOwnerAsPlayer().GetViableMousePosition();
                Projectile.netUpdate = true;
            }

            Helper.InertiaMoveTowards(ref Projectile.velocity, Projectile.Center, _mousePosition, 20f, 2.5f, 60f);

            Projectile.rotation = Projectile.velocity.X * 0.025f;

            Projectile.Animate(4);
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.WriteVector2(_mousePosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            _mousePosition = reader.ReadVector2();
        }
    }
}
