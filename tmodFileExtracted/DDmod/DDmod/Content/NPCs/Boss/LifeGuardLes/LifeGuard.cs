using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.LifeGuardLes
{
    [AutoloadBossHead]
    public class LifeGuard : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/LifeGuardLes/大地光效");
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            DDSystem.HBar(NPC.type, "生命守卫", new Vector2(-47, 0));
            /*
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/LifeGuardLes/LifeGuardTexture",
                Scale = 0.6f,
                PortraitScale = 0.8f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
            */
        }
        public override void SetDefaults()
        {
            int Strengthen = (NPCDowned.downedLifeGuard ? 1 : 0) + (NPCDowned.downedLifeGuard2 ? 2 : 0);
            NPC.aiStyle = -1;
            NPC.lifeMax = 800 + (650 * Strengthen);
            NPC.damage = 15 + 15 * (Strengthen);
            NPC.defense = 12;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(48 * NPC.scale);
            NPC.height = (int)(48 * NPC.scale);
            NPC.scale = 1f;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ) Music = DDSystem.Music(2, "星心守卫");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.buffImmune[24] = true;
            NPC.NPCHB().NeedBlood = true;
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().Properties.Stone = true;
            if(NPCDowned.downedLifeGuard)
            {
                NPC.Dnpc().Properties.BossLife = 1.05F;
            }
            if(NPCDowned.downedLifeGuard2)
            {
                NPC.Dnpc().Properties.BossLife = 1.1F;
            }
        }


        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1;
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

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.LifeGuard"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBagByCondition(new downedLifeGuard2(), ModContent.ItemType<LifeGuardTreasureBag>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeGuardTrophyItem>(), 10));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<LifeGuardMask>());
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LifeGuardRelic>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<LifeAmulet>(), 4));

            npcLoot.SpecialLoot(ModContent.ItemType<护心晶盾>(), 2);
            //普通模式
            int[] A = new int[4];
            A[0] = ModContent.ItemType<HeartBow>();
            A[1] = ModContent.ItemType<HeartStaffItem>();
            A[2] = ModContent.ItemType<HeartSword>();
            A[3] = ModContent.ItemType<HeartWhipItem>();
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);

            npcLoot.NormalLoot(1, 29, 1, 3);

        }
        public class downedLifeGuard2 : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return NPCDowned.downedLifeGuard2;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return null;
            }
        }
        public override void BossLoot(ref int potionType)
        {
        }
        public override void OnKill()
        {
            if (!NPCDowned.downedLifeGuard)
            {
                NPCDowned.downedLifeGuard = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            else if (!NPCDowned.downedLifeGuard2)
            {
                NPCDowned.downedLifeGuard2 = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            else
            {
                SetEventFlagCleared(ref NPCDowned.downedLifeGuard3, -1);
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;

        }
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
        int TL = 0;
        int TLTime = 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.position, 2f, 0.5f, 0.5f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || P.Distance(NPC.Center) > 300)
            {
                NPC.TargetClosest(true);
                if (P.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
            }
            //知道玩家的方向
            Vector2 vector = Vector2.Subtract(P.Center, NPC.Center);
            vector.DirectPerfectNormalize();
            vector *= 5;
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity += -vector / 5;
                return;
            }
            //如果距离玩家太远直接脱战
            //开始动画
            if (NPC.localAI[1] == 0)
            {
                NPC.localAI[0]++;
                NPC.ai[2] = 1.5f;
                if (NPC.localAI[0] > 240)
                {
                    NPC.localAI[1] = 1;
                    NPC.localAI[0] = 0;
                }
                else if (NPC.localAI[0] < 200)
                {
                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 2, false, 0.05f);
                }
                NPC.rotation = NPC.velocity.X * 0.05f;
                Vector2 vector2 = Vector2.Subtract(P.Center + new Vector2(300 * P.direction, 0), NPC.Center);
                if (vector2 != Vector2.Zero) vector2.Normalize();
                NPC.velocity = (NPC.velocity * 20 + vector2 * 10) / 21;
                NPC.dontTakeDamage = true;
                return;
            }
            //结束动画
            if (NPC.localAI[1] == 2)
            {
                if (NPC.ai[2] > 1) NPC.ai[2] -= 0.1f;
                NPC.damage = 0;
                NPC.rotation = NPC.velocity.X * 0.05f;
                Color color = new Color(255, 0, 0);
                Vector2 vector2 = Vector2.Subtract(P.Center - new Vector2(0, 100), NPC.Center);
                float SP = vector2.Length() / 10;
                vector2.DirectPerfectNormalize();

                NPC.velocity = (NPC.velocity * 20 + vector2 * SP) / 21;

                TLTime++;
                void T()
                {
                    NPC.localAI[0]++;
                    TLTime = 0;
                }
                if (NPCDowned.downedLifeGuard3)
                {
                    if (NPC.localAI[0] < 5&& NPC.localAI[0]!=4)
                    {
                        NPC.localAI[0] = 5;
                        NPC.NPCLoot();
                    }
                }
                if(TLTime > 300 && NPC.localAI[0] <4)
                {
                    T();
                    if (NPC.localAI[0] == 4)
                    {
                        if (!NPCDowned.downedLifeGuard)
                        {
                            if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue4"));

                            NPCLoader.OnKill(NPC);

                        }
                        else if (!NPCDowned.downedLifeGuard2)
                        {
                            if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue8"));

                            NPCLoader.OnKill(NPC);

                        }
                        else
                        {
                            if (!NPCDowned.downedLifeGuard3)
                            {
                                if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                    Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue12"));
                            }
                            NPC.NPCLoot();
                        }
                    }
                }
                if (NPC.localAI[0]==0)
                {
                    Text = Main.LocalPlayer.name;
                }

                if (NPC.localAI[0] == 1)
                {
                    if (!NPCDowned.downedLifeGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue");
                    }
                    else if (!NPCDowned.downedLifeGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue5");
                    }
                    else
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue9");
                    }
                }
                if (NPC.localAI[0] == 2)
                {
                    if (!NPCDowned.downedLifeGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue2");
                    }
                    else if (!NPCDowned.downedLifeGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue6");
                    }
                    else
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue10");
                    }
                }
                if (NPC.localAI[0] == 3)
                {
                    if (!NPCDowned.downedLifeGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue3");

                    }
                    else if (!NPCDowned.downedLifeGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue7");
                    }
                    else
                    {
                        if (!NPCDowned.downedLifeGuard3)
                        {
                            Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue11");
                        }
                    }
                }
                if (NPC.localAI[0] >= 4)
                {
                    Text = "";
                    NPC.alpha++;
                    if (NPC.alpha > 255) NPC.Kill(false);
                }
                if (NPC.localAI[0] >= 5)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue13");
                }
                NPC.NPCText(Text, 生命);
                return;
            }
            //切换二阶段
            if (NPC.life <= NPC.lifeMax / 2 && NPC.ai[3] == 0)
            {
                NPC.ai[3]++;
                for (int i = 0; i < 30; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, DustID.HeartCrystal, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 0, default, 1f);
                }
            }
            NPC.dontTakeDamage = false;
            //如果有仆从存活
            if (AnyNPCs(ModContent.NPCType<LifeServant>()))
            {
                //遍历一遍知道数量
                int Proj = 0;
                for (int T = 0; T < 200; T++)
                {
                    if (Main.npc[T].type == ModContent.NPCType<LifeServant>() && Main.npc[T].active)
                    {
                        if (Main.npc[T].ai[0] == NPC.whoAmI)
                        {
                            Proj++;
                        }
                    }
                }
                //调整旋转速度
                float U = 0.05f / (Proj * 0.1f);
                if (U > 0.2f)
                {
                    U = 0.2f;
                }
                NPC.localAI[2] += U;
            }

            //调整碰撞箱
            //NPC.width = (int)(48 * NPC.scale * 1.15f * NPC.ai[2]);
            //NPC.height = (int)(48 * NPC.scale * 1.15f * NPC.ai[2]);
            NPC.damage = 0;
            int W = (int)(NPC.width * 1.1f * NPC.ai[2]);
            int H = (int)(NPC.height * 1.1f * NPC.ai[2]);

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
            
            int ProjType = ModContent.ProjectileType<BossHeart>();
            int damage = 6 + (NPCDowned.downedLifeGuard ? 3 : 0) + (NPCDowned.downedLifeGuard2 ? 3 : 0);
            //靠近玩家才启用AI
            if (Vector2.Subtract(P.Center, NPC.Center).Length() < 800 || NPC.ai[1] >= 300)
            {
                NPC.ai[1]++;
            }
            else
            {
                NPC.rotation = NPC.velocity.X * 0.05f;
                NPC.velocity = (NPC.velocity * 20 + vector * 5) / 21;
                return;
            }
            //AI
            if (NPC.ai[1] < 300)
            {
                //发射！
                if (NPC.ai[2] > 1) NPC.ai[2] -= 0.1f;
                NPC.rotation = NPC.velocity.X * 0.05f;
                NPC.velocity = (NPC.velocity * 20 + vector) / 21;

                if (NPC.ai[3] == 0)
                {
                    if (NPC.ai[1] % (Main.masterMode ? (90 - (NPCDowned.downedLifeGuard ? 30 : 0) - (NPCDowned.downedLifeGuard2 ? 30 : 0)) : 120 - (NPCDowned.downedLifeGuard ? 30 : 0) - (NPCDowned.downedLifeGuard2 ? 30 : 0)) == 0)
                    {
                        if (NPC.ai[1] < 240)
                        {
                            NPC.NewNPCProj(NPC.Center, vector * 2, ProjType, damage, 1);

                            NPC.velocity = vector * (Main.masterMode ? -1 : -2);
                        }
                        else
                        {
                            int Proj = 0 + (NPCDowned.downedLifeGuard ? 1 : 0) + (NPCDowned.downedLifeGuard2 ? 1 : 0);
                            for (int i = -Proj; i <= Proj; i++)
                            {
                                Vector2 projDirection = vector.RotatedBy(0.2f * i + Main.rand.NextFloat(-0.1F, 0.1F));
                                NPC.NewNPCProj(NPC.Center, projDirection * 1.25F, ProjType, damage, 1);
                            }
                            NPC.velocity = vector * (Main.masterMode ? -2.5F : -5);
                        }

                    }
                }
                else
                {
                    if (NPC.ai[1] % (Main.masterMode ? (50 - (NPCDowned.downedLifeGuard ? 15 : 0) - (NPCDowned.downedLifeGuard2 ? 15 : 0)) : 60 - (NPCDowned.downedLifeGuard ? 15 : 0) - (NPCDowned.downedLifeGuard2 ? 15 : 0)) == 0)
                    {
                        if (NPC.ai[1] < 100)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Vector2 projDirection = Utils.RotatedBy(vector, Main.rand.NextFloat(-0.2F, 0.2F), default);
                                NPC.NewNPCProj(NPC.Center, projDirection * 1.25F, ProjType, damage, 1);
                            }

                            NPC.velocity = vector * (Main.masterMode ? -0.5F : 1);
                        }
                        else if (NPC.ai[1] <= 120)
                        {
                            int Proj = 1 + (NPCDowned.downedLifeGuard ? 1 : 0) + (NPCDowned.downedLifeGuard2 ? 1 : 0);
                            for (int i = -Proj; i <= Proj; i++)
                            {
                                Vector2 projDirection = Utils.RotatedBy(vector, 0.2f * i + Main.rand.NextFloat(-0.1F, 0.1F), default);
                                NPC.NewNPCProj(NPC.Center, projDirection * 1.25F, ProjType, damage, 1);
                            }
                            NPC.velocity = vector * (Main.masterMode ? -1.5F : 3);
                        }

                    }
                }
            }
            else
            {
                if (!NPCDowned.downedLifeGuard)
                {
                    NPC.ai[1] = 0;
                }
                //冲刺！
                if (Main.masterMode ? NPC.ai[2] < 3 : NPC.ai[2] < 2) NPC.ai[2] += 0.1f;

                if (!NPCDowned.downedLifeGuard2)
                {
                    if (NPC.ai[1] < 440)
                    {
                        NPC.rotation = NPC.velocity.X * 0.05f;
                        vector = Vector2.Subtract(P.Center - new Vector2(400 * NPC.direction, 400), NPC.Center);
                        if (vector.Length() < 60 && NPC.ai[1] < 360) NPC.ai[1] = 360;
                        if (vector != Vector2.Zero) vector.Normalize();
                        vector *= 25;
                        NPC.velocity = (NPC.velocity * 20 + vector) / 21;
                    }
                    if (NPC.ai[1] == 440)
                    {
                        NPC.velocity = vector * 4;
                        NPC.rotation = Vector2.Subtract(P.Center, NPC.Center).ToRotation() - MathHelper.Pi / 2;
                    }
                    if (NPC.ai[1] < 600)
                    {
                        if (Vector2.Subtract(P.Center, NPC.Center).Length() > 800 && NPC.ai[1] > 500) NPC.ai[1] = 600;
                        if (NPC.ai[1] > 440)
                        {
                            if (NPC.ai[1] % 60 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<LifeWarrior>());
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.ai[1] = 0;
                    }
                }
                else
                {
                    if (NPC.ai[1] < 420)
                    {
                        NPC.rotation = NPC.velocity.X * 0.05f;
                        vector = Vector2.Subtract(P.Center - new Vector2(600 * NPC.direction, 0), NPC.Center);
                        if (vector.Length() < 60 && NPC.ai[1] < 360) NPC.ai[1] = 360;
                        if (vector != Vector2.Zero) vector.Normalize();
                        vector *= 25;
                        NPC.velocity = (NPC.velocity * 20 + vector) / 21;
                    }
                    else if (NPC.ai[1] < 440)
                    {
                        NPC.velocity = -vector * 3;
                        NPC.rotation = Vector2.Subtract(P.Center, NPC.Center).ToRotation() - MathHelper.Pi / 2;
                    }
                    if (NPC.ai[1] == 440)
                    {
                        NPC.velocity = vector * 6;
                        NPC.rotation = Vector2.Subtract(P.Center, NPC.Center).ToRotation() - MathHelper.Pi / 2;
                        if (NPC.ai[3]==1)
                        {
                            byte[] LifeTextures =
                [
                    0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < LifeTextures.Length; a++)
                                {
                                    if (LifeTextures[a] == 1)
                                    {
                                        Vector2 vector2 = new Vector2(a % 11, a / 11).RotatedBy(NPC.rotation);
                                        Vector2 velocity = (vector2 - (new Vector2(10).RotatedBy(NPC.rotation) / 2));
                                       DNPC.NewNPCProj(NPC, NPC.Center, velocity * 2, ProjType, damage, 0, -1, 0, 0);
                                    }
                                }
                            }
                        }
                    }
                    if (NPC.ai[1] < 600)
                    {
                        if (Vector2.Subtract(P.Center, NPC.Center).Length() > 800 && NPC.ai[1] > 500) NPC.ai[1] = 600;
                        if (NPC.ai[1] > 440)
                        {
                            if (NPC.ai[1] % 60 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<LifeWarrior>());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (NPC.ai[0] >= 3)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[0] = 0;
                        }
                        else
                        {
                            NPC.ai[1] = 300;
                            NPC.ai[0]++;
                        }
                    }

                }
            }
            //召唤大地仆从
            if (NPC.ai[1] % 300 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int a = 0; a < 3 + (NPCDowned.downedLifeGuard ? 1 : 0) + (NPCDowned.downedLifeGuard2 ? -2 : 0); a++)
                    {
                        NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<LifeServant>(), NPC.whoAmI, NPC.whoAmI);
                    }
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.pick > 0)
            {
                modifiers.SourceDamage *= item.pick / 10;
            }
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
                if (NPC.localAI[1] == 1)
                {
                    NPC.localAI[1] = 2;
                }
                NPC.dontTakeDamage = true;
                if (NPC.Dnpc().Deathrattle)
                {
                    NPC.life = 5;
                }
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
                        spriteBatch.Draw(Glow.Value, vector2, null, color * 0.5f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    }
                }
            }
            Vector2 vector = new Vector2(texture.Width / 4, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(Glow.Value, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, null, new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2], spriteEffects, 0f);
            spriteBatch.Draw(Glow.Value, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, null, new Color(0, 105 - NPC.alpha, 105 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] / 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] != 2) spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, new Rectangle?(NPC.life <= NPC.lifeMax / 2 ? new Rectangle(texture.Width / 2, NPC.frame.Y, texture.Width / 2, texture.Height / 5) : new Rectangle(0, NPC.frame.Y, texture.Width / 2, texture.Height / 5)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);

            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position- screenPos, null, Color.White*0.5F, 0, Vector2.Zero, NPC.Size/2* 1.1f * NPC.ai[2], spriteEffects, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override bool CheckDead()
        {
            return true;
        }
    }
}