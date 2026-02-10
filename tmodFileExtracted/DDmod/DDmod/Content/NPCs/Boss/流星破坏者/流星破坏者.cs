using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Dusts;
using Terraria.ModLoader.Utilities;
using DDmod.Content.NPCs.IittleMonster.旗子;
using Terraria.Graphics.Shaders;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Boss.夜光蘑菇王;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.Boss.流星破坏者
{
    [AutoloadBossHead]
    public class 流星破坏者 : ModNPC
    {
        public static int Head;
        public override void BossHeadSlot(ref int index)
        {
            Head = ModContent.GetModBossHeadSlot("DDmod/Content/NPCs/Boss/流星破坏者/流星破坏者2_Head_Boss");
            if (NPC.Dnpc().Stage >= 2)
            {
                index = Head;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
                DDSystem.HBar(NPC.type, "流星破坏者", new Vector2(-111114, -2));
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 0,
                PortraitPositionXOverride = -52F,
                PortraitScale = 0.75f,
                Scale = 0.5F,
                Position = new Vector2(7.5F, -60),
                Rotation = 0F,
                Direction = 1,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Extra;
        public static Asset<Texture2D> Extra2;
        public static Asset<Texture2D> Extra3;
        public static Asset<Texture2D> Shoulder;
        public static Asset<Texture2D> ShoulderGlow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
                Extra = ModContent.Request<Texture2D>(Texture + "_Extra");
                Extra2 = ModContent.Request<Texture2D>(Texture + "_Extra2");
                Extra3 = ModContent.Request<Texture2D>(Texture + "_Extra3");
                Shoulder = ModContent.Request<Texture2D>(Texture + "_Shoulder");
                ShoulderGlow = ModContent.Request<Texture2D>(Texture + "_Shoulder_Glow");
            }
        }

        public override void SetDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.75f,
                Scale = 0.5F,
                Rotation = 0F,
                Direction = 1,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;

            NPC.damage = 50;
            NPC.width = 140;
            NPC.height = 140;
            NPC.aiStyle = -1;
            NPC.defense = 40;
            NPC.scale = 1f;
            NPC.lifeMax = 18000;
            NPC.knockBackResist = 0f;
            NPC.value = 100000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.Dnpc().Properties.Iron = true;
            NPC.NPCHB().Multiple = true;
            if (!Main.dedServ)
            {
                Music = DDSystem.Music(2, "流星歼灭者");
                NPC.NPCHB().HealthBarFrame(new Vector2(4, 8), new Vector2(4, 8), new Vector2(4, 8), new Vector2(4, 8));
            }
            NPC.Dnpc().Properties.BossLife = 1.25F;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }

        bool ST2;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            NPC.rotation = NPC.velocity.X * 0.03F;
            if (!Main.dedServ)
            {
                if (NPC.Dnpc().Stage >= 2 && !ST2)
                {
                    NPC.NPCHB().Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星破坏者/Head2");
                    NPC.NPCHB().Mid2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星破坏者/Mid2");
                    NPC.NPCHB().Tail2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星破坏者/Tail2");
                    NPC.NPCHB().Fill2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星破坏者/Fill2");
                    NPC.NPCHB().Lock2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星破坏者/Lock2");
                    ST2 = true;
                }
            }
            //地盘位置
            if (NPC.Dnpc().vector[2] == Vector2.Zero)
            {
                NPC.Dnpc().vector[2] = NPC.Center + new Vector2(0, 120 * NPC.scale).RotatedBy(NPC.rotation);
            }
            else
            {
                if (NPC.ai[0] < 120 && NPC.Dnpc().Stage == 0)
                {
                    NPC.Dnpc().vector[2] = NPC.Center + new Vector2(0, 120 * NPC.scale).RotatedBy(NPC.rotation) + NPC.velocity;
                }
                Vector2 vector2 = NPC.Dnpc().vector[2] - (NPC.Center + new Vector2(0, 120 * NPC.scale).RotatedBy(NPC.rotation));
                NPC.Dnpc().vector[2] -= vector2 / 2;
            }
            if (player.dead && NPC.target != -1)
            {
                NPC.velocity.Y -= 1;
                if (NPC.timeLeft > 25)
                {
                    NPC.timeLeft = 25;
                }
                return;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.ai[1] == 0 || vector.X > 500)
                {
                    NPC.ai[1] = -500;
                }
                if (vector.X < -500)
                {
                    NPC.ai[1] = 500;
                }
                vector = player.Center + new Vector2(-NPC.ai[1], -350) - NPC.Center;
                NPC.dontTakeDamage = true;
                if (NPC.ai[0] == 0)
                {
                    NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(40, -40), ModContent.NPCType<流星大炮>(), 0);
                    NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(40, -40), ModContent.NPCType<流星激光枪>(), 0);
                }
                if (!NPC.AnyNPCs(ModContent.NPCType<流星大炮>()) && !NPC.AnyNPCs(ModContent.NPCType<流星激光枪>()))
                {
                    NPC.Dnpc().Stage = 1;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.netUpdate = true;
                    return;
                }
                if (NPC.ai[0]++ < 52)
                {
                    NPC.velocity.Y += 0.5f;
                }
                else if (NPC.ai[0] < 120)
                {
                    NPC.velocity.Y *= 0.96f;

                }
                else
                {
                    if (NPC.ai[0] % 120 == 0)
                    {

                        if (Main.netMode != 1)
                        {
                            for (int a = -1; a <= 1; a++)
                            {
                                if (a != 0)
                                {
                                    Vector2 vector1 = new Vector2(0, -1).RotatedBy(-0.6f * a + NPC.rotation) * 15;
                                    NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector1 * 8, ModContent.NPCType<破坏者导弹>(), 0)];
                                    npc.velocity = vector1;
                                }
                            }
                        }
                        PlaySound(SoundID.Item12, NPC.position);
                    }
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -7)
                        {
                            NPC.velocity.X -= 0.3F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X < 7)
                        {
                            NPC.velocity.X += 0.3F;
                        }
                    }
                    if (vector.Y < 0)
                    {
                        if (NPC.velocity.Y > -7)
                        {
                            NPC.velocity.Y -= 0.3F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < 7)
                        {
                            NPC.velocity.Y += 0.3F;
                        }
                    }
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.dontTakeDamage = true;
                vector = player.Center + new Vector2(0, -350) - NPC.Center;

                if (NPC.ai[0] <= 0)
                {
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -15)
                        {
                            NPC.velocity.X -= 1F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X < 15)
                        {
                            NPC.velocity.X += 1F;
                        }
                    }
                }
                if (vector.Y < 0)
                {
                    if (NPC.velocity.Y > -15)
                    {
                        NPC.velocity.Y -= 1F;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < 15)
                    {
                        NPC.velocity.Y += 1F;
                    }
                }
                if (vector.Length() < 150)
                {
                    if (NPC.ai[1] == 0)
                    {
                        NPC.ai[1]++;
                    }
                }
                if (NPC.ai[1] > 0)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 60)
                    {
                        NPC.ai[1] = -1;
                        NPC.netUpdate = true;
                    }
                    NPC.velocity *= 0.86F;
                    NPC.velocity.Y *= 0.86F;
                }
                else if (NPC.ai[1] < 0)
                {
                    NPC.ai[0] -= 2;
                    if (NPC.ai[0] <= 0)
                    {
                        NPC.Dnpc().Stage = 2;
                        if (Main.netMode != 1)
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Boss陨石弹>(), 500, 0, -1, 3);
                        NewDustChange4(200, NPC.position, NPC.Size, ModContent.DustType<光球粒子>(), 0, 12, true, 4, 8, 100, 1000, new Color(50, 190, 57, 0), 5);
                        Main.LocalPlayer.Dplayer().PlayerShake(10, 30);
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                }
            }
            else if (NPC.Dnpc().Stage == 2)
            {
                NPC.dontTakeDamage = false;
                if (NPC.ai[1] == 0 || vector.X > 500)
                {
                    NPC.ai[1] = -500;
                }
                if (vector.X < -500)
                {
                    NPC.ai[1] = 500;
                }
                vector = player.Center + new Vector2(-NPC.ai[1], -350) - NPC.Center;
                void Speed()
                {
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -12)
                        {
                            NPC.velocity.X -= 0.75F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X < 12)
                        {
                            NPC.velocity.X += 0.75F;
                        }
                    }
                    if (vector.Y < 0)
                    {
                        if (NPC.velocity.Y > -12)
                        {
                            NPC.velocity.Y -= 0.75F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < 12)
                        {
                            NPC.velocity.Y += 0.75F;
                        }
                    }
                }
                NPC.ai[0]++;
                if (NPC.ai[0] <= 480)
                {
                    Speed();
                    vector = player.Center - NPC.Center;
                    if (NPC.ai[0] == 300)
                    {
                        if (Main.netMode != 1)
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 1), ModContent.ProjectileType<破坏者激光束>(), 50, 0, -1, NPC.whoAmI);

                    }
                    if (NPC.ai[0] >= 180 && NPC.ai[0] <= 300 && NPC.ai[0] % 10 == 0)
                    {
                        NPC.localAI[0] = 8;
                        if (Main.netMode != 1)
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 8, ModContent.ProjectileType<Boss机械绿激光>(), 30, 0, -1, NPC.whoAmI);
                        PlaySound(SoundID.Item12, NPC.position);

                    }
                }
                else if (NPC.ai[0] <= 780)
                {
                    Speed();
                    NPC.velocity.X *= 0.86F;
                    if (NPC.ai[0] % 8 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Vector2 vector1 = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F) + NPC.rotation) * 15;
                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector1 * 8, ModContent.NPCType<破坏者导弹2>(), 0)];
                            npc.velocity = vector1;
                        }
                        PlaySound(SoundID.Item12, NPC.position);
                    }
                    if (NPC.ai[0] == 780)
                    {
                        if (Main.netMode != 1)
                        {
                            for (int a = 0; a < 8; a++)
                            {
                                NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 1).RotatedBy(MathHelper.TwoPi / 8 * a), ModContent.ProjectileType<破坏者激光束>(), 50, 0, -1, NPC.whoAmI, 0.1F);
                            }
                        }
                    }
                }
                else if (NPC.ai[0] <= 1080)
                {
                    Speed();
                    NPC.velocity.X *= 0.86F;
                    NPC.velocity.Y *= 0.86F;
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if(NPC.localAI[0]>0)
                {
                    NPC.localAI[0]--;
                }
            }
            else
            {
                if(NPC.Dnpc().Stage==4)
                {
                    NPC.Dnpc().Stage = 5;
                    NPC.NPCLoot();
                }
                NPC.dontTakeDamage = true;
                vector = player.Center - NPC.Center;

                for (int a = 0; a < 2; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(80, 80, 80, 155), Main.rand.NextFloat(1F, 2.5F))];
                    Vector2 vector2 = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 8), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                    dust.velocity = vector2 * Main.rand.NextFloat(4F);
                    dust.position += vector2.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                    dust.alpha = -Main.rand.Next(200, 1000);
                }
                if (vector.X < 0)
                {
                    if (NPC.velocity.X > -8)
                    {
                        NPC.velocity.X -= 0.5F;
                    }
                }
                else
                {
                    if (NPC.velocity.X < 8)
                    {
                        NPC.velocity.X += 0.5F;
                    }
                }
                if (vector.Y < 0)
                {
                    if (NPC.velocity.Y > -8)
                    {
                        NPC.velocity.Y -= 0.5F;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < 8)
                    {
                        NPC.velocity.Y += 0.5F;
                    }
                }
                NPC.velocity *= 0.86F;
                NPC.velocity.Y *= 0.86F;
                if (NPC.ai[1] == 0)
                {
                    NPC.ai[1]++;
                }
                if (NPC.ai[1] > 0)
                {
                    NPC.ai[0]+=0.3f;
                    if (NPC.ai[0] > 90)
                    {
                        NPC.ai[1] = -1;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[1] < 0)
                {
                    NPC.ai[0] -= 5;
                    if (NPC.ai[0] <= 0)
                    {
                        if (Main.netMode != 1)
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Boss陨石弹>(), 5000, 0, -1, 6);
                        Main.LocalPlayer.Dplayer().PlayerShake(20, 60);
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        NPC.Kill(false);
                        NPC.netUpdate = true;
                    }
                }
            }

            //NPC.velocity = Vector2.Zero;
            //NPC.position = NPC.oldPosition;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.流星破坏者")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<流星保险箱>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<流星破坏者纪念章物品>(), 10));
            //面具
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<夜光蘑菇王面具>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<流星破坏者圣物>()));
            //大师
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<流星塔信号器>(), 4));

            //特别引用,普通模式
            //普通模式
            int[] A =
            [
                ModContent.ItemType<破坏者巨刃>(),
                ModContent.ItemType<破坏者激光枪>(),
                ModContent.ItemType<流星炮>(),
                ModContent.ItemType<流星召唤杖>(),
            ];
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            NPC.frameCounter %= 64;
            NPC.frame.Width = 204;
            NPC.frame.Y = ((int)NPC.frameCounter/8) * frameHeight;
            if (NPC.Dnpc().Stage == 2)
            {
                NPC.frame.X = 204;
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(248, 66,5 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(248, 66, 5), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Color ColorFunction2(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(50, 255,57 ),
                new Color(50, 210,57 ),
                new Color(50, 190,57 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(50, 160, 57), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                50,
                60,
                70,
                40,
                30,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
        internal static Trailing TrailDrawer3;
        internal static Trailing TrailDrawer4;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer3 == null)
            {
                TrailDrawer3 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"],1);
            }
            if (TrailDrawer4 == null)
            {
                TrailDrawer4 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"], 1);
            }
            Vector2 vector = new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) - new Vector2(0, 44 * 4).RotatedBy(NPC.oldRot[1]) + NPC.velocity;
            vector *= 3;
            Vector2 vector2 = new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) - new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.oldRot[1]) + NPC.velocity;
            vector2 *= 3;

            Vector2 Pos = NPC.Center + new Vector2(44, 34).RotatedBy(NPC.rotation) * NPC.scale;
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
            Vector2[] vectors = [Pos, Pos + new Vector2(0.8f, 1).RotatedBy(NPC.rotation) * (60 + NPC.velocity.Length() * 4), Pos + new Vector2(0.8f, 1).RotatedBy(NPC.rotation) * (120 + NPC.velocity.Length() * 8) - NPC.velocity * 3];

            if (NPC.IsABestiaryIconDummy)
            {
                if (NPC.Dnpc().Stage < 2)
                    TrailDrawer3.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);

                Pos = NPC.Center + new Vector2(-44, 34).RotatedBy(NPC.rotation) * NPC.scale;
                vectors = [Pos, Pos + new Vector2(-0.8f, 1).RotatedBy(NPC.rotation) * (60 + NPC.velocity.Length() * 4), Pos + new Vector2(-0.8f, 1).RotatedBy(NPC.rotation) * (120 + NPC.velocity.Length() * 8) - NPC.velocity * 3];

                if (NPC.Dnpc().Stage < 2)
                    TrailDrawer3.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);
            }
            else
            {
                if (NPC.Dnpc().Stage < 2)
                    TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);
                else
                    TrailDrawer2.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.5f, 1, spriteBatch);

                Pos = NPC.Center + new Vector2(-44, 34).RotatedBy(NPC.rotation) * NPC.scale;
                vectors = [Pos, Pos + new Vector2(-0.8f, 1).RotatedBy(NPC.rotation) * (60 + NPC.velocity.Length() * 4), Pos + new Vector2(-0.8f, 1).RotatedBy(NPC.rotation) * (120 + NPC.velocity.Length() * 8) - NPC.velocity * 3];

                if (NPC.Dnpc().Stage < 2)
                    TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);
                else
                    TrailDrawer2.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.5F, 1, spriteBatch);
            }
            vector = NPC.Dnpc().vector[2] - (NPC.Center + new Vector2(0, 90 * NPC.scale).RotatedBy(NPC.rotation));

            Texture2D texture = Extra.Value;
            if (NPC.Dnpc().Stage < 2)
            {
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - vector * 0.33F - screenPos, new Rectangle(0, 0, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);
                texture = Extra2.Value;
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - vector * 0.66F - screenPos, new Rectangle(0, 0, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);

                texture = Extra3.Value;
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - screenPos, new Rectangle(0, 0, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);
            }
            else
            {
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - vector * 0.33F - screenPos, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);
                texture = Extra2.Value;
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - vector * 0.66F - screenPos, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);

                texture = Extra3.Value;
                spriteBatch.Draw(texture, NPC.Dnpc().vector[2] - screenPos, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), Color.White, vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), NPC.scale, 0, 0);

            }

            if (NPC.AnyNPCs(ModContent.NPCType<流星大炮>()))
            {
                //左手
                texture = Shoulder.Value;
                Rectangle rectangle = new Rectangle(0, texture.Height / 8 * ((int)NPC.frameCounter / 8), texture.Width, texture.Height / 8);
                Vector2 vector1 = new Vector2(46, 76);
                spriteBatch.Draw(texture, NPC.Center - new Vector2(92, 38).RotatedBy(NPC.rotation) * NPC.scale - screenPos, rectangle, drawColor, NPC.Dnpc().Times[3], new Vector2(texture.Width - 12, 28), NPC.scale, SpriteEffects.FlipHorizontally, 0);
                texture = ShoulderGlow.Value;
                spriteBatch.Draw(texture, NPC.Center - new Vector2(92, 38).RotatedBy(NPC.rotation) * NPC.scale - screenPos, rectangle, drawColor, NPC.Dnpc().Times[3], new Vector2(texture.Width - 12, 28), NPC.scale, SpriteEffects.FlipHorizontally, 0);
            }
            if (NPC.AnyNPCs(ModContent.NPCType<流星激光枪>()))
            {
                //右手
                texture = Shoulder.Value;
                Rectangle rectangle = new Rectangle(0, texture.Height / 8 * ((int)NPC.frameCounter / 8), texture.Width, texture.Height / 8);
                Vector2 vector1 = new Vector2(46, 76);
                spriteBatch.Draw(texture, NPC.Center - new Vector2(-92, 38).RotatedBy(NPC.rotation) * NPC.scale - screenPos, rectangle, drawColor, NPC.Dnpc().Times[4], new Vector2(12, 28), NPC.scale, 0, 0);
                texture = ShoulderGlow.Value;
                spriteBatch.Draw(texture, NPC.Center - new Vector2(-92, 38).RotatedBy(NPC.rotation) * NPC.scale - screenPos, rectangle, drawColor, NPC.Dnpc().Times[4], new Vector2(12, 28), NPC.scale, 0, 0);
            }

            texture = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects sprite = 0;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, 0, 0);
            texture = Glow.Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, 0, 0);

            if (NPC.Dnpc().Stage == 1|| NPC.Dnpc().Stage>=3)
            {
                if (NPC.ai[0] > 0)
                {
                    texture = DDTextures.Starlight3.Value;
                    spriteBatch.Draw(texture, NPC.Center- screenPos, null, new Color(50, 255, 57, 0), 0, texture.Size() / 2, NPC.scale * (NPC.ai[0] / 10) * new Vector2(1, 2), 0, 0);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * (NPC.ai[0] / 10) * new Vector2(1, 3), 0, 0);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), 0, texture.Size() / 2, NPC.scale * (NPC.ai[0] / 10) * new Vector2(1, 2), 0, 0);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * (NPC.ai[0] / 10) * new Vector2(1, 3), 0, 0);
                }
            }
            if (NPC.localAI[0]>0)
            {
                texture = DDTextures.Starlight3.Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), 0, texture.Size() / 2, NPC.scale * new Vector2(1, 2), 0, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(1, 3), 0, 0);
         
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), 0, texture.Size() / 2, NPC.scale * new Vector2(1, 2), 0, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, new Color(50, 255, 57, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(1, 3), 0, 0);
            }
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.Dnpc().Stage < 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 10);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 4);
                }
            }
            else
            {

                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(50, 190, 57, 0));
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 4);
                }
            }
            if (NPC.Dnpc().Stage < 3&&NPC.life<=0)
            {
                NPC.Dnpc().Stage = 4;
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
                NPC.life = 1000;

                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
            }
            if (hit.Damage == 10000&&hit.DamageType==DamageClass.Default)
            {
                Vector2 Center = NPC.position;
                for (int a = 0; a < 300; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(50, 190, 57, 0), Main.rand.NextFloat(2, 6))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 43), Main.rand.NextFloat(2, 117)) / 4, (Math.PI * 2 / 300) + a, default);
                    dust.velocity = vector;
                    dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 12);
                    dust.noGravity = true;
                }
                for (int a = 0; a < 120; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(80, 80, 80, 155), Main.rand.NextFloat(3F, 5.5F))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 8), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                    dust.velocity = vector * Main.rand.NextFloat(4F);
                    dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                    dust.alpha = -Main.rand.Next(3000, 6000);
                }
                int GoreType = Mod.Find<ModGore>("流星破坏者1").Type;
                Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
                GoreType = Mod.Find<ModGore>("流星破坏者2").Type;
                Gore.NewGore(NPC.GetSource_Death(), Center + new Vector2(100, 0) * NPC.scale, new Vector2(-1, -1).RotatedBy(NPC.rotation) * 5, GoreType, NPC.scale);
                GoreType = Mod.Find<ModGore>("流星破坏者3").Type;
                Gore.NewGore(NPC.GetSource_Death(), Center + new Vector2(NPC.width / 2, NPC.height), Vector2.Zero, GoreType, NPC.scale);
                GoreType = Mod.Find<ModGore>("流星破坏者4").Type;
                Gore.NewGore(NPC.GetSource_Death(), Center + new Vector2(NPC.width / 2, NPC.height), Vector2.Zero, GoreType, NPC.scale);
                GoreType = Mod.Find<ModGore>("流星破坏者5").Type;
                Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
            }
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.流星破坏者, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }
}
