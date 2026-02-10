using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Summon;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class 僵尸 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Zombies");
           //DisplayName.AddTranslation(7, "僵尸");

            NPCID.Sets.Zombies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            new NPCID.Sets.NPCBestiaryDrawModifiers();
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPC.aiStyle = 3;
            AIType = 3;
            AnimationType = 3;
            NPC.lifeMax = 110;
            NPC.damage = 14;
            NPC.defense = 8;
            NPC.knockBackResist = 0.8f;
            NPC.width = 38;
            NPC.height = 54;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.Meat = true; ;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return Main.player[NPC.target].position.Y>NPC.position.Y;
        }
        public override void AI()
        {
            if(!NPC.Dnpc().Bool[0])
            {
                for (int A = -1; A <= 1; A += 2)
                {
                    int N= NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<恶魔眼>(), 0, 0);
                    Main.npc[N].velocity = new Vector2(A*5, -5);
                    Main.npc[N].Dnpc().Times[0] = NPC.whoAmI;
                }
                NPC.Dnpc().Bool[0] = true;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
            });
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
            List<int> vs = new List<int>();
            for (int a = 0; a < 200; a++)
            {
                if (Main.npc[a].type == ModContent.NPCType<恶魔眼>() && Main.npc[a].active && Main.npc[a].Dnpc().Times[0] == NPC.whoAmI)
                {
                    vs.Add(a);
                }
            }
            for (int a = 0; a < vs.Count; a++)
            {
                if (a == 0)
                {
                    NPC npc = Main.npc[vs[a]];
                    SpriteEffects spriteEffects = 0;
                    if (npc.active)
                    {
                        NPC.position -= new Vector2(-2 * NPC.direction, 2);
                        Vector2 vector = NPC.Size / 2;
                        for (int W = 0; W < (npc.Center - NPC.Center).Length() / 恶魔眼.命根子.Height(); W++)
                        {
                            if (W < (npc.Center - NPC.Center).Length() / 恶魔眼.命根子.Height() - 1)
                            {
                                Main.spriteBatch.Draw(恶魔眼.命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 恶魔眼.命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(恶魔眼.命根子.Width()), 恶魔眼.命根子.Height())), Color.White, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(恶魔眼.命根子.Width() / 2, 8), 1, spriteEffects, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(恶魔眼.命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 恶魔眼.命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(恶魔眼.命根子.Width()), (int)(恶魔眼.命根子.Height() - (恶魔眼.命根子.Height() - (npc.Center - NPC.Center).Length() % 恶魔眼.命根子.Height())))), Color.White, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(恶魔眼.命根子.Width() / 2, 8), 1, spriteEffects, 0f);
                            }
                        }
                        NPC.position += new Vector2(-2 * NPC.direction, 2);
                    }
                }
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            List<int> vs = new List<int>();
            for (int a = 0; a < 200; a++)
            {
                if (Main.npc[a].type == ModContent.NPCType<恶魔眼>() && Main.npc[a].active && Main.npc[a].Dnpc().Times[0] == NPC.whoAmI)
                {
                    vs.Add(a);
                }
            }
            for (int a = 0; a < vs.Count; a++)
            {
                if (a == 1)
                {
                    NPC npc = Main.npc[vs[a]];
                    SpriteEffects spriteEffects = 0;
                    if (npc.active)
                    {
                        NPC.position -= new Vector2(-2 * NPC.direction, 2);
                        Vector2 vector = NPC.Size / 2;
                        for (int W = 0; W < (npc.Center - NPC.Center).Length() / 恶魔眼.命根子.Height(); W++)
                        {
                            if (W < (npc.Center - NPC.Center).Length() / 恶魔眼.命根子.Height() - 1)
                            {
                                Main.spriteBatch.Draw(恶魔眼.命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 恶魔眼.命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(恶魔眼.命根子.Width()), 恶魔眼.命根子.Height())), Color.White, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(恶魔眼.命根子.Width() / 2, 8), 1, spriteEffects, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(恶魔眼.命根子.Value, NPC.position + (npc.Center - NPC.Center).PerfectNormalize() * (8 + 恶魔眼.命根子.Height() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(恶魔眼.命根子.Width()), (int)(恶魔眼.命根子.Height() - (恶魔眼.命根子.Height() - (npc.Center - NPC.Center).Length() % 恶魔眼.命根子.Height())))), Color.White, (npc.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, new Vector2(恶魔眼.命根子.Width() / 2, 8), 1, spriteEffects, 0f);
                            }
                        }
                        NPC.position += new Vector2(-2 * NPC.direction, 2);
                    }
                }
            }
        }
    }
}