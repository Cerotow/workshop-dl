using DDmod.Content.Dusts;
using DDmod.Content;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;

namespace DDmod.Content.NPCs.EliteMonster.四柱护卫
{
    [AutoloadBossHead]
    public class 星尘护卫尾 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
            }
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.netAlways = true;
            NPC.width = 66;
            NPC.height = 66;
            NPC.aiStyle = -1;
            NPC.damage = 80;
            NPC.defense = 32;
            NPC.lifeMax = 2500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.dontCountMe = true;
            NPC.netAlways = true;
            NPC.scale = 1.15f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.Dnpc().PenetrationProtection = 0.2F;
            NPC.Dnpc().MaxPenetrationProtection = 0.3F;
            NPC.Dnpc().BossPhysique = true;
            NPC.alpha = 0;
            NPC.Dnpc().Properties.BossLife = 1.325F;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return null;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
            parameters.MinimumItemDropsCount = 8;
            parameters.MaximumItemDropsCount = 12;
            parameters.ChanceNumerator = 1;
            parameters.ChanceDenominator = 1;
            parameters.MinimumStackPerChunkBase = 2;
            parameters.MaximumStackPerChunkBase = 4;
            parameters.BonusMinDropsPerChunkPerPlayer = 1;
            parameters.BonusMaxDropsPerChunkPerPlayer = 2;
            DropOneByOne.Parameters parameters2 = parameters;
            DropOneByOne.Parameters parameters3 = parameters2;
            DropOneByOne.Parameters parameters4 = parameters3;
            parameters3.BonusMinDropsPerChunkPerPlayer = 2;
            parameters3.BonusMaxDropsPerChunkPerPlayer = 3;
            parameters4.BonusMinDropsPerChunkPerPlayer = 3;
            parameters4.BonusMaxDropsPerChunkPerPlayer = 4;
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(3459, parameters2), new DropOneByOne(3459, parameters3), new DropOneByOne(3459, parameters4), true));
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘遗物>(), 1, 1, 1, true);
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘护卫纪念章>(), 10, 10, 10, true);
            npcLoot.CompleteModeLoot(ModContent.ItemType<星尘护卫圣物>(), 0, 0, 1, true);

        }
        public override bool SpecialOnKill()
        {
            bool flag = true;
            for (int i = 0; i < 200; i++)
            {
                if (i != NPC.whoAmI && Main.npc[i].active && (Main.npc[i].type == ModContent.NPCType<星尘护卫头>() || Main.npc[i].type == ModContent.NPCType<星尘护卫身>() || Main.npc[i].type == ModContent.NPCType<星尘护卫尾>()))
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                NPC.boss = true;
                NPC.NPCLoot();
            }
            else
            {
                NPC.NPCLoot();
            }
            return true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
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
        private float AngleDifference(float a, float b)
        {
            float diff = (a - b + (float)Math.PI) % (float)(2 * Math.PI) - (float)Math.PI;
            return diff < -(float)Math.PI ? diff + (float)(2 * Math.PI) : diff;
        }
        bool Bool2;
        public override void AI()
        {
            Main.player[Main.myPlayer].stardustMonolithShader = true;
            if (NPC.ai[1] == 0)
            {
                NPC.Kill(false);
                return;
            }
            NPC HostNPC = Main.npc[(int)NPC.ai[1] - 1];
            NPC MasterNPC = Main.npc[NPC.Dnpc().Master];
            NPC.Dnpc().vector[0] = HostNPC.Center;

            //保持距离
            if (NPC.ai[1] < (double)Main.npc.Length && HostNPC.active)
            {
                float ro = AngleDifference(NPC.rotation, HostNPC.rotation);
                NPC.position -= (HostNPC.rotation).ToRotationVector2() * Math.Abs(ro) * 10;

                Vector2 vector = HostNPC.Center - NPC.Center;
                NPC.rotation = (float)Math.Atan2(vector.Y, vector.X);

                float Distance = (vector.Length() - (42 * NPC.scale)) / vector.Length();
                if (HostNPC.type == ModContent.NPCType<星尘护卫头>())
                {
                    Distance = (vector.Length() - (54 * NPC.scale)) / vector.Length();
                }
                NPC.velocity = Vector2.Zero;
                NPC.position = NPC.position + vector * Distance;
            }
            if (!HostNPC.active || (HostNPC.type != ModContent.NPCType<星尘护卫身>() && HostNPC.type != ModContent.NPCType<星尘护卫头>()))
            {
                NPC.Kill(true);
            }

            NPC.Dnpc().Stage = MasterNPC.Dnpc().Stage;
            if (NPC.Dnpc().Stage == 1)
            {
                for (int A = 0; A < 5; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 0.5f + Main.rand.NextFloat(1, 2);
                    Main.dust[D].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 1;
                }
            }

            if (NPC.Dnpc().Stage > 1)
            {

            }
                return;
        }
        float B = 60;
        public override void DrawEffects(ref Color drawColor)
        {
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), hit.HitDirection, -1f, 0, new Color(40, 185, 255, 0), 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != 1)
                {
                    for (int I = 0; I < Main.rand.Next(2); I++)
                    {
                        int A = DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center, 406, 0);
                        Main.npc[A].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 5);
                    }
                }
                for (int a = 0; a < 100; a++)
                {
                    int B = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), 1.5f);
                    Main.dust[B].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 5);
                }
                for (int A = 6; A <= 7; A++)
                {
                    int GoreType = Mod.Find<ModGore>("星尘护卫" + A).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(hit.HitDirection, -1), GoreType, NPC.scale);
                }
            }
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale, 0, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(Color.White), NPC.rotation + MathHelper.PiOver2, Glow.Size() / 2, NPC.scale, 0, 0f);
        }
    }
}