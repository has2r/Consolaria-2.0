using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous
{
	public class GoldenCarrot : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.IsFood[Type] = true;
			ItemID.Sets.FoodParticleColors[Type] = new Color[3]
			{
				new Color(224, 183, 31),
				new Color(241, 234, 83),
				new Color(186, 143, 23)
			};
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			Tooltip.SetDefault("Medium improvements to all stats\nReduces last received debuff time by half\n'GOLDEN CARROT?! WHAT?!'");
		}

		public override void SetDefaults() {
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.UseSound = SoundID.Item2;
			Item.useTurn = true;
			Item.buffType = 206;
			Item.buffTime = 7200;
			Item.maxStack = 20;
			Item.width = 18;
			Item.height = 28;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			int buff = ModContent.BuffType<Buffs.GoldenCarrotDebuff>();
			return !player.HasBuff(buff);
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			int buff = ModContent.BuffType<Buffs.GoldenCarrotDebuff>();
			if (player.HasBuff(buff))
			{
				return;
			}
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				int index = -1;
				for (int i = player.buffType.Length - 1; i > 0; i--)
				{
					if (Main.debuff[player.buffType[i]])
					{
						index = player.buffType[i];
						break;
					}
				}
				if (index == -1)
				{
					return;
				}
				int index2 = player.FindBuffIndex(index);
				if (index2 == -1)
				{
					return;
				}
				player.buffTime[index2] /= 2;
				player.AddBuff(buff, player.buffTime[index2] * 2);
				//player.GetModPlayer<GoldenCarrotPlayer>().index = index2;
			}
		}
	}

	public class GoldenCarrotPlayer : ModPlayer
	{
		public int index = -1;
		public override void PostUpdateBuffs()
		{
			if (index == -1)
			{
				return;
			}
			ReduceBuffTime(ref Player.buffTime[index], 2);
			index = -1;
		}

		private int ReduceBuffTime(ref int buffTime, int denominator)
			=> buffTime -= buffTime / 30 * denominator;
	}
}
