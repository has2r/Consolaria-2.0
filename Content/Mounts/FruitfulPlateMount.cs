using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Mounts {
    public class FruitfulPlateMount : ModMount {
        public override void SetStaticDefaults () {
            // Movement
            MountData.jumpHeight = 16; // How high the mount can jump.
            MountData.acceleration = 4f; // The rate at which the mount speeds up.
            MountData.jumpSpeed = 12f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is presssed.
            MountData.blockExtraJumps = true;// Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
            MountData.constantJump = false; // Allows you to hold the jump button down.
            MountData.heightBoost = 0; // Height between the mount and the ground
            MountData.fallDamage = 0; // Fall damage multiplier.
            MountData.runSpeed = 1.75f; // The speed of the mount
            MountData.dashSpeed = 0f; // The speed the mount moves when in the state of dashing.
            MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

            // Misc
            MountData.fatigueMax = 0;
            MountData.buff = ModContent.BuffType<Buffs.FruitfulPlate>(); // The ID number of the buff assigned to the mount.

            // Effects
            MountData.spawnDust = DustID.Lead; // The ID of the dust spawned when mounted or dismounted.

            // Frame data and player offsets
            MountData.totalFrames = 1; // Amount of animation frames for the mount
            MountData.playerYOffsets = Enumerable.Repeat(10, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
            MountData.xOffset = 0;
            MountData.yOffset = 14;
            MountData.playerHeadOffset = 0;
            MountData.bodyFrame = 0;
            // Standing
            MountData.standingFrameCount = 0;
            MountData.standingFrameDelay = 0;
            MountData.standingFrameStart = 0;
            // Running
            MountData.runningFrameCount = 0;
            MountData.runningFrameDelay = 0;
            MountData.runningFrameStart = 0;
            // Flying
            MountData.flyingFrameCount = 0;
            MountData.flyingFrameDelay = 0;
            MountData.flyingFrameStart = 0;
            // In-air
            MountData.inAirFrameCount = 0;
            MountData.inAirFrameDelay = 0;
            MountData.inAirFrameStart = 0;
            // Idle
            MountData.idleFrameCount = 0;
            MountData.idleFrameDelay = 0;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = false;
            // Swim
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;

            if (!Main.dedServ) {
                MountData.textureWidth = MountData.frontTexture.Width();
                MountData.textureHeight = MountData.frontTexture.Height();
            }
        }

        public override void UpdateEffects (Player player) {
            if (player.velocity.Y == 0) {
                if (Math.Abs(player.velocity.X) > 0.5f) {
                    Rectangle rect = player.getRect();
                    Dust.NewDust(new Vector2(rect.X - 20 * player.direction, rect.Y + 20), rect.Width, rect.Height, DustID.Lead, player.velocity.X / 2, 0f, 70, default, 0.8f);
                }
            }
            else player.velocity.X = 0;
        }
    }
}