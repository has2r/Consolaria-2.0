using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed class Eggplant_Stem : ModProjectile {
    private const float MAXANGLE = 180f;

    private List<Vector2> _stemPoints = null;

    private ref float UnwrappedRotationValue => ref Projectile.localAI[0];
    private ref float WrappedRotationValue => ref Projectile.localAI[1];

    private int ParentIdentity => (int)Projectile.ai[0];
    private float BaseAngle => Projectile.ai[1];
    private float Tier => Projectile.ai[2];

    private int RotationDirection => -Math.Sign(Projectile.localAI[0]);

    private bool Tier1Stem => Tier == 0f;
    private bool Tier2Stem => Tier == 1f;
    private bool IsEggplant => Tier == 2f;
    private float Length {
        get {
            float result = 45f;
            if (Tier2Stem) {
                result = 70f;
            }
            else if (IsEggplant) {
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
        return null;
    }

    public static int CreateMe(IEntitySource source, Projectile parent, float baseAngle, float myTier) => Projectile.NewProjectile(source, parent.Center, Vector2.Zero, ModContent.ProjectileType<Eggplant_Stem>(), parent.damage, parent.knockBack, parent.owner, Projectile.GetByUUID(parent.owner, parent.whoAmI), baseAngle, myTier);
    public static bool DrawMe(Projectile projectile) {
        Eggplant_Stem me = projectile.ModProjectile as Eggplant_Stem;
        Projectile parent = me.GetParent();
        if (parent != null) {
            float opacity = parent.Opacity;
            Texture2D leafTexture = ModContent.Request<Texture2D>(Eggplant.Path + "_Leaf").Value;
            Rectangle? sourceRectangle = null;
            float opacity2 = Eggplant_Root.OpacityEaseFunction(parent.ai[2] > 1f ? parent.ai[2] - 1f : 0.01f), opacity3 = Eggplant_Root.OpacityEaseFunction(parent.ai[2] > 2f ? parent.ai[2] - 2f : 0.01f);
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            if (me.Tier1Stem) {
                scale *= 0.9f;
            }
            SpriteEffects effects = parent.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 startPosition = parent.Center;
            bool secondFrameChosen = false;
            int meInQueue = 0;
            int scissorHeight = 14;
            int height = (int)(scissorHeight * scale);
            Texture2D stemTexture = ModContent.Request<Texture2D>(Eggplant.Path + "_Stem").Value;
            Vector2 origin = new(leafTexture.Width / 2, height);
            float length = me.Length;
            float strength = length / 100f;
            Vector2 velocity = startPosition.DirectionTo(projectile.Center).RotatedBy(strength * parent.direction);
            float lastRotation = rotation;
            float progress = Eggplant_Root.ScaleEaseFunction(Math.Abs(me.WrappedRotationValue) / MAXANGLE);
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
                leafTexture = ModContent.Request<Texture2D>(Eggplant.Path + "_Shoot").Value;
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
            if (me.Tier2Stem) {
                color *= Math.Min(1f, opacity2);
            }
            else if (me.IsEggplant) {
                color *= Math.Min(1f, opacity3);
            }
            Main.EntitySpriteDraw(leafTexture, startPosition - Main.screenPosition, sourceRectangle, color, lastRotation, origin, scale, effects);

            return true;
        }

        return false;
    }

    public override string Texture => "Consolaria/Assets/Textures/Empty";
    public override bool PreDraw(ref Color lightColor) => false;

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
        Projectile.timeLeft = Eggplant_Root.TIMELEFT;

        Projectile.Opacity = 0f;

        Projectile.penetrate = -1;

        Projectile.tileCollide = false;

        Projectile.appliesImmunityTimeOnSingleHits = true;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 30;
    }

    public override void AI() {
        Projectile parent = GetParent();
        bool checkForParentBeingActive() {
            if (parent == null || !parent.active || parent.ModProjectile is not Eggplant_Root) {
                Projectile.Kill();
                return false;
            }

            return true;
        }
        if (!checkForParentBeingActive()) {
            return;
        }

        void setUpHitboxSizes() {
            if (Tier1Stem) {
                Projectile.Size = 20 * Vector2.One;
            }
            else if (Tier2Stem) {
                Projectile.Size = 30 * Vector2.One;
            }
            else if (IsEggplant) {
                Projectile.Size = 40 * Vector2.One;
            }
        }
        setUpHitboxSizes();

        Eggplant_Root parentProjectileAsRoot = parent.ModProjectile as Eggplant_Root;
        Projectile.rotation = Projectile.AngleTo(parent.Center) - MathHelper.PiOver2;
        float add = parentProjectileAsRoot.RotationAdd;
        float maxAdd = 0.1f;
        add = MathHelper.Clamp(add, -maxAdd, maxAdd);
        Projectile.scale = parent.scale;
        float opacity = parent.Opacity;
        float parentRotationOverTimer = parentProjectileAsRoot.RotationOverTime;
        float opacity1 = parentRotationOverTimer, opacity2 = parentRotationOverTimer > 1f ? parentRotationOverTimer - 1f : 0.01f, opacity3 = parentRotationOverTimer > 2f ? parentRotationOverTimer - 2f : 0.01f;
        if (Tier2Stem) {
            opacity *= opacity2;
        }
        else if (IsEggplant) {
            opacity *= opacity3;
        }
        Projectile.Opacity = Math.Max(0.5f, opacity);
        float baseAngle = BaseAngle;
        UnwrappedRotationValue += parent.direction * Math.Abs(add) * 20f;
        float maxAngle = MAXANGLE;
        WrappedRotationValue = UnwrappedRotationValue * 5f;
        if (WrappedRotationValue > maxAngle || WrappedRotationValue < -maxAngle) {
            WrappedRotationValue = maxAngle * Math.Sign(WrappedRotationValue);
        }
        UnwrappedRotationValue += Length * 0.01f * parent.direction;
        float counter = UnwrappedRotationValue / maxAngle;
        float angle = MathHelper.Pi * 2f + baseAngle;
        float offset = Length * Projectile.Opacity;
        Projectile.Center = parent.Center + (counter * (MathHelper.Pi * 2f) + angle + baseAngle).ToRotationVector2() * offset;
    }

    public override void OnKill(int timeLeft) {
        void makeKillDusts() {
            float speed = 1f;
            if (Tier1Stem) {
                speed *= 0.9f;
            }

            if (_stemPoints != null) {
                Vector2 stemVelocity = Vector2.One.RotatedBy(Projectile.rotation + MathHelper.PiOver2 * RotationDirection) * 3f * speed;
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
            }
        }
        makeKillDusts();

        void createEggplants() {
            if (!IsEggplant) {
                return;
            }

            if (Projectile.owner == Main.myPlayer) {
                Vector2 velocity = Helper.VelocityToPoint(Projectile.Center, Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver2 * RotationDirection), 6f);
                int potentialDamageOfAttachedWeapon = RoACompat.GetDruidicWeaponBasePotentialDamage(RoACompat.GetAttachedNatureWeaponToDruidicProjectile(Projectile), Main.player[Projectile.owner]);
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, velocity, ModContent.ProjectileType<Eggplant_Shoot>(),
                    potentialDamageOfAttachedWeapon, Projectile.knockBack, Projectile.owner);
            }
        }
        createEggplants();
    }
}
