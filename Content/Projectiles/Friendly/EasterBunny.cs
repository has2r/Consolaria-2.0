using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class EasterBunny : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Easter Bunny");

            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            int width = 30; int height = 24;
            Projectile.Size = new Vector2(width, height);

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon; 
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }

		public override void AI()
		{
            Player player = Main.player[Projectile.owner];
            if (Math.Abs(Projectile.velocity.X) > 0.1f)
            {
                Projectile.direction = -Math.Sign(Projectile.velocity.X);
                Projectile.spriteDirection = Projectile.direction;
            }
            if (!CheckActive(player))
            {
                return;
            }
            GeneralAI(player);
        }

        private bool CheckActive(Player player)
        {
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.EasterBunny>());
                return false;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.EasterBunny>()))
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Content/Projectiles/Friendly/EasterBunnyEgg");
            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.EntitySpriteDraw(/*Projectile.Opacity > 0.75f && Projectile.Opacity <= 0.99f && Projectile.ai[1] > 100f ? texture2 : */texture, position, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
            return false;
		}

		private void GeneralAI(Player player)
        {
            float jumpVelX = 4f;
            float jumpVelY = 4f;
            float dist = Vector2.Distance(Projectile.Center, player.Center);
            float maxDist = 2000f;
            if (dist > maxDist)
            {
                Projectile.Center = player.Center;
            }
            int tileX = (int)(Projectile.Center.X / 16f), tileY = (int)(Projectile.Center.Y / 16f);
            Tile tile = Main.tile[tileX, tileY];
            bool inTile = tile.HasTile && Main.tileSolid[tile.TileType];
            int lineDist = 40, returnDist = 400;
            int behaviour = ((Projectile.ai[0] == 1 && (player.velocity.Y != 0 || dist > (float)Math.Max(lineDist, (float)returnDist / 10f))) || dist > returnDist || inTile) ? 1 : 0;
            Projectile.tileCollide = true;
            FindEnemies(out float x, out float y, out int targetID);
            Vector2 targetCenter = targetID != -1 ? new Vector2(x, y) : default;
            Vector2 idlePosition = player.Center;
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX;
            bool isOwner = targetCenter == idlePosition;
            if (targetCenter == default)
            {
                targetCenter = idlePosition;
                targetCenter.X += (lineDist + (lineDist * Projectile.minionPos)) * -player.direction;
            }
            float targetDistX = Math.Abs(Projectile.Center.X - targetCenter.X);
            int moveDirection = targetCenter.X > Projectile.Center.X ? 1 : -1;
            float prevAI2 = Projectile.ai[1];
            float jumpStrength = 3f;
            if (targetCenter != default && targetID != -1 && Vector2.Distance(targetCenter, Main.npc[targetID].Center) < 300f)
            {
                Projectile.Opacity = Projectile.ai[1] / 200f;
                if (Projectile.ai[1] > 100f)
                {
                    if (Projectile.ai[1] > 190f && Projectile.ai[1] < 193f)
                    {
                        Projectile.velocity += Projectile.DirectionTo(Main.npc[targetID].Center) * 3f;
                    }
                    else if (Projectile.ai[1] <= 180f)
                    {
                        Projectile.velocity.X *= 0.9f;
                        if (Projectile.velocity.X > -0.1f && Projectile.velocity.X < 0.1f)
                        {
                            Projectile.velocity.X = 0f;
                        }
                    }
                    Projectile.ai[1] -= 1f;
                }
                else
                {
                    Vector2 center = Projectile.Center;
                    Vector2 npcCenter = Main.npc[targetID].Center;
                    float offsetY = 50f;
                    float rotation = (float)Math.Atan2(center.Y - (npcCenter.Y - offsetY), center.X - npcCenter.X);
                    Projectile.velocity.X = 5f * (npcCenter.X > center.X ? 1 : -1);
                    float jumpSpeedY = Math.Min(6f, Math.Abs((float)(Math.Sin(rotation) * 14.0)));
                    Projectile.velocity.Y = -jumpSpeedY;
                    Projectile.ai[1] = 200f;
                }
            }
            else
            {
                Projectile.ai[1] += 0.5f;
                if (isOwner && Projectile.velocity.X < 0.025f && Projectile.velocity.Y == 0f && targetDistX < 15f)
                {
                    Projectile.velocity.X *= Math.Abs(Projectile.velocity.X) > 0.01f ? 0.8f : 0f;
                }
                else if (Projectile.velocity.Y == 0f && ++Projectile.ai[1] >= 30f + Projectile.ai[0])
                {
                    if (Projectile.ai[1] != prevAI2)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.tileCollide = false;
                    Projectile.ai[1] = 0f;
                    Projectile.ai[0] = Main.rand.NextFloat(-5f, 6f);
                    Projectile.velocity.Y = -(jumpVelY + Main.rand.NextFloat(1.5f, 4f) / 2f);
                    Projectile.velocity.X += (jumpVelX + Main.rand.NextFloat(1.25f, 3f)) * moveDirection;
                    Projectile.position += player.velocity;
                }
                if (Projectile.ai[1] > 2f)
                {
                    Projectile.tileCollide = true;
                }
                if (Projectile.ai[1] > 15f - jumpStrength)
                {
                    Projectile.velocity.X *= 0.9f;
                    if (Projectile.velocity.X > -0.1f && Projectile.velocity.X < 0.1f)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
                else if (Projectile.ai[1] < 15f && Math.Abs(Projectile.velocity.X) > 0.05f)
                {
                    Projectile.velocity.X *= 0.95f;
                    if (Projectile.velocity.X > -0.05f && Projectile.velocity.X < 0.05f)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
            }
            if (HitTileOnSide(Projectile, 3))
            {
                if ((Projectile.velocity.X < 0f && moveDirection == -1) || (Projectile.velocity.X > 0f && moveDirection == 1))
                {
                    Vector2 newVec = Projectile.velocity;
                    if (Projectile.tileCollide)
                    {
                        newVec = Collision.TileCollision(Projectile.position, newVec, Projectile.width, Projectile.height);
                        Vector4 slopeVec = Collision.SlopeCollision(Projectile.position, newVec, Projectile.width, Projectile.height);
                        Projectile.position = new Vector2(slopeVec.X, slopeVec.Y);
                        Projectile.velocity = new Vector2(slopeVec.Z, slopeVec.W);
                    }
                    if (Projectile.velocity != newVec)
                    {
                        Projectile.velocity = newVec;
                        Projectile.netUpdate = true;
                    }
                }
            }
            else
            {
                Projectile.velocity.Y += 0.35f;
            }
        }

        private void FindEnemies(out float npcsX, out float npcsY, out int targetID)
		{
            npcsX = Projectile.position.X;
            npcsY = Projectile.position.Y;
            float num135 = 100000f;
            float num136 = num135;
            targetID = -1;
            NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
            {
                float x = ownerMinionAttackTargetNPC2.Center.X;
                float y = ownerMinionAttackTargetNPC2.Center.Y;
                float num138 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - x) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - y);
                if (num138 < num135)
                {
                    if (targetID == -1 && num138 <= num136)
                    {
                        num136 = num138;
                        npcsX = x;
                        npcsY = y;
                    }
                    if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                    {
                        num135 = num138;
                        npcsX = x;
                        npcsY = y;
                        targetID = ownerMinionAttackTargetNPC2.whoAmI;
                    }
                }
            }
            if (targetID == -1)
            {
                for (int num139 = 0; num139 < 200; num139++)
                {
                    if (!Main.npc[num139].CanBeChasedBy(Projectile))
                    {
                        continue;
                    }
                    float num140 = Main.npc[num139].position.X + (float)(Main.npc[num139].width / 2);
                    float num141 = Main.npc[num139].position.Y + (float)(Main.npc[num139].height / 2);
                    float num142 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num140) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num141);
                    if (num142 < num135)
                    {
                        if (targetID == -1 && num142 <= num136)
                        {
                            num136 = num142;
                            npcsX = num140;
                            npcsY = num141;
                        }
                        if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num139].position, Main.npc[num139].width, Main.npc[num139].height))
                        {
                            num135 = num142;
                            npcsX = num140;
                            npcsY = num141;
                            targetID = num139;
                        }
                    }
                }
            }
        }

        private bool HitTileOnSide(Projectile projectile, int dir, bool noYMovement = true)
        {
            if (!noYMovement || projectile.velocity.Y == 0f)
            {
                Vector2 dummyVec = default(Vector2);
                return HitTileOnSide(projectile.position, projectile.width, projectile.height, dir, ref dummyVec);
            }
            return false;
        }

        private bool HitTileOnSide(Vector2 position, int width, int height, int dir, ref Vector2 hitTilePos)
        {
            int tilePosX = 0;
            int tilePosY = 0;
            int tilePosWidth = 0;
            int tilePosHeight = 0;
            if (dir == 0) 
            {
                tilePosX = (int)(position.X - 8f) / 16;
                tilePosY = (int)position.Y / 16;
                tilePosWidth = tilePosX + 1;
                tilePosHeight = (int)(position.Y + (float)height) / 16;
            }
            else if (dir == 1) 
            {
                tilePosX = (int)(position.X + (float)width + 8f) / 16;
                tilePosY = (int)position.Y / 16;
                tilePosWidth = tilePosX + 1;
                tilePosHeight = (int)(position.Y + (float)height) / 16;
            }
            else if (dir == 2)
            {
                tilePosX = (int)position.X / 16;
                tilePosY = (int)(position.Y - 8f) / 16;
                tilePosWidth = (int)(position.X + (float)width) / 16;
                tilePosHeight = tilePosY + 1;
            }
            else if (dir == 3)
            {
                tilePosX = (int)position.X / 16;
                tilePosY = (int)(position.Y + (float)height + 8f) / 16;
                tilePosWidth = (int)(position.X + (float)width) / 16;
                tilePosHeight = tilePosY + 1;
            }
            for (int x2 = tilePosX; x2 < tilePosWidth; x2++)
            {
                for (int y2 = tilePosY; y2 < tilePosHeight; y2++)
                {
                    if (!Main.tile[x2, y2].HasTile)
                    { 
                        return false;
                    }
                    if (Main.tile[x2, y2].IsActuated && Main.tileSolid[(int)Main.tile[x2, y2].TileType])
                    {
                        hitTilePos = new Vector2(x2, y2);
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }
}