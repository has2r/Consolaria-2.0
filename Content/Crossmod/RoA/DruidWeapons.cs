using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA;

sealed class RoACompat : ModSystem {
    private static Mod _riseOfAges;

    internal static Mod RiseOfAges {
        get {
            if (_riseOfAges == null && ModLoader.TryGetMod("RoA", out Mod mod)) {
                _riseOfAges = mod;
            }
            return _riseOfAges;
        }
    }

    public static bool IsRoAEnabled => RiseOfAges != null;

    public override void Unload() => _riseOfAges = null;

    internal static void MakeItemNature(Item item) => RiseOfAges?.Call("MakeItemNature", item);
    internal static void MakeItemDruidicWeapon(Item item) => RiseOfAges?.Call("MakeItemDruidicWeapon", item);
    internal static void SetDruidicWeaponPotentialDamage(Item item, ushort potentialDamage) => RiseOfAges?.Call("SetDruidicWeaponPotentialDamage", item, potentialDamage);
    internal static void SetDruidicWeaponFillingRate(Item item, float fillingRate) => RiseOfAges?.Call("SetDruidicWeaponFillingRate", item, fillingRate);
    internal static void SetDruidicWeaponValues(Item item, ushort potentialDamage, float fillingRate) {
        MakeItemDruidicWeapon(item);
        SetDruidicWeaponPotentialDamage(item, potentialDamage);
        SetDruidicWeaponFillingRate(item, fillingRate);
    }

    internal static ushort GetDruidicWeaponBasePotentialDamage(Item item, Player player) => (ushort)RiseOfAges?.Call("GetDruidicWeaponBasePotentialDamage", item, player);

    internal static void MakeProjectileDruidicDamageable(Projectile projectile) => RiseOfAges?.Call("MakeProjectileDruidicDamageable", projectile);
    internal static void SetDruidicProjectileValues(Projectile projectile, bool shouldChargeWreath = true, bool shouldApplyAttachedItemDamage = true, float wreathFillingFine = 0f) {
        MakeProjectileDruidicDamageable(projectile);
        RiseOfAges?.Call("SetDruidicProjectileValues", projectile, shouldChargeWreath, shouldApplyAttachedItemDamage, wreathFillingFine);
    }
    internal static Item GetAttachedItemToDruidicProjectile(Projectile projectile) => (Item)RiseOfAges?.Call("GetAttachedItemToDruidicProjectile", projectile);
}

sealed class Eggplant : ModItem {
    public override bool IsLoadingEnabled(Mod mod) => RoACompat.IsRoAEnabled;

    public override void SetDefaults() {
        int width = 30, height = 26;
        Item.width = width; Item.height = height;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.noUseGraphic = false;
        Item.useTurn = false;
        Item.autoReuse = false;
        Item.UseSound = SoundID.Item7;

        int baseDamage = 15;
        float knockBack = 3f;
        ushort potentialDamage = 25;
        float fillingRateModifier = 1f;
        Item.damage = baseDamage;
        Item.knockBack = knockBack;
        RoACompat.SetDruidicWeaponValues(Item, potentialDamage, fillingRateModifier);

        Item.shoot = ModContent.ProjectileType<EggplantProjectile>();
        Item.shootSpeed = 1f;
        Item.noMelee = true;

        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Blue;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        position = player.Center;
        Vector2 destination = player.GetViableMousePosition(240f, 150f);
        while (!Collision.SolidCollision(position, 4, 4)) {
            if (Vector2.Distance(position, destination) < 60f) {
                break;
            }
            position += velocity.SafeNormalize(Vector2.Zero);
        }
    }

    private class EggplantProjectile : ModProjectile {
        private const int TIMELEFT = 480;

        private class EggplantProjectile3 : ModProjectile {
            public override string Texture => "Consolaria/Content/Crossmod/RoA/EggplantProjectile";

            public override void SetDefaults() {
                bool shouldChargeWreath = true;
                bool shouldApplyAttachedItemDamage = false;
                float wreathFillingFine = 0f;
                RoACompat.SetDruidicProjectileValues(Projectile, shouldChargeWreath, shouldApplyAttachedItemDamage, wreathFillingFine);

                Projectile.Size = 20 * Vector2.One;

                Projectile.ignoreWater = true;
                Projectile.friendly = true;

                Projectile.aiStyle = -1;
                Projectile.timeLeft = 240;

                Projectile.penetrate = 3;
            }

            public override void AI() {
                if (Main.windPhysics) {
                    Projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
                }
                Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] >= 20f) {
                    Projectile.velocity.Y += 0.3f;
                    Projectile.velocity.X *= 0.98f;
                }
            }

            public override bool OnTileCollide(Vector2 oldVelocity) {
                SoundEngine.PlaySound(SoundID.NPCHit18, Projectile.position);

                return base.OnTileCollide(oldVelocity);
            }

            public override void OnKill(int timeLeft) {
                if (!Main.dedServ) {
                    for (int i = 0; i < 3; i++)
                        Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.oldVelocity * 0.2f, ModContent.Find<ModGore>("Consolaria/EggplantGore").Type, 1f);
                }
                for (int i = 0; i < 10; i++) {
                    int dust = Dust.NewDust(Projectile.Center - Vector2.One * 10, 20, 20, DustID.Water_Desert, Projectile.velocity.X * 0.3f, 0, 0, new Color(250, 200, 100).MultiplyRGB(Color.Purple), 1.5f);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.9f;
                    int dust2 = Dust.NewDust(Projectile.position - Vector2.One * 10, 20, 20, DustID.Water_Desert, 0, 0, 0, new Color(250, 200, 100).MultiplyRGB(Color.Purple), 1.5f);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale *= 0.9f;
                }
            }
        }

        private class EggplantProjectile2 : ModProjectile {
            private List<Vector2> _stemPoints;

            private int ParentIdentity => (int)Projectile.ai[0];

            private bool IsEggplant => Projectile.ai[2] == 2f;
            private float Length {
                get {
                    float result = 45f;
                    if (Projectile.ai[2] == 1f) {
                        result = 70f;
                    }
                    else if (Projectile.ai[2] == 2f) {
                        result = 80f;
                    }
                    return result;
                }
            }

            private Projectile GetParent() {
                int byUUID = Projectile.GetByUUID(Projectile.owner, ParentIdentity);
                if (byUUID == -1) {
                    goto exit;
                }
                if (Main.projectile.IndexInRange(byUUID)) {
                    Projectile parent = Main.projectile[byUUID];
                    return parent;
                }
                goto exit;
                exit:
                Projectile.Kill();
                return null;
            }

            public static int CreateMe(IEntitySource source, Projectile parent, float baseAngle, float length) => Projectile.NewProjectile(source, parent.Center, Vector2.Zero, ModContent.ProjectileType<EggplantProjectile2>(), parent.damage, parent.knockBack, parent.owner, Projectile.GetByUUID(parent.owner, parent.whoAmI), baseAngle, length);
            public static bool DrawMe(Projectile projectile) {
                if (projectile.ModProjectile is not EggplantProjectile2 me) {
                    return false;
                }
                Projectile parent = me.GetParent();
                if (parent == null) {
                    return false;
                }
                float opacity = parent.Opacity;
                Texture2D leafTexture = ModContent.Request<Texture2D>(parent.ModProjectile.Texture + "_Leaf").Value;
                Rectangle? sourceRectangle = null;
                float opacity2 = OpacityEaseFunction(parent.ai[2] > 1f ? parent.ai[2] - 1f : 0.01f), opacity3 = OpacityEaseFunction(parent.ai[2] > 2f ? parent.ai[2] - 2f : 0.01f);
                float rotation = projectile.rotation;
                float scale = projectile.scale;
                switch (projectile.ai[2]) {
                    case 0f:
                        scale *= 0.9f;
                        break;
                    case 1f:
                        scale *= 1f;
                        break;
                    case 2f:
                        scale *= 1f;
                        break;
                }
                SpriteEffects effects = parent.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 startPosition = parent.Center;
                bool secondFrameChosen = false;
                int meInQueue = 0;
                int scissorHeight = 14;
                int height = (int)(scissorHeight * scale);
                Texture2D stemTexture = ModContent.Request<Texture2D>(parent.ModProjectile.Texture + "_Stem").Value;
                Vector2 origin = new(leafTexture.Width / 2, height);
                float length = me.Length;
                float strength = length / 100f;
                Vector2 velocity = startPosition.DirectionTo(projectile.Center).RotatedBy(strength * parent.direction);
                float lastRotation = rotation;
                float progress = ScaleEaseFunction(Math.Abs(projectile.localAI[1]) / 180f);
                List<Vector2> stemPoints = [];
                while (true) {
                    meInQueue++;
                    if (startPosition.Distance(projectile.Center) < height) {
                        break;
                    }
                    Rectangle stemSourceRectangle = new(0, secondFrameChosen ? scissorHeight + 2 : 0, stemTexture.Width, scissorHeight);
                    Color stemColor = Lighting.GetColor(startPosition.ToTileCoordinates()) * opacity;
                    float stemRotation = velocity.ToRotation() + MathHelper.PiOver2;
                    Main.EntitySpriteDraw(stemTexture, startPosition - Main.screenPosition, stemSourceRectangle, stemColor, stemRotation, origin, scale, effects);
                    stemPoints.Add(startPosition);
                    float lerpAmount = MathHelper.Lerp(1f, 0.3f + Math.Max((10 - meInQueue) * 0.01f + 0f, 0), progress);
                    velocity = Vector2.Lerp(velocity, startPosition.DirectionTo(projectile.Center), lerpAmount);
                    startPosition += velocity * height;
                    secondFrameChosen = !secondFrameChosen;
                    lastRotation = stemRotation;
                }
                me._stemPoints = stemPoints;
                lastRotation += progress * MathHelper.PiOver4 * -parent.direction * strength;
                if (me.IsEggplant) {
                    leafTexture = ModContent.Request<Texture2D>(parent.ModProjectile.Texture).Value;
                    lastRotation += MathHelper.Pi;
                    sourceRectangle = new(0, 0, leafTexture.Width, leafTexture.Height);
                    origin = new Vector2(leafTexture.Width / 2, 0);
                    startPosition += new Vector2(0f, -4f * scale).RotatedBy(lastRotation);
                }
                else {
                    startPosition += (Vector2.UnitY * -height / 2f * scale).RotatedBy(lastRotation);
                }
                me._stemPoints.Add(startPosition);
                Color color = Lighting.GetColor(startPosition.ToTileCoordinates()) * opacity;
                switch (projectile.ai[2]) {
                    case 1f:
                        color *= Math.Min(1f, opacity2);
                        break;
                    case 2f:
                        color *= Math.Min(1f, opacity3);
                        break;
                }
                Main.EntitySpriteDraw(leafTexture, startPosition - Main.screenPosition, sourceRectangle, color, lastRotation, origin, scale, effects);
                return true;
            }

            public override string Texture => "Consolaria/Assets/Textures/Empty";

            public override bool PreDraw(ref Color lightColor) => false;

            public override void SetDefaults() {
                bool shouldChargeWreath = true;
                bool shouldApplyAttachedItemDamage = true;
                float wreathFillingFine = 0f;
                RoACompat.SetDruidicProjectileValues(Projectile, shouldChargeWreath, shouldApplyAttachedItemDamage, wreathFillingFine);

                Projectile.Size = 20 * Vector2.One;

                Projectile.ignoreWater = true;
                Projectile.friendly = true;

                Projectile.aiStyle = -1;
                Projectile.timeLeft = TIMELEFT;

                Projectile.Opacity = 0f;

                Projectile.penetrate = -1;

                Projectile.tileCollide = false;

                Projectile.appliesImmunityTimeOnSingleHits = true;
                Projectile.usesIDStaticNPCImmunity = true;
                Projectile.idStaticNPCHitCooldown = 30;
            }

            public override void AI() {
                Projectile parent = GetParent();
                if (parent == null) {
                    Projectile.Kill();
                    return;
                }
                EggplantProjectile parentModProjectile = parent.ModProjectile as EggplantProjectile;
                if (parentModProjectile == null) {
                    Projectile.Kill();
                    return;
                }
                switch (Projectile.ai[2]) {
                    case 0f:
                        Projectile.Size = 20 * Vector2.One;
                        break;
                    case 1f:
                        Projectile.Size = 30 * Vector2.One;
                        break;
                    case 2f:
                        Projectile.Size = 40 * Vector2.One;
                        break;
                }
                Projectile.rotation = Projectile.AngleTo(parent.Center) - MathHelper.PiOver2;
                Projectile.direction = parent.direction;
                float add = (parent.ModProjectile as EggplantProjectile).RotationAdd;
                float maxAdd = 0.1f;
                add = MathHelper.Clamp(add, -maxAdd, maxAdd);
                Projectile.scale = parent.scale;
                float opacity = parent.Opacity;
                float opacity1 = parent.ai[2], opacity2 = parent.ai[2] > 1f ? parent.ai[2] - 1f : 0.01f, opacity3 = parent.ai[2] > 2f ? parent.ai[2] - 2f : 0.01f;
                switch (Projectile.ai[2]) {
                    case 1f:
                        opacity *= opacity2;
                        break;
                    case 2f:
                        opacity *= opacity3;
                        break;
                }
                Projectile.Opacity = Math.Max(0.5f, opacity);
                float baseAngle = Projectile.ai[1];
                Projectile.localAI[0] += parent.direction * Math.Abs(add) * 20f;
                float maxAngle = 180f;
                Projectile.localAI[1] = Projectile.localAI[0] * 5f;
                if (Projectile.localAI[1] > maxAngle || Projectile.localAI[1] < -maxAngle) {
                    Projectile.localAI[1] = maxAngle * Math.Sign(Projectile.localAI[1]);
                }
                Projectile.localAI[0] += Length * 0.01f * parent.direction;
                float counter = Projectile.localAI[0] / maxAngle;
                float angle = MathHelper.Pi * 2f + baseAngle;
                Projectile.localAI[2] = Length;
                float offset = Projectile.localAI[2] * Projectile.Opacity;
                Projectile.Center = parent.Center + (counter * (MathHelper.Pi * 2f) + angle + baseAngle).ToRotationVector2() * offset;
            }

            public override void OnKill(int timeLeft) {
                SoundEngine.PlaySound(SoundID.Grass with { PitchVariance = 0.1f }, Projectile.Center);
                float speed = 1f;
                switch (Projectile.ai[2]) {
                    case 0f:
                        speed *= 0.9f;
                        break;
                    case 1f:
                        speed *= 1f;
                        break;
                    case 2f:
                        speed *= 1f;
                        break;
                }
                int direction = -Math.Sign(Projectile.localAI[0]);
                Vector2 stemVelocity = Vector2.One.RotatedBy(Projectile.rotation + MathHelper.PiOver2 * direction) * 3f * speed;
                foreach (Vector2 collisionPoint in _stemPoints) {
                    for (int i2 = 0; i2 < 5; i2++) {
                        Vector2 vector39 = collisionPoint - Vector2.One * 4;
                        Dust obj2 = Main.dust[Dust.NewDust(vector39, 8, 8, DustID.Grass, stemVelocity.X, stemVelocity.Y, 0, default, 1f + 0.1f * Main.rand.NextFloat())];
                        obj2.velocity *= 0.5f;
                        obj2.noGravity = true;
                        obj2.fadeIn = 0.5f;
                        obj2.noLight = true;
                    }
                }

                if (!IsEggplant) {
                    return;
                }

                SoundEngine.PlaySound(SoundID.Item7 with { PitchVariance = 0.2f }, Projectile.Center);

                if (Projectile.owner == Main.myPlayer) {
                    Vector2 velocity = Helper.VelocityToPoint(Projectile.Center, Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver2 * direction), 6f);
                    Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, velocity, ModContent.ProjectileType<EggplantProjectile3>(), 
                        RoACompat.GetDruidicWeaponBasePotentialDamage(RoACompat.GetAttachedItemToDruidicProjectile(Projectile), Main.player[Projectile.owner]), Projectile.knockBack, Projectile.owner);
                }
            }
        }

        private struct CenterLeafData {
            public int Index;
            public float Scale, MaxScale, ExtraScale, MaxExtraScale;
            public bool ExtraScaleDirection;
            public float Rotation;
            public float PositionOffsetModifier;
        }

        private CenterLeafData[] _centerLeaves;
        private HashSet<Projectile> _childProjectiles;

        public float RotationAdd => (Projectile.ai[0] + Projectile.ai[2]) * 0.015f * Projectile.direction;

        public static float ScaleEaseFunctionIn(float value) => -(float)Math.Cos(MathHelper.PiOver2 * value) + 1;
        public static float ScaleEaseFunctionOut(float value) => (float)Math.Sin(MathHelper.PiOver2 * value);
        public static float ScaleEaseFunction(float value) => value > 0.5f ? ScaleEaseFunctionOut(value * 2f - 1f) / 2f + 0.5f : ScaleEaseFunctionIn(value * 2f) / 2f;

        public static float OpacityEaseFunctionIn(float value) => value * value * value * value * value;
        public static float OpacityEaseFunctionOut(float value) => 1f - (float)Math.Pow(1.0 - (double)value, 5);
        public static float OpacityEaseFunction(float value) => value > 0.5f ? OpacityEaseFunctionOut(value * 2f - 1f) / 2f + 0.5f : OpacityEaseFunctionIn(value * 2f) / 2f;

        public override bool PreDraw(ref Color lightColor) {
            foreach (Projectile projectile in _childProjectiles) {
                EggplantProjectile2.DrawMe(projectile);
            }

            Texture2D leafTexture = ModContent.Request<Texture2D>(Texture + "_Leaf").Value;
            Rectangle? sourceRectangle = null;
            int height = leafTexture.Height;
            Vector2 origin = new(leafTexture.Width / 2f, height - 2f);
            foreach (CenterLeafData leafData in _centerLeaves) {
                float rotation = MathHelper.WrapAngle(leafData.Rotation) + Projectile.rotation;
                Vector2 position = Projectile.Center - Main.screenPosition;
                float scale = Projectile.scale * (ScaleEaseFunction(Utils.Remap(leafData.Scale, 0f, leafData.MaxScale, 0f, 1f)) * leafData.MaxScale + leafData.ExtraScale);
                SpriteEffects effects = SpriteEffects.None;
                Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates()) * Projectile.Opacity;
                scale *= 0.9f;
                Main.EntitySpriteDraw(leafTexture, position, sourceRectangle, color, rotation, origin, scale, effects);
            }

            return false;
        }

        public override void SetDefaults() {
            bool shouldChargeWreath = true;
            bool shouldApplyAttachedItemDamage = true;
            float wreathFillingFine = 0f;
            RoACompat.SetDruidicProjectileValues(Projectile, shouldChargeWreath, shouldApplyAttachedItemDamage, wreathFillingFine);

            Projectile.Size = 20 * Vector2.One;

            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = TIMELEFT;

            Projectile.Opacity = 0f;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.tileCollide = false;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            int centerLeafCount = 5;

            if (Projectile.Opacity < 1f) {
                Projectile.Opacity += 0.1f;
            }

            if (Projectile.owner == Main.myPlayer) {
                if (Projectile.ai[2] >= 1f && Projectile.localAI[1] == 0f) {
                    Projectile.localAI[1] = 1f;
                    int whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, MathHelper.PiOver4 - MathHelper.PiOver4 / 3f, 1f);
                    _childProjectiles.Add(Main.projectile[whoAmI]);
                    whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, MathHelper.PiOver2 + MathHelper.PiOver4 - MathHelper.PiOver4 / 3f, 1f);
                    _childProjectiles.Add(Main.projectile[whoAmI]);
                }
                if (Projectile.ai[2] >= 2f && Projectile.localAI[1] == 1f) {
                    Projectile.localAI[1] = 2f;
                    int whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, MathHelper.PiOver4, 2f);
                    _childProjectiles.Add(Main.projectile[whoAmI]);
                    whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, MathHelper.PiOver2 + MathHelper.PiOver4, 2f);
                    _childProjectiles.Add(Main.projectile[whoAmI]);
                }
            }

            void init() {
                if (Projectile.localAI[0] == 0f) {
                    Projectile.localAI[0] = 1f;

                    _childProjectiles = [];

                    if (Projectile.owner == Main.myPlayer) {
                        for (int num163 = 0; num163 < 10; num163++) {
                            Dust obj13 = Main.dust[Dust.NewDust(Projectile.Center, 2, 2, DustID.Grass, Projectile.velocity.X, Projectile.velocity.Y, 0)];
                            obj13.velocity = (Main.rand.NextFloatDirection() * (float)Math.PI).ToRotationVector2() * 2f + Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
                            obj13.scale = 0.9f;
                            obj13.fadeIn = 1.1f;
                            obj13.position = Projectile.Center - Projectile.velocity * 2.5f;
                            obj13.velocity *= 0.75f;
                            obj13.velocity += Projectile.velocity / 2f;
                        }

                        int whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, 0f, 0f);
                        _childProjectiles.Add(Main.projectile[whoAmI]);
                        whoAmI = EggplantProjectile2.CreateMe(Projectile.GetSource_FromAI(), Projectile, MathHelper.PiOver2, 0f);
                        _childProjectiles.Add(Main.projectile[whoAmI]);

                        Projectile.ai[1] = Main.rand.NextBool().ToDirectionInt();
                        Projectile.netUpdate = true;
                    }

                    Projectile.direction = (int)Projectile.ai[1];

                    _centerLeaves = new CenterLeafData[centerLeafCount];
                    for (int i = 0; i < centerLeafCount; i++) {
                        ref CenterLeafData leafData = ref _centerLeaves[i];
                        int index = i + 1, previousIndex = Math.Max(0, index - 1), nextIndex = Math.Min(centerLeafCount - 1, index + 1);
                        leafData.Index = Main.rand.Next(2, 5);
                        int attempts = centerLeafCount;
                        while (attempts-- > 0 && ((leafData.Index == _centerLeaves[previousIndex].Index && previousIndex != index) || (leafData.Index == _centerLeaves[nextIndex].Index && nextIndex != index))) {
                            leafData.Index = Main.rand.Next(2, 5);
                        }
                        leafData.Rotation = MathHelper.TwoPi / centerLeafCount * index;
                        leafData.MaxScale = Main.rand.NextFloat(1f, 1.5f);
                        leafData.MaxExtraScale = leafData.MaxScale * Main.rand.NextFloat(0.5f, 1.5f) * 0.5f;
                        leafData.PositionOffsetModifier = 0.4f;
                        leafData.ExtraScaleDirection = Main.rand.NextBool();
                    }
                }
            }
            init();

            void scale() {
                for (int i = 0; i < centerLeafCount; i++) {
                    ref CenterLeafData leafData = ref _centerLeaves[i];
                    float index = (leafData.Index + 1) / (float)centerLeafCount * 0.75f;
                    if (leafData.Scale >= leafData.MaxScale) {
                        leafData.Scale = leafData.MaxScale;
                        Projectile.ai[0] += leafData.Scale;
                        float edge = leafData.MaxExtraScale * 0.25f;
                        leafData.ExtraScale += edge * 0.05f * leafData.ExtraScaleDirection.ToDirectionInt();
                        if (leafData.ExtraScale > edge || leafData.ExtraScale < -edge) {
                            leafData.ExtraScaleDirection = !leafData.ExtraScaleDirection;
                        }
                        continue;
                    }
                    leafData.Scale += leafData.MaxScale * 0.1f * index;
                }
            }
            scale();

            void rotate() {
                Projectile.ai[0] = MathHelper.Clamp(Projectile.ai[0], 0f, centerLeafCount) / centerLeafCount;
                if (Projectile.ai[0] >= 1f && Projectile.ai[2] < 3f) {
                    Projectile.ai[2] += 0.02f;
                }
                Projectile.rotation += RotationAdd;
            }
            rotate();
        }
    }

    public override void Load() {
        On_PlayerDrawLayers.DrawPlayer_27_HeldItem += On_PlayerDrawLayers_DrawPlayer_27_HeldItem;
    }

    private void On_PlayerDrawLayers_DrawPlayer_27_HeldItem(On_PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawinfo) {
        if (drawinfo.heldItem.type == ModContent.ItemType<Eggplant>()) {
            if (!drawinfo.drawPlayer.JustDroppedAnItem) {
                if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
                    drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;

                Item heldItem = drawinfo.heldItem;
                int num = heldItem.type;
                if (drawinfo.drawPlayer.UsingBiomeTorches) {
                    switch (num) {
                        case 8:
                            num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
                            break;
                        case 966:
                            num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
                            break;
                    }
                }

                float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
                Main.instance.LoadItem(num);
                Texture2D value = TextureAssets.Item[num].Value;
                Vector2 position = new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y));
                Rectangle itemDrawFrame = drawinfo.drawPlayer.GetItemDrawFrame(num);
                drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
                bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
                bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
                if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
                    flag2 = false;

                if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || !(flag || flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
                    return;

                float num9 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
                float num10 = 0f;
                float num11 = 0f;
                Vector2 origin5 = new Vector2(0f, itemDrawFrame.Height);
                if (num == 3210) {
                    num10 = 8 * -drawinfo.drawPlayer.direction;
                    num11 = 2 * (int)drawinfo.drawPlayer.gravDir;
                }

                num11 = (int)((float)(24 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num9));

                if (num == 3870) {
                    Vector2 vector6 = (drawinfo.drawPlayer.itemRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
                    num10 = (int)vector6.X;
                    num11 = (int)vector6.Y;
                }

                if (num == 3787)
                    num11 = (int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num9));

                if (num == 3209) {
                    Vector2 vector7 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
                    num10 = vector7.X;
                    num11 = vector7.Y;
                }

                if (drawinfo.drawPlayer.gravDir == -1f) {
                    if (drawinfo.drawPlayer.direction == -1) {
                        num9 += 1.57f;
                        origin5 = new Vector2(itemDrawFrame.Width, 0f);
                        num10 -= (float)itemDrawFrame.Width;
                    }
                    else {
                        num9 -= 1.57f;
                        origin5 = Vector2.Zero;
                    }
                }
                else if (drawinfo.drawPlayer.direction == -1) {
                    origin5 = new Vector2(itemDrawFrame.Width, itemDrawFrame.Height);
                    num10 -= (float)itemDrawFrame.Width;
                }


                var drawPlayer = drawinfo.drawPlayer;
                var itemEffect = SpriteEffects.None;
                if (drawPlayer.gravDir == 1f) {
                    if (drawPlayer.direction == 1) {
                        itemEffect = SpriteEffects.FlipHorizontally;
                    }
                    else {
                        itemEffect = SpriteEffects.None;
                    }
                }
                else {
                    if (drawPlayer.direction == 1) {
                        itemEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    }
                    else {
                        itemEffect = SpriteEffects.FlipVertically;
                    }
                }

                DrawData item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num9, origin5, adjustedItemScale, itemEffect);
                drawinfo.DrawDataCache.Add(item);
                if (num == 3870) {
                    item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, new Color(255, 255, 255, 127), num9, origin5, adjustedItemScale, drawinfo.itemEffect);
                    drawinfo.DrawDataCache.Add(item);
                }

                return;
            }
        }
    }
}
