using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class EyeOfEternity : ModProjectile
    {
        private float shootSpeed = 8f;
        private float glowRotation;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Eye of Eternity");
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
            Player owner = Main.player[Projectile.owner];
            if (!CheckActive(owner)) return;    
            Shoot();
            glowRotation += 0.04f;
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
                float NearestNPCDist = 400;
                int NearestNPC = -1;
                foreach (NPC npc in Main.npc) {
                    if (!npc.active) continue;
                    if (npc.friendly || npc.lifeMax <= 5 || npc.type == NPCID.TargetDummy) continue;
                    if (NearestNPCDist == -1 || npc.Distance(Projectile.Center) < NearestNPCDist && Collision.CanHitLine(Projectile.Center, 16, 16, npc.Center, 16, 16)) {
                        NearestNPCDist = npc.Distance(Projectile.Center);
                        NearestNPC = npc.whoAmI;
                    }
                }
                if (NearestNPC == -1) return;
            if (Main.rand.Next(2) == 0) {
                Vector2 Velocity = Helper.VelocityToPoint(new Vector2(Projectile.Center.X, Projectile.Center.Y + 12), Main.npc[NearestNPC].Center, shootSpeed);
                if (Projectile.localAI[0] % 35 == 0) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Velocity.X, Velocity.Y, (ushort)ModContent.ProjectileType<EternalScythe>(), Projectile.damage / 2, 4, Projectile.owner);
                    SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                }
                if (Projectile.localAI[0] % 20 == 0)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Velocity.X, Velocity.Y, (ushort)ModContent.ProjectileType<EternalLaser>(), Projectile.damage / 3, 1, Projectile.owner);
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
            => true;

        public override void PostDraw(Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/EyeOfEternity_Glow");
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, glowRotation, origin, 1f, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
