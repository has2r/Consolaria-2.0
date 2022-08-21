using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Summon {
    [AutoloadEquip(EquipType.Head)]
    public class WarlockHood : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Warlock Hood");
            Tooltip.SetDefault("9% increased minion damage" + "\nIncreases your max number of minions");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 7;
        }

        public override void UpdateEquip (Player player) {
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.09f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
           => body.type == ModContent.ItemType<WarlockRobe>() || body.type == ModContent.ItemType<AncientWarlockRobe>()
           && legs.type == ModContent.ItemType<WarlockLeggings>() || legs.type == ModContent.ItemType<AncientWarlockLeggings>();

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Killing enemies minions healing the player for a small amount of life";
            player.GetModPlayer<WarlockPlayer>().necroHealing = true;
        }

        public override void UpdateVanitySet (Player player)
            => Lighting.AddLight(player.Center, 0.5f, 0.3f, 0.7f);

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedHood)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class WarlockPlayer : ModPlayer {
        public bool necroHealing;

        private bool startCooldownTimer;
        private int setbonusCooldown;

        public override void ResetEffects ()
            => necroHealing = false;

        public override void UpdateEquips () {
            int cooldownTimeMax = 10;

            if (startCooldownTimer)
                setbonusCooldown++;

            if (setbonusCooldown >= cooldownTimeMax) {
                setbonusCooldown = 0;
                startCooldownTimer = false;
            }
        }

        public override void OnHitNPCWithProj (Projectile proj, NPC target, int damage, float knockback, bool crit) {
            if (target.type == NPCID.TargetDummy || startCooldownTimer)
                return;

            if (necroHealing && (proj.minion  || proj.DamageType == DamageClass.Summon) && target.life <= 0) {
                int helLife = Player.statLifeMax / 20;
                if (helLife > 0) {
                    float _dustCountMax = 40;
                    int _dustCount = 0;
                    while (_dustCount < _dustCountMax) {
                        Vector2 vector = Vector2.UnitX * 0f;
                        vector += -Vector2.UnitY.RotatedBy(_dustCount * (7f / _dustCountMax), default) * new Vector2(26f, 26);
                        vector = vector.RotatedBy(proj.velocity.ToRotation(), default);
                        int _dust = Dust.NewDust(proj.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 100, Color.DarkViolet, 1.2f);
                        Main.dust [_dust].noGravity = true;
                        Main.dust [_dust].position = proj.Center + vector;
                        Main.dust [_dust].velocity = proj.velocity * 0f + vector.SafeNormalize(Vector2.UnitY) * 0.8f;
                        int _dustCountMax2 = _dustCount;
                        _dustCount = _dustCountMax2 + 1;
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath55, proj.Center);
                    Player.statLife += helLife;
                    Player.HealEffect(helLife);
                    NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, proj.owner, helLife);
                    startCooldownTimer = true;
                }
            }
        }
    }
}