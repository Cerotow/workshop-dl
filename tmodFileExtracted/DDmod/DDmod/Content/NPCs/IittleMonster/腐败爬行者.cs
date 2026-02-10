using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Tiles.农场;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class 腐败爬行者 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 80;
            NPC.damage = 35;
            NPC.defense = 4;
            NPC.knockBackResist = 1.4f;
            NPC.width = 40;
            NPC.height = 64;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            Banner = NPC.type;
            NPC.behindTiles = true;
            BannerItem = ModContent.ItemType<腐败爬行者旗>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            Player player = Main.player[NPC.target];
            return (player.position.Y + player.height) - (NPC.position.Y + NPC.height) > 0;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return !NPC.noTileCollide;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int I = 0; I < 10; I++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 18, hit.HitDirection, -2f, NPC.alpha, NPC.color, NPC.scale);
            }
            if (NPC.life <= 0)
            {
                for (int I = 0; I < 50; I++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 18, hit.HitDirection, -2f, NPC.alpha, NPC.color, NPC.scale);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("腐败爬行者1").Type;
                    int GoreType2 = Mod.Find<ModGore>("腐败爬行者2").Type;
                    int GoreType3 = Mod.Find<ModGore>("腐败爬行者3").Type;
                    int GoreType4 = Mod.Find<ModGore>("腐败爬行者4").Type;
                    int GoreType5 = Mod.Find<ModGore>("腐败爬行者5").Type;

                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -4), GoreType, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(4, 0), GoreType2, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-4, 0), GoreType3, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-4, 0), GoreType4, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(4, 0), GoreType5, 1f);
                }
            }
        }
        public int Height => 64;
        public int OldPY = 0;
        public override bool PreAI()
        {
            NPC.knockBackResist = 1.4f;
            NPC.noTileCollide = false;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (NPC.ai[0] != 2)
            {
                if ((player.Center - NPC.Center).Length() < 150)
                {
                    NPC.ai[0] = 1;
                    NPC.ai[2] = 0;
                }
                if ((player.Center - NPC.Center).Length() > 800)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[2] = 0;
                }
            }
            if ((NPC.position.Y + NPC.height) - (player.position.Y + player.height) < 0 && Math.Abs(NPC.Center.X - player.Center.X) < 20)
            {
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
            }

            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y + NPC.height), NPC.width, 2,true);
            //已经被掩埋
            bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X+NPC.width/4, NPC.position.Y + NPC.height/4), NPC.width/2, NPC.height/2);
            //检测左右撞墙
            bool TileCollision3 = Collision.SolidCollision(new Vector2(NPC.position.X-1, NPC.position.Y), NPC.width+2, NPC.height);
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
                NPC.velocity.Y = 0;
            }
            if (NPC.velocity.Y != 0)
            {
                NPC.velocity.X *= 0.96F;
            }
            if (TileCollision2 && (player.Center - NPC.Center).Length() <= 100)
            {
                NPC.ai[0] = 2;
                NPC.ai[2] = 0;
            }
            if (NPC.ai[0] == 0)
            {
                //挖洞潜伏和寻找能挖洞的区域
                bool Dig = false;
                //寻找合适的地形
                bool LookforTileCollision = Collision.SolidCollision(new Vector2(NPC.position.X+NPC.width+2, NPC.position.Y+NPC.height),4,4);
                bool LookforTileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X-1, NPC.position.Y+ NPC.height + 1),1,1 );
                //假如头部被埋了就停止挖掘
                bool TopTileCollision = Collision.SolidCollision(new Vector2(NPC.position.X+NPC.width/2, NPC.position.Y), NPC.width, 16);
                //底部没有挖掘空间也停止
                bool UnderTileCollision = Collision.SolidCollision(new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y+NPC.height), 1, 2);
                bool UnderTileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y+NPC.height+16), 1, 2);
                bool UnderTileCollision3 = Collision.SolidCollision(new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y+NPC.height+32), 1, 2);
                if (LookforTileCollision&& LookforTileCollision2&& !TopTileCollision && NPC.velocity.Y >= 0)
                {
                    Dig = true;
                    if(!TileCollision2&& (!UnderTileCollision|| !UnderTileCollision2|| !UnderTileCollision3))
                    {
                        Dig = false;
                    }
                }
                if((NPC.position.Y + NPC.height) - (player.position.Y + player.height)<0&& Math.Abs(NPC.Center.X-player.Center.X)<20&&NPC.velocity.Y>=0)
                {
                    Dig = true;
                }
                if (Dig)
                {
                    NPC.knockBackResist = 0f;
                    NPC.velocity.X = 0;
                    NPC.noTileCollide = true;
                    NPC.frameCounter++;
                    if (NPC.frameCounter < 5)
                    {
                        NPC.frame.Y = 0;
                    }
                    else if (NPC.frameCounter < 10)
                    {
                        NPC.frame.Y = Height;
                    }
                    else
                    {
                        NPC.frameCounter = 0;
                    }
                    if (NPC.velocity.Y == 0 && !TileCollision2)
                    {
                        NPC.spriteDirection = 0;
                        if (player.Center.X - NPC.Center.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                    }
                    NPC.position.Y += 1;
                }
                if (!Dig)
                {
                    if (!TileCollision2)
                    {
                        NPC.noTileCollide = false;
                        if (NPC.ai[1] == 0)
                        {
                            NPC.ai[1] = Main.rand.NextBool(2) ? 1 : -1;
                            NPC.netUpdate = true;
                        }
                        if (NPC.localAI[0] ==0&& Main.rand.NextBool(100))
                        {
                            NPC.ai[1] = Main.rand.NextBool(2) ? 1 : -1;
                            NPC.netUpdate = true;
                        }
                        NPC.frameCounter+= Math.Abs(NPC.velocity.X);
                        if (NPC.frameCounter >= 12)
                        {
                            NPC.frame.Y += Height;
                            if (NPC.frame.Y >= Height * 5)
                            {
                                NPC.frame.Y = 0;
                            }
                            NPC.frameCounter -= 12;
                        }
                        if (NPC.velocity.Y != 0)
                        {
                            NPC.frame.Y = Height * 2;
                        }

                        //行动
                        if (NPC.velocity.Y == 0 && TileCollision3 && NPC.velocity.X == 0)
                        {
                            if (NPC.localAI[0] < 3)
                            {
                                NPC.velocity.Y = -3+-3* NPC.localAI[0];
                                NPC.velocity.X = 1 * NPC.ai[1];
                                NPC.localAI[0]++;
                            }
                            else
                            {
                                NPC.ai[1] *=  -1;
                                NPC.localAI[0] = 0;
                            }
                        }
                        if (TileCollision3 && NPC.velocity.X == 0)
                        {
                            NPC.velocity.X = 1 * NPC.ai[1];
                        }
                        if(Math.Abs(NPC.velocity.X)>1.5F)
                        {
                            NPC.localAI[0] = 0;
                        }
                        if (NPC.velocity.Y == 0)
                        {
                            if (NPC.ai[1] > 0)
                            {
                                if (NPC.velocity.X < 4)
                                {
                                    NPC.velocity.X += 0.17F;
                                }
                            }
                            else
                            {
                                if (NPC.velocity.X > -4)
                                {
                                    NPC.velocity.X -= 0.17F;
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.knockBackResist = 0f;
                        NPC.frame.Y = 0;
                        NPC.noTileCollide = true;

                    }
                    if (NPC.velocity.Y == 0&& !TileCollision2)
                    {
                        NPC.spriteDirection = 0;
                        if (NPC.velocity.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                    }
                }

            }
            //逃跑
            else if (NPC.ai[0] == 1)
            {
                NPC.ai[2]++;
                if(player.Center.X - NPC.Center.X<0)
                {
                    NPC.ai[1] = -1;
                }
                else
                {
                    NPC.ai[1] = 1;
                }
                if (NPC.velocity.Y == 0 && !TileCollision2)
                {
                    NPC.spriteDirection = 0;
                    if (player.Center.X - NPC.Center.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                }
                if (NPC.ai[2]>300)
                {
                    NPC.ai[1] *= -1;
                    if (NPC.velocity.Y == 0 && !TileCollision2)
                    {
                        NPC.spriteDirection = 0;
                        if (NPC.velocity.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                    }
                }
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter >= 12)
                {
                    NPC.frame.Y += Height;
                    if (NPC.frame.Y >= Height * 5)
                    {
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter -= 12;
                }
                if (NPC.velocity.Y != 0)
                {
                    NPC.frame.Y = Height * 2;
                }

                //行动
                if (NPC.velocity.Y == 0 && TileCollision3 && NPC.velocity.X == 0)
                {
                    if (NPC.localAI[0] < 3)
                    {
                        NPC.velocity.Y = -3 + -3 * NPC.localAI[0];
                        NPC.velocity.X = 1 * NPC.ai[1];
                        NPC.localAI[0]++;
                    }
                    else
                    {
                        NPC.ai[1] *= -1;
                        NPC.localAI[0] = 0;
                    }
                }
                if (TileCollision3 && NPC.velocity.X == 0)
                {
                    NPC.velocity.X = 1 * NPC.ai[1];
                }
                if (Math.Abs(NPC.velocity.X) > 1.5F)
                {
                    NPC.localAI[0] = 0;
                }
                if (NPC.velocity.Y == 0)
                {
                    if (NPC.ai[1] > 0)
                    {
                        if (NPC.velocity.X < 4)
                        {
                            NPC.velocity.X += 0.17F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X > -4)
                        {
                            NPC.velocity.X -= 0.17F;
                        }
                    }

                    if (NPC.ai[2] <= 300)
                    {
                        if (NPC.Center.Y - player.Center.Y > 150)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[2] = 0;
                            return base.PreAI();
                        }
                        if (Math.Abs(NPC.Center.X - player.Center.X)<(Math.Abs(NPC.velocity.X)*10+10) &&NPC.Center.Y-player.Center.Y > NPC.height/2&& NPC.Center.Y - player.Center.Y < 200)
                        {
                            NPC.velocity.Y = -8;
                        }
                    }
                }
            }
            else
            {
                NPC.noTileCollide = true;
                NPC.velocity.X = 0;
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 0;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = Height;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
                NPC.position.Y -= 1;
                if(!TileCollision && !TileCollision2)
                {
                    NPC.ai[0] = 1;
                }
            }
            return base.PreAI();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.腐败爬行者"))

            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.CompleteModeLoot(68, 3, 3, 3);
        }
        public override void OnHitByProjectile(Projectile projectile, HitInfo hit, int damageDone)
        {
            NPC.ai[0] = 1;
            NPC.netUpdate = true;
        }
        public override void OnHitByItem(Player player, Item item, HitInfo hit, int damageDone)
        {
            NPC.ai[0] = 1;
            NPC.netUpdate = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            int[] TileArray = { 22,23 };

            return TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType)
               && !NPC.AnyNPCs(NPCID.LunarTowerVortex)
               && !NPC.AnyNPCs(NPCID.LunarTowerStardust)
               && !NPC.AnyNPCs(NPCID.LunarTowerNebula)
               && !NPC.AnyNPCs(NPCID.LunarTowerSolar) && Main.invasionType == 0
               ? 0.2f : 0f;
        }
        float R;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame;
            frame = NPC.frame;
            if (NPC.IsABestiaryIconDummy)
            {
                spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width / 2, NPC.height + 2) - screenPos, frame, Color.White, NPC.rotation, new Vector2(frame.Width / 2, frame.Height - 2), NPC.scale, sprite, 0);
                return false;
            }
            spriteBatch.Draw(texture, NPC.position+new Vector2(NPC.width/2,NPC.height+2) - screenPos, frame, drawColor, NPC.rotation, new Vector2(frame.Width/2, frame.Height-2), NPC.scale, sprite, 0);

            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if(NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 6)
                {
                    NPC.frame.Y += Height;
                    if (NPC.frame.Y >= Height * 5)
                    {
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter -= 12;
                }
                if (NPC.velocity.Y != 0)
                {
                    NPC.frame.Y = Height * 2;
                }
            }
            base.FindFrame(frameHeight);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
    }
}