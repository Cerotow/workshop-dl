using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.Boss.流星破坏者
{
    public class 流星激光枪 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DDSystem.HBar(NPC.type, "流星破坏者", new Vector2(-111114, -2), "手");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 0,
                PortraitPositionXOverride = -20F,
                PortraitScale = 0.65f,
                Scale = 0.3F,
                Position = new Vector2(-10F, -0),
                Rotation = 0.4F,
                Direction = 0,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Arm;
        public static Asset<Texture2D> ArmGlow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            Arm = ModContent.Request<Texture2D>(Texture + "_Arm");
            ArmGlow = ModContent.Request<Texture2D>(Texture + "_Arm_Glow");
            }
        }

        public override void SetDefaults()
        {

            NPC.damage = 50;
            NPC.width = 84;
            NPC.height = 84;
            NPC.aiStyle = -1;
            NPC.defense = 40;
            NPC.scale = 1f;
            NPC.lifeMax = 12000;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontCountMe = true;
            NPC.Dnpc().Properties.Iron = true;
            NPC.hide = true;
            NPC.Dnpc().BossPhysique = true;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            NPC.NPCHB().Child = true;
            NPC.Dnpc().Properties.BossLife = 1.25F;
        }


        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            NPC parent = Main.npc[NPC.Dnpc().Master];
            if (parent.active && parent.type == ModContent.NPCType<流星破坏者>())
            {
                Vector2 vector3 = player.Center - parent.Center;
                vector3.Y *= 2;
                if (NPC.Dnpc().vector[0] == Vector2.Zero)
                {
                    NPC.Dnpc().vector[0] = parent.Center - new Vector2(-92, 38).RotatedBy(parent.rotation) * parent.scale;
                }
                //手臂位置
                Vector2 ArmCen = parent.Center - new Vector2(-92, 38).RotatedBy(parent.rotation) * parent.scale;
                Vector2 vector2 = NPC.Center - (ArmCen + new Vector2(140, 200));


                NPC.velocity = -vector2 / 8;
                vector2 = ArmCen + (NPC.Center - ArmCen) / 4;
                NPC.Dnpc().vector[0] -= (NPC.Dnpc().vector[0] - vector2) / 4;
                parent.Dnpc().Times[4] = (NPC.Dnpc().vector[0] - ArmCen).ToRotation();

                if (parent.Dnpc().Times[4] < 0f)
                {
                    parent.Dnpc().Times[4] += MathHelper.TwoPi;
                }
                else if (parent.Dnpc().Times[4] > MathHelper.TwoPi)
                {
                    parent.Dnpc().Times[4] -= MathHelper.TwoPi;
                }
                if (parent.Dnpc().Times[4] > MathHelper.Pi + MathHelper.PiOver2)
                {
                    parent.Dnpc().Times[4] = 0;
                }
                if (parent.Dnpc().Times[4] > MathHelper.PiOver2)
                {
                    parent.Dnpc().Times[4] = MathHelper.PiOver2;
                }
                parent.Dnpc().Times[4] -= MathHelper.PiOver4;
                if (NPC.ai[0] < 60)
                {
                    NPC.RotationSpeed(vector.ToRotation(), 0.2F);
                }
                else
                {
                    NPC.RotationSpeed(vector.ToRotation(), 0.01F);
                }
                    if (parent.ai[0] > 120)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] % 10 == 0 && NPC.ai[0] < 60)
                    {
                        if (Main.netMode != 1)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * 120, NPC.rotation.ToRotationVector2() * 10, ModContent.ProjectileType<Boss机械橙激光>(), 20, 0);
                        }

                        NPC.ai[1] = 5;
                        PlaySound(SoundID.Item12, NPC.position);
                        ShootFrame = true;
                    }
                    if (NPC.ai[0]==60)
                    {
                        if (Main.netMode != 1)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * 120, NPC.rotation.ToRotationVector2() * 10, ModContent.ProjectileType<破坏者激光束>(), 30, 0,-1,NPC.whoAmI);
                        }

                    }
                    if (NPC.ai[0] > 300)
                    {
                        NPC.ai[0] = 0;
                    }
                    if (NPC.ai[1] > 0)
                    {
                        NPC.ai[1]--;
                    }
                }

                if (vector.X > 0)
                {
                    NPC.spriteDirection = 0;
                }
                else
                {
                    NPC.spriteDirection = 1;
                }
            }
            else
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<流星破坏者>()) || (NPC.Dnpc().Master != 0))
                {
                    NPC.Kill(false);
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
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
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.流星激光枪")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 10);
                Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 4);
            }
            if (NPC.life <= 0)
            {
                NPC parent = Main.npc[NPC.Dnpc().Master];
                {
                    Vector2 Center = NPC.Center;
                    for (int a = 0; a < 120; a++)
                    {
                        Dust dust = Main.dust[NewDust(Center, 1, 1, 6, 0f, 0f, 0, default, 1.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 23), Main.rand.NextFloat(2, 117)) / 4, (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 12);
                        dust.noGravity = true;
                    }
                    for (int a = 0; a < 60; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(80, 80, 80, 155), Main.rand.NextFloat(2F, 4.5F))];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                        dust.velocity = vector * Main.rand.NextFloat(4F);
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                        dust.alpha = -Main.rand.Next(3000, 6000);
                    }
                    int GoreType = Mod.Find<ModGore>("流星激光枪").Type;
                    Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
                }
                if (parent.active && parent.type == ModContent.NPCType<流星破坏者>())
                {
                    Vector2 Center = NPC.Dnpc().vector[0];
                    for (int a = 0; a < 120; a++)
                    {
                        Dust dust = Main.dust[NewDust(Center, 1, 1, 6, 0f, 0f, 0, default, 1.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 23), Main.rand.NextFloat(2, 117)) / 4, (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 12);
                        dust.noGravity = true;
                    }
                    for (int a = 0; a < 60; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(80, 80, 80, 155), Main.rand.NextFloat(2F, 4.5F))];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                        dust.velocity = vector * Main.rand.NextFloat(4F);
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                        dust.alpha = -Main.rand.Next(3000, 6000);
                    }
                    int GoreType = Mod.Find<ModGore>("流星手臂3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
                }
                if (parent.active && parent.type == ModContent.NPCType<流星破坏者>())
                {
                    Vector2 Center = parent.Center - new Vector2(-92, 38).RotatedBy(parent.rotation) * parent.scale;
                    for (int a = 0; a < 120; a++)
                    {
                        Dust dust = Main.dust[NewDust(Center, 1, 1, 6, 0f, 0f, 0, default, 1.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 23), Main.rand.NextFloat(2, 117)) / 4, (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 12);
                        dust.noGravity = true;
                    }
                    for (int a = 0; a < 60; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(80, 80, 80, 155), Main.rand.NextFloat(2F, 4.5F))];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                        dust.velocity = vector * Main.rand.NextFloat(4F);
                        dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                        dust.alpha = -Main.rand.Next(3000, 6000);
                    }
                    int GoreType = Mod.Find<ModGore>("流星肩膀2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("流星肩膀3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), Center, Vector2.Zero, GoreType, NPC.scale);
                }
            }
        }

        private bool ShootFrame;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 306;
            if (ShootFrame)
            {
                NPC.frame.X = NPC.frame.Width;
                NPC.frame.Y = 0;
                NPC.frameCounter = 0;
                ShootFrame = false;
            }
            NPC.frameCounter++;
            if (NPC.frameCounter > 8)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.X > 0 && NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = 0;
                NPC.frame.X = 0;
            }
            if (NPC.frame.Y >= frameHeight * 8)
            {
                NPC.frame.Y = 0;
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return new Color(252, 128, 48);
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
            return 30;
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
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
            Vector2 Center = NPC.Center;

            NPC parent = Main.npc[NPC.Dnpc().Master];
            Texture2D texture = Arm.Value;
            float ArmRot = (Center - NPC.Dnpc().vector[0]).ToRotation() - 0.4f;
            if (parent.active && parent.type == ModContent.NPCType<流星破坏者>())
            {
                Rectangle rectangle = new Rectangle(0, texture.Height / 8 * ((int)parent.frameCounter / 8), texture.Width, texture.Height / 8);
                Main.spriteBatch.End();
                RasterizerState state = new RasterizerState()
                {
                    CullMode = CullMode.CullCounterClockwiseFace,
                    ScissorTestEnable = true,
                };
                Vector2 ArmCen = parent.Center - new Vector2(-92, 38).RotatedBy(parent.rotation) * parent.scale;
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);
                Vector2 Pos = ArmCen + parent.Dnpc().Times[4].ToRotationVector2().PerfectNormalize().RotatedBy(MathHelper.PiOver4 + 0.15f) * 64;

                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
                Vector2 vector = NPC.Dnpc().vector[0] + (Center - NPC.Dnpc().vector[0]).RotatedBy(parent.rotation + 0.1f).PerfectNormalize() * 28;
                Vector2[] vectors = [Pos, NPC.Dnpc().vector[0] + (Pos - NPC.Dnpc().vector[0]) / 2 + NPC.velocity * 4, vector, vector, vector, vector];

                TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale, 1, spriteBatch);

                spriteBatch.Draw(texture, NPC.Dnpc().vector[0] - screenPos, rectangle, drawColor, ArmRot, new Vector2(0, 0), NPC.scale, 0, 0);
                texture = ArmGlow.Value;
                spriteBatch.Draw(texture, NPC.Dnpc().vector[0] - screenPos, rectangle, Color.White, ArmRot, new Vector2(0, 0), NPC.scale, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Pos - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Pos - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);

                Vector2 Pos2 = NPC.Dnpc().vector[0] - (ArmRot + MathHelper.Pi + 0.5F).ToRotationVector2() * 104;
                Vector2[] vectors2 = [Pos2, Center + (Pos2 - Center) / 2 + NPC.velocity * 10, Center, Center, Center, Center];
                TrailDrawer.Draw(vectors2, -screenPos, 104, null, NPC.scale, 1, spriteBatch);

                spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Pos2 - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Center - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Center - screenPos, null, new Color(252, 128, 48, 155), 0, DDTextures.VoidStar.Size() / 2, NPC.scale / 2, 0, 0);

            }

            if (NPC.spriteDirection == 0)
            {
                texture = TextureAssets.Npc[NPC.type].Value;
                spriteBatch.Draw(texture, Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, 0, 0);
                texture = Glow.Value;
                spriteBatch.Draw(texture, Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, 0, 0);
            }
            else
            {
                texture = TextureAssets.Npc[NPC.type].Value;
                spriteBatch.Draw(texture, Center - screenPos, NPC.frame, drawColor, NPC.rotation + MathHelper.Pi, NPC.frame.Size()/2, NPC.scale, SpriteEffects.FlipHorizontally, 0);
                texture = Glow.Value;
                spriteBatch.Draw(texture, Center - screenPos, NPC.frame, Color.White, NPC.rotation + MathHelper.Pi, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.FlipHorizontally, 0);
            }

            if (NPC.ai[1] > 0)
            {
                texture = DDTextures.Starlight3.Value;
                spriteBatch.Draw(texture, Center + NPC.rotation.ToRotationVector2() * 150 - screenPos, null, new Color(252, 128, 48, 0), 0, texture.Size() / 2, NPC.scale * new Vector2(1, 1.5F) * 0.5F, 0, 0);
                spriteBatch.Draw(texture, Center + NPC.rotation.ToRotationVector2() * 150 - screenPos, null, new Color(252, 128, 48, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(1, 4) * 0.5F, 0, 0);
                spriteBatch.Draw(texture, Center + NPC.rotation.ToRotationVector2() * 150 - screenPos, null, new Color(252, 128, 48, 0), 0, texture.Size() / 2, NPC.scale * new Vector2(1, 1.5F) * 0.75F, 0, 0);
                spriteBatch.Draw(texture, Center + NPC.rotation.ToRotationVector2() * 150 - screenPos, null, new Color(252, 128, 48, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(1, 4) * 0.75F, 0, 0);

            }
            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White, 0, Vector2.Zero, NPC.Size / 2, 0, 0);
            return false;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        }
    }
}
