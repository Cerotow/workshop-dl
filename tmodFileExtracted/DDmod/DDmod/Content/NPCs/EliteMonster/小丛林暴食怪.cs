
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
    public class 小丛林暴食怪 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;

		}

		public override void SetDefaults()
        {
            NPC.damage = 25;
			NPC.width = 50;
			NPC.height = 50;
			NPC.aiStyle = -1;
			NPC.defense = 6;
			NPC.lifeMax = 800;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 20, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
			NPC.scale = 1F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Grass = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().BossPhysique = true;
            NPC.Dnpc().Properties.BossLife = 1.1f;
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
            if (!npc.active || npc.type != ModContent.NPCType<丛林暴食怪根>())
            {
                NPC.Kill(false);
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
                if(NPC.ai[1]<0)
                {
                    if (NPC.velocity.X > -0.3)
                    {
                        NPC.velocity.X -= 0.1F;
                    }
                    NPC.ai[1] += Math.Abs(NPC.velocity.X);
                    if(NPC.ai[1]>0)
                    {
                        NPC.ai[1] = 0;
                    }
                }
                else if(NPC.ai[1]>0)
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
                else if (Distance.Y>0)
                {
                    if (NPC.velocity.Y < 0.3)
                    {
                        NPC.velocity.Y += 0.1F;
                    }
                }
                if(Distance.Length()>300)
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
                if (NPC.ai[1]<300)
                {
                    if (Distance.Length() > 400)
                    {
                        NPC.velocity += Distance.PerfectNormalize() * 0.03F;
                        if (NPC.velocity.Length() > 3)
                            NPC.velocity *= 0.92F;
                    }
                    else
                    {

                            NPC.velocity += vector.PerfectNormalize() * 0.03F;
                    }
                }
                else if (NPC.ai[1] < 600)
                {
                    if (Distance.Length() > 400)
                    {
                        NPC.velocity += Distance.PerfectNormalize() * 0.05F;
                        if (NPC.velocity.Length() > 5)
                            NPC.velocity *= 0.92F;
                    }
                    else
                    {
 NPC.velocity += vector.PerfectNormalize() * 0.05F;
                    }
                }
                else
                {
                    NPC.ai[1] = 0;
                }
            }
            for (int a= 0;a<200;a++)
            {
                NPC N = Main.npc[a];
                if(N.active&&(N.type==Type||N.type==ModContent.NPCType<丛林暴食怪>())&&NPC.getRect().Intersects(N.getRect()))
                {
                    NPC.velocity += (NPC.Center- N.Center).PerfectNormalize()*0.2F; 
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
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.小丛林暴食怪"))
            ]);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Asset<Texture2D> L = 丛林暴食怪根.ML;
            Asset<Texture2D> L2 = 丛林暴食怪根.ML2;
            NPC npc = Main.npc[(int)NPC.ai[0]];
            SpriteEffects spriteEffects = SpriteEffects.FlipVertically;
            if (npc.active)
            {

                Vector2 vector = NPC.Size / 2;
                for (int W = 0; W < (npc.Center - NPC.Center).Length() / L.Height(); W++)
                {
                    if (W < (npc.Center - NPC.Center).Length() / L.Height() - 1)
                    {
                        Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector;
                        Color color = Lighting.GetColor((int)P.X/16, (int)P.Y/16);
                        Main.spriteBatch.Draw(L.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), L.Height())), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(L2.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W-2) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), L2.Height())), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 14), 1, spriteEffects, 0f);
                    }
                    else
                    {
                        Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector;
                        Color color = Lighting.GetColor((int)P.X/16, (int)P.Y/16);
                        Main.spriteBatch.Draw(L.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), (int)(L.Height() - (L.Height() - (npc.Center - NPC.Center).Length() % L.Height())))), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(L2.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W-2) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), (int)(L2.Height() - (L2.Height() - (npc.Center - NPC.Center).Length() % L2.Height())))), color, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 14), 1, spriteEffects, 0f);
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
            if(NPC.frameCounter%8==0)
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
                int D = NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(2, 4), 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 30; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(2, 4), 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    NPC npc = Main.npc[(int)NPC.ai[0]];
                    Asset<Texture2D> L = 丛林暴食怪根.ML;
                    Asset<Texture2D> L2 = 丛林暴食怪根.ML2;
                    int GoreType = Mod.Find<ModGore>("小丛林暴食怪身").Type;
                    int GoreType2 = Mod.Find<ModGore>("小丛林暴食怪身2").Type;
                    int GoreType3 = Mod.Find<ModGore>("小丛林暴食怪1").Type;
                    int GoreType4 = Mod.Find<ModGore>("小丛林暴食怪2").Type;
                    int GoreType5 = Mod.Find<ModGore>("小丛林暴食怪3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType3, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType3, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType4, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType4, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 0), GoreType5, 1f);
                    Vector2 vector = NPC.Size / 2;

                    for (int W = 0; W < (npc.Center - NPC.Center).Length() / L.Height(); W++)
                    {
                        if (W < (npc.Center - NPC.Center).Length() / L.Height() - 1)
                        {
                            Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector;
                            Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType, 1f);
                            P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W-2) + vector;
                            Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType2, 1f);
                        }
                        else
                        {
                            Vector2 P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W) + vector;
                            Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType, 1f);
                            P = NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + L.Height() * W - 2) + vector;
                            Gore.NewGore(NPC.GetSource_Death(), P, new Vector2(0, 0), GoreType2, 1f);
                        }
                    }
                }
            }
		}
	}
}
