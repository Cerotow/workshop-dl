namespace DDmod.Content.NPCs.Boss.MeteorDigger
{
    [AutoloadBossHead]
    public class MeteorDigger_Tail : ModNPC
    {
        public static Asset<Texture2D> Tail;
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Tail = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Tail");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Tail_Glow");
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
            NPC.damage = 15;
            NPC.defense = 18;
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
        public override string BossHeadTexture => "DDmod/Content/NPCs/Boss/MeteorDigger/MeteorDigger_Tail_Boss";
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }
        public float Timer4 = 60;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Timer4);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Timer4 = reader.ReadFloat();
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation + MathHelper.PiOver2;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        bool Bool2;
        public override void AI()
        {
            if (Main.netMode == 2)
            {
                NPC.Dnpc().netUpdate = true;
            }
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
            if (NPC.ai[1] < (double)Main.npc.Length&& HostNPC.active)
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
                if (!HostNPC.active || HostNPC.type != ModContent.NPCType<MeteorDigger_Body>())
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
                return;
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
            NPC.localAI[3] += Main.rand.NextFloat(3);
            if (Timer4 % 2 == 1 && Main.rand.NextBool(10))
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
            if (!HostNPC.active&&!Main.npc[(int)NPC.ai[3]].active)
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
        public override void DrawEffects(ref Color drawColor)
        {
            if (NPC.localAI[2] > 0 && NPC.localAI[3] % 60 <= Timer4)
            {
                drawColor = new Color(255, 0, 0);
            }
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (NPC.Dnpc().Deathrattle)
                {
                    NPC.dontTakeDamage = true;
                    NPC.localAI[2]++;
                    NPC.life = Main.npc[(int)NPC.ai[3]].life;
                }
            }
        }
        public override bool CheckDead()
        {
            if (!NPC.Dnpc().Deathrattle)
            {
                for (int a = 0; a < 100; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 2.5f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(10, 20), Main.rand.NextFloat(10, 20)), (Math.PI * 2 / a) + a, default);
                    dust.velocity *= vector;
                }
                int GoreType = Mod.Find<ModGore>("MeteorDigger4").Type;
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 12, GoreType, 1.5f);
            }
            return !NPC.Dnpc().Deathrattle;
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(36, 120, true);
            player.AddBuff(30, 120, true);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Tail.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, new Vector2(Tail.Width() / 2, Tail.Height() / 2), NPC.scale, 0, 0f);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}