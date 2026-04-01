using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
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

using static Consolaria.Helper;

namespace Consolaria;

public static class Helper {
    public static T TakeRandom<T>(this List<T> list) {
        int index = Main.rand.Next(list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    public static ushort TILESIZE => 16;

    public static Color MultiplyAlpha(this Color color, float alpha) => new(color.R, color.G, color.B, (int)(color.A / 255f * MathHelper.Clamp(alpha, 0f, 1f) * 255f));
    public static Color SetAlpha(this Color color, byte alpha) => new(color.R, color.G, color.B, alpha);

    public static Vector2 Centered(this Rectangle rectangle) => rectangle.Size() / 2f;

    public static Vector2 BottomCenter(this Rectangle rectangle) => new(rectangle.Width / 2f, rectangle.Height);
    public static Vector2 TopCenter(this Rectangle rectangle) => new(rectangle.Width / 2f, 0);
    public static Vector2 LeftCenter(this Rectangle rectangle) => new(rectangle.Width / 2f, rectangle.Height / 2f);

    /// <summary>Contains the data for a <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> call.</summary>
    public struct SpriteBatchSnapshot {
        private static readonly Matrix identityMatrix = Matrix.Identity;
        public SpriteSortMode sortMode;
        public BlendState blendState;
        public SamplerState samplerState;
        public DepthStencilState depthStencilState;
        public RasterizerState rasterizerState;
        public Effect? effect;
        public Matrix transformationMatrix;

        public SpriteBatchSnapshot(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, Effect? effect = null, Matrix? transformationMatrix = null) {
            this.sortMode = sortMode;
            this.blendState = blendState ?? BlendState.AlphaBlend;
            this.samplerState = samplerState ?? SamplerState.LinearClamp;
            this.depthStencilState = depthStencilState ?? DepthStencilState.None;
            this.rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            this.effect = effect;
            this.transformationMatrix = transformationMatrix ?? identityMatrix;
        }

        /// <summary>Calls <seealso cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> with the data on this <seealso cref="SpriteBatchSnapshot"/> instance.</summary>
        /// <param name="spriteBatch">The spritebatch to begin.</param>
        public readonly void Begin(SpriteBatch spriteBatch) {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix);
        }

        public static SpriteBatchSnapshot Capture(SpriteBatch spriteBatch) {
            SpriteSortMode sortMode = (SpriteSortMode)SpriteBatchSnapshotCache.SortModeField.GetValue(spriteBatch);
            BlendState blendState = (BlendState)SpriteBatchSnapshotCache.BlendStateField.GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)SpriteBatchSnapshotCache.SamplerStateField.GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)SpriteBatchSnapshotCache.DepthStencilStateField.GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)SpriteBatchSnapshotCache.RasterizerStateField.GetValue(spriteBatch);
            Effect effect = (Effect)SpriteBatchSnapshotCache.EffectField.GetValue(spriteBatch);
            Matrix transformMatrix = (Matrix)SpriteBatchSnapshotCache.TransformMatrixField.GetValue(spriteBatch);

            return new SpriteBatchSnapshot(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        //void Revalidate()
        //{
        //    blendState ??= BlendState.AlphaBlend;
        //    samplerState ??= SamplerState.LinearClamp;
        //    depthStencilState ??= DepthStencilState.None;
        //    rasterizerState ??= RasterizerState.CullCounterClockwise;
        //}
    }

    public readonly struct DrawInfo() {
        public static DrawInfo Default => new();

        public Vector2 Origin { get; init; } = Vector2.Zero;

        public Vector2 Offset { get; init; } = Vector2.Zero;

        public float Rotation { get; init; } = 0f;
        public Vector2 Scale { get; init; } = Vector2.One;
        public Color Color { get; init; } = Color.White;

        public SpriteEffects ImageFlip { get; init; } = SpriteEffects.None;

        public Rectangle Clip { get; init; } = Rectangle.Empty;

        public DrawInfo WithScale(float scale) => this with { Scale = Scale * scale };
        public DrawInfo WithScaleX(float scale) => this with { Scale = new Vector2(Scale.X * scale, Scale.Y) };
        public DrawInfo WithScaleY(float scale) => this with { Scale = new Vector2(Scale.X, Scale.Y * scale) };

        public DrawInfo WithColor(Color color) => this with { Color = Color.MultiplyRGB(color) };
        public DrawInfo WithColorModifier(float colorModifier) => this with { Color = Color * colorModifier };
        public DrawInfo WithColorRGBModifier(float colorModifier) => this with { Color = Color.ModifyRGB(colorModifier) };

        public DrawInfo WithRotation(float rotation) => this with { Rotation = Rotation + rotation };
    }

    public static void Draw(this SpriteBatch spriteBatch, Texture2D textureToDraw, Vector2 position, DrawInfo drawInfo, bool onScreen = true) {
        spriteBatch.Draw(textureToDraw, position - (onScreen ? Main.screenPosition : Vector2.Zero), drawInfo.Clip, drawInfo.Color, drawInfo.Rotation, drawInfo.Origin, drawInfo.Scale, drawInfo.ImageFlip, 0f);
    }

    public static void DrawWithSnapshot(this SpriteBatch spriteBatch, Texture2D textureToDraw, Vector2 position, DrawInfo drawInfo, bool onScreen = true, SpriteSortMode? sortMode = null, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, Effect? effect = null, Matrix? transformationMatrix = null) {
        SpriteBatchSnapshot snapshot = SpriteBatchSnapshot.Capture(spriteBatch);
        sortMode ??= snapshot.sortMode;
        blendState ??= snapshot.blendState;
        samplerState ??= snapshot.samplerState;
        depthStencilState ??= snapshot.depthStencilState;
        rasterizerState ??= snapshot.rasterizerState;
        effect ??= snapshot.effect;
        transformationMatrix ??= snapshot.transformationMatrix;
        spriteBatch.Begin(new SpriteBatchSnapshot(sortMode.Value, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix), true);
        Draw(spriteBatch, textureToDraw, position, drawInfo, onScreen);
        spriteBatch.Begin(in snapshot, true);
    }

    public static void DrawWithSnapshot(this SpriteBatch spriteBatch, Action draw, SpriteSortMode? sortMode = null, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, Effect? effect = null, Matrix? transformationMatrix = null) {
        SpriteBatchSnapshot snapshot = SpriteBatchSnapshot.Capture(spriteBatch);
        sortMode ??= snapshot.sortMode;
        blendState ??= snapshot.blendState;
        samplerState ??= snapshot.samplerState;
        depthStencilState ??= snapshot.depthStencilState;
        rasterizerState ??= snapshot.rasterizerState;
        effect ??= snapshot.effect;
        transformationMatrix ??= snapshot.transformationMatrix;
        spriteBatch.Begin(new SpriteBatchSnapshot(sortMode.Value, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix), true);
        draw();
        spriteBatch.Begin(in snapshot, true);
    }

    public static void DrawOutlined(this SpriteBatch spriteBatch, Texture2D textureToDraw, Vector2 position, DrawInfo drawInfo, float outlineSize = 2f, bool onScreen = true) {
        for (int i = 0; i < 4; i++) {
            switch (i) {
                case 0:
                    spriteBatch.Draw(textureToDraw, position + -Vector2.UnitX * outlineSize, drawInfo, onScreen);
                    break;
                case 1:
                    spriteBatch.Draw(textureToDraw, position + Vector2.UnitX * outlineSize, drawInfo, onScreen);
                    break;
                case 2:
                    spriteBatch.Draw(textureToDraw, position + -Vector2.UnitY * outlineSize, drawInfo, onScreen);
                    break;
                case 3:
                    spriteBatch.Draw(textureToDraw, position + -Vector2.UnitY * outlineSize, drawInfo, onScreen);
                    break;
            }
        }
    }

    public static Color ModifyRGB(this Color color, float modifier) => (color * modifier) with { A = color.A };

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

public static class SpriteBatchSnapshotCache {
    private const BindingFlags SBBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    internal static FieldInfo _sortModeField, _blendStateField, _samplerStateField, _depthStencilStateField, _rasterizerStateField, _effectField, _transformMatrixField;
    internal static FieldInfo SortModeField => _sortModeField ??= typeof(SpriteBatch).GetField("sortMode", SBBindingFlags);
    internal static FieldInfo BlendStateField => _blendStateField ??= typeof(SpriteBatch).GetField("blendState", SBBindingFlags);
    internal static FieldInfo SamplerStateField => _samplerStateField ??= typeof(SpriteBatch).GetField("samplerState", SBBindingFlags);
    internal static FieldInfo DepthStencilStateField => _depthStencilStateField ??= typeof(SpriteBatch).GetField("depthStencilState", SBBindingFlags);
    internal static FieldInfo RasterizerStateField => _rasterizerStateField ??= typeof(SpriteBatch).GetField("rasterizerState", SBBindingFlags);
    internal static FieldInfo EffectField => _effectField ??= typeof(SpriteBatch).GetField("customEffect", SBBindingFlags);
    internal static FieldInfo TransformMatrixField => _transformMatrixField ??= typeof(SpriteBatch).GetField("transformMatrix", SBBindingFlags);

    public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshot, bool end = false) {
        if (end) {
            spriteBatch.End();
        }
        spriteBatch.Begin(snapshot.sortMode, snapshot.blendState, snapshot.samplerState, snapshot.depthStencilState, snapshot.rasterizerState, snapshot.effect, snapshot.transformationMatrix);
    }

    /// <inheritdoc cref="SpriteBatchSnapshot.Capture(SpriteBatch)"/>
    public static SpriteBatchSnapshot CaptureSnapshot(this SpriteBatch spriteBatch) {
        return SpriteBatchSnapshot.Capture(spriteBatch);
    }

    class Loader : ILoadable {
        void ILoadable.Load(Mod mod) {
        }

        void ILoadable.Unload() {
            _sortModeField = null;
            _blendStateField = null;
            _samplerStateField = null;
            _depthStencilStateField = null;
            _rasterizerStateField = null;
            _effectField = null;
            _transformMatrixField = null;
        }
    }
}

public static class Ease {
    public delegate float Easer(float value);

    public static Easer SineIn => value => -(float)Math.Cos(MathHelper.PiOver2 * value) + 1;
    public static Easer SineOut => value => (float)Math.Sin(MathHelper.PiOver2 * value);
    public static Easer SineInOut => value => -(float)Math.Cos(MathHelper.Pi * value) / 2f + 0.5f;

    public static Easer ExpoIn => value => (float)Math.Pow(2.0, 10.0 * ((double)value - 1.0));
    public static Easer ExpoOut => Invert(ExpoIn);
    public static Easer ExpoInOut => Follow(ExpoIn, ExpoOut);

    public static Easer ExpoInSinOut => Follow(ExpoIn, SineOut);
    public static Easer SineInExpoOut => Follow(SineIn, ExpoOut);

    public static Easer CubeIn => value => value * value * value;
    public static Easer CubeOut => Invert(CubeIn);
    public static Easer CubeInOut => Follow(CubeIn, CubeOut);

    public static Easer CubeInExpoOut => Follow(CubeIn, ExpoOut);

    public static Easer ExpoInCubeOut => Follow(ExpoIn, CubeOut);

    public static Easer QuadIn => value => value * value;
    public static Easer QuadOut => Invert(QuadIn);
    public static Easer QuadInOut => Follow(QuadIn, QuadOut);

    public static Easer QuartIn => value => value * value * value * value;
    public static Easer QuartOut => value => 1f - (float)Math.Pow(1.0 - (double)value, 4);
    public static Easer QuartInOut => Follow(QuartIn, QuartIn);

    public static Easer QuintIn => value => value * value * value * value * value;
    public static Easer QuintOut => value => 1f - (float)Math.Pow(1.0 - (double)value, 5);
    public static Easer QuintInOut => Follow(QuintIn, QuintOut);

    public static Easer TestIn => value => value * value * value * value * value * value * value;

    public static Easer CircOut => value => (float)Math.Sqrt(1 - Math.Pow(value - 1.0, 2));
    public static Easer CircIn => Invert(CircOut);

    private const float B1 = 1f / 2.75f;
    private const float B2 = 2f / 2.75f;
    private const float B3 = 1.5f / 2.75f;
    private const float B4 = 2.5f / 2.75f;
    private const float B5 = 2.25f / 2.75f;
    private const float B6 = 2.625f / 2.75f;

    public static readonly Easer BounceIn = (float value) => {
        value = 1 - value;
        if (value < B1) {
            return 1 - 7.5625f * value * value;
        }
        if (value < B2) {
            return 1 - (7.5625f * (value - B3) * (value - B3) + .75f);
        }
        if (value < B4) {
            return 1 - (7.5625f * (value - B5) * (value - B5) + .9375f);
        }
        return 1 - (7.5625f * (value - B6) * (value - B6) + .984375f);
    };

    public static readonly Easer BounceOut = (float value) => {
        if (value < B1)
            return 7.5625f * value * value;
        if (value < B2)
            return 7.5625f * (value - B3) * (value - B3) + .75f;
        if (value < B4)
            return 7.5625f * (value - B5) * (value - B5) + .9375f;
        return 7.5625f * (value - B6) * (value - B6) + .984375f;
    };

    public static readonly Easer BounceInOut = (float value) => {
        if (value < .5f) {
            value = 1 - value * 2;
            if (value < B1) {
                return (1 - 7.5625f * value * value) / 2;
            }
            if (value < B2) {
                return (1 - (7.5625f * (value - B3) * (value - B3) + .75f)) / 2;
            }
            if (value < B4) {
                return (1 - (7.5625f * (value - B5) * (value - B5) + .9375f)) / 2;
            }
            return (1 - (7.5625f * (value - B6) * (value - B6) + .984375f)) / 2;
        }
        value = value * 2 - 1;
        if (value < B1) {
            return (7.5625f * value * value) / 2 + .5f;
        }
        if (value < B2) {
            return (7.5625f * (value - B3) * (value - B3) + .75f) / 2 + .5f;
        }
        if (value < B4) {
            return (7.5625f * (value - B5) * (value - B5) + .9375f) / 2 + .5f;
        }
        return (7.5625f * (value - B6) * (value - B6) + .984375f) / 2 + .5f;
    };

    public static Easer Invert(Easer easer) => value => 1f - easer(1f - value);

    public static Easer Follow(Easer first, Easer second) => value => value > 0.5f ? second(value * 2f - 1f) / 2f + 0.5f : first(value * 2f) / 2f;
}
