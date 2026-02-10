using DDmod.Content.Biome;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Tiles.农场;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class 自律工程模块 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 200;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.knockBackResist = 0.4f;
            NPC.width = 58;
            NPC.height = 56;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
            NPC.scale = 1f;
            NPC.chaseable = false;
            NPC.ai[2] = 255;
            NPC.alpha = 255;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<自律工程模块旗帜>();
            NPC.Dnpc().Properties.Level = 5;
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
                    int GoreType = Mod.Find<ModGore>("自律工程模块1").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, Main.rand.NextFloat(2, 6)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("自律工程模块2").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, Main.rand.NextFloat(2, 6)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("自律工程模块3").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, Main.rand.NextFloat(2, 6)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("自律工程模块4").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, Main.rand.NextFloat(2,6)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("自律工程模块5").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, 2), GoreType, NPC.scale);
                }
                for (int A = 0; A < 100; A++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height,6, 0f, 0f, 10,Scale: Main.rand.NextFloat(0.5F, 1.5F));
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                }
            }
        }
        public override bool PreAI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            NPC.ai[3]++;
            if (NPC.ai[3] % 180 == 0)
            {
                NPC.life += 5;
                if (NPC.life > NPC.lifeMax)
                {
                    NPC.life = NPC.lifeMax;
                }
            }
            // NPC和物块相撞
            bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 1)), NPC.width, 1);
            bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 2)), NPC.width, 1);
            bool TileCollision3= Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y)), NPC.width, NPC.height);
            int PO = (int)((player.position.Y + player.height) / 16 - (NPC.position.Y + NPC.height) / 16);
            PO *= 16;
            NPC.velocity.Y += NPC.gravity;
            if (NPC.velocity.Y > NPC.maxFallSpeed)
            {
                NPC.velocity.Y = NPC.maxFallSpeed;
            }
            if ((NPC.velocity.Y > 0 && PO <= 16))
                NPC.velocity.Y = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false).Y;

            if (TileCollision || TileCollision2)
            {
                if ((NPC.velocity.Y > 0 && PO <= 16))
                {
                    NPC.velocity.Y = 0;
                }
            }
            NPC.alpha = (int)NPC.ai[2];
            if (NPC.ai[2] > 0)
            {
                NPC.ai[2] -= 5;

                for (int a = 0; a < 5; a++)
                {
                    int A = NewDust(NPC.position, NPC.width, NPC.height, 6, Scale: 0.7F);
                    Main.dust[A].noGravity = false;
                }
                return false;
            }
            NPC.ai[2] = 0;
            if (TileCollision3&& PO < 16)
            {
                NPC.position.Y -= 2;
            }
            Vector2 vector = player.Center - NPC.Center;
            if (NPC.ai[1] == 0)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] >= 300&& NPC.CountNPCS(ModContent.NPCType<流星炮台>()) < 20)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 1;
                }
                if (NPC.velocity.Y == 0)
                {
                    if (vector.X < 0)
                    {
                        if (NPC.velocity.X > -3)
                        {
                            NPC.velocity.X -= 0.1F;
                        }
                        NPC.spriteDirection = 0;
                    }
                    else
                    {
                        if (NPC.velocity.X < 3)
                        {
                            NPC.velocity.X += 0.1F;
                        }
                        NPC.spriteDirection = 1;
                    }
                }
            }
            else
            {
                NPC.velocity.X *= 0.92F;
                if (Math.Abs(NPC.velocity.X)<0.5F)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] >= 30 * 4)
                    {
                        NPC.ai[1] = 0;
                        if (NPC.spriteDirection == 1)
                        {
                            NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(6, 14), ModContent.NPCType<流星炮台>(), NPC.whoAmI, 0);
                        }
                        else
                        {
                            NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-6, 14), ModContent.NPCType<流星炮台>(), NPC.whoAmI, 0);
                        }
                    }
                    NPC.velocity.X = 0;
                }
            }
            return base.PreAI();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.自律工程模块")),

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
            NPC.frame.Width = 62;
            NPC.frameCounter += Math.Abs(NPC.velocity.X);
            if (NPC.ai[1] == 0|| Math.Abs(NPC.velocity.X) >= 0.5F)
            {
                NPC.frame.X = 0;
                if (NPC.frameCounter >= 20)
                {
                    NPC.frameCounter -= 20;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                int F = (int)NPC.ai[1]/4+2;
                NPC.frame.X = NPC.frame.Width * (F /8);
                NPC.frame.Y = frameHeight * (F %8);
            }

        }
        float R;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.alpha = 0;
            }

                SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            Rectangle frame;
            frame = NPC.frame;
            spriteBatch.Draw(texture, NPC.position+new Vector2(NPC.width/2,NPC.height) - screenPos, frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(frame.Width/2, frame.Height-2), NPC.scale, sprite, 0);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
    }
}