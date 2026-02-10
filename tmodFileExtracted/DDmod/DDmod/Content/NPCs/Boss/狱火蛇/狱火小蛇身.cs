using DDmod.Content.Projectiles.Boss;

namespace DDmod.Content.NPCs.Boss.狱火蛇
{
    //[AutoloadBossHead]
    public class 狱火小蛇身 : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"2");
        }
        public override void SetStaticDefaults()
        {
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
            NPC.damage = 75;
            NPC.defense = 0;
            NPC.lifeMax = 1500;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.dontCountMe = true;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.BossLife = 1.275F;
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

            Lighting.AddLight(NPC.Center, new Vector3(253, 62, 3) * 0.008F);
            //保持距离
            if (NPC.ai[1] < (double)Main.npc.Length && HostNPC.active)
            {
                float ro = DDHelper.AngleDifference(NPC.rotation, HostNPC.rotation);
                NPC.position -= (HostNPC.rotation).ToRotationVector2() * Math.Abs(ro) * 10;
                Vector2 vector = HostNPC.Center - NPC.Center;
                NPC.rotation = (float)Math.Atan2(vector.Y, vector.X);

                float Distance = (vector.Length() - (18 * NPC.scale)) / vector.Length();
                if (HostNPC.type == ModContent.NPCType<狱火小蛇头>())
                {
                    Distance = (vector.Length() - (18 * NPC.scale)) / vector.Length();
                }
                NPC.velocity = Vector2.Zero;
                NPC.position = NPC.position + vector * Distance;
            }
            if (NPC.localAI[2] == 0)
            {
                if (!HostNPC.active || (HostNPC.type != ModContent.NPCType<狱火小蛇头>()&& HostNPC.type != NPC.type))
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
                return;
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
                for (int a = 0; a < 40; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 0, default, 0.5f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(10, 20), Main.rand.NextFloat(10, 20)), (Math.PI * 2 / a) + a, default);
                    dust.velocity *= vector;
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
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0)), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, null, NPC.GetAlpha(new Color(100, 100, 255,255)), NPC.rotation + MathHelper.PiOver2, Glow.Size() / 2, NPC.scale, 0, 0f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), NPC.GetAlpha(new Color(254, 62, 3, 0) * 0.5F), NPC.rotation + MathHelper.PiOver2, texture.Size() / 2, NPC.scale / 3, 0, 0f);
            return false;
        }
    }
}
