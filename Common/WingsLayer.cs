using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Consolaria
{
	public sealed class WingsLayer : PlayerDrawLayer
	{
		private static Dictionary<int, DrawLayerData> WingsLayerData { get; set; }

		public static void RegisterData(int wingSlot, DrawLayerData data) {
			if (!WingsLayerData.ContainsKey(wingSlot))
				WingsLayerData.Add(wingSlot, data);	
		}

		public override void Load()
			=> WingsLayerData = new Dictionary<int, DrawLayerData>();
		
		public override void Unload()
			=> WingsLayerData = null;
		
		public override Position GetDefaultPosition()
			=> new AfterParent(PlayerDrawLayers.Wings);
		
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.wings == -1) {
				return false;
			}
			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			Player drawPlayer = drawInfo.drawPlayer;

			if (!WingsLayerData.TryGetValue(drawPlayer.wings, out DrawLayerData data))
				return;	

			Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);
			Texture2D texture = data.Texture.Value;

			Vector2 directions = drawPlayer.Directions;
			Vector2 offset = new Vector2(0f, 7f);
			Vector2 position = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2) + offset;

			int num11 = 0;
			int num12 = 0;
			int numFrames = 4;

			position += new Vector2(num12 - 9, num11 + 2) * directions;
			position = position.Floor();
			Rectangle frame = new Rectangle(0, texture.Height / numFrames * drawPlayer.wingFrame, texture.Width, texture.Height / numFrames);
			DrawData drawData = new DrawData(texture, position.Floor(), frame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2, texture.Height / numFrames / 2), 1f, drawInfo.playerEffect, 0);
			drawData.shader = drawInfo.cWings;	
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}
