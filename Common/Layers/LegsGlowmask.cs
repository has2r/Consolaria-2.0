using Consolaria.Content.Items.Armor.Ranged;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Consolaria {
    public sealed class LegsGlowmask : PlayerDrawLayer {
        private static Dictionary<int, DrawLayerData> LegsLayerData { get; set; }
        public static void RegisterData(int legSlot, DrawLayerData data) {
            if (!LegsLayerData.ContainsKey(legSlot))
                LegsLayerData.Add(legSlot, data);
        }

        public override void Load()
            => LegsLayerData = new Dictionary<int, DrawLayerData>();

        public override void Unload()
            => LegsLayerData = null;

        public override Position GetDefaultPosition()
            => new AfterParent(PlayerDrawLayers.Leggings);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.dead || drawPlayer.invis || drawPlayer.legs == -1)
                return false;
            return true;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo) {
            Player drawPlayer = drawInfo.drawPlayer;
            if (!LegsLayerData.TryGetValue(drawPlayer.legs, out DrawLayerData data))
                return;
            Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);
            if (drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(TitanLeggings), EquipType.Legs) &&
                drawPlayer.GetModPlayer<TitanPlayer>().titanPower2) {
                color *= 0f;
            }
            Texture2D texture = data.Texture.Value;
            Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
            Vector2 legsOffset = drawInfo.legsOffset;
            DrawData drawData = new DrawData(texture, drawPos.Floor() + legsOffset, drawPlayer.legFrame, color, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0);
            drawData.shader = drawInfo.cLegs;
            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}
