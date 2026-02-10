using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.苦难;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.NPCs.Boss.恐惧缝合体
{
    public class 恐惧滋生体 : ModNPC
    {
        public static Asset<Texture2D> YJ;
        public override void Load()
        {
            YJ = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/恐惧缝合体/恐惧滋生体_YJ");
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 40;
            NPC.damage = 28;
            NPC.defense = 0;
            NPC.knockBackResist = 1f;
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.scale = 1f;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.恐惧滋生体"))
            });
        }
        public override bool PreAI()
        {
            if (NPC.ai[1]==0)
            {
                NPC.alpha = 255;
            }
            if(NPC.alpha>0)
            {
                NPC.alpha -= 20;
            }
            NPC.TargetClosest();
            //上下
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            NPC.ai[1]++;
            if (NPC.ai[1]<30)
            {
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                if (NPC.velocity.X > 0)
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                return false;
            }

            if (Math.Abs(vector.Y) > 50)
            {
                if (vector.Y < 0)
                {
                    if (NPC.velocity.Y > -6F)
                    {
                        NPC.velocity.Y -= 0.2f;
                    }
                    else
                    {
                        NPC.velocity.Y += 0.2F;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < 6F)
                    {
                        NPC.velocity.Y += 0.2f;
                    }
                    else
                    {
                        NPC.velocity.Y -= 0.2F;
                    }
                }
            }
            else
            {
                NPC.ai[0]++;
                if (NPC.ai[0] < 80)
                {
                    if (NPC.velocity.Y < 2F)
                    {
                        NPC.velocity.Y += 0.2f;
                    }
                    else
                    {
                        NPC.velocity.Y -= 0.2f;
                    }
                }
                else
                {
                    if (NPC.velocity.Y > -2F)
                    {
                        NPC.velocity.Y -= 0.2f;
                    }
                    else
                    {
                        NPC.velocity.Y += -0.2f;
                    }
                    if (NPC.ai[0] >= 160)
                    {
                        NPC.ai[0] = 0;
                    }
                }
            }

            //左右移动
            if (vector.X < 0)
            {
                if (NPC.velocity.X > -4)
                {
                    NPC.velocity.X -= 0.05f;
                }
                else
                {
                    NPC.velocity.X = -4;
                }
            }
            else
            {
                if (NPC.velocity.X < 4)
                {
                    NPC.velocity.X += 0.05f;
                }
                else
                {
                    NPC.velocity.X = 4;
                }
            }
            NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.02F);
            return false;
        }

        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 100, default, 1.2f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 220; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, 5, Main.rand.NextFloat(-5,5), Main.rand.NextFloat(-5, 5), 100, default, 1.5f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("恐惧滋生体1").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), GoreType, NPC.scale);
                    
                    GoreType = Mod.Find<ModGore>("恐惧滋生体2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), GoreType, NPC.scale);

                    GoreType = Mod.Find<ModGore>("恐惧滋生体3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), GoreType, NPC.scale);

                    GoreType = Mod.Find<ModGore>("恐惧滋生体4").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), GoreType, NPC.scale);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 8 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<苦难之魂>(), 50,1,1));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            SpriteEffects spriteEffects = 0;
            if (NPC.velocity.X > 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Vector2 ve = Vector2.Zero;
            if(NPC.frame.Y == NPC.frame.Height*3)
            {
                ve.X += 2;
            }
            if(NPC.frame.Y == NPC.frame.Height*1)
            {
                ve.Y -= 2;
            }
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            drawColor = NPC.GetAlpha(drawColor);
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size()/2, NPC.scale, spriteEffects, 0f);
            Vector2 vector = player.Center- (NPC.Center + (new Vector2(ve.X, 10+ ve.Y)* NPC.scale).RotatedBy(NPC.rotation));
            spriteBatch.Draw(YJ.Value, NPC.Center + (new Vector2(ve.X, 10 + ve.Y)* NPC.scale).RotatedBy(NPC.rotation)+(vector.PerfectNormalize()*4* NPC.scale) - screenPos, null, drawColor, NPC.rotation, YJ.Value.Size()/2, NPC.scale*0.75f, spriteEffects, 0f);
            return false;
        }
    }
}