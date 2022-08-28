using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Common.Commands
{
	public class RabbinInvasion : ModCommand
	{
		public override CommandType Type
			=> CommandType.World;

		public override string Command
			=> "rabbitinvasion";

		public override string Usage
			=> "/rabbitinvasion";

		public override string Description
			=> "Start Disaster Bunnies invasion";

		public override void Action(CommandCaller caller, string input, string[] args) {
			NPC.SetEventFlagCleared(eventFlag: ref RabbitInvasion.rabbitInvasion, -1);
			string text = "Bunnies are everywhere!";
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(text, new Color(50, 255, 130));
			}
			else if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), new Color(50, 255, 130));
			}
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
		}
	}
}