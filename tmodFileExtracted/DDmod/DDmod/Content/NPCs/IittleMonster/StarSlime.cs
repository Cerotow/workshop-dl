using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Summon;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class StarSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Star Slime");
           //DisplayName.AddTranslation(7, "星之史莱姆");
            Main.npcFrameCount[NPC.type] = 2;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
            AIType = 1;
            AnimationType = 1;
            NPC.lifeMax = 80;
            NPC.damage = 18;
            NPC.defense = 4;
            NPC.knockBackResist = 0.4f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.Dnpc().Properties.Gel = true; ;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<星之史莱姆旗>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 20; a++)
                {
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, Main.rand.NextFloat(1,3)).RotatedBy(Main.rand.NextFloat(-1.7F, 1.7F)), Main.rand.Next(16, 18), Main.rand.NextFloat(0.7F, 1.1F));
                }
            }
            else
            {
                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedBy(Main.rand.NextFloat(-1.7F, 1.7F)), Main.rand.Next(16, 18), Main.rand.NextFloat(0.7F, 1.1F));
            }
        }
        public override bool PreAI()
        {
            if(Main.dayTime)
            {
                NPC.StrikeInstantKill();
            }
            R++;
            Lighting.AddLight(NPC.Center, new Color(155, 155, 155).ToVector3());
            if (R % 30 == 0)
            {
                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(-0.7F, 0.7F)),Main.rand.Next(16,18), Main.rand.NextFloat(0.7F, 1.1F));
            }
            if (R % 12 == 0)
            {
                Vector2 center = NPC.Center + new Vector2(0f, NPC.height * -0.1f);

                Vector2 direction = Main.rand.NextVector2CircularEdge(NPC.width * 0.6f, NPC.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = NewDustPerfect(center + direction * distance, 170, velocity,0);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
            return base.PreAI();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.StarSlime"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(75, 1, 1, 1));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            if (spawnInfo.Player.ZoneForest && !Main.dayTime)
            {
                return 0.01f;
            }
            return 0;
        }
        float R;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            Rectangle frame;
            frame = NPC.frame;
            for (int a = 0; a < 3 + (R/4 % 6); a++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2(NPC.width/2, NPC.height+4), frame, new Color(240, 213, 255, 0), NPC.rotation, new Vector2(frame.Width/2, frame.Height), NPC.scale * new Vector2(1 - NPC.Dnpc().Times[4], 1 + NPC.Dnpc().Times[4]), SpriteEffects.None, 0);
            }
        }
    }
}