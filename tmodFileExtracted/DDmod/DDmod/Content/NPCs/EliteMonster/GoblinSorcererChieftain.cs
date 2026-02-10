using DDmod.Content.Dusts;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Projectiles.Summon;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using Terraria.ID;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Worlds;
using Terraria.GameContent.Achievements;
using Terraria.Chat;
using DDmod.Content.Items.Boss.MiniBoss;

namespace DDmod.Content.NPCs.EliteMonster
{
	[AutoloadBossHead]
	public class GoblinSorcererChieftain : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 7;

			NPCID.Sets.BossBestiaryPriority.Add(Type);
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 2050;
			NPC.damage = 20;
			NPC.defense = 2;
			NPC.knockBackResist = 0f;
			NPC.width = 28;
			NPC.height = 52;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.boss = true;
			if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
			NPC.npcSlots = 80f;
			NPC.lavaImmune = true;
			NPC.scale = 1f;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath39;
			NPC.netAlways = true;
			NPC.NPCHB().MiniBoss = true;
			NPC.Dnpc().Stage = -1;
			NPC.Dnpc().Properties.ShadowFire = true;
			NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.1f;
        }
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
		}
		public override bool? CanFallThroughPlatforms()
		{
			return NPC.noGravity;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				Biomes.Surface,
				Invasions.Goblins,
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.GoblinSorcererChieftain"))
			});
		}
		public override void OnKill()
		{
			SetEventFlagCleared(ref NPCDowned.downedGoblinSorcererChieftain, -1);

			SetEventFlagCleared(ref NPC.downedGoblins, 0);
			if (Main.netMode == 2)
				NetMessage.SendData(7);

			AchievementsHelper.NotifyProgressionEvent(10);

			LocalizedText empty = Lang.misc[0];
			if (Main.netMode == 0)
				Main.NewText(empty.ToString(), 175, 75);
			else if (Main.netMode == 2 && empty.Value != "")
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(empty.Key), new Color(175, 75, 255));

			Main.invasionType = 0;
			Main.invasionDelay = 0;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<ShadowFlame>()));
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<ShadowNecklaceItem>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<哥布林巫师首领圣物>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<哥布林巫师首领面具>(), 10));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<哥布林巫师首领纪念章物品>(), 10));
		}
		public override void ReceiveExtraAI(BinaryReader binaryReader)
		{
			Genki = binaryReader.ReadInt32();
		}
		public override void SendExtraAI(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Genki);
		}
		public int Genki;
		public override void AI()
		{
			NPC.TargetClosest();
			NPC GenkiBomb = null;
			if (Genki >= 0)
			{
				GenkiBomb = Main.npc[Genki];
			}
			Player player = Main.player[NPC.target];
			if (!player.dead)
			{
				NPC.timeLeft = 5;
			}
			if (player.dead)
			{
				ObtainPosition(player);
				if (NPC.Dnpc().Stage != -1)
				{
					if (Main.netMode == 2)
						NetMessage.SendData(7);

					AchievementsHelper.NotifyProgressionEvent(10);

					LocalizedText empty = Language.GetText("Mods.DDmod.WorldTips.Goblin3");
					if (Main.netMode == 0)
						Main.NewText(empty.Value, 175, 75);
					else if (Main.netMode == 2 && empty.Value != "")
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(empty.Key), new Color(175, 75, 255));

					Main.invasionType = 0;
					Main.invasionDelay = 0;
					NPC.active = false;
				}
			}
			NPC.velocity.X *= 0.93f;
			if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
				NPC.velocity.X = 0f;

			Teleport();
			if ((float)NPC.life / NPC.lifeMax <= 0.5f && NPC.Dnpc().Stage == 0)
			{
				NPC.Dnpc().Stage = 1;
				for (int A = 0; A < 300; A++)
				{
					Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 20, NPC.position.Y), NPC.width + 40, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3))];
					dust.velocity *= 3f;
					if (dust.scale > 1f)
						dust.noGravity = true;
				}
				NPC.netUpdate = true;
			}
			if (NPC.Dnpc().Bool[4] && NPC.Dnpc().Stage == 1)
			{
				NPC.Dnpc().Stage = 2;
				NPC.netUpdate = true;
			}
			if (NPC.Dnpc().Stage == -1)
			{
				Teleport();
				return;
			}
			//亡语
			if (NPC.Dnpc().Stage != 2)
			{
				//传送和发射计时器
				NPC.ai[0] += 1f;
				if (NPC.ai[1] == 0)
				{
					if (NPC.ai[0] == 60)
					{
						PlaySound(SoundID.Item8, NPC.position);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Genki = NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X + NPC.width / 1.5f * NPC.direction), (int)NPC.Center.Y, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, NPC.Dnpc().Stage);
							Main.npc[Genki].localAI[0] = NPC.whoAmI;
							Main.npc[Genki].netUpdate = true;
						}
						NPC.netUpdate = true;
					}
					if (NPC.ai[0] >= 400f && Main.netMode != 1)
					{
						NPC.ai[0] = 1f;
						NPC.ai[1]++;
						ObtainPosition(player);
					}
					if (Main.npc[Genki].active && !GenkiBomb.Dnpc().Bool[0])
					{
						Main.npc[Genki].position -= new Vector2(0.2f * NPC.direction, 0.3f);
						if (Main.npc[Genki].Dnpc().Bool[1])
						{
							Main.npc[Genki].position.X -= 2f * NPC.direction;
						}
					}
				}
				else if (NPC.ai[1] == 1)
				{
					if (NPC.ai[0] > 60 && NPC.ai[0] < 120)
					{
						for (int a = 0; a < 10; a++)
						{
							Dust dust = Main.dust[NewDust(new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) + new Vector2(Main.rand.NextFloat(40, 80)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
							dust.noGravity = true;
							dust.scale = 1.7F;
							dust.velocity = (new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) - dust.position) / 7.5F;
						}
						if (NPC.ai[0] % 10 == 0)
						{
							PlaySound(SoundID.Item8, NPC.position);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (NPC.Dnpc().Stage == 0)
								{
									NPC npc = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, 2)];
									npc.Dnpc().vector[0] = (player.Center - npc.Center).PerfectNormalize() * 12;
									npc.scale = 2;
									npc.netUpdate = true;
								}
								else
								{
									for (int A = -1; A <= 1; A++)
									{
										NPC npc = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, 2)];
										npc.Dnpc().vector[0] = (player.Center - npc.Center).PerfectNormalize().RotatedBy(-0.2F * A) * 12;
										npc.scale = 2;
										npc.netUpdate = true;
									}
								}
							}
							NPC.netUpdate = true;
						}
					}
					if (NPC.ai[0] > 200)
					{
						NPC.ai[0] = 1f;
						NPC.ai[2]++;
						ObtainPosition(player);
					}
					if (NPC.ai[2] > 3)
					{
						NPC.ai[1]++;
						NPC.ai[2] = 0;
					}
				}
				else if (NPC.ai[1] == 2)
				{
					if (NPC.ai[0] > 60 && NPC.ai[0] <= 480)
					{
						for (int a = 0; a < 10; a++)
						{
							Dust dust = Main.dust[NewDust(new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) + new Vector2(Main.rand.NextFloat(40, 80)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
							dust.noGravity = true;
							dust.scale = 1.2F;
							dust.velocity = (new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) - dust.position) / 7.5F;
						}
						if (NPC.Dnpc().Stage == 0)
						{
							if (NPC.ai[0] % 50 == 0)
							{
								PlaySound(SoundID.Item8, NPC.position);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									for (int A = -2; A <= 2; A++)
									{
										NPC npc = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, 3)];
										npc.Dnpc().vector[0] = new Vector2(0, -12).RotatedBy(A * 0.2f);
										npc.scale = 2.5F;
										npc.netUpdate = true;
									}
								}
								NPC.netUpdate = true;
							}
							if (NPC.ai[0] % 50 == 10)
							{
								ObtainPosition(player);
							}
						}
						else
						{
							if (NPC.ai[0] % 25 == 0)
							{
								PlaySound(SoundID.Item8, NPC.position);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									NPC npc = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, 4)];
									npc.Dnpc().vector[0] = new Vector2(0, -8);
									npc.scale = 2.5F;
									npc.netUpdate = true;
								}
								NPC.netUpdate = true;
							}
							if (NPC.ai[0] % 25 == 5)
							{
								ObtainPosition(player);
							}
						}
					}
					if (NPC.ai[0] > 600)
					{
						NPC.ai[0] = 1f;
						NPC.ai[1]++;
						NPC.ai[2] = 0;
					}
				}
				else
				{
					if (NPC.ai[0] > 60 && NPC.ai[0] <= 150)
					{
						for (int a = 0; a < 10; a++)
						{
							Dust dust = Main.dust[NewDust(new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) + new Vector2(Main.rand.NextFloat(40, 80)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
							dust.noGravity = true;
							dust.scale = 1.8F;
							dust.velocity = (new Vector2((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16) - dust.position) / 7.5F;
						}
						if (NPC.ai[0] == 150)
						{
							PlaySound(SoundID.Item8, NPC.position);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								for (int a = -2; a <= 2; a++)
								{
									if (a != 0)
									{
										if (NPC.Dnpc().Stage == 0)
										{
											if (a == 1 || a == -1)
											{
												Projectile proj = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y - 16), new Vector2(0, -15).RotatedBy(-0.2f * a), ModContent.ProjectileType<ShadowFireball>(), 30, 0, Main.myPlayer)];
												proj.hostile = true;
												proj.scale = 0.5f;
												proj.friendly = false;
												proj.netUpdate = true;
											}
										}
										else
										{
											Projectile proj = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y - 16), new Vector2(0, -15).RotatedBy(-0.2f * a), ModContent.ProjectileType<ShadowFireball>(), 30, 0, Main.myPlayer)];
											proj.hostile = true;
											proj.scale = 0.5f;
											proj.friendly = false;
											proj.netUpdate = true;
										}
									}
								}
							}
							NPC.netUpdate = true;
						}
					}
					if (NPC.ai[0] > 600)
					{
						NPC.ai[0] = 1f;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						ObtainPosition(player);
					}
				}
			}
			else
			{
				NPC.ai[1]++;
				if (GenkiBomb != null && GenkiBomb.active && GenkiBomb.type == ModContent.NPCType<ChaosBall>() && GenkiBomb.ai[1] <= 1)
				{
					GenkiBomb.StrikeInstantKill();
				}
				if (GenkiBomb != null || !GenkiBomb.active || GenkiBomb.Dnpc().Bool[0] || GenkiBomb.type != ModContent.NPCType<ChaosBall>())
				{
					if (NPC.ai[1] < 500 && GenkiBomb.ai[1] == 5)
					{
						//NPC.ai[1] = 500;
					}
				}
				if (NPC.ai[1] > 600)
				{
					NPC.ai[0] -= 0.3f;
					if (NPC.ai[0] > 0)
					{
						if (NPC.ai[0] < 56)
						{
							for (int A = 0; A < 2; A++)
							{
								Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 10, NPC.position.Y + 52 - NPC.ai[0]), NPC.width + 20, 2, 27, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), Main.rand.NextFloat(0.6f, 1f))];
								dust.noGravity = true;
								dust.velocity.X *= 0.5f;
								dust.velocity.Y = -8f;
								dust.velocity = dust.velocity.RotatedBy(Main.rand.NextFloat(-1f, 1f));
							}
						}
					}
					else
					{
						for (int A = 0; A < 300; A++)
						{
							Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 20, NPC.position.Y), NPC.width + 40, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.8f, 1.2f))];
							dust.velocity *= 3f;
							if (dust.scale > 1f)
								dust.noGravity = true;
						}
						NPC.active = false;
					}
				}
				else
				{
					if (NPC.ai[1] == 60)
					{
						PlaySound(SoundID.Item8, NPC.position);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							GenkiBomb = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y - 16, ModContent.NPCType<ChaosBall>(), 0, NPC.whoAmI, 5)];
							GenkiBomb.localAI[0] = NPC.whoAmI;
							GenkiBomb.netUpdate = true;
						}
						NPC.netUpdate = true;
					}
				}
			}

			NPC.position += NPC.netOffset;

			if (Main.rand.NextBool(2) && NPC.Dnpc().Stage != 2)
			{
				Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 20, NPC.position.Y + 2), NPC.width + 40, NPC.height, 27, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 2f)];
				dust.noGravity = true;
				dust.velocity.X *= 0.5f;
				dust.velocity.Y = -8f;
			}

			NPC.position -= NPC.netOffset;

		}
		//获取传送位置
		public void ObtainPosition(Player player)
		{
			if (NPC.target == -1 || Main.player[NPC.target].dead)
			{

				NPC.Dnpc().vector[0] = new Vector2(1, 1);
				NPC.netUpdate = true;
				return;
			}
			//看不懂的re神代码,我是傻逼
			int num87 = (int)player.position.X / 16;
			int num88 = (int)player.position.Y / 16;
			int num89 = (int)NPC.position.X / 16;
			int num90 = (int)NPC.position.Y / 16;
			int num91 = 40;
			int num92 = 0;
			bool flag4 = false;
			/*
			if (Math.Abs(NPC.position.X - player.position.X) + Math.Abs(NPC.position.Y - player.position.Y) > 2000f)
			{
				num92 = 100;
				flag4 = true;
			}*/
			while (!flag4 && num92 < 100)
			{
				num92++;
				int num93 = Main.rand.Next(num87 - num91, num87 + num91);
				int num94 = Main.rand.Next(num88 - num91, num88 + num91);
				if (num93 - num87 > -20 && num93 - num87 / 16 <= 0)
				{
					num93 = num87 - 20;
				}
				if (num93 + num87 < 20 && num93 + num87 / 16 >= 0)
				{
					num93 = num87 + 20;
				}
				for (int num95 = num94; num95 < num88 + num91; num95++)
				{
					if ((num95 < num88 - 4 || num95 > num88 + 4 || num93 < num87 - 4 || num93 > num87 + 4) && (num95 < num90 - 1 || num95 > num90 + 1 || num93 < num89 - 1 || num93 > num89 + 1) && Main.tile[num93, num95].HasTile)
					{
						bool flag5 = true;
						if (Main.tile[num93, num95 - 1].LiquidType == 1)
							flag5 = false;

						if (flag5 && Main.tileSolid[Main.tile[num93, num95].TileType] && !Collision.SolidTiles(num93 - 1, num93 + 1, num95 - 4, num95 - 1))
						{
							NPC.Dnpc().vector[0] = new Vector2(num93, num95);
							flag4 = true;
							break;
						}
					}
				}
			}
			NPC.netUpdate = true;
		}
		//传送
		public void Teleport()
		{
			if ((NPC.Dnpc().vector[0].X != 0f && NPC.Dnpc().vector[0].Y != 0f) || NPC.Dnpc().Stage == -1)
			{
				if (NPC.Dnpc().Stage == -1)
				{
					ObtainPosition(Main.player[NPC.target]);
					if (NPC.Dnpc().vector[0].X == 0 || NPC.Dnpc().vector[0].Y == 0)
					{
						return;
					}

					NPC.Dnpc().Stage = 0;
					NPC.netUpdate = true;
				}


				//传送
				NPC.position += NPC.netOffset;

				PlaySound(SoundID.Item8, NPC.position);
				for (int num69 = 0; num69 < 100; num69++)
				{
					Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 20, NPC.position.Y), NPC.width + 40, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3))];
					dust.velocity *= 3f;
					if (dust.scale > 1f)
						dust.noGravity = true;
				}

				NPC.position -= NPC.netOffset;
				NPC.position.X = NPC.Dnpc().vector[0].X * 16f - NPC.width / 2 + 8f;
				NPC.position.Y = NPC.Dnpc().vector[0].Y * 16f - NPC.height;
				NPC.netOffset *= 0f;
				NPC.velocity.X = 0f;
				NPC.velocity.Y = 0f;
				NPC.Dnpc().vector[0].X = 0f;
				NPC.Dnpc().vector[0].Y = 0f;
				PlaySound(SoundID.Item8, NPC.position);
				for (int num78 = 0; num78 < 100; num78++)
				{
					Dust dust = Main.dust[NewDust(new Vector2(NPC.position.X - 20, NPC.position.Y), NPC.width + 40, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3))];
					dust.velocity *= 3f;
					if (dust.scale > 1f)
						dust.noGravity = true;

				}
			}
		}
		public override void HitEffect(HitInfo hit)
		{
			if (NPC.Dnpc().Stage == 0)
			{
				for (int a = 0; a < 10; a++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0, 0, 0, default, 1.2F);
				}
			}
			else
			{
				for (int a = 0; a < 10; a++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 27, 0, 0, 0, default, 1.2F);
				}
			}
			if (NPC.life <= 0&& !NPC.Dnpc().Bool[4])
			{
				NPC.life = 1;
				NPC.Dnpc().Bool[4] = true;
				NPC.ai[0] = 52;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				NPC.NPCLoot();
				NPC.dontTakeDamage = true;

			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector = NPC.Size / 2;
			vector.Y += 2;
			SpriteEffects spriteEffects = 0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}

			if (NPC.Dnpc().Stage <= 0)
			{
				spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7) / 2, NPC.scale, spriteEffects, 0f);
			}
			else if (NPC.Dnpc().Stage == 1)
			{
				spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7) / 2, NPC.scale, spriteEffects, 0f);
				for (int a = 0; a < 2; a++)
				{
					spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), new Color(81, 6, 233, 0), NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7) / 2, NPC.scale, spriteEffects, 0f);
				}
			}
			else
			{
				for (int a = 0; a < 5; a++)
				{
					spriteBatch.Draw(texture, NPC.position - screenPos + vector - new Vector2(0, (int)NPC.ai[0] - TextureAssets.Npc[NPC.type].Height() / 7), new Rectangle?(NPC.frame), new Color(81, 6, 233, 0), NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7) / 2, NPC.scale, spriteEffects, 0f);
				}
			}
			return false;
		}
		public override void FindFrame(int frameHeight)
		{
			
			int Fr = 0;
			NPC GenkiBomb = null;
			if(NPC.localAI[0]<0)
			{
				NPC.localAI[0] = 0;

            }
			if (Genki >= 0)
			{
				GenkiBomb = Main.npc[Genki];
			}
			if (NPC.ai[1] == 0)
			{
				if (GenkiBomb == null || !GenkiBomb.active || GenkiBomb.Dnpc().Bool[0] || GenkiBomb.type != ModContent.NPCType<ChaosBall>())
				{
					NPC.localAI[0] = 0;
					Fr = 0;
				}
				else
				{
					if (GenkiBomb.scale <= 10)
					{
						Fr = (int)NPC.localAI[0]++ / 5;
						if (Fr > 6)
						{
							NPC.localAI[0] = 30;
							Fr = 6;
						}
					}
					else if (NPC.ai[0] <= 400)
					{
						Fr = (int)NPC.localAI[0]++ / 5;
						if (Fr > 13)
						{
							Fr = 13;
						}
					}
					else
					{
						NPC.localAI[0] = 0;
						Fr = 0;

					}
				}
			}
			else if (NPC.ai[1] == 1)
			{
				if (NPC.ai[0] >= 40 && NPC.ai[0] < 120)
				{
					Fr = (int)NPC.localAI[0]++ / 5;
					if (Fr > 6)
					{
						Fr = 6;
					}
				}
				else
				{
					if (NPC.localAI[0] > 30)
					{
						NPC.localAI[0] = 30;
					}
					Fr = (int)NPC.localAI[0]-- / 5;
					if (Fr < 0)
					{
						Fr = 0;
					}
				}

			}
			else if (NPC.ai[1] == 2)
			{
				if (NPC.ai[0] > 60 && NPC.ai[0] <= 480)
				{
					Fr = (int)NPC.localAI[0]++ / 5;
					if (Fr > 6)
					{
						Fr = 6;
					}
				}
				else
				{
					if (NPC.localAI[0] > 30)
					{
						NPC.localAI[0] = 30;

					}
					Fr = (int)NPC.localAI[0]-- / 5;
					if (Fr < 0)
					{
						Fr = 0;
					}
				}
			}
			else
			{
				if (NPC.ai[0] > 60 && NPC.ai[0] <= 150)
				{
					Fr = (int)NPC.localAI[0]++ / 5;
					if (Fr > 6)
					{
						Fr = 6;
					}
				}
				else
				{
					if (NPC.localAI[0] > 30)
					{
						NPC.localAI[0] = 30;

					}
					Fr = (int)NPC.localAI[0]-- / 5;
					if (Fr < 0)
					{
						Fr = 0;
					}
				}
			}
			NPC.frame.Width = 48;
			NPC.frame.X = NPC.frame.Width*(int)(Fr/7);
			NPC.frame.Y = (Fr % 7) * NPC.frame.Height;

            if (NPC.Dnpc().Stage == 2)
            {
                if (GenkiBomb != null && GenkiBomb.active && !GenkiBomb.Dnpc().Bool[0] && GenkiBomb.type == ModContent.NPCType<ChaosBall>())
                {
                    NPC.frame.Y = frameHeight * 6;
                    NPC.frame.X = 0;
                }
				else
                {
                    NPC.frame.Y = 0;
                    NPC.frame.X = 0;
                }
                NPC.frame = new Rectangle(0, frameHeight * 6 - (int)NPC.ai[0], NPC.frame.Width, (int)NPC.ai[0]);
            }
        }
	}
}