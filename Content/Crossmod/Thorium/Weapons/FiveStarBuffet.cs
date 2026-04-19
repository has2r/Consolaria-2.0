using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using RoA.Core.Utility.Vanilla;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items;
using ThoriumMod.NPCs;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class FiveStarBuffet : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetHealerDefaults() {
        Item.SetSizeValues(54, 54);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.Orange3, Item.sellPrice(gold: 1, silver: 25));
        Item.SetShootableValues<FiveStarBuffet_Use>();

        Item.SetDefaultsToUsable(ItemUseStyleID.Shoot, 50, showItemOnUse: false, useSound: SoundID.Item35);

        Item.channel = true;

        Item.mana = 25;
    }

    public override void SetHealerValues(ref bool IsDarkHealer, ref HealType healType, ref int healAmount, ref bool healDisplay, ref bool isAHealerTool) {
        healType = HealType.AllyAndPlayer;
        healAmount = 20;
        healDisplay = true;
        isAHealerTool = true;
    }

    public sealed class FiveStarBuffet_Food : ThoriumProjectile_HealerBase {
        public static Asset<Texture2D>[] FoodTextures { get; private set; } = null!;

        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetStaticDefaults() {
            if (Main.dedServ) {
                return;
            }

            FoodTextures = new Asset<Texture2D>[4];
            string itemTexture = Helper.GetItemTexturePath<FiveStarBuffet>();
            FoodTextures[0] = ModContent.Request<Texture2D>(itemTexture + "_Food1");
            FoodTextures[1] = ModContent.Request<Texture2D>(itemTexture + "_Food2");
            FoodTextures[2] = ModContent.Request<Texture2D>(itemTexture + "_Food3");
            FoodTextures[3] = ModContent.Request<Texture2D>(itemTexture + "_Food4");
        }

        public override void SetHealerDefaults() {
            Projectile.SetSizeValues(10);

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.penetrate = -1;
        }

        public override void AI() {
            if (Projectile.localAI[0] == 0f) {
                Projectile.localAI[0] = 1f;

                //Projectile.ai[0] = Main.rand.Next(4);
                Projectile.netUpdate = true;
            }

            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[2] == 0f) {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<FiveStarBuffet_Use>()] < 1) {
                    Projectile.ai[2] = 1f;

                    if (player.whoAmI == Projectile.owner) {
                        int playerWhoAmIToHeal = player.whoAmI;
                        foreach (Player target in Main.ActivePlayers) {
                            if (target.IsAlive() && ThoriumUtils.CanBeHealed(Projectile, player, target) && !target.InOpposingTeam(player) && target.statLife < player.statLife) {
                                playerWhoAmIToHeal = target.whoAmI;
                            }
                        }
                        if (player.statLife == player.statLifeMax2 && playerWhoAmIToHeal == player.whoAmI) {
                            int dummyType = ModContent.NPCType<HealingDummy>();
                            for (int u = 0; u < Main.maxNPCs; u++) {
                                NPC dummy = Main.npc[u];
                                if (((Entity)dummy).active && dummy.type == dummyType) {
                                    playerWhoAmIToHeal = -(u + 200);
                                }
                            }
                        }
                        Projectile.ai[1] = playerWhoAmIToHeal;

                        Projectile.netUpdate = true;
                    }
                }
            }

            void disappear(Entity? target) {
                Projectile.Opacity -= 0.1f;
                Projectile.scale -= 0.15f;

                if (Projectile.Opacity <= 0f) {
                    Projectile.Kill();
                }
            }

            Entity? target2 = null;
            void foodDusts(Entity? target) {
                if (target is null) {
                    return;
                }

                SoundEngine.PlaySound(SoundID.Item2 with { MaxInstances = 1 }, Projectile.Center);

                Color[] food1 = [new Color(238, 220, 158),
                                     new Color(223, 197, 122),
                                     new Color(211, 162, 82)];

                Color[] food2 = [new Color(255, 235, 132),
                                     new Color(194, 204, 197),
                                     new Color(162, 218, 137)];

                Color[] food3 = [new Color(235, 194, 151),
                                     new Color(200, 147, 115),
                                     new Color(166, 106, 86)];

                Color[] food4 = [new Color(255, 190, 97),
                                     new Color(247, 111, 111),
                                     new Color(255, 115, 0),
                                     new Color(227, 62, 18)];

                Color[] array = food1;
                switch ((int)Projectile.ai[0]) {
                    case 1:
                        array = food2;
                        break;
                    case 2:
                        array = food3;
                        break;
                    case 3:
                        array = food4;
                        break;
                }
                for (int i = 0; i < 10; i++) {
                    if (array != null && array.Length != 0) {
                        Vector2 mouthPosition = target.Top + Vector2.UnitY * target.height * 0.375f;
                        Vector2 vector = mouthPosition + Main.rand.NextVector2Square(-4f, 4f);
                        Vector2 spinningpoint = new Vector2(target.direction, target is Player player ? ((0f - player.gravDir) * 0.8f) : (-1f * 0.8f));
                        Dust.NewDustPerfect(vector, DustID.FoodPiece, 1.3f * spinningpoint.RotatedBy((float)Math.PI / 5f * Main.rand.NextFloatDirection()), 0, array[Main.rand.Next(array.Length)], 0.8f + 0.2f * Main.rand.NextFloat()).fadeIn = 0f;
                    }
                }
            }
            if (Projectile.ai[2] == 0f) {
                Vector2 vector = player.GetPlayerCorePoint() - Vector2.UnitY * 50f;
                AI_GetMyGroupIndexAndFillBlackList(null, out var index, out var totalIndexesInGroup);
                float num2 = (float)Math.PI * 2f / (float)totalIndexesInGroup;
                float num3 = (float)totalIndexesInGroup * 2f;
                Vector2 vector2 = new Vector2(60f, 20f) / 4f * (totalIndexesInGroup - 1);
                Vector2 vector3 = Vector2.UnitY.RotatedBy(num2 * (float)index + (Main.GlobalTimeWrappedHourly * player.direction) % num3 / num3 * ((float)Math.PI * 2f)) * player.direction;
                vector += vector3 * vector2;
                vector = vector.Floor();

                Projectile.Center = Vector2.Lerp(Projectile.Center, vector, 0.15f);
                Projectile.Center += player.velocity * 0.2f;
                Projectile.Center += player.position - player.oldPosition;

                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, -(vector.X - Projectile.Center.X) * 0.025f, 0.15f);
            }
            else {
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, 0f, 0.15f);

                if (Projectile.ai[1] >= 0f) {
                    Player playerToHeal = Main.player[(int)Projectile.ai[1]];
                    target2 = playerToHeal;
                    if (!playerToHeal.IsAlive()) {
                        disappear(playerToHeal);
                        return;
                    }

                    Projectile.Center = Vector2.Lerp(Projectile.Center, playerToHeal.GetPlayerCorePoint(), 0.15f);
                    Projectile.Center += player.position - player.oldPosition;

                    if (Projectile.getRect().Intersects(playerToHeal.getRect())) {
                        if (Projectile.ai[2] != 10f) {
                            foodDusts(target2);

                            Projectile.ThoriumHeal(20, 60f, onHealEffects: true, bonusHealing: true, delegate {
                                SoundEngine.PlaySound(in SoundID.Item85, Projectile.position);
                            }, null, -1, ignoreHealer: false);
                        }
                        Projectile.ai[2] = 10f;
                    }
                }
                if (Projectile.ai[1] < 0f) {
                    NPC healDummy = Main.npc[-(int)Projectile.ai[1] - 200];
                    target2 = healDummy;
                    if (!healDummy.active) {
                        disappear(healDummy);
                        return;
                    }

                    Projectile.Center = Vector2.Lerp(Projectile.Center, healDummy.Center, 0.15f);
                    Projectile.Center += player.position - player.oldPosition;

                    if (Projectile.getRect().Intersects(healDummy.getRect())) {
                        if (Projectile.ai[2] != 10f) {
                            foodDusts(target2);

                            Projectile.ThoriumHeal(20, 60f, onHealEffects: true, bonusHealing: true, delegate {
                                SoundEngine.PlaySound(in SoundID.Item85, Projectile.position);
                            }, null, -1, ignoreHealer: false);
                        }
                        Projectile.ai[2] = 10f;
                    }
                }
            }
            if (Projectile.ai[2] == 10f) {
                disappear(target2);
            }

            Projectile.velocity *= 0f;
        }

        private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup) {
            index = 0;
            totalIndexesInGroup = 0;
            for (int i = 0; i < 1000; i++) {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type) {
                    if (Projectile.whoAmI > i)
                        index++;

                    totalIndexesInGroup++;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = FoodTextures[(int)Projectile.ai[0]].Value;
            Projectile.QuickDraw(lightColor * Projectile.Opacity, texture: texture);

            return false;
        }
    }


    public sealed class FiveStarBuffet_Use : ThoriumProjectile_BardBase {
        private static Asset<Texture2D> _topTexture = null!,
                                        _bottomTexture = null!;

        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private Vector2 _shake;
        private byte _nextFoodIndex;
        private float _spawnProgress = 1f;
        private float _bellSoundCounter;

        public override void SetStaticDefaults() {
            if (Main.dedServ) {
                return;
            }

            string itemTexture = Helper.GetItemTexturePath<FiveStarBuffet>();
            _topTexture = ModContent.Request<Texture2D>(itemTexture + "_Top");
            _bottomTexture = ModContent.Request<Texture2D>(itemTexture + "_Bottom");
        }

        public override void SetBardDefaults() {
            Projectile.SetSizeValues(10);

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.penetrate = -1;

            //Projectile.Opacity = 0f;
        }

        public override bool PreDraw(ref Color lightColor) {
            var texture = _topTexture.Value;

            Player player = Projectile.GetOwnerAsPlayer();

            var pos = Projectile.Center - Main.screenPosition;
            var effects = (Projectile.spriteDirection == -1) ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically : Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            if (player.gravDir < 0) {
                effects = (Projectile.spriteDirection == -1) ? Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
            }

            float rotation = Projectile.rotation;

            float progress = Helper.Clamp01(MathF.Pow(Projectile.ai[2], 1.5f));
            float progress2 = Ease.CubeOut(progress);

            texture = _bottomTexture.Value;
            Main.EntitySpriteDraw(texture, pos, null, Projectile.GetAlpha(lightColor), rotation, texture.Frame().Left(), Projectile.scale, effects);

            texture = FiveStarBuffet_Food.FoodTextures[_nextFoodIndex].Value;
            Main.EntitySpriteDraw(texture, pos + Projectile.velocity * 28f, null, Projectile.GetAlpha(lightColor) * progress2, rotation, texture.Frame().Center(),
                Projectile.scale * progress2, effects);

            texture = _topTexture.Value;
            float y = -20f * MathF.Max(Ease.CubeInOut(_spawnProgress) * 0.5f, progress);
            Main.EntitySpriteDraw(texture, pos + _shake + Vector2.UnitY * y, null, Projectile.GetAlpha(lightColor), rotation, texture.Frame().Left(), Projectile.scale, effects);

            return false;
        }

        public override bool? CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool ShouldUpdatePosition() => false;

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(_nextFoodIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            _nextFoodIndex = reader.ReadByte();
        }

        public override void AI() {
            _spawnProgress = Helper.Approach(_spawnProgress, 0f, 0.1f);

            Projectile.Opacity = Helper.Approach(Projectile.Opacity, 1f, 0.5f);

            Player player4 = Projectile.GetOwnerAsPlayer();
            //if (!player4.CheckMana(player4.GetSelectedItem(), pay: false)) {
            //    Projectile.Kill();
            //}

            float time = 120f;
            int foodType = ModContent.ProjectileType<FiveStarBuffet_Food>();
            int foodCount = player4.ownedProjectileCounts[foodType];
            if (foodCount < 5) {
                if (Projectile.ai[0] == 0f) {
                    if (player4.whoAmI == Projectile.owner) {
                        _nextFoodIndex = (byte)Main.rand.Next(4);
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.ai[0]++;
                if (Projectile.ai[0] > time) {
                    Projectile.ai[0] = 0f;

                    SoundEngine.PlaySound(SoundID.Item53 with { Pitch = 1f, PitchVariance = 0.1f, MaxInstances = 2 }, Projectile.Center);

                    if (Projectile.IsOwnerLocal()) {
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                                                       Projectile.Center + Projectile.velocity * 28f,
                                                       Vector2.Zero,
                                                       foodType,
                                                       0,
                                                       0f,
                                                       player4.whoAmI,
                                                       _nextFoodIndex);
                    }

                    if (Projectile.localAI[1] != 0f) {
                        if (!player4.CheckMana(player4.HeldItem, pay: true)) {
                            Projectile.Kill();
                        }
                    }

                    Projectile.localAI[1] = 1f;
                }
            }

            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], Utils.GetLerpValue(time * 0.75f, time, Projectile.ai[0], true), 0.2f);
            Projectile.localAI[2] = MathHelper.Lerp(Projectile.localAI[2], Utils.GetLerpValue(time * 0.5f, time, Projectile.ai[0], true), 0.2f);
            _shake = Vector2.Lerp(_shake, Main.rand.NextVector2Unit() * 5f * Projectile.localAI[2], 0.25f);
            if (Projectile.localAI[2] < 0.1f && foodCount > 0) {
                float time2 = 10f;
                if (_bellSoundCounter < time2) {
                    _bellSoundCounter++;
                }
                if (_bellSoundCounter == time2 - 1f) {
                    SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 0.075f * foodCount, MaxInstances = 1 }, Projectile.Center);

                    _bellSoundCounter = time2;
                }
            }
            else {
                _bellSoundCounter = 0f;
            }

            if (player4.noItems || !player4.active || player4.dead) {
                Projectile.Kill();
            }

            Player player = Main.player[Projectile.owner];

            int owner = Projectile.owner;
            ref Vector2 velocity = ref Projectile.velocity;
            float scale = Projectile.scale;
            Vector2 vector21 = Main.player[owner].GetPlayerCorePoint();
            if (Main.myPlayer == owner) {
                int dir = -((player.Center.X - player.GetViableMousePosition().X) > 0).ToDirectionInt();
                if (Projectile.ai[1] != dir) {
                    Projectile.netUpdate = true;
                }
                Projectile.ai[1] = dir;

                if (Main.player[owner].channel) {
                    float num = (float)Math.PI / 2f;
                    Vector2 vector = vector21;
                    int num2 = 2;
                    float num3 = 0f;
                    float num8 = 1f * Projectile.scale;
                    Vector2 vector3 = vector;
                    Vector2 value = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector3;
                    if (player.gravDir == -1f)
                        value.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector3.Y;

                    Vector2 vector4 = Vector2.Normalize(value);
                    if (float.IsNaN(vector4.X) || float.IsNaN(vector4.Y))
                        vector4 = -Vector2.UnitY;

                    vector4 = Vector2.Normalize(Vector2.Lerp(vector4, Vector2.Normalize(Projectile.velocity), 0.9f));
                    vector4 *= num8;
                    if (vector4.X != Projectile.velocity.X || vector4.Y != Projectile.velocity.Y)
                        Projectile.netUpdate = true;

                    vector4.Y *= 0.75f;

                    Projectile.velocity = new Vector2(MathF.Abs(vector4.X) * player.direction, vector4.Y);
                }
                else {
                    Projectile.Kill();
                }
            }

            //if (velocity.X > 0f)
            //    Main.player[owner].ChangeDir(1);
            //else if (velocity.X < 0f)
            //    Main.player[owner].ChangeDir(-1);

            Main.player[owner].ChangeDir((int)Projectile.ai[1]);

            Projectile.direction = (Projectile.velocity.X > 0f).ToDirectionInt();
            Projectile.spriteDirection = Projectile.direction;
            Main.player[owner].heldProj = Projectile.whoAmI;
            Main.player[owner].SetDummyItemTime(2);
            Projectile.position.X = vector21.X - (float)(Projectile.width / 2);
            Projectile.position.Y = vector21.Y - (float)(Projectile.height / 2);
            Projectile.rotation = (float)(Math.Atan2(velocity.Y, velocity.X) + 1.5700000524520874) - MathHelper.PiOver2;
            if (Main.player[owner].direction == 1)
                Main.player[owner].itemRotation = (float)Math.Atan2(velocity.Y * (float)Projectile.direction, velocity.X * (float)Projectile.direction);
            else
                Main.player[owner].itemRotation = (float)Math.Atan2(velocity.Y * (float)Projectile.direction, velocity.X * (float)Projectile.direction);

            Projectile.Center = Utils.Floor(Projectile.Center);
        }
    }
}
