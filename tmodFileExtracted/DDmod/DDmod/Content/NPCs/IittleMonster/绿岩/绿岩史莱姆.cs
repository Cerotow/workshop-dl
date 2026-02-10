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
	public class 绿岩史莱姆 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
			DGlobalNPC.IgnoreTile[NPC.type] = true;
        }

		public override void SetDefaults()
		{
			NPC.damage = 20;
			NPC.width = 32;
			NPC.height = 26;
			NPC.aiStyle = 1;
			NPC.defense = 12;
			NPC.scale = 1f;
			NPC.lifeMax = 80;
			NPC.knockBackResist = 0.3f;
			NPC.value = Item.buyPrice(0, 0, 2, 0);
			NPC.alpha = 200;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Tink;
			AnimationType = 1;
			NPC.color = Color.White;
            NPC.Dnpc().Properties.Stone = true;
            NPC.Dnpc().Properties.Gel = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Biome.绿岩实验室>().Type };
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<绿岩史莱姆旗>();
            NPC.Dnpc().Properties.Level = 2;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo)|| !NPCDowned.绿岩刷怪)
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
        }
        public override bool? CanFallThroughPlatforms()
		{
			return false;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<Biome.绿岩实验室>().ModBiomeBestiaryInfoElement),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩史莱姆")),
            });
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<绿岩晶石>(), 1, 2, 6));
		}
		public override void HitEffect(HitInfo hit)
        {
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0, Color.White, 1f);
			}

			if (NPC.life <= 0)
			{
				for (int A = 0; A < 50; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), 0f, 0f, 10, Color.White, 1.2f);

                    Main.dust[dust].velocity *= 3;
                }
			}
		}
	}
}
