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
    public class 金毛幼鸟 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Demon Eye");
           //DisplayName.AddTranslation(7, "恶魔眼");
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.DemonEyes[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            new NPCID.Sets.NPCBestiaryDrawModifiers();
        }
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPC.aiStyle = 24;
            AIType = 74;
            NPC.lifeMax = 105;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0.8f;
            NPC.width = 15;
            NPC.height = 15;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Battlepet = true;
            NPC.Dnpc().BattlepetType = Players.BattlePets.金毛幼鸟;
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.金毛幼鸟")),
            });
        }
        public override void AI()
        {
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
            return SpawnCondition.OverworldDayBirdCritter.Chance * 1f;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0]==0)
            {
                NPC.frame.Y = frameHeight * 4;
                return;
            }
            NPC.spriteDirection = -1;
            NPC.rotation = NPC.velocity.X*0.03f;
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
            }
            NPC.frameCounter++;
            if (NPC.frameCounter % 3 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = 0;
            }
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