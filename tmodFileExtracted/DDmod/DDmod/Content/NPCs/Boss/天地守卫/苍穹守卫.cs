using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.天地守卫
{
    [AutoloadBossHead]
    public class 苍穹守卫 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/星星守卫2_Glow");
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Guardian : Les");
           //DisplayName.AddTranslation(7, "生命守卫:莱斯");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            DDSystem.HBar(NPC.type, "苍穹守卫", new Vector2(-4007, 0));

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 28000;
            NPC.defense = 12;
            NPC.damage = 80;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(74 * NPC.scale);
            NPC.height = (int)(74 * NPC.scale);
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.苍穹守卫"))
            });
        }
        public override void BossLoot(ref int potionType)
        {
        }
        public override bool ModifyDeathMessage(ref NetworkText customText, ref Color color)
        {
            return true;
        }

        public bool title;
        public bool brothers;
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

        int TL = 0;
        int TLTime = 0;
        public byte[] ManaTextures =
        [
            0,0,0,1,0,0,0,
            0,0,1,0,1,0,0,
            1,1,0,0,0,1,1,
            1,0,0,0,0,0,1,
            0,1,0,1,0,1,0,
            1,0,1,0,1,0,1,
            1,1,0,0,0,1,1,
        ];
        public override void AI()
        {
            Lighting.AddLight(NPC.position, 0.5f, 1f, 2f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            //Main.LocalPlayer.Dplayer().Bossperspective(P.Center - new Vector2(0, 300), 10, false, 0.15F);
            Vector2 vector = P.Center - NPC.Center;
            if (!NPC.AnyNPCs(ModContent.NPCType<大地守卫>()))
            {
                NPC.velocity.Y -= 1;
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
                NPC.velocity.Y -= 1F;
                NPC.timeLeft--;
            }
            else if (NPC.localAI[1] == 2)
            {
                Vector2 PO = P.Center - new Vector2(-300, 0) - NPC.Center;
                float speed = PO.Length() / 20;
                if (NPC.ai[1] <= 7)
                {
                    if (Main.LocalPlayer.position.Y <= NPC.ai[3])
                    {
                        Main.LocalPlayer.velocity.Y = 0.01F;
                        Main.LocalPlayer.position.Y = NPC.ai[3];
                    }
                }
                if (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<大地守卫>())].localAI[1] != 2)
                {
                    NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + 11), 魔力);
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
                        if (NPC.ai[0] >= 300)
                        {
                            NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + (NPC.ai[1] + 12)), 魔力);
                        }
                        if (NPC.ai[0] < 300)
                        {
                            if (NPC.ai[1] == 0 || (NPC.ai[1] >= 2 && NPC.ai[1] <= 4))
                            {
                                NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + (NPC.ai[1] + 12)), 魔力);
                            }
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
                        NPC.boss = false;
                        NPC.velocity.Y -= 0.2F;

                        NPC.ai[1]++;
                        if (NPC.ai[1] == 180)
                        {
                            NPC.Kill(true);
                        }
                    }
                }
            }
            else if (NPC.Dnpc().Stage == 0)
            {
                NPC.timeLeft = 10;
                int ProjType = ModContent.ProjectileType<BossArousalStar>();
                Vector2 PO = P.Center - new Vector2(-300, 300) - NPC.Center;
                NPC.ai[0]++;
                //ai0
                if (NPC.ai[0] < 0)
                {
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                }
                //ai1
                else
                if (NPC.ai[0] < 300)
                {
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    if (NPC.ai[0] % 60 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ProjType, 22, 0, -1, 0, 0.5f);
                    }
                }
                //ai2
                else if (NPC.ai[0] < 900)
                {
                    PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    NPC.rotation += 0.1f;
                    if (NPC.ai[0] % 100 == 0)
                    {
                        float ro = (NPC.rotation % MathHelper.TwoPi) - MathHelper.PiOver2;
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2 + MathHelper.PiOver4);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2 - MathHelper.PiOver4);
                    }

                }
                //ai3
                else if (NPC.ai[0] < 1500)
                {
                    NPC.Dnpc().Bool[0] = true;
                    PO = P.Center + NPC.Dnpc().vector[0];
                    //ManaTextures
                    if (NPC.ai[0] > 980 && NPC.ai[0] < 1430)
                    {
                        //传送
                        if (NPC.ai[0] % 100 == 80)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.Center = PO;
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                        }
                        //后退
                        if (NPC.ai[0] % 100 >= 80)
                        {
                            NPC.velocity = -vector.PerfectNormalize() * 4;
                        }
                        //冲刺
                        if (NPC.ai[0] % 100 == 0)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 5;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.velocity = vector.PerfectNormalize() * 32;
                            NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(300, 500), Main.rand.Next(-300, 300));
                        }
                        if (NPC.ai[0] % 100 >= 30 && NPC.ai[0] % 100 < 80 && NPC.ai[0] % 5 == 0)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7);
                                    Vector2 velocity = (dustPO - (new Vector2(6) / 2));

                                    int Proj = NewDust(PO - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 0.5F);
                                    Main.dust[Proj].velocity = velocity;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                        }
                    }
                    else
                    {
                        if (NPC.ai[0] == 1430)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.Center = P.Center - new Vector2(-500, 0);
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.velocity *= 0.1f;
                        }
                        PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    }
                }
                //ai4
                else if (NPC.ai[0] < 2100)
                {
                    PO = P.Center - new Vector2(-300, 300) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    if (NPC.ai[0] % 20 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center + new Vector2(200, Main.rand.Next(-500, 500)).RotatedBy(-MathHelper.PiOver4), new Vector2(-0.01f, 0.01f), ModContent.ProjectileType<BossMagicStarCircle>(), 25, 0, -1, NPC.whoAmI, 0, 1);

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
                        int D = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(100, 100, 255, 0), Scale: 10.2F);
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
                if (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<大地守卫>())].Dnpc().Stage != 0)
                {
                    NPC.Dnpc().Stage = 2;
                }
                Vector2 PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                float speed = PO.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                NPC.ai[1] = 1;
            }
            else if (NPC.Dnpc().Stage == 2)
            {
                NPC.ai[0]++;
                NPC.dontTakeDamage = true;
                NPC.life = NPC.lifeMax / 2;
                NPC.NPCText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + 10), 魔力);
                if (NPC.ai[2] < 500)
                    NPC.ai[2] += 2;
                if (NPC.ai[0] > 300)
                {
                    NPC.Dnpc().Stage = 3;
                    for (int a = 0; a < 80; a++)
                    {
                        Vector2 v = new Vector2(Main.rand.NextFloat(100, 200), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        int D = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), newColor: new Color(100, 100, 255, 0), Scale: 10.2F);
                        Main.dust[D].velocity = v / 5;
                        Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                        Main.dust[D].customData = 2;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                    }
                    NPC.ai[1] = 0.25f;
                    NPC.ai[0] = -120;
                }
                NPC.ai[3] = NPC.position.Y - NPC.ai[2];
                Vector2 PO = P.Center - new Vector2(-500, 0) - NPC.Center;
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
                    if (Main.player[a].active && !Main.player[a].dead && Main.player[a].position.Y <= NPC.ai[3])
                    {
                        Main.player[a].position.Y = NPC.ai[3];
                        Main.player[a].velocity.Y = 0.01F;
                    }
                }
                int ProjType = ModContent.ProjectileType<BossArousalStar>();
                Vector2 PO = P.Center - new Vector2(-300, 0) - NPC.Center;
                NPC.ai[0]++;
                //ai0
                if (NPC.ai[0] < 0)
                {
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                }
                //ai1
                else
                if (NPC.ai[0] < 300)
                {
                    PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    if (NPC.ai[0] % 60 >= 10 && NPC.ai[0] % 60 <= 30)
                    {
                        if (NPC.ai[0] % 4 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 18, ProjType, 22, 0, -1, 0, 0.5f);
                        }
                    }
                }
                //ai2
                else if (NPC.ai[0] < 900)
                {
                    PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    NPC.rotation += 0.1f;
                    if (NPC.ai[0] > 360 && NPC.ai[0] % 120 == 0)
                    {
                        float ro = (NPC.rotation % MathHelper.TwoPi) - MathHelper.PiOver2;
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2 + MathHelper.PiOver4);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2 - MathHelper.PiOver4);

                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0f, ro);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0f, ro + MathHelper.PiOver2 + MathHelper.PiOver4);
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 10, ProjType, 22, 0, -1, 1, 0f, ro - MathHelper.PiOver2 - MathHelper.PiOver4);
                    }

                }
                //ai3
                else if (NPC.ai[0] < 1500)
                {
                    NPC.Dnpc().Bool[0] = true;
                    PO = P.Center + NPC.Dnpc().vector[0];
                    //ManaTextures
                    if (NPC.ai[0] > 980 && NPC.ai[0] < 1430)
                    {
                        //传送
                        if (NPC.ai[0] % 100 == 80)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.Center = PO;
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                        }
                        //后退
                        if (NPC.ai[0] % 100 >= 80)
                        {
                            NPC.velocity = -vector.PerfectNormalize() * 4;
                        }
                        //冲刺
                        if (NPC.ai[0] % 100 == 0)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 5;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.velocity = vector.PerfectNormalize() * 32;
                            NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(300, 500), Main.rand.Next(-300, 300));
                        }
                        if (NPC.ai[0] % 100 >= 30 && NPC.ai[0] % 100 < 80 && NPC.ai[0] % 5 == 0)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7);
                                    Vector2 velocity = (dustPO - (new Vector2(6) / 2));

                                    int Proj = NewDust(PO - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 0.5F);
                                    Main.dust[Proj].velocity = velocity;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                        }
                    }
                    else
                    {
                        if (NPC.ai[0] == 1430)
                        {
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 3;

                                }
                            }
                            NPC.Center = P.Center - new Vector2(-500, 0);
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                                    Main.dust[Proj].velocity = velocity * 3;
                                    Main.dust[Proj].customData = 1;

                                }
                            }
                            NPC.velocity *= 0.1f;
                        }
                        PO = P.Center - new Vector2(-500, 0) - NPC.Center;
                        float speed = PO.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    }
                }
                //ai4
                else if (NPC.ai[0] < 2100)
                {
                    PO = P.Center - new Vector2(-300, 0) - NPC.Center;
                    float speed = PO.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + PO.PerfectNormalize() * speed) / 21;
                    if (NPC.ai[0] % 15 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(200, 400), Main.rand.Next(-500, 500)), new Vector2(-0.01f, 0), ModContent.ProjectileType<BossMagicStarCircle>(), 25, 0, -1, NPC.whoAmI, 0, 1);

                    }
                }
                else
                {
                    NPC.ai[0] = -120;
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
            }
            NPC.damage = 0;
            DDHelper.MaxandMinF(ref NPC.Dnpc().Times[1], 2.4f, 1.2f);
            NPC.rotation += NPC.velocity.X * 0.01F;
            if (NPC.velocity.X > 0)
            {
                NPC.rotation += Math.Abs(NPC.velocity.Y) * 0.01F;
            }
            else
            {
                NPC.rotation -= Math.Abs(NPC.velocity.Y) * 0.01F;
            }
            return;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
                }
                if (NPC.Dnpc().Stage == 3)
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
                        Color color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                        spriteBatch.Draw(Glow.Value, vector2, null, color * 0.5f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4 * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    }
                }
            }
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4, spriteEffects, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, new Color(155 - NPC.alpha, 155 - NPC.alpha, 0, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * NPC.Dnpc().Times[1] / 4 / 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] != 2)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(0, 100, 255, 0) * NPC.ai[1], NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(0, 100, 255, 0) * NPC.ai[1], NPC.rotation, vector, NPC.scale, spriteEffects, 0f);

            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = DDTextures.限制框.Value;
            if (NPC.localAI[1] != 2 || (NPC.localAI[1] == 2 && NPC.ai[1] <= 7))
                spriteBatch.Draw(texture, new Vector2(Main.LocalPlayer.Center.X, NPC.ai[3]) - Main.screenPosition, null, new Color(0, 100, 255, 0), MathHelper.Pi, new Vector2(texture.Width / 2, texture.Height * 0), NPC.scale * (NPC.ai[2] / 500), 0, 0f);
            return false;
        }
        public override bool CheckDead()
        {
            return true;
        }
    }
}