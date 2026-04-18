using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.HandsOn)]
    public class ValentineRing : ModItem {
        private static string SAVEKEY => nameof(Consolaria) + "valentinering";
        private static string SPECIALKEY1 => nameof(ValentineRing) + "isInOwnerInventory2";
        private static string SPECIALKEY2 => nameof(ValentineRing) + "isNotInOwnerInventory2";

        private string _ownerName = string.Empty;

        public bool HasOwner => !_ownerName.Equals(string.Empty);

        public bool IsInOwnerInventory(Player owner) => _ownerName.Equals(owner.name);

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            if (!HasOwner) {
                return;
            }

            bool isInOwnerInventory = IsInOwnerInventory(Main.LocalPlayer);
            Color specialTooltipColor_Red = new(205, 75, 115);
            Color specialTooltipColor_Gray = new(130, 130, 130);

            if (isInOwnerInventory) {
                tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isInOwnerInventory", Language.GetTextValue("Mods.Consolaria.ValentineRingTooltip1")));
                TooltipLine specialTooltip = new(Mod, SPECIALKEY1, Language.GetText("Mods.Consolaria.ValentineRingTooltip3").Format(_ownerName));
                specialTooltip.OverrideColor = specialTooltipColor_Gray;
                specialTooltip.Text += "      ";
                tooltips.Add(specialTooltip);
                return;
            }
            {
                tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isNotInOwnerInventory", Language.GetTextValue("Mods.Consolaria.ValentineRingTooltip2")));
                TooltipLine specialTooltip = new(Mod, SPECIALKEY2, Language.GetText("Mods.Consolaria.ValentineRingTooltip4").Format(_ownerName));
                specialTooltip.OverrideColor = specialTooltipColor_Red;
                specialTooltip.Text += "      ";
                tooltips.Add(specialTooltip);
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name.Equals(SPECIALKEY2)) {
                Texture2D icon = TextureAssets.Item[ItemID.Heart].Value;
                float x = line.X;
                Vector2 getPosition(float waveOffset = 0f) => new Vector2(x, line.Y) + new Vector2(10, 10 + (float)Math.Sin(Main.timeForVisualEffects / 20f + waveOffset) * 2f);
                float scale = Math.Max(icon.Width, icon.Height) / 20f;
                Color color = Color.Lerp(line.OverrideColor.Value, Color.Lerp(line.OverrideColor.Value, GetShimmerGradient().MultiplyAlpha(1f), 0.25f), 1f);
                color = Color.Lerp(color, Color.Red, 0.25f);
                SpriteBatch batch = Main.spriteBatch;
                float maxRotation = 0.25f;
                float rotationWaveFrequency = 2.5f;
                float rgbFactor = 0.75f;
                for (float num5 = 0f; num5 < 1f; num5 += 0.25f) {
                    Vector2 vector2 = (num5 * ((float)Math.PI * 2f)).ToRotationVector2() * 2f * scale;
                    batch.Draw(icon, getPosition(0f) + vector2, null, (color.MultiplyAlpha(0f) * 0.125f).ModifyRGB(rgbFactor), Helper.Wave(-maxRotation, maxRotation, rotationWaveFrequency, 0f), icon.Size() / 2, scale, default, 0);
                }
                batch.Draw(icon, getPosition(0f), null, (color.MultiplyAlpha(0f) * 1f).ModifyRGB(rgbFactor), Helper.Wave(-maxRotation, maxRotation, rotationWaveFrequency, 0f), icon.Size() / 2, scale, default, 0);
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X + 20f, line.Y), color.MultiplyAlpha(0.5f));
                Vector2 offset = Vector2.UnitX * FontAssets.MouseText.Value.MeasureString(line.Text).X + Vector2.UnitX * -20f;
                for (float num5 = 0f; num5 < 1f; num5 += 0.25f) {
                    Vector2 vector2 = (num5 * ((float)Math.PI * 2f)).ToRotationVector2() * 2f * scale;
                    batch.Draw(icon, getPosition(2f) + offset + vector2, null, (color.MultiplyAlpha(0f) * 0.125f).ModifyRGB(rgbFactor), Helper.Wave(-maxRotation, maxRotation, rotationWaveFrequency, 2f), icon.Size() / 2, scale, default, 0);
                }
                batch.Draw(icon, getPosition(2f) + offset, null, (color.MultiplyAlpha(0f) * 1f).ModifyRGB(rgbFactor), Helper.Wave(-maxRotation, maxRotation, rotationWaveFrequency, 2f), icon.Size() / 2, scale, default, 0);

                return false;
            }

            return base.PreDrawTooltipLine(line, ref yOffset);
        }

        public static Color GetShimmerGradient() {
            float factor = MathF.Sin((float)Main.timeForVisualEffects * 0.1f) * 0.5f + 0.5f;
            Color color = Color.Lerp(Color.Lerp(Color.White, new Color(150, 214, 245), factor), Color.Lerp(new Color(150, 214, 245), new Color(240, 146, 251), factor), factor);
            return color;
        }

        public override void SetDefaults() {
            int width = 30; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            bool isInOwnerInventory = IsInOwnerInventory(player);
            if (isInOwnerInventory) {
                return;
            }

            player.lifeRegen += 3;
            player.jumpSpeedBoost += 2.5f;
        }

        public override void SaveData(TagCompound tag) {
            tag[SAVEKEY] = _ownerName;
        }

        public override void LoadData(TagCompound tag) {
            _ownerName = tag.GetString(SAVEKEY);
        }

        public override void NetSend(BinaryWriter writer) {
            writer.Write(_ownerName);
        }

        public override void NetReceive(BinaryReader reader) {
            _ownerName = reader.ReadString();
        }

        public override void UpdateInventory(Player player) {
            if (HasOwner) {
                return;
            }

            _ownerName = player.name;
        }
    }
}