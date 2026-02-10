using DDmod.Content.Biome;
using DDmod.Content.Buffs.DeBuffs;
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
    public class 胡萝卜精 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Slime");
            //DisplayName.AddTranslation(7, "星之史莱姆");
            Main.npcFrameCount[NPC.type] = 9;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 80;
            NPC.damage = 0;
            NPC.defense = 4;
            NPC.knockBackResist = 0.4f;
            NPC.width = 12;
            NPC.height = 20;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1f;
            SpawnModBiomes = new int[] { ModContent.GetInstance<农场>().Type };
            NPC.chaseable = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<胡萝卜精旗>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void HitEffect(HitInfo hit)
        {
            return;
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 20; a++)
                {
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedBy(Main.rand.NextFloat(-1.7F, 1.7F)), Main.rand.Next(16, 18), Main.rand.NextFloat(0.7F, 1.1F));
                }
            }
            else
            {
                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedBy(Main.rand.NextFloat(-1.7F, 1.7F)), Main.rand.Next(16, 18), Main.rand.NextFloat(0.7F, 1.1F));
            }
        }
        public override bool PreAI()
        {
            NPC.ai[3]++;
            if (NPC.ai[3] % 180 == 0)
            {
                NPC.life += 1;
                if (NPC.life > NPC.lifeMax)
                {
                    NPC.life = NPC.lifeMax;
                }
            }

            if (NPC.ai[0] == 0)
            {
                NPC.spriteDirection = 0;
                NPC.ai[1]++;
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.X *= 0.2F;
                    if (Math.Abs(NPC.velocity.X) < 0.02)
                    {
                        NPC.velocity.X = 0;
                    }
                }
                if (Main.rand.NextBool(100) && NPC.ai[1] > 300 && NPC.velocity.Y == 0)
                {
                    NPC.ai[0] = 1;
                    if (Main.rand.NextBool(2))
                    {
                        NPC.ai[0] = -1;
                    }
                    NPC.velocity.X = 0.1F * NPC.ai[0];
                    NPC.netUpdate = true;
                    NPC.ai[1] = 0;
                    NPC.frameCounter = 0;
                }
                if (NPC.ai[2] != 0)
                {
                    NPC.ai[0] = 1;
                    if (Main.rand.NextBool(2))
                    {
                        NPC.ai[0] = -1;
                    }
                    NPC.velocity.X = 0.1F * NPC.ai[0];
                    NPC.netUpdate = true;
                    NPC.ai[1] = 0;
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.ai[2] != 0)
                {
                    Player player = Main.player[(int)NPC.ai[2] + 1];
                    if ((player.Center - NPC.Center).Length() < 500)
                    {
                        NPC.ai[1] = 0;
                        if (player.Center.X - NPC.Center.X < 0)
                        {
                            NPC.ai[0] = 1;
                        }
                        else
                        {
                            NPC.ai[0] = -1;
                        }
                    }
                }
                NPC.ai[1]++;
                if (NPC.velocity.Y == 0 && NPC.velocity.X == 0)
                {
                    NPC.velocity.Y = -5;
                    NPC.velocity.X = 1 * NPC.ai[0];
                }
                if (NPC.ai[0] > 0)
                {
                    if (NPC.velocity.X < 1)
                    {
                        NPC.velocity.X += 0.05F;
                    }
                }
                else
                {
                    if (NPC.velocity.X > -1)
                    {
                        NPC.velocity.X -= 0.05F;
                    }
                }
                NPC.spriteDirection = 1;
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = 0;
                }
                if (Main.rand.NextBool(100) && NPC.ai[1] > 300)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.netUpdate = true;
                    NPC.frameCounter = 0;
                }
            }
            if (NPC.velocity.X > 1)
            {
                NPC.velocity.X = 1;
            }
            if (NPC.velocity.X < -1)
            {
                NPC.velocity.X = -1;
            }
            return base.PreAI();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.胡萝卜精")),
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<农场>().ModBiomeBestiaryInfoElement),

            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<胡萝卜种子>()));
            npcLoot.CompleteModeLoot(ModContent.ItemType<巨大的胡萝卜种子>(),100,80,60);
        }
        public override void OnHitByProjectile(Projectile projectile, HitInfo hit, int damageDone)
        {
            NPC.ai[2] = projectile.owner-1;
            NPC.netUpdate = true;
        }
        public override void OnHitByItem(Player player, Item item, HitInfo hit, int damageDone)
        {
            NPC.ai[2] = player.whoAmI - 1;
            NPC.netUpdate = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            int[] TileArray = { ModContent.TileType<胡萝卜Tile>() };

            return TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY-1].TileType)
               && !NPC.AnyNPCs(NPCID.LunarTowerVortex)
               && !NPC.AnyNPCs(NPCID.LunarTowerStardust)
               && !NPC.AnyNPCs(NPCID.LunarTowerNebula)
               && !NPC.AnyNPCs(NPCID.LunarTowerSolar) && Main.invasionType == 0&&!Main.dayTime
               ? 0.5f : 0f;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] == 0)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 10 == 0)
                {
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 10 == 0)
                {
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 9)
                    {
                        NPC.frame.Y = frameHeight * 4;
                    }
                    else if (NPC.frame.Y <= frameHeight * 4)
                    {
                        NPC.frame.Y = frameHeight * 4;
                    }
                }
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
            spriteBatch.Draw(texture, NPC.Center - screenPos, frame, drawColor, NPC.rotation, new Vector2(frame.Width, frame.Height+8) / 2, NPC.scale, sprite, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>(Texture+"_Glow").Value, NPC.Center - screenPos, frame, Color.White, NPC.rotation, new Vector2(frame.Width, frame.Height+8) / 2, NPC.scale, sprite, 0);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
    }
}