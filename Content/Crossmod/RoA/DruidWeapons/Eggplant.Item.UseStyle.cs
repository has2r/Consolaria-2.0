using Microsoft.Xna.Framework.Graphics;
using System;

using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed partial class Eggplant : ModItem {
    public override void Load() {
        On_PlayerDrawLayers.DrawPlayer_27_HeldItem += On_PlayerDrawLayers_DrawPlayer_27_HeldItem;
    }

    private void On_PlayerDrawLayers_DrawPlayer_27_HeldItem(On_PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawinfo) {
        if (drawinfo.heldItem.type == ModContent.ItemType<Eggplant>()) {
            if (!drawinfo.drawPlayer.JustDroppedAnItem) {
                if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
                    drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;

                Item heldItem = drawinfo.heldItem;
                int num = heldItem.type;
                if (drawinfo.drawPlayer.UsingBiomeTorches) {
                    switch (num) {
                        case 8:
                            num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
                            break;
                        case 966:
                            num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
                            break;
                    }
                }

                float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
                Main.instance.LoadItem(num);
                Texture2D value = TextureAssets.Item[num].Value;
                Vector2 position = new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y));
                Rectangle itemDrawFrame = drawinfo.drawPlayer.GetItemDrawFrame(num);
                drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
                bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
                bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
                if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
                    flag2 = false;

                if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || !(flag || flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
                    return;

                float num9 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
                float num10 = 0f;
                float num11 = 0f;
                Vector2 origin5 = new Vector2(0f, itemDrawFrame.Height);
                if (num == 3210) {
                    num10 = 8 * -drawinfo.drawPlayer.direction;
                    num11 = 2 * (int)drawinfo.drawPlayer.gravDir;
                }

                num11 = (int)((float)(24 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num9));

                if (num == 3870) {
                    Vector2 vector6 = (drawinfo.drawPlayer.itemRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
                    num10 = (int)vector6.X;
                    num11 = (int)vector6.Y;
                }

                if (num == 3787)
                    num11 = (int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num9));

                if (num == 3209) {
                    Vector2 vector7 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
                    num10 = vector7.X;
                    num11 = vector7.Y;
                }

                if (drawinfo.drawPlayer.gravDir == -1f) {
                    if (drawinfo.drawPlayer.direction == -1) {
                        num9 += 1.57f;
                        origin5 = new Vector2(itemDrawFrame.Width, 0f);
                        num10 -= (float)itemDrawFrame.Width;
                    }
                    else {
                        num9 -= 1.57f;
                        origin5 = Vector2.Zero;
                    }
                }
                else if (drawinfo.drawPlayer.direction == -1) {
                    origin5 = new Vector2(itemDrawFrame.Width, itemDrawFrame.Height);
                    num10 -= (float)itemDrawFrame.Width;
                }


                var drawPlayer = drawinfo.drawPlayer;
                var itemEffect = SpriteEffects.None;
                if (drawPlayer.gravDir == 1f) {
                    if (drawPlayer.direction == 1) {
                        itemEffect = SpriteEffects.FlipHorizontally;
                    }
                    else {
                        itemEffect = SpriteEffects.None;
                    }
                }
                else {
                    if (drawPlayer.direction == 1) {
                        itemEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    }
                    else {
                        itemEffect = SpriteEffects.FlipVertically;
                    }
                }

                DrawData item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num9, origin5, adjustedItemScale, itemEffect);
                drawinfo.DrawDataCache.Add(item);
                if (num == 3870) {
                    item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, new Color(255, 255, 255, 127), num9, origin5, adjustedItemScale, drawinfo.itemEffect);
                    drawinfo.DrawDataCache.Add(item);
                }

                return;
            }
        }

        orig(ref drawinfo);
    }
}
