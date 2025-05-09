﻿using Consolaria.Content.Items.Armor.Ranged;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Consolaria {
    public class BodyGlowmask : ModPlayer {
        private static Dictionary<int, Func<Color>> BodyColor { get; set; }

        public static void RegisterData(int bodySlot, Func<Color> color) {
            if (!BodyColor.ContainsKey(bodySlot))
                BodyColor.Add(bodySlot, color);
        }

        public override void Load()
            => BodyColor = new Dictionary<int, Func<Color>>();

        public override void Unload()
            => BodyColor = null;

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
            if (!BodyColor.TryGetValue(Player.body, out Func<Color> color))
                return;
            var drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(TitanMail), EquipType.Body) &&
                drawPlayer.GetModPlayer<TitanPlayer>().titanPower2) {
                color = () => { return Color.Transparent; };
            }
            drawInfo.bodyGlowColor = color();
            drawInfo.armGlowColor = color();
        }
    }
}
