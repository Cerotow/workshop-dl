using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;

namespace DDmod.Content.NPCs.Boss.鬼牙
{
    [AutoloadBossHead]
    public class 鬼牙身 : ModNPC
    {
        public static Asset<Texture2D> Body;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Glow2;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Body = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙身");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙身_Glow");
                Glow2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/鬼牙/鬼牙身_Glow2");
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
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.damage = 45;
            NPC.defense = 16;
            NPC.lifeMax = 90000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.dontCountMe = true;
            NPC.netAlways = true;
            NPC.scale = 1.3f;
            NPC.Dnpc().Deathrattle = true;
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().Properties.Meat = true;
            NPC.alpha = 255;
            NPC.Dnpc().Properties.BossLife = 1.3F;
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }

        public override void DrawEffects(ref Color drawColor)
        {
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
            //AI[1]知道自己跟随哪个npc
            //AI[2]知道自己在第几节
            //AI[3]知道知道谁跟着自己
            NPC HostNPC = Main.npc[(int)NPC.ai[1]];
            NPC ServantNPC = Main.npc[(int)NPC.ai[3]];
            NPC MasterNPC = Main.npc[NPC.Dnpc().Master];
            Player player = Main.player[MasterNPC.target];

            //保持距离
            if (NPC.ai[1] < (double)Main.npc.Length && HostNPC.active)
            {
                Vector2 vector = HostNPC.Center - NPC.Center;
                NPC.rotation = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;

                float Distance = (vector.Length() - (42 * NPC.scale)) / vector.Length();
                if (HostNPC.type == ModContent.NPCType<鬼牙头>())
                {
                    Distance = (vector.Length() - (42 * NPC.scale)) / vector.Length();
                }
                NPC.velocity = Vector2.Zero;
                NPC.position = NPC.position + vector * Distance;
            }
            if (!HostNPC.active || (HostNPC.type != ModContent.NPCType<鬼牙头>() && HostNPC.type != NPC.type))
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if(MasterNPC.localAI[0] < 0)
            {
                NPC.alpha = MasterNPC.alpha;
                NPC.dontTakeDamage = true;
                return;
            }
            NPC.dontTakeDamage = false;
            if (NPC.alpha >= 20)
            {
                NPC.dontTakeDamage = true;
            }
            NPC.Dnpc().Stage = MasterNPC.Dnpc().Stage;
            if(NPC.Dnpc().Stage==1)
            {
                if (MasterNPC.ai[1] > 600&& MasterNPC.ai[1]%10==0)
                {
                    for (int r = 0; r < 1; r++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(4) - new Vector2(52, 20).RotatedBy(NPC.rotation), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(220, 0, 25, 0), 0.7F);
                        Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(1), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        Main.dust[A].customData = 0.2F;
                        int B = NewDust(NPC.Center - new Vector2(4) - new Vector2(-52, 20).RotatedBy(NPC.rotation), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(220, 0, 25, 0), 0.7F);
                        Main.dust[B].velocity = new Vector2(Main.rand.NextFloat(1), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        Main.dust[B].customData = 0.2F;
                    }
                }
            }
            if(NPC.Dnpc().Stage==2)
            {
                NPC.alpha = MasterNPC.alpha;
            }
            if (NPC.Dnpc().Stage == 3)
            {
                NPC.Dnpc().Times[0] = MasterNPC.Dnpc().Times[0];
                NPC.Dnpc().Times[1] = MasterNPC.Dnpc().Times[1];
                if(NPC.Dnpc().Times[1]<4)
                {
                    NPC.alpha = (int)(NPC.Center - player.Center).Length() / 2 - 100;
                }
                else
                {
                    if (NPC.Dnpc().Times[0] < -60)
                    {
                        NPC.alpha -= 5;
                    }
                    else
                    {
                        NPC.alpha += 15;
                    }
                }
            }
            if (NPC.localAI[2] == 0)
            {
                NPC.damage = 0;
            }
            else
            {
                NPC.damage = NPC.defDamage;
            }
            if (NPC.localAI[2] == 0&& (HostNPC.alpha<=200|| NPC.localAI[2]!=0))
            {
                if (NPC.alpha != 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = NewDust(NPC.Center, 1, 1, 5, 0f, 0f, 100, default, 1.5f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].noLight = true;
                        Main.dust[num].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2F, 12F);
                    }
                }
                if ((NPC.Dnpc().vector[0] - NPC.Center).Length() > NPC.height*3)
                {
                    NPC.alpha -= 25;
                }
                if (NPC.alpha <= 0)
                {
                    NPC.localAI[2] = 1;
                    NPC.alpha = 0;
                }
            }
            if (!ServantNPC.active || (ServantNPC.type != ModContent.NPCType<鬼牙尾>() && ServantNPC.type != ModContent.NPCType<鬼牙身>()))
            {
                float i = NPC.ai[2];
                int Master = NPC.Dnpc().Master;
                int Alpha = NPC.alpha;
                NPC.Transform(ModContent.NPCType<鬼牙尾>());
                NPC.ai[1] = HostNPC.whoAmI;
                NPC.realLife = Master;
                NPC.Dnpc().Master = Master;
                NPC.ai[2] = i;
                NPC.alpha = Alpha;
                NPC.netUpdate = true;
            }
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            if (NPC.alpha > 255)
            {
                NPC.alpha = 255;
            }
            return;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int a = 0; a < 100; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 5, 0f, 0f, 0, default, 2.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(3, 8), Main.rand.NextFloat(3, 8)), (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                    }
                    int GoreType = Mod.Find<ModGore>("鬼牙2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, GoreType, NPC.scale);
                }
            }
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Body.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(Body.Width() / 2, Body.Height() / 2), NPC.scale, 0, 0f);

            if (Main.npc[NPC.Dnpc().Master].ai[1] > 600)
            {
                spriteBatch.Draw(Body.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), new Color(220, 0, 25, 0), NPC.rotation, new Vector2(Body.Width() / 2, Body.Height() / 2), NPC.scale, 0, 0f);

            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), NPC.scale, 0, 0f);
        }
    }
}
