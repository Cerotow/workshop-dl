using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.Boss.狱火蛇
{
    //[AutoloadBossHead]
    public class 狱火小蛇头 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "2");
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Small Hellfire Serpent");
            //DisplayName.AddTranslation(7, "狱火小蛇");

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                if (!BuffID.Sets.IsATagBuff[k])
                {
                    NPCID.Sets.SpecificDebuffImmunity[Type][k] = true;
                }
            }
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.defense = 0;
            NPC.damage = 40;
            NPC.lifeMax = 1500;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.dontCountMe = true;
            NPC.behindTiles = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.BossLife = 1.275F;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public bool title;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int a = 0; a < 60; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 0.5f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(3, 8), Main.rand.NextFloat(3, 8)), (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                    }
                }
            }
        }
        public override bool CheckDead()
        {
            return true;
        }

        public override void AI()
        {

            NPC.defense = NPC.defDefense;
            Lighting.AddLight(NPC.Center, new Vector3(253, 62, 3) * 0.008F);
            Player player = Main.player[NPC.target];

            NPC.rotation = NPC.velocity.ToRotation();
            if (NPC.target < 0 || NPC.target == 255 || player.dead)
            {
                NPC.TargetClosest(true);
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
            if (NPC.Dnpc().vector[0] == Vector2.Zero && NPC.velocity!=Vector2.Zero)
            {
                NPC.Dnpc().vector[0] = NPC.velocity;
            }
            DDHelper.BackAndForth(-1, 1, 0.04F, ref NPC.ai[1], ref NPC.Dnpc().Bool[0]);
            Vector2 vector = player.Center - NPC.Center;
            if (vector.Length() <= 400)
            {
                if (vector.Length() >= 100)
                {
                    NPC.Dnpc().vector[0] = NPC.velocity;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize().RotatedBy(NPC.ai[1]) * 8) / 21;
                }
                else
                {
                    NPC.velocity = (NPC.velocity * 20 + NPC.Dnpc().vector[0].PerfectNormalize().RotatedBy(NPC.ai[1]) * 8) / 21;
                }
            }
            else
            {
                NPC.velocity = NPC.Dnpc().vector[0].RotatedBy(NPC.ai[1]);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                //头
                if (NPC.ai[0] == 0f)
                {
                    int NPCWhoAmI = NPC.whoAmI;
                    int Length = 8;
                    for (int i = 0; i <= Length; i++)
                    {
                        int NPCWhoAmI2;
                        if (i < Length)
                        {
                            NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<狱火小蛇身>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        }
                        else
                        {
                            NPCWhoAmI2 = NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<狱火小蛇尾>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
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
                    NPC.ai[0] = 1;
                }
            }
        }
        float Speed;
        public override void DrawEffects(ref Color drawColor)
        {
        }

        public override bool CheckActive()
        {
            return true;
        }
        public override void BossLoot(ref int potionType)
        {
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(24, 240, true);
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
            Texture2D texture = TextureAssets.Npc[Type].Value;

            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(100, 100, 255, 255)), NPC.rotation + MathHelper.PiOver2, Glow.Size() / 2, NPC.scale, 0, 0f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0) * 0.5F), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);

            return false;
        }
    }
}
