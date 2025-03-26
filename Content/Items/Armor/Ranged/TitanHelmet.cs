using Consolaria.Content.Projectiles.Friendly;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using System;
using System.IO;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Head)]
    public class TitanHelmet : ModItem {
        public static Lazy<Asset<Texture2D>> helmetGlowmask;
        public override void Unload() => helmetGlowmask = null;

        public static LocalizedText SetBonusText {
            get; private set;
        }

        public override void SetStaticDefaults() {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientTitanHelmet>();
            SetBonusText = this.GetLocalization("SetBonus");

            if (!Main.dedServ) {
                helmetGlowmask = new(() => ModContent.Request<Texture2D>(Texture + "_Glow"));
                HeadGlowmask.RegisterData(Item.headSlot, new DrawLayerData() {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
                });
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            => Item.BasicInWorldGlowmask(spriteBatch, helmetGlowmask.Value.Value, new Color(255, 255, 255, 0) * 0.8f, rotation, scale);

        public override void SetDefaults() {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip(Player player) {
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<TitanMail>() || body.type == ModContent.ItemType<AncientTitanMail>())
            && (legs.type == ModContent.ItemType<TitanLeggings>() || legs.type == ModContent.ItemType<AncientTitanLeggings>());

        public override void ArmorSetShadows(Player player)
            => player.armorEffectDrawOutlinesForbidden = true;

        public override void UpdateArmorSet(Player player) {
            player.setBonus = SetBonusText.ToString();
            player.GetModPlayer<TitanPlayer>().titanPower = true;
        }
    }

    internal class TitanPlayer : ModPlayer {
        public bool titanPower, titanPower2;
        public float newMaxFallSpeed;

        public int titanBlastTimer;
        public readonly int titanBlastTimerLimit = 300;

        public override void SaveData(TagCompound tag) {
            tag.Add("titanPower", titanPower);
            tag.Add("titanPower2", titanPower2);
        }

        public override void LoadData(TagCompound tag) {
            titanPower = tag.GetBool("titanPower");
            titanPower2 = tag.GetBool("titanPower2");
        }

        public void ReceivePlayerSync(BinaryReader reader) {
            titanPower2 = reader.ReadBoolean();
        }

        public override void CopyClientState(ModPlayer targetCopy) {
            TitanPlayer clone = targetCopy as TitanPlayer;
            clone.titanPower2 = titanPower2;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)Consolaria.MessageType.TitanPower);
            packet.Write((byte)Player.whoAmI);
            packet.Write(titanPower2);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer) {
            TitanPlayer clone = (TitanPlayer)clientPlayer;

            if (titanPower2 != clone.titanPower2)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }

        public override void SetControls() {
            for (int i = 0; i < 4; i++) {
                bool JustPressed = false;
                switch (i) {
                    case 0:
                        JustPressed = (Player.controlDown && Player.releaseDown);
                        break;
                    case 1:
                        JustPressed = (Player.controlUp && Player.releaseUp);
                        break;
                }
                if (JustPressed && Player.doubleTapCardinalTimer[i] > 0 && JustPressed && Player.doubleTapCardinalTimer[i] < 15)
                    KeyDoubleTap(i);
            }
        }

        private void KeyDoubleTap(int keyDir) {
            int inputKey = 0;
            if (Main.ReversedUpDownArmorSetBonuses)
                inputKey = 1;
            if (keyDir == inputKey) {
                if (titanPower) {
                    titanPower2 = !titanPower2;
                    SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/TitanBlastReload");
                    SoundEngine.PlaySound(style with { Volume = 0.3f, Pitch = !titanPower2 ? 0.35f : -0.35f }, Player.Center);
                }
            }
        }

        public override void Initialize()
           => titanBlastTimer = titanBlastTimerLimit;

        public override void ResetEffects()
           => titanPower = false;

        public override void PostUpdateEquips() {
            if (titanPower2 && !titanPower) {
                titanPower2 = false;
            }

            if (!titanPower || titanPower2) return;

            if (titanBlastTimer == titanBlastTimerLimit) newMaxFallSpeed = 24;
            if (newMaxFallSpeed > 0) newMaxFallSpeed -= 1;
            Player.maxFallSpeed += newMaxFallSpeed;

            if (titanBlastTimer == 30) {
                SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/TitanBlastReload");
                SoundEngine.PlaySound(style with { Volume = 0.3f, PitchRange = (-0.25f, 0.25f) }, Player.Center);
            }
            if (titanBlastTimer > 0)
                titanBlastTimer--;
        }
    }

    internal class TitanArmorBonuses : GlobalItem {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            TitanPlayer modPlayer = player.GetModPlayer<TitanPlayer>();
            ushort type2 = (ushort)ModContent.ProjectileType<TitanBlast>();

            if (modPlayer.titanPower2) {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }

            if (modPlayer.titanPower && player.ownedProjectileCounts[type2] < 1 && item.DamageType == DamageClass.Ranged &&
                modPlayer.titanBlastTimer == 0) {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);
                int blastDamage = damage * 2;
                if (blastDamage > 500) blastDamage -= (blastDamage - 500) / 2;
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, type2, blastDamage, 7.5f, player.whoAmI);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                if (Main.netMode != NetmodeID.Server) {
                    Vector2 dustVel = new Vector2(10, 0).RotatedBy(velocity.ToRotation());
                    for (int i = 0; i <= 15; i++) {
                        int dust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke, dustVel.X * 0.4f, dustVel.Y * 0.4f, 120, default, Main.rand.NextFloat(0.5f, 1.5f));
                        int dust2 = Dust.NewDust(player.Center, 0, 0, DustID.SolarFlare, dustVel.X * 1.3f, dustVel.Y * 1.3f, 100, default, Main.rand.NextFloat(0.5f, 1.5f));
                        Main.dust[dust2].velocity = Main.dust[dust2].velocity.RotatedByRandom(0.8f);
                        Main.dust[dust2].noGravity = true;
                        Main.dust[dust2].noLight = false;
                        Main.dust[dust].fadeIn = Main.rand.NextFloat(0.4f, 1.4f);
                        Main.dust[dust2].fadeIn = Main.rand.NextFloat(0.4f, 1.4f);
                    }
                }
                modPlayer.titanBlastTimer = modPlayer.titanBlastTimerLimit;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player) {
            float dontConsumeAmmoChance = 0f;
            if (weapon.useAmmo >= 0) {
                if (player.armor[0].type == ModContent.ItemType<TitanHelmet>() || player.armor[0].type == ModContent.ItemType<AncientTitanHelmet>())
                    dontConsumeAmmoChance += 0.25f;
                if (player.armor[1].type == ModContent.ItemType<TitanMail>() || player.armor[1].type == ModContent.ItemType<AncientTitanMail>())
                    dontConsumeAmmoChance += 0.2f;
                if (player.armor[2].type == ModContent.ItemType<TitanLeggings>() || player.armor[2].type == ModContent.ItemType<AncientTitanLeggings>())
                    dontConsumeAmmoChance += 0.15f;
                return Main.rand.NextFloat() >= dontConsumeAmmoChance;
            }
            return true;
        }
    }
}