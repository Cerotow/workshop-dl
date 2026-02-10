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
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Biome;
using DDmod.Worlds;

namespace DDmod.Content.NPCs.IittleMonster.绿岩
{
	public class 绿岩史莱姆2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
            DGlobalNPC.IgnoreTile[NPC.type] = true;
        }

		public override void SetDefaults()
		{
			NPC.damage = 20;
			NPC.width = 34;
			NPC.height = 46;
			NPC.aiStyle = -1;
			NPC.defense = 12;
			NPC.scale = 1f;
			NPC.lifeMax = 40;
			NPC.knockBackResist = 0.3f;
			NPC.value = 0;
			NPC.alpha = 200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
            NPC.Dnpc().Properties.Iron = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Biome.绿岩实验室>().Type };
            Banner = ModContent.NPCType<绿岩史莱姆>();
            BannerItem = ModContent.ItemType<绿岩史莱姆旗>();

            NPC.Dnpc().Properties.Level = 2;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPCdirection.Incident(spawnInfo))
			{
				return 0;
            }
            if (!spawnInfo.Player.ZoneForest  || !NPCDowned.绿岩刷怪)
            {
				return 0;
            }
            return SpawnCondition.OverworldDay.Chance * 0.2f;
        }
        public override void AI()
        {
			Player player = Main.player[NPC.target];
			NPC.TargetClosest();
			Vector2 vector = player.Center - NPC.Center;
			NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 4) / 21;
			NPC.velocity.Y += NPC.ai[0]/10;
			DDHelper.BackAndForth(-1, 1, 0.03F, ref NPC.ai[0], ref NPC.Dnpc().Bool[0]);
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
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩史莱姆2")),
            });
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
		}
        public override void FindFrame(int frameHeight)
        {
			NPC.rotation = NPC.velocity.X * 0.03F;
			NPC.frameCounter++;
			if(NPC.frameCounter>=2)
			{
				NPC.frame.Y += frameHeight;
				NPC.frameCounter = 0;
			}
			if(NPC.frame.Y >= frameHeight*4)
			{
				NPC.frame.Y = 0;
				NPC.frameCounter = 0;
			}
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			SpriteEffects sprite = 0;
			spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, 0, 0);
			return false;
		}
        public override void HitEffect(HitInfo hit)
        {
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0,Color.White, 1f);
			}
			for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255), 1f);
                Main.dust[dust].velocity = new Vector2(1,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.NextFloat(0,4);
            }

			if (NPC.life <= 0)
			{
				if(Main.netMode!=2)
                {
                    int GoreType = Mod.Find<ModGore>("直升机碎块").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, -2), GoreType, NPC.scale);
				}
				for (int A = 0; A < 30; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130,255), 1f);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                }
				DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.position+new Vector2(0,NPC.height/2), ModContent.NPCType<绿岩史莱姆>(),0);
			}
		}
	}
}
