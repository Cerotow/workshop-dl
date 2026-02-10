
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
    public class 丛林暴食怪根 : ModNPC
	{
        public static Asset<Texture2D> L;
        public static Asset<Texture2D> L2;
        public static Asset<Texture2D> ML;
        public static Asset<Texture2D> ML2;
        public override void Load()
        {
            L = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/丛林暴食怪身");
            L2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/丛林暴食怪身2");
            ML = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/小丛林暴食怪身");
            ML2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/小丛林暴食怪身2");
        }
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;

        }

		public override void SetDefaults()
		{
			NPC.damage = 28;
			NPC.width = 40;
			NPC.height = 22;
			NPC.aiStyle = -1;
			NPC.defense = 0;
			NPC.scale = 1f;
			NPC.lifeMax = 6000;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 50, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.Grass;
            NPC.HitSound = SoundID.Grass;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Grass = true;
            NPC.Dnpc().Neutrality = true;
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
            NPC.damage = 0;
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if((player.Center-NPC.Center).Length()<600)
            {
                //战斗模式
                NPC.Dnpc().Bool[1] = true;
                NPC.ai[1] = 0;
            }
            else
            {
                NPC.ai[1]++;
                if (NPC.ai[1]>600)
                {
                    NPC.Dnpc().Bool[1] = false;
                }
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (Main.netMode != 1)
                {
                    NPC.ai[0] = DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<丛林暴食怪>(), NPC.whoAmI, NPC.whoAmI);
                    for(int a= -1;a<=2;a++)
                    {
                        DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center+new Vector2(-50*a,0), ModContent.NPCType<小丛林暴食怪>(), NPC.whoAmI, NPC.whoAmI);
                    }
                    NPC.netUpdate = true;
                }
                NPC.Dnpc().Stage = 1;
            }
            
            if(NPC.Dnpc().Stage==1)
            {
                if (NPC.AnyNPCs(ModContent.NPCType<小丛林暴食怪>())|| !NPC.Dnpc().Bool[1])
                {
                    if (NPC.life < NPC.lifeMax)
                    {
                        NPC.life += NPC.lifeMax / 100 + 1;
                    }
                    if (NPC.life > NPC.lifeMax)
                    {
                        NPC.life = NPC.lifeMax;
                    }
                    if (!NPC.Dnpc().Bool[1] && NPC.life >= NPC.lifeMax && NPC.CountNPCS(ModContent.NPCType<小丛林暴食怪>()) <= 3)
                    {
                        if (Main.netMode != 1)
                        {
                            DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 0), ModContent.NPCType<小丛林暴食怪>(), NPC.whoAmI, NPC.whoAmI);
                            NPC.netUpdate = true;
                        }
                    }
                }
                bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height-9);
                bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height - 10);
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
                    if (TileCollision2)
                    {
                        NPC.velocity.Y = -1;
                    }
                    else
                        NPC.velocity.Y = 0;
                }

                NPC npc = Main.npc[(int)NPC.ai[0]];
                if(!npc.active||npc.type != ModContent.NPCType<丛林暴食怪>())
                {
                    NPC.Kill(false);
                }
                if (NPC.velocity == Vector2.Zero && !NPC.Dnpc().Bool[0])
                {
                    NPC.position.Y += 1;
                    NPC.Dnpc().Bool[0] = true;
                }
                if(NPC.velocity!= Vector2.Zero)
                {
                    NPC.Dnpc().Bool[0] = false;
                }
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
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
            database.Entries.Remove(bestiaryEntry);
        }
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<荆棘戒指>()));
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;


            spriteBatch.Draw(texture, NPC.Center + new Vector2(0, NPC.height / 2 + NPC.gfxOffY) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height - 2), NPC.scale, 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.丛林暴食怪, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 4; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(2, 4), 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
            if (NPC.life <= 0)
            {
                for (int A = 0; A < 50; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(2, 4), 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
            }
        }
	}
}
