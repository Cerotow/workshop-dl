using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Tiles.农场;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.Boss.鬼牙
{
    [AutoloadBossHead]
    public class 鬼牙头 : ModNPC
    {
        public static Asset<Texture2D> Head;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Glow2;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Head = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙头");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙头_Glow");
                Glow2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙头_Glow2");
            }
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/鬼牙/鬼牙Texture",
                Scale = 0.5f,
                PortraitScale = 0.8f,
                PortraitPositionXOverride = 30
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);


            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                if (!BuffID.Sets.IsATagBuff[k])
                {
                    NPCID.Sets.SpecificDebuffImmunity[Type][k] = true;
                }
            }
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            DDSystem.HBar(NPC.type, "鬼牙", new Vector2(-32000, 4));

        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.defense = 8;
            NPC.damage = 100;
            NPC.lifeMax = 90000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.value = 12000f;
            NPC.scale = 1.3f;
            NPC.alpha = 255;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = DDSystem.Music(3, "鬼牙");
            }
            //NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.3F;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.鬼牙"))
            ]);
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation + MathHelper.PiOver2;
        }
        public bool title;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2.5f;
            position = Vector2.Zero;
            if (NPC.alpha <= 50)
            {
                position = NPC.Center + new Vector2(0, NPC.height);
            }
            return new bool?(true);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<鬼牙宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<鬼牙纪念章物品>(), 10));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<鬼牙面具>());

            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<鬼牙圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Content.Items.Talisman.鬼牙>(), 4));

            int[] A =
            [
                ModContent.ItemType<鬼牙夺命棒>(),
                ModContent.ItemType<鬼血枪>(),
                ModContent.ItemType<鬼牙夺魂杖>(),
                ModContent.ItemType<鬼仆之唤>(),
            ];
            npcLoot.NormalLoot(1, A);

        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.鬼牙, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int a = 0; a < 100; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 5, 0f, 0f, 0, default, 2.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(3, 8), Main.rand.NextFloat(3, 8)), (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                    }
                    int GoreType = Mod.Find<ModGore>("鬼牙1").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, GoreType, NPC.scale);
                }
            }
        }
        public override bool CheckDead()
        {
            if (NPC.Dnpc().Stage > 2)
            {
                return true;
            }
            else
            {
                NPC.life = 666;
                return false;
            }
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (player.dead && NPC.Distance(player.Center) > 2000)
            {
                NPC.TargetClosest(true);
                player = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || player.dead)
                {
                    NPC.active = false;
                }
            }
            //头
            if (NPC.Dnpc().Stage == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int NPCWhoAmI = NPC.whoAmI;
                    int Length = 49;
                    int NPCWhoAmI2;
                    for (int i = 0; i <= Length; i++)
                    {
                        if (i < Length)
                        {
                            NPCWhoAmI2 = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<鬼牙身>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                            Main.npc[NPCWhoAmI].ai[3] = NPCWhoAmI2;
                        }
                        else
                        {
                            NPCWhoAmI2 = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<鬼牙尾>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        }
                        Main.npc[NPCWhoAmI2].realLife = NPC.whoAmI;
                        //Main.npc[NPCWhoAmI2].Dnpc().Master = NPC.whoAmI;
                        Main.npc[NPCWhoAmI2].ai[1] = NPCWhoAmI;
                        Main.npc[NPCWhoAmI2].ai[2] = i;
                        Main.npc[NPCWhoAmI2].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPCWhoAmI2, 0f, 0f, 0f, 0, 0, 0);
                        NPCWhoAmI = NPCWhoAmI2;
                    }
                }
                NPC.Dnpc().Stage = 1;
                SoundStyle sound = SoundID.Roar;
                sound.Pitch = -1;
                PlaySound(sound, NPC.position);
                NPC.localAI[0] = -10000;
                NPC.netUpdate = true;
            }
            if (NPC.Dnpc().Stage == 1)
            {
                if (NPC.localAI[0] < 120)
                {
                    NPC.dontTakeDamage = true;
                }
                if (NPC.localAI[0] < 0)
                {
                    DDTileDawnSystem.Filter(new Color(220, 0, 25, 255), 1, 0.01F);
                    if (NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体.恐惧缝合体>()))
                    {
                        NPC.alpha -= 20;
                        DDHelper.RotateSpeed(ref NPC.rotation, (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<恐惧缝合体.恐惧缝合体>())].Center - NPC.Center).ToRotation(), 0.1F);
                        NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * 30;
                        if (Main.npc[NPC.FindFirstNPC(ModContent.NPCType<恐惧缝合体.恐惧缝合体>())].getRect().Intersects(NPC.getRect()))
                        {
                            PlaySound(SoundID.NPCDeath1, NPC.Center);
                            Main.npc[NPC.FindFirstNPC(ModContent.NPCType<恐惧缝合体.恐惧缝合体>())].Kill();
                        }
                    }
                    else
                    {
                        int T = -1;
                        for (int a = 0; a < 1000; a++)
                        {
                            Projectile projectile = Main.projectile[a];
                            if (projectile.active && projectile.type == ModContent.ProjectileType<诡异肉块Proj>())
                            {
                                T = projectile.whoAmI;
                            }
                        }
                        if (NPC.localAI[1] > 120)
                        {
                            if (T >= 0)
                            {
                                NPC.alpha -= 20;
                                DDHelper.RotateSpeed(ref NPC.rotation, (Main.projectile[T].Center - NPC.Center).ToRotation(), 0.1F);
                                NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * 30;
                            }
                            else
                            {
                                NPC.localAI[1]++;
                                if (NPC.localAI[1] > 160)
                                {
                                    NPC.alpha += 20;
                                }
                                if (NPC.alpha >= 255)
                                {
                                    NPC.localAI[0] = 0;
                                    NPC.localAI[1] = 0;
                                }
                            }
                        }
                        else
                        {
                            NPC.localAI[1]++;
                        }
                    }
                    DDHelper.MaxandMin(ref NPC.alpha, 255, 0);
                    NPC.netUpdate = true;
                    return;
                }
                NPC.localAI[0]++;
                if (NPC.localAI[0] > 120 && NPC.localAI[0] < 240)
                {
                    if (NPC.target < 0 || NPC.target == 255 || player.dead)
                    {
                        NPC.TargetClosest(true);
                    }
                    float Speed = 30;
                    //移动代码
                    if (NPC.ai[0] < Speed)
                    {
                        NPC.ai[0] += Speed / 60;
                    }
                    else
                    {
                        NPC.ai[0] = Speed;
                    }
                    if ((player.Center - NPC.Center).Length() < 300)
                    {
                        DDHelper.RotateSpeed(ref NPC.rotation, (player.Center - NPC.Center).ToRotation() + MathHelper.Pi + 0.3F, 0.1F);
                    }
                    NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0];
                }
                if (NPC.localAI[0] < 120)
                {
                    return;
                }
            }
            if (NPC.Dnpc().vector[0] == Vector2.Zero)
            {
                NPC.Dnpc().vector[0] = NPC.Center;
            }
            NPC.dontTakeDamage = false;
            if (NPC.alpha >= 20)
            {
                NPC.dontTakeDamage = true;
            }
            if (NPC.localAI[0] < 300)
            {
                NPC.dontTakeDamage = true;
            }
            if (NPC.localAI[2] == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int num = NewDust(NPC.Center, 1, 1, 5, 0f, 0f, 100, default, 1.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2F, 12F);
                }
                NPC.alpha -= 5;
                if (NPC.alpha <= 0)
                {
                    NPC.localAI[2] = 1;
                }
            }
            if (player.dead)
            {
                NPC.velocity.Y++;
            }
            else
            {
                if (NPC.Dnpc().Stage == 1)
                {
                    if (NPC.target < 0 || NPC.target == 255 || player.dead)
                    {
                        NPC.TargetClosest(true);
                    }
                    NPC.ai[1]++;
                    float Speed = 10 + 10 * (1 - (float)NPC.life / NPC.lifeMax);

                    if (NPC.ai[1] > 1200)
                    {
                        NPC.ai[1] = 0;
                    }
                    if (NPC.ai[1] == 600)
                    {
                        SoundStyle sound = SoundID.Roar;
                        sound.Pitch = -1;
                        PlaySound(sound, Main.LocalPlayer.Center);
                    }
                    if (NPC.ai[1] > 600)
                    {
                        for (int r = 0; r < 1; r++)
                        {
                            int A = NewDust(NPC.Center - new Vector2(4) - new Vector2(16, 82).RotatedBy(NPC.rotation + MathHelper.PiOver2), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(220, 0, 25, 0), 0.7F);
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(1), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[A].customData = 0.2F;
                            int B = NewDust(NPC.Center - new Vector2(4) - new Vector2(-16, 82).RotatedBy(NPC.rotation + MathHelper.PiOver2), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(220, 0, 25, 0), 0.7F);
                            Main.dust[B].velocity = new Vector2(Main.rand.NextFloat(1), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[B].customData = 0.2F;
                        }
                        Speed *= 2;
                    }
                    //移动代码
                    if (NPC.ai[0] < Speed)
                    {
                        NPC.ai[0] += Speed / 60;
                    }
                    else
                    {
                        NPC.ai[0] = Speed;
                    }
                    if (NPC.Dnpc().Times[0] <= 0 && NPC.Dnpc().Times[1] > 0)
                    {
                        DDHelper.RotateSpeed(ref NPC.rotation, (player.Center - NPC.Center).ToRotation(), NPC.Dnpc().Times[1]);
                    }
                    if ((player.Center - NPC.Center).Length() < ((NPC.ai[1] <= 600) ? 200 : 400))
                    {
                        NPC.Dnpc().Times[0] = 5 + 5 * ((float)NPC.life / NPC.lifeMax);
                        NPC.Dnpc().Times[1] = 0;
                    }
                    if (NPC.Dnpc().Times[0] > 0)
                    {
                        NPC.Dnpc().Times[0]--;
                    }
                    else
                    {
                        if (NPC.Dnpc().Times[1] < 0.1F + 0.1F * (1 - (float)NPC.life / NPC.lifeMax))
                        {
                            NPC.Dnpc().Times[1] += 0.001f + 0.001F * (1 - (float)NPC.life / NPC.lifeMax);
                        }
                    }
                    NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0];
                    if (NPC.life <= NPC.lifeMax / 4)
                    {
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        NPC.life = NPC.lifeMax / 4;
                        NPC.Dnpc().Stage = 2;
                        NPC.dontTakeDamage = true;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.Dnpc().Stage == 2)
                {
                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 5, false, 0.1F);
                    if (NPC.ai[0] == 0)
                    {
                        SoundStyle sound = SoundID.Roar;
                        sound.Pitch = -1;
                        PlaySound(sound, NPC.position);
                        float PL = 20 - ((Main.LocalPlayer.Center - NPC.Center).Length() / 16);
                        if (PL < 0)
                        {
                            PL = 0;
                        }
                        Main.LocalPlayer.Dplayer().PlayerShake(50, PL);

                    }
                    NPC.velocity = NPC.velocity.PerfectNormalize();
                    NPC.ai[0]++;
                    NPC.life = NPC.lifeMax / 4;
                    NPC.dontTakeDamage = true;
                    if (NPC.ai[0] > 60)
                    {
                        NPC.alpha += 3;
                    }
                    if (NPC.ai[0] > 200)
                    {
                        NPC.ai[0] = 0;
                        NPC.Dnpc().Times[0] = 0;
                        NPC.Dnpc().Times[1] = 0;
                        NPC.Dnpc().Stage = 3;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.Dnpc().Stage == 3)
                {
                    //隐身

                    //Main.dayTime = false;
                    //Main.time = 3600 * 4.5F;
                    if (NPC.Dnpc().Times[1] < 4)
                    {
                        if (NPC.Dnpc().Times[0] < 200 || NPC.Dnpc().Times[0] > 300)
                        {
                            NPC.alpha = (int)(player.Center - NPC.Center).Length() / 2 - 50;
                        }
                    }
                    if (NPC.Dnpc().vector[1] == Vector2.Zero)
                    {
                        NPC.Dnpc().vector[1] = (player.Center - NPC.Center).PerfectNormalize();
                    }
                    NPC.Dnpc().Times[0]++;
                    NPC.Dnpc().Times[2]++;
                    if ((player.Center - NPC.Center).Length() > 400)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int num = NewDust(Main.LocalPlayer.Center - (player.Center - NPC.Center).PerfectNormalize() * 400, 1, 1, ModContent.DustType<光球粒子>(), 0f, 0f, 100, new Color(200, 0, 12), 1.5f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].noLight = true;
                            Main.dust[num].customData = 4;
                            Main.dust[num].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2F, 8F);
                        }
                        if (NPC.Dnpc().Bool[0])
                        {
                            if (NPC.Dnpc().Times[0] >= 400 || (NPC.Dnpc().Times[0] >= 300 && (player.Center - NPC.Center).Length() > 1200))
                            {
                                NPC.Dnpc().Times[1]++;
                                NPC.Dnpc().Bool[0] = false;
                                NPC.Dnpc().Times[0] = -120;
                                NPC.Dnpc().Times[2] = 0;
                                NPC.Dnpc().vector[1] = (player.Center - NPC.Center).PerfectNormalize();
                            }
                        }
                    }
                    float Speed = 20;
                    //移动代码
                    if (NPC.ai[0] < Speed)
                    {
                        NPC.ai[0] += Speed / 60;
                    }
                    else
                    {
                        NPC.ai[0] = Speed;
                    }
                    if (NPC.Dnpc().Times[1] < 3)
                    {
                        if (NPC.Dnpc().Times[0] < 200)
                        {
                            NPC.Center = player.Center - NPC.Dnpc().vector[1].RotatedBy(NPC.Dnpc().Times[2] / 30) * 800;
                            NPC.rotation = (-NPC.Dnpc().vector[1]).RotatedBy(NPC.Dnpc().Times[2] / 30).ToRotation() + MathHelper.PiOver2;
                            if (NPC.target < 0 || NPC.target == 255 || player.dead)
                            {
                                NPC.TargetClosest(true);
                            }
                            if (NPC.Dnpc().Times[0] >= 0 && NPC.Dnpc().Times[0] % 40 == 0 && Main.netMode != 1)
                            {
                                NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.PerfectNormalize() * 3, ModContent.ProjectileType<Projectiles.Boss.Boss鬼牙>(), 20, 1, -1, -1, NPC.whoAmI);
                            }
                        }
                        else if (NPC.Dnpc().Times[0] == 200)
                        {
                            NPC.Dnpc().vector[1] = (player.Center - NPC.Center).PerfectNormalize();

                            if (Main.netMode != 1)
                            {
                                for (int a = -5; a <= 5; a++)
                                {
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(400 * a, -400).RotatedBy(NPC.rotation), (NPC.rotation + MathHelper.PiOver2).ToRotationVector2(), ModContent.ProjectileType<Projectiles.Boss.Boss鬼牙>(), 60, 1, -1, -3, NPC.whoAmI);
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(400 * a, -800).RotatedBy(NPC.rotation), (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 5, ModContent.ProjectileType<Projectiles.Boss.Boss鬼牙>(), 60, 1, -1, -4, NPC.whoAmI);
                                }
                            }
                        }
                        else if (NPC.Dnpc().Times[0] < 300)
                        {
                            NPC.alpha += 10;
                            NPC.ai[0] = 1;
                            NPC.rotation = (NPC.Dnpc().vector[1]).ToRotation();
                            NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0];
                            NPC.Center = player.Center - NPC.Dnpc().vector[1].PerfectNormalize() * 800;
                            if (NPC.target < 0 || NPC.target == 255 || player.dead)
                            {
                                NPC.TargetClosest(true);
                            }
                        }
                        else
                        {
                            NPC.Dnpc().Bool[0] = true;
                            NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0] * 5;
                        }
                    }
                    else if (NPC.Dnpc().Times[1] < 4)
                    {
                        NPC.Center = player.Center - NPC.Dnpc().vector[1].RotatedBy(NPC.Dnpc().Times[2] / 30) * 800;
                        NPC.rotation = (-NPC.Dnpc().vector[1]).RotatedBy(NPC.Dnpc().Times[2] / 30).ToRotation() + MathHelper.PiOver2;
                        if (NPC.Dnpc().Times[0] % 20 == 0 && Main.netMode != 1)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.PerfectNormalize() * 3, ModContent.ProjectileType<Projectiles.Boss.Boss鬼牙>(), 40, 1, -1, -2, NPC.whoAmI);
                        }
                        if (NPC.Dnpc().Times[0] > 300)
                        {
                            NPC.Dnpc().Times[0] = -600;
                            NPC.Dnpc().Times[1]++;
                            NPC.Dnpc().Times[2] = 0;
                            NPC.Dnpc().vector[1] = (player.Center - NPC.Center).PerfectNormalize();
                        }
                    }
                    else
                    {
                        if (NPC.Dnpc().Times[0] < -60)
                        {
                            NPC.alpha -= 5;
                        }
                        else
                        {
                            NPC.alpha += 15;
                        }
                        NPC.velocity = NPC.rotation.ToRotationVector2();

                        if (NPC.Dnpc().Times[0] >= 0)
                        {
                            NPC.Dnpc().Times[0] = 0;
                            NPC.Dnpc().Times[1] = 0;
                            NPC.Dnpc().Times[2] = 0;
                            NPC.Dnpc().vector[1] = (player.Center - NPC.Center).PerfectNormalize();
                        }
                    }
                }
            }
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            if (NPC.alpha > 255)
            {
                NPC.alpha = 255;
            }
            NPC.Dnpc().netUpdate = true;
        }
        public override void DrawEffects(ref Color drawColor)
        {
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot(ref int potionType)
        {
            potionType = 499;
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(36, 240, true);
            player.AddBuff(30, 240, true);
        }
        private bool flies;
        private bool TailSpawned;
        private bool TE;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(TE);
            writer.Write(TailSpawned);
            writer.Write(flies);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            TE = reader.ReadBoolean();
            TailSpawned = reader.ReadBoolean();
            flies = reader.ReadBoolean();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.Dnpc().Stage == 1)
            {
                if (NPC.localAI[0] > 0)
                {
                    if (NPC.localAI[0] < 120)
                    {
                        DDWorld.SunLight = (1F - NPC.localAI[0] / 120F);
                    }
                    else if (NPC.localAI[0] < 180)
                    {
                        DDWorld.SunLight = 0;
                    }
                    else if (NPC.localAI[0] < 300)
                    {
                        DDWorld.SunColor = new Color(100, 0, 15);
                        DDWorld.SunLight = ((NPC.localAI[0] - 180) / 120F);
                    }
                    else
                    {
                        DDWorld.SunColor = new Color(100, 0, 15);
                    }
                }
                else
                {
                    DDWorld.SunLight = 1F - DDTileDawnSystem.FilterValue + 0.01F;
                    DDWorld.SunLightScale = 1F - DDTileDawnSystem.FilterValue + 0.01F;
                }
            }
            if (NPC.Dnpc().Stage == 2)
            {
                DDWorld.SunColor = NPC.GetAlpha(new Color(100, 0, 15));
                SkyDowned.BackgroundColor = NPC.GetAlpha(new Color(100, 0, 15));
                DDWorld.SunColor.R += 20;
                SkyDowned.BackgroundColor.R += 20;
                DDWorld.SunColor.A = 255;
                SkyDowned.BackgroundColor.A = 255;
            }
            if (NPC.Dnpc().Stage == 3)
            {
                DDWorld.SunColor = new Color(20, 0, 0, 255);
                SkyDowned.BackgroundColor = new Color(20, 0, 0, 255);
            }
            spriteBatch.Draw(Head.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, new Vector2(Head.Width() / 2, Head.Height() / 2), NPC.scale, 0, 0f);

            if (NPC.ai[1] > 600)
            {
                spriteBatch.Draw(Head.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), new Color(220, 0, 25, 0), NPC.rotation + MathHelper.PiOver2, new Vector2(Head.Width() / 2, Head.Height() / 2), NPC.scale, 0, 0f);

            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}
