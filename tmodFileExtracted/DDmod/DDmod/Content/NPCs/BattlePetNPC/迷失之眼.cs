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
    public class 迷失之眼 : ModNPC
    {
        public override void SetStaticDefaults()
        {
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
            Main.npcFrameCount[NPC.type] = 4;
            NPC.aiStyle = 2;
            AIType = 2;
            NPC.lifeMax = 80;
            NPC.damage = 14;
            NPC.defense = 8;
            NPC.knockBackResist = 0.8f;
            NPC.width = 38;
            NPC.height = 38;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Battlepet = true;
            NPC.Dnpc().BattlepetType = Players.BattlePets.迷失之眼;
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.迷失之眼")),
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
            return SpawnCondition.OverworldNight.Chance * 0.01f;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -1;
            NPC.rotation = NPC.velocity.ToRotation();
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.rotation += MathHelper.Pi;
            }
            NPC.frameCounter++;
            if(NPC.frameCounter%6==0)
            {
                NPC.frame.Y += frameHeight;
            }
            if(NPC.frame.Y>= frameHeight*4)
            {
                NPC.frame.Y = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects spriteEffects = 0;
            if(NPC.spriteDirection == 1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, 1, spriteEffects, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
    }
}