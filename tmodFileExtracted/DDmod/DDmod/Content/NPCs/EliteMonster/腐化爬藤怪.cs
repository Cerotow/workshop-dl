
using DDmod.Content.Dusts;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Tiles.Trophy;
using DDmod.Worlds;
using System.Linq;
using Terraria;

namespace DDmod.Content.NPCs.EliteMonster
{
    //[AutoloadBossHead]
    public class 腐化爬藤怪 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;

		}

		public override void SetDefaults()
        {
            NPC.damage = 60;
			NPC.width = 50;
			NPC.height = 50;
			NPC.aiStyle = -1;
			NPC.defense = 6;
			NPC.lifeMax = 500;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 0, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
			NPC.scale = 1F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Grass = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().BossPhysique = true;
            NPC.behindTiles = true;
            NPC.Dnpc().Properties.BossLife = 1.2f;
        }
        public override void ModifyTypeName(ref string typeName)
        {
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        int Landing = 0;
        public override void AI()
        {
            NPC.timeLeft = 100;
            NPC npc = Main.npc[(int)NPC.ai[0]];
            if (!npc.active || npc.type != ModContent.NPCType<腐化虫巢>())
            {
                NPC.Kill(false);
            }
            else
            {

                if (npc.getRect().Intersects(NPC.getRect()))
                {
                    if(NPC.velocity.Length()<3)
                    NPC.velocity = new Vector2(0,-1).RotatedBy(Main.rand.NextFloat(-1.2F,1.2F))*4;
                }
            }
            Vector2 Distance = npc.Center - NPC.Center;
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.life += NPC.lifeMax / 100 + 1;
                }
                if (NPC.life > NPC.lifeMax)
                {
                    NPC.life = NPC.lifeMax;
                }
                NPC.rotation = Distance.ToRotation() - MathHelper.PiOver2;
                if (NPC.ai[1] < 0)
                {
                    if (NPC.velocity.X > -0.3)
                    {
                        NPC.velocity.X -= 0.1F;
                    }
                    NPC.ai[1] += Math.Abs(NPC.velocity.X);
                    if (NPC.ai[1] > 0)
                    {
                        NPC.ai[1] = 0;
                    }
                }
                else if (NPC.ai[1] > 0)
                {
                    if (NPC.velocity.X < 0.3)
                    {
                        NPC.velocity.X += 0.1F;
                    }
                    NPC.ai[1] -= Math.Abs(NPC.velocity.X);
                    if (NPC.ai[1] < 0)
                    {
                        NPC.ai[1] = 0;
                    }
                }
                if (!NPC.Dnpc().Bool[0])
                {
                    if (NPC.velocity.Y > -0.3F)
                    {
                        NPC.velocity.Y -= 0.1F;
                    }
                }
                else if (Distance.Y > 0)
                {
                    if (NPC.velocity.Y < 0.3)
                    {
                        NPC.velocity.Y += 0.1F;
                    }
                }
                if (Distance.Length() > 200)
                {
                    NPC.velocity += Distance.PerfectNormalize() * 0.2F;
                }
                if (Distance.Y > 140)
                {
                    NPC.Dnpc().Bool[0] = true;
                }
                if (Distance.Y < 80)
                {
                    NPC.Dnpc().Bool[0] = false;
                    NPC.ai[1] = Main.rand.NextFloat(-50, 50);
                }
            }
            //战斗模式
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                Vector2 vector = player.Center - NPC.Center;
                NPC.rotation = Distance.ToRotation() - MathHelper.PiOver2;
                NPC.ai[1]++;
                if (NPC.ai[1]>=120+Main.rand.Next(360))
                {
                    NPC.ai[1] = 0;
                    NPC.NewNPCProj(NPC.Center, -Distance.PerfectNormalize()*6,96,30,0,-1);
                }
                if (Distance.Length() > 200|| Distance.Y<0)
                {
                    NPC.velocity += Distance.PerfectNormalize() * 0.1F;
                    if (NPC.velocity.Length() > 3)
                        NPC.velocity *= 0.92F;
                }
                else
                {
                    NPC.SmoothVelocity(vector.PerfectNormalize() * 4, 100);
                }
            }
            for (int A = 0; A < 200; A++)
            {

                NPC n = Main.npc[A];
                if (n.active && n.type == NPC.type && n.getRect().Intersects(NPC.getRect()))
                {
                    Vector2 vector = n.Center - NPC.Center;
                        NPC.velocity += vector.PerfectNormalize()*-0.2F;
                }
            }
            if (npc.Dnpc().Bool[1])
            {
                NPC.Dnpc().Neutrality = false;
                if (NPC.Dnpc().Stage!=1)
                {
                    NPC.Dnpc().Stage = 1;
                    NPC.ai[1] = 0;
                }
            }
            else
            {
                NPC.Dnpc().Neutrality = true;
                NPC.Dnpc().Stage = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            
            database.Entries.Remove(bestiaryEntry);
        }
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Asset<Texture2D> L = 腐化虫巢.L;
            Asset<Texture2D> L2 = 腐化虫巢.L2;
            NPC npc = Main.npc[(int)NPC.ai[0]];
            SpriteEffects spriteEffects = SpriteEffects.FlipVertically;
            float H = L.Height()-4;
            if (npc.active)
            {
                for (int W = 0; W < (npc.Center - NPC.Center).Length() / H; W++)
                {
                    if (W < (npc.Center - NPC.Center).Length() / H - 1)
                    {
                        Vector2 P = NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W);
                        Color color = Lighting.GetColor((int)P.X/16, (int)P.Y/16);
                        if (W %2== 0)
                        {
                            Main.spriteBatch.Draw(L.Value, NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W)- Main.screenPosition, null, color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, L2.Size() / 2, 1, spriteEffects, 0f);
                        }
                        else
                        Main.spriteBatch.Draw(L2.Value, NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W) - Main.screenPosition, null, color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, L2.Size()/2, 1, spriteEffects, 0f);
                    }
                    else
                    {
                        Vector2 P = NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W);
                        Color color = Lighting.GetColor((int)P.X/16, (int)P.Y/16);
                        if (W % 2 == 0)
                        {
                            Main.spriteBatch.Draw(L.Value, NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), (int)(L.Height() - (L.Height() - (npc.Center - NPC.Center).Length() % L.Height())))), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, L2.Size() / 2, 1, spriteEffects, 0f);
                        }
                        else Main.spriteBatch.Draw(L2.Value, NPC.Center + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W)  - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), (int)(L2.Height() - (L2.Height() - (npc.Center - NPC.Center).Length() % L2.Height())))), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, L2.Size() / 2, 1, spriteEffects, 0f);
                    }
                }
            }
            spriteBatch.Draw(texture, NPC.Center  - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size()/2, NPC.scale, 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if(NPC.frameCounter%4==0)
            {
                NPC.frame.Y += frameHeight;  
            }
            if (NPC.frame.Y>=frameHeight*5)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void OnKill()
        {
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 2; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 18, 0f, 0f, 100, default, 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1f + Main.rand.Next(2) / 2;
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 30; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 18, 0f, 0f, 100, default, 1f);
                    Main.dust[D].noGravity = false;
                    Main.dust[D].scale *= 1f + Main.rand.Next(2) / 2;
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    NPC npc = Main.npc[(int)NPC.ai[0]];
                    Asset<Texture2D> L = 丛林暴食怪根.L;
                    int GoreType = Mod.Find<ModGore>("腐化蠕虫身").Type;
                    int GoreType2 = Mod.Find<ModGore>("腐化蠕虫身2").Type;
                    int GoreType3 = 114;
                    int GoreType4 =115;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType3, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType3, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType4, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType4, 1f);
                    Vector2 vector = NPC.Size / 2;

                    float H = L.Height() - 4;
                    for (int W = 0; W < (npc.Center - NPC.Center).Length() / H; W+=2)
                    {
                        if (W < (npc.Center - NPC.Center).Length() / H - 1)
                        {
                            Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W) + vector;
                            if (W % 4 == 0)
                            {
                                Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType, 1f);
                            }
                            else
                            {
                                Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType2, 1f);

                            }
                            for (int A = 0; A < 10; A++)
                            {
                                int D = NewDust(P-new Vector2(12), 24, 24, 18, 0f, 0f, 100, default, 1f);
                                Main.dust[D].noGravity = false;
                                Main.dust[D].scale *= 1f + Main.rand.Next(2)/2;
                                Main.dust[D].velocity = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2()*Main.rand.NextFloat(8);
                            }
                        }
                        else
                        {
                            Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + H * W) + vector;
                            Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType, 1f);
                        }
                    }
                }
            }
		}
	}
}
