
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
    public class 腐化虫巢 : ModNPC
	{
        public static Asset<Texture2D> L;
        public static Asset<Texture2D> L2;
        public override void Load()
        {
            L = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/腐化蠕虫身");
            L2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/腐化蠕虫身2");
        }
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
        }

		public override void SetDefaults()
		{
			NPC.damage = 66;
			NPC.width = 94;
			NPC.height = 124;
			NPC.aiStyle = -1;
			NPC.defense = 20;
			NPC.scale = 1f;
            NPC.lifeMax = 9750;
            NPC.knockBackResist = 0;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Dnpc().Neutrality = true;
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
            NPC.damage = 0;
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            NPC.boss = !NPC.Dnpc().Neutrality;
            if (NPC.boss)
            {
                if (!Main.dedServ && Music == -1) Music = DDSystem.MiniBossMusic;
            }
            else
            {
                if (!Main.dedServ) Music = -1;
            }
            Point point = new Point((int)(NPC.position.X / 16) - 3, (int)(NPC.position.Y + NPC.height) / 16);
            for (int A = 0;A<NPC.width/16+8;A++)
            {
                Tile tile = Main.tile[point.X+A, point.Y];
                if(!tile.HasTile)
                {
                    tile.TileType = 25;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(point.X + A, point.Y);
                }
            }
            point.Y++;
            for (int A = 0;A<NPC.width/16+8;A++)
            {
                Tile tile = Main.tile[point.X+A, point.Y];
                if(!tile.HasTile)
                {
                    tile.TileType = 25;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(point.X + A, point.Y);
                }
            }
            point.Y++;
            for (int A = 0;A<NPC.width/16+8;A++)
            {
                Tile tile = Main.tile[point.X+A, point.Y];
                if (!tile.HasTile)
                {
                    tile.TileType = 25;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(point.X + A, point.Y);
                }
            }
            if((player.Center-NPC.Center).Length()<600)
            {
                //战斗模式
                NPC.Dnpc().Bool[1] = true;
                NPC.ai[1] = 0;
                NPC.ai[0]++;
                if (NPC.ai[0] %60 == 0)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position+new Vector2(Main.rand.Next(20,NPC.width-20), Main.rand.Next(20,NPC.height-20)), 112, NPC.whoAmI);
                }
                if (NPC.ai[0] %180 == 0)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position+new Vector2(Main.rand.Next(20,NPC.width-20), Main.rand.Next(20,NPC.height-20)), 6, NPC.whoAmI);
                }
                if (NPC.ai[0] %492 == 0&&!NPC.AnyNPCs(98))
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position+new Vector2(Main.rand.Next(20,NPC.width-20), Main.rand.Next(20,NPC.height-20)), 98, NPC.whoAmI);
                }
                if (NPC.ai[0] %327 == 0)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position+new Vector2(Main.rand.Next(20,NPC.width-20), Main.rand.Next(20,NPC.height-20)), 94, NPC.whoAmI);
                }
                NPC.Dnpc().Neutrality = false;
            }
            else
            {
                NPC.ai[1]++;
                if (NPC.ai[1]>600)
                {
                    NPC.Dnpc().Bool[1] = false;
                }
                NPC.Dnpc().Neutrality = true;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (Main.netMode != 1&&NPC.CountNPCS(ModContent.NPCType<腐化爬藤怪>())<5)
                {
                    for (int A = 0; A < 5; A++)
                    {
                        DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<腐化爬藤怪>(), NPC.whoAmI, NPC.whoAmI);
                    }
                }
                NPC.Dnpc().Stage = 1;
            }
            else
            {
                if (Main.netMode != 1 && NPC.CountNPCS(ModContent.NPCType<腐化爬藤怪>()) < 5)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2]>300)
                    {
                        NPC.ai[2] = 0;
                        DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<腐化爬藤怪>(), NPC.whoAmI, NPC.whoAmI);
                    }
                }

            }
            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height);
            bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height-1);
            if (!TileCollision)
            {
                NPC.velocity.Y += NPC.gravity;
                if (NPC.velocity.Y > NPC.maxFallSpeed)
                {
                    NPC.velocity.Y = NPC.maxFallSpeed;
                }
            }
            else
            {
                if(TileCollision2)
                {
                    NPC.velocity.Y = -1;
                }
                else
                NPC.velocity.Y = 0;
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
            if (NPCdirection.Incident(spawnInfo)|| NPC.AnyNPCs(Type))
            {
                return 0;

            }
            if (!spawnInfo.Player.HasBuff(ModContent.BuffType<丛林香水Buff>()) && !NPC.downedBoss2)
            {
                return 0;
            }
            int[] TileArray = { 60 };
            if(TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType))
            {
                if(spawnInfo.Player.HasBuff(ModContent.BuffType<丛林香水Buff>()))
                {
                    return 0.1f;
                }
                else if(NPCDowned.丛林暴食怪)
                {
                    return 0.001f;
                }
                else
                {
                    return 0.02f;
                }
            }

            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //database.Entries.Remove(bestiaryEntry);
        }
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
            parameters.MinimumItemDropsCount = 6;
            parameters.MaximumItemDropsCount = 9;
            parameters.ChanceNumerator = 1;
            parameters.ChanceDenominator = 1;
            parameters.MinimumStackPerChunkBase = 2;
            parameters.MaximumStackPerChunkBase = 4;
            parameters.BonusMinDropsPerChunkPerPlayer = 1;
            parameters.BonusMaxDropsPerChunkPerPlayer = 2;
            DropOneByOne.Parameters parameters2 = parameters;
            DropOneByOne.Parameters parameters3 = parameters2;
            DropOneByOne.Parameters parameters4 = parameters3;
            parameters3.BonusMinDropsPerChunkPerPlayer = 2;
            parameters3.BonusMaxDropsPerChunkPerPlayer = 3;
            parameters4.BonusMinDropsPerChunkPerPlayer = 3;
            parameters4.BonusMaxDropsPerChunkPerPlayer = 4;
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(521, parameters2), new DropOneByOne(521, parameters3), new DropOneByOne(521, parameters4)));

            parameters2.MinimumItemDropsCount = 10;
            parameters2.MaximumItemDropsCount = 12;
            parameters3.MinimumItemDropsCount = 10;
            parameters3.MaximumItemDropsCount = 12;
            parameters4.MinimumItemDropsCount = 10;
            parameters4.MaximumItemDropsCount = 12;
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(1332, parameters2), new DropOneByOne(1332, parameters3), new DropOneByOne(522, parameters4)));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            spriteBatch.Draw(texture, NPC.Center + new Vector2(0, NPC.height / 2 + NPC.gfxOffY+2) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), NPC.scale, 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
        }
        public override void OnKill()
        {
            //SetEventFlagCleared(ref NPCDowned.丛林暴食怪, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 4; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 18, 0f, 0f, 100, default, 1f);
                Main.dust[D].noGravity = false;
                Main.dust[D].scale *= 2f + Main.rand.Next(1)/2;
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-3, -0)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
            if (NPC.life <= 0)
            {
                for (int A = 0; A < 350; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 18, 0f, 0f, 100, default, 1f);
                    Main.dust[D].noGravity = false;
                    Main.dust[D].scale *= 2f + Main.rand.Next(2)/2;
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-5, -0)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
                for (int A = 0; A < 8; A++)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(20, NPC.width - 20), Main.rand.Next(20, NPC.height - 20)), 6, NPC.whoAmI);
                }
                for (int A = 0; A < 2; A++)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(20, NPC.width - 20), Main.rand.Next(20, NPC.height - 20)), 98, NPC.whoAmI);
                }
                for (int A = 0; A < 5; A++)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(20, NPC.width - 20), Main.rand.Next(20, NPC.height - 20)), 94, NPC.whoAmI);
                }
            }
        }
	}
}
