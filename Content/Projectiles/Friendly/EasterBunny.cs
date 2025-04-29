using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly;

sealed class EasterBunny : ModProjectile {
    public override void SetStaticDefaults() {
        Main.projFrames[Type] = 7;

        Main.projPet[Type] = true;

        ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        ProjectileID.Sets.MinionSacrificable[Type] = true; 
        ProjectileID.Sets.CultistIsResistantTo[Type] = true;
    }

    public override bool PreDraw(ref Color lightColor) {
        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
        SpriteEffects spriteEffects = (SpriteEffects)(Projectile.spriteDirection != 1).ToInt();
        int height = texture.Height / Main.projFrames[Type];
        Rectangle sourceRectangle = new(0, height * Projectile.frame, texture.Width, height);
        Vector2 origin = sourceRectangle.Size() / 2f;
        Vector2 position = Projectile.Center - Main.screenPosition;
        Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates()) * Projectile.Opacity;
        if (Projectile.localAI[1] > 0f) {
            float opacity = MathHelper.Clamp(Projectile.localAI[1] / 30f, 0f, 1f);
            for (int i = 0; i < 5; i++) {
                Main.EntitySpriteDraw(texture, position + new Vector2(0f, 0.5f).RotatedBy((double)i * Math.PI + (double)Main.GlobalTimeWrappedHourly * (double)4f) * 2f * 4f,
                    sourceRectangle, Utils.MultiplyRGB(Color.HotPink, color) * 1.25f * 0.666f * opacity, Projectile.rotation + Utils.NextFloat(Main.rand, -0.05f, 0.05f), origin,
                    Projectile.scale * 0.5f * (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f, spriteEffects, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                Main.EntitySpriteDraw(texture, position + new Vector2(1f, -0.5f).RotatedBy((double)-i * Math.PI + (double)Main.GlobalTimeWrappedHourly * (double)3f) * 1.75f * 4f,
                    sourceRectangle,
                    Utils.MultiplyRGB(Color.HotPink, color) * 0.75f * 0.66f * opacity,
                    Projectile.rotation + Utils.NextFloat(Main.rand, -0.05f, 0.05f), origin,
                    Projectile.scale * 0.4f * (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f, spriteEffects);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, spriteEffects);

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (Projectile.owner == Main.myPlayer && Projectile.velocity.Y >= 0f) {
            Vector2 vector2 = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            vector2.X += (-0.5f + Main.rand.NextFloat()) * 13f;
            vector2.Y = -5f;
            Projectile.velocity.X = vector2.X;
            Projectile.velocity.Y = vector2.Y;
            Projectile.netUpdate = true;
        }
    }

    public override bool MinionContactDamage() => Projectile.friendly;

    public override void SetDefaults() {
        int width = 30, height = 24;
        Projectile.width = width; Projectile.height = height;

        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;

        Projectile.minion = true;
        Projectile.minionSlots = 1f;

        Projectile.timeLeft *= 5;
        Projectile.friendly = true;

        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Summon;

        Projectile.decidesManualFallThrough = true;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 10;

        Projectile.netImportant = true;
    }

    public override bool? CanCutTiles() => false;

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
        bool result = Projectile.ai[1] > 5 && Utils.CenteredRectangle(Projectile.Center, Projectile.Size).Intersects(targetHitbox);
        return result;
    }

    public override void AI() {
        if (!CheckActive()) {
            return;
        }

        AI_FreakingBunny();

        PlayAnimation();
    }
    
    private bool CheckActive() {
        Player player = Main.player[Projectile.owner];
        if (!player.active) {
            Projectile.active = false;
            return false;
        }

        int buffType = ModContent.BuffType<Buffs.EasterBunny>();
        if (player.dead || !player.active) {
            player.ClearBuff(buffType);
        }
        if (player.HasBuff(buffType)) {
            Projectile.timeLeft = 2;
        }

        return true;
    }

    private void PlayAnimation() {
        if (Projectile.velocity.Y == 0.4f) {
            if (Projectile.velocity.X == 0f) {
                //int num47 = 4;
                //if (++Projectile.frameCounter >= 7 * num47 && Main.rand.Next(50) == 0)
                //    Projectile.frameCounter = 0;

                //int num48 = Projectile.frameCounter / num47;
                //if (num48 >= 4)
                //    num48 = 6 - num48;

                //if (num48 < 0)
                //    num48 = 0;

                Projectile.frame = 0;
            }
            else if (Math.Abs(Projectile.velocity.X) >= 0.5f) {
                Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
                Projectile.frameCounter++;
                int num49 = 15;
                int num50 = 7;
                if (Projectile.frameCounter >= num50 * num49)
                    Projectile.frameCounter = 0;

                int num51 = Projectile.frameCounter / num49;
                Projectile.frame = num51;
            }
            else {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }
            ClearLepusVisuals();
        }
        else {
            Projectile.frame = 5;
            ApplyLepusVisuals();
        }
    }

    private void ClearLepusVisuals() {
        if (Projectile.localAI[1] > 0f) {
            Projectile.localAI[1] -= 1f;
        }
    }

    private void ApplyLepusVisuals() {
        if (Projectile.localAI[1] < 30f) {
            Projectile.localAI[1] += 1f;
        }
    }

    private void AI_FreakingBunny() {
        Player player = Main.player[Projectile.owner];
        bool pirate = Projectile.type == 393 || Projectile.type == 394 || Projectile.type == 395;
        bool easterBunny = true;
        bool stormTiger = Projectile.type == 833 || Projectile.type == 834 || Projectile.type == 835;
        bool flag4 = Projectile.type == 834 || Projectile.type == 835;
        bool flinx = Projectile.type == 951;
        int num = 450;
        float num2 = 500f;
        float num3 = 300f;
        int num4 = 10;

        Vector2 vector = player.Center;
        if (flinx) {
            vector.X -= (45 + player.width / 2) * player.direction;
            vector.X -= Projectile.minionPos * 30 * player.direction;
        }
        else if (pirate) {
            vector.X -= (15 + player.width / 2) * player.direction;
            vector.X -= Projectile.minionPos * 20 * player.direction;
        }
        else if (stormTiger) {
            vector.X -= (15 + player.width / 2) * player.direction;
            vector.X -= Projectile.minionPos * 40 * player.direction;
        }
        else if (easterBunny) {
            vector.X -= ((20 + (player.direction != 1 ? 20 : 0)) + player.width / 2) * player.direction;
            vector.X -= Projectile.minionPos * 40 * player.direction;
        }

        bool flag8 = true;

        Projectile.shouldFallThrough = player.position.Y + (float)player.height - 12f > Projectile.position.Y + (float)Projectile.height;
        Projectile.friendly = false;
        int num8 = 0;
        int num9 = 15;
        int attackTarget = -1;
        bool flag9 = false;
        bool flag10 = Projectile.ai[0] == 5f;

        if (easterBunny) {
            Projectile.friendly = true;
            num9 = 20;
            num8 = 30;
        }

        bool flag11 = Projectile.ai[0] == 0f;
        if (stormTiger && flag10)
            flag11 = true;

        if (flag11 && flag8)
            Projectile.Minion_FindTargetInRange(num, ref attackTarget, skipIfCannotHitWithOwnBody: true, (otherEntity, type) => true);

        if (Projectile.ai[0] == 1f) {
            Projectile.tileCollide = false;
            float num17 = 0.2f;
            float num18 = 10f;
            int num19 = 200;
            if (num18 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                num18 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);

            Vector2 vector6 = player.Center - Projectile.Center;
            float num20 = vector6.Length();
            if (num20 > 2000f)
                Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;

            if (num20 < (float)num19 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height 
                && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
                if (Projectile.velocity.Y < -6f)
                    Projectile.velocity.Y = -6f;
            }

            if (!(num20 < 60f)) {
                vector6.Normalize();
                vector6 *= num18;
                if (Projectile.velocity.X < vector6.X) {
                    Projectile.velocity.X += num17;
                    if (Projectile.velocity.X < 0f)
                        Projectile.velocity.X += num17 * 1.5f;
                }

                if (Projectile.velocity.X > vector6.X) {
                    Projectile.velocity.X -= num17;
                    if (Projectile.velocity.X > 0f)
                        Projectile.velocity.X -= num17 * 1.5f;
                }

                if (Projectile.velocity.Y < vector6.Y) {
                    Projectile.velocity.Y += num17;
                    if (Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y += num17 * 1.5f;
                }

                if (Projectile.velocity.Y > vector6.Y) {
                    Projectile.velocity.Y -= num17;
                    if (Projectile.velocity.Y > 0f)
                        Projectile.velocity.Y -= num17 * 1.5f;
                }
            }

            if (Projectile.velocity.X != 0f)
                Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);

            //if (flinx) {
            //    frameCounter++;
            //    if (frameCounter > 3) {
            //        frame++;
            //        frameCounter = 0;
            //    }

            //    if (frame < 2 || frame >= Main.projFrames[type])
            //        frame = 2;

            //    rotation = rotation.AngleTowards(rotation + 0.25f * (float)spriteDirection, 0.25f);
            //}

            //if (pirate) {
            //    frameCounter++;
            //    if (frameCounter > 3) {
            //        frame++;
            //        frameCounter = 0;
            //    }

            //    if ((frame < 10) | (frame > 13))
            //        frame = 10;

            //    rotation = velocity.X * 0.1f;
            //}

            if (easterBunny) {
                //int num21 = 3;
                //if (++Projectile.frameCounter >= num21 * 4)
                //    Projectile.frameCounter = 0;

                //Projectile.frame = 14 + Projectile.frameCounter / num21;
                //Projectile.rotation = Projectile.velocity.X * 0.15f;

                Projectile.frame = 4;
            }

            //if (stormTiger) {
            //    frame = 8;
            //    if (flag4)
            //        frame = 10;

            //    rotation += 0.6f * (float)spriteDirection;
            //}
        }

        if (Projectile.ai[0] == 2f && Projectile.ai[1] < 0f) {
            Projectile.friendly = false;
            Projectile.ai[1] += 1f;
            if (num9 >= 0) {
                Projectile.ai[1] = 0f;
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
                return;
            }
        }
        else if (Projectile.ai[0] == 2f) {
            Projectile.spriteDirection = -Projectile.direction;
            Projectile.rotation = 0f;
            //if (pirate) {
            //    friendly = true;
            //    frame = 4 + (int)((float)num9 - ai[1]) / (num9 / 3);
            //    if (velocity.Y != 0f)
            //        frame += 3;
            //}

            if (easterBunny) {
                ApplyLepusVisuals();
                float num23 = ((float)num9 - Projectile.ai[1]) / (float)num9;
                if ((double)num23 > 0.25 && (double)num23 < 0.75)
                    Projectile.friendly = true;

                int num24 = (int)(num23 * 5f);
                if (num24 > 2)
                    num24 = 4 - num24;

                //if (Projectile.velocity.Y != 0f)
                //    Projectile.frame = 21 + num24;
                //else
                //    Projectile.frame = 18 + num24;

                //Projectile.frame = 0;

                //PlayAnimation();

                //if (Projectile.velocity.Y == 0f)
                //    Projectile.velocity.X *= 0.8f;
            }

            Projectile.velocity.Y += 0.4f;
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;

            Projectile.ai[1] -= 1f;
            if (Projectile.ai[1] <= 0f) {
                if (num8 <= 0) {
                    Projectile.ai[1] = 0f;
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                    return;
                }

                Projectile.ai[1] = -num8;
            }
        }

        if (attackTarget >= 0) {
            float maxDistance2 = num;
            float num25 = 20f;
            if (easterBunny)
                num25 = 50f;

            NPC nPC2 = Main.npc[attackTarget];
            Vector2 center = nPC2.Center;
            vector = center;
            if (Projectile.IsInRangeOfMeOrMyOwner(nPC2, maxDistance2, out var _, out var _, out var _)) {
                Projectile.shouldFallThrough = nPC2.Center.Y > Projectile.Bottom.Y;
                bool flag12 = Projectile.velocity.Y == 0f;
                if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
                    flag12 = true;

                if (center.Y < Projectile.Center.Y - 30f && flag12) {
                    float num26 = (center.Y - Projectile.Center.Y) * -1f;
                    float num27 = 0.4f;
                    float num28 = (float)Math.Sqrt(num26 * 2f * num27);
                    if (num28 > 26f)
                        num28 = 26f;

                    Projectile.velocity.Y = 0f - num28;
                }

                if (Vector2.Distance(Projectile.Center, vector) < num25) {
                    if (Projectile.velocity.Length() > 10f)
                        Projectile.velocity /= Projectile.velocity.Length() / 10f;

                    Projectile.ai[0] = 2f;
                    Projectile.ai[1] = num9;
                    Projectile.netUpdate = true;
                    Projectile.direction = ((center.X - Projectile.Center.X > 0f) ? 1 : (-1));
                }

                //if (flag9 && Vector2.Distance(Projectile.Center, vector) < num25) {
                //    if (Projectile.velocity.Length() > 10f)
                //        Projectile.velocity /= Projectile.velocity.Length() / 10f;

                //    Projectile.ai[0] = 2f;
                //    Projectile.ai[1] = num9;
                //    Projectile.netUpdate = true;
                //    Projectile.direction = ((center.X - Projectile.Center.X > 0f) ? 1 : (-1));
                //}
            }

            if (easterBunny) {
                //int num31 = 1;
                //if (center.X - Projectile.Center.X < 0f)
                //    num31 = -1;

                //vector.X += 20 * -num31;
            }
        }

        if (Projectile.ai[0] == 0f && attackTarget < 0) {
            if (Main.player[Projectile.owner].rocketDelay2 > 0) {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }

            Vector2 vector7 = player.Center - Projectile.Center;
            if (vector7.Length() > 2000f) {
                Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
            }
            else if (vector7.Length() > num2 || Math.Abs(vector7.Y) > num3) {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
                if (Projectile.velocity.Y > 0f && vector7.Y < 0f)
                    Projectile.velocity.Y = 0f;

                if (Projectile.velocity.Y < 0f && vector7.Y > 0f)
                    Projectile.velocity.Y = 0f;
            }
        }

        if (Projectile.ai[0] == 0f) {
            if (attackTarget < 0) {
                if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(vector) > 60f && Math.Sign(vector.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
                    vector = player.Center;

                Rectangle r = Utils.CenteredRectangle(vector, Projectile.Size);
                for (int i = 0; i < 20; i++) {
                    if (Collision.SolidCollision(r.TopLeft(), r.Width, r.Height))
                        break;

                    r.Y += 16;
                    vector.Y += 16f;
                }

                Vector2 vector8 = Collision.TileCollision(player.Center - Projectile.Size / 2f, vector - player.Center, Projectile.width, Projectile.height);
                vector = player.Center - Projectile.Size / 2f + vector8;
                if (Projectile.Distance(vector) < 32f) {
                    float num32 = player.Center.Distance(vector);
                    if (player.Center.Distance(Projectile.Center) < num32)
                        vector = Projectile.Center;
                }

                Vector2 vector9 = player.Center - vector;
                if (vector9.Length() > num2 || Math.Abs(vector9.Y) > num3) {
                    Rectangle r2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
                    Vector2 vector10 = vector - player.Center;
                    Vector2 vector11 = r2.TopLeft();
                    for (float num33 = 0f; num33 < 1f; num33 += 0.05f) {
                        Vector2 vector12 = r2.TopLeft() + vector10 * num33;
                        if (Collision.SolidCollision(r2.TopLeft() + vector10 * num33, r.Width, r.Height))
                            break;

                        vector11 = vector12;
                    }

                    vector = vector11 + Projectile.Size / 2f;
                }
            }

            Projectile.tileCollide = true;
            float num34 = 0.5f;
            float num35 = 4f;
            float num36 = 4f;
            float num37 = 0.1f;
            //if (flinx && attackTarget != -1) {
            //    num34 = 0.65f;
            //    num35 = 5.5f;
            //    num36 = 5.5f;
            //}

            //if (pirate && attackTarget != -1) {
            //    num34 = 1f;
            //    num35 = 8f;
            //    num36 = 8f;
            //}

            if (easterBunny && attackTarget != -1) {
                num34 = 0.7f;
                num35 = 6f;
                num36 = 6f;
            }

            //if (stormTiger && attackTarget != -1) {
            //    num34 = 1f;
            //    num35 = 8f;
            //    num36 = 8f;
            //}

            if (num36 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y)) {
                num36 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                num34 = 0.7f;
            }

            //if (type == 653 || type == 1018) {
            //    float num38 = player.velocity.Length();
            //    if (num38 < 0.1f)
            //        num38 = 0f;

            //    if (num38 != 0f && num38 < num36)
            //        num36 = num38;
            //}

            int num39 = 0;
            bool flag13 = false;
            float num40 = vector.X - Projectile.Center.X;
            Vector2 vector13 = vector - Projectile.Center;
            if (Math.Abs(num40) > 5f) {
                if (num40 < 0f) {
                    num39 = -1;
                    if (Projectile.velocity.X > 0f - num35)
                        Projectile.velocity.X -= num34;
                    else
                        Projectile.velocity.X -= num37;
                }
                else {
                    num39 = 1;
                    if (Projectile.velocity.X < num35)
                        Projectile.velocity.X += num34;
                    else
                        Projectile.velocity.X += num37;
                }

                bool flag14 = true;
                if (pirate)
                    flag14 = false;

                if (easterBunny && attackTarget == -1)
                    flag14 = false;

                if (stormTiger)
                    flag14 = vector13.Y < -80f;

                if (flinx)
                    flag14 = attackTarget > -1 && Main.npc[attackTarget].Hitbox.Intersects(Projectile.Hitbox);

                if (flag14)
                    flag13 = true;
            }
            else {
                Projectile.velocity.X *= 0.9f;
                if (Math.Abs(Projectile.velocity.X) < num34 * 2f)
                    Projectile.velocity.X = 0f;
            }

            bool flag15 = Math.Abs(vector13.X) >= 64f || (vector13.Y <= -48f && Math.Abs(vector13.X) >= 8f);
            if (num39 != 0 && flag15) {
                int num41 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                int num42 = (int)Projectile.position.Y / 16;
                num41 += num39;
                num41 += (int)Projectile.velocity.X;
                for (int j = num42; j < num42 + Projectile.height / 16 + 1; j++) {
                    if (WorldGen.SolidTile(num41, j))
                        flag13 = true;
                }
            }

            Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
            float num43 = Utils.GetLerpValue(0f, 100f, vector13.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
            if (Projectile.velocity.Y == 0f) {
                if (flag13) {
                    for (int k = 0; k < 3; k++) {
                        int num44 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                        if (k == 0)
                            num44 = (int)Projectile.position.X / 16;

                        if (k == 2)
                            num44 = (int)(Projectile.position.X + (float)Projectile.width) / 16;

                        int num45 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
                        if (!WorldGen.SolidTile(num44, num45) && !Main.tile[num44, num45].IsHalfBlock && Main.tile[num44, num45].Slope <= 0 &&
                            (!TileID.Sets.Platforms[Main.tile[num44, num45].TileType] || !Main.tile[num44, num45].HasTile || Main.tile[num44, num45].IsActuated))
                            continue;

                        try {
                            num44 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                            num45 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
                            num44 += num39;
                            num44 += (int)Projectile.velocity.X;
                            if (!WorldGen.SolidTile(num44, num45 - 1) && !WorldGen.SolidTile(num44, num45 - 2))
                                Projectile.velocity.Y = -5.1f;
                            else if (!WorldGen.SolidTile(num44, num45 - 2))
                                Projectile.velocity.Y = -7.1f;
                            else if (WorldGen.SolidTile(num44, num45 - 5))
                                Projectile.velocity.Y = -11.1f;
                            else if (WorldGen.SolidTile(num44, num45 - 4))
                                Projectile.velocity.Y = -10.1f;
                            else
                                Projectile.velocity.Y = -9.1f;
                        }
                        catch {
                            Projectile.velocity.Y = -9.1f;
                        }
                    }

                    if (vector.Y - Projectile.Center.Y < -48f) {
                        float num46 = vector.Y - Projectile.Center.Y;
                        num46 *= -1f;
                        if (num46 < 60f)
                            Projectile.velocity.Y = -6f;
                        else if (num46 < 80f)
                            Projectile.velocity.Y = -7f;
                        else if (num46 < 100f)
                            Projectile.velocity.Y = -8f;
                        else if (num46 < 120f)
                            Projectile.velocity.Y = -9f;
                        else if (num46 < 140f)
                            Projectile.velocity.Y = -10f;
                        else if (num46 < 160f)
                            Projectile.velocity.Y = -11f;
                        else if (num46 < 190f)
                            Projectile.velocity.Y = -12f;
                        else if (num46 < 210f)
                            Projectile.velocity.Y = -13f;
                        else if (num46 < 270f)
                            Projectile.velocity.Y = -14f;
                        else if (num46 < 310f)
                            Projectile.velocity.Y = -15f;
                        else
                            Projectile.velocity.Y = -16f;
                    }

                    if (Projectile.wet && num43 == 0f)
                        Projectile.velocity.Y *= 2f;
                }
            }

            if (Projectile.velocity.X > num36)
                Projectile.velocity.X = num36;

            if (Projectile.velocity.X < 0f - num36)
                Projectile.velocity.X = 0f - num36;

            if (Projectile.velocity.X < 0f)
                Projectile.direction = -1;

            if (Projectile.velocity.X > 0f)
                Projectile.direction = 1;

            if (Projectile.velocity.X == 0f)
                Projectile.direction = ((player.Center.X > Projectile.Center.X) ? 1 : (-1));

            if (Projectile.velocity.X > num34 && num39 == 1)
                Projectile.direction = 1;

            if (Projectile.velocity.X < 0f - num34 && num39 == -1)
                Projectile.direction = -1;

            Projectile.spriteDirection = -Projectile.direction;
            //if (flinx) {
            //    if (velocity.Y == 0f) {
            //        rotation = rotation.AngleTowards(0f, 0.3f);
            //        if (velocity.X == 0f) {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //        else if (Math.Abs(velocity.X) >= 0.5f) {
            //            frameCounter += (int)Math.Abs(velocity.X);
            //            frameCounter++;
            //            if (frameCounter > 10) {
            //                frame++;
            //                frameCounter = 0;
            //            }

            //            if (frame < 2 || frame >= Main.projFrames[type])
            //                frame = 2;
            //        }
            //        else {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //    }
            //    else if (velocity.Y != 0f) {
            //        rotation = Math.Min(4f, velocity.Y) * -0.1f;
            //        if (spriteDirection == -1)
            //            rotation -= (float)Math.PI * 2f;

            //        frameCounter = 0;
            //        frame = 1;
            //    }
            //}

            //if (pirate) {
            //    rotation = 0f;
            //    if (velocity.Y == 0f) {
            //        if (velocity.X == 0f) {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //        else if (Math.Abs(velocity.X) >= 0.5f) {
            //            frameCounter += (int)Math.Abs(velocity.X);
            //            frameCounter++;
            //            if (frameCounter > 10) {
            //                frame++;
            //                frameCounter = 0;
            //            }

            //            if (frame >= 4)
            //                frame = 0;
            //        }
            //        else {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //    }
            //    else if (velocity.Y != 0f) {
            //        frameCounter = 0;
            //        frame = 14;
            //    }
            //}

            if (easterBunny) {
                Projectile.rotation = 0f;
                //PlayAnimation();
                //else if (velocity.Y != 0f) {
                //    if (velocity.Y < 0f) {
                //        if (frame > 9 || frame < 5) {
                //            frame = 5;
                //            frameCounter = 0;
                //        }

                //        if (++frameCounter >= 1 && frame < 9) {
                //            frame++;
                //            frameCounter = 0;
                //        }
                //    }
                //    else {
                //        if (frame > 13 || frame < 9) {
                //            frame = 9;
                //            frameCounter = 0;
                //        }

                //        if (++frameCounter >= 2 && frame < 11) {
                //            frame++;
                //            frameCounter = 0;
                //        }
                //    }
                //}
            }

            //if (stormTiger) {
            //    int num52 = 8;
            //    if (flag4)
            //        num52 = 10;

            //    rotation = 0f;
            //    if (velocity.Y == 0f) {
            //        if (velocity.X == 0f) {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //        else if (Math.Abs(velocity.X) >= 0.5f) {
            //            frameCounter += (int)Math.Abs(velocity.X);
            //            frameCounter++;
            //            if (frameCounter > 10) {
            //                frame++;
            //                frameCounter = 0;
            //            }

            //            if (frame >= num52 || frame < 2)
            //                frame = 2;
            //        }
            //        else {
            //            frame = 0;
            //            frameCounter = 0;
            //        }
            //    }
            //    else if (velocity.Y != 0f) {
            //        frameCounter = 0;
            //        frame = 1;
            //        if (flag4)
            //            frame = 9;
            //    }
            //}

            Projectile.velocity.Y += 0.4f + num43 * 1f;
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;
        }

        //if (!pirate)
        //    return;

        //localAI[0] += 1f;
        //if (velocity.X == 0f)
        //    localAI[0] += 1f;

        //if (localAI[0] >= (float)Main.rand.Next(900, 1200)) {
        //    localAI[0] = 0f;
        //    for (int m = 0; m < 6; m++) {
        //        int num53 = Dust.NewDust(base.Center + Vector2.UnitX * -direction * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, -direction, 1f);
        //        Main.dust[num53].velocity /= 2f;
        //        Main.dust[num53].scale = 0.8f;
        //    }

        //    int num54 = Gore.NewGore(base.Center + Vector2.UnitX * -direction * 8f, Vector2.Zero, Main.rand.Next(580, 583));
        //    Main.gore[num54].velocity /= 2f;
        //    Main.gore[num54].velocity.Y = Math.Abs(Main.gore[num54].velocity.Y);
        //    Main.gore[num54].velocity.X = (0f - Math.Abs(Main.gore[num54].velocity.X)) * (float)direction;
        //}
    }
}