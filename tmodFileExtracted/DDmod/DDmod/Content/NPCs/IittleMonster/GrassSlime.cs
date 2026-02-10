
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.IittleMonster.旗子;
using System.Linq;

namespace DDmod.Content.NPCs.IittleMonster
{
	public class GrassSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;

		}

		public override void SetDefaults()
		{
			NPC.damage = 8;
			NPC.width = 32;
			NPC.height = 26;
			NPC.aiStyle = 1;
			NPC.defense = 8;
			NPC.scale = 1f;
			NPC.lifeMax = 60;
			NPC.knockBackResist = 1f;
			NPC.value = Item.buyPrice(0, 0, 1, 0);
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = 1;
			NPC.HitSound = SoundID.Grass;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<草史莱姆旗>();

            NPC.Dnpc().Properties.Level = 0;
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPCdirection.Incident(spawnInfo))
			{
				return 0;
			}
			int[] TileArray = { 2 };

			return TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType)? 0.05f : 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			});
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(62, 1, 3, 6));
		}

		public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height,Main.rand.Next(2,4), hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 50; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, Main.rand.Next(2, 4), 0f, 0f, 10, default(Color), 1f);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dust].scale = 0.1f;
						Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
					}
				}
			}
		}
	}
}
