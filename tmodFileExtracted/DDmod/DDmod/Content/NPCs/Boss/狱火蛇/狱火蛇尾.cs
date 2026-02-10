using Terraria;
namespace DDmod.Content.NPCs.Boss.狱火蛇
{
   [AutoloadBossHead]
    public class 狱火蛇尾 : ModNPC
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
            NPC.damage = 50;
            NPC.defense = 80;
            NPC.lifeMax = 72000;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.scale = 1.5f;
            NPC.Dnpc().MaxPenetrationProtection = 1F;
            NPC.Dnpc().Properties.Fire = true;
            NPC.Dnpc().Properties.Stone = true;
            NPC.alpha = 255;
            NPC.Dnpc().Times[4] = 60;
            NPC.Dnpc().Properties.BossLife = 1.275F;
        }
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
            NPC.defense = NPC.defDefense;
            NPC HostNPC = Main.npc[(int)NPC.ai[1]];
            NPC.Dnpc().Stage = HostNPC.Dnpc().Stage;
            if (NPC.Dnpc().Stage == 4)
            {
                NPC.Dnpc().Properties.Stone = false;
                NPC.defense = 0;
                NPC.HitSound = null;
                NPC.DeathSound = null;
                if (!NPC.Dnpc().Bool[4] && Main.npc[(int)NPC.ai[3]].ai[0] >= 100)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        int GoreType = Mod.Find<ModGore>("狱火蛇7").Type;
                        int GoreType2 = Mod.Find<ModGore>("狱火蛇8").Type;

                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(4, 0).RotatedBy(NPC.rotation), GoreType, NPC.scale);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-4, 0).RotatedBy(NPC.rotation), GoreType2, NPC.scale);
                    }
                    NPC.Dnpc().Bool[4] = true;
                }
            }
            else
            {
                if ((float)Main.npc[(int)NPC.ai[3]].life / NPC.lifeMax <= 0.5F && Main.npc[(int)NPC.ai[3]].ai[0] > 400)
                {
                    NPC.defense = 200;
                }
            }
            Lighting.AddLight(NPC.Center, new Vector3(253, 62, 3) * 0.003F);
            //Lighting.AddLight(NPC.Center, new Vector3(0, 120, 255) * 0.003F);

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
                if (HostNPC.type == ModContent.NPCType<狱火蛇头>())
                {
                    Distance = (vector.Length() - (66 * NPC.scale)) / vector.Length();
                }
                NPC.velocity = Vector2.Zero;
                NPC.position = NPC.position + vector * Distance;
            }
            if (!HostNPC.active || (HostNPC.type != ModContent.NPCType<狱火蛇身>() && HostNPC.type != ModContent.NPCType<狱火蛇身2>()))
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            return;
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
            }
            return true;
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            NPC HostNPC = Main.npc[(int)NPC.ai[3]];
            Texture2D textureGlow = texture2_Glow.Value;
            if (NPC.Dnpc().Stage == 4)
            {
                texture = texture2.Value;

                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(100, 100, 100, 255)), NPC.rotation+ MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
                spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(254, 62, 3, 0) * 0.25f), NPC.rotation+ MathHelper.PiOver2, textureGlow.Size() / 2, NPC.scale / 4, 0, 0f);
            }

            if (NPC.Dnpc().Stage != 4 || HostNPC.ai[0] < 100)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, 0, 0f);

                if ((float)HostNPC.life / NPC.lifeMax <= 0.5F && HostNPC.ai[0] < 400)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, 0, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, 0, 0f);
                }
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.Dnpc().Stage != 4)
                spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(255, 155, 155, 0)), NPC.rotation+ MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
            //spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(0, 76, 255, 0)), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale / 4, 0, 0f);
        }
    }
}