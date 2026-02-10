using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.NPCs
{
    public class DGlobalNPCMonsterAI : GlobalNPC
    {

		public override bool InstancePerEntity => true;
		public static Asset<Texture2D> SkeletronHead;
        public override void Load()
        {
			//SkeletronHead = ModContent.Request<Texture2D>("DDmod/Image/骷髅王");

		}
        public override void ResetEffects(NPC npc)
        {
        }
        public override void ModifyIncomingHit(NPC NPC, ref NPC.HitModifiers modifiers)
		{
			//暗影球
			if (NPC.type == 30)
			{
                modifiers.ModifyHitInfo += Modifiers_ModifyHitInfo;
			    void Modifiers_ModifyHitInfo(ref HitInfo info)
				{
					NPC.scale -= (float)info.Damage / 20;
					if (NPC.scale >= 0.3f)
					{
						info.Damage = 0;
					}
				}
				NPC.life++;
				if (Main.netMode == 1)
                {
                    DDmod.SyncData(DDType.NPCLife, NPC.whoAmI, -1, Main.myPlayer);
                }
				NewDustChange((int)(NPC.scale*50), NPC.position, NPC.Size, 27, 2* NPC.scale, 5* NPC.scale, true, NPC.scale/1.5f);
			}
        }


        public override void AI(NPC npc)
        {

		}
        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return base.CanBeHitByProjectile(npc, projectile);
        }
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            return base.CanBeHitByItem(npc, player, item);
        }
		public override bool PreAI(NPC npc)
        {/*
            if (npc.type == 111)
			{
				if ((npc.Center-Main.player[npc.target].Center).Length()<750)
				{
					if(npc.velocity == Vector2.Zero)
					npc.ai[1] = 91;
				}
			}*/
            //史莱姆
            /*
			if (npc.damage > 0 && npc.aiStyle == 1 && Main.npcFrameCount[npc.type] == 2  && npc.type != ModContent.NPCType<BraveGrassSlime>())
			{
				Main.NewText(npc.velocity.Y);
				if (npc.velocity.Y == 0 && npc.Dnpc().Stage == 1)
				{
					if (Main.netMode == 2)
					{
						DGlobalNPC.SendScale(npc);
					}
				}
				if (npc.Dnpc().Stage ==0&&Main.masterMode)
                {
					npc.Dnpc().Stage = 1;
					npc.scale *= Main.rand.NextFloat(1f, 1.5F);
					npc.NScale(npc.scale,true);
					npc.netUpdate = true;
				}
			}*/
            //哥布林巫师
            if (npc.type == 29 && Main.masterMode)
			{
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				npc.velocity.X *= 0.93f;
				if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
					npc.velocity.X = 0f;

				if (npc.ai[0] == 0f)
					npc.ai[0] = 500f;

				if (npc.ai[2] != 0f && npc.ai[3] != 0f)
				{
					//传送
					npc.position += npc.netOffset;

					PlaySound(SoundID.Item8, npc.position);
					for (int num69 = 0; num69 < 50; num69++)
					{
						Dust dust = Main.dust[NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3))];
						dust.velocity *= 3f;
						if (dust.scale > 1f)
							dust.noGravity = true;

					}

					npc.position -= npc.netOffset;
					npc.position.X = npc.ai[2] * 16f - npc.width / 2 + 8f;
					npc.position.Y = npc.ai[3] * 16f - npc.height;
					npc.netOffset *= 0f;
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					PlaySound(SoundID.Item8, npc.position);
					for (int num78 = 0; num78 < 50; num78++)
					{
						Dust dust = Main.dust[NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3))];
						dust.velocity *= 3f;
						if (dust.scale > 1f)
							dust.noGravity = true;

					}
				}

				//传送和发射计时器
				npc.ai[0] += 1f;

				if (npc.ai[0] == 100f || npc.ai[0] == 200f || npc.ai[0] == 300f)
				{
					npc.ai[1] = 80f;
					PlaySound(SoundID.Item8, npc.position);

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + npc.width / 2, (int)npc.position.Y - 8, 30, 0, npc.whoAmI);
					}
					npc.netUpdate = true;
				}
				//看不懂的re神代码,我是傻逼
				if (npc.ai[0] >= 500f && Main.netMode != 1)
				{
					npc.ai[0] = 1f;
					int num87 = (int)player.position.X / 16;
					int num88 = (int)player.position.Y / 16;
					int num89 = (int)npc.position.X / 16;
					int num90 = (int)npc.position.Y / 16;
					int num91 = 20;
					int num92 = 0;
					bool flag4 = false;
					if (Math.Abs(npc.position.X - player.position.X) + Math.Abs(npc.position.Y - player.position.Y) > 2000f)
					{
						num92 = 100;
						flag4 = true;
					}

					while (!flag4 && num92 < 100)
					{
						num92++;
						int num93 = Main.rand.Next(num87 - num91, num87 + num91);
						int num94 = Main.rand.Next(num88 - num91, num88 + num91);
						for (int num95 = num94; num95 < num88 + num91; num95++)
						{
							if ((num95 < num88 - 4 || num95 > num88 + 4 || num93 < num87 - 4 || num93 > num87 + 4) && (num95 < num90 - 1 || num95 > num90 + 1 || num93 < num89 - 1 || num93 > num89 + 1) && Main.tile[num93, num95].HasTile)
							{
								bool flag5 = true;
								if (Main.tile[num93, num95 - 1].LiquidType == 1)
									flag5 = false;

								if (flag5 && Main.tileSolid[Main.tile[num93, num95].TileType] && !Collision.SolidTiles(num93 - 1, num93 + 1, num95 - 4, num95 - 1))
								{
									npc.ai[1] = 20f;
									npc.ai[2] = num93;
									npc.ai[3] = num95;
									flag4 = true;
									break;
								}
							}
						}
					}

					npc.netUpdate = true;
				}

				if (npc.ai[1] > 0f)
				{
					npc.ai[1] -= 1f;
				}

				npc.position += npc.netOffset;
				if (Main.rand.NextBool(5))
				{
					Dust dust = Main.dust[NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 27, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default(Color), 1.5f)];
					dust.noGravity = true;
					dust.velocity.X *= 0.5f;
					dust.velocity.Y = -2f;
				}

				npc.position -= npc.netOffset;
				return false;
			}
			//暗影球
			if (npc.type == 30 && Main.masterMode)
			{
				if (npc.scale < 0.8f)
				{
					npc.SimpleStrikeNPC(npc.damage * 2, 0,false,0,noPlayerInteraction:true);
				}
				NPCID.Sets.TrailingMode[npc.type] = 0;
				NPCID.Sets.TrailCacheLength[npc.type] = 15;
				npc.extraValue = 0;
				if (Main.npc[(int)npc.ai[0]].active && Main.npc[(int)npc.ai[0]].type == 29 && Main.npc[(int)npc.ai[0]].ai[1] >= 2f && npc.velocity == Vector2.Zero)
				{
					npc.scale += 0.02f;
					npc.position = npc.Center - new Vector2(0, 0.2F);
					npc.width = (int)(16 * npc.scale);
					npc.height = (int)(16 * npc.scale);
					npc.Center = npc.position;
				}
				else
				{
					if (npc.target == 255 || npc.velocity == Vector2.Zero)
					{
						npc.TargetClosest();
						npc.velocity = (Main.player[npc.target].Center - npc.Center).PerfectNormalize() * 6;
					}
				}
				npc.EncourageDespawn(100);
				npc.position += npc.netOffset;

				NewDustChange(1, npc.position, npc.Size, 27, 1, 1, true, npc.scale / 3);

				npc.rotation += 0.4f;
				npc.position -= npc.netOffset;
				return false;
			}
			if (npc.type == 20)
            {
                if (npc.Dnpc().Bool[0])
				{
                    npc.Dnpc().Times[0]++; 
					if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
					{
                        npc.TargetClosest(true);
					}
                    if (npc.Dnpc().Times[0] == 10)
					{
						if (NPC.AnyNPCs(ModContent.NPCType<大地守卫>()))
						{
							npc.position = Main.player[Main.npc[NPC.FindFirstNPC(ModContent.NPCType<大地守卫>())].target].Center - new Vector2(0, 400);
							npc.target = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<大地守卫>())].target;
						}
						else
                        {
                            npc.TargetClosest(true);
                            npc.position = Main.player[npc.target].Center + new Vector2(180 * Main.player[npc.target].direction, 0);

                        }
						for (int a = 0; a < 100; a++)
						{
							Vector2 v = new Vector2(Main.rand.NextFloat(30, 80), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
							int D = NewDust(npc.Center - new Vector2(4) , 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(50, 255, 50, 0), Scale: 5.2F);
							Main.dust[D].velocity = v / 10;
							Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
							Main.dust[D].customData = 2;
							GlobalDust.DustNPCOwner[D] = npc.whoAmI;
						}
						npc.netUpdate = true;
					}
					if (npc.Dnpc().Times[0] > 10)
                    {
						if (npc.Dnpc().Times[0] / 300 > 1)
						{
							if (npc.spriteDirection != -Main.player[npc.target].direction)
							{
								npc.spriteDirection = -Main.player[npc.target].direction;
								npc.position = Main.player[npc.target].Center + new Vector2(180 * Main.player[npc.target].direction, 0);
								for (int a = 0; a < 100; a++)
								{
									Vector2 v = new Vector2(Main.rand.NextFloat(30, 80), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
									int D = NewDust(npc.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(50, 255, 50, 0), Scale: 5.2F);
									Main.dust[D].velocity = v / 10;
									Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
									Main.dust[D].customData = 2;
									GlobalDust.DustNPCOwner[D] = npc.whoAmI;
								}
							}
						}
						else
						{
                            npc.spriteDirection = Main.player[npc.target].direction;
                        }
                        for (int a = 0; a < 2; a++)
                        {
                            int D = NewDust(npc.Center + new Vector2(npc.width / 2 - 40, npc.height -20+npc.velocity.Y), 60, 1, ModContent.DustType<速度粒子>(), newColor: new Color(50, 255, 50, 0), Scale: 1.2F);
                            Main.dust[D].velocity = new Vector2(0, Main.rand.NextFloat(5F, 10));
                            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                            Main.dust[D].customData = 0.25F;
                        }

                        npc.noGravity = true;
						npc.noTileCollide = true;
                        Vector2 vector = (Main.player[npc.target].Center + new Vector2(180 * Main.player[npc.target].direction, 0) - npc.Center);
                        float SP = vector.Length() / 10;
                        npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * SP) / 21;
						if (npc.Dnpc().Times[0] % 300 >= 60 && npc.Dnpc().Times[0] / 300 <= 5)
						{
							npc.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.树妖.Dialogue" + ((int)npc.Dnpc().Times[0] / 300 + 1)));

						}
						if(npc.Dnpc().Times[0] / 300 > 5)
						{
							npc.Dnpc().Times[0] = 0;
							NPCDowned.树妖 = true;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.WorldData);
                            }
                            npc.Dnpc().Bool[0] = false;

						}
                    }
                    return false;
                }
                npc.noGravity = false;
                npc.noTileCollide = false;
            }
			if (npc.type == 368)
			{
				if (CText <= 0)
				{
					CText2 = "";
					if (Main.rand.NextBool(300))
					{
						CText = 300;
						CText2 = Language.GetTextValue("Mods.DDmod.NPCDialogue.旅商.Dialogue1");
						if (Main.rand.NextBool(2)) return true;
						int r = 0;

						for (int a = 0; a < 40; a++)
						{
							if (Main.travelShop[a] != 0)
							{
								r++;
							}
						}
						Item item = new Item(Main.travelShop[Main.rand.Next(r)]);
						string Text = "";
						int Value = item.value;
						int 铂金 = Value / 1000000;
						if (铂金 > 0) Text += 铂金 + Language.GetTextValue("Currency.Platinum");
						int 金 = Value / 10000;
						金 %= 100;
						if (金 > 0) Text += 金 + Language.GetTextValue("Currency.Gold");
						int 银 = Value / 100;
						银 %= 100;
						if (银 > 0) Text += 银 + Language.GetTextValue("Currency.Silver");
						int 铜 = Value;
						铜 %= 100;
						if (铜 > 0) Text += 铜 + Language.GetTextValue("Currency.Copper");

						CText2 = Language.GetTextValue("Mods.DDmod.NPCDialogue.旅商.Dialogue2", new string[] { Text, item.Name });
					}
				}
				npc.NPCText(CText2);
            }
			/*
            if (npc.type == 134|| npc.type == 135||npc.type == 136)
			{
				if(npc.frame.Y==0)
				Lighting.AddLight(npc.Center, new Color(255, 100, 100).ToVector3()*0.5F);
            }*/
            CText--;
			return base.PreAI(npc);
		}
		int CText;
		string CText2;
		float CTextAlpha;
        public override void FindFrame(NPC npc, int frameHeight)
        {
			if(npc.Dnpc().Bool[0]&&npc.type==20)
			{
				npc.frame.Y = 0;
				if(npc.Dnpc().Times[0]%20<10)
				{
					npc.frame.Y = frameHeight * 17;
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
			/*
            modifiers.ModifyHitInfo += Modifiers_ModifyHitInfo;

            void Modifiers_ModifyHitInfo(ref HitInfo info)
            {
				info.Damage = projectile.damage;
            }*/
        }


        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if(npc.type == 29 && npc.life<=0&&npc.SpawnedFromStatue)
            {
				npc.active = false;
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			//暗影球
			if (npc.type == 30 && Main.masterMode)
			{
				SpriteEffects spriteEffects = (SpriteEffects)1;
				if (npc.direction == 1)
				{
					spriteEffects = 0;
				}
				Color color = new Color(81, 6, 233, 0);
				Texture2D texture = DDTextures.VoidStar.Value;
				Vector2 vector = new Vector2(npc.width, npc.height) / 2;
				for (int i = 0; i < npc.oldPos.Length; i++)
				{
					Vector2 vector2 = npc.oldPos[i] + vector - screenPos;
					Color Trailcolor = color * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length / 2f);
					spriteBatch.Draw(texture, vector2, null, Trailcolor, npc.rotation, texture.Size() / 2, npc.scale / 5f * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length), spriteEffects, 0f);
					spriteBatch.Draw(texture, vector2, null, Trailcolor, npc.rotation, texture.Size() / 2, npc.scale / 5f * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length), spriteEffects, 0f);

				}
				spriteBatch.Draw(texture, npc.position + vector - screenPos, null, color, npc.rotation, texture.Size() / 2, npc.scale / 5f, spriteEffects, 0f);
				spriteBatch.Draw(texture, npc.position + vector - screenPos, null, new Color(132, 166, 21, 0) * 0.75F, npc.rotation, texture.Size() / 2, npc.scale / 10, spriteEffects, 0f);

				return false;
            }
			//哥布林术士
			if (npc.type == 471)
			{
				SpriteEffects spriteEffects = (SpriteEffects)1;
				if (npc.direction == -1)
				{
					spriteEffects = 0;
				}
				Color color = new Color(81, 6, 233, 0);
                Texture2D texture = TextureAssets.Npc[npc.type].Value;
                Vector2 vector = new Vector2(npc.width, npc.height-12) / 2;
                spriteBatch.Draw(texture, npc.position + vector - screenPos, npc.frame, new Color(132, 166, 21, 200), npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, npc.position + vector - screenPos, npc.frame, color, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0f);
                for (int i = 0; i < npc.oldPos.Length; i++)
				{
					Vector2 vector2 = npc.oldPos[i] + vector - screenPos;
					Color Trailcolor = color * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length/4);
					spriteBatch.Draw(texture, vector2, npc.frame, Trailcolor, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0f);
					spriteBatch.Draw(texture, vector2, npc.frame, Trailcolor, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0f);
				}

				return false;
            }
            if (npc.Dnpc().Bool[0] && npc.type == 20)
            {
                Texture2D texture = DDTextures.限制框.Value;
                spriteBatch.Draw(texture, npc.position +new Vector2(npc.width/2,npc.height-2) - screenPos, null, new Color(50, 255, 50, 0), 0, new Vector2(texture.Width/2,0), new Vector2(0.05F,0.1F), 0, 0f);
                spriteBatch.Draw(texture, npc.position +new Vector2(npc.width/2,npc.height-2) - screenPos, null, new Color(50, 255, 50, 0), 0, new Vector2(texture.Width/2,0), new Vector2(0.05F,0.1F), 0, 0f);
                spriteBatch.Draw(texture, npc.position +new Vector2(npc.width/2,npc.height-2) - screenPos, null, new Color(50, 255, 50, 0), 0, new Vector2(texture.Width/2,0), new Vector2(0.05F,0.1F), 0, 0f);
                spriteBatch.Draw(texture, npc.position +new Vector2(npc.width/2,npc.height-2) - screenPos, null, new Color(50, 255, 50, 0), 0, new Vector2(texture.Width/2,0), new Vector2(0.05F,1F), 0, 0f);
            }
			/*
            if (npc.type == 134)
            {
                Texture2D texture = TextureAssets.Npc[134].Value;
                spriteBatch.Draw(texture, npc.Center - screenPos, null, drawColor, npc.rotation,texture.Size()/2,npc.scale, 0, 0f);
                texture = TextureAssets.Dest[0].Value;
                if (drawColor.ToVector3().Length() > 0)
                    spriteBatch.Draw(texture, npc.Center - screenPos, null, new Color(255, 255, 255, 0), npc.rotation, texture.Size() / 2, npc.scale, 0, 0f);
                return false;
            }*/
            if (npc.type == 135)
            {

                Texture2D texture = TextureAssets.Npc[135].Value;
                spriteBatch.Draw(texture, npc.Center - screenPos, npc.frame, drawColor, npc.rotation, npc.frame.Size()/2,npc.scale, 0, 0f);
                texture = TextureAssets.Dest[1].Value;
				if(drawColor.ToVector3().Length()>0)
                spriteBatch.Draw(texture, npc.Center - screenPos,npc.frame, new Color(255,255,255,0), npc.rotation, npc.frame.Size() / 2, npc.scale, 0, 0f);
                return false;
            }
            if (npc.type == 136)
            {
                Texture2D texture = TextureAssets.Npc[136].Value;
                spriteBatch.Draw(texture, npc.Center - screenPos, null, drawColor, npc.rotation,texture.Size()/2,npc.scale, 0, 0f);
                texture = TextureAssets.Dest[2].Value;
                if (drawColor.ToVector3().Length() > 0)
                    spriteBatch.Draw(texture, npc.Center - screenPos, null, new Color(255, 255, 255, 0), npc.rotation, texture.Size() / 2, npc.scale, 0, 0f);
                return false;
            }
            //骷髅王
            if (npc.type == NPCID.SkeletronHead&&Main.worldName=="牢大")
            {
                SpriteEffects spriteEffects = 0;
                if (npc.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture = TextureAssets.Npc[35].Value;
                Vector2 vector = npc.Size/2;
                spriteBatch.Draw(texture, npc.position + vector - screenPos+new Vector2(0,100), null, drawColor, npc.rotation, texture.Size() / 2, npc.scale, spriteEffects, 0f);

				return false;
			}

			return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
		}
		public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
		{
			//暗影球
			if (npc.type == 30)
			{
                modifiers.ModifyHurtInfo += Modifiers_ModifyHurtInfo;

				void Modifiers_ModifyHurtInfo(ref Player.HurtInfo info)
				{
					info.Damage = (int)((info.Damage / 2) + (info.Damage * npc.scale / 2));
				}
			}
		}
    }
}