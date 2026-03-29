using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Consolaria;

public static class Helper {
    public static bool IsAliveAndFree(this Player player) => player.IsAlive() && !player.CCed;
    public static bool IsAlive(this Player player) => player.active && !player.dead;

    public static bool IsLocal(this Player player) => player.whoAmI == Main.myPlayer;

    public static void AddBuff<T>(this Player player, ushort time) where T : ModBuff => player.AddBuff(ModContent.BuffType<T>(), time);

    public static float Approach(float val, float target, float maxMove) => (double)val <= (double)target ? Math.Min(val + maxMove, target) : Math.Max(val - maxMove, target);
    public static int Approach(int val, int target, int maxMove) => (double)val <= (double)target ? Math.Min(val + maxMove, target) : Math.Max(val - maxMove, target);

    public static Vector2 Approach(Vector2 val, Vector2 target, float maxMove) => new(Approach(val.X, target.X, maxMove), Approach(val.Y, target.Y, maxMove));
    public static float Wave(float minimum, float maximum, float speed = 1f, float offset = 0f) => Wave((float)Main.GlobalTimeWrappedHourly, minimum, maximum, speed, offset);
    public static float Wave(float step, float minimum, float maximum, float speed = 1f, float offset = 0f) => minimum + ((float)Math.Sin(step * (double)speed + (double)offset) + 1f) / 2f * (maximum - minimum);

    public static void KillDustThatOutOfScreen(this Dust dust) {
        if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
            dust.active = false;
    }

    public static void ApplyDustScale(this Dust dust, bool killDust = true) {
        KillDustThatOutOfScreen(dust);

        float num113 = 0.1f;
        if ((double)Dust.dCount == 0.5)
            dust.scale -= 0.001f;

        if ((double)Dust.dCount == 0.6)
            dust.scale -= 0.0025f;

        if ((double)Dust.dCount == 0.7)
            dust.scale -= 0.005f;

        if ((double)Dust.dCount == 0.8)
            dust.scale -= 0.01f;

        if ((double)Dust.dCount == 0.9)
            dust.scale -= 0.02f;

        if ((double)Dust.dCount == 0.5)
            num113 = 0.11f;

        if ((double)Dust.dCount == 0.6)
            num113 = 0.13f;

        if ((double)Dust.dCount == 0.7)
            num113 = 0.16f;

        if ((double)Dust.dCount == 0.8)
            num113 = 0.22f;

        if ((double)Dust.dCount == 0.9)
            num113 = 0.25f;

        if (killDust && dust.scale < num113)
            dust.active = false;
    }

    public static void BasicDust(this Dust dust, bool applyGravity = true, bool onlyScale = false) {
        ApplyDustScale(dust);

        if (!onlyScale) {
            if (applyGravity && !dust.noGravity) {
                dust.velocity.Y += 0.1f;
            }
        }
        dust.position += dust.velocity;
        dust.rotation += dust.velocity.X * 0.5f;
        if (dust.fadeIn > 0f && dust.fadeIn < 100f) {
            dust.scale += 0.03f;
            if (dust.scale > dust.fadeIn)
                dust.fadeIn = 0f;
        }
        else {
            dust.scale -= 0.01f;
        }

        if (dust.noGravity) {
            if (!onlyScale) {
                dust.velocity *= 0.92f;
            }
            if (dust.fadeIn == 0f)
                dust.scale -= 0.04f;
        }
    }

    public static SpriteEffects ToSpriteEffects(this bool facedRight) => facedRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    public static SpriteEffects ToSpriteEffects(this int direction) => direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    public static SpriteEffects ToSpriteEffects2(this bool facedRight) => facedRight ? SpriteEffects.None : SpriteEffects.FlipVertically;
    public static SpriteEffects ToSpriteEffects2(this int direction) => direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

    public static Texture2D GetTexture(this Projectile projectile) => TextureAssets.Projectile[projectile.type].Value;

    public static void SetTrail(this Projectile projectile, int trailingMode = 2, int length = -1) {
        if (length > 0) {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = length;
        }
        ProjectileID.Sets.TrailingMode[projectile.type] = trailingMode;
    }

    public static string GetItemTexturePath<T>() where T : ModItem => ItemLoader.GetItem(ModContent.ItemType<T>()).Texture;
    public static string GetProjectileTexturePath<T>() where T : ModProjectile => ProjectileLoader.GetProjectile(ModContent.ProjectileType<T>()).Texture;

    public static void SetDefaultsToUsable(this Item item, int vanillaUseStyleID, int useTime, int animationTime, bool showItemOnUse = true, bool useTurn = false, bool autoReuse = false, SoundStyle? useSound = null) {
        item.useStyle = vanillaUseStyleID;
        item.useTime = useTime;
        item.useAnimation = animationTime;
        item.noUseGraphic = !showItemOnUse;
        item.useTurn = useTurn;
        item.autoReuse = autoReuse;
        if (useSound != null) {
            item.UseSound = useSound;
        }
    }

    public static void SetUsableValues(this Item item, int vanillaUseStyleID, int timeToUse, bool showItemOnUse = true, bool useTurn = false, bool autoReuse = false, SoundStyle? useSound = null)
        => item.SetDefaultsToUsable(vanillaUseStyleID, timeToUse, timeToUse, showItemOnUse, useTurn, autoReuse, useSound);

    public static void SetDefaultsToUsable(this Item item, int vanillaUseStyleID, int timeToUse, bool showItemOnUse = true, bool useTurn = false, bool autoReuse = false, SoundStyle? useSound = null) 
        => item.SetUsableValues(vanillaUseStyleID, timeToUse, showItemOnUse, useTurn, autoReuse, useSound);

    public static bool IsAWeapon(this Projectile projectile) => projectile.damage > 0;
    public static bool IsAWeapon(this Item item) => item.damage > 0;

    public static float Clamp01(this float value) => MathHelper.Clamp(value, 0f, 1f);

    public static void InertiaMoveTowards(ref Vector2 velocity, Vector2 position, Vector2 destination, float inertia = 15f, float speed = 5f, float minDistance = 10f, bool max = false) {
        Vector2 direction = destination - position;
        bool flag = max && velocity.Length() >= 1f || !max;
        if (direction.Length() > minDistance) {
            direction.Normalize();
            velocity = (velocity * inertia + direction * speed) / (inertia + 1f);
        }
        else if (flag) {
            velocity *= (float)Math.Pow(0.97, inertia * 2.0 / inertia);
        }
    }

    public static Player GetOwnerAsPlayer(this Projectile projectile) => Main.player[projectile.owner];
    public static bool IsOwnerLocal(this Projectile projectile) => projectile.owner == Main.myPlayer;

    public static ushort SecondsToFrames(float seconds) => (ushort)MathF.Round(seconds * 60f);

    public static void Animate(this Projectile projectile, int frameCounter, int maxFrames = 0) {
        if (++projectile.frameCounter >= frameCounter) {
            projectile.frameCounter = 0;
            if (++projectile.frame >= (maxFrames > 0 ? maxFrames : projectile.GetFrameCount())) {
                projectile.frame = 0;
            }
        }
    }

    public static void SetFrameCount(this Projectile projectile, int frameCount) => Main.projFrames[projectile.type] = frameCount;
    public static int GetFrameCount(this Projectile projectile) => Main.projFrames[projectile.type];

    public static void SetSizeValues(this Projectile projectile, int size) => projectile.width = projectile.height = size;

    public static void SetSizeValues(this Projectile projectile, int width, int height) {
        projectile.width = width;
        projectile.height = height;
    }

    public static void SetShootableValues(this Item item, ushort shootType = 0, float shootSpeed = 1f, bool noMelee = true) {
        item.shoot = shootType == 0 ? (ushort)ProjectileID.WoodenArrowFriendly : shootType;
        item.shootSpeed = shootSpeed;
        item.noMelee = noMelee;
    }

    public static void SetShootableValues<T>(this Item item, float shootSpeed = 1f, bool noMelee = true) where T : ModProjectile {
        item.shoot = ModContent.ProjectileType<T>();
        item.shootSpeed = shootSpeed;
        item.noMelee = noMelee;
    }

    public static void SetWeaponValues(this Item item, int dmg, float knockback, int bonusCritChance = 0, DamageClass? damageClass = null) {
        item.damage = dmg;
        item.knockBack = knockback;
        item.crit = bonusCritChance;
        if (damageClass != null) {
            item.DamageType = damageClass;
        }
    }

    public static void SetSizeValues(this Item item, int width, int height) {
        item.width = width;
        item.height = height;
    }

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "swapMusic")]
    public extern static ref bool Main_swapMusic(Main self);

    public static Vector2 GetPlayerCorePoint(this Player player, bool addGfY = true) {
        Vector2 vector = player.Bottom;
        Vector2 pos = player.MountedCenter;
        Vector2 result = Utils.Floor(vector + (pos - vector) + new Vector2(0f, addGfY ? player.gfxOffY : 0f));
        return result;
    }

    // adapted vanilla
    public static void LimitPointToPlayerReachableArea(this Player player, ref Vector2 pointPoisition, float maxX = 960f, float maxY = 600f) {
        Vector2 center = player.GetPlayerCorePoint();
        Vector2 vector = pointPoisition - center;
        float num = Math.Abs(vector.X);
        float num2 = Math.Abs(vector.Y);
        float num3 = 1f;
        if (num > maxX) {
            float num4 = maxX / num;
            if (num3 > num4)
                num3 = num4;
        }

        if (num2 > maxY) {
            float num5 = maxY / num2;
            if (num3 > num5)
                num3 = num5;
        }

        Vector2 vector2 = vector * num3;
        pointPoisition = center + vector2;
    }

    public static Vector2 GetViableMousePosition(this Player player) {
        Vector2 result = Main.ReverseGravitySupport(Main.MouseScreen) + Main.screenPosition;
        player.LimitPointToPlayerReachableArea(ref result);
        return result;
    }

    public static Vector2 GetViableMousePosition(this Player player, float maxX = 960f, float maxY = 600f) {
        //float num14 = (float)Main.mouseX + Main.screenPosition.X;
        //float num15 = (float)Main.mouseY + Main.screenPosition.Y;
        //if (player.gravDir == -1f)
        //    num15 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
        Vector2 result = Main.ReverseGravitySupport(Main.MouseScreen) + Main.screenPosition;
        player.LimitPointToPlayerReachableArea(ref result, maxX, maxY);
        return result;
    }

    public static Vector2 GetLimitedPosition(Vector2 startPosition, Vector2 endPosition, float maxLength, float minLength = 0f) {
        Vector2 dif = endPosition - startPosition;
        Vector2 result = startPosition + dif.SafeNormalize(Vector2.UnitY) * MathHelper.Clamp(dif.Length(), minLength, maxLength);
        return result;
    }

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
	
	public static string ArmorSetBonusKey => Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
}