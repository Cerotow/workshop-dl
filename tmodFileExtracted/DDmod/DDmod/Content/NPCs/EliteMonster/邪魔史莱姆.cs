
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Tiles.Trophy;
using DDmod.Sync;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;

namespace DDmod.Content.NPCs.EliteMonster
{
    //[AutoloadBossHead]
    public class 邪魔史莱姆 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;

		}

		public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 15;
            NPC.damage = 28;
			NPC.width = 200;
			NPC.height = 146;
			NPC.aiStyle = -1;
			NPC.defense = 4;
			NPC.scale = 1f;
			NPC.lifeMax = 1200;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 0, 50, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
			NPC.scale = 1.3F;
            NPC.noTileCollide = true;
			NPC.boss = true;
            NPC.Dnpc().Properties.Gel=true;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Neutrality = true;
        }
        public override void ModifyTypeName(ref string typeName)
        {
            if (NPC.Dnpc().Stage == 0)
			{
				typeName = "???";
			}
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        int Landing = 0;
        float Landings = 0;
        float speed = 0;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            int PO = (int)((player.position.Y + player.height) / 16 - (NPC.position.Y + NPC.height) / 16);
            PO *= 16;

            //血量越少就越小
            //NPC.scale = ((float)NPC.life / NPC.lifeMax) / 2 + 0.8F;

            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                NPC.oldPos[a].X += NPC.width / 2;
            }
            NPC.position.X += NPC.width/2;
            NPC.width = (int)(200 * (NPC.scale * (1 - NPC.Dnpc().Times[4])));
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                NPC.oldPos[a].X -= NPC.width / 2;
            }
            NPC.position.X -= NPC.width / 2;


            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                NPC.oldPos[a].Y += NPC.height;
            }
            NPC.position.Y += NPC.height;
            NPC.height = (int)(146 * (NPC.scale * (1 + NPC.Dnpc().Times[4])));
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                NPC.oldPos[a].Y -= NPC.height;
            }
            NPC.position.Y -= NPC.height;
            NPC.oldRot[0] = NPC.Dnpc().Times[4];
            for (int a = NPC.oldPos.Length - 1; a > 0; a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
            if (Landing  == 0)
            {
                Landing  = (int)(NPC.position.Y + NPC.height);
            }
            NPC.velocity.Y += 1F;
            if (NPC.velocity.Y > 64)
            {
                NPC.velocity.Y = 64;
            }
            if (NPC.position.Y + NPC.height+NPC.velocity.Y-1 >= Landing  && NPC.velocity.Y >= 0)
            {
                NPC.velocity.Y = Landing -(NPC.position.Y + NPC.height);
            }
            if (NPC.position.Y + NPC.height >= Landing && NPC.velocity.Y>=0)
            {
                NPC.velocity.Y = 0;
            }
            if (NPC.velocity.Y == 0f)
            {
                float TY = NPC.ai[0] % 300;
                if (TY > 240 && TY < 300 && NPC.ai[1] > 0)
                {
                    NPC.Dnpc().Times[4] -= 0.02F;
                    DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.5F, -0.5F);
                }
                else
                {
                    if (NPC.Dnpc().Times[3] > 0.1F)
                    {
                        NPC.Dnpc().Times[3] -= 0.008F;

                    }
                    else
                    {
                        NPC.Dnpc().Times[3] = 0.1f;
                    }
                    DDHelper.BackAndForth(-NPC.Dnpc().Times[3], NPC.Dnpc().Times[3], NPC.Dnpc().Times[3] / 6, ref NPC.Dnpc().Times[4], ref NPC.Dnpc().Bool[4]);
                }
                speed = 0;
            }
            else
            {
                if(speed==0)
                {
                    speed= Math.Abs(NPC.velocity.Y);
                }
                NPC.Dnpc().Times[3] = 0.7F;

                if (NPC.velocity.Y < 0)
                {
                    if (Math.Abs(NPC.velocity.Y) > speed/2)
                    {
                        NPC.Dnpc().Times[4] += 0.3F;
                        NPC.Dnpc().Bool[4] = false;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.8F, -0.8F);
                    }
                    else
                    {

                        NPC.Dnpc().Times[4] -= 0.05F;
                        NPC.Dnpc().Bool[4] = false;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.4F, -0.4F);
                    }
                }
                else
                {

                    if (Math.Abs(NPC.velocity.Y) > speed / 10)
                    {
                        NPC.Dnpc().Times[4] += 0.3F;
                        NPC.Dnpc().Bool[4] = false;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.8F, -0.8F);
                    }
                    else
                    {

                        NPC.Dnpc().Times[4] -= 0.05F;
                        NPC.Dnpc().Bool[4] = false;
                        DDHelper.MaxandMinF(ref NPC.Dnpc().Times[4], 0.4F, -0.4F);
                    }
                }
            }
            if (NPC.Dnpc().Stage==0)
            {
                if (NPC.life<NPC.lifeMax)
                {
                    NPC.Dnpc().Stage = 1;
                }
                NPC.scale = 0.1F;
                NPC.damage = 0;
                NPC.boss = false;
                return;
            }
            NPC.damage = NPC.defDamage;
            NPC.Dnpc().Neutrality = false;
            NPC.chaseable = true;
            NPC.boss = true;
            if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
            NPC.NPCHB().MiniBoss = true;

            if (NPC.Dnpc().Stage == 1)
			{
				NPC.dontTakeDamage = true;
				if (NPC.scale < 1.3f)
				{
					NPC.scale += 0.01F;
                    for (int num251 = 0; num251 < 10; num251++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, 4, NPC.velocity.X, NPC.velocity.Y, 150, new Color(170, 150, 255, 55), NPC.scale+0.2F)];
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                    }
                }
				else
				{
					NPC.Dnpc().Stage = 2;
					NPC.velocity.Y = -16;
                    NPC.scale = 1.3f;
                    for (int A = 0; A < 100; A++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                        Vector2 vector2 = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                        Main.dust[D].velocity = vector2 * 2;
                    }
                    NPC.netUpdate = true;
                }
				return;
			}
            NPC.dontTakeDamage = false;
            Vector2 vector = player.Center - NPC.Center;


            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X *= 0.92F;
                NPC.ai[0]++;
            }
            else
            {
                if (PO > 120 && NPC.ai[2] < 30)
                {
                    Landing = (int)(player.position.Y + player.height + NPC.height / 4);
                    NPC.velocity.Y *= 0.8f;
                    NPC.Dnpc().Times[3] = 0.6F;
                    float SP = Math.Abs(vector.X);
                    if (SP > 100)
                    {
                        SP = 100;
                    }
                    NPC.velocity.X = (NPC.velocity.X * 1 + vector.PerfectNormalize().X * SP) / 2;
                    NPC.ai[2]++;
                    NPC.position += player.Dplayer().PrePosition;
                }
                if (NPC.ai[2] == 30)
                {
                    NPC.velocity.Y = 40;
                    NPC.ai[2]++;
                }
                if (NPC.velocity.Y > 0&&Math.Abs(NPC.position.Y + NPC.height-Landing)>32)
                {
                    Landing = (int)(player.position.Y + player.height + NPC.height / 4);
                }
            }

            if (NPC.ai[0] % 300 == 0)
			{
				NPC.ai[0]++;
                NPC.ai[2] = 0;


                float SP = Math.Abs(vector.X);
                if (SP > 100)
                {
                    SP = 100;
                }
                if (NPC.ai[1] >0&& NPC.ai[1] < 4)
				{
					NPC.velocity.X = vector.PerfectNormalize().X * SP;
					NPC.velocity.Y = -10 * 8;
				}
				else if (NPC.ai[1] == 4)
                {
                    NPC.velocity.X = vector.PerfectNormalize().X * SP;
                    NPC.velocity.Y = -14 * 8;
                }
				else if (NPC.ai[1]>4 && NPC.ai[1] < 10)
                {
					NPC.ai[0] += 200;
                    NPC.velocity.X = vector.PerfectNormalize().X * SP;
                    NPC.velocity.Y = -8 * 8;
                }
                else if (NPC.ai[1] == 10)
                {
                    NPC.velocity.X = vector.PerfectNormalize().X * SP;
                    NPC.velocity.Y = -14 * 8;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
                NPC.ai[1]++;
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.超级蓝史莱姆"))
            ]);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegendaryGelItem>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<超级蓝史莱姆圣物>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<超级蓝史莱姆纪念章物品>()));
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            drawColor = Color.White;
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                spriteBatch.Draw(texture, NPC.oldPos[a] + new Vector2(NPC.width / 2, NPC.height + NPC.gfxOffY) - screenPos, NPC.frame, new Color(242, 60, 209, 50) * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, new Vector2(texture.Width / 2, texture.Height), NPC.scale * new Vector2(1 - NPC.oldRot[a] / 2, 1 + NPC.oldRot[a] / 2), 0, 0f); ;

            }

            drawColor *= 0.7F;
            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width / 2, NPC.height + NPC.gfxOffY) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height), NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4] / 2, 1 + NPC.Dnpc().Times[4] / 2), 0, 0f);
            texture = DDTextures.限制框.Value;
            float L = Landing - (NPC.position.Y + NPC.height);
            L /= 300;
            if (L > 1)
            {
                L = 1;
            }
            if (L < 0)
            {
                L = 0;
            }
            L = 1.05F - L;
            spriteBatch.Draw(texture, new Vector2(NPC.Center.X, Landing) - screenPos, null, new Color(242, 60, 209, 0) * L, NPC.rotation, new Vector2(texture.Width / 2, 0), new Vector2(NPC.scale / 2, 3 * NPC.scale), 0, 0f);
            spriteBatch.Draw(texture, new Vector2(NPC.Center.X, Landing) - screenPos, new Rectangle(0, 0, texture.Width, 8), new Color(242, 60, 209, 0) * L, NPC.rotation, new Vector2(texture.Width / 2, 0), new Vector2(NPC.scale / 2, 0.4F), 0, 0f);
            spriteBatch.Draw(texture, new Vector2(NPC.Center.X, Landing) - screenPos, new Rectangle(0, 0, texture.Width, 8), new Color(242, 60, 209, 0) * L, NPC.rotation, new Vector2(texture.Width / 2, 0), new Vector2(NPC.scale / 2, 0.4F), 0, 0f);

            return false;
        }
		public override void FindFrame(int frameHeight)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 16,
            };

            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
			NPC.frame.Y = 0;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.超级蓝史莱姆, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 10; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 300; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 4, 0f, 0f, 100, new Color(170, 150, 255, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.Next(2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                    Main.dust[D].velocity = vector * 2;
                }
			}
		}
	}
}
