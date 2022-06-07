using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

namespace Consolaria.Content.Items.BossDrops.Ocram
{
    public class ShadowboundExoskeleton : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadowbound Exoskeleton");
            Tooltip.SetDefault("Allows the player to do rocket jump");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Generic;
            Item.damage = 60;
            Item.knockBack = 5.5f;

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<RocketJumpPlayer>().ocramJump = true;
            player.GetModPlayer<RocketJumpPlayer>().rocketJumpDamage = Item.damage;
            player.GetModPlayer<RocketJumpPlayer>().rocketJumpKnockBack = Item.knockBack;
        }
    }

    internal class RocketJumpPlayer : ModPlayer
    {
        public bool ocramJump;
        public int rocketJumpDamage, rocketTimer;
        public float rocketJumpKnockBack;

        private bool inAir;

        public override void ResetEffects()
         => ocramJump = false;

        public override void PreUpdateMovement() {
            if (Consolaria.OcramJumpKeybind.JustReleased && ocramJump) DoRocketJump();            
        }

        public override void UpdateEquips() {
            if (ocramJump) {
                if (rocketTimer > 0 && ((Player.gravDir == 1f && Player.velocity.Y < 0f) || (Player.gravDir == -1f && Player.velocity.Y > 0f)))  {
                    if (Player.gravDir == -1f) Player.height = -6;

                    var size = ((float)Player.jump / 50f + 1f) / 2f;
                    for (int i = 0; i < 3; i++) {
                        var dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2)), Player.width, 19, DustID.Shadowflame, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, default(Color), 1f * size);
                        Main.dust[dust].velocity *= 0.5f * size;
                        Main.dust[dust].fadeIn = 1.5f * size;
                        var dust2 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2)), Player.width, 32, DustID.Shadowflame, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 120, default(Color), 1f * size);
                        Main.dust[dust2].velocity *= 0.5f * size;
                        Main.dust[dust2].fadeIn = 1.5f * size;
                    }
                }
                if (Player.velocity.Y == 0f || Player.sliding) inAir = false;

                if (inAir) rocketTimer++;
                else rocketTimer = 0;
            }
        }

        void DoRocketJump() {
            if (!inAir) {
                int radius = 80;
                Vector2 dustPos = new(Player.position.X, Player.position.Y + Player.height / 2);
                Player.velocity.Y = -18;

                for (int i = 0; i < 30; i++) {
                    int dust = Dust.NewDust(dustPos, Player.width, 4, DustID.Shadowflame, 0f, -1.5f, 100, default(Color), 1.2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 4f;
                    dust = Dust.NewDust(dustPos, Player.width, 4, DustID.Shadowflame, 0f, -1.5f, 100, default(Color), 1.5f);
                    Main.dust[dust].velocity *= 2.5f;
                    if (Main.dust[dust].position != Player.Center)
                        Main.dust[dust].velocity = Player.DirectionTo(Main.dust[dust].position) * 5f;
                }
                for (int g = 0; g < 5; g++) {
                    int goreIndex = Gore.NewGore(null, dustPos, default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].GetAlpha(new Color(75, 0, 130, 100));
                }

                for (int _npc = 0; _npc < Main.maxNPCs; _npc++) {
                    NPC npc = Main.npc[_npc];
                    if (npc.active && !npc.friendly && npc.life > 0 && npc.Distance(Player.Center) <= radius) {
                        npc.StrikeNPCNoInteraction(rocketJumpDamage, rocketJumpKnockBack, 0, false, false, false);
                        npc.AddBuff(BuffID.ShadowFlame, 180);
                    }
                }
                SoundEngine.PlaySound(SoundID.Item14, Player.position);
            }
            inAir = true;
        }  
    }
}