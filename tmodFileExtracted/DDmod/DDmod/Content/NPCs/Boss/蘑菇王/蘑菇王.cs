
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.Items.Talisman;
using DDmod.Sync;
using DDmod.Worlds;
using System.Linq;
using Terraria;

namespace DDmod.Content.NPCs.Boss.蘑菇王
{
    [AutoloadBossHead]
    public class 蘑菇王 : ModNPC
	{
        public static Asset<Texture2D> 生气;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 11;
            DDSystem.HBar(NPC.type, "蘑菇王", new Vector2(-10000, 2));

        }
        public override void Load()
        {
            生气 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/蘑菇王/生气");
        }

        public override void SetDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.damage = 26;
			NPC.width = 66;
			NPC.height = 66;
			NPC.aiStyle = -1;
			NPC.defense = 4;
			NPC.scale = 1.3f;
			NPC.lifeMax = 2500;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 80, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.HitSound = SoundID.NPCHit1;
            if (!Main.dedServ) Music = DDSystem.Music(2, "蘑菇王") ;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
			NPC.boss = true;
            NPC.Dnpc().Properties.Fungi = true;
            NPC.Dnpc().Properties.BossLife = 1.05F;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void ModifyTypeName(ref string typeName)
        {
        }
        public override void AI()
        {
			Player player = Main.player[NPC.target];
			
            // NPC和物块相撞
            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 1)), NPC.width, 1);
            bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 2)), NPC.width, 1);
            int PO = (int)((player.position.Y + player.height) / 16 - (NPC.position.Y + NPC.height) / 16);
            PO *= 16;

            NPC.velocity.Y += NPC.gravity;
            if (NPC.velocity.Y > NPC.maxFallSpeed)
            {
                NPC.velocity.Y = NPC.maxFallSpeed;
            }
            if ((NPC.velocity.Y > 0 && PO <= 16))
                NPC.velocity.Y = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false).Y;

            if (TileCollision || TileCollision2)
            {
                if ((NPC.velocity.Y > 0 && PO <= 16) || NPC.velocity.X == 0)
                {
                    NPC.velocity.Y = 0;
                }
            }
            if (NPC.velocity.Y == 0)
            {
                if (NPC.velocity.X != 0)
                {
                    if (PO > 16 && Collision.CanHitLine(player.position + new Vector2(player.width / 2, player.height - 1), 1, 1, new Vector2(player.position.X + player.width / 2, NPC.position.Y + NPC.height - 1), 1, 1))
                    {
                        NPC.position.Y += NPC.Dnpc().Times[2];
                    }
                    if (PO <= 0)
                    {
                        if (TileCollision2)
                        {
                            NPC.position.Y -= NPC.Dnpc().Times[2]/2;
                        }
                    }
                    NPC.Dnpc().Times[2] += NPC.gravity;
                    if (NPC.Dnpc().Times[2] > NPC.maxFallSpeed)
                    {
                        NPC.Dnpc().Times[2] = NPC.maxFallSpeed;
                    }
                }
                else
                {
                    NPC.Dnpc().Times[2] = 0;
                    NPC.Dnpc().Times[3] = 0;
                }
            }
            NPC.TargetClosest();
            if (NPC.target < 0 || NPC.target == 255 || player.dead)
            {
                NPC.position.Y += 8;
                if (NPC.Dnpc().Times[3]++ > 300)
                {
                    NPC.active = false;
                }
            }
            if(NPC.Distance(player.Center) > 2000)
            {
                if (NPC.Dnpc().Times[3]++ > 1200)
                {
                    NPC.active = false;
                }
            }
            else
            {
                NPC.Dnpc().Times[3] = 0;
            }
            Vector2 vector = player.Center - NPC.Center;
            if (NPC.velocity == Vector2.Zero)
            {
                if (vector.X < 0)
                {
                    NPC.spriteDirection = 0;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
            }
            else
            {
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = 0;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
            }
            //浮空时间
            if(NPC.ai[2]>0)
            {
                NPC.ai[2]--;
                NPC.velocity.Y = 0.01f;
            }
            if (NPC.velocity.Y == 0 && NPC.velocity.X != 0)
            {
                if (NPC.soundDelay == 0)
                {
                    for (int a = 0; a < Math.Abs(NPC.velocity.X); a++)
                    {
                        int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, ModContent.DustType<蘑菇粒子>(), Scale: 0.7F);
                        Main.dust[A].noGravity = false;
                    }
                    NPC.soundDelay = (int)(24 -Math.Abs(NPC.velocity.X));
                    SoundStyle sound = SoundID.NPCDeath1;
                    sound.Volume = Math.Abs(NPC.velocity.X)/10;
                    sound.Pitch = -0.4F;
                    PlaySound(sound, NPC.position);
                }
            }
            //NPC.ai[3] = 1  == 预备跳
            if (NPC.Dnpc().Stage==0)
            {
                NPC.ai[1]++;
                if (PO < -180)
                {
                    if (NPC.ai[1] % 300 == 0 && Main.netMode != 1)
                    {
                        int A = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[A].velocity = vector.PerfectNormalize() * 25;
                        int B = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[B].velocity = vector.PerfectNormalize().RotatedBy(-0.2F) * 25;
                        int C = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[C].velocity = vector.PerfectNormalize().RotatedBy(0.2F) * 25;
                    }
                }
                //行走
                if (NPC.ai[0] < 300)
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0]++;
                    if (vector.X > 0)
                    {
                        if (NPC.velocity.X < 4)
                        {
                            NPC.velocity.X += 0.1F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X > -4)
                        {
                            NPC.velocity.X -= 0.1F;
                        }
                    }
                    if (NPC.ai[0] == 180)
                    {
                        if (PO < -16)
                        {
                            NPC.velocity.Y = -10;
                        }
                        else
                        {
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 12;
                            }
                            else
                            {
                                NPC.velocity.X = -12;
                            }
                        }
                    }
                    else if (NPC.ai[0] > 180 && NPC.velocity.Y < 0 && PO >= 0)
                    {
                        if (vector.X > 0)
                        {
                            NPC.velocity.X = 12;
                            NPC.ai[2] = 12;
                        }
                        else
                        {
                            NPC.velocity.X = -12;
                            NPC.ai[2] = 12;
                        }
                    }
                }
                //跳跃
                else if (NPC.ai[0] < 600)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X *= 0.92F;
                        if (NPC.velocity.Length() < 0.1F)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.ai[0]++;
                    }
                    NPC.ai[3] = 0;
                    if (NPC.ai[0] > 360 && NPC.ai[0] < 540)
                    {
                        if (NPC.ai[0] % 30 >= 10)
                        {
                            NPC.ai[3] = 1;
                        }
                        if (NPC.ai[0] % 30 == 0)
                        {
                            NPC.ai[0]++;
                            if (NPC.velocity.Y == 0)
                                NPC.velocity.Y = -4;
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 6;
                            }
                            else
                            {
                                NPC.velocity.X = -6;
                            }
                        }
                    }
                    else
                    {
                        if (NPC.ai[0] >= 540)
                        {
                            NPC.ai[3] = 1;
                        }
                        if (NPC.ai[0] == 600)
                        {
                            if (NPC.velocity.Y == 0)
                                NPC.velocity.Y = -8;
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 10;
                            }
                            else
                            {
                                NPC.velocity.X = -10;
                            }
                        }

                    }
                }
                else
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.ai[0]++;
                        NPC.velocity.X *= 0.92F;
                        if (NPC.velocity.Length() < 0.1F)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    NPC.ai[3] = 1;
                    if (NPC.ai[0] > 780)
                    {
                        NPC.ai[3] = 0;
                        if (NPC.velocity == Vector2.Zero)
                        {
                            NPC.ai[0] = 0;
                        }
                    }
                }
                if((float)NPC.life/NPC.lifeMax<0.5f)
                {
                    NPC.velocity.X = 0; 
                    NPC.Dnpc().Stage = 1;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                }
            }
            else if (NPC.Dnpc().Stage==1)
            {
                NPC.ai[0]++;
                if (Main.netMode != NetmodeID.Server&& NPC.ai[0]==2)
                {
                    int T = CombatText.NewText(NPC.getRect(), new Color(255, 32, 80), "O˰O", true);
                    SoundStyle s = SoundID.Roar;
                    s.Pitch = -1;
                    PlaySound(s, NPC.Center);
                }
                NPC.dontTakeDamage = true;
                if(NPC.ai[0]>120)
                {
                    NPC.ai[0] = 0;
                    NPC.Dnpc().Stage = 2;
                }
            }
            else if (NPC.Dnpc().Stage == 2)
            {

                NPC.dontTakeDamage = false;
                NPC.ai[1]++;
                if (PO < -180)
                {
                    if (NPC.ai[1] % 300 == 0 && Main.netMode != 1)
                    {
                        int A = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[A].velocity = vector.PerfectNormalize() * 25;
                        int B = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[B].velocity = vector.PerfectNormalize().RotatedBy(-0.2F) * 25;
                        int C = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<蘑菇怪>(), 0);
                        Main.npc[C].velocity = vector.PerfectNormalize().RotatedBy(0.2F) * 25;
                    }
                }
                //行走
                if (NPC.ai[0] < 300)
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0]++;
                    if (vector.X > 0)
                    {
                        if (NPC.velocity.X < 6)
                        {
                            NPC.velocity.X += 0.1F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X > -6)
                        {
                            NPC.velocity.X -= 0.1F;
                        }
                    }
                    if (NPC.ai[0] == 180)
                    {
                        if (PO < -16)
                        {
                            NPC.velocity.Y = -14;
                        }
                        else
                        {
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 16;
                            }
                            else
                            {
                                NPC.velocity.X = -16;
                            }
                        }
                    }
                    else if (NPC.ai[0] > 180 && NPC.velocity.Y < 0 && PO >= 0)
                    {
                        if (vector.X > 0)
                        {
                            NPC.velocity.X = 16;
                            NPC.ai[2] = 16;
                        }
                        else
                        {
                            NPC.velocity.X = -16;
                            NPC.ai[2] = 16;
                        }
                    }
                }
                //跳跃
                else if (NPC.ai[0] < 600)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X *= 0.92F;
                        if (NPC.velocity.Length() < 0.1F)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.ai[0]++;
                    }
                    NPC.ai[3] = 0;
                    if (NPC.ai[0] > 360 && NPC.ai[0] < 540)
                    {
                        if (NPC.ai[0] % 30 >= 10)
                        {
                            NPC.ai[3] = 1;
                        }
                        if (NPC.ai[0] % 30 == 0)
                        {
                            NPC.ai[0]++;
                            if (NPC.velocity.Y == 0)
                                NPC.velocity.Y = -7;
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 8;
                            }
                            else
                            {
                                NPC.velocity.X = -8;
                            }
                        }
                    }
                    else
                    {
                        if (NPC.ai[0] >= 540)
                        {
                            NPC.ai[3] = 1;
                        }
                        if (NPC.ai[0] == 600)
                        {
                            if (NPC.velocity.Y == 0)
                                NPC.velocity.Y = -11;
                            if (vector.X > 0)
                            {
                                NPC.velocity.X = 14;
                            }
                            else
                            {
                                NPC.velocity.X = -14;
                            }
                        }

                    }
                }
                else
                {
                    
                    if (NPC.velocity.Y == 0)
                    {
                        if (!NPC.Dnpc().Bool[0])
                        {
                            NPC.Dnpc().Bool[0] = true;
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 8; a++)
                                {
                                    int A = NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 12), ModContent.NPCType<蘑菇怪>(), 0);
                                    Main.npc[A].velocity = new Vector2(0, -10).RotatedBy(Main.rand.NextFloat(-1.5F, 1.5F))+NPC.velocity;
                                }
                            }
                        }
                        NPC.ai[0]++;
                        NPC.velocity.X *= 0.92F;
                        if (NPC.velocity.Length() < 0.1F)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    NPC.ai[3] = 1;
                    if (NPC.ai[0] > 780)
                    {
                        NPC.ai[3] = 0;
                        if (NPC.velocity == Vector2.Zero)
                        {
                            NPC.Dnpc().Bool[0] = false;
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.蘑菇王"))
            ]);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<蘑菇王宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<蘑菇王纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<蘑菇王面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<蘑菇王圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<可再生蘑菇>(), 4));

            npcLoot.SpecialLoot(ModContent.ItemType<蘑菇飞刀>(), 1);
            //特别引用,普通模式
            //普通模式
            int[] A = new int[4];
            A[0] = ModContent.ItemType<蘑菇短剑>();
            A[1] = ModContent.ItemType<蘑菇弓>();
            A[2] = ModContent.ItemType<蘑菇杖>();
            A[3] = ModContent.ItemType<蘑菇鞭>();
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.蘑菇王, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 4), NPC.frame, drawColor, NPC.rotation, new Vector2(NPC.frame.Width / 2, NPC.frame.Height), NPC.scale, sprite, 0);

            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] + NPC.Size / 2 - screenPos + new Vector2(0, NPC.height / 2 +4);
                Color oldcolor = drawColor * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length) * 0.5F;
                spriteBatch.Draw(texture, vector2, NPC.frame, oldcolor, NPC.rotation, new Vector2(NPC.frame.Width / 2, NPC.frame.Height), NPC.scale, sprite, 0f);
            }
            if(NPC.Dnpc().Stage==1)
            {
                NPC.Dnpc().Times[4]+=0.1F;
                texture = 生气.Value;
                Rectangle rectangle = new Rectangle(texture.Width/2,texture.Height/2*((int)NPC.Dnpc().Times[4]%2), texture.Width / 2, texture.Height / 2);
                if (sprite == 0)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(-16, 16) * NPC.scale, rectangle, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 2) / 2, NPC.scale, sprite, 0);
                }
                else
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(16, 16) * NPC.scale, rectangle, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 2) / 2, NPC.scale, sprite, 0);
                }
            }
            return false;
        }
        int F = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 70;
            if (NPC.velocity.Y == 0)
            {
                NPC.frame.X = 70;
                if ((NPC.ai[1] == 3 && NPC.ai[0] > 120))
                {

                    NPC.frame.X = 0;
                    NPC.frame.Y = frameHeight * 9;
                    return;
                }
                if (F == 3 || F == 4 || F == 5)
                {
                    if (F != 5)
                    {
                        NPC.frameCounter = 0;
                        F = 5;
                        NPC.frame.Y = frameHeight * 8;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 6)
                    {
                        if (NPC.frame.Y > frameHeight * 9)
                        {
                            NPC.frameCounter = 0;
                            F = 0;
                            NPC.frame.Y = 0;
                        }
                        else
                        {
                            NPC.frame.Y += frameHeight;
                            NPC.frameCounter = 0;
                        }
                    }
                    return;
                }
                if (NPC.ai[3] == 1)
                {
                    if (F != 2)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = 0;
                        F = 2;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 2)
                    {
                        NPC.frame.Y = frameHeight * 2;
                    }
                    return;
                }
                NPC.frame.X = 0;
                //1:0-3原地
                //1:4-9行走
                if (NPC.velocity.X == 0)
                {
                    if (F != 0)
                    {
                        NPC.frameCounter = 0;
                        F = 0;
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y > frameHeight * 4)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    NPC.frameCounter += Math.Abs(NPC.velocity.X);
                    if (F != 1)
                    {
                        NPC.frameCounter = 0;
                        F = 1;
                        NPC.frame.Y = frameHeight * 5;
                    }
                    float SP = 14;
                    if (NPC.frameCounter > SP)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter -= SP;
                    }
                    if (NPC.frame.Y > frameHeight * 9)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
            }
            else
            {
                //2:0-2预备跳
                //2:3-5上升
                //2:6-7下落
                //2:8-10落地
                NPC.frame.X = 70;
                if (NPC.velocity.Y < 0)
                {
                    if (F != 3)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 3;
                        F = 3;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
                else
                {
                    if (F != 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 6;
                        F = 4;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 7)
                    {
                        NPC.frame.Y = frameHeight * 7;
                    }
                }
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 10; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<蘑菇粒子>(), 0f, 0f, 100);
                Main.dust[D].noGravity = false;
                Main.dust[D].scale *= 1f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector/2;
            }
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 300; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<蘑菇粒子>(), 0f, 0f, 100);
                    Main.dust[D].noGravity = false;
                    Main.dust[D].scale *= 1f + Main.rand.Next(2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int T = CombatText.NewText(NPC.getRect(), new Color(255, 32, 80), "XxX", true);
                    Main.combatText[T].lifeTime += 300;
                    SoundStyle s = SoundID.NPCDeath21;

                    int GoreType = Mod.Find<ModGore>("蘑菇王1").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 0), new Vector2(0, -10).RotatedBy(NPC.rotation), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("蘑菇王2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, NPC.height/2), new Vector2(0, 10).RotatedBy(NPC.rotation), GoreType, NPC.scale);

                    s.Pitch = -1;
                    PlaySound(s, NPC.Center);
                }
            }
		}
	}
}
