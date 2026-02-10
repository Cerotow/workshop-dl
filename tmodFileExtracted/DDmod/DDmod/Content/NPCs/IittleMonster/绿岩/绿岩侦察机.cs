using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Dusts;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Biome;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.IittleMonster.绿岩
{
	public class 绿岩侦察机 : ModNPC
	{
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 19;
            DGlobalNPC.IgnoreTile[NPC.type] = true;
        }

		public override void SetDefaults()
		{
			NPC.damage = 30;
			NPC.width = 40;
			NPC.height = 40;
			NPC.aiStyle = -1;
			NPC.defense = 12;
			NPC.scale = 1f;
			NPC.lifeMax = 120;
			NPC.knockBackResist = 0.3f;
			NPC.value = 0;
			NPC.alpha = 200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.Iron = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Biome.绿岩实验室>().Type };
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<绿岩无人机旗>();
            NPC.Dnpc().Properties.Level = 2;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo) || !NPCDowned.绿岩刷怪)
            {
				return 0;
            }
            int[] TileArray = { ModContent.TileType<绿岩砖Tile>(), ModContent.TileType<绿岩格网块Tile>() };
            if (TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType))
            {
                return 0.5f;

            }
            if (!spawnInfo.Player.ZoneForest)
            {
				return 0;
            }
            return SpawnCondition.OverworldDay.Chance * 0.2f;
        }
        public override void AI()
        {
            if (NPC.Dnpc().Bool[3] && !NPC.Dnpc().Bool[2])
            {
                NPC.TargetClosest();
                NPC.Dnpc().Bool[1] = false;

                if (NPC.velocity.X > 0)
                {
                    NPC.ai[2]=1;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.ai[2]=-1;
                }
                NPC.velocity.X = NPC.ai[2];
                NPC.velocity.Y = NPC.ai[0];
                NPC.spriteDirection = 0;
                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;

                }
                NPC.Dnpc().Neutrality = true;
                NPC.chaseable = false;
                NPC.velocity.Y += NPC.ai[0] / 10;
                NPC.noTileCollide = false;
            }
            else
            {
                Player player = Main.player[NPC.target];
                NPC.Dnpc().Neutrality = false;
                NPC.chaseable = true;
                NPC.Dnpc().Bool[1] = true;
                Vector2 vector = player.Center - NPC.Center;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 4) / 21;
                NPC.spriteDirection = 0;
                if (vector.X > 0)
                {
                    NPC.spriteDirection = 1;

                }
                NPC.Dnpc().Bool[3] = true;
                if (player.dead)
                {
                    NPC.Dnpc().Bool[2] = false;
                }

            }
            if (NPC.Dnpc().Times[0] > 0)
            {
                NPC.Dnpc().Times[0]--;
            }
            if (!NPC.Dnpc().Bool[1])
            {
                bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y)), NPC.width, NPC.height + 40);
                bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y)), NPC.width, NPC.height + 120);
                bool TileCollision3 = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y-10)), NPC.width, 10);
                bool TileCollision4 = Collision.SolidCollision(new Vector2(NPC.position.X+16* NPC.ai[2], (NPC.position.Y)), NPC.width, NPC.height);
                if(TileCollision3)
                {
                    NPC.ai[2] *= -1;
                }
                if (TileCollision4)
                {
                    DDHelper.BackAndForth(-8, 0, 0.12F, ref NPC.ai[0], ref NPC.Dnpc().Bool[0], false);
                }
                else if (TileCollision)
                {
                    DDHelper.BackAndForth(-4, 0, 0.06F, ref NPC.ai[0], ref NPC.Dnpc().Bool[0], false);
                }
                else if (TileCollision2)
                {
                    DDHelper.BackAndForth(-1, 1, 0.03F, ref NPC.ai[0], ref NPC.Dnpc().Bool[0], false);
                }
                else
                {
                    DDHelper.BackAndForth(0, 2, 0.06F, ref NPC.ai[0], ref NPC.Dnpc().Bool[0], false);
                }
            }
        }
        public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<Biome.绿岩实验室>().ModBiomeBestiaryInfoElement),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩侦察机")),
            });
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<绿岩晶石>(), 1, 4, 10));
        }
		//1-5生气
		//6-8转换微笑
		//9-14微笑
		//15-18转换生气
		//19挨打
        public override void FindFrame(int frameHeight)
        {
			NPC.rotation = NPC.velocity.X * 0.03F;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frame.Y += frameHeight;
                NPC.Dnpc().Times[1]++;
                NPC.Dnpc().Times[2]++;
                NPC.frameCounter = 0;
            }
            if (!NPC.Dnpc().Bool[1])
			{
                if (NPC.frame.Y == frameHeight * 14)
                {
                    NPC.frame.Y = frameHeight * 9;
                    NPC.frameCounter = 0;
                }
            }
			else
            {
                if (NPC.frame.Y == frameHeight * 5)
                {
                    NPC.frame.Y = frameHeight * 0;
                    NPC.frameCounter = 0;
                }
            }
            if (NPC.frame.Y >= frameHeight * 17)
            {
                NPC.frame.Y = frameHeight * 0;
                NPC.frameCounter = 0;
            }
            if (NPC.Dnpc().Times[1]>=19)
			{
				NPC.Dnpc().Times[1] = 0;
            }
			if(NPC.Dnpc().Times[2]>=5)
			{
				NPC.Dnpc().Times[2] = 0;
            }
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			SpriteEffects sprite = 0;
			Rectangle rectangle = NPC.frame;
            rectangle.Width = 52;

            if (NPC.spriteDirection == 1)
			{
				sprite = SpriteEffects.FlipHorizontally;

            }
            if (NPC.Dnpc().Times[0] > 0)
            {
				rectangle.Y = rectangle.Height * 18;
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos-new Vector2(0,0), rectangle, drawColor, NPC.rotation, rectangle.Size() / 2 - new Vector2(0, 10), NPC.scale, sprite, 0);
			rectangle.X += 52;
            spriteBatch.Draw(texture, NPC.Center - screenPos-new Vector2(0,0), rectangle, Color.White, NPC.rotation, rectangle.Size() / 2 - new Vector2(0, 10), NPC.scale, sprite, 0);
			rectangle.X += 52;
			rectangle.Y = (int)(rectangle.Height* NPC.Dnpc().Times[1]);
            spriteBatch.Draw(texture, NPC.Center - screenPos-new Vector2(0, 0), rectangle, Color.White, NPC.rotation, rectangle.Size() / 2 - new Vector2(0, 10), NPC.scale, sprite, 0);
            rectangle.X+= 52;
            rectangle.Y = (int)(rectangle.Height * NPC.Dnpc().Times[2]);
            spriteBatch.Draw(texture, NPC.Center - screenPos-new Vector2(0, 0), rectangle, Color.White*0.6F, NPC.rotation, rectangle.Size() / 2-new Vector2(0,10), NPC.scale, sprite, 0);

			
			return false;
		}
        public override void HitEffect(HitInfo hit)
        {
			NPC.Dnpc().Bool[2] = true;

            NPC.Dnpc().Times[0] = 10;
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0,Color.White, 1f);
			}
			for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<绿岩电光粒子>(), 0f, 0f, 10, Scale: Main.rand.NextFloat(0.75F, 1.25F));
                Main.dust[dust].velocity = new Vector2(1,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.NextFloat(0,4);
            }

			if (NPC.life <= 0)
			{
				if(Main.netMode!=2)
                {
                    int GoreType = Mod.Find<ModGore>("绿岩侦察机").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, -2), GoreType, NPC.scale);
				}
				for (int A = 0; A < 30; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<绿岩电光粒子>(), 0f, 0f, 10,Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                }
			}
		}
	}
}
