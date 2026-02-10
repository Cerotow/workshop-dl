using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Summon;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class 恶魔眼 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Demon Eye");
           //DisplayName.AddTranslation(7, "恶魔眼");
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.DemonEyes[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            new NPCID.Sets.NPCBestiaryDrawModifiers();
        }
        public static Asset<Texture2D> 命根子;
        public override void Load()
        {
            命根子 = ModContent.Request<Texture2D>(Texture + "命根子");
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPC.aiStyle = 2;
            AIType = 2;
            AnimationType = 2;
            NPC.lifeMax = 80;
            NPC.damage = 14;
            NPC.defense = 8;
            NPC.knockBackResist = 0.8f;
            NPC.width = 38;
            NPC.height = 38;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.Meat = true;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
            });
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)NPC.Dnpc().Times[0]];
            if (npc.active)
            {
                if((npc.Center-NPC.Center).Length()>100)
                {
                    NPC.velocity += (npc.Center - NPC.Center).PerfectNormalize()/2;
                }
            }
            for (int a = 0; a < 200; a++)
            {
                if (Main.npc[a].type == ModContent.NPCType<恶魔眼>() && Main.npc[a].active&& Main.npc[a].whoAmI!=NPC.whoAmI && Main.npc[a].Dnpc().Times[0] == NPC.Dnpc().Times[0])
                {
                    if(NPC.getRect().Intersects(Main.npc[a].getRect()))
                    {
                        NPC.velocity += (Main.npc[a].Center - NPC.Center).PerfectNormalize() * -2;
                    }
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            return 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC npc = Main.npc[(int)NPC.Dnpc().Times[0]];
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects spriteEffects = 0;
            if(NPC.spriteDirection == 1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            if (npc.active&&false)
            {
                npc.position -= new Vector2(-2 * NPC.direction, 2);
                Vector2 vector = NPC.Size / 2;
                for (int W = 0; W < (npc.Center - NPC.Center).Length() / 命根子.Height(); W++)
                {
                    if (W < (npc.Center - NPC.Center).Length() / 命根子.Height() - 1)
                    {
                        Main.spriteBatch.Draw(命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(命根子.Width()), 命根子.Height())), Color.White, (npc.Center - NPC.Center).ToRotation()-MathHelper.PiOver2, 命根子.Size() / 2, 1, spriteEffects, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(命根子.Width() ), (int)(命根子.Height() - (命根子.Height() - (npc.Center - NPC.Center).Length() % 命根子.Height())))), Color.White, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, 命根子.Size() / 2, 1, spriteEffects, 0f);
                    }
                }
                npc.position += new Vector2(-2 * NPC.direction, 2);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle?(NPC.frame), drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 2) / 2, 1, spriteEffects, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
    }
}