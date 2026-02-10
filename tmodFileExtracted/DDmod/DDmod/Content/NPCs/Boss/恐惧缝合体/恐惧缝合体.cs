using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Tiles.农场;
using DDmod.Sync;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.Boss.恐惧缝合体
{
    [AutoloadBossHead]
    public class 恐惧缝合体 : ModNPC
    {
        //眼皮
        public static Asset<Texture2D> YR;
        //眼睛
        public static Asset<Texture2D> YJ;
        public static int Head;
        public override void Load()
        {
            YR = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/恐惧缝合体/恐惧缝合体_YR");
            YJ = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/恐惧缝合体/恐惧缝合体_YJ");
        }
        public override void SetStaticDefaults()
        {
            DDSystem.HBar(NPC.type, "恐惧缝合体", new Vector2(-19000, 4));
        }

        public override void SetDefaults()
        {
            DDSystem.HBar(NPC.type, "恐惧缝合体", new Vector2(-19000, 4));
            NPC.damage = 40;
            NPC.width = 92;
            NPC.height = 92;
            NPC.defense = 12;
            NPC.lifeMax = 5000;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            Main.npcFrameCount[Type] = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1.3F;
            NPC.value = 12000f;
            NPC.boss = true;
            NPC.localAI[2] = -1;

            if (!Main.dedServ) Music = DDSystem.Music(2, "恐惧缝合体");
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.恐惧缝合体"))
            });
        }
        public override void BossLoot( ref int potionType)
        {
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<恐惧缝合体宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<恐惧缝合体纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<恐惧缝合体面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<恐惧缝合体圣物>()));
            
            //大师掉落物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<灵腥盒>(), 4));

            //特别引用,普通模式

            npcLoot.NormalLoot(1,ModContent.ItemType<恐惧肉块>(),15,25);
            
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.恐惧缝合体, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.localAI[2] != -1)
            {
                player = Main.player[(int)NPC.localAI[2]];
            }
            NPC.TargetClosest();
            SoundStyle sound = SoundID.NPCDeath13;
            SoundStyle sound2 = SoundID.NPCDeath10;
            sound.MaxInstances = 20;
            sound2.MaxInstances = 20;
            Vector2 vector = player.Center - NPC.Center;
                NPC.localAI[0]++;
                if (NPC.localAI[0] > 4 * 8)
                {
                    NPC.localAI[0] = 0;
                }
            //粒子视觉效果(用到了bool[4])
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
                if (Main.netMode != 2)
                {
                    if (NPC.frame.Y >= NPC.frame.Width / 4)
                    {
                        if (!NPC.Dnpc().Bool[4])
                        {
                            for (int a = 0; a < 30; a++)
                            {
                                int D = NewDust(NPC.Center - (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation), 1, 1, 5, 0, 0, 100, default, 1.5F);
                                Main.dust[D].velocity = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(2, 5) + NPC.velocity;
                            }
                            NPC.Dnpc().Bool[4] = true;
                            sound2.Volume = 0.1F;
                            sound2.Pitch = -0.5F;
                            PlaySound(sound2, NPC.Center);
                        }
                    }
                    else
                    {
                        NPC.Dnpc().Bool[4] = false;
                    }
                }
            }
            else
            {
                NPC.spriteDirection = 0;
                if (Main.netMode != 2)
                {
                    if (NPC.frame.Y >= NPC.frame.Width / 4)
                    {
                        if (!NPC.Dnpc().Bool[4])
                        {
                            for (int a = 0; a < 30; a++)
                            {
                                int D = NewDust(NPC.Center - (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation), 1, 1, 5, 0, 0, 100, default, 1.5F);
                                Main.dust[D].velocity = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(2, 5) + NPC.velocity;
                            }
                            NPC.Dnpc().Bool[4] = true;
                            sound2.Volume = 0.1F;
                            sound2.Pitch = -0.5F;
                            PlaySound(sound2, NPC.Center);
                        }
                    }
                    else
                    {
                        NPC.Dnpc().Bool[4] = false;
                    }
                }
            }
            NPC.dontTakeDamage = true;
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.alpha = 255;
                NPC.velocity = Vector2.Zero;
                NPC.Dnpc().Stage = 1;
                return;
            }
            if (NPC.Dnpc().Stage == 1)
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 2;
                }
                else
                {
                    NPC.Dnpc().Stage = 2;
                    NPC.alpha = 0;
                }
                NPC.velocity = Vector2.Zero;
                return;
            }
            NPC.dontTakeDamage = false;
            if (player.statLife < 120)
            {
                NPC.localAI[2] = player.whoAmI;
                NPC.netUpdate = true;
            }
            if (player.dead)
            {
                NPC.localAI[2] = -1;
            }
            //斩杀玩家
            if (player.active && !player.dead && NPC.localAI[2] != -1)
            {
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 0.1F) / 21;
                DDTileDawnSystem.Filter(new Color(220, 0, 25, 255), 1, 0.05F);
                player.AddBuff(ModContent.BuffType<恐吓>(), 5);
                DDWorld.SunLight = 1F - DDTileDawnSystem.FilterValue + 0.01F;
                DDWorld.SunLightScale = 1F - DDTileDawnSystem.FilterValue + 0.01F;
                if (vector.Length() > 300)
                {
                    NPC.alpha += 10;
                    if (NPC.alpha > 0)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    if (NPC.alpha >= 255)
                    {
                        if (vector.X < 0)
                        {
                            NPC.spriteDirection = 1;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(-200, 0);
                        }
                        else
                        {
                            NPC.spriteDirection = 0;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(200, 0);
                        }
                    }
                }
                else
                {
                    NPC.alpha -= 3;
                    if (NPC.alpha > 0)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 0.1f;
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    else
                    {
                        NPC.velocity = vector.PerfectNormalize() * 10;
                    }
                }
                if (NPC.getRect().Intersects(player.getRect()))
                {
                    //player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.DDmod.PlayerKill.Kill" + Main.rand.Next(5, 7), new string[] { player.name, NPC.FullName })), 1000, 0);
                    player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill" + Main.rand.Next(5, 7), new string[] { player.name, NPC.FullName })), 1000, 0);
                }
                NPC.rotation = vector.ToRotation() + MathHelper.PiOver2 + 0.7F;
                if (NPC.spriteDirection == 1)
                    NPC.rotation = vector.ToRotation() + MathHelper.PiOver2 - 0.7F;

                return;
            }
            if (NPC.velocity.Length() > 20F)
            {
                NPC.velocity *= 0.92F;
            }
            if (NPC.target < 0 || NPC.target == 255 || player.dead||Main.dayTime)
            {
                NPC.timeLeft = 0;
                NPC.velocity.Y += 0.2F;
                return;
            }

            NPC.timeLeft = 10;
            if (AnyNPCs(ModContent.NPCType<鬼牙头>()))
            {
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 60, false, 0.2F);
                vector = Main.npc[FindFirstNPC(ModContent.NPCType<鬼牙头>())].Center - NPC.Center;
                //上下
                if (Math.Abs(vector.Y) > 50)
                {
                    if (vector.Y < 0)
                    {
                        if (NPC.velocity.Y > -12F)
                        {
                            NPC.velocity.Y -= 2.2f;
                        }
                        else
                        {
                            NPC.velocity.Y = -12f;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < 12F)
                        {
                            NPC.velocity.Y += 2.2f;
                        }
                        else
                        {
                            NPC.velocity.Y = 12f;
                        }
                    }
                }
                else
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] < 80)
                    {
                        if (NPC.velocity.Y < 12F)
                        {
                            NPC.velocity.Y += 2.2f;
                        }
                        else
                        {
                            NPC.velocity.Y -= 2.2f;
                        }
                        if (NPC.ai[0] >= 160)
                        {
                            NPC.ai[0] = 0;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y > -12F)
                        {
                            NPC.velocity.Y -= 2.2f;
                        }
                        else
                        {
                            NPC.velocity.Y += -2.2f;
                        }
                    }
                }

                //左右移动
                if (vector.X < 0)
                {
                    if (NPC.velocity.X < 24)
                    {
                        NPC.velocity.X += 1f;
                    }
                    else
                    {
                        NPC.velocity.X = 24;
                    }
                }
                else
                {
                    if (NPC.velocity.X > -24)
                    {
                        NPC.velocity.X -= 1f;
                    }
                    else
                    {
                        NPC.velocity.X = -24;
                    }
                }
                return;
            }
            if (NPC.Dnpc().Stage == 2)
            {
                if (!NPC.Dnpc().Bool[3])
                {
                    //上下
                    Vector2 SpeedY = new Vector2(0.05F, 6F);
                    if (Math.Abs(vector.Y) > 150)
                    {

                        if (vector.Y < 0)
                        {
                            if (NPC.velocity.Y > -SpeedY.Y)
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y = -SpeedY.Y;
                            }
                            NPC.ai[0] = 80;
                        }
                        else
                        {
                            if (NPC.velocity.Y < SpeedY.Y)
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y = SpeedY.Y;
                            }
                            NPC.ai[0] = 0;
                        }
                    }
                    else
                    {
                        NPC.ai[0]++;
                        if (NPC.velocity.Y > SpeedY.X * 4)
                        {
                            NPC.velocity.Y *= 0.98F;
                        }
                        if (NPC.ai[0] < 80)
                        {
                            if (NPC.velocity.Y < SpeedY.Y / 6)
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y > -SpeedY.Y / 6)
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            if (NPC.ai[0] >= 160)
                            {
                                NPC.ai[0] = 0;
                            }
                        }
                    }

                    //左右移动
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -4)
                        {
                            NPC.velocity.X -= 0.05f;
                        }
                        else
                        {
                            NPC.velocity.X = -4;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X < 4)
                        {
                            NPC.velocity.X += 0.05f;
                        }
                        else
                        {
                            NPC.velocity.X = 4;
                        }
                    }
                }
                NPC.ai[1]++;
                NPC.ai[2] = (int)NPC.ai[1] / 300;
                if (NPC.ai[2] > 12)
                {
                    NPC.Dnpc().Bool[2] = false;
                    NPC.Dnpc().Bool[3] = false;
                    NPC.ai[1] = 240;
                }
                NPC.ai[2] = (int)NPC.ai[1] / 300;
                if (NPC.ai[2] < 5)
                {
                    NPC.alpha = 0;
                }
                if (NPC.ai[2] == 0)
                {
                    if (NPC.velocity.Length() > 20F)
                    {
                        NPC.velocity *= 0.92F;
                    }
                }
                else
                if (NPC.ai[2] == 1)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss血弹>(), 12, 1);
                        }
                        sound.Volume = 0.4F;
                        sound.Pitch = -0.3F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else
                if (NPC.ai[2] == 2)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 12; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss血弹>(), 12, 1);
                        }
                        sound.Volume = 0.9F;
                        sound.Pitch = -0.9F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else if (NPC.ai[2] == 3)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 4; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss血弹>(), 12, 1);
                        }
                        sound.Volume = 0.6F;
                        sound.Pitch = 0.5F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else if (NPC.ai[2] == 4)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            }
                            int A = NewNPCs(NPC.GetSource_FromAI(), NPC.Center - Cen, ModContent.NPCType<恐惧滋生体>(), 0);
                            Main.npc[A].velocity = Velo;
                        }
                    }
                    sound.Volume = 1F;
                    sound.Pitch = -1F;
                    PlaySound(sound, NPC.Center);
                }
                else if (NPC.ai[2] == 5)
                {
                    if (!NPC.Dnpc().Bool[2])
                    {
                        NPC.alpha += 10;
                        if (NPC.alpha > 0)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                            }
                        }
                        if (NPC.alpha >= 255)
                        {
                            if (vector.X < 0)
                            {
                                NPC.spriteDirection = 1;
                                NPC.velocity = vector.PerfectNormalize() * 0.1f;
                                NPC.position = player.position + new Vector2(-500, -200);
                            }
                            else
                            {
                                NPC.spriteDirection = 0;
                                NPC.velocity = vector.PerfectNormalize() * 0.1f;
                                NPC.position = player.position + new Vector2(500, -200);
                            }
                            NPC.Dnpc().Bool[2] = true;
                        }
                    }
                    else
                    {
                        NPC.alpha -= 4;
                        if (NPC.alpha > 0)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                            }
                        }
                        else if (!NPC.Dnpc().Bool[3])
                        {
                            NPC.velocity = vector.PerfectNormalize() * 13;
                            sound2.Volume = 1F;
                            sound2.Pitch = 1F;
                            PlaySound(sound2, NPC.Center);
                            NPC.Dnpc().Bool[3] = true;
                        }

                    }
                }
                else if (NPC.ai[2] == 6)
                {
                    NPC.alpha += 10;
                    if (NPC.alpha > 0)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    if (NPC.alpha >= 255)
                    {
                        if (vector.X < 0)
                        {
                            NPC.spriteDirection = 0;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(-500, -200);
                        }
                        else
                        {
                            NPC.spriteDirection = 1;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(500, -200);
                        }
                        NPC.ai[1] = 7 * 300;
                    }
                }
                else
                {
                    NPC.alpha -= 20;
                    if (NPC.alpha > 0)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    else
                    {
                        NPC.ai[1] = 100000;
                    }
                    NPC.Dnpc().Bool[3] = false;
                }
                if (!NPC.Dnpc().Bool[3])
                {
                    NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.02F);
                }
                else
                {
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2 + 0.7F;
                    if (NPC.spriteDirection == 1)
                        NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2 - 0.7F;
                    if (vector.Length() > 800 && NPC.ai[1] < 6 * 300)
                    {
                        NPC.ai[1] = 6 * 300;
                        NPC.Dnpc().Bool[2] = false;
                    }
                }
                if (NPC.life <= NPC.lifeMax / 2)
                {
                    NPC.Dnpc().Stage = 3;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.Dnpc().Bool[2] = false;
                    NPC.Dnpc().Bool[3] = false;
                }

            }
            else
            {
                //上下
                if (!NPC.Dnpc().Bool[3])
                {
                    Vector2 SpeedY = new Vector2(0.05F, 6F);
                    if (Math.Abs(vector.Y) > 150)
                    {
                        if (vector.Y < 0)
                        {
                            if (NPC.velocity.Y > -SpeedY.Y)
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y = -SpeedY.Y;
                            }
                            NPC.ai[0] = 80;
                        }
                        else
                        {
                            if (NPC.velocity.Y < SpeedY.Y)
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y = SpeedY.Y;
                            }
                            NPC.ai[0] = 0;
                        }
                    }
                    else
                    {
                        NPC.ai[0]++;
                        if (NPC.velocity.Y > SpeedY.X * 4)
                        {
                            NPC.velocity.Y *= 0.98F;
                        }
                        if (NPC.ai[0] < 80)
                        {
                            if (NPC.velocity.Y < SpeedY.Y / 6)
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y > -SpeedY.Y / 6)
                            {
                                NPC.velocity.Y -= SpeedY.X;
                            }
                            else
                            {
                                NPC.velocity.Y += SpeedY.X;
                            }
                            if (NPC.ai[0] >= 160)
                            {
                                NPC.ai[0] = 0;
                            }
                        }
                    }

                    //左右移动
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -5)
                        {
                            NPC.velocity.X -= 0.05f;
                        }
                        else
                        {
                            NPC.velocity.X = -5;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X < 5)
                        {
                            NPC.velocity.X += 0.05f;
                        }
                        else
                        {
                            NPC.velocity.X = 5;
                        }
                    }
                }
                NPC.ai[1]++;
                NPC.ai[2] = (int)NPC.ai[1] / 300;
                if (NPC.ai[2] > 12)
                {
                    NPC.Dnpc().Bool[2] = false;
                    NPC.Dnpc().Bool[3] = false;
                    NPC.ai[1] = 240;
                }
                NPC.ai[2] = (int)NPC.ai[1] / 300;
                if (NPC.ai[2] < 5)
                {
                    NPC.alpha = 0;
                }
                if (NPC.ai[2] == 0)
                {
                    if (NPC.velocity.Length() > 20F)
                    {
                        NPC.velocity *= 0.92F;
                    }
                }
                else
                if (NPC.ai[2] == 1)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 1; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * (5 + a * 0.5f) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss大血弹>(), 24, 1);
                        }
                        sound.Volume = 0.4F;
                        sound.Pitch = -0.3F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else
                if (NPC.ai[2] == 2)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(4, 8) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss大血弹>(), 24, 1);
                        }
                        sound.Volume = 0.9F;
                        sound.Pitch = -0.9F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else if (NPC.ai[2] == 3)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 1; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-2, -2).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            }
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center - Cen, Velo, ModContent.ProjectileType<Boss大血弹>(), 24, 1);
                        }
                        sound.Volume = 0.6F;
                        sound.Pitch = 0.5F;
                        PlaySound(sound, NPC.Center);
                    }
                }
                else if (NPC.ai[2] == 4)
                {
                    if (Main.netMode != 1 && NPC.localAI[0] == 4)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            //右发射
                            Vector2 Cen = (new Vector2(-26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                            //速度
                            Vector2 Velo = new Vector2(1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            //左发射
                            if (NPC.spriteDirection == 0)
                            {
                                Cen = (new Vector2(26, 19) * NPC.scale).RotatedBy(NPC.rotation);
                                //速度
                                Velo = new Vector2(-1, -1).RotatedBy(NPC.rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(8, 10) + NPC.velocity;
                            }
                            int A = NewNPCs(NPC.GetSource_FromAI(), NPC.Center - Cen, ModContent.NPCType<恐惧滋生体>(), 0);
                            Main.npc[A].velocity = Velo;
                        }
                    }
                    sound.Volume = 1F;
                    sound.Pitch = -1F;
                    PlaySound(sound, NPC.Center);
                }
                else if (NPC.ai[2] == 5)
                {
                    if (!NPC.Dnpc().Bool[2])
                    {
                        NPC.alpha += 10;
                        if (NPC.alpha > 0)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                            }
                        }
                        if (NPC.alpha >= 255)
                        {
                            if (vector.X < 0)
                            {
                                NPC.spriteDirection = 1;
                                NPC.velocity = vector.PerfectNormalize() * 0.1f;
                                NPC.position = player.position + new Vector2(-500, -200);
                            }
                            else
                            {
                                NPC.spriteDirection = 0;
                                NPC.velocity = vector.PerfectNormalize() * 0.1f;
                                NPC.position = player.position + new Vector2(500, -200);
                            }
                            NPC.Dnpc().Bool[2] = true;
                        }
                    }
                    else
                    {
                        NPC.alpha -= 4;
                        if (NPC.alpha > 0)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                            }
                        }
                        else if (!NPC.Dnpc().Bool[3])
                        {
                            NPC.velocity = vector.PerfectNormalize() * 13;
                            sound2.Volume = 1F;
                            sound2.Pitch = 1F;
                            PlaySound(sound2, NPC.Center);
                            NPC.Dnpc().Bool[3] = true;
                        }

                    }
                }
                else if (NPC.ai[2] == 6)
                {
                    NPC.alpha += 10;
                    if (NPC.alpha > 0)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    if (NPC.alpha >= 255)
                    {
                        if (vector.X < 0)
                        {
                            NPC.spriteDirection = 0;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(-500, -200);
                        }
                        else
                        {
                            NPC.spriteDirection = 1;
                            NPC.velocity = vector.PerfectNormalize() * 0.1f;
                            NPC.position = player.position + new Vector2(500, -200);
                        }
                        NPC.ai[1] = 7 * 300;
                    }
                }
                else
                {
                    NPC.alpha -= 20;
                    if (NPC.alpha > 0)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            int D = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 4, 100, default, 1.5F);
                        }
                    }
                    else
                    {
                        NPC.ai[1] = 100000;
                    }
                    NPC.Dnpc().Bool[3] = false;
                }
                if (!NPC.Dnpc().Bool[3])
                {
                    NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.02F);
                }
                else
                {
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2 + 0.7F;
                    if (NPC.spriteDirection == 1)
                        NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2 - 0.7F;
                    if (vector.Length() > 800&& NPC.ai[1]<6*300)
                    {
                        NPC.ai[1] = 6*300;
                        NPC.Dnpc().Bool[2] = false;
                    }
                }
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 600; i++)
                {
                    if (i < 300)
                    {
                        NewDust(NPC.position, NPC.width, NPC.height, 5, Main.rand.NextFloat(-12, 12), Main.rand.NextFloat(-8, 8), 100, default, NPC.scale * 1.3F);
                    }
                    else
                    {
                        NewDust(NPC.position+new Vector2(0,NPC.height/2), NPC.width, NPC.height/4, 5,0, Main.rand.NextFloat(2, 8), 100, default, NPC.scale);
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int A = 1; A <= 9; A++)
                    {
                        int GoreType = Mod.Find<ModGore>("恐惧缝合体"+ A).Type;
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), GoreType, NPC.scale);
                    }
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.Dnpc().Defaults)
            {
                NPC.localAI[0]++;
                if (NPC.localAI[0] > 4 * 8)
                {
                    NPC.localAI[0] = 0;
                }
            }
            NPC.frameCounter++;
            NPC.frame.Y = frameHeight * ((int)NPC.localAI[0] / 8);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            drawColor = NPC.GetAlpha(drawColor);
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            int X = NPC.frame.Y / NPC.frame.Height;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 5)/2, NPC.scale,  sprite, 0f);
            texture = YJ.Value;
            int fx = 0;
            int fy = 0;
            if(X!=0&&X!=4)
            {
                fy = 2;
            }
            if(X==2)
            {
                fx = 6;
            }
            if(X==3)
            {
                fx = 4;
            }
            if(X==4)
            {
                fx = -2;
            }
            if (sprite == 0)
            {
                Vector2 vector = player.Center - (NPC.Center + (new Vector2(22 + fx, -29)* NPC.scale).RotatedBy(NPC.rotation));
                spriteBatch.Draw(texture, (NPC.Center + (new Vector2(22 + fx, -29) * NPC.scale).RotatedBy(NPC.rotation)) + vector.PerfectNormalize() * 10 - screenPos, null, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height) / 2, NPC.scale, sprite, 0f);
                Color color = drawColor * 0.75F;
                color.A = 255;
                color = NPC.GetAlpha(color);
                vector = player.Center - (NPC.Center + (new Vector2(-29, 17 + fy) * NPC.scale).RotatedBy(NPC.rotation));
                spriteBatch.Draw(texture, (NPC.Center + (new Vector2(-29, 17+ fy) * NPC.scale).RotatedBy(NPC.rotation)) + vector.PerfectNormalize() * 6 - screenPos, null, color, NPC.rotation, new Vector2(texture.Width, texture.Height) / 2, NPC.scale * 0.75F, sprite, 0f);
            }
            else
            {
                Vector2 vector = player.Center - (NPC.Center + (new Vector2(-(22 + fx), -29) * NPC.scale).RotatedBy(NPC.rotation));
                spriteBatch.Draw(texture, (NPC.Center + (new Vector2(-(22 + fx), -29) * NPC.scale).RotatedBy(NPC.rotation)) + vector.PerfectNormalize() *10 - screenPos, null, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height) / 2, NPC.scale, sprite, 0f);
                Color color = drawColor * 0.75F;
                color.A = 255;
                color = NPC.GetAlpha(color);
                vector = player.Center - (NPC.Center + (new Vector2(29, 17 + fy) * NPC.scale).RotatedBy(NPC.rotation));
                spriteBatch.Draw(texture, (NPC.Center + (new Vector2(29, 17+ fy) * NPC.scale).RotatedBy(NPC.rotation)) + vector.PerfectNormalize() * 6 - screenPos, null, color, NPC.rotation, new Vector2(texture.Width, texture.Height) / 2, NPC.scale*0.75F, sprite, 0f);
            }
            texture = YR.Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 5)/2, NPC.scale,  sprite, 0f);
            
            return false;
        }
    }
}