
using DDmod.Content.Dusts;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Tiles.Trophy;
using DDmod.Helper;
using DDmod.Worlds;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.Utilities;

namespace DDmod.Content.NPCs.EliteMonster.四柱护卫
{
    [AutoloadBossHead]
    public class 星旋护卫分身 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/四柱护卫/星旋护卫_Glow");

        }


        public override void SetDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPC.damage = 80;
            NPC.width = 98;
			NPC.height = 98;
			NPC.aiStyle = -1;
			NPC.defense = 5;
			NPC.lifeMax = 2500;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 0, 0);
			NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true;
			NPC.scale = 1F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Light = true;
            NPC.localAI[2] = 2;
            if (!Main.dedServ) Music = 34;
            NPC.Dnpc().Properties.BossLife = 1.325F;
        }
        public override void ModifyTypeName(ref string typeName)
        {
        }
        public override void BossHeadRotation(ref float rotation)
        {
            //rotation = NPC.rotation;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        int Landing = 0;
        public override void AI()
        {
            SoundStyle sound = SoundID.Item173;
            sound.Pitch = 0;
            sound.MaxInstances = 20;
            NPC.TargetClosest();
            Main.player[Main.myPlayer].vortexMonolithShader = true;
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            if (NPC.localAI[2] < 2)
            {
                NPC.localAI[2] += 0.06F;
            }
            NPC.localAI[1] -= 0.02F;
            NPC.spriteDirection = 0;
            if (vector.X < 0)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -0.4F;

                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 2;
                    for (int A = 0; A < 5; A++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 229, 0f, 0f, 100, new Color(34, 221, 151, 55), 1f);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 0.5f + Main.rand.NextFloat(1, 2);
                        vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                        Main.dust[D].velocity = vector / 2;
                    }
                }
                else
                {
                    NPC.localAI[2] = 1;
                    Main.LocalPlayer.Dplayer().PlayerShake(30, 12);

                    sound.Pitch = 0.9F;
                    PlaySound(sound);
                    PlaySound(sound);
                    PlaySound(sound);
                    for (int t = 0; t < 4; t++)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(34, 221, 151, 55))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                        }
                    }
                    NPC.Dnpc().Stage = 1;
                    NPC.netUpdate = true;
                }
                return;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[0]++;
                if (NPC.localAI[2] >= 2 && NPC.ai[0] > 30)
                {
                    NPC.ai[0] = 0;
                    NPC.Dnpc().Stage = 2;
                    NPC.netUpdate = true;
                }
                return;

            }
            NPC.alpha = 0;
            if (!NPC.Dnpc().Bool[2])
                NPC.rotation = NPC.velocity.X * 0.03F;

            if (NPC.ai[3] == 0)
            {
                if (!NPC.Dnpc().Bool[2])
                {
                    Vector2 P = player.Center - NPC.Center;
                    if (vector.X > 0)
                    {
                        P.X -= 400;
                    }
                    else
                    {
                        P.X += 400;
                    }
                    NPC.SmoothVelocity(P.PerfectNormalize() * ((P.Length() / 5) > 20 ? 20 : P.Length() / 5), 20);
                    if (Math.Abs(P.Y) < 20 || NPC.ai[1] > 0)
                    {
                        if (NPC.ai[1] == 10)
                        {
                            NPC.localAI[1] = 1;
                        }
                        if (NPC.ai[1] == 30)
                        {
                            NPC.localAI[2] = 1;
                        }
                        NPC.velocity *= 0.86f;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > 50)
                        {
                            NPC.velocity = new Vector2(vector.X > 0 ? 35 : -35, 0);
                            NPC.Dnpc().Bool[2] = true;
                            NPC.ai[0]++;
                        }
                    }
                }
                else
                {
                    for (int a = 0; a < 3; a++)
                    {
                        int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(34, 221, 151, 55), NPC.scale * Main.rand.NextFloat(1F, 1.6F));
                        Main.dust[A].velocity = -NPC.velocity.PerfectNormalize() * 2;
                        Main.dust[A].rotation = NPC.velocity.ToRotation();
                        Main.dust[A].customData = 1F;
                        Main.dust[A].noGravity = true;
                    }
                    NPC.spriteDirection = 0;
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver4;
                    if (NPC.velocity.X < 0)
                    {
                        NPC.spriteDirection = 1;
                        NPC.rotation = NPC.velocity.ToRotation() + MathHelper.Pi - MathHelper.PiOver4;
                        if (vector.X > 400)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.Dnpc().Bool[2] = false;
                        }
                    }
                    else
                    {
                        if (vector.X < -400)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.Dnpc().Bool[2] = false;
                        }
                    }
                }
            }
            else
            {

                Vector2 P = player.Center - NPC.Center;
                P.Y -= 220;
                if (vector.X > 0)
                {
                    P.X -= 400;
                }
                else
                {
                    P.X += 400;
                }
                NPC.SmoothVelocity(P.PerfectNormalize() * ((P.Length() / 5) > 20 ? 20 : P.Length() / 5), 20);
                if (Math.Abs(P.Y) < 20 || NPC.ai[1] > 0)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] % 55 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center + new Vector2((NPC.spriteDirection == 0) ? 16 : -16, 62).RotatedBy(NPC.rotation), ((NPC.spriteDirection == 0) ? (NPC.rotation + 0.4F) : (NPC.rotation - 0.4F + MathHelper.Pi)).ToRotationVector2() * Main.rand.NextFloat(8, 12), ModContent.ProjectileType<Boss异星毒刺>(), 30, 0, -1, 1);
                    }
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.Reverse();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)NPC.spriteDirection;
            float C = 1- NPC.alpha / 255F;
            Texture2D texture = Glow.Value;
            Rectangle rectangle = new Rectangle(0, NPC.frame.Y * 2, NPC.frame.Width * 2, NPC.frame.Height * 2);
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0)* C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
            int L = NPC.oldPos.Length;
            if (NPC.Dnpc().Stage > 1)
            {
                for (int i = 0; i < L; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] - screenPos + NPC.Size / 2;
                    Color color = new Color(34, 221, 151, 55) * ((L - i) / (float)L)*0.1F;
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, vector2, NPC.frame, color * C, NPC.oldRot[i], NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0f);
                    color = new Color(255, 255, 255, 100) * ((L - i) / (float)L) * 0.1F;
                    spriteBatch.Draw(texture, vector2, rectangle, color * C, NPC.oldRot[i], rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
                }
            }
            if (NPC.localAI[1] > 0)
            {
                Main.spriteBatch.Draw(DDTextures.Wire.Value,NPC.Center - screenPos, null, new Color(34, 221, 151, 55) * (NPC.localAI[1]*3), spriteEffects==0?0:MathHelper.Pi, new Vector2(0, 1), new Vector2(5, 10) * (1 - NPC.localAI[1]), 0, 0f);
            }
            texture = TextureAssets.Npc[NPC.type].Value;
            if (NPC.localAI[2] < 2)
            {
                for (int A = 0; A < 3; A++)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(34, 221, 151, 55) * (2 - NPC.localAI[2]) * 2 * C, NPC.rotation, NPC.frame.Size() / 2, NPC.scale * NPC.localAI[2], spriteEffects, 0f);
                    spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0) * C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
                }
            }
            for (int A = 0; A < 2; A++)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(34, 221, 151, 0) * C, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0) * C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
            }
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.oldRot[0] = NPC.rotation;

            for (int a = NPC.oldRot.Length - 1; a > 0; a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
            NPC.localAI[3]-=2;
            NPC.frameCounter++;
            if (NPC.frameCounter % 5 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = frameHeight;
            }
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.星旋护卫, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 12; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 229, 0f, 0f, 100, new Color(34, 221, 151, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
                PlaySound(NPC.DeathSound, NPC.Center);
				for (int A = 0; A < 220; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 229, 0f, 0f, 100, new Color(34, 221, 151, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.NextFloat(1,2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector * 2;
                }
            }
		}
	}
}
