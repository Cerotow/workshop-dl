using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.天地守卫;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.NPCs.Boss.天地守卫
{
    [AutoloadBossHead]
    public class 大地守卫 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/心心守卫2_Glow");
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Guardian : Les");
           //DisplayName.AddTranslation(7, "生命守卫:莱斯");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            DDSystem.HBar(NPC.type, "大地守卫", new Vector2(-4007, 0));

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 31000;
            NPC.defense = 12;
            NPC.damage = 120;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(64 * NPC.scale);
            NPC.height = (int)(64 * NPC.scale);
            NPC.scale = 1f;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.npcSlots = 111f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ) Music = DDSystem.Music(3, "天地守卫");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.boss = true;
            //NPC.dontTakeDamage = true;
            NPC.localAI[0] = 5;
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().Properties.Stone = true;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.Dnpc().Properties.BossLife = 1.25F;
        }


        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.2;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.大地守卫"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<天地守卫宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<天地守卫纪念章物品>(), 10));
            //面具
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<鬼牙面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<天地守卫圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<星心之魂>(), 4));

            int[] A = new int[4];
            A[0] = ModContent.ItemType<苍穹原典>();
            A[1] = ModContent.ItemType<繁星万象>();
            A[2] = ModContent.ItemType<大地之锋>();
            A[3] = ModContent.ItemType<晶凝蓄能炮>();

            npcLoot.NormalLoot(1, A);

        }
        public override void BossLoot(ref int potionType)
        {
            potionType = 499;
        }
        public override LocalizedText DeathMessage => Language.GetText("Announcement.HasBeenDefeated_Plural").WithFormatArgs(Language.GetText("Mods.DDmod.NPCs.天地守卫.EntryName"));
        public bool title;
        public bool brothers;
        public bool RecognizeX;
        public bool RecognizeY;
        public int brothersTimer;
        public int brothersTimer2;
        public int 次数;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.觉醒星心双子, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        int TL = 0;
        int TLTime = 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.position, 2f, 0.5f, 0.5f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            //Main.LocalPlayer.Dplayer().Bossperspective(P.Center - new Vector2(0, 300), 10, false, 0.15F);
            Vector2 vector = P.Center - NPC.Center;
            if (!NPC.AnyNPCs(ModContent.NPCType<苍穹守卫>()))
            {
                NPC.velocity.Y += 1F;
                NPC.timeLeft--;
                NPC.ai[1]++;
                if (NPC.ai[1] > 300)
                {
                    NPC.active = false;
                }
            }
            else if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y += 1F;
                NPC.timeLeft--;
            }
            else if (NPC.localAI[1] == 2)
            {
                Vector2 PO = P.Center - new Vector2(300, 0) - NPC.Center;
                float speed = PO.Length() / 20;
                if (NPC.ai[1]<=7)
                {
                    if (Main.LocalPlayer.position.Y + Main.LocalPlayer.height >= NPC.ai[3])
                    {
                        Main.LocalPlayer.position.Y = NPC.ai[3] - Main.LocalPlayer.height;
                        Main.LocalPlayer.Aplayer().Stand3 = 3;
                    }
                }
                if (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<苍穹守卫>())].localAI[1] != 2)
                {
                    NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.ArousalDialogue" + 11), 生命);
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                }
                else
                {
                    if (NPCDowned.觉醒星心双子) NPC.ai[1] = 8;
                    if (NPC.ai[1] <= 7)
                    {
                        if (!Main.dedServ) Music = DDSystem.Music(3, "星心对话");
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        NPC.ai[0]++;
                        if (NPC.ai[0] < 300)
                        {
                            NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.ArousalDialogue" + (NPC.ai[1] + 12)), 生命);
                        }
                        if (NPC.ai[1] == 0 || (NPC.ai[1] >= 2 && NPC.ai[1] <= 4))
                        {
                            if (NPC.ai[0] >= 300)
                            {
                                NPC.ai[0] = 0;
                                NPC.ai[1]++;
                            }
                        }
                        else
                        {
                            if (NPC.ai[0] >= 600)
                            {
                                NPC.ai[0] = 0;
                                NPC.ai[1]++;
                            }
                        }
                    }
                    else
                    {
                        if (!Main.dedServ) Music = -1;
                        if (!NPC.Dnpc().Bool[1])
                        {
                            NPC.Dnpc().Bool[1] = true;
                            NPC.NPCLoot();
                            Main.npc[NPC.FindFirstNPC(ModContent.NPCType<苍穹守卫>())].NPCLoot();
                            NPC.boss = false;
                        }
                        NPC.velocity.Y += 0.2F;

                        NPC.ai[1]++;
                        if (NPC.ai[1]>300)
                        {
                            NPC.active = false;
                        }
                    }
                }
            }
            else
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<星心考验>(),5);
                NPC.timeLeft = 10;
                if (NPC.Dnpc().Stage == 0)
                {
                    int ProjType = ModContent.ProjectileType<BossArousalHeart>();
                    Vector2 PO = P.Center - new Vector2(300) - NPC.Center;
                    NPC.ai[0]++;
                    //ai0
                    if (NPC.ai[0] < 0)
                    {
                        PO = P.Center - new Vector2(300, -300) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    }
                    else
                    //ai1
                    if (NPC.ai[0] < 300)
                    {
                        PO = P.Center - new Vector2(300, -300) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] % 60 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 26, 0, -1, 0, 0.5f);
                        }
                    }
                    //ai2
                    else if (NPC.ai[0] < 900)
                    {
                        PO = P.Center - new Vector2(500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] % 60 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 0.5f, -1);
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 0.5f, 0);
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 0.5f, 1);
                        }

                    }
                    //ai3
                    else if (NPC.ai[0] < 1500)
                    {
                        NPC.Dnpc().Bool[0] = true;
                        if (NPC.ai[0] < 1420)
                        {
                            PO = P.Center - new Vector2(700, 0) - NPC.Center;
                            float speed = PO.Length() / 20;
                            NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;

                            NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
                        }
                        else if (NPC.ai[0] <= 1440)
                        {
                            PO = P.Center - new Vector2(700 + (NPC.ai[0] - 1420) * 200, 0) - NPC.Center;
                            float speed = PO.Length() / 20;
                            NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;

                            NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
                            if (NPC.ai[0] == 1440)
                            {
                                NPC.velocity = vector.PerfectNormalize() * 42;
                            }
                        }
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        }

                    }
                    //ai4
                    else if (NPC.ai[0] < 2100)
                    {
                        PO = P.Center - new Vector2(300, -300) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] >= 1600)
                        {
                            if (NPC.ai[0] % 100 == 40)
                            {
                                NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 24, ProjType, 22, 0, -1, 2, 3.5f, 5);
                                NPC.velocity = -vector.PerfectNormalize() * 12;
                            }
                            else if (NPC.ai[0] % 100 <= 40)
                            {
                                for (int a = 0; a < 4; a++)
                                {
                                    Vector2 v = new Vector2(Main.rand.NextFloat(160, 200), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    int D = NewDust(NPC.Center - new Vector2(4) + v, 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(255, 100, 100, 0), Scale: 5.2F);
                                    Main.dust[D].velocity = -v / 10;
                                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                                    Main.dust[D].customData = 4;
                                    GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.ai[0] = -120;
                    }
                    if (NPC.life <= NPC.lifeMax / 2)
                    {
                        NPC.Dnpc().Stage = 1;
                        NPC.ai[0] = 0;
                        for (int a = 0; a < 80; a++)
                        {
                            Vector2 v = new Vector2(Main.rand.NextFloat(100, 200), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            int D = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(255, 100, 100, 0), Scale: 10.2F);
                            Main.dust[D].velocity = v / 5;
                            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                            Main.dust[D].customData = 2;
                            GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        }
                    }
                }
                else if (NPC.Dnpc().Stage == 1)
                {
                    NPC.ai[0] = 0;
                    NPC.dontTakeDamage = true;
                    NPC.life = NPC.lifeMax / 2;
                    if (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<苍穹守卫>())].Dnpc().Stage != 0)
                    {
                        NPC.Dnpc().Stage = 2;
                    }
                    Vector2 PO = P.Center - new Vector2(500, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    NPC.ai[1] = 1;
                }
                else if (NPC.Dnpc().Stage == 2)
                {
                    NPC.ai[0]++;
                    NPC.dontTakeDamage = true;
                    NPC.life = NPC.lifeMax / 2;
                    NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.ArousalDialogue" + 10), 生命);
                    if (NPC.ai[2] < 500)
                        NPC.ai[2] += 2;
                    if (NPC.ai[0] > 300)
                    {
                        NPC.Dnpc().Stage = 3;
                        for (int a = 0; a < 80; a++)
                        {
                            Vector2 v = new Vector2(Main.rand.NextFloat(100, 200), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            int D = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(255, 100, 100, 0), Scale: 10.2F);
                            Main.dust[D].velocity = v / 5;
                            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                            Main.dust[D].customData = 2;
                            GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        }
                        NPC.ai[1] = 0.25f;
                        NPC.ai[0] = -120;
                    }
                    NPC.ai[3] = NPC.position.Y + P.height + NPC.ai[2];
                    Vector2 PO = P.Center - new Vector2(500, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    NPC.ai[1] = 1;
                }
                else if (NPC.Dnpc().Stage == 3)
                {
                    NPC.dontTakeDamage = false;
                    NPC.timeLeft = 10;
                    for (int a = 0; a < 255; a++)
                    {
                        if (Main.player[a].active&& !Main.player[a].dead&& Main.player[a].position.Y + Main.player[a].height >= NPC.ai[3])
                        {
                            Main.player[a].position.Y = NPC.ai[3] - Main.player[a].height;
                            Main.player[a].Aplayer().Stand3 = 3;
                        }
                    }
                    int ProjType = ModContent.ProjectileType<BossArousalHeart>();
                    Vector2 PO = P.Center - new Vector2(300,0) - NPC.Center;
                    NPC.ai[0]++;
                    //ai0
                    if (NPC.ai[0] < 0)
                    {
                        PO = P.Center - new Vector2(300, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    }
                    else
                    //ai1
                    if (NPC.ai[0] < 300)
                    {
                        PO = P.Center - new Vector2(500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] % 60 <= 50 && NPC.ai[0] % 60 >= 30)
                        {
                            if (NPC.ai[0] % 4 == 0)
                            {
                                NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 18, ProjType, 26, 0, -1, 0, 0.5f);
                            }
                        }
                    }
                    //ai2
                    else if (NPC.ai[0] < 900)
                    {
                        PO = P.Center - new Vector2(500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] >360&& NPC.ai[0] % 100 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 1f, -1);
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 1.5f, 0);
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 1, 1f, 1);

                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0.5f, -1);
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0.5f, 1);
                        }

                    }
                    //ai3
                    else if (NPC.ai[0] < 1500)
                    {
                        NPC.Dnpc().Bool[0] = true;
                        if (NPC.ai[0] < 1420)
                        {
                            PO = P.Center - new Vector2(700, 0) - NPC.Center;
                            float speed = PO.Length() / 20;
                            NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;

                            NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
                            if (NPC.ai[0] >=1000&& NPC.ai[0]%60==0)
                            {
                                NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 16, ProjType, 22, 0, -1, 1, 2f, 0);
                            }
                        }
                        else if (NPC.ai[0] <= 1440)
                        {
                            PO = P.Center - new Vector2(700 + (NPC.ai[0] - 1420) * 200, 0) - NPC.Center;
                            float speed = PO.Length() / 20;
                            NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;

                            NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
                            if (NPC.ai[0] == 1440)
                            {
                                NPC.velocity = vector.PerfectNormalize() * 42;
                            }
                        }
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        }

                    }
                    //ai4
                    else if (NPC.ai[0] < 2100)
                    {
                        PO = P.Center - new Vector2(500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] >= 1600)
                        {
                            if (NPC.ai[0] % 100 == 40)
                            {
                                for (int a = 0; a < 4; a++)
                                {
                                    NPC.NewNPCProj(NPC.Center, -vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1.2F,1.2F)) * 12, ProjType, 22, 0, -1, 3, 0.5f);
                                }
                                NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 18, ProjType, 22, 0, -1, 2, 2f, 5);
                                NPC.velocity = -vector.PerfectNormalize() * 12;
                            }
                            else if (NPC.ai[0] % 100 <= 40)
                            {
                                for (int a = 0; a < 4; a++)
                                {
                                    Vector2 v = new Vector2(Main.rand.NextFloat(160, 200), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    int D = NewDust(NPC.Center - new Vector2(4) + v, 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(255, 100, 100, 0), Scale: 5.2F);
                                    Main.dust[D].velocity = -v / 10;
                                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                                    Main.dust[D].customData = 4;
                                    GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.ai[0] = -120;
                    }
                }
            }
            if (NPC.Dnpc().Bool[0])
            {
                NPC.Dnpc().Times[1] += 0.2f;
                NPC.Dnpc().Bool[0] = false;
                int W = (int)(NPC.width * NPC.Dnpc().Times[1]);
                int H = (int)(NPC.height * NPC.Dnpc().Times[1]);
                if (!Main.LocalPlayer.immune)
                {
                    if (new Rectangle((int)NPC.Center.X - W / 2, (int)NPC.Center.Y - H / 2, W, H).Intersects(Main.LocalPlayer.getRect()))
                    {
                        if (!Main.LocalPlayer.noKnockback)
                        {
                            Main.LocalPlayer.velocity = vector.PerfectNormalize() * 6;
                        }
                        Main.LocalPlayer.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(NPC.defDamage), 0);
                    }
                }

            }
            else
            {
                NPC.Dnpc().Times[1] -= 0.2f;
                NPC.rotation = NPC.velocity.X * 0.05f;
            }
            NPC.damage = 0;
            DDHelper.MaxandMinF(ref NPC.Dnpc().Times[1], 4.8f, 1.2f);
            return;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, DustID.HeartCrystal, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, DustID.HeartCrystal, hit.HitDirection, -1f, 0, default, 1f);
                }
                if(NPC.Dnpc().Stage==3)
                NPC.localAI[1] = 2;
                NPC.dontTakeDamage = true;
                if (NPC.Dnpc().Deathrattle)
                {
                    NPC.life = 5;
                }
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (NPC.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            //绘制光效残影
            if (NPC.localAI[1] != 2)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    if (i != 0)
                    {
                        Vector2 vector2 = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2 - screenPos;
                        Color color = new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                        spriteBatch.Draw(Glow.Value, vector2, null, color * 0.5f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4 * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    }
                }
            }
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(Glow.Value, NPC.position + NPC.Size / 2 - screenPos, null, new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4, spriteEffects, 0f);
            spriteBatch.Draw(Glow.Value, NPC.position + NPC.Size / 2 - screenPos, null, new Color(0, 105 - NPC.alpha, 105 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4 / 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] != 2)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(255, 50, 50, 0) * NPC.ai[1], NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(255, 50, 50, 0) * NPC.ai[1], NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = DDTextures.限制框.Value;
            if (NPC.localAI[1] != 2 || (NPC.localAI[1] == 2 && NPC.ai[1] <= 7))
                spriteBatch.Draw(texture, new Vector2(Main.LocalPlayer.Center.X, NPC.ai[3]) - Main.screenPosition, null, new Color(255, 100, 100, 0), 0, new Vector2(texture.Width / 2, texture.Height * 0), NPC.scale * (NPC.ai[2] / 500), 0, 0f);
            return false;
        }
        public override bool CheckDead()
        {
            return true;
        }
    }
}