using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Reflection;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Consolaria;

public static class Helper {
    public static void SearchForTargets(Projectile projectile, Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
        distanceFromTarget = 700f;
        targetCenter = projectile.position;
        foundTarget = false;

        if (owner.HasMinionAttackTargetNPC) {
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            float between = Vector2.Distance(npc.Center, projectile.Center);

            if (between < 2000f) {
                distanceFromTarget = between;
                targetCenter = npc.Center;
                foundTarget = true;
            }
        }

        if (!foundTarget) {
            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                    bool closeThroughWall = between < 100f;

                    if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }
        }
    }

    public static bool NextChance(this UnifiedRandom rand, double chance)
        => rand.NextDouble() <= chance;

    public static string GetPriceText(long price, bool ignoreCopperCoins = false) {
        long investPrice = price;

        string result = string.Empty;

        long copperCoins = 0, silverCoins = 0, goldCoins = 0, platinumCoins = 0;

        const int COPPER = 1000000, SILVER = 10000, GOLD = 100, PLATINUM = 1;

        if (investPrice >= COPPER) {
            copperCoins = investPrice / COPPER;
            investPrice -= copperCoins * COPPER;
        }
        if (investPrice >= SILVER) {
            silverCoins = investPrice / SILVER;
            investPrice -= silverCoins * SILVER;
        }
        if (investPrice >= GOLD) {
            goldCoins = investPrice / GOLD;
            investPrice -= goldCoins * GOLD;
        }
        if (investPrice >= PLATINUM) {
            platinumCoins = investPrice;
        }

        if (copperCoins > 0 && !ignoreCopperCoins) {
            string goldCoinText = Lang.inter[15].Value;
            result += " " + copperCoins + " " + goldCoinText;
        }
        if (silverCoins > 0) {
            string silverCoinText = Lang.inter[16].Value;
            result += " " + silverCoins + " " + silverCoinText;
        }
        if (goldCoins > 0) {
            string goldCoinText = Lang.inter[17].Value;
            result += " " + goldCoins + " " + goldCoinText;
        }
        if (platinumCoins > 0) {
            string platinumCoinText = Lang.inter[18].Value;
            result += " " + platinumCoins + " " + platinumCoinText;
        }

        return result.Substring(1);
    }

    public static double GetRandomSpawnTime(double minTime, double maxTime)
        => (maxTime - minTime) * Main.rand.NextDouble() + minTime;

    public static bool CanTownNPCSpawn(bool spawnCondition) {
        bool anyEvents = Main.IsFastForwardingTime() || Main.eclipse || Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
        if (anyEvents) {
            return false;
        }
        return spawnCondition;
    }

    public static bool IsNPCOnScreen(Vector2 center) {
        int width = NPC.sWidth + NPC.safeRangeX * 2;
        int height = NPC.sHeight + NPC.safeRangeY * 2;
        Rectangle npcScreenRect = new((int)center.X - width / 2, (int)center.Y - height / 2, width, height);
        foreach (Player player in Main.player) {
            if (player.active && player.getRect().Intersects(npcScreenRect)) {
                return true;
            }
        }
        return false;
    }

    public static bool CanDrawArmorLayer(PlayerDrawSet drawInfo, int armorSlotID) {
        if (drawInfo.drawPlayer.armor[armorSlotID].type == ItemID.None || drawInfo.drawPlayer.armor[armorSlotID] == null || drawInfo.drawPlayer.armor[armorSlotID] == new Item())
            return true;
        return false;
    }

    public static Color FadeToColor(Color first, Color second, float blendSpeed, int alpha) {
        int r = (int)(second.R * blendSpeed + first.R * (1f - blendSpeed));
        int g = (int)(second.G * blendSpeed + first.G * (1f - blendSpeed));
        int b = (int)(second.B * blendSpeed + first.B * (1f - blendSpeed));
        int a = alpha;
        return new Color(r, g, b, a);
    }

    public static void BasicInWorldGlowmask(this Item item, SpriteBatch spriteBatch, Texture2D glowTexture, Color color, float rotation, float scale) {
        spriteBatch.Draw(
            glowTexture,
            new Vector2(
                item.position.X - Main.screenPosition.X + item.width * 0.5f,
                item.position.Y - Main.screenPosition.Y + item.height - glowTexture.Height * 0.5f
            ),
            new Rectangle(0, 0, glowTexture.Width, glowTexture.Height),
            color,
            rotation,
            glowTexture.Size() * 0.5f,
            scale,
            SpriteEffects.None,
            0f);
    }

    public static Vector2 RandomPositon(Vector2 pos1, Vector2 pos2) {
        Random _rand = new Random();
        return new Vector2(_rand.Next((int)pos1.X, (int)pos2.X) + 1, _rand.Next((int)pos1.Y, (int)pos2.Y) + 1);
    }

    public static Vector2 VelocityFPTP(Vector2 pos1, Vector2 pos2, float speed) {
        Vector2 move = pos2 - pos1;
        return move * (speed / (float)Math.Sqrt(move.X * move.X + move.Y * move.Y));
    }

    public static float GradtoRad(float Grad) => Grad * (float)Math.PI / 180.0f;
    public static float RadtoGrad(float Rad) => Rad * 180.0f / (float)Math.PI;

    public static int GetNearestNPC(Vector2 Point, bool Friendly = false, bool NoBoss = false) {
        float NearestNPCDist = -1;
        int NearestNPC = -1;
        foreach (NPC npc in Main.npc) {
            if (!npc.active) continue;
            if (NoBoss && npc.boss) continue;
            if (!Friendly && (npc.friendly || npc.lifeMax <= 5)) continue;
            if (NearestNPCDist == -1 || npc.Distance(Point) < NearestNPCDist) {
                NearestNPCDist = npc.Distance(Point);
                NearestNPC = npc.whoAmI;
            }
        }
        return NearestNPC;
    }

    public static int GetNearestPlayer(Vector2 Point, bool Alive = false) {
        float NearestPlayerDist = -1;
        int NearestPlayer = -1;
        foreach (Player player in Main.player) {
            if (Alive && (!player.active || player.dead)) continue;
            if (NearestPlayerDist == -1 || player.Distance(Point) < NearestPlayerDist) {
                NearestPlayerDist = player.Distance(Point);
                NearestPlayer = player.whoAmI;
            }
        }
        return NearestPlayer;
    }

    public static Vector2 VelocityToPoint(Vector2 A, Vector2 B, float Speed) {
        Vector2 Move = (B - A);
        return Move * (Speed / (float)Math.Sqrt(Move.X * Move.X + Move.Y * Move.Y));
    }

    public static Vector2 RandomPointInArea(Vector2 A, Vector2 B)
        => new Vector2(Main.rand.Next((int)A.X, (int)B.X) + 1, Main.rand.Next((int)A.Y, (int)B.Y) + 1);

    public static Vector2 RandomPointInArea(Rectangle Area)
        => new Vector2(Main.rand.Next(Area.X, Area.X + Area.Width), Main.rand.Next(Area.Y, Area.Y + Area.Height));

    public static float RotateBetween2Points(Vector2 A, Vector2 B) => (float)Math.Atan2(A.Y - B.Y, A.X - B.X);

    public static Vector2 CenterPoint(Vector2 A, Vector2 B) => new Vector2((A.X + B.X) / 2.0f, (A.Y + B.Y) / 2.0f);

    public static Vector2 PolarPos(Vector2 Point, float Distance, float Angle, int XOffset = 0, int YOffset = 0) {
        Vector2 ReturnedValue = new();
        ReturnedValue.X = (Distance * (float)Math.Sin((double)Helper.RadtoGrad(Angle)) + Point.X) + XOffset;
        ReturnedValue.Y = (Distance * (float)Math.Cos((double)Helper.RadtoGrad(Angle)) + Point.Y) + YOffset;
        return ReturnedValue;
    }

    public static bool Chance(float chance) => Main.rand.NextFloat() <= chance;

    public static Vector2 SmoothFromTo(Vector2 From, Vector2 To, float Smooth = 60f) => From + ((To - From) / Smooth);


    //Here we use reflections to get AddSpecialPoint from TileDrawing.cs
    public static void Load() {
        _addSpecialPointSpecialPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialPositions", BindingFlags.NonPublic | BindingFlags.Instance);
        _addSpecialPointSpecialsCount = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialsCount", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public static void Unload() {
        _addSpecialPointSpecialPositions = null; //This gets the position of the root wind tile
        _addSpecialPointSpecialsCount = null; //This counts how many wind tiles are loaded
    }

    public static FieldInfo _addSpecialPointSpecialPositions;
    public static FieldInfo _addSpecialPointSpecialsCount;

    public static void AddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int x, int y, int type) //Reconstruction of AddSpecialPoint from Terraria/GameContent/Drawing/TileDrawing.cs
    {
        if (_addSpecialPointSpecialPositions.GetValue(tileDrawing) is Point[][] _specialPositions) {
            if (_addSpecialPointSpecialsCount.GetValue(tileDrawing) is int[] _specialsCount) {
                _specialPositions[type][_specialsCount[type]++] = new Point(x, y);
            }
        }
    }
}