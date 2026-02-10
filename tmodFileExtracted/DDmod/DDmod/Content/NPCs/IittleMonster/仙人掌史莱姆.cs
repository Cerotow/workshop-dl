using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.NPCs.IittleMonster.旗子;

namespace DDmod.Content.NPCs.IittleMonster
{
	public class 仙人掌史莱姆 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
		}

		public override void SetDefaults()
		{
			NPC.damage = 12;
			NPC.width = 32;
			NPC.height = 26;
			NPC.aiStyle = 1;
			NPC.defense = 8;
			NPC.scale = 1.25f;
			NPC.lifeMax = 50;
			NPC.knockBackResist = 1f;
			NPC.value = Item.buyPrice(0, 0, 10, 0);
			NPC.alpha = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = 1;
			NPC.color = new Color(200, 200, 200, 160);
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<仙人掌史莱姆旗>();
            NPC.Dnpc().Properties.Level = 1;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPCdirection.Incident(spawnInfo))
			{
				return 0;
			}
			if (spawnInfo.Player.ZoneDesert)
			{
				return 0.05f;
			}
			return 0;
		}
        public override void AI()
        {
			NPC.ai[1] = ModContent.ItemType<仙人掌之魂>();
        }
        public override bool? CanFallThroughPlatforms()
		{
			return false;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(276, 1, 3, 6));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<仙人掌之魂>(), 1, 1, 2));
		}
		public override void HitEffect(HitInfo hit)
        {
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 40, hit.HitDirection, -1f, 0,new Color(200, 200, 200, 160), 1f);
			}

			if (NPC.life <= 0)
			{
				for (int A = 0; A < 50; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 40, 0f, 0f, 10, new Color(200, 200, 200, 160), 1f);
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
