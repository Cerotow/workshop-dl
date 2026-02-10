using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.先祖咒魂;
using DDmod.Content.Items.Boss.天雷怒云;
using DDmod.Content.Items.Boss.海幽浮王;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.农场;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.天雷怒云
{
    [AutoloadBossHead]
    public class 天雷怒云 : ModNPC
    {
        public static int Head;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Pointer;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
            Pointer = ModContent.Request<Texture2D>(Texture+ "_Pointer");
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            DDSystem.HBar(NPC.type, "天雷怒云", new Vector2(-1146, 0));
        }

        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.damage = 75;
            NPC.width = 140;
            NPC.height = 84;
            NPC.defense = 12;
            NPC.lifeMax = 21600;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            SoundStyle sound = SoundID.NPCHit30;
            NPC.HitSound = sound;
            NPC.DeathSound = SoundID.NPCDeath33;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.scale = 1.3F;
            NPC.value = 122000f;
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.Dnpc().Properties.Light= true;
            if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
            NPC.Dnpc().Properties.BossLife = 1.175F;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Rain,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.天雷怒云"))
            });
        }
        public override void BossLoot(ref int potionType)
        {
            potionType = 188;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<天雷怒云宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<天雷怒云纪念章>(), 10));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<天雷怒云面具>());
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<天雷怒云圣物>()));
            //大师掉落物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<怒云之灵>(), 4));

            //特别引用,普通模式
            int[] A = [ModContent.ItemType<雷霆剑>(), ModContent.ItemType<电击枪>(), ModContent.ItemType<风暴召唤杖>()];

            npcLoot.NormalLoot(1,A);
            
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.天雷怒云, -1);
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
            if (NPC.ai[3]==0)
            {
                NPC.ai[3] = Main.rand.NextBool(2)?-1:1;
            }
            Main.raining = true;
            Main.rainTime = DDHelper.Second(600);
            Main.maxRaining = 0.5F;
            Main.windSpeedCurrent = 0.6F;
            Main.windSpeedTarget = 0.6F;
            NPC.TargetClosest();
            void Reset(int AI =0)
            {
                NPC.ai[0] = AI;
                NPC.Dnpc().Times[4] = 0;
                NPC.Dnpc().Times[3] = 0;
                NPC.Dnpc().Bool[4] = false;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
            }
            Lighting.AddLight(NPC.Center, new Vector3(0, 0.66F, 1));
            Player player = Main.player[NPC.target];
            if (!player.active||player.dead||NPC.target==255)
            {
                if(NPC.timeLeft>5)
                NPC.timeLeft = 5;
                NPC.velocity.Y -= 0.4F;
                NPC.velocity.X *= 0.92F;
                return;
            }
                Vector2 vector = player.Center - NPC.Center;
            vector.Y -= 300;
            float R = vector.Length() / 5;
            if (R > 20)
            {
                R = 20;
            }
            NPC.dontTakeDamage = NPC.alpha>0;
            if (NPC.alpha > 0)
            {
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 30, false, 0.2F);
                NPC.alpha -= 1;
                NPC.velocity = Vector2.Zero;

                for (int a = 0; a < 1; a++)
                {
                    int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                    Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                    Main.dust[A].noGravity = true;
                    Main.dust[A].customData = -7 - Main.dust[A].DustAI(10);
                    GlobalDust.DustNPCOwner[A] = NPC.whoAmI;
                }
                if(NPC.alpha==0)
                {
                    SoundStyle sound = SoundID.ForceRoar;
                    sound.Pitch = 0f;
                    PlaySound(sound,NPC.Center);
                    for (int a = 0; a < 80; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 4F);
                        Main.dust[A].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 14);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = -3;
                    }
                }
                return;
            }
            //转换ai,npc.ai[1][2]会重置
            if (NPC.Dnpc().Stage == 0)
            {
                if(NPC.life<=NPC.lifeMax*0.5F)
                {
                    Reset(0);
                    NPC.Dnpc().Stage = 1;
                }
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[1]++;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R) / 21;
                    if (NPC.ai[1] % 4 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, 0), new Vector2(0, 12), ModContent.ProjectileType<Boss电雨>(), 30, 1, -1, 0, 0, Main.rand.NextFloat(1F, 1.5F));
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        Reset(1);
                    }
                }
                else if (NPC.ai[0] == 1)
                {
                    vector = player.Center - NPC.Center;
                    NPC.ai[1]++;
                    //移动,为冲刺做准备
                    if (NPC.ai[1] < 40)
                    {
                        if (vector.Length() < 300)
                        {
                            NPC.velocity *= 0.94F;
                        }
                        else
                        {
                            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R / 4) / 21;
                        }
                        if (NPC.Dnpc().Times[4] < 1)
                        {
                            NPC.Dnpc().Times[4] += 0.1F;
                        }
                        NPC.Dnpc().Times[3] = vector.ToRotation() + MathHelper.PiOver2;
                        NPC.Dnpc().Bool[4] = false;
                    }
                    else
                    //冲刺
                    if (NPC.ai[1] == 40)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 18;
                        NPC.Dnpc().Bool[4] = true;
                    }
                    //冲刺结束
                    else if (NPC.ai[1] >= 80)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2]++;
                        for (int A = 0; A < 18; A++)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(80, 150));
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                        NPC.Dnpc().Bool[4] = false;
                        NPC.velocity = Vector2.Zero;
                    }
                    //正在冲刺
                    else
                    {
                        if (NPC.ai[1] % 3 == 0)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 1, 60);
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                        NPC.Dnpc().Times[4] = 0;
                        NPC.Dnpc().Times[3] = 0;
                    }
                    if (NPC.ai[2] >= 3)
                    {
                        Reset(2);
                    }
                }
                else if (NPC.ai[0] == 2)
                {
                    vector = player.Center - NPC.Center;
                    vector.Y -= 200;
                    NPC.ai[1]++;
                    //移动
                    if (NPC.ai[1] < 40)
                    {
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R/2) / 21;
                    }
                    //最后一雷
                    else if (NPC.ai[1] >= 100)
                    {
                        if (NPC.ai[1] == 100)
                        {
                            for (int A = 0; A < 18; A++)
                            {
                                Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                                NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(150, 200));
                                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                                sound.Pitch = 1f;
                                sound.MaxInstances = 20;
                                sound.Volume = .1f;
                                PlaySound(sound, P);
                            }
                            NPC.velocity = Vector2.Zero;
                        }
                        if (NPC.ai[1] > 100)
                        {
                            Reset(3);
                        }
                    }
                    //移动打雷
                    else if (NPC.ai[1] >= 60)
                    {
                        if (NPC.ai[1] % 2 == 0)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 1, 120);
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R / 4) / 21;
                    }
                }
                else if (NPC.ai[0] == 3)
                {
                    vector = player.Center - NPC.Center;
                    vector.Y -= 450;
                    vector.X += NPC.ai[2];
                    if (NPC.ai[1] == 0)
                    {
                        if (vector.X > 0)
                        {
                            NPC.Dnpc().Bool[3] = true;
                        }
                        else
                        {
                            NPC.Dnpc().Bool[3] = false;
                        }
                    }
                    NPC.ai[1]++;
                    DDHelper.BackAndForth(-500, 500, 10, ref NPC.ai[2], ref NPC.Dnpc().Bool[3]);
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R) / 21;
                    Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                    if (NPC.ai[1] % 10 == 0)
                    {
                        NPC.NewNPCProj(P, new Vector2(0, 12), ModContent.ProjectileType<Boss电雨>(), 30, 1, -1, 0, 0, Main.rand.NextFloat(1F, 1.5F));
                    }
                    if (NPC.ai[1] % 30 == 0)
                    {
                        NPC.NewNPCProj(P, new Vector2(0, 12), ModContent.ProjectileType<Boss电珠>(), 30, 1, -1, Main.rand.NextFloat(MathHelper.TwoPi), 0, Main.rand.Next(70, 80));
                    }
                    if (NPC.ai[1] > 600)
                    {
                        Reset(4);
                    }
                }
                else
                {
                    Reset(0);
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.dontTakeDamage = true;
                NPC.velocity *= 0.96F;
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 30,false,0.2F);
                for (int a = 0; a < 3; a++)
                {
                    int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                    Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                    Main.dust[A].noGravity = true;
                    Main.dust[A].customData = -7 - Main.dust[A].DustAI(10);
                    GlobalDust.DustNPCOwner[A] = NPC.whoAmI;
                }
                if (NPC.ai[0] % 2 == 0)
                {
                    Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(80, 150) * (0.5F + NPC.ai[0] / 300));
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                    sound.Pitch = 1f;
                    sound.MaxInstances = 20;
                    sound.Volume = .1f;
                    PlaySound(sound, P);
                }
                NPC.ai[0]++;
                if(NPC.ai[0]>300)
                {
                    for (int A = 0; A < 24; A++)
                    {
                        Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 1.5F, 0, Main.rand.Next(180, 250));
                        SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                        sound.Pitch = 1f;
                        sound.MaxInstances = 20;
                        sound.Volume = .1f;
                        PlaySound(sound, P);
                    }
                    for (int a = 0; a < 120; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 4F);
                        Main.dust[A].velocity =Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 14);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = -3;
                    }
                    if (Main.netMode!=1)
                    {
                        int a = NewNPCs(NPC.GetSource_FromAI(),NPC.Center,ModContent.NPCType<天雷怒云幻象>(),0);
                        Main.npc[a].velocity.X = 12;
                        a=  NewNPCs(NPC.GetSource_FromAI(),NPC.Center,ModContent.NPCType<天雷怒云幻象2>(),0);
                        Main.npc[a].velocity.X = -12;
                    }
                    Reset(0);
                    NPC.Dnpc().Stage = 2;
                }
            }
            else
            {

                NPC.dontTakeDamage = false;
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[1]++;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R/2) / 21;
                    if (NPC.ai[1] % 4 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, 0), new Vector2(0, 12), ModContent.ProjectileType<Boss电雨>(), 24, 1, -1, 0, 0, Main.rand.NextFloat(1F, 1.5F));
                    }
                    if (NPC.ai[1] % 6 == 0)
                    {
                        Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                        NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 1, 120);
                        SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                        sound.Pitch = 1f;
                        sound.MaxInstances = 20;
                        sound.Volume = .1f;
                        PlaySound(sound, P);
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        Reset(1);
                    }
                }
                else if (NPC.ai[0] == 1)
                {
                    vector = player.Center - NPC.Center;
                    NPC.ai[1]++;
                    //移动,为冲刺做准备
                    if (NPC.ai[1] < 40)
                    {
                        if (vector.Length() < 300)
                        {
                            NPC.velocity *= 0.94F;
                        }
                        else
                        {
                            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R / 4) / 21;
                        }
                        if (NPC.Dnpc().Times[4] < 1)
                        {
                            NPC.Dnpc().Times[4] += 0.1F;
                        }
                        NPC.Dnpc().Times[3] = vector.ToRotation() + MathHelper.PiOver2;
                        NPC.Dnpc().Bool[4] = false;
                    }
                    else
                    //冲刺
                    if (NPC.ai[1] == 40)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 22;
                        for (int A = 0; A < 8; A++)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * Main.rand.Next(500,1400)/100, ModContent.ProjectileType<Boss电珠>(), 30, 1, -1, Main.rand.NextFloat(MathHelper.TwoPi), 0, Main.rand.Next(70, 80));
                        }
                        NPC.Dnpc().Bool[4] = true;
                    }
                    //冲刺结束
                    else if (NPC.ai[1] >= 80)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2]++;
                        for (int A = 0; A < 18; A++)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.NextFloat(60)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(80, 150));
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                        NPC.Dnpc().Bool[4] = false;
                        NPC.velocity = Vector2.Zero;
                    }
                    //正在冲刺
                    else
                    {
                        NPC.Dnpc().Times[4] = 0;
                        NPC.Dnpc().Times[3] = 0;
                    }
                    if (NPC.ai[2] >= 3)
                    {
                        Reset(2);
                    }
                }
                else if (NPC.ai[0] == 2)
                {
                    vector = player.Center - NPC.Center;
                    vector.Y -= 200;
                    NPC.ai[1]++;
                    //移动
                    if (NPC.ai[1] < 40)
                    {
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R/2) / 21;
                    }
                    //最后一雷
                    else if (NPC.ai[1] >= 80)
                    {
                        if (NPC.ai[1] == 80)
                        {
                            for (int A = 0; A < 18; A++)
                            {
                                Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                                NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(150, 200));
                                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                                sound.Pitch = 1f;
                                sound.MaxInstances = 20;
                                sound.Volume = .1f;
                                PlaySound(sound, P);
                            }
                            NPC.velocity = Vector2.Zero;
                        }
                        if (NPC.ai[1] > 100)
                        {
                            Reset(3);
                        }
                    }
                    //移动打雷
                    else
                    {
                        if (NPC.ai[1] % 2 == 0)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 1, 120);
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R /4) / 21;
                    }
                }
                else if (NPC.ai[0] == 3)
                {
                    vector = player.Center - NPC.Center;
                    vector.Y -= 450;
                    vector.X += NPC.ai[2];
                    if (NPC.ai[1] == 0)
                    {
                        if (vector.X > 0)
                        {
                            NPC.Dnpc().Bool[3] = true;
                        }
                        else
                        {
                            NPC.Dnpc().Bool[3] = false;
                        }
                    }
                    NPC.ai[1]++;
                    DDHelper.BackAndForth(-500, 500, 10, ref NPC.ai[2], ref NPC.Dnpc().Bool[3]);
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R) / 21;
                    Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                    if (NPC.ai[1] % 15 == 0)
                    {
                        NPC.NewNPCProj(P, new Vector2(0, 12), ModContent.ProjectileType<Boss电雨>(), 30, 1, -1, 0, 0, Main.rand.NextFloat(1F, 1.5F));
                    }
                    if (NPC.ai[1] > 600)
                    {
                        Reset(4);
                    }
                }
                else
                {
                    Reset(0);
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
                for (int i = 0; i < 200; i++)
                {
                    int A= NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0,0, newColor: new Color(74, 189, 226, 0), Scale: 1.6F);
                    Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, 10));
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Main.rand.Next([11,12,13]);
                    for(int a=0;a<8;a++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), Main.rand.NextFloat(NPC.height)), Vector2.Zero, GoreType, NPC.scale);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 180;
            NPC.frameCounter++;
            if (NPC.Dnpc().Bool[4])
            {
                NPC.frame.X = 180;
                NPC.frame.Y = frameHeight * ((int)(NPC.frameCounter / 6) % 3);
            }
            else
            {

                NPC.frame.X = 0;
                NPC.frame.Y = frameHeight * ((int)(NPC.frameCounter / 6) % Main.npcFrameCount[Type]);
            }
            //Main.NewText(((int)NPC.localAI[0] / 8));
        }
        Vector4[] vectors = new Vector4[4];
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Color color = NPC.GetAlpha(new Color(0,255,255));
            color.A = 0;
            if (!NPC.IsABestiaryIconDummy)
            {
                drawColor = NPC.GetAlpha(drawColor);
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle rectangle;
            if (NPC.Dnpc().Times[4] > 0)
            {
                rectangle = new Rectangle(0, Pointer.Height() / 6 * ((int)(NPC.frameCounter / 6) % 6), Pointer.Width(), Pointer.Height() / 6);
                spriteBatch.Draw(Pointer.Value, NPC.Center - screenPos, rectangle, Color.White * NPC.Dnpc().Times[4], NPC.Dnpc().Times[3], rectangle.Size() / 2, NPC.scale, sprite, 0f);
            }
            rectangle = NPC.frame;
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                Vector2 vector = NPC.oldPos[a] + NPC.Size / 2;
                spriteBatch.Draw(texture, vector - screenPos, rectangle, color * 0.4f * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, rectangle, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            return false;
        }
    }
}