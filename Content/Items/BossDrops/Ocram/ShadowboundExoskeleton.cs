using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;

namespace Consolaria.Content.Items.BossDrops.Ocram {
    public class ShadowboundExoskeleton : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Shadowbound Exoskeleton");

            string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.DOWN" : "Key.UP");
            Tooltip.SetDefault("Allows the player to rocket jump" + $"\nDouble tap {tapDir}");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Melee;
            Item.damage = 90;
            Item.knockBack = 6f;

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateAccessory (Player player, bool hideVisual) {
            player.GetModPlayer<RocketJumpPlayer>().ocramJump = true;
            player.GetModPlayer<RocketJumpPlayer>().rocketJumpDamage = Item.damage;
            player.GetModPlayer<RocketJumpPlayer>().rocketJumpKnockBack = Item.knockBack;
        }
    }

    internal class RocketJumpPlayer : ModPlayer {
        public bool ocramJump;
        public int rocketJumpDamage;
        public float rocketJumpKnockBack;

        private bool rocketJumped;
        private int rocketTimer, rocketCooldown;

        public override void ResetEffects ()
            => ocramJump = false;

        public override void UpdateEquips () {
            if (ocramJump) {
                if (rocketTimer > 0 && ((Player.gravDir == 1f && Player.velocity.Y < 0f) || (Player.gravDir == -1f && Player.velocity.Y > 0f))) {
                    if (Player.gravDir == -1f) Player.height = -6;

                    float size = (Player.jump / 50f + 1f) / 2f;
                    for (int i = 0; i < 3; i++) {
                        var dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (Player.height / 2)), Player.width, 19, DustID.Shadowflame, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, default, 1f * size);
                        Main.dust [dust].velocity *= 0.5f * size;
                        Main.dust [dust].fadeIn = 1.5f * size;
                        var dust2 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + Player.height / 2), Player.width, 32, DustID.Shadowflame, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 120, default, 1f * size);
                        Main.dust [dust2].velocity *= 0.5f * size;
                        Main.dust [dust2].fadeIn = 1.5f * size;
                    }
                }
                //  if (Player.velocity.Y == 0f || Player.sliding) canJump = false;
                if (rocketCooldown > 50) {
                    rocketJumped = false;
                    rocketCooldown = 0;
                }

                if (rocketJumped) {
                    rocketTimer++;
                    rocketCooldown++;
                }
                else rocketTimer = 0;
            }
        }

        private void DoRocketJump () {
            if (rocketJumped)
                return;

            int radius = 120;
            Vector2 dustPos = new(Player.position.X, Player.position.Y + Player.height / 2);
            Player.velocity.Y = -18;
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 30; i++) {
                    int dust = Dust.NewDust(dustPos, Player.width, 4, DustID.Shadowflame, 0f, -1.5f, 100, default, 1.25f);
                    Main.dust [dust].noGravity = true;
                    Main.dust [dust].velocity *= 4f;
                    dust = Dust.NewDust(dustPos, Player.width, 4, DustID.Shadowflame, 0f, -1.5f, 50, default, 1.5f);
                    Main.dust [dust].velocity *= 2.5f;
                    if (Main.dust [dust].position != Player.Center)
                        Main.dust [dust].velocity = Player.DirectionTo(Main.dust [dust].position) * 5f;
                }
                for (int g = 0; g < 6; g++) {
                    int goreIndex = Gore.NewGore(Player.GetSource_Misc("Rocket_Jump"), dustPos, default, Main.rand.Next(61, 64), 1f);
                    Main.gore [goreIndex].GetAlpha(new Color(75, 0, 130, 100));
                }
            }

            for (int _npc = 0; _npc < Main.maxNPCs; _npc++) {
                NPC npc = Main.npc [_npc];
                if (npc.active && !npc.friendly && npc.life > 0 && !npc.dontTakeDamage && npc.Distance(Player.position) <= radius) {
                    npc.StrikeNPCNoInteraction(rocketJumpDamage, rocketJumpKnockBack, 0, false, false, false);
                    npc.AddBuff(BuffID.ShadowFlame, 180);
                }
            }
            SoundEngine.PlaySound(SoundID.Item14 with { Pitch = 0.1f, Volume = 0.7f}, Player.position);
            rocketJumped = true;
        }

        public override void SetControls () {
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
                if (JustPressed && Player.doubleTapCardinalTimer [i] > 0 && JustPressed && Player.doubleTapCardinalTimer [i] < 15)
                    KeyDoubleTap(i);
            }
        }

        private void KeyDoubleTap (int keyDir) {
            int inputKey = 1;
            if (Main.ReversedUpDownArmorSetBonuses)
                inputKey = 0;
            if (keyDir == inputKey) {
                if (ocramJump)
                    DoRocketJump();
            }
        }
    }
}