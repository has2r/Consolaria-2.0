using Consolaria.Content.Projectiles.Enemies;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Turkor {
    [AutoloadBossHead]
    public class TurkortheUngratefulHead : ModNPC {
        private int turntimer = 0;
        private ref float timer => ref NPC.ai[0];

        private bool spawn = false;
        private bool charge {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value ? 1f : 0f;
        }
        private bool chase = false;
        private bool projSpam {
            get => NPC.ai[3] == 1f;
            set => NPC.ai[3] = value ? 1f : 0f;
        }
        private bool attackingPhase = false;

        private int hurtFrame = 0;
        private float rotatepoint = 0;

        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults() {
            int width = 50; int height = 100;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;

            NPC.damage = 55;
            NPC.defense = 10;
            NPC.lifeMax = 1200;


            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble");

            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;

            NPC.alpha = 255;

            NPC.lavaImmune = true;
            NPC.noGravity = true;

            NPC.BossBar = Main.BigBossProgressBar.NeverValid;

            if (Main.masterMode) NPC.lifeMax = (int)(NPC.lifeMax * 0.798f);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
            NPC.lifeMax = (int)((double)NPC.lifeMax * 0.5f * (double)balance * (double)bossAdjustment);
            NPC.damage = (int)((double)NPC.damage * 0.65f);
        }

        private Rectangle GetFrame(int number)
            => new Rectangle(0, NPC.frame.Height * (number - 1), NPC.frame.Width, NPC.frame.Height);

        public override void FindFrame(int frameHeight) {
            if (!attackingPhase || charge && timer > 230 || projSpam) {
                /*if (!projSpam && NPC.velocity.X * NPC.direction < 0 && turntimer < 15) {
                    turntimer++;
                    NPC.frame = GetFrame(4);
                }
                else */
                if (hurtFrame > 0) {
                    NPC.frame = GetFrame(3);
                    hurtFrame--;
                }
                else {

                    //if (NPC.velocity.X * NPC.direction > 0) { turntimer = 0; }
                    if (!projSpam) NPC.spriteDirection = NPC.direction;
                    if (charge || !attackingPhase) {
                        if (turntimer <= 0 || charge)
                        {
                            NPC.frameCounter += 0.08f;
                            NPC.frameCounter %= 2;
                            int frame = (int)NPC.frameCounter;
                            NPC.frame.Y = frame * frameHeight;
                        }
                        else
                        {
                            turntimer--;
                            NPC.spriteDirection = 1;
                            NPC.frame = GetFrame(4);
                        }
                    }
                    else if (projSpam && timer % 80 < 20) NPC.frame = GetFrame(2);
                    else NPC.frame = GetFrame(1);
                }
            }
            if (charge && timer <= 230) NPC.frame = GetFrame(4);
        }

        //public override bool CanHitPlayer (Player target, ref int cooldownSlot) => charge;
        static int segmentCount = 31;
        Vector2[] body = new Vector2[segmentCount];
        float[] rotor = new float[segmentCount];
        int oldDir;
        Vector2 RandP;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Content/NPCs/Bosses/Turkor/TurkorNeck");
            
            for (int k = 1; k < segmentCount-1; k++)
            {
                spriteBatch.Draw(texture, body[k] - Main.screenPosition,
                       null, Lighting.GetColor((int)body[k].X / 16, (int)(body[k].Y / 16f))*((255-NPC.alpha)/255f), rotor[k] - 1.57f,
                       texture.Size()/2, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void AI() {
            NPC.direction = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
            if (NPC.direction != oldDir && turntimer<=0)
            {
                turntimer = 10;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                if (!spawn) {
                    oldDir = NPC.direction;
                    NPC.realLife = NPC.whoAmI;
                    /*int neck = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<TurkorNeck>(), NPC.whoAmI, 0, NPC.whoAmI); //, 1, NPC.ai[1]);
                    Main.npc[neck].ai[2] = 30;
                    Main.npc[neck].ai[3] = -1f;
                    Main.npc[neck].realLife = NPC.whoAmI;
                    Main.npc[neck].ai[0] = NPC.whoAmI;
                    Main.npc[neck].ai[1] = NPC.whoAmI;
                    Main.npc[neck].position = NPC.position;
                    NetMessage.SendData(MessageID.SyncNPC, number: neck);*/
                    for (int k = 0; k < segmentCount; k++)
                    {
                        body[k] = NPC.Center;
                    }
                    spawn = true;
                    NPC.netUpdate = true;
                }
                else
                {
                    //F
                    for (int k = segmentCount - 2; k > 0; k--)
                    {
                        Vector2 To = Vector2.Normalize(body[k + 1] - body[k - 1] - new Vector2(0, (float)Math.Sin((float)(Math.PI * 2 * k / (segmentCount - 1))) * 4f));
                        float dis = 7.3f;
                        body[k] += (body[k + 1] - To * dis - body[k]) / ((k> (int)(segmentCount/1.5f)) ?1.1f:1.35f);

                    }
                    //B
                    for (int k = 1; k < segmentCount - 1; k++)
                    {
                        Vector2 To = Vector2.Normalize(body[k + 1] - body[k - 1] + new Vector2(0, (float)Math.Sin((float)(Math.PI * 2 * k / (segmentCount - 1))) * 4f));
                        //rotor[k] = To.ToRotation();
                        float dis = 7.3f;
                        body[k] += (body[k - 1] + To * dis - body[k]) / 1.35f;
                    }
                    for(int k = 1; k <segmentCount-1; k++)
                    {
                        rotor[k] = (body[k + 1] - body[k]).ToRotation();
                    }
                    body[0] = NPC.Center + new Vector2(0, 30);
                    body[segmentCount - 1] = Main.npc[(int)NPC.ai[1]].Center + new Vector2(-50, 20);
                }
            }
            if (!Main.npc[(int)NPC.ai[1]].active) {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if (NPC.alpha >= 0) NPC.alpha -= 15;

            timer++;
            if (Main.player[NPC.target].dead) {
                timer = 0;
                charge = false;
                NPC.TargetClosest(false);
                NPC.velocity.Y -= 0.1f;
                if (NPC.timeLeft > 10 && Main.player[NPC.target].dead) {
                    NPC.timeLeft = 10;
                    return;
                }
                NPC.netUpdate = true;
            }
            else if (!Main.player[NPC.target].dead) NPC.TargetClosest(true);

            if (timer > 200 && Main.rand.NextBool(50) && !attackingPhase) {
                attackingPhase = true;
                //pick random attack
                switch (Main.rand.Next(2)) {
                    case 0:
                        charge = true;
                        break;

                    case 1:
                        projSpam = true;
                        break;

                    default:
                        break;
                }
                timer = 200;
                NPC.velocity *= 0.46f;
                NPC.rotation = 0;
                NPC.netUpdate = true;
            }

            //attack1: charge at player
            if (charge) {
                if (timer <= 230) {
                    NPC.rotation = Vector2.UnitY.RotatedBy((double)(timer / 40f * 6.2f), default).Y * 0.2f;
                }
                if (timer >= 230) {
                    NPC.rotation = 0;
                    if (timer <= 230) SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble"), NPC.position); // SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y, 10);

                    NPC.velocity.X *= 0.98f;
                    NPC.velocity.Y *= 0.98f;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
                    {
                        float rotation = (float)Math.Atan2((vector8.Y) - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), (vector8.X) - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        NPC.velocity.X = (float)(Math.Cos(rotation) * 14) * -1;
                        NPC.velocity.Y = (float)(Math.Sin(rotation) * 14) * -1;
                    }
                }

                //if near player then bounce back
                if (timer >= 230 && Main.player[NPC.target].Distance(NPC.Center) <= 30) {
                    timer = 0;
                    charge = false;
                    attackingPhase = false;
                    NPC.velocity.X *= -0.38f;
                    NPC.velocity.Y *= -0.38f;
                }

                //if hit/running out of time then bounce back
                if (timer > 270 || timer > 230 && NPC.justHit) {
                    if (timer <= 270) hurtFrame = 20;
                    timer = 0;
                    charge = false;
                    attackingPhase = false;
                    NPC.velocity.X *= -0.38f;
                    NPC.velocity.Y *= -0.38f;
                }
                NPC.netUpdate = true;
            }

            //attck2: spawn feather around it self while slowly drift toward the player
            if (projSpam) {
                if (NPC.spriteDirection == -1) NPC.rotation = rotatepoint;
                else NPC.rotation = -rotatepoint;
                if (rotatepoint <= 1.5f && timer < 360) rotatepoint += 0.1f;
                if (!chase) {
                    NPC.velocity.X *= 0.86f;
                    NPC.velocity.Y *= 0.86f;
                    NPC.netUpdate = true;
                }

                if (timer % 80 == 0 && rotatepoint >= 1.5f) {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        for (int i = 0; i < 3; i++) {
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Main.rand.NextFloat(-5, 5) + (Main.player[NPC.target].Center-NPC.Center).X/50, -12 + Main.rand.NextFloat(-3, 0), ModContent.ProjectileType<TurkorFeather>(), 
                                26, 1, Main.myPlayer);
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                        }
                    NPC.velocity.Y = 5;
                    SoundEngine.PlaySound(SoundID.NPCDeath48, NPC.position);
                    NPC.netUpdate = true;
                }
                if (timer >= 360) {
                    rotatepoint -= 0.1f;
                    if (rotatepoint <= 0) {
                        timer = 0;
                        projSpam = false;
                        attackingPhase = false;
                        NPC.rotation = 0;
                    }
                }
                NPC.netUpdate = true;
            }

            if (!charge) {
                Vector2 pPos = Main.player[NPC.target].Center + RandP;
                Vector2 getTo = Main.npc[(int)NPC.ai[1]].Center + Vector2.Normalize(pPos - NPC.Center)*Math.Min(800, pPos.Distance(NPC.Center));
                if (getTo.Distance(NPC.Center) <= 100)
                    RandP = new Vector2(Main.rand.NextFloat(-200, 201), Main.rand.NextFloat(-200, -100));
                if (getTo.X < NPC.Center.X)
                {
                    if (NPC.velocity.X > -6) NPC.velocity.X -= 0.08f;
                }
                else if (getTo.X > NPC.Center.X)
                {
                    if (NPC.velocity.X < 6) NPC.velocity.X += 0.08f;
                }
                if (getTo.Y < NPC.Center.Y)
                {
                    if (NPC.velocity.Y > -6) NPC.velocity.Y -= 0.08f;
                }
                else if (getTo.Y > NPC.Center.Y)
                {
                    if (NPC.velocity.Y < 6) NPC.velocity.Y += 0.08f;
                }
                /*if (!chase) {
                    if (Main.player[NPC.target].Center.X - Main.rand.Next(-200, 201) < NPC.Center.X) {
                        if (NPC.velocity.X > -6) NPC.velocity.X -= 0.08f;
                    }
                    else if (Main.player[NPC.target].Center.X - Main.rand.Next(-200, 201) > NPC.Center.X) {
                        if (NPC.velocity.X < 6) NPC.velocity.X += 0.08f;
                    }
                    if (Main.player[NPC.target].Center.Y - Main.rand.Next(-150, 201) < NPC.Center.Y) {
                        if (NPC.velocity.Y > -6) NPC.velocity.Y -= 0.14f;
                    }
                    else if (Main.player[NPC.target].Center.Y - Main.rand.Next(-150, 201) > NPC.Center.Y) {
                        if (NPC.velocity.Y < 6) NPC.velocity.Y += 0.14f;
                    }
                }
                else {
                    if (Main.npc[(int)NPC.ai[1]].Center.X - Main.rand.Next(-200, 201) < NPC.Center.X) {
                        if (NPC.velocity.X > -6) NPC.velocity.X -= 0.08f;
                    }
                    else if (Main.npc[(int)NPC.ai[1]].Center.X - Main.rand.Next(-200, 201) > NPC.Center.X) {
                        if (NPC.velocity.X < 6) NPC.velocity.X += 0.08f;
                    }
                    if (Main.npc[(int)NPC.ai[1]].Center.Y - Main.rand.Next(-150, 201) < NPC.Center.Y) {
                        if (NPC.velocity.Y > -6) NPC.velocity.Y -= 0.14f;
                    }
                    else if (Main.npc[(int)NPC.ai[1]].Center.Y - Main.rand.Next(-150, 201) > NPC.Center.Y) {
                        if (NPC.velocity.Y < 6) NPC.velocity.Y += 0.14f;
                    }
                }
                Vector2 vector101 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num855 = Main.npc[(int)NPC.ai[1]].Center.X - vector101.X;
                float num856 = Main.npc[(int)NPC.ai[1]].Center.Y - vector101.Y;
                float num857 = (float)Math.Sqrt((double)(num855 * num855 + num856 * num856));
                if (num857 > 600f) chase = true;
                if (num857 <= 400 && chase) chase = false;*/
                NPC.netUpdate = true;
            }
            oldDir = NPC.direction;
        }

        public override void HitEffect(NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            if (NPC.life <= 0) {
                if (NPC.life <= 0) {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-3, 4)), ModContent.Find<ModGore>("Consolaria/TurkorBeakGore").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorEyeGore").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorEyeGore").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
                }
                for(int k = 1; k < segmentCount - 1; k++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), body[k], new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), ModContent.Find<ModGore>("Consolaria/TurkorNeck").Type);
                    }
                }
                for (int k = 0; k < 10; k++) {
                    int dust_ = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 3f * hit.HitDirection, -3f, 0, default, 2f);
                    Main.dust[dust_].velocity *= 0.2f;
                }
            }
        }
    }
}