
using DDmod.Content.Dusts;
using DDmod.Content.Items.Magic.Staff.NPCLoot;
using DDmod.Content.Items.农场.种子;
using System.Linq;

namespace DDmod.Content.NPCs.Boss.蘑菇王
{
	public class 蘑菇怪 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;

		}

		public override void SetDefaults()
		{
			NPCID.Sets.TrailCacheLength[Type] = 3;
			NPCID.Sets.TrailingMode[Type] = 0;
            NPC.damage = 12;
			NPC.width = 36;
			NPC.height = 26;
			NPC.aiStyle = 1;
			NPC.defense = 2;
			NPC.scale = 1f;
			NPC.lifeMax = 18;
            if (Main.expertMode)
            {
                NPC.scale = 1.1f;
            }
            if (Main.masterMode)
            {
                NPC.scale = 1.2f;
            }
			NPC.knockBackResist = 1f;
			NPC.alpha = 0;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = 1;
			NPC.HitSound = SoundID.Grass;
            NPC.Dnpc().Properties.Fungi= true;
            if(NPC.AnyNPCs(ModContent.NPCType<蘑菇王>()))
            {

                NPC.Dnpc().Properties.BossLife = 1.05F;
            }
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.蘑菇怪")),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            });
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(5, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<奇怪的孢子>(),3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            int[] TileArray = { 2 };

            return TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType)
               && !NPC.AnyNPCs(NPCID.LunarTowerVortex)
               && !NPC.AnyNPCs(NPCID.LunarTowerStardust)
               && !NPC.AnyNPCs(NPCID.LunarTowerNebula)
               && !NPC.AnyNPCs(NPCID.LunarTowerSolar) && Main.invasionType == 0
               ? 0.08f : 0f;
        }
        public override Color? GetAlpha(Color drawColor)
        {
			return null;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
		public override void PostAI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest();
            Vector2 vector = player.Center - NPC.Center;
            if (NPC.velocity.Y == 0)
            {
                NPC.spriteDirection = 0;
                if (vector.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            //spriteBatch.Draw(texture, NPC.Center + new Vector2(0, NPC.height / 2 + NPC.gfxOffY + 2) - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4], 1 + NPC.Dnpc().Times[4]), sprite, 0f);
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<蘑菇粒子>(), hit.HitDirection, -1f, 0, default(Color), 1f);
            }
            if (NPC.life <= 0)
            {
                for (int A = 0; A < 50; A++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<蘑菇粒子>(), 0f, 0f, 0, default(Color), 1f);
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
