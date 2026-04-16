using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Consolaria.Content.Crossmod.Thorium.Projectiles;

public sealed class SeraphimHeal : ThoriumProjectile_HealerBase {
    private record struct RayInfo(float Rotation, float ScaleFactor = 1f, float Opacity = 1f);

    private RayInfo[] _rayInfos = null!;
    private bool _onTop;

    public ref float HealTime => ref Projectile.localAI[0];
    public ref float AICounter_ForDusts => ref Projectile.localAI[1];
    public ref float InitValue => ref Projectile.localAI[2];

    public bool Init {
        get => InitValue != 0f;
        set => InitValue = value.ToInt();
    }

    public override void SetHealerDefaults() {
        Projectile.SetSizeValues(10);

        Projectile.netImportant = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;

        Projectile.hide = true;

        Projectile.Opacity = 0f;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
        if (_onTop) {
            overPlayers.Add(index);
        }
        else {
            behindProjectiles.Add(index);
        }
    }

    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    public override bool? CanCutTiles() => false;

    public override void AI() {
        Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 1f, 0.125f);

        _onTop = false;

        Player owner = Projectile.GetOwnerAsPlayer();
        if (!owner.IsAlive() || owner.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimEffectOnCooldown) {
            Projectile.Kill();

            foreach (Player player in Main.ActivePlayers) {
                ThoriumPlayer_Consolaria handler = player.GetModPlayer<ThoriumPlayer_Consolaria>();
                if (handler.HealedBySeraphim_HealerWhoAmI == owner.whoAmI) {
                    handler.HealedBySeraphim_HealerWhoAmI = -1;
                }
            }
        }

        if (Init) {
            foreach (RayInfo rayInfo in _rayInfos) {
                DelegateMethods.v3_1 = ThoriumPlayer_Consolaria.SERAPHIMGLOWCOLOR.ToVector3() * 0.25f;
                Vector2 center = Projectile.Center;
                Utils.PlotTileLine(center, center + Vector2.UnitY.RotatedBy(rayInfo.Rotation) * 30f * rayInfo.ScaleFactor, 4, DelegateMethods.CastLightOpen);
            }
        }

        if (!Init) {
            Init = true;

            _rayInfos = new RayInfo[10];
            int count = _rayInfos.Length;
            for (int i = 0; i < count; i++) {
                _rayInfos[i] = new RayInfo(MathHelper.TwoPi * ((float)i / count) + Main.rand.NextFloatDirection() * MathHelper.PiOver4 * 0.5f,
                                           Main.rand.NextFloat(0.75f, 1.5f),
                                           Main.rand.NextFloat(0.75f, 1f));
            }
        }
 
        Projectile.Center = owner.GetPlayerCorePoint();

        float healTime = 15f;
        if (++HealTime > healTime) {
            int radius = 60;
            int heal = 30;
            Projectile.ThoriumHeal(heal, radius, onHealEffects: true, bonusHealing: true, (Player player, Player target, ref int heals, ref int selfHeals) => {
                SoundEngine.PlaySound(in SoundID.Item85, Projectile.position);

                target.GetModPlayer<ThoriumPlayer_Consolaria>().HealedBySeraphim_HealerWhoAmI = owner.whoAmI;
            }, (player) => {
                return player.GetModPlayer<ThoriumPlayer_Consolaria>().HealedBySeraphim_HealerWhoAmI != owner.whoAmI;
            }, -1, ignoreHealer: false);

            HealTime = 0f;
        }

        AICounter_ForDusts++;

        Projectile projectile = Projectile;
        int num4 = 100;
        Vector2 vector2 = new Vector2(projectile.Top.X, projectile.position.Y + (float)num4);
        for (int j = 0; j < 4; j++) {
            if (!Main.rand.NextChance(Projectile.Opacity)) {
                continue;
            }
            if (Main.rand.NextBool()) {
                continue;
            }
            Vector2 vector3 = Main.rand.NextVector2Unit();
            //if (!(Math.Abs(vector3.X) < 0.12f))
            {
                Vector2 targetPosition = projectile.Center + vector3 * new Vector2((projectile.height - num4) / 2) * 0.75f;
                Dust dust = Dust.NewDustDirect(targetPosition, 0, 0, DustID.TintableDustLighted, 0f, 0f, 100, newColor:
                    Color.Lerp(ThoriumPlayer_Consolaria.SERAPHIMGLOWCOLOR, Color.Lerp(Color.White, Color.LightYellow, Main.rand.NextFloat(0.5f, 1f)), 0.5f) with { A = 100 });
                dust.position = targetPosition - Vector2.UnitY * 4f;
                dust.velocity = (vector2 - dust.position).SafeNormalize(Vector2.Zero);
                dust.velocity += dust.position.DirectionTo(projectile.Center).RotatedBy(-owner.direction * 0.5f) * Main.rand.NextFloat(2.5f, 5f) * 0.125f;
                //dust.velocity += dust.position.DirectionTo(projectile.Center).RotatedBy(0f) * Main.rand.NextFloat(2.5f, 5f) * 0.5f;
                dust.scale = 0.7f + 0.7f * Main.rand.NextFloatDirection();
                dust.scale *= Main.rand.NextFloat(1f, 1.5f);
                dust.fadeIn = 1f;
                dust.noGravity = true;
                dust.customData = projectile;
                dust.velocity.X += Main.rand.NextFloat() * 3.5f * (AICounter_ForDusts % 2 == 0).ToDirectionInt();
                dust.velocity += owner.velocity;
            }
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        int index = 0;
        foreach (RayInfo rayInfo in _rayInfos) {
            index++;
            Texture2D texture = Projectile.GetTexture();
            Vector2 position = Projectile.Center;
            Rectangle clip = texture.Bounds;
            Vector2 origin = clip.BottomCenter();
            float wave = Helper.Wave(0.75f, 1.25f, 2.5f, index * 3 + Projectile.identity);
            float wave2 = Helper.Wave(0.75f, 1.25f, 2.5f, 3 + index * 3 + Projectile.identity);
            float rotation = rayInfo.Rotation + wave * 0.25f;
            Vector2 scale = new Vector2(0.75f, 0.25f * rayInfo.ScaleFactor * Projectile.Opacity) * 0.375f * wave2;
            Color color = ThoriumPlayer_Consolaria.SERAPHIMGLOWCOLOR.SetAlpha(255) * 0.875f * Ease.CubeOut(Projectile.Opacity) * rayInfo.Opacity;
            color *= Helper.Wave(0.75f, 0.875f, 5f, Projectile.identity) * 1.25f;
            Helper.DrawInfo drawInfo = new() {
                Clip = clip,
                Origin = origin,
                Rotation = rotation,
                Color = color,
                Scale = scale
            };
            Main.spriteBatch.DrawWithSnapshot(texture, position, drawInfo, blendState: BlendState.Additive);
        }

        return false;
    }
}
