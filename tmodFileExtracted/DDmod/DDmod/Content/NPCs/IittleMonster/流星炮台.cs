using DDmod.Content.Biome;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Hostile;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Tiles.农场;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class 流星炮台 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 120;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.knockBackResist = 0f;
            NPC.width = 26;
            NPC.height = 48;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.chaseable = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<流星炮台旗帜>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != 2)
                {
                    int GoreType = Mod.Find<ModGore>("流星炮台1").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, 2), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("流星炮台2").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, -2).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), GoreType, NPC.scale);
                }
                for (int A = 0; A < 15; A++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 10);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                }
            }
        }
        public override bool PreAI()
        {
            NPC.damage = 0;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 vector = player.Center - (NPC.Center - new Vector2(0, 16));
            if (NPC.ai[0]++ > 180)
            {
                NPC.ai[0] = 0;
                NPC.ai[1] = 10;
                if (Main.netMode != 1)
                {
                    int A = NewProjectile(NPC.GetSource_FromAI(), NPC.Center-new Vector2(0,16), vector.PerfectNormalize() * 4, ModContent.ProjectileType<H绿激光>(), 20, 0);
                }
            }
            if(NPC.ai[1]>0)
            {
                NPC.ai[1]--;
            }
                return base.PreAI();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.流星炮台")),

            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override void OnHitByProjectile(Projectile projectile, HitInfo hit, int damageDone)
        {
        }
        public override void OnHitByItem(Player player, Item item, HitInfo hit, int damageDone)
        {
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
            if (NPC.ai[1]>0)
            {
                NPC.frame.Y = frameHeight;
            }

        }
        float R;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            Rectangle frame;
            frame = NPC.frame;
            spriteBatch.Draw(texture, NPC.position+new Vector2(NPC.width/2,NPC.height) - screenPos, frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(frame.Width/2, frame.Height-2), NPC.scale, sprite, 0);
            if(NPC.ai[1]>0)
            {
                texture = DDTextures.Starlight3.Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(0, 16), null, new Color(100, 255, 100,0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(0.5f,3)*0.75f, sprite, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(0, 16), null,new Color(100,255,100,0), 0, texture.Size()/2, NPC.scale * new Vector2(0.5f, 1) * 0.75f, sprite, 0);
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
    }
}