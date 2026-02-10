
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Tiles.Trophy;
using DDmod.Worlds;
using System.Linq;
using Terraria;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 超级蓝史莱姆 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;

		}

		public override void SetDefaults()
		{
			NPC.damage = 28;
			NPC.width = 64;
			NPC.height = 46;
			NPC.aiStyle = -1;
			NPC.defense = 4;
			NPC.scale = 1f;
			NPC.lifeMax = 1200;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 50, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
			NPC.scale = 1.3F;
            NPC.noTileCollide = true;
			NPC.boss = true;
            NPC.Dnpc().Properties.Gel = true;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Neutrality = true;
        }
        public override void ModifyTypeName(ref string typeName)
        {
            if (NPC.Dnpc().Stage == 0)
			{
				typeName = "???";
			}
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        int Landing = 0;
        public override void AI()
        {
			Player player = Main.player[NPC.target];
			
            // NPC和物块相撞
            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 1)), NPC.width, 1);
            bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 2)), NPC.width, 1);
            int PO = (int)((player.position.Y + player.height) / 16 - (NPC.position.Y + NPC.height) / 16);
            PO *= 16;
			if (NPC.Dnpc().Stage == 2)
			{
				NPC.scale = ((float)NPC.life / NPC.lifeMax) / 2 + 0.8F;

				NPC.position = NPC.Center;
				NPC.width = (int)(64 * (NPC.scale * (1 - NPC.Dnpc().Times[4])));
				NPC.Center = NPC.position;

                
				NPC.position += new Vector2(0, NPC.height);
				NPC.height = (int)(46 * (NPC.scale * (1 + NPC.Dnpc().Times[4])));
				NPC.position -= new Vector2(0, NPC.height);
			}
            if (Collision.WetCollision(NPC.position, NPC.width, NPC.height))
			{
				if (NPC.velocity.Y > -1)
				{
					NPC.velocity.Y -= 0.03F;
				}
			}
			else
			{
				NPC.velocity.Y += NPC.gravity;
                if(NPC.velocity.Y>0)
                {
                    NPC.velocity.Y += NPC.gravity/2;
                }
				if (NPC.velocity.Y > NPC.maxFallSpeed)
				{
					NPC.velocity.Y = NPC.maxFallSpeed;
                }
                if ((NPC.velocity.Y > 0 && PO <= 16))
                    NPC.velocity.Y = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false).Y;

            }

            if (TileCollision || TileCollision2)
            {
                if ((NPC.velocity.Y > 0 && PO <= 16) || NPC.velocity.X == 0)
                {
                    NPC.velocity.Y = 0;
                }
            }
            if (NPC.Dnpc().Stage==0)
            {
                NPC.damage = 0;
                NPC.boss = false;

                if (((float)NPC.life / NPC.lifeMax)!=1)
				{
					NPC.Dnpc().Stage = 1;
                    NPC.netUpdate = true;
                }
                NPC.scale = 0.1F;

                NPC.position = NPC.Center;
                NPC.width = (int)(64 * (NPC.scale * (1 - NPC.Dnpc().Times[4])));
                NPC.Center = NPC.position;

                NPC.position = NPC.Center + new Vector2(0, NPC.height / 2);
                NPC.height = (int)(46 * (NPC.scale * (1 + NPC.Dnpc().Times[4])));
                NPC.Center = NPC.position - new Vector2(0, NPC.height / 2);
                return;
            }
            NPC.damage = NPC.defDamage;
            NPC.Dnpc().Neutrality = false;
            NPC.chaseable = true;
            NPC.boss = true;
            if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
            NPC.NPCHB().MiniBoss = true;

            if (NPC.Dnpc().Stage == 1)
			{
				NPC.dontTakeDamage = true;
				if (NPC.scale < (((float)NPC.life / NPC.lifeMax) / 2 + 0.8F))
				{
					NPC.scale += 0.01F;
                    NPC.position = NPC.Center;
                    NPC.width = (int)(64 * (NPC.scale * (1 - NPC.Dnpc().Times[4])));
                    NPC.Center = NPC.position;

                    NPC.position = NPC.Center + new Vector2(0, NPC.height / 2);
                    NPC.height = (int)(46 * (NPC.scale * (1 + NPC.Dnpc().Times[4])));
                    NPC.Center = NPC.position - new Vector2(0, NPC.height / 2);

                    for (int num251 = 0; num251 < 10; num251++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 4, NPC.velocity.X, NPC.velocity.Y, 150, new Color(170, 150, 255, 55), NPC.scale+0.2F)];
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                    }
                }
				else
				{
					NPC.Dnpc().Stage = 2;
					NPC.velocity.Y = -4;
                    NPC.scale = ((float)NPC.life / NPC.lifeMax) / 2 + 0.8F;
                    for (int A = 0; A < 100; A++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                        Vector2 vector2 = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                        Main.dust[D].velocity = vector2 * 2;
                    }
                    NPC.netUpdate = true;
                }
				return;
			}
            NPC.dontTakeDamage = false;
            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X *= 0.92F;
                if (PO > 16)
				{
					NPC.position.Y += 4;
                }
                NPC.ai[0]++;
            }

            NPC.TargetClosest();
			Vector2 vector = player.Center - NPC.Center;

            if (NPC.ai[0] % 300 == 0)
			{
				NPC.ai[0]++;


				if (NPC.ai[1] >0&& NPC.ai[1] < 4)
				{
					NPC.velocity.X = vector.PerfectNormalize().X * 8/ NPC.scale;
					NPC.velocity.Y = -10;
				}
				else if (NPC.ai[1] == 4)
                {
                    NPC.velocity.X = vector.PerfectNormalize().X * 8 / NPC.scale;
                    NPC.velocity.Y = -14;
                }
				else if (NPC.ai[1]>4 && NPC.ai[1] < 10)
                {
					NPC.ai[0] += 200;
                    NPC.velocity.X = vector.PerfectNormalize().X * 4 / NPC.scale;
                    NPC.velocity.Y = -8;
                }
                else if (NPC.ai[1] == 10)
                {
                    NPC.velocity.X = vector.PerfectNormalize().X * 8 / NPC.scale;
                    NPC.velocity.Y = -14;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
                NPC.ai[1]++;
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo)|| NPC.AnyNPCs(Type))
            {
                return 0;
            }
            int[] TileArray = { 2 };
            if(TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType))
            {
                if(NPCDowned.超级蓝史莱姆)
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
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.超级蓝史莱姆"))
            ]);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));

            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<LegendaryGelItem>()));
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegendaryGelItem>()));

			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<超级蓝史莱姆圣物>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<超级蓝史莱姆纪念章物品>()));
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            Vector2 vector = NPC.Center - new Vector2(0, 4 * (1.3F - NPC.scale) + 14 * NPC.Dnpc().Times[4] / 10 * NPC.scale) - screenPos;
            if (NPC.Dnpc().Stage == 0 && NPC.boss)
            {
                vector = NPC.Center - new Vector2(0, 6 * NPC.Dnpc().Times[4] / 10 * NPC.scale - 10) - screenPos;
            }
            spriteBatch.Draw(TextureAssets.Item[ModContent.ItemType<LegendaryGelItem>()].Value, vector, null, drawColor, NPC.rotation, TextureAssets.Item[ModContent.ItemType<LegendaryGelItem>()].Size() / 2, 1, 0, 0f);

            drawColor *= 0.7F;
            spriteBatch.Draw(texture, NPC.Center + new Vector2(0, NPC.height / 2 + NPC.gfxOffY) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height - 2), NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4] / 2, 1 + NPC.Dnpc().Times[4] / 2), 0, 0f);
            drawColor.A = 0;
            drawColor.R = 105;
            drawColor.G = 20;
            drawColor.B = 40;

            spriteBatch.Draw(texture, NPC.Center + new Vector2(0, NPC.height / 2 + NPC.gfxOffY) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height - 2), NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4] / 2, 1 + NPC.Dnpc().Times[4] / 2), 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 16,
            };

            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
            if (NPC.velocity.Y == 0f)
            {
                float TY = NPC.ai[0] % 300;
                if (TY > 240 && TY < 300 && NPC.ai[1] > 0)
                {
                    //int W = 4;
                    //if (NPC.frameCounter % W == 0)
                    {
                        NPC.Dnpc().Times[4] -= 0.02F;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.5F, -0.5F);
                    }
                }
                else
                {
                    if (NPC.Dnpc().Times[3] > 0.1F)
                    {
                        NPC.Dnpc().Times[3] -= 0.008F;

                    }
                    else
                    {
                        NPC.Dnpc().Times[3] = 0.1f;
                    }
                    DDHelper.BackAndForth(-NPC.Dnpc().Times[3], NPC.Dnpc().Times[3], NPC.Dnpc().Times[3] / 6, ref NPC.Dnpc().Times[4], ref NPC.Dnpc().Bool[4]);
                }
                speed = 0;
            }
            else
            {
                if (speed == 0)
                {
                    speed = Math.Abs(NPC.velocity.Y);
                }
                NPC.Dnpc().Times[3] = 0.9F;

                if (NPC.velocity.Y < 0)
                {
                    if (Math.Abs(NPC.velocity.Y) < speed / 2)
                    {
                        NPC.Dnpc().Times[3] = speed / 20;
                        if (NPC.Dnpc().Times[3] > 0.1F)
                        {
                            NPC.Dnpc().Times[3] -= 0.008F;

                        }
                        else
                        {
                            NPC.Dnpc().Times[3] = 0.1f;
                        }
                        DDHelper.BackAndForth(-NPC.Dnpc().Times[3], NPC.Dnpc().Times[3], NPC.Dnpc().Times[3] / 4, ref NPC.Dnpc().Times[4], ref NPC.Dnpc().Bool[4]);
                    }
                    else
                    {
                        NPC.Dnpc().Times[4] += Math.Abs(NPC.velocity.Y) / 40;
                        NPC.Dnpc().Bool[4] = false;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.8F, -0.8F);
                    }
                }
                else
                {
                    NPC.Dnpc().Times[4] += Math.Abs(NPC.velocity.Y) / 50;
                    NPC.Dnpc().Bool[4] = false;
                    DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.8F, -0.8F);
                }
            }
            NPC.frameCounter++;
            NPC.frame.Y = 0;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.超级蓝史莱姆, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 10; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 300; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
			}
		}
	}
}
