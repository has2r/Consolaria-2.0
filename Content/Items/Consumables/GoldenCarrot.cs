using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Consumables
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
			Item.ResearchUnlockCount = 5;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Ambrosia;
		}

		public override void SetDefaults() {
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 10);
			Item.useTime = Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.UseSound = SoundID.Item2;
			Item.useTurn = true;
			Item.buffType = 206;
			Item.buffTime = 3600 * 5;
			Item.maxStack = 9999;
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
				List<int> buffIDS = new List<int>();
				for (int i = 0; i < player.buffType.Length; i++)
				{
					int type = player.buffType[i];
					if (!Main.debuff[type] && !Main.buffNoTimeDisplay[type] && !Main.vanityPet[type] && type != 206 && type != 207 && type != 26)
					{
						buffIDS.Add(player.FindBuffIndex(type));
					}
				}
				int sum = 0;
				foreach (int index in buffIDS)
				{
					if (index != -1)
					{
						int addTime = 900;
						player.buffTime[index] += addTime;
						sum += addTime;
					}
				}
				player.AddBuff(buff, sum);
				/*int index = -1;
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
				player.AddBuff(buff, player.buffTime[index2] * 2);*/
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
