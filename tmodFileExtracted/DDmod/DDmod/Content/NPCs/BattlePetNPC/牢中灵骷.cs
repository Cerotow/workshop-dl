using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Summon;
using DDmod.NoContent.Config;
using System.Linq;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.Utilities;

namespace DDmod.Content.NPCs.BattlePetNPC
{
    public class 牢中灵骷 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Demon Eye");
           //DisplayName.AddTranslation(7, "恶魔眼");
            Main.npcFrameCount[NPC.type] = 1;

            NPCID.Sets.DemonEyes[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            new NPCID.Sets.NPCBestiaryDrawModifiers();
        }
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 205;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.knockBackResist = 0.8f;
            NPC.width = 42;
            NPC.height = 44;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.Dnpc().Battlepet = true;
            NPC.Dnpc().BattlepetType = Players.BattlePets.牢中灵骷;
            NPC.Dnpc().Neutrality = true;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.牢中灵骷")),
            });
        }
        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                NPC.ai[0] = Main.rand.NextFloat(MathHelper.TwoPi) + 1;
                NPC.rotation = NPC.ai[0] - 1;
            }
            NPC.rotation += NPC.velocity.X * 0.05F;
            NPC.velocity.X *= 0.96F;
            return;
            if (Main.netMode != NetmodeID.Server)
            {
                int GoreType = Main.rand.Next([11, 12, 13]);
                for (int a = 0; a < 8; a++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), Main.rand.NextFloat(NPC.height)), Vector2.Zero, GoreType, NPC.scale);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo)|| !ModContent.GetInstance<DDConfigServer>().Battlepet)
            {
                return 0;
            }
            return SpawnCondition.DungeonNormal.Chance * 0.01f;
        }
        public override void FindFrame(int frameHeight)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
    }
}