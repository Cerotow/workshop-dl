using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Dusts;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Sync;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Biome;
using DDmod.Content.Tiles.绿岩;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.Boss.绿岩之视
{
    [AutoloadBossHead]
    public class 绿岩之视 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
            DDSystem.HBar(NPC.type, "绿岩之视", new Vector2(22, 0));
        }

		public override void SetDefaults()
		{
			NPC.damage = 40;
			NPC.width = 110;
			NPC.height = 110;
			NPC.aiStyle = -1;
			NPC.defense = 18;
			NPC.scale = 1.1f;
			NPC.lifeMax = 4000;
			NPC.knockBackResist = 0;
			NPC.value = 3000;
			NPC.alpha = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
            NPC.Dnpc().Properties.Iron = true; ;
            if (!Main.dedServ)
            {
                Music =DDSystem.Music(2, "绿岩之视2");
                MusicA = DDSystem.Music(2, "绿岩之视");
            }
            NPC.NPCHB().Multiple = true;

            SpawnModBiomes = new int[] { ModContent.GetInstance<Biome.绿岩实验室>().Type };
            if (!Main.dedServ)
                NPC.GetGlobalNPC<NPCHealthBar>().HealthBarFrame(new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0));
            NPC.Dnpc().Properties.BossLife = 1.125F;
        }
		public static Asset<Texture2D> asset;
        public static int MusicA = -1;
        public override void Load()
        {
			asset = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/绿岩之视/绿岩眼");
            base.Load();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void BossLoot( ref int potionType)
        {
			potionType = 188;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            if (!spawnInfo.Player.ZoneForest || !NPCDowned.绿岩刷怪)
            {
                return 0;
            }
            return SpawnCondition.OverworldDay.Chance * 0.01f;
        }
        public override void AI()
        {
            int sd = -1;
            if(NPC.spriteDirection==1)
            {
                sd = 1;
            }
            /*
            for (int a = 0; a < 1; a++)
            {
                Dust dust = Main.dust[NewDust(NPC.Center+new Vector2(30*sd,-20), 1,1, ModContent.DustType<冰雾>(), 0f, 0f, -Main.rand.Next(20, 100), new Color(119, 237, 130, 50), Main.rand.NextFloat(0.02F, 0.05F))];
                Vector2 vector = new Vector2(10 *sd, -Main.rand.NextFloat(8, 18))+NPC.velocity;
                dust.velocity = vector;
                dust.noGravity = false;
            }*/
            NPC.TargetClosest();
			Player player = Main.player[NPC.target];
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.Dnpc().Stage = 1;
                if (Main.netMode != 1)
                {
                    int N = DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<绿岩炮>(), NPC.whoAmI);
                    NPC.localAI[0] = N;
                }
                NPC.netUpdate = true;
            }
            //NPC.Dnpc().Bool[1]战斗模式启动
            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y)), NPC.width, NPC.height+80);
            if ((NPC.velocity.X != 0) && !NPC.Dnpc().Bool[1])
            {
                Vector2 vector = player.Center - NPC.Center;
                Vector2 vector2 = vector / 10;
                if (vector2.Length() > 40)
                {
                    vector2 = vector2.PerfectNormalize() * 40;
                }
                if (NPC.velocity.X > 0)
				{
					NPC.velocity.X = 1;
				}
				else
				{
					NPC.velocity.X = -1;
				}
				NPC.direction = -1;
				NPC.spriteDirection = 0;
				if (NPC.velocity.X > 0)
				{
					NPC.direction = 1;
					NPC.spriteDirection = 1;

				}
                if (vector.Length()<500 && ((vector.X>0&& NPC.direction==1) || (vector.X < 0 && NPC.direction == -1)))
                {
                    NPC.Dnpc().vector[0] += (vector2 - NPC.Dnpc().vector[0]).PerfectNormalize() * (vector2 - NPC.Dnpc().vector[0]).Length() / 10;
                }
                else
                {
                    NPC.Dnpc().vector[0] += (new Vector2(21 * NPC.direction, 0).RotatedBy(NPC.rotation) - NPC.Dnpc().vector[0]).PerfectNormalize() * (new Vector2(21 * NPC.direction, 0).RotatedBy(NPC.rotation) - NPC.Dnpc().vector[0]).Length() / 10;
                }
                NPC.Dnpc().vector[1] = new Vector2(2 * NPC.direction, 0.4F).RotatedBy(NPC.rotation);
                NPC.Dnpc().Times[2] = NPC.Dnpc().vector[1].ToRotation();

                NPC.velocity.Y = 0;
                
				NPC.rotation = NPC.velocity.X * 0.03F;
			}
			else
			{
				Vector2 vector = player.Center - NPC.Center;
				Vector2 vector2 = vector / 10;

				if (vector2.Length() > 40)
				{
					vector2 = vector2.PerfectNormalize() * 40;
				}
				NPC.Dnpc().vector[0] += (vector2 - NPC.Dnpc().vector[0]).PerfectNormalize() * (vector2 - NPC.Dnpc().vector[0]).Length() / 10;

				if (Main.npc[(int)NPC.localAI[0]].active && Main.npc[(int)NPC.localAI[0]].type == ModContent.NPCType<绿岩炮>())
				{
					NPC.Dnpc().vector[1] = NPC.Dnpc().Times[2].ToRotationVector2();
					DDHelper.RotateSpeed(ref NPC.Dnpc().Times[2], (player.Center - Main.npc[(int)NPC.localAI[0]].Center).ToRotation(), 0.03F);
				}
				else
				{
					NPC.Dnpc().vector[1] = NPC.Dnpc().Times[2].ToRotationVector2();
					DDHelper.RotateSpeed(ref NPC.Dnpc().Times[2], (player.Center - (NPC.Center - new Vector2(-1 * NPC.direction * NPC.scale, 64*NPC.scale).RotatedBy(NPC.rotation))).ToRotation(), 0.03F);
                    /*
					if (Main.rand.NextBool(10))
					{
						for (int a = 0; a < 2; a++)
						{
							int dust = NewDust((NPC.Center - new Vector2(-1 * NPC.direction, Main.rand.NextFloat(58, 70)).RotatedBy(NPC.rotation)) + NPC.Dnpc().vector[1].PerfectNormalize() * 40 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
							Main.dust[dust].velocity = NPC.Dnpc().vector[1].PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(5, 12);
							Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
							Main.dust[dust].noGravity = true;
						}
					}*/

                }
                if (vector.Length() < 700)
				{
					NPC.Dnpc().Bool[2] = false;
				}
				if (!NPC.Dnpc().Bool[2])
				{
					DDHelper.RotateSpeed(ref NPC.rotation, NPC.velocity.X * 0.03F, 0.03F);
					NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 4) / 21;
				}
				else
				{
					DDHelper.RotateSpeed(ref NPC.rotation, NPC.velocity.X * 0.08F, 0.03F);
					NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 18) / 21;
				}
				if (vector.Length() > 1000)
				{
					NPC.Dnpc().Bool[2] = true;
				}
				NPC.spriteDirection = 0;
				NPC.direction = -1;
				if (vector.X > 0)
				{
					NPC.spriteDirection = 1;
					NPC.direction = 1;

				}

			}
			if (player.dead)
			{
				NPC.Dnpc().Bool[1] = false;
				NPC.velocity.Y -= 10;
				Vector2 vector = player.Center - NPC.Center;
				if (vector.Length() > 1500)
				{
					NPC.active = false;
				}
			}

			//受到攻击
			if (NPC.Dnpc().Times[1] > 0)
			{
				NPC.Dnpc().Times[1]--;
			}
			//发光
			Lighting.AddLight(NPC.Center, new Color(121, 255, 120).ToVector3() / 3);

			//NPC.Dnpc().Bool[0]浮游模式
			if (!NPC.Dnpc().Bool[1])
            {
                NPC.boss = false;

                NPC.velocity.Y += NPC.Dnpc().Times[0];
                if(TileCollision)
                {
                    DDHelper.BackAndForth(-4, 0, 0.15F, ref NPC.Dnpc().Times[0], ref NPC.Dnpc().Bool[0],false);
                }
                else
                {
                    DDHelper.BackAndForth(0, 2, 0.05F, ref NPC.Dnpc().Times[0], ref NPC.Dnpc().Bool[0], false);
                }
			}
			else
            {
                NPC.boss = true;
                if (!Main.dedServ&& Music!= MusicA) Music = MusicA;
                if (NPC.Dnpc().Stage == 1)
				{
					NPC.ai[0]++;

					if (NPC.ai[0] % 120 == 0)
					{
						if (Main.netMode != 1)
						{
							int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.width/2* sd, 20).RotatedBy(NPC.rotation), new Vector2(0, -Main.rand.NextFloat(6, 12)).RotatedBy(NPC.rotation), ModContent.ProjectileType<Boss绿岩导弹>(), 20, 0, -1, NPC.whoAmI);
                        }
                        for (int a = 0; a < 12; a++)
                        {
                            int dust = NewDust(NPC.Center - new Vector2(NPC.width / 2 * sd, 20).RotatedBy(NPC.rotation) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
                            Main.dust[dust].velocity = new Vector2(0, -1).RotatedBy(NPC.rotation).PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(12, 24);
                            Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                            Main.dust[dust].noGravity = true;
                        }
                    }
					if (NPC.lifeMax / 2 >= NPC.life)
                    {
                        NPC.ai[0] = 0;
                        NPC.Dnpc().Stage = 2;

					}
				}
				else
				if (NPC.Dnpc().Stage == 2)
                {
                    NPC.ai[0]++;
					if (NPC.ai[0]<300)
					{
						if(NPC.ai[0]>120)
						{
                            if (NPC.ai[0] % 10 == 0)
                            {
                                if (Main.netMode != 1)
                                {
                                    int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.width / 2 * sd, 20).RotatedBy(NPC.rotation), new Vector2(0, -Main.rand.NextFloat(6, 12)).RotatedBy(NPC.rotation), ModContent.ProjectileType<Boss绿岩导弹>(), 20, 0, -1, NPC.whoAmI,1);
                                }
                                for (int a = 0; a < 12; a++)
                                {
                                    int dust = NewDust(NPC.Center - new Vector2(NPC.width / 2 * sd, 20).RotatedBy(NPC.rotation) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
                                    Main.dust[dust].velocity = new Vector2(0, -1).RotatedBy(NPC.rotation).PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(12, 24);
                                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                                    Main.dust[dust].noGravity = true;
                                }
                            }
                        }
					}
					else if (NPC.ai[0] < 600)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            int dust = NewDust((NPC.Center - new Vector2(38 * NPC.direction, 88).RotatedBy(NPC.rotation)) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
                            Main.dust[dust].velocity = new Vector2(0, -1).RotatedBy(NPC.rotation).PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(12, 24);
                            Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                            Main.dust[dust].noGravity = true;
                        }
                        if (Main.netMode != 1 && Main.rand.NextBool(6))
                        {
                            int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(38 * NPC.direction, 88), new Vector2(0, -2).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ModContent.ProjectileType<Boss绿岩能量>(), 18, 0, -1, NPC.whoAmI, 1);
                        }
                    }
                    else
					if(NPC.ai[0] < 900)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            int dust = NewDust((NPC.Center - new Vector2(38 * NPC.direction, 88).RotatedBy(NPC.rotation)) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
                            Main.dust[dust].velocity = new Vector2(0, -1).RotatedBy(NPC.rotation).PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(6, 10);
                            Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                            Main.dust[dust].noGravity = true;
                        }
                        Vector2 vector = player.Center - NPC.Center;
                        if (Main.netMode != 1&&Main.rand.NextBool(6))
                        {
                            int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(38 * NPC.direction, 88), new Vector2(2,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<Boss绿岩能量>(), 18, 0, -1, NPC.whoAmI, 1);
                        }
                    }

                    else
					{
						NPC.ai[0] = 0;

                    }
                }
            }
        }
        public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<Biome.绿岩实验室>().ModBiomeBestiaryInfoElement),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩之视")),
            });
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<绿岩之视宝藏箱>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<绿岩之视纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<绿岩之视面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<绿岩之视圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<迷你绿岩之视>(), 4));

            npcLoot.SpecialLoot(ModContent.ItemType<绿岩能源法杖>(), 1);
            npcLoot.SpecialLoot(ModContent.ItemType<绿岩双剑>(),2);
            npcLoot.NormalLoot(1, ModContent.ItemType<绿岩电池>(), 8, 12);
            npcLoot.NormalLoot(1, ModContent.ItemType<绿岩晶石>(), 40, 80);

        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref Worlds.NPCDowned.绿岩之视, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        //1-5生气
        //6-8转换微笑
        //9-14微笑
        //15-18转换生气
        //19挨打
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter++;
			NPC.frame.Width = 208;
            if (NPC.frameCounter >= 5)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = frameHeight * 0;
                NPC.frameCounter = 0;
            }
            DDHelper.BackAndForth(1.5F, 1.75F,0.02F,ref NPC.Dnpc().Times[3],ref NPC.Dnpc().Bool[3]) ;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects sprite = 0;
            Texture2D texture = DDTextures.VoidStar.Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
            
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(-58 * NPC.direction, 0).RotatedBy(NPC.rotation), rectangle, new Color(100, 255, 100, 0), NPC.rotation, new Vector2(texture.Width/2,0), new Vector2(0.25F, NPC.Dnpc().Times[3]) *NPC.scale, 0, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(-58 * NPC.direction, 0).RotatedBy(NPC.rotation), rectangle, new Color(100, 255, 100, 0), NPC.rotation, new Vector2(texture.Width/2,0), new Vector2(0.25F, NPC.Dnpc().Times[3]) *NPC.scale, 0, 0);

            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;

            }

            texture = TextureAssets.Npc[ModContent.NPCType<绿岩炮>()].Value;
            rectangle = new Rectangle(0, 0, texture.Width/2, texture.Height / 2);


            sprite = 0;
            Player player = Main.player[NPC.target];
            if (NPC.spriteDirection == 1)
			{
				sprite = SpriteEffects.FlipHorizontally;

            }
			//炮
            if (Main.npc[(int)NPC.localAI[0]].active && Main.npc[(int)NPC.localAI[0]].type == ModContent.NPCType<绿岩炮>())
            {
                rectangle.X = 0;
                Vector2 vector2 = NPC.Dnpc().vector[1];
                spriteBatch.Draw(texture, Main.npc[(int)NPC.localAI[0]].Center - screenPos, rectangle, drawColor, vector2.ToRotation()+MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0);
                if (Main.npc[(int)NPC.localAI[0]].Dnpc().Times[1] > 0 || NPC.Dnpc().Stage == 2)
                {
                    spriteBatch.Draw(texture, Main.npc[(int)NPC.localAI[0]].Center - screenPos, rectangle, new Color(33, 111, 33, 0), vector2.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0);
                }
                rectangle.Y += texture.Height / 2;
                spriteBatch.Draw(texture, Main.npc[(int)NPC.localAI[0]].Center - screenPos, rectangle, Color.White, vector2.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0);
            }
			else
			{
                Vector2 vector2 = NPC.Dnpc().vector[1];
				rectangle.X += texture.Width/2;
                spriteBatch.Draw(texture, (NPC.Center - new Vector2(-1 * NPC.direction * NPC.scale, 64 * NPC.scale).RotatedBy(NPC.rotation)) - screenPos, rectangle, drawColor, vector2.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0); 
				if (NPC.Dnpc().Times[1] > 0 || NPC.Dnpc().Stage == 2)
                {
                    spriteBatch.Draw(texture, (NPC.Center - new Vector2(-1 * NPC.direction * NPC.scale, 64 * NPC.scale).RotatedBy(NPC.rotation)) - screenPos, rectangle, new Color(33, 111, 33, 0), vector2.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0);
                }
                rectangle.Y += texture.Height / 2;
                spriteBatch.Draw(texture, (NPC.Center - new Vector2(-1 * NPC.direction * NPC.scale, 64 * NPC.scale).RotatedBy(NPC.rotation)) - screenPos, rectangle, Color.White, vector2.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, NPC.scale, sprite, 0);


            }
            //BOSS
            sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;

            }
            texture = TextureAssets.Npc[NPC.type].Value;
            rectangle = NPC.frame;
			spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
            if (NPC.Dnpc().Times[1] > 0 || NPC.Dnpc().Stage == 2)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(33, 111, 33, 0), NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
            }
            
            rectangle.X += 208;
            /*spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, Color.White, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
			if(NPC.Dnpc().Stage == 2)
			{
                spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(33, 111, 33, 0), NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
            }*/
            rectangle.X += 208;
			spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, Color.White, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
			
			if (NPC.Dnpc().Times[1] > 0|| NPC.Dnpc().Stage==2)
			{
				rectangle.X += 208;
				spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, Color.White, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
			}
            //绘制眼球
			texture = asset.Value;
			Vector2 vector = NPC.Dnpc().vector[0];
            rectangle = new Rectangle(0, 0, texture.Width/2, texture.Height / 2);
            if(vector.Length()<20)
            {
                rectangle.X = texture.Width / 2;
            }
            sprite = SpriteEffects.FlipVertically;
            if (NPC.spriteDirection == 1)
            {
                sprite = 0;
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos+ vector, rectangle, Color.White, vector.ToRotation(), rectangle.Size() / 2, NPC.scale* new Vector2(1-vector.Length() / 120, 1), sprite, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos+ vector, rectangle, new Color(100,255,100,0)*0.4F, vector.ToRotation(), rectangle.Size() / 2, NPC.scale* new Vector2(1-vector.Length() / 120, 1), sprite, 0);

            return false;
		}
        public override void HitEffect(HitInfo hit)
        {
            NPC.Dnpc().Times[1] = 6;
            NPC.Dnpc().Bool[1] = true;

			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0,Color.White, 1f);
			}
			for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255),1f);
                Main.dust[dust].velocity = new Vector2(1,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.NextFloat(0,4);
            }

			if (NPC.life <= 0)
			{
				if(Main.netMode!=2)
                {
                    int sd = -1;
                    if (NPC.spriteDirection == 1)
                    {
                        sd = 1;
                    }
                    int GoreType = Mod.Find<ModGore>("绿岩之视1").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position+new Vector2(0,0), new Vector2(0, -4), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩之视2").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position+new Vector2(0,NPC.height / 2), new Vector2(0,0), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩之视4").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position+new Vector2(0,NPC.height), new Vector2(0,4), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩之视5").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.width / 2 * sd, 20).RotatedBy(NPC.rotation), new Vector2(0,4), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩之视3").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -2), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩炮2").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position - new Vector2(-NPC.width / 2, 20), new Vector2(0, -4), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("绿岩炮3").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position - new Vector2(-NPC.width / 2, 20), new Vector2(0, -4), GoreType, NPC.scale);
                }
                for (int i = 0; i < 300; i++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255), 2f);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 12);
                    Main.dust[dust].customData = Main.dust[dust].DustAI(1);
                }
            }
		}
	}
}
