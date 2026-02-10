using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Worlds;
using Terraria.ModLoader;

namespace DDmod.Content.NPCs.Boss.MeteorDigger
{
    [AutoloadBossHead]
    public class MeteorDigger_Head : ModNPC
    {
        public static Asset<Texture2D> Head;
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Head = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Head");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Head_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
            DDSystem.HBar(NPC.type, "流星血条", new Vector2(4, -2));
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDiggerTexture",
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

        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.defense = 8;
            NPC.damage = 40;
            NPC.lifeMax = 6000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.value = 12000f;
            NPC.scale = 1.5f;
            NPC.boss = true;
            if (!Main.dedServ) Music = DDSystem.Music(2, "陨坑轰炸");
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().Times[1] = 60;
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().Properties.Iron = true;
            NPC.Dnpc().Properties.BossLife = 1.1F;

        }
        public override string BossHeadTexture => "DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Head_Boss";
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.MeteorDigger"))
            });
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation + MathHelper.PiOver2;
        }
        public bool title;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2.5f;
            return new bool?(true);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MeteorToolbox>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeteorDiggerTrophyItem>(), 10));
            //第一次击败
            npcLoot.Add(ItemDropRule.ByCondition(new NodownedMeteorDigger(),ModContent.ItemType<奇怪的控制器>()));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<MeteorDiggerMask>());
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<流星掘地者圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<SlightlyDamagedCore>(), 4));

            //特别引用,普通模式

            npcLoot.NormalLoot(1, ModContent.ItemType<LargeMechanicalScrap>(), 10, 16);
            npcLoot.NormalLoot(1, ModContent.ItemType<流星电池>(), 3, 8);

        }
        public class NodownedMeteorDigger : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return !NPCDowned.downedMeteorDigger;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }
            

            public string GetConditionDescription()
            {
                return "第一次被击败时";
            }
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.downedMeteorDigger, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (!NPC.Dnpc().Deathrattle&&NPC.life<=0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int a = 0; a < 100; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 2.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(3, 8), Main.rand.NextFloat(3, 8)), (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                    }
                    int GoreType = Mod.Find<ModGore>("MeteorDigger3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 12, GoreType, 1.5f);
                }
            }
        }
        public override bool CheckDead()
        {
            if(NPC.Dnpc().Deathrattle)
            {
                NPC.life = 1000;
                NPC.NPCLoot();
                NPC.dontTakeDamage = true;
                NPC.localAI[2] = 1;
                NPC.netUpdate = true;
            }
            return !NPC.Dnpc().Deathrattle;
        }
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, 2.48f * 0.3f, 0.66f * 0.3f, 0.05f * 0.3f);
            Player player = Main.player[NPC.target];
            //钻头音效
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 20;
                PlaySound(new SoundStyle("DDmod/NoContent/Sounds/NPC/钻头"), NPC.position);

                //Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 22, 1.5f, 0.3f);
            }
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

            //火焰
            for (int a = 0; a < 20; a++)
            {
                Dust dust = Main.dust[NewDust(NPC.Center + (NPC.rotation).ToRotationVector2() * 100, 1, 1, 6, 0, 0, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.2F;
                dust.velocity = Main.rand.NextVector2Unit(NPC.rotation, Main.rand.NextFloat(-1.3F, 1.3F)) * Main.rand.NextFloat(10, 20);

            }

            if (NPC.target < 0 || NPC.target == 255 || player.dead)
            {
                NPC.TargetClosest(true);
            }
            if (player.dead && NPC.Distance(player.Center) > 2000)
            {
                NPC.active = false;
            }
            if (NPC.alpha != 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int num = NewDust(NPC.Center, 1, 1, 6, 0f, 0f, 100, default, 1.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2F, 12F);
                }
            }
            if (NPC.Dnpc().vector[0] == Vector2.Zero)
            {
                NPC.Dnpc().vector[0] = NPC.Center;
            }
            if (NPC.localAI[2] == 0)
            {
                NPC.dontTakeDamage = NPC.alpha != 0;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                //头
                if (NPC.ai[0] == 0f)
                {
                    int NPCWhoAmI = NPC.whoAmI;
                    int Length = 40;
                    for (int i = 0; i <= Length; i++)
                    {
                        int NPCWhoAmI2;
                        if (i < Length)
                        {
                            NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<MeteorDigger_Body>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        }
                        else
                        {
                            NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<MeteorDigger_Tail>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        }
                        Main.npc[NPCWhoAmI2].Dnpc().Times[4] = (float)i * 0.2f;
                        Main.npc[NPCWhoAmI2].realLife = NPC.whoAmI;
                        Main.npc[NPCWhoAmI2].ai[3] = NPC.whoAmI;
                        Main.npc[NPCWhoAmI2].ai[2] = i;
                        Main.npc[NPCWhoAmI2].ai[1] = NPCWhoAmI;
                        Main.npc[NPCWhoAmI].ai[0] = NPCWhoAmI2;
                        Main.npc[NPCWhoAmI2].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPCWhoAmI2, 0f, 0f, 0f, 0, 0, 0);
                        NPCWhoAmI = NPCWhoAmI2;
                    }
                    NPC.ai[0] = 1;
                }
            }

            Rectangle Position = new Rectangle((int)NPC.position.X / 16, (int)NPC.position.Y / 16, NPC.width / 16, NPC.height / 16);

            float Acceleration = 0.4f * (1 + (1 - (float)NPC.life / NPC.lifeMax));
            float Speed = 30f;

            // NPC和物块相撞
            bool TileCollision = false;
            for (int i = Position.X; i < Position.X + Position.Width; i++)
            {
                for (int j = Position.Y; j < Position.Y + Position.Height; j++)
                {
                    if (i >= 0 && j >= 0 && i <= Main.maxTilesX && j <= Main.maxTilesY)
                    {
                        if (Main.tile[i, j] != null && ((Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[Main.tile[i, j].TileType] || (Main.tileSolidTop[Main.tile[i, j].TileType] && Main.tile[i, j].TileFrameY == 0))) || Main.tile[i, j].LiquidAmount > 64))
                        {
                            Vector2 v;
                            v.X = i * 16;
                            v.Y = j * 16;
                            if (NPC.position.X + NPC.width > v.X && NPC.position.X < v.X + 16f && NPC.position.Y + NPC.height > v.Y && NPC.position.Y < v.Y + 16f)
                            {
                                TileCollision = true;
                                break;
                            }
                        }
                    }
                }
            }
            //地震
            if (TileCollision)
            {
                //客户端玩家距离
                float ClientPlayerLength = (Main.LocalPlayer.Center - NPC.Center).Length();
                if (Main.LocalPlayer.velocity.Y == 0)
                {
                    if (ClientPlayerLength > 4000)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 1);
                    }
                    else if (ClientPlayerLength > 3000)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 2);
                    }
                    else if (ClientPlayerLength > 2500)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 3);
                    }
                    else if (ClientPlayerLength > 2000)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 4);
                    }
                    else if (ClientPlayerLength > 1500)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 5);
                    }
                    else if (ClientPlayerLength > 1200)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 8);
                    }
                    else if (ClientPlayerLength > 1000)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 10);
                    }
                    else if (ClientPlayerLength > 800)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 12);
                    }
                    else if (ClientPlayerLength > 500)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 14);
                    }
                    else if (ClientPlayerLength > 200)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 16);
                    }
                    else
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(5, 20);
                    }
                }
            }

            if (NPC.Center.Y - player.Center.Y > 1500)
            {
                TileCollision = true;
            }

            if (player.dead)
            {
                TileCollision = false;
                NPC.velocity.Y++;
            }
            Vector2 vector = player.Center - NPC.Center;
            float X = Math.Abs(vector.X);
            float Y = Math.Abs(vector.Y);
            if (NPC.life / NPC.lifeMax <= 0.5F && Main.expertMode)
            {
                NPC.ai[3]++;
                if (DDHelper.SpecifyDirection(NPC.rotation, (player.Center - NPC.Center).ToRotation(), 0.1F) && NPC.ai[3] > 300)
                {
                    NPC.velocity = NPC.velocity.PerfectNormalize() * (Acceleration * 60);
                    NPC.ai[3] = 0;

                }
            }
            //移动代码
            if (NPC.localAI[2] == 0)
            {
                if (!TileCollision)
                {
                    NPC.TargetClosest(true);
                    NPC.velocity.Y += 0.2f;
                    if (NPC.velocity.Y > 0) NPC.velocity.Y += 0.4f;

                    if (NPC.velocity.Y > Speed)
                    {
                        NPC.velocity.Y = Speed;
                    }
                }
                else
                {
                    vector = vector.PerfectNormalize() * Speed;
                    if ((NPC.velocity.X > 0f && vector.X > 0f) || (NPC.velocity.X < 0f && vector.X < 0f) || (NPC.velocity.Y > 0f && vector.Y > 0f) || (NPC.velocity.Y < 0f && vector.Y < 0f))
                    {
                        if (NPC.velocity.X < vector.X)
                        {
                            NPC.velocity.X += Acceleration;
                        }
                        else if (NPC.velocity.X > vector.X)
                        {
                            NPC.velocity.X -= Acceleration;
                        }
                        if (NPC.velocity.Y < vector.Y)
                        {
                            NPC.velocity.Y += Acceleration;
                        }
                        else if (NPC.velocity.Y > vector.Y)
                        {
                            NPC.velocity.Y -= Acceleration;
                        }
                        if ((double)Math.Abs(vector.Y) < (double)Speed * 0.2 && ((NPC.velocity.X > 0f && vector.X < 0f) || (NPC.velocity.X < 0f && vector.X > 0f)))
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y += Acceleration * 2f;
                            }
                            else
                            {
                                NPC.velocity.Y -= Acceleration * 2f;
                            }
                        }
                        if ((double)Math.Abs(vector.X) < (double)Speed * 0.2 && ((NPC.velocity.Y > 0f && vector.Y < 0f) || (NPC.velocity.Y < 0f && vector.Y > 0f)))
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X += Acceleration * 2f;
                            }
                            else
                            {
                                NPC.velocity.X -= Acceleration * 2f;
                            }
                        }
                    }
                    else if (X > Y)
                    {
                        if (NPC.velocity.X < vector.X)
                        {
                            NPC.velocity.X += Acceleration * 1.1f;
                        }
                        else if (NPC.velocity.X > vector.X)
                        {
                            NPC.velocity.X -= Acceleration * 1.1f;
                        }
                        if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)Speed * 0.5)
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y += Acceleration;
                            }
                            else
                            {
                                NPC.velocity.Y -= Acceleration;
                            }
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < vector.Y)
                        {
                            NPC.velocity.Y += Acceleration * 1.1f;
                        }
                        else if (NPC.velocity.Y > vector.Y)
                        {
                            NPC.velocity.Y -= Acceleration * 1.1f;
                        }
                        if ((double)(Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < (double)Speed * 0.5)
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X += Acceleration;
                            }
                            else
                            {
                                NPC.velocity.X -= Acceleration;
                            }
                        }
                    }
                }
            }
            if (NPC.localAI[2] > 0)
            {
                /*if (!Main.npc[(int)NPC.ai[0]].active && (Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<MeteorDigger_Body>() || Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<MeteorDigger_Tail>()))
                {
                    Main.npc[(int)NPC.ai[0]].life = NPC.life;
                    Main.npc[(int)NPC.ai[0]].active = true;
                    Main.npc[(int)NPC.ai[0]].ai[1] = NPC.whoAmI;
                    Main.npc[(int)NPC.ai[0]].localAI[2] = 1;
                    Main.npc[(int)NPC.ai[0]].dontTakeDamage = true;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, (int)NPC.ai[0], 0f, 0f, 0f, 0, 0, 0);
                }*/
                for (int a = 0; a < 200; a++)
                {
                    if (Main.npc[a].active)
                    {
                        if ((Main.npc[a].type == ModContent.NPCType<MeteorDigger_Head>() || Main.npc[a].type == ModContent.NPCType<MeteorDigger_Body>() || Main.npc[a].type == ModContent.NPCType<MeteorDigger_Tail>()) && (Main.npc[a].realLife == NPC.realLife || NPC.whoAmI == Main.npc[a].realLife))
                        {
                            if (!Main.npc[a].dontTakeDamage)
                            {
                                Main.npc[a].dontTakeDamage = true;
                            }
                            Main.npc[a].life = 1000;
                            if (Main.npc[a].localAI[2] <= 0)
                            {
                                Main.npc[a].localAI[2] = 1;
                            }
                        }
                        if (Main.npc[a].type == ModContent.NPCType<MeteorProbe>())
                        {
                            Main.npc[a].Kill(false);
                        }
                    }
                }
                Main.LocalPlayer.Dplayer().PlayerShake(30, 10);
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 120, false, 0.2F);
                if (NPC.soundDelay == 0)
                {
                    NPC.soundDelay = 60;
                    PlaySound(new SoundStyle("DDmod/NoContent/Sounds/NPC/警报"), NPC.position);
                }
                NPC.Dnpc().Times[0] += Main.rand.NextFloat(3);
                if (NPC.Dnpc().Times[1] % 2 == 1 && Main.rand.NextBool(10))
                {
                    CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(255, 0, 0), "过热警告！！过热警告！！", true, false);
                }
                if (Main.rand.NextBool(20))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 31, 0f, 0f, 100, default, 1f);
                        Main.dust[D].noGravity = true;
                        Dust dust34 = Main.dust[D];
                        dust34.scale *= 1f + Main.rand.Next(10) * 0.5f;
                        dust34.velocity.Y = dust34.velocity.Y - 2f;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default, 1f);
                        Main.dust[D].noGravity = true;
                        Dust dust34 = Main.dust[D];
                        dust34.scale *= 1f + Main.rand.Next(10) * 0.5f;
                        dust34.velocity.Y = dust34.velocity.Y - 2f;
                    }
                }
                NPC.Dnpc().Times[1] -= 0.1F;
                if (Main.netMode==2)
                {
                    NPC.Dnpc().netUpdate = true;
                    NPC npc = NPC;
                    //同步位置
                    DDmod.SyncData(DDType.NPCCenter, npc.whoAmI, -1, -1);
                }
                if (NPC.Dnpc().Times[1] <= 0)
                {
                    NPC.Dnpc().Deathrattle = false;
                    NPC.life = 0;
                    NPC.HitEffect(0, 100);
                    NPC.active = false;
                }
                if (NPC.velocity.Y > 25)
                {
                    NPC.velocity.Y = 25;
                }
                if (NPC.velocity.Y < 0)
                {
                    NPC.velocity.Y = 1;
                }

                NPC.noTileCollide = false;
                if (NPC.velocity.Y <= 0.02f && NPC.Dnpc().Times[1] > 0.4f)
                {
                    NPC.Dnpc().Times[1] = 0.4f;
                }
                NPC.velocity.Y += 0.5F;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (NPC.localAI[2] > 0 && NPC.Dnpc().Times[0] % 60 <= NPC.Dnpc().Times[1])
            {
                drawColor = new Color(255, 0, 0);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot( ref int potionType)
        {
            potionType = 188;
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(36, 240, true);
            player.AddBuff(30, 240, true);
        }
        private bool flies;
        private bool TailSpawned;
        private bool TE;
        public int[] Timer = new int[5];
        public override void SendExtraAI(BinaryWriter writer)
        {
            for (int a = 0; a < 5; a++)
            {
                writer.Write(Timer[a]);
            }
            writer.WriteVector2(NPC.Dnpc().vector[0]);
            writer.Write(TE);
            writer.Write(TailSpawned);
            writer.Write(flies);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int a = 0; a < 5; a++)
            {
                Timer[a] = reader.ReadInt32();
            }
            NPC.Dnpc().vector[0] = reader.ReadVector2();
            TE = reader.ReadBoolean();
            TailSpawned = reader.ReadBoolean();
            flies = reader.ReadBoolean();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Head.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, new Vector2(Head.Width() / 2, Head.Height() / 2), NPC.scale, 0, 0f);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}
