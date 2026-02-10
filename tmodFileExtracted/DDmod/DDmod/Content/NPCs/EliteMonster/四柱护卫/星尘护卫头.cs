using DDmod.Content.Dusts;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Tiles.农场;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.EliteMonster.四柱护卫
{
    [AutoloadBossHead]
    public class 星尘护卫头 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
            
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = Texture+"_Texture",
                Scale = 0.5f,
                PortraitScale = 0.8f,
                PortraitPositionXOverride = 30
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
            

            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            DDSystem.HBar(NPC.type, "四柱/星尘柱", new Vector2(-38, 0), Shield: true);

        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.defense = 8;
            NPC.damage = 150;
            NPC.lifeMax = 2500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.scale = 1.15f;
            NPC.alpha = 0;
            NPC.boss = false;
            NPC.NPCHB().NeedBlood = true;
            if (!Main.dedServ) Music = 34;
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().BossPhysique = true;
            NPC.localAI[2] = 2;
            NPC.Dnpc().Properties.BossLife = 1.325F;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.星尘护卫"))
            ]);
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation + MathHelper.PiOver2;
        }
        public bool title;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;
            return new bool?(true);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
            parameters.MinimumItemDropsCount = 8;
            parameters.MaximumItemDropsCount = 12;
            parameters.ChanceNumerator = 1;
            parameters.ChanceDenominator = 1;
            parameters.MinimumStackPerChunkBase = 2;
            parameters.MaximumStackPerChunkBase = 4;
            parameters.BonusMinDropsPerChunkPerPlayer = 1;
            parameters.BonusMaxDropsPerChunkPerPlayer = 2;
            DropOneByOne.Parameters parameters2 = parameters;
            DropOneByOne.Parameters parameters3 = parameters2;
            DropOneByOne.Parameters parameters4 = parameters3;
            parameters3.BonusMinDropsPerChunkPerPlayer = 2;
            parameters3.BonusMaxDropsPerChunkPerPlayer = 3;
            parameters4.BonusMinDropsPerChunkPerPlayer = 3;
            parameters4.BonusMaxDropsPerChunkPerPlayer = 4;
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(3459, parameters2), new DropOneByOne(3459, parameters3), new DropOneByOne(3459, parameters4), true));
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘遗物>(), 1, 1, 1, true);
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘护卫纪念章>(), 10, 10, 10, true);
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘护卫圣物>(), 0, 0, 1, true);
        }
        public override bool SpecialOnKill()
        {
            bool flag = true;
            for (int i = 0; i < 200; i++)
            {
                if (i != NPC.whoAmI && Main.npc[i].active && (Main.npc[i].type == ModContent.NPCType<星尘护卫头>() || Main.npc[i].type == ModContent.NPCType<星尘护卫身>() || Main.npc[i].type == ModContent.NPCType<星尘护卫尾>()))
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                NPC.boss = true;
                NPC.NPCLoot();
            }
            else
            {
                NPC.NPCLoot();
            }
            return true;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.星尘护卫, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), hit.HitDirection, -1f, 0, new Color(40, 185, 255, 0), 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != 1)
                {
                    for (int I = 0; I < Main.rand.Next(2); I++)
                    {
                        int A = DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, 406, 0);
                        Main.npc[A].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 5);
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int a = 0; a < 100; a++)
                    {
                        int B = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), 1.5f);
                        Main.dust[B].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 5);
                    }
                    for (int A = 1; A <= 3; A++)
                    {
                        int GoreType = Mod.Find<ModGore>("星尘护卫" + A).Type;
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(hit.HitDirection, -1), GoreType, NPC.scale);
                    }
                }
            }
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override void AI()
        {
            Main.player[Main.myPlayer].stardustMonolithShader = true;
            Player player = Main.player[NPC.target];

            if (player.dead && NPC.Distance(player.Center) > 2000)
            {
                NPC.TargetClosest(true);
                player = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || player.dead)
                {
                    bool B = true;
                    for (int A = 0; A < 200; A++)
                    {
                        NPC npc = Main.npc[A];
                        if (npc.active && (npc.type == ModContent.NPCType<星尘护卫头>() || npc.type == ModContent.NPCType<星尘护卫身>() || npc.type == ModContent.NPCType<星尘护卫尾>()))
                        {
                            if (npc.Distance(player.Center) <= 2000)
                            {
                                B = false;
                                break;
                            }
                        }
                    }
                    if(B)
                    {
                        for (int A = 0; A < 200; A++)
                        {
                            NPC npc = Main.npc[A];
                            if (npc.active && (npc.type == ModContent.NPCType<星尘护卫头>() || npc.type == ModContent.NPCType<星尘护卫身>() || npc.type == ModContent.NPCType<星尘护卫尾>()))
                            {
                                npc.active = false;
                            }
                        }
                    }
                }
            }
            if (NPC.localAI[2] < 2)
            {
                NPC.localAI[2] += 0.06F;
            }
            if (!NPC.Dnpc().Bool[4])
            {
                if(NPC.Dnpc().Stage==0)
                {
                    NPC.velocity = new Vector2(1F, -8);
                    Main.player[Main.myPlayer].Dplayer().Bossperspective(NPC.Center-new Vector2(0,500),180,false);
                    int[] Gather = new int[3];
                    Gather[0] = ModContent.NPCType<星尘护卫身>();
                    Gather[1] = ModContent.NPCType<星尘护卫头>();
                    Gather[2] = NPC.type;
                    HealthBar hb = new HealthBar();
                    BossDisplayInfo.SetCustomHealthBarMultiple(hb, Gather);
                }
                if(NPC.Dnpc().Stage==50)
                {
                    NPC.velocity = new Vector2(-12, 0);
                    NPC.ai[1] = -120;
                    int[] Gather = new int[3];
                    Gather[0] = ModContent.NPCType<星尘护卫身>();
                    Gather[1] = ModContent.NPCType<星尘护卫头>();
                    Gather[2] = NPC.type;
                    HealthBar hb = new HealthBar();
                    BossDisplayInfo.SetCustomHealthBarMultiple(hb, Gather);
                }
                if(NPC.Dnpc().Stage==100)
                {
                    NPC.velocity = new Vector2(12, 0);
                    NPC.ai[1] = -240;
                    int[] Gather = new int[3];
                    Gather[0] = ModContent.NPCType<星尘护卫身>();
                    Gather[1] = ModContent.NPCType<星尘护卫头>();
                    Gather[2] = NPC.type;
                    HealthBar hb = new HealthBar();
                    BossDisplayInfo.SetCustomHealthBarMultiple(hb, Gather);
                }
                NPC.rotation = NPC.velocity.ToRotation();
                NPC.Dnpc().Bool[4] = true;
            }
            //头
            if (NPC.Dnpc().Stage == 0|| NPC.Dnpc().Stage == 50|| NPC.Dnpc().Stage == 100)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int NPCWhoAmI = NPC.whoAmI;
                    int Length = 33;
                    if(NPC.Dnpc().Stage == 50 || NPC.Dnpc().Stage == 100)
                    {
                        Length = 20;
                    }
                    int NPCWhoAmI2;
                    for (int i = 0; i <= Length; i++)
                    {
                        if (i < Length)
                        {
                            NPCWhoAmI2 = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<星尘护卫身>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                            Main.npc[NPCWhoAmI].ai[3] = NPCWhoAmI2 + 1;
                        }
                        else
                        {
                            NPCWhoAmI2 = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<星尘护卫尾>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                            Main.npc[NPCWhoAmI].ai[3] = NPCWhoAmI2 + 1;
                        }
                        Main.npc[NPCWhoAmI2].ai[1] = NPCWhoAmI + 1;
                        Main.npc[NPCWhoAmI2].ai[2] = i;
                        Main.npc[NPCWhoAmI2].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPCWhoAmI2, 0f, 0f, 0f, 0, 0, 0);
                        NPCWhoAmI = NPCWhoAmI2;
                    }
                    if (NPC.Dnpc().Stage == 0)
                    {
                        int N = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<星尘护卫头>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[N].Dnpc().Stage = 50;
                        Main.npc[N].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, N, 0f, 0f, 0f, 0, 0, 0);
                    }
                    if (NPC.Dnpc().Stage == 50)
                    {
                        int N = NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<星尘护卫头>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[N].Dnpc().Stage = 100;
                        Main.npc[N].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, N, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                NPC.Dnpc().Stage = 1;
                NPC.localAI[0] = -10000;
                NPC.netUpdate = true;
            }
            if (NPC.ai[3] == 0 || !Main.npc[(int)NPC.ai[3] - 1].active || ((Main.npc[(int)NPC.ai[3] - 1].type != ModContent.NPCType<星尘护卫尾>()) && (Main.npc[(int)NPC.ai[3] - 1].type != ModContent.NPCType<星尘护卫身>())))
            {
                NPC.Kill(true);
                return;
            }
            if (player.dead)
            {
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
                if (NPC.Dnpc().Times[0] <= 0 && NPC.Dnpc().Times[1] > 0)
                {
                    DDHelper.RotateSpeed(ref NPC.rotation, -MathHelper.PiOver2, NPC.Dnpc().Times[1]);
                }
                if (NPC.Dnpc().Times[0] > 0)
                {
                    NPC.Dnpc().Times[0]--;
                }
                else
                {
                    if (NPC.Dnpc().Times[1] < 0.15F)
                    {
                        NPC.Dnpc().Times[1] += 0.0015f;
                    }
                }
                NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0];
            }
            else
            {
                if (NPC.Dnpc().Stage == 1)
                {
                    if (NPC.ai[1]++>180)
                    {
                        NPC.ai[1]=0;
                        NPC.Dnpc().Stage = 2;
                        NPC.netUpdate = true;
                    }
                    float Speed = 15;
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
                        DDHelper.RotateSpeed(ref NPC.rotation, -MathHelper.PiOver2, NPC.Dnpc().Times[1]);
                    }
                    if (NPC.Dnpc().Times[0] > 0)
                    {
                        NPC.Dnpc().Times[0]--;
                    }
                    else
                    {
                        if (NPC.Dnpc().Times[1] < 0.15F)
                        {
                            NPC.Dnpc().Times[1] += 0.0015f;
                        }
                    }
                    NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * NPC.ai[0];
                }
                else
                if (NPC.Dnpc().Stage == 2)
                {
                    if (NPC.target < 0 || NPC.target == 255 || player.dead)
                    {
                        NPC.TargetClosest(true);
                    }
                    NPC.ai[1] ++;
                    float Speed = 15;
                    //移动代码

                    if (NPC.ai[1] <= 120)
                    {
                        if (NPC.ai[0] < Speed)
                        {
                            NPC.ai[0] += Speed / 60;
                        }
                        else
                        {
                            NPC.ai[0] = Speed;
                        }
                        if ((player.Center - NPC.Center).Length() < 400)
                        {
                            NPC.Dnpc().Times[0] = 8;
                            NPC.Dnpc().Times[1] = 0;
                        }
                        if (NPC.Dnpc().Times[0] > 0)
                        {
                            NPC.Dnpc().Times[0]--;
                        }
                        else
                        {
                            if (NPC.Dnpc().Times[1] < NPC.ai[0] / 100)
                            {
                                NPC.Dnpc().Times[1] += NPC.ai[0] / 10000;
                            }
                        }
                        if (NPC.Dnpc().Times[2] < 3&& NPC.ai[1]==100)
                        {
                            SoundStyle sound = SoundID.Roar;
                            sound.Pitch = 0;
                            sound.MaxInstances = 10;
                            PlaySound(sound, NPC.position);
                            NPC.localAI[2] = 1;
                        }
                    }
                    else
                    {
                        if (NPC.Dnpc().Times[2] < 3)
                        {
                            NPC.ai[2] = 22;
                            NPC.ai[0] *= 0.92f;
                            if (NPC.Dnpc().Times[0] > 0)
                            {
                                NPC.Dnpc().Times[0]--;
                            }
                            else
                            {
                                if (NPC.Dnpc().Times[1] < NPC.ai[0] / 100)
                                {
                                    NPC.Dnpc().Times[1] += NPC.ai[0] / 10000;
                                }
                            }
                            if ((player.Center - NPC.Center).Length() < 200)
                            {
                                NPC.Dnpc().Times[0] = 8;
                                NPC.Dnpc().Times[1] = 0;
                            }
                            if (NPC.ai[1] > 160)
                            {
                                NPC.ai[1] = 0;
                                NPC.Dnpc().Times[2]++;
                                if (NPC.Dnpc().Times[2] >= 5)
                                {
                                    NPC.Dnpc().Times[2] = 0;
                                }
                            }
                        }
                        else
                        {
                            NPC.ai[0] *= 0.86f;
                            if (NPC.ai[1] % 20 == 0&& NPC.ai[1]<200)
                            {
                                NPC.NewNPCProj(NPC.Center, NPC.rotation.ToRotationVector2() * 10, ModContent.ProjectileType<Boss星尘光束>(), 60, 0, -1, 1, 20);
                            }
                            NPC.Dnpc().Times[1] = 0.02F;
                            if (NPC.ai[1] > 230)
                            {
                                NPC.ai[1] = 0;
                                NPC.Dnpc().Times[2]++;
                                if (NPC.Dnpc().Times[2] >= 5)
                                {
                                    NPC.Dnpc().Times[2] = 0;
                                }
                            }
                            NPC.Dnpc().Times[0] = 0;

                        }
                    }

                    if (NPC.Dnpc().Times[0] <= 0 && NPC.Dnpc().Times[1] > 0)
                    {
                        DDHelper.RotateSpeed(ref NPC.rotation, (player.Center - NPC.Center).ToRotation(), NPC.Dnpc().Times[1]);
                    }

                    NPC.velocity = NPC.rotation.ToRotationVector2().PerfectNormalize() * (NPC.ai[0] + NPC.ai[2]);
                    NPC.ai[2] *= 0.92f;
                }
            }
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
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            if (NPC.localAI[2] < 2)
            {
                //Main.spriteBatch.Draw(DDTextures.Wire.Value, NPC.Center - screenPos, null, new Color(40, 185, 255, 0) * (2 - NPC.localAI[2]) * 2, NPC.rotation, new Vector2(0, 1), new Vector2(12, 4) * NPC.localAI[2], 0, 0f);
                for (int A = 0; A < 3; A++)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(40, 185, 255, 0) * (2 - NPC.localAI[2]) * 2, NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale * NPC.localAI[2], 0, 0f);
                }
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, Glow.Size()/2, NPC.scale, 0, 0f);
        }
    }
}
