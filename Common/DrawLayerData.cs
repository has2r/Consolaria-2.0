using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;

namespace Consolaria
{
	public class DrawLayerData {
		public static Color DefaultColor() => new Color(255, 255, 255, 0) * 0.8f;
		public Asset<Texture2D> Texture { get; init; }
		public Func<Color> Color { get; init; } = DefaultColor;
	}
}
