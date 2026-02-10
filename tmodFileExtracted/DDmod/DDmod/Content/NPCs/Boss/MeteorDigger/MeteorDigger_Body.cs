using DDmod.Content.Projectiles.Boss;

namespace DDmod.Content.NPCs.Boss.MeteorDigger
{
    [AutoloadBossHead]
    public class MeteorDigger_Body : ModNPC
    {
        public static Asset<Texture2D> Body;
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Body = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Body");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Body_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meteor Excavater");
            //DisplayName.AddTranslation(7, "流星掘地者");
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
            NPC.damage = 25;
            NPC.defense = 16;
            NPC.lifeMax = 6000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.dontCountMe = true;
            NPC.netAlways = true;
            NPC.scale = 1.5f;
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().Properties.Iron = true;
            NPC.alpha = 255;
            NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public override string BossHeadTexture => "DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Body_Boss";
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation + MathHelper.PiOver2;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }

        public override void DrawEffects(ref Color drawColor)
        {
            if (NPC.localAI[2] > 0 && NPC.localAI[3] % 60 <= NPC.Dnpc().Times[4])
            {
                drawColor = new Color(255, 0, 0);
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void AI()
        {
            NPC HostNPC = Main.npc[(int)NPC.ai[1]];

            Lighting.AddLight(NPC.Center, 2.48f * 0.3f, 0.66f * 0.3f, 0.05f * 0.3f);
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
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                else if (NPC.alpha > 0)
                {
                    NPC.alpha = (int)(255 - (NPC.Dnpc().vector[0] - NPC.Center).Length() / 1.5f);
                }
            }
            //保持距离
            if (NPC.ai[1] < (double)Main.npc.Length && HostNPC.active)
            {
                float ro = DDHelper.AngleDifference(NPC.rotation, HostNPC.rotation);
                NPC.position -= (HostNPC.rotation).ToRotationVector2() * Math.Abs(ro) * 10;
                Vector2 vector = HostNPC.Center - NPC.Center;
                NPC.rotation = (float)Math.Atan2(vector.Y, vector.X);

                float Distance = (vector.Length() - (36 * NPC.scale)) / vector.Length();
                if (HostNPC.type == ModContent.NPCType<MeteorDigger_Head>())
                {
                    Distance = (vector.Length() - (66 * NPC.scale)) / vector.Length();
                }
                NPC.velocity = Vector2.Zero;
                NPC.position = NPC.position + vector * Distance;
            }
            if (NPC.localAI[2] == 0)
            {
                if (!HostNPC.active || (HostNPC.type != ModContent.NPCType<MeteorDigger_Head>() && HostNPC.type != NPC.type))
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
                AI1(NPC);
                return;
            }
            if (Main.netMode == 2)
            {
                NPC.Dnpc().netUpdate = true;
            }
            if (!Main.npc[(int)NPC.ai[0]].active && (Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<MeteorDigger_Body>() || Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<MeteorDigger_Tail>()))
            {
                /*Main.npc[(int)NPC.ai[0]].life = NPC.life;
                Main.npc[(int)NPC.ai[0]].active = true;
                Main.npc[(int)NPC.ai[0]].ai[1] = NPC.whoAmI;
                Main.npc[(int)NPC.ai[0]].localAI[2] = 1;
                Main.npc[(int)NPC.ai[0]].dontTakeDamage = true;
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, (int)NPC.ai[0], 0f, 0f, 0f, 0, 0, 0);*/
            }
            for (int a = 0; a < 200; a++)
            {
                if ((Main.npc[a].type == ModContent.NPCType<MeteorDigger_Head>() || Main.npc[a].type == ModContent.NPCType<MeteorDigger_Body>() || Main.npc[a].type == ModContent.NPCType<MeteorDigger_Tail>()) && (Main.npc[a].realLife == NPC.realLife || NPC.whoAmI == Main.npc[a].realLife))
                {
                    if (!Main.npc[a].dontTakeDamage)
                        Main.npc[a].dontTakeDamage = true;
                    //Main.npc[a].life = 1000;
                    if (Main.npc[a].localAI[2] == 0)
                    {
                        Main.npc[a].localAI[2] = 1;
                    }
                }
            }
            if (Main.npc[(int)NPC.ai[3]].localAI[2] == 0)
            {
                Main.npc[(int)NPC.ai[3]].localAI[2] = 1;
            }
            NPC.realLife = -1;

            NPC.Dnpc().Times[0] = 0;
            NPC.localAI[3] += Main.rand.NextFloat(3);
            if (NPC.Dnpc().Times[4]% 2 == 1 && Main.rand.NextBool(10))
            {
                CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(255, 0, 0), "! ! !", true, false);
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
            if (!HostNPC.active && !Main.npc[(int)NPC.ai[3]].active)
            {
                Main.LocalPlayer.Dplayer().PlayerShake(2, 12);
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 120, false, 0.2f);
                NPC.noTileCollide = false;
            }
            HostNPC = Main.npc[(int)NPC.ai[3]];
            if (!HostNPC.active || HostNPC.type != ModContent.NPCType<MeteorDigger_Head>())
            {
                NPC.Dnpc().Times[4] -= 0.1F;
                if (NPC.Dnpc().Times[4] <= 0)
                {
                    NPC.Dnpc().Deathrattle = false;
                    NPC.StrikeInstantKill();
                }
            }
        }
        float B = 60;

        private void AI1(NPC npc)
        {
            Player player = Main.player[Main.npc[(int)npc.ai[3]].target];
            NPC.Dnpc().Times[0]++;
            if (CountNPCS(ModContent.NPCType<MeteorProbe>()) < 10)
            {
                NPC.Dnpc().Times[2]++;
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.Dnpc().Times[2] == 300 * npc.ai[2])
                {
                    NewNPC(NPC.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<MeteorProbe>(), npc.whoAmI, npc.whoAmI);
                }
            }
            if (NPC.Dnpc().Times[2] > 24000)
            {
                NPC.Dnpc().Times[2] = 0;
            }

            if (NPC.Dnpc().Times[0] >= 600)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % 3 == NPC.Dnpc().Times[3])
                {
                    Vector2 vector = player.Center - npc.Center;
                    vector.Normalize();
                    Projectile C = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), npc.Center, vector * 7, ModContent.ProjectileType<BossGreenLaser>(), 20, 0, 0)];
                    C.friendly = false;
                    C.hostile = true;
                    C.tileCollide = false;
                    PlaySound(SoundID.Item12, NPC.position);
                }
                NPC.Dnpc().Times[3]++;
                NPC.Dnpc().Times[0] = 0;
            }
            if (NPC.Dnpc().Times[3] >= 3)
            {
                NPC.Dnpc().Times[3] = 0;
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (!NPC.Dnpc().Deathrattle)
                {
                    for (int a = 0; a < 100; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 2.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(10, 20), Main.rand.NextFloat(10, 20)), (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                    }
                    int GoreType = Mod.Find<ModGore>("MeteorDigger").Type;
                    int GoreType2 = Mod.Find<ModGore>("MeteorDigger2").Type;

                    if (Main.rand.NextBool(2))
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 12, GoreType, 1.5f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 12, GoreType2, 1.5f);
                    }
                    else
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 12, GoreType2, 1.5f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 12, GoreType, 1.5f);
                    }
                }
                else
                {
                    NPC.dontTakeDamage = true;
                    NPC.localAI[2]++;
                    NPC.life = Main.npc[(int)NPC.ai[3]].life;
                }
            }
        }
        public override bool CheckDead()
        {
            if (NPC.Dnpc().Deathrattle)
            {
                NPC.life = Main.npc[(int)NPC.ai[3]].life;
            }
            return !NPC.Dnpc().Deathrattle;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Body.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, new Vector2(Body.Width() / 2, Body.Height() / 2), NPC.scale, 0, 0f);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}
