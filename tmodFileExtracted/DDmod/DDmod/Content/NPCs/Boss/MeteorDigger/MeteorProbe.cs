using DDmod.Content.Projectiles.Boss;

namespace DDmod.Content.NPCs.Boss.MeteorDigger
{
    public class MeteorProbe : ModNPC
    {
        public static Asset<Texture2D> NPCTexture;
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                NPCTexture = ModContent.Request<Texture2D>(Texture);
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Meteor Probe");
           //DisplayName.AddTranslation(7, "流星探测器");
        }

        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPC.damage = 0;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.scale = 1.3f;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.netAlways = true;
            NPC.Dnpc().Properties.Iron = true;
            NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.MeteorProbe"))
            });
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 direction = player.Center - NPC.Center;
            direction.Normalize();
            direction *= 12f;
            NPC.rotation = direction.ToRotation() - MathHelper.PiOver2;

            if (!Main.player[NPC.target].dead)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] >= 120)
                {
                    if (NPC.ai[0] % (Main.masterMode ? 60 : 20) == 0)
                    {
                        Vector2 vector = Utils.RotatedBy(direction, Main.rand.NextFloat(-0.1f, 0.1f), default);
                        Projectile A = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Vector2.Normalize(direction) * 2, vector, ModContent.ProjectileType<BossGreenLaser>(), 12, 0, 0)];
                        A.friendly = false;
                        A.hostile = true;
                        PlaySound(SoundID.Item12, NPC.position);
                    }
                }
                if (NPC.ai[0] > 200)
                {
                    NPC.ai[0] = 0;
                }
                float Q = (player.Center - NPC.Center).Length() / 10;
                float E = Q - 40;
                if (E > 10)
                {
                    E = 10;
                }
                Vector2 vector4 = Vector2.Subtract(player.Center/*调整npc要去的位置*/, NPC.Center);
                vector4.Normalize();
                vector4 *= E;//速度
                NPC.velocity = (NPC.velocity * 9 + vector4) / 10;
                NPC.rotation = (float)Math.Atan2(direction.Y, direction.X) + 1.57f;
            }
            else
            {
                NPC.velocity.Y -= 0.2f;
                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
            }
            float spacing = NPC.width * NPC.scale * 1.5f;
            float idleAccel = 0.2f;
            for (int k = 0; k < 200; k++)
            {
                NPC npc2 = Main.npc[k];
                if (k != NPC.whoAmI && npc2.active && npc2.type == NPC.type && Math.Abs(NPC.position.X - npc2.position.X) + Math.Abs(NPC.position.Y - npc2.position.Y) < spacing)
                {
                    if (NPC.position.X < Main.npc[k].position.X)
                    {
                        NPC.velocity.X -= idleAccel;
                    }
                    else
                    {
                        NPC.velocity.X += idleAccel;
                    }
                    if (NPC.position.Y < Main.npc[k].position.Y)
                    {
                        NPC.velocity.Y -= idleAccel;
                    }
                    else
                    {
                        NPC.velocity.Y += idleAccel;
                    }
                }
            }
        }

        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Vector2 vector2 = new Vector2(4, 0);
                vector2 = Utils.RotatedBy(vector2, NPC.rotation, default);
                for (int a = 0; a < 100; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 2.5f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(3, 8), Main.rand.NextFloat(3, 8)), (Math.PI * 2 / a) + a, default);
                    dust.velocity *= vector;
                }
                int GoreType = Mod.Find<ModGore>("MeteorProbe").Type;
                int GoreType2 = Mod.Find<ModGore>("MeteorProbe2").Type;
                int GoreType3 = Mod.Find<ModGore>("MeteorProbe3").Type;

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation).ToRotationVector2() * 8, GoreType2, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.Pi).ToRotationVector2() * 8, GoreType2, NPC.scale);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 4), Main.rand.NextFloat(-3, 4)), GoreType, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 4), Main.rand.NextFloat(-3, 4)), GoreType3, NPC.scale);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            DDHelper.BackAndForth(0,0.3F,0.05f,ref NPC.localAI[0], ref NPC.Dnpc().Bool[0]);
            /*if (NPC.localAI[0] < 0f)
            {
                NPC.localAI[0] = 0f;
                NPC.localAI[1] = 1;
            }
            if (NPC.localAI[0] > 0.3f)
            {
                NPC.localAI[0] = 0.3f;
                NPC.localAI[1] = 0;
            }
            if (NPC.localAI[1] == 1)
            {
                NPC.localAI[0] += 0.05f;
            }
            else
            {
                NPC.localAI[0] -= 0.05f;
            }*/
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Vector2 vector = NPC.Center - screenPos + ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -6* NPC.scale);

            Texture2D Scanning = DDTextures.Scanning.Value;
            spriteBatch.Draw(Scanning, NPC.Center - screenPos, null, new Color(0, 255, 0, 0), NPC.rotation + MathHelper.PiOver4 + MathHelper.Pi, new Vector2(0, 0), 0.6F* NPC.scale + NPC.scale * (NPC.localAI[0] / 3), 0, 0f);
            for (int a = 0; a < 5; a++)
            {
                spriteBatch.Draw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(253, 62, 3, 0) * 1f, NPC.rotation - MathHelper.PiOver2, new Vector2((int)(VoidStar.Width / 1.7F), VoidStar.Height / 2), new Vector2(NPC.scale / 2 * (0.5F + NPC.localAI[0]) * 2, NPC.scale / 3.5F) * (0.5F + NPC.localAI[0]), 0, 0f);
            }
            //spriteBatch.Draw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(2, 200, 252, 0) * 0.8f, NPC.rotation - MathHelper.PiOver2, VoidStar.Size()/2, new Vector2(NPC.scale * 1.25f, NPC.scale / 2.5F) / 3F * NPC.localAI[0], 0, 0f);
            spriteBatch.Draw(NPCTexture.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(NPCTexture.Width() / 2, NPCTexture.Height() / 2), NPC.scale, 0, 0f);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D Scanning = DDTextures.Scanning.Value;
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), new Color(10, 255, 10, 0), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), new Color(10, 255, 10, 0), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), new Color(10, 255, 10, 0), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}