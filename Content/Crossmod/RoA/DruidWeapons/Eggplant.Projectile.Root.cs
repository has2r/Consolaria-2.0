using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed class Eggplant_Root : ModProjectile {
    public override bool IsLoadingEnabled(Mod mod) => RoACompat.IsRoAEnabled;

    public const int TIMELEFT = 480;

    private struct CenterLeafData {
        public int Index;
        public float Scale, MaxScale, ExtraScale, MaxExtraScale;
        public bool ExtraScaleDirection;
        public float Rotation;
        public float PositionOffsetModifier;
    }

    private CenterLeafData[] _rootLeaves;

    public ref float RotationByScale => ref Projectile.ai[0];
    public ref float Direction => ref Projectile.ai[1];
    public ref float RotationOverTime => ref Projectile.ai[2];

    public ref float InitOnSpawnValue => ref Projectile.localAI[0];
    public ref float InitOnStemSpawnValue => ref Projectile.localAI[1];

    public float RotationAdd => (RotationByScale + RotationOverTime) * 0.015f * Projectile.direction;

    public static float ScaleEaseFunctionIn(float value) => -(float)Math.Cos(MathHelper.PiOver2 * value) + 1;
    public static float ScaleEaseFunctionOut(float value) => (float)Math.Sin(MathHelper.PiOver2 * value);
    public static float ScaleEaseFunction(float value) => value > 0.5f ? ScaleEaseFunctionOut(value * 2f - 1f) / 2f + 0.5f : ScaleEaseFunctionIn(value * 2f) / 2f;

    public static float OpacityEaseFunctionIn(float value) => value * value * value * value * value;
    public static float OpacityEaseFunctionOut(float value) => 1f - (float)Math.Pow(1.0 - (double)value, 5);
    public static float OpacityEaseFunction(float value) => value > 0.5f ? OpacityEaseFunctionOut(value * 2f - 1f) / 2f + 0.5f : OpacityEaseFunctionIn(value * 2f) / 2f;

    public override string Texture => "Consolaria/Assets/Textures/Empty";

    public override bool PreDraw(ref Color lightColor) {
        void drawChildStems() {
            foreach (Projectile projectile in Main.ActiveProjectiles) {
                if (projectile.owner != Projectile.owner) {
                    continue;
                }
                if (projectile.type != ModContent.ProjectileType<Eggplant_Stem>()) {
                    continue;
                }
                if ((int)projectile.ai[0] == Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI)) {
                    Eggplant_Stem.DrawMe(projectile);
                }
            }
        }
        drawChildStems();

        void drawRootLeafs() {
            Texture2D leafTexture = ModContent.Request<Texture2D>(Eggplant.Path + "_Leaf").Value;
            Rectangle? sourceRectangle = null;
            int height = leafTexture.Height;
            Vector2 origin = new(leafTexture.Width / 2f, height - 2f);
            foreach (CenterLeafData leafData in _rootLeaves) {
                float rotation = MathHelper.WrapAngle(leafData.Rotation) + Projectile.rotation;
                Vector2 position = Projectile.Center - Main.screenPosition;
                float scale = Projectile.scale * (ScaleEaseFunction(Utils.Remap(leafData.Scale, 0f, leafData.MaxScale, 0f, 1f)) * leafData.MaxScale + leafData.ExtraScale);
                SpriteEffects effects = SpriteEffects.None;
                Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates()) * Projectile.Opacity;
                scale *= 0.9f;
                Main.EntitySpriteDraw(leafTexture, position, sourceRectangle, color, rotation, origin, scale, effects);
            }
        }
        drawRootLeafs();

        return false;
    }

    public override void SetStaticDefaults() => ProjectileID.Sets.NeedsUUID[Type] = true;

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

    private void CreateChildProjectile(float angle, float meInQueue) {
        if (Projectile.owner != Main.myPlayer) {
            return;
        }

        int whoAmI = Eggplant_Stem.CreateMe(Projectile.GetSource_FromAI(), Projectile, angle, meInQueue);
        Projectile projectile = Main.projectile[whoAmI];
        RoACompat.SetAttachedNatureWeaponToDruidicProjectile(projectile, RoACompat.GetAttachedNatureWeaponToDruidicProjectile(Projectile));
    }

    public override void AI() {
        int centerLeafCount = 5;

        void appear() {
            if (Projectile.Opacity < 1f) {
                Projectile.Opacity += 0.1f;
            }
        }
        appear();

        void createNewStemsOverTime() {
            if (RotationOverTime >= 1f && InitOnStemSpawnValue == 0f) {
                InitOnStemSpawnValue = 1f;

                CreateChildProjectile(MathHelper.PiOver4 - MathHelper.PiOver4 / 3f, 1f);
                CreateChildProjectile(MathHelper.PiOver2 + MathHelper.PiOver4 - MathHelper.PiOver4 / 3f, 1f);
            }
            if (RotationOverTime >= 2f && InitOnStemSpawnValue == 1f) {
                InitOnStemSpawnValue = 2f;

                CreateChildProjectile(MathHelper.PiOver4, 2f);
                CreateChildProjectile(MathHelper.PiOver2 + MathHelper.PiOver4, 2f);
            }
        }
        createNewStemsOverTime();

        void init() {
            if (InitOnSpawnValue == 0f) {
                InitOnSpawnValue = 1f;

                for (int num163 = 0; num163 < 10; num163++) {
                    Dust obj13 = Main.dust[Dust.NewDust(Projectile.Center, 2, 2, DustID.Grass, Projectile.velocity.X, Projectile.velocity.Y, 0)];
                    obj13.velocity = (Main.rand.NextFloatDirection() * (float)Math.PI).ToRotationVector2() * 2f + Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
                    obj13.scale = 0.9f;
                    obj13.fadeIn = 1.1f;
                    obj13.position = Projectile.Center - Projectile.velocity * 2.5f;
                    obj13.velocity *= 0.75f;
                    obj13.velocity += Projectile.velocity / 2f;
                }

                CreateChildProjectile(0f, 0f);
                CreateChildProjectile(MathHelper.PiOver2, 0f);

                Direction = Main.player[Projectile.owner].direction;
                Projectile.direction = (int)Direction;

                _rootLeaves = new CenterLeafData[centerLeafCount];
                for (int i = 0; i < centerLeafCount; i++) {
                    ref CenterLeafData leafData = ref _rootLeaves[i];
                    int index = i + 1, previousIndex = Math.Max(0, index - 1), nextIndex = Math.Min(centerLeafCount - 1, index + 1);
                    leafData.Index = Main.rand.Next(2, 5);
                    int attempts = centerLeafCount;
                    while (attempts-- > 0 && ((leafData.Index == _rootLeaves[previousIndex].Index && previousIndex != index) || (leafData.Index == _rootLeaves[nextIndex].Index && nextIndex != index))) {
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
                ref CenterLeafData leafData = ref _rootLeaves[i];
                float index = (leafData.Index + 1) / (float)centerLeafCount * 0.75f;
                if (leafData.Scale >= leafData.MaxScale) {
                    leafData.Scale = leafData.MaxScale;
                    RotationByScale += leafData.Scale;
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
            RotationByScale = MathHelper.Clamp(RotationByScale, 0f, centerLeafCount) / centerLeafCount;
            if (RotationByScale >= 1f && RotationOverTime < 3f) {
                RotationOverTime += 0.02f;
            }
            Projectile.rotation += RotationAdd;
        }
        rotate();
    }
}