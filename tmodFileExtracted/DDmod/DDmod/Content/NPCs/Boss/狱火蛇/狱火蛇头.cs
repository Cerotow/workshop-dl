using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.狱火蛇
{
    [AutoloadBossHead]
    public class 狱火蛇头 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> texture2;
        public static Asset<Texture2D> texture2_Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
                texture2 = ModContent.Request<Texture2D>(Texture + "2");
                texture2_Glow = ModContent.Request<Texture2D>(Texture + "2_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hellfire Serpent");
            //DisplayName.AddTranslation(7, "狱火蛇");
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/狱火蛇/狱火蛇Texture",
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
            DDSystem.HBar(NPC.type, "狱火蛇", new Vector2(-24, 0));

        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.defense = 30;
            NPC.damage = 140;
            NPC.lifeMax = 72000;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.value = 12000f;
            NPC.scale = 1.5f;
            NPC.boss = true;
            NPC.Dnpc().MaxPenetrationProtection = 1F;
            NPC.Dnpc().Properties.Fire = true;
            NPC.Dnpc().Properties.Stone = true;

            if (!Main.dedServ) Music = DDSystem.Music(3, "狱火·");
            NPC.Dnpc().Properties.BossLife = 1.275F;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.狱火蛇"))
            });
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation+MathHelper.PiOver2;
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
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<狱火蛇宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<狱火蛇纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<狱火蛇面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<狱火蛇圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<狱火晶石>(), 4));

            npcLoot.SpecialLoot(ModContent.ItemType<狱火链刃>(), 1);
            //特别引用,普通模式
            //普通模式
            int[] A = new int[4];
            A[0] = ModContent.ItemType<狱火剑>();
            A[1] = ModContent.ItemType<狱火炮>();
            A[2] = ModContent.ItemType<狱火杖>();
            A[3] = ModContent.ItemType<狱火鞭>();
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);

        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.欲火蛇, -1);
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
                }
            }
        }
        public override bool? CanFallThroughPlatforms()
        {
            return base.CanFallThroughPlatforms();
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override void AI()
        {

            NPC.defense = NPC.defDefense;
            Lighting.AddLight(NPC.Center, new Vector3(253, 12, 3) * 0.008F);
            //Lighting.AddLight(NPC.Center, new Vector3(0, 120, 255) * 0.003F);

            if (NPC.Dnpc().Stage == 4)
            {
                if (!NPC.Dnpc().Bool[4] && NPC.ai[0] == 100)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        int GoreType = Mod.Find<ModGore>("狱火蛇1").Type;
                        int GoreType2 = Mod.Find<ModGore>("狱火蛇2").Type;

                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(4, 0).RotatedBy(NPC.rotation + MathHelper.PiOver2), GoreType, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-4, 0).RotatedBy(NPC.rotation + MathHelper.PiOver2), GoreType2, NPC.scale);
                    }
                    NPC.Dnpc().Bool[4] = true;
                }
            }

            Player player;
            if (NPC.target < 0 || NPC.target == 255)
            {
                NPC.TargetClosest(true);
            }
            player = Main.player[NPC.target];
            if (player.dead)
            {
                NPC.TargetClosest(true);
            }
            player = Main.player[NPC.target];
            if ((player.dead)||(NPC.Dnpc().Stage == 1&&(NPC.Center.X<0|| NPC.Center.X>Main.maxTilesX*16)))
            {
                NPC.active = false;
            }
            if(player.ActiveItem().type==ModContent.ItemType<火蛇卵>())
            {
                if (NPC.Dnpc().Stage == 1)
                {
                    NPC.Dnpc().Stage = 2;
                }
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
                if (NPC.Dnpc().Stage == 0)
                {
                    int NPCWhoAmI = NPC.whoAmI;
                    int Length = 30;
                    for (int i = 0; i <= Length; i++)
                    {
                        int NPCWhoAmI2;
                        if (i < Length)
                        {
                            if (i % 2 == 0)
                            {
                                NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<狱火蛇身>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                            }
                            else
                            {
                                NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<狱火蛇身2>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);

                            }
                        }
                        else
                        {
                            NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<狱火蛇尾>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        }
                        Main.npc[NPCWhoAmI2].realLife = NPC.whoAmI;
                        Main.npc[NPCWhoAmI2].ai[3] = NPC.whoAmI;
                        Main.npc[NPCWhoAmI2].ai[2] = i;
                        Main.npc[NPCWhoAmI2].ai[1] = NPCWhoAmI;
                        Main.npc[NPCWhoAmI].ai[0] = NPCWhoAmI2;
                        Main.npc[NPCWhoAmI2].netUpdate = true;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPCWhoAmI2, 0f, 0f, 0f, 0, 0, 0);
                        NPCWhoAmI = NPCWhoAmI2;
                    }
                    NPC.Dnpc().Times[0] = Main.rand.NextBool(2)?1:-1;
                    NPC.Dnpc().Stage = 1;
                    NPC.netUpdate = true;
                }
            }
            float MaxSpeed = 10 + 50f * (1F - (float)NPC.life / NPC.lifeMax / 2);
            if ((float)NPC.life / NPC.lifeMax > 0.5F && (float)NPC.life / NPC.lifeMax <= 0.75F)
            {
                if (!player.lavaImmune && (NPC.Center - Main.LocalPlayer.Center).Length() < 4000)

                    Main.LocalPlayer.AddBuff(BuffID.OnFire, 2, false);
            }
            else if ((float)NPC.life / NPC.lifeMax > 0.25F && (float)NPC.life / NPC.lifeMax <= 0.5F)
            {
                if (!player.lavaImmune && (NPC.Center - Main.LocalPlayer.Center).Length() < 4000)
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<地狱之火>(), 2, false);
            }
            else if ((float)NPC.life / NPC.lifeMax <= 0.25F)
            {
                if ((NPC.Center - Main.LocalPlayer.Center).Length() < 4000)
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<炼狱之火>(), 2, false);

            }
            if ((float)NPC.life / NPC.lifeMax <= 0.95F)
            {
                if (NPC.Dnpc().Stage == 1)
                {
                    NPC.Dnpc().Times[0] = 0;
                    NPC.Dnpc().Stage = 2;
                    NPC.ai[1] = 0;
                    NPC.netUpdate = true;
                }
            }
            if ((float)NPC.life / NPC.lifeMax <= 0.5F)
            {
                if (NPC.Dnpc().Stage == 2)
                {
                    NPC.Dnpc().Stage = 3;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.netUpdate = true;
                }
            }
            if ((float)NPC.life / NPC.lifeMax <= 0.25F)
            {
                if (NPC.Dnpc().Stage == 3)
                {
                    NPC.Dnpc().Stage = 4;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.Dnpc().Stage >= 2)
            {
                if ((NPC.Center - Main.LocalPlayer.Center).Length() < 4000)
                {
                    DDmodOn.Filter(new Color(253, 62, 3, 100), 0.25F * (1 - (float)NPC.life / NPC.lifeMax), 0.01F);
                    狱火蛇背景.狱火 = true;
                }
                NPC.boss = true;
                if (!Main.dedServ) Music = DDSystem.Music(3, "狱火蛇");
                if (NPC.Dnpc().Stage == 2)
                {
                    MaxSpeed /= 3;
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 180)
                    {
                        if (NPC.ai[0] % 20 == 0 && Main.netMode != NetmodeID.MultiplayerClient&&Main.expertMode)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.PerfectNormalize() * 40, ModContent.ProjectileType<BossHellfireball>(), 40, 2);
                        }
                    }
                    if (NPC.ai[0] > 260)
                    {
                        NPC.ai[0] = 0;
                    }
                }
                else if (NPC.Dnpc().Stage==3)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 1200)
                    {
                        if (NPC.ai[0] % 20 == 0 && Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.PerfectNormalize() * 40, ModContent.ProjectileType<BossHellfireball>(), 40, 2);
                        }
                    }
                    else if (NPC.ai[0] > 400)
                    {
                        MaxSpeed /= 3;
                        NPC.defense = 100;
                        NPC.ai[1]++;
                    }
                    if (NPC.ai[0] > 100)
                    {
                        if (NPC.ai[0] % 30 == 0 && Main.netMode != 1)
                        {
                            if (Main.rand.NextBool(5))
                            {
                                if (Main.rand.NextBool(2))
                                {
                                    int A = NewNPCs(NPC.GetSource_FromAI(), player.Center - new Vector2(1000, Main.rand.NextFloat(-800, 800)), ModContent.NPCType<狱火小蛇头>(), 0);
                                    Main.npc[A].velocity = new Vector2(5, 0);
                                }
                                else
                                {
                                    int A = NewNPCs(NPC.GetSource_FromAI(), player.Center + new Vector2(1000, Main.rand.NextFloat(-800, 800)), ModContent.NPCType<狱火小蛇头>(), 0);
                                    Main.npc[A].velocity = new Vector2(-5, 0);
                                }
                            }
                            else
                            {
                                if (Main.rand.NextBool(2))
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(1000, Main.rand.NextFloat(-800, 800)), new Vector2(5, 0), ModContent.ProjectileType<狱火小蛇>(), 50, 1);
                                    Main.projectile[A].DProj().Times[0] = 15;
                                }
                                else
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(1000, Main.rand.NextFloat(-800, 800)), new Vector2(-5, 0), ModContent.ProjectileType<狱火小蛇>(), 50, 1);
                                    Main.projectile[A].DProj().Times[0] = 15;
                                }
                            }
                        }
                        if (Main.rand.NextBool(200))
                        {
                            杂物Sky.NewDirectionalProj(5, 6, 4, 1, false, false, true, true);
                        }
                    }
                    if (NPC.ai[0] > 1260)
                    {
                        NPC.ai[0] = 1;
                    }
                }
                else if (NPC.Dnpc().Stage == 4)
                {
                    NPC.defense = 0;
                    NPC.Dnpc().Properties.Stone = false;
                    NPC.HitSound = null;
                    NPC.DeathSound = null;
                    if (NPC.ai[0] % 30 == 0 && Main.netMode != 1)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            int A = NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(1000, Main.rand.NextFloat(-800, 800)), new Vector2(5, 0), ModContent.ProjectileType<狱火小蛇>(), 50, 1);
                            Main.projectile[A].DProj().Times[0] = 15;
                        }
                        else
                        {
                            int A = NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(1000, Main.rand.NextFloat(-800, 800)), new Vector2(-5, 0), ModContent.ProjectileType<狱火小蛇>(), 50, 1);
                            Main.projectile[A].DProj().Times[0] = 15;
                        }
                    }
                    if (Main.rand.NextBool(100))
                    {
                        杂物Sky.NewDirectionalProj(5, 6, 4, 1,false, false, true, true);
                    }
                    NPC.ai[0]++;
                }
            }
            else
            {
                NPC.boss = false;
                MaxSpeed = 6;
            }

            Rectangle Position = new Rectangle((int)NPC.position.X / 16, (int)NPC.position.Y / 16, NPC.width / 16, NPC.height / 16);


            // NPC和物块相撞
            bool TileCollision = Collision.SolidCollision(NPC.position, NPC.width, NPC.height)|| NPC.Dnpc().Stage == 4 || NPC.Center.Y - player.Center.Y > 1500;
            if (!TileCollision)
            {
                for (int i = Position.X; i < Position.X + Position.Width; i++)
                {
                    for (int j = Position.Y; j < Position.Y + Position.Height; j++)
                    {
                        if (i >= 0 && j >= 0 && i <= Main.maxTilesX && j <= Main.maxTilesY)
                        {
                            if (Main.tile[i, j].LiquidAmount > 64 && Main.tile[i, j].LiquidType == 1)
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
            } 

            if (player.dead)
            {
                TileCollision = false;
                NPC.velocity.Y++;
            }
            Vector2 vector;
            if (NPC.Dnpc().Stage <= 1)
            {
                MaxSpeed = 2;
                DDHelper.BackAndForth(-0.2F, 0.2F, 0.001F, ref NPC.ai[1], ref NPC.Dnpc().Bool[0]);
                vector = (NPC.Center + new Vector2(NPC.Dnpc().Times[0], -0F).RotatedBy(NPC.ai[1]) * 40) - NPC.Center;
            }
            else
            if (NPC.Dnpc().Stage != 4)
            {
                vector = player.Center - NPC.Center;
            }
            else
            {
                vector = player.Center - NPC.Center;
                if (NPC.ai[0] > 100)
                {
                    vector = player.Center + new Vector2(400, 0).RotatedBy(NPC.ai[0] / 30) - NPC.Center;
                }

            }
            if (NPC.Dnpc().Stage > 1 && (Main.LocalPlayer.Center - NPC.Center).Length() < 5000 && Main.LocalPlayer.position.Y < Main.UnderworldLayer * 16)
            {
                Main.LocalPlayer.velocity.Y = 0;
                if (Math.Abs(Main.LocalPlayer.position.Y+4 - Main.UnderworldLayer * 16)> Main.LocalPlayer.height * 2)
                {
                    Main.LocalPlayer.position.Y += Main.LocalPlayer.height * 2;
                }
                else
                {
                    Main.LocalPlayer.position.Y += Math.Abs(Main.LocalPlayer.position.Y - Main.UnderworldLayer * 16);
                }
            }
            //移动代码
            if (NPC.Dnpc().Stage !=4)
            {
                if (!TileCollision)
                {
                    NPC.TargetClosest(true);
                    NPC.velocity.Y += 0.2f;
                    if (NPC.velocity.Y > 0) NPC.velocity.Y += 0.4f;

                    if (NPC.velocity.Y > 16)
                    {
                        NPC.velocity.Y = 16;
                    }
                    if (NPC.velocity.X > 0 && NPC.velocity.X < 3)
                    {
                        NPC.velocity.X += 0.1F;
                    }
                    if (NPC.velocity.X < 0 && NPC.velocity.X > -3)
                    {
                        NPC.velocity.X -= 0.1F;
                    }
                    if (NPC.position.Y < Main.UnderworldLayer * 16)
                    {
                        NPC.velocity.Y = 10;
                    }
                    NPC.rotation = NPC.velocity.ToRotation();
                }
                else
                {
                    if (MaxSpeed > Speed)
                    {
                        Speed += 0.4F;
                    }
                    else
                    {
                        Speed = MaxSpeed;
                    }
                    vector = vector.PerfectNormalize();
                    NPC.velocity = (NPC.rotation).ToRotationVector2() * Speed;
                    if (NPC.position.Y < Main.UnderworldLayer * 16)
                    {
                        NPC.rotation = MathHelper.PiOver2;
                        NPC.velocity = (NPC.rotation).ToRotationVector2() * Speed;
                    }
                    else
                    {
                        NPC.RotationSpeed(vector.ToRotation(), 0.002F * Speed + 0.02F);
                    }
                    if (!DDHelper.SpecifyDirection(NPC.rotation, vector.ToRotation() + MathHelper.PiOver2, 0.2F))
                    {
                        if (Speed > MaxSpeed / 2)
                        {
                            Speed -= 1F;
                        }
                    }
                }
            }
            else
            {
                if (NPC.ai[0] == 100)
                {
                    NPC.Dnpc().vector[0] = NPC.velocity;
                }
                if (NPC.ai[0] <= 100)
                {
                    NPC.velocity = (NPC.rotation).ToRotationVector2() * Speed;
                }
                else
                {
                    if (MaxSpeed > Speed)
                    {
                        Speed += 0.4F;
                    }
                    else
                    {
                        Speed = MaxSpeed;
                    }
                    float SP = vector.Length();
                    if (SP > Speed)
                    {
                        SP = Speed;
                    }
                    //vector = vector.PerfectNormalize();
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.velocity = vector/4;
                }
            }
        }
        float Speed;
        public override void DrawEffects(ref Color drawColor)
        {
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (NPC.Dnpc().Stage == 4)
            {
                modifiers.SourceDamage *= 0.2f;

            }
                
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
            player.AddBuff(24, 240, true);
            player.AddBuff(36, 240, true);
            player.AddBuff(30, 240, true);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D textureGlow = texture2_Glow.Value;
            Color color = NPC.GetAlpha(new Color(254, 62, 3, 0));
            if (NPC.Dnpc().Stage == 4)
            {
                texture = texture2.Value;

                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, color, NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null,  color,NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null,  color,NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null,  color,NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(100, 100, 100, 255)),NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0) * 0.25f),NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
            }
            if (NPC.Dnpc().Stage != 4 || NPC.ai[0] < 100)
            {
                texture = TextureAssets.Npc[Type].Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor),NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                if ((float)NPC.life / NPC.lifeMax <= 0.5F && NPC.ai[0] < 400)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame),  color,NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame),  color,NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame),  color,NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame),  color,NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                }
            }

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.Dnpc().Stage != 4)
                spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(255, 155, 155, 0)),NPC.rotation+ MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);

            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
        }
    }
}
