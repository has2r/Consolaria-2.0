using System.IO;

using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Consolaria.Content.Items.Armor.Ranged;

namespace Consolaria {
    public partial class Consolaria : Mod {
        internal enum MessageType : byte {
            TitanPower
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType) {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals
                case MessageType.TitanPower:
                    byte playerNumber = reader.ReadByte();
                    TitanPlayer examplePlayer = Main.player[playerNumber].GetModPlayer<TitanPlayer>();
                    examplePlayer.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server) {
                        // Forward the changes to the other clients
                        examplePlayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}