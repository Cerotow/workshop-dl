using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.Boss特殊;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Projectiles.Boss;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.StarGuardBulan
{
    [AutoloadBossHead]
    public class StarGuard : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> asset;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/StarGuardBulan/星辰光效");
            asset = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Whip/Star");
        }
        public int ColorDust(Color color)
        {
            if (color == new Color(0, 7, 255))
            {
                return ModContent.DustType<星星粒子>();
            }
            return -1;
        }
        public void NDust(Texture2D texture, Vector2 Position, float Speed, float Scale)
        {
            Color[] colors = DDHelper.GetColors(texture);
            NPC.netUpdate = true;
            for (int i = 0; i < colors.Length; i++)
            {
                float x = i % texture.Width;
                float y = i / texture.Width;
                int DustPos = ColorDust(colors[i]);
                if (DustPos >= 0)
                {
                    Dust dust = Main.dust[NewDust(Position + (new Vector2(x, y) - texture.Size() / 2), 1, 1, ModContent.DustType<星星粒子>(), 0, 0, 0, default, Scale)];
                    //dust.customData = true;
                    dust.velocity = ((new Vector2(x, y) - texture.Size() / 2)) * Speed;
                    dust.customData = 3;
                }
            }
        }
        public void Data(Vector2 vector, float Speed, float Scale)
        {
            if (Main.netMode != 2)
            {
                NDust(asset.Value, vector, Speed, Scale);
            }
        }
        public override void SetStaticDefaults()
        {
            DDSystem.HBar(NPC.type, "魔力守卫", new Vector2(-47, -2), "", true);
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            int Strengthen = (NPCDowned.downedStarGuard ? 1 : 0) + (NPCDowned.downedStarGuard2 ? 2 : 0);
            NPC.aiStyle = -1;
            NPC.lifeMax = 800 + (650 * Strengthen);
            NPC.damage = 12* Strengthen;
            NPC.defense = 2;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(58);
            NPC.height = (int)(58);
            NPC.scale = 1.3f;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ) Music = DDSystem.Music(2, "星心守卫");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.buffImmune[24] = true;
            if (!AnyNPCs(ModContent.NPCType<ServantOfTheStars>()))
            {
                NPC.NPCHB().HealthNPCType = new int[1];
                NPC.NPCHB().HealthNPCType[0] = ModContent.NPCType<ServantOfTheStars>();
            }
            NPC.NPCHB().multiNPCLifeMax = 0;
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().BossPhysique = true;
            NPC.Dnpc().Properties.Stone = true;
            if (NPCDowned.downedStarGuard)
            {
                NPC.Dnpc().Properties.BossLife = 1.05F;
            }
            if (NPCDowned.downedStarGuard2)
            {
                NPC.Dnpc().Properties.BossLife = 1.1F;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.StarGuard"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            //专家掉落
            npcLoot.Add(ItemDropRule.BossBagByCondition(new downedStarGuard2(), ModContent.ItemType<StarGuardTreasureBag>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarguardTrophyItem>(), 10));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<StarGuardMask>());
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<StarguardRelic>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<StarAmulet>(), 4));
            npcLoot.SpecialLoot(ModContent.ItemType<星辰炮>(), 2);
            //普通模式
            int[] A = new int[4];
            A[0] = ModContent.ItemType<MagicStarSwordItem>();
            A[1] = ModContent.ItemType<StarwandItem>();
            A[2] = ModContent.ItemType<MagicStarStaff>();
            A[3] = ModContent.ItemType<MagicStarBowItem>();
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);
        }
        public class downedStarGuard2 : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return NPCDowned.downedStarGuard2;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return Language.GetTextValue("Mods.DDmod.NPCBestiary.StarGuard");
            }
        }
        public override void OnKill()
        {
            if (!NPCDowned.downedStarGuard)
            {
                NPCDowned.downedStarGuard = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            else if (!NPCDowned.downedStarGuard2)
            {
                NPCDowned.downedStarGuard2 = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            else
            {
                SetEventFlagCleared(ref NPCDowned.downedStarGuard3, -1);

            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public void Opening()
        {
            Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 5, false, 0.1f);
            NPC.localAI[2]++;
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            vector.Y -= 200;
            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 10) / 21;
            if (vector.Length() < 50 || NPC.localAI[2] > 180)
            {
                NPC.localAI[1] = 1;
                NPC.localAI[2] = 0;
            }
            NPC.rotation += 0.08f;
            //星星能量体大小
            if (NPC.ai[2] < 1)
            {
                NPC.ai[2] = 1;
            }
            //NPC.damage = 0;
        }
        Vector2 PlayerC;
        int TLTime = 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.position, 0f, 0.25f, 1f);
            Player player = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active || player.Distance(NPC.Center) > 300)
            {
                NPC.TargetClosest(true);
                if (player.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
            }
            if (player.dead)
            {
                NPC.velocity.Y -= 0.4f;
                if ((player.Center - NPC.Center).Length() > 3000)
                {
                    NPC.active = false;
                }
                return;
            }
            Lighting.AddLight(player.position, 0f, 0.25f, 1f);
            Vector2 vector = player.Center - NPC.Center;

            if (NPC.localAI[1] == 0)
            {
                Opening();
                return;
            }
            NPC.localAI[2] += 0.1f;
            //结束动画
            if (NPC.localAI[1] == 2)
            {
                if (NPC.ai[2] > 1) NPC.ai[2] -= 0.1f;
                NPC.damage = 0;
                NPC.rotation += 0.08f;
                Color color = new Color(0, 100, 255);
                Vector2 vector2 = Vector2.Subtract(player.Center - new Vector2(0, 100), NPC.Center);
                if (vector2 != Vector2.Zero) vector2.Normalize();
                NPC.velocity = (NPC.velocity * 20 + vector2 * 30) / 21;

                TLTime++;
                void T()
                {
                    NPC.localAI[0]++;
                    TLTime = 0;
                }
                if (NPCDowned.downedStarGuard3)
                {
                    if (NPC.localAI[0] < 5 && NPC.localAI[0] != 4)
                    {
                        NPC.localAI[0] = 5;
                        NPC.NPCLoot();
                    }
                }
                if (TLTime > 300 && NPC.localAI[0] < 4)
                {
                    T();
                    if (NPC.localAI[0] == 4)
                    {
                        if (!NPCDowned.downedStarGuard)
                        {
                            if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue4"));

                            NPCLoader.OnKill(NPC);

                        }
                        else if (!NPCDowned.downedStarGuard2)
                        {
                            if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue8"));

                            NPCLoader.OnKill(NPC);

                        }
                        else
                        {
                            if (!NPCDowned.downedStarGuard3)
                            {
                                if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                    Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue12"));
                            }
                            NPC.NPCLoot();
                        }
                    }
                }
                if (NPC.localAI[0] == 0)
                {
                    Text = Main.LocalPlayer.name;
                }

                if (NPC.localAI[0] == 1)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue");
                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue5");
                    }
                    else
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue9");
                    }
                }
                if (NPC.localAI[0] == 2)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue2");
                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue6");
                    }
                    else
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue10");
                    }
                }
                if (NPC.localAI[0] == 3)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue3");

                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue7");
                    }
                    else
                    {
                        if (!NPCDowned.downedStarGuard3)
                        {
                            Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue11");
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
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue13");
                }
                NPC.NPCText(Text, 魔力);
                return;
                if (NPCDowned.downedStarGuard3)
                {
                    if (NPC.localAI[0] < 360)
                    {
                        NPC.localAI[0] = 360;
                    }
                }
                if (NPC.localAI[0] == 60)
                {
                    CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Main.LocalPlayer.name, true, false);
                }
                if (NPC.localAI[0] == 160)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue"), true, false);
                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue5"), true, false);
                    }
                    else
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue9"), true, false);
                    }
                }
                if (NPC.localAI[0] == 260)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue2"), true, false);
                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue6"), true, false);
                    }
                    else
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue10"), true, false);
                    }
                }
                if (NPC.localAI[0] == 360)
                {
                    if (!NPCDowned.downedStarGuard)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue3"), true, false);
                        if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                            Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue4"));

                        NPCLoader.OnKill(NPC);

                    }
                    else if (!NPCDowned.downedStarGuard2)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue7"), true, false);
                        if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                            Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue8"));

                        NPCLoader.OnKill(NPC);

                    }
                    else
                    {
                        if (!NPCDowned.downedStarGuard3)
                        {
                            CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue11"), true, false);
                            if (!Main.dedServ && !ModContent.GetInstance<DDConfigServer>().ForceMechanism)
                                Main.NewText(Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue12"));
                        }
                        else
                        {
                            CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue13"), true, false);
                        }
                        NPC.NPCLoot();
                    }
                }
                if (NPC.localAI[0] >= 360)
                {
                    NPC.alpha++;
                    if (NPC.alpha > 255) NPC.active = false;
                }
                return;
            }
            int damage = 10 - (Main.expertMode ? 4 : 0) - (Main.masterMode ? 1 : 0);
            if (!NPCDowned.downedStarGuard)
            {
                AI1(player, vector, damage);
            }
            else if (!NPCDowned.downedStarGuard2)
            {
                damage = (int)(damage * 1.5F);
                AI2(player, vector, damage);
            }
            else
            {
                damage = (int)(damage * 2F);
                AI3(player, vector, damage);
            }

        }
        //第一阶段
        public void AI1(Player player, Vector2 vector, int damage)
        {
            NPC.ai[0]++;
            NPC.damage = 0;
            if ((float)NPC.life / NPC.lifeMax <= 0.5F && NPC.Dnpc().Stage==0)
            {
                NPC.Dnpc().Stage = 1;
                NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] <= 600)
            {
                vector.Y -= 500;
                if (NPC.ai[0] % 100 >= 70)
                {
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                }
                if (NPC.ai[0] % 100 == 99)
                {
                    for (float A = 0; A < MathHelper.TwoPi - MathHelper.PiOver4; A += MathHelper.PiOver4)
                    {
                        if (A != 0 && A != MathHelper.PiOver4 * 2 && A != MathHelper.PiOver4 * 5 && Main.netMode != 1)
                        {
                            Vector2 projDirection = Utils.RotatedBy(new Vector2(-1, -1), A + NPC.rotation, default);
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + projDirection * 10, projDirection * 4, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI);
                        }
                    }
                }
                if (NPC.ai[2] > 1F)
                {
                    NPC.ai[2] -= 0.05F;
                }
            }
            else if (NPC.ai[0] <= 1000)
            {
                NPC.damage = NPC.defDamage*2;
                if (NPC.ai[0] % 100 < 40)
                {
                    if (vector.Length() > 400)
                    {
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 14) / 21;
                    }
                    else
                    {
                        NPC.velocity = (NPC.velocity * 20 + -vector.PerfectNormalize() * 14) / 21;
                    }
                }
                else if (NPC.ai[0] % 100 < 50)
                {
                    NPC.velocity = (NPC.velocity * 20 + -vector.PerfectNormalize() * 26) / 21;
                }
                else if (NPC.ai[0] % 100 == 50)
                {
                    NPC.velocity = vector.PerfectNormalize()*26;
                }
                NPC.rotation += 0.08f;
            }
            else if (NPC.ai[0] <= 1200)
            {
                NPC.damage = NPC.defDamage;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 4) / 21;
                NPC.rotation += 0.08f;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                if (NPC.ai[2] < 1.5F)
                {
                    NPC.ai[2] += 0.05F;
                }
                NPC.rotation += 0.08f;
                float XY = 80;
                if (NPC.ai[0] <= 1360)
                {
                    vector.Y -= 2.5f * XY;
                    vector.X -= 6.75f * XY;
                    float a = vector.Length() / 3;
                    if (a < 3)
                    {
                        a = 3;
                    }
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * a) / 21;
                    if (vector.Length() <= 5)
                    {
                        NPC.ai[0] = 1361;
                    }
                }
                else if (NPC.ai[0] <= 1375)
                {
                    NPC.velocity = new Vector2(XY, 0);
                }
                else if (NPC.ai[0] <= 1390)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 1405)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 1420)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 1435)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] == 1436)
                {
                    NPC.velocity = Vector2.Zero;
                    PlayerC = player.Center;
                }
                else if (NPC.ai[0] < 1910)
                {
                    vector.Y -= 500;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                    if (vector.Length() < 50)
                    {
                        NPC.ai[0] = 1910;
                    }
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.ai[0] > 1360 && NPC.ai[0] <= 1436)
                {
                    NPC.velocity += player.velocity;
                    if (NPC.ai[0] % 1 == 0)
                    {
                        Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI, 1)];
                    }
                }
                if (NPC.ai[0] == 1436)
                {
                    for (int A = 0; A < 1000; A++)
                    {
                        if (Main.projectile[A].active && Main.projectile[A].ai[1] == 1 && Main.projectile[A].type == ModContent.ProjectileType<BossStar>())
                        {
                            Main.projectile[A].ai[1] = 2;
                            Main.projectile[A].timeLeft = 1100;
                            Main.projectile[A].DProj().vector[0] = PlayerC;
                        }
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0;
            }
        }
        //第二阶段
        public void AI2(Player player, Vector2 vector, int damage)
        {
            if ((float)NPC.life / NPC.lifeMax <= 0.5F && NPC.Dnpc().Stage == 0)
            {
                NPC.Dnpc().Stage = 1;
                NPC.ai[0] = 1201;
                NPC.netUpdate = true;
            }
            NPC.ai[0]++;
            if (NPC.ai[0] <= 600)
            {
                Vector2 vector2;
                if (NPC.ai[0] <= 100)
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), 0, default);
                }
                else if (NPC.ai[0] <= 200)
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 2, default);
                }
                else if (NPC.ai[0] <= 300)
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 3, default);
                }
                else if (NPC.ai[0] <= 400)
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 5, default);
                }
                else if (NPC.ai[0] <= 500)
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 6, default);
                }
                else
                {
                    vector2 = Utils.RotatedBy(new Vector2(0, -500), 0, default);
                }
                if (NPC.ai[0] % 100 >= 70)
                {
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    vector += vector2;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                }
                if (NPC.ai[0] % 100 == 99)
                {
                    for (float A = 0; A < MathHelper.TwoPi - MathHelper.PiOver4; A += MathHelper.PiOver4)
                    {
                        if (A != 0 && A != MathHelper.PiOver4 * 2 && A != MathHelper.PiOver4 * 5 && Main.netMode != 1)
                        {
                            Vector2 projDirection = Utils.RotatedBy(new Vector2(-1, -1), A + NPC.rotation, default);
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + projDirection * 10, projDirection * 3, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI);
                        }
                    }
                }
                if (NPC.ai[2] > 1F)
                {
                    NPC.ai[2] -= 0.05F;
                }
            }
            else if (NPC.ai[0] <= 1200)
            {
                NPC.damage = NPC.defDamage * 2;
                if (NPC.ai[0] > 660&& NPC.ai[0] < 700)
                {
                    if(NPC.ai[0]%10==0)
                        Data(player.Center + Utils.RotatedBy(new Vector2(0, -500), 0), 0.5f, 0.5f);
                }
                if (NPC.ai[0] > 760&& NPC.ai[0] < 800)
                {
                    if (NPC.ai[0] % 10 == 0)
                        Data(player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 2), 0.5f,0.5f);

                }
                if (NPC.ai[0] > 860&& NPC.ai[0] < 900)
                {
                    if (NPC.ai[0] % 10 == 0)
                        Data( player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 3), 0.5f, 0.5f);
                }
                if (NPC.ai[0] > 960&& NPC.ai[0] < 1000)
                {
                    if (NPC.ai[0] % 10 == 0)
                        Data(player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 5), 0.5f, 0.5f);
                }
                if (NPC.ai[0] > 1060&& NPC.ai[0] < 1100)
                {
                    if (NPC.ai[0] % 10 == 0)
                        Data( player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 6), 0.5f, 0.5f);
                }
                if (NPC.ai[0] == 700)
                {
                    NPC.Center = player.Center + Utils.RotatedBy(new Vector2(0, -500), 0, default);
                }
                else if (NPC.ai[0] == 800)
                {
                    NPC.Center = player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 2, default);
                }
                else if (NPC.ai[0] == 900)
                {
                    NPC.Center = player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 3, default);
                }
                else if (NPC.ai[0] == 1000)
                {
                    NPC.Center = player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 5, default);
                }
                else if (NPC.ai[0] == 1100)
                {
                    NPC.Center = player.Center + Utils.RotatedBy(new Vector2(0, -500), MathHelper.PiOver4 * 6, default);
                }
                if (NPC.ai[0] >= 700)
                {
                    if(NPC.ai[0] % 100==0)
                    {
                        Data(NPC.Center, 2f, 1.2f);
                        SoundStyle sound = SoundID.Item4;
                        sound.Pitch = 0.4f;
                        PlaySound(sound, NPC.position);
                        NPC.velocity = Vector2.Zero;
                    }
                    if (NPC.ai[0] % 100 < 20)
                    {
                        NPC.velocity = -vector.PerfectNormalize() * 15;
                    }
                    else if (NPC.ai[0] % 100 == 20)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 26;
                    }
                }
                NPC.rotation += 0.08f;
            }
            else if (NPC.ai[0] <= 1200)
            {
                NPC.damage = NPC.defDamage;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 10) / 21;
                NPC.rotation += 0.08f;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                if (NPC.ai[2] < 1.5F && NPC.ai[0] < 1910)
                {
                    NPC.ai[2] += 0.05F;
                }
                NPC.rotation += 0.08f;
                float XY = 80;
                if (NPC.ai[0] <= 1960)
                {
                    vector.Y -= 2.5f * XY;
                    vector.X -= 6.75f * XY;
                    float a = vector.Length() / 3;
                    if (a < 3)
                    {
                        a = 3;
                    }
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * a) / 21;
                    if (vector.Length() <= 5)
                    {
                        NPC.ai[0] = 1961;
                    }
                }
                else if (NPC.ai[0] <= 1975)
                {
                    NPC.velocity = new Vector2(XY, 0);
                }
                else if (NPC.ai[0] <= 1990)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 2005)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 2020)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 2035)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] == 2036)
                {
                    NPC.velocity = Vector2.Zero;
                }
                else if (NPC.ai[0] < 2510)
                {
                    vector.Y -= 500;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                    if (vector.Length() < 50)
                    {
                        NPC.ai[0] = 2510;
                    }
                }
                else if (NPC.ai[0] < 3026)
                {
                    vector.Y -= 400;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                    if (NPC.ai[2] > 1F)
                    {
                        NPC.ai[2] -= 0.05F;
                    }
                    int Ti = (player.Center - PlayerC).Length() > 160 ? 20 : 80;
                    if (NPC.ai[0] % Ti == 0)
                    {
                        for (float A = 0; A < MathHelper.TwoPi - MathHelper.PiOver4; A += MathHelper.PiOver4)
                        {
                            if (A != 0 && A != MathHelper.PiOver4 * 2 && A != MathHelper.PiOver4 * 5 && Main.netMode != 1)
                            {
                                Vector2 projDirection = Utils.RotatedBy(new Vector2(-1, -1), A + NPC.rotation, default);
                                //NewProjectile(NPC.GetSource_FromAI(), NPC.Center + projDirection * 10, (player.Center - PlayerC).Length() > 160 ? projDirection * Main.rand.NextFloat(0.5F, 4) : projDirection / 2, ModContent.ProjectileType<BossStar>(), 0, 1, Main.myPlayer, NPC.whoAmI, 5);
                                NewProjectile(NPC.GetSource_FromAI(), NPC.Center + projDirection * 10, (player.Center - PlayerC).Length() > 160 ? projDirection * Main.rand.NextFloat(0.5F, 4) : projDirection / 2, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI, 4);
                            }
                        }
                    }
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.ai[0] > 1960 && NPC.ai[0] <= 2036)
                {
                    NPC.velocity += player.velocity;
                    Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI, 1)];
                    projectile.scale = 2;
                }
                if (NPC.ai[0] == 2510)
                {
                    PlayerC = player.Center;
                    for (int A = 0; A < 1000; A++)
                    {
                        if (Main.projectile[A].active && Main.projectile[A].ai[1] == 1 && Main.projectile[A].type == ModContent.ProjectileType<BossStar>())
                        {
                            Main.projectile[A].ai[1] = 2;
                            Main.projectile[A].timeLeft = 1100;
                            Main.projectile[A].DProj().vector[0] = PlayerC;
                        }
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0;
            }
        }
        //第三阶段
        public void AI3(Player player, Vector2 vector, int damage)
        {
            if ((float)NPC.life / NPC.lifeMax <= 0.5F && NPC.Dnpc().Stage == 0)
            {
                NPC.Dnpc().Stage = 1;
                NPC.ai[0] = 1000;
                //NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }
            NPC.damage = 0;
            NPC.dontTakeDamage = AnyNPCs(ModContent.NPCType<ServantOfTheStars>());
            if (NPC.ai[0] <= 600)
            {
                NPC.ai[0]++;
                vector.Y -= 400;
                if (NPC.ai[0] % 60 >= 40)
                {
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                }
                if (NPC.ai[0] % 60 == 59)
                {
                    for (float A = 0; A < MathHelper.TwoPi - MathHelper.PiOver4; A += MathHelper.PiOver4)
                    {
                        if (A != 0 && A != MathHelper.PiOver4 * 2 && A != MathHelper.PiOver4 * 5 && Main.netMode != 1)
                        {
                            Vector2 projDirection = Utils.RotatedBy(new Vector2(-1, -1), A + NPC.rotation, default);
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + projDirection * 10, projDirection * 3.5f, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI);
                        }
                    }
                }
                if (NPC.ai[2] > 1F)
                {
                    NPC.ai[2] -= 0.05F;
                }
            }
            else if (!AnyNPCs(ModContent.NPCType<ServantOfTheStars>()) && NPC.ai[0] == 601)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int a = 0; a < 5; a++)
                    {
                        NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ServantOfTheStars>(), NPC.whoAmI, NPC.whoAmI);
                    }
                }
                NPC.ai[1] = 0;
            }
            else if (AnyNPCs(ModContent.NPCType<ServantOfTheStars>()))
            {
                NPC.ai[0] = 602;
                NPC.ai[1] += 0.01f;
                NPC.rotation += 0.08f;
                Vector2 vector2 = player.Center + Utils.RotatedBy(new Vector2(0f, -400), NPC.ai[1], default);
                vector = vector2 - NPC.Center;
                float a = vector.Length() / 3;
                if (a < 3)
                {
                    a = 3;
                }
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * a) / 21;
            }
            else if (NPC.ai[0] < 1000)
            {
                NPC.ai[0]++;
                NPC.ai[3]++;
                NPC.ai[1] += 0.15f;
                NPC.rotation += 0.15f;
                Vector2 vector2 = player.Center + Utils.RotatedBy(new Vector2(0f, -100), NPC.ai[1], default);
                if (NPC.ai[0] == 603)
                {
                    Data(NPC.Center, 0.6f, 0.6f);
                    Data(NPC.Center, 1f, 1f);
                    Data(NPC.Center, 1.4f, 1.4f);
                }
                NPC.Center = vector2;
                if(NPC.ai[0]==603)
                {
                    Data(NPC.Center, 0.6f, 0.6f);
                    Data(NPC.Center, 1f, 1f);
                    Data(NPC.Center, 1.4f, 1.4f);
                }
                if (NPC.ai[3] % 80 == 0)
                {
                    if (Main.netMode != 1)
                    {
                        NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(0, 300).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi)), Vector2.Zero, ModContent.ProjectileType<BossMagicStarCircle>(), damage, 1, Main.myPlayer, NPC.whoAmI, 10);
                    }
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[0]++;
                if (NPC.ai[2] < 1.5F)
                {
                    NPC.ai[2] += 0.05F;
                }
                NPC.rotation += 0.08f;
                float XY = 60;
                if (NPC.ai[0] <= 1160)
                {
                    vector.Y -= 2.5f * XY;
                    vector.X -= 6.75f * XY;
                    float a = vector.Length() / 3;
                    if (a < 3)
                    {
                        a = 3;
                    }
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * a) / 21;
                    if (vector.Length() <= 5)
                    {
                        NPC.ai[0] = 1161;
                    }
                }
                else if (NPC.ai[0] <= 1175)
                {
                    NPC.velocity = new Vector2(XY, 0);
                }
                else if (NPC.ai[0] <= 1190)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5*4);
                }
                else if (NPC.ai[0] <= 1205)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 1220)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] <= 1235)
                {
                    NPC.velocity = new Vector2(XY, 0).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4).RotatedBy(MathHelper.Pi / 5 * 4);
                }
                else if (NPC.ai[0] == 1236)
                {
                    NPC.velocity = Vector2.Zero;
                    PlayerC = player.Center;
                }
                else if (NPC.ai[0] < 1710)
                {
                    vector.Y -= 500;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 20) / 21;
                    NPC.rotation += 0.08f;
                    if (vector.Length() < 50)
                    {
                        NPC.ai[0] = 1710;
                    }
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.ai[0] > 1161 && NPC.ai[0] <= 1236)
                {
                    NPC.velocity += player.velocity;
                    Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BossStar>(), damage, 1, Main.myPlayer, NPC.whoAmI, 1)];
                    projectile.scale = 2f;
                }
                if (NPC.ai[0] == 1237)
                {
                    for (int A = 0; A < 1000; A++)
                    {
                        if (Main.projectile[A].active && Main.projectile[A].ai[1] == 1 && Main.projectile[A].type == ModContent.ProjectileType<BossStar>())
                        {
                            Main.projectile[A].ai[1] = 2;
                            Main.projectile[A].timeLeft = 750;
                            Main.projectile[A].DProj().vector[0] = PlayerC;
                        }
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0;
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, new Color(155, 155, 155, 0), 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, new Color(155, 155, 155, 0), 1f);
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
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType != DamageClass.Magic)
            {
                modifiers.SourceDamage *= 0.66f;
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType != DamageClass.Magic)
            {
                modifiers.SourceDamage *= 0.66f;
            }
        }
        string Text = "";
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            Texture2D glow = Glow.Value;
            //绘制光效残影
            if (NPC.localAI[1] != 2)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2 - screenPos;
                    Color color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length/2);
                    spriteBatch.Draw(glow, vector2, null, color, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);

                }
            }
            Vector2 vector = new Vector2(texture.Width / 4, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(glow, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2], spriteEffects, 0f);
            spriteBatch.Draw(glow, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, null, new Color(255 - NPC.alpha, 155 - NPC.alpha, 0, 0) * 1f, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] / 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] == 2)
            {
                return false;
            }
            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, new Rectangle?(((float)NPC.life / NPC.lifeMax <= 0.5F) ? new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height) : new Rectangle(0, 0, texture.Width / 2, texture.Height)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
            if (NPCDowned.downedStarGuard2 && AnyNPCs(ModContent.NPCType<ServantOfTheStars>()))
            {
                spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, new Rectangle?(((float)NPC.life / NPC.lifeMax <= 0.5F) ? new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height) : new Rectangle(0, 0, texture.Width / 2, texture.Height)), new Color(0, 100, 255, 0), NPC.rotation, vector, NPC.scale*1.5f, spriteEffects, 0f);
                spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - screenPos, new Rectangle?(((float)NPC.life / NPC.lifeMax <= 0.5F) ? new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height) : new Rectangle(0, 0, texture.Width / 2, texture.Height)), new Color(0, 100, 255, 0), NPC.rotation, vector, NPC.scale*1.5f, spriteEffects, 0f);
            }
            return false;
        }
    }
}