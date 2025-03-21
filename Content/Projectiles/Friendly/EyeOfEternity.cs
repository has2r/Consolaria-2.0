using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class EyeOfEternity : ModProjectile {
        private float glowRotation;

        private bool laserAttack = false;
        private bool scytheAttack = false;

        private int laserCount;
        private Vector2 Velocity;

        public override void SetStaticDefaults() {
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(317);
            Projectile.aiStyle = 62;

            int width = 36; int height = 32;
            Projectile.Size = new Vector2(width, height);

            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
        }

        public override void AI() {
            Projectile.spriteDirection = -1;
            Lighting.AddLight(Projectile.Center, 0.3f, 0.1f, 0.3f);
            Player owner = Main.player[Projectile.owner];
            if (!CheckActive(owner)) return;
            Shoot();
            glowRotation += 0.05f;
        }

        private bool CheckActive(Player owner) {
            if (owner.dead || !owner.active) {
                owner.ClearBuff(ModContent.BuffType<Buffs.EyeOfEternity>());
                return false;
            }
            if (owner.HasBuff(ModContent.BuffType<Buffs.EyeOfEternity>())) Projectile.timeLeft = 2;
            return true;
        }

        private void Shoot() {
            Projectile.localAI[0]++;
            float NearestNPCDist = 600;
            int NearestNPC = -1;
            foreach (NPC npc in Main.npc) {
                if (!npc.active) continue;
                if (npc.friendly || npc.lifeMax <= 5 || npc.type == NPCID.TargetDummy) continue;
                if (NearestNPCDist == -1 || npc.Distance(Projectile.Center) < NearestNPCDist && Collision.CanHitLine(Projectile.Center, 2, 2, npc.Center, 2, 2)) {
                    NearestNPCDist = npc.Distance(Projectile.Center);
                    NearestNPC = npc.whoAmI;
                }
            }
            if (NearestNPC == -1) {
                scytheAttack = false;
                laserAttack = false;
                return;
            }
            float shootSpeed = 8f;
            if (!scytheAttack && !laserAttack && Projectile.localAI[0] % 90 == 0) {
                Velocity = Helper.VelocityToPoint(new Vector2(Projectile.Center.X, Projectile.Center.Y + 12), Main.npc[NearestNPC].Center, shootSpeed);
                if (Main.rand.NextBool(4)) scytheAttack = true;
                else laserAttack = true;
            }
            if (scytheAttack) {
                for (int i = 0; i < 5; i++) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -16).RotatedBy(Math.PI * 2f * (4 - i) / 4), Velocity * 0.5f, (ushort)ModContent.ProjectileType<EternalScythe>(), Projectile.damage, 4, Projectile.owner);
                SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                Projectile.velocity -= Velocity * 0.5f;
                scytheAttack = false;
            }
            if (laserAttack) {
                if (Projectile.localAI[0] % 6 == 0) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Velocity.RotatedByRandom(Math.PI * 0.05f), (ushort)ModContent.ProjectileType<EternalLaser>(), (int)(Projectile.damage * 1.3f), 1, Projectile.owner);
                    SoundEngine.PlaySound(SoundID.Item33 with { Volume = 0.2f, Pitch = 0.5f }, Projectile.Center);
                    if (laserCount++ > 3) {
                        laserCount = 0;
                        laserAttack = false;
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.tileCollide = false;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.tileCollide = false;
            return false;
        }

        public override bool? CanCutTiles()
            => false;

        public override bool MinionContactDamage()
            => false;

        public override void PostDraw(Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/EyeOfEternity_Glow");
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, glowRotation, origin, 1f, SpriteEffects.FlipHorizontally, 0f);
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/EyeOfEternity_Pulse");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = SpriteEffects.None;
            Color color2 = new Color(Color.BlueViolet.R, Color.BlueViolet.G, Color.BlueViolet.B, 0);
            Vector2 pulseOffset = new Vector2(3, 0).RotatedBy(Math.PI * 2f * (Math.Sin(Projectile.localAI[0] / 16) / 2 + 0.5));
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin;

            Main.spriteBatch.Draw(texture, drawPos + pulseOffset, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(texture, drawPos - pulseOffset, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            pulseOffset = pulseOffset.RotatedBy(Math.PI * 2f / 4) * 0.5f;
            Main.spriteBatch.Draw(texture, drawPos + pulseOffset, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            Main.spriteBatch.Draw(texture, drawPos - pulseOffset, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);

            return true;
        }
    }
}