using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Summon.Strengthen;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Projectiles.Hostile;
using DDmod.Content.Projectiles.Summon;
using System.Linq;

namespace DDmod.Content.NPCs.IittleMonster
{
    public class AcornSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Acorn Spirit");
           //DisplayName.AddTranslation(7, "橡果之灵");
            Main.npcFrameCount[NPC.type] = 1;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/IittleMonster/AcornSpirit_Glow");
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }
        public override void SetDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 12;
            NPC.defense = 2;
            if (Main.hardMode)
            {
                NPC.lifeMax = 180;
                NPC.defense = 12;
            }
            
            //NPC.Dnpc().LifeUP = false;
            NPC.damage = 0;
            NPC.knockBackResist = 1f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.npcSlots = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[24] = true;
            NPC.netAlways = true;
            NPC.Dnpc().Neutrality = true;
            NPC.dontTakeDamage = true;
            Banner = NPC.type;
            NPC.Dnpc().Properties.Level = 0;
            BannerItem = ModContent.ItemType<橡果之灵旗>();
        }
        public override bool? CanFallThroughPlatforms()
        {
            return NPC.noGravity;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.AcornSpirit"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfNature>(), 1, 2, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<自然橡果>(), 20));
            npcLoot.CompleteModeLoot(ModContent.ItemType<橡果灵灯>(), 40, 30, 20);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            bool U = false;
            if (Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType == 2)
            {
                for (int A = 0; A < 25; A++)
                {
                    if (Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY - A-1].HasTile && Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY - A-1].TileType == 5)
                    {
                        U = true;
                    }
                }
            }
            if (U)
            {
                int[] TileArray = { 2 };
                if (TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType)
                   && !NPC.AnyNPCs(NPCID.LunarTowerVortex)
                   && !NPC.AnyNPCs(NPCID.LunarTowerStardust)
                   && !NPC.AnyNPCs(NPCID.LunarTowerNebula)
                   && !NPC.AnyNPCs(NPCID.LunarTowerSolar))
                {
                    return 1.05f;
                }
            }
            return 0;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (NPC.Dnpc().NoMove)
            {
                NPC.ai[1] = 0;

            }
                if (Main.netMode == 2)
            {
                NPC.Dnpc().netUpdate = true;
            }
            if (NPC.ai[3] == 0)
            {
                NPC.ai[3]++;
                if (player.ZoneGraveyard &&Main.rand.NextBool(10) && !AnyNPCs(ModContent.NPCType<WitheredAcornSpirit>()))
                {
                    NPC.ai[3] = 100;
                }
                NPC.netUpdate = true;
            }
            if(player.dead)
            {
                NPC.ai[0] = -1;
            }
            if(NPC.ai[3]>=100)
            {
                int A=NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WitheredAcornSpirit>());
                Main.npc[A].velocity = NPC.velocity; 
                NPC.active = false;
            }
            if (NPC.ai[0] == -1)
            {
                NPC.rotation = NPC.velocity.X * 0.03f;
                Vector2 direction = NPC.Dnpc().vector[0] - NPC.Center;
                if (direction.Length() > 30)
                {
                    NPC.velocity = (NPC.velocity * 20 + direction.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1.1f,1.1f))* 0.5F) / 21;
                }
                if (NPC.velocity == Vector2.Zero)
                {
                    NPC.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 0.5F;
                }
                if (NPC.velocity.Length()<0.5F)
                {
                    NPC.velocity *= 1.01F;
                }
                if (NPC.ai[1] < 60)
                {
                    NPC.ai[1]++;
                }
            }
            if (NPC.ai[0] == 1)
            {
                NPC.rotation = NPC.velocity.X * 0.03f;
                int D = 1;
                if (player.Center.X - NPC.Center.X > 0)
                {
                    D = -1;
                }
                Vector2 direction = player.Center + new Vector2(200 * D, -200) - NPC.Center;
                NPC.velocity = (NPC.velocity * 20 + direction.PerfectNormalize() * 7) / 21;

                if (NPC.ai[1] < 60)
                {
                    NPC.ai[1]++;
                }
                if (NPC.ai[1] >= 60 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && direction.Length() < 100)
                {
                    direction = player.Center - NPC.Center;
                    if (Main.netMode != 1)
                    {
                        int damage =6;
                        if (Main.hardMode&&Main.expertMode)
                        {
                            damage = 36;
                        }
                            Projectile projectile = Main.projectile[NPC.NewNPCProj(NPC.Center, direction.PerfectNormalize() * 10, ModContent.ProjectileType<NaturalLight>(), damage, 1, Main.myPlayer)];
                        projectile.friendly = false;
                        projectile.hostile = true;
                        projectile.tileCollide = true;
                    }
                    NPC.velocity = -direction.PerfectNormalize() * 4;
                    NPC.ai[0] = 180;
                    NPC.ai[1] = 0;
                }
            }
            else
            {
                if (NPC.ai[2] == 0)
                {
                    Vector2 vector = NPC.Center/16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            break;
                        }
                    }
                    if (vector == Vector2.Zero|| (int)NPC.Center.X / 16<=0|| (int)NPC.Center.X / 16>=Main.maxTilesX|| (int)NPC.Center.Y / 16<=0|| (int)NPC.Center.Y/16>=Main.maxTilesY)
                    {
                        NPC.active = false;
                    }
                    NPC.Center = vector * 16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            }
                            break;
                        }
                    }
                    NPC.Center = vector * 16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            }
                            break;
                        }
                    }
                    NPC.Center = vector * 16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            }
                            break;
                        }
                    }
                    NPC.Center = vector * 16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            }
                            break;
                        }
                    }
                    NPC.Center = vector * 16;
                    for (int A = 0; A < 25; A++)
                    {
                        if (Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].HasTile && Main.tile[(int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1].TileType == 5)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                vector = new Vector2((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16) - A - 1);
                            }
                            break;
                        }
                    }
                    NPC.Center = vector * 16;
                    NPC.Dnpc().vector[0] = vector * 16;
                    NPC.ai[2] += 2;
                    NPC.ai[0] = -1;
                    NPC.netUpdate = true;
                }
                else if (NPC.ai[2] == 1)
                {
                    NPC.velocity.Y = -2f;
                    NPC.velocity.X = Main.rand.NextFloat(-5, 5);
                    NPC.ai[2]++;
                }
                else
                {
                    NPC.ai[2]++;
                }
                NPC.rotation += NPC.velocity.X * 0.1f;

                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.X *= 0.98F;
                }
                if (NPC.ai[0] > 1)
                {
                    NPC.ai[0]--;
                }
            }
            if (NPC.ai[0] >= 1)
            {
                NPC.Dnpc().Neutrality = false;
                NPC.chaseable = true;
                for (int a = 0; a < 200; a++)
                {
                    if (Main.npc[a].active && (Main.npc[a].Center - NPC.Center).Length() < 200 && Main.npc[a].ai[0] <= 0&& Main.npc[a].type == NPC.type)
                    {
                        Main.npc[a].ai[0] = 1;
                        Main.npc[a].noGravity = true;
                        Main.npc[a].netUpdate = true;
                    }
                }
            }
            NPC.dontTakeDamage = NPC.ai[2] <= 120;
            NPC.noGravity = NPC.ai[0] == 1|| NPC.ai[0] == -1;
            Lighting.AddLight(NPC.Center, new Vector3(20, 255, 20) * (0.003F* NPC.ai[1] / 50));
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.ai[0] <= 0)
            {
                NPC.ai[0] = 1;
                NPC.noGravity = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/AcornSpiritMini_Glow");
            Vector2 vector = NPC.Size / 2;
            if (!NPC.Dnpc().NoMove)
            {

                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                    Color color2 = NPC.GetAlpha(new Color(0, 255, 0, 0)) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f);
                    spriteBatch.Draw(texture2, vector2, null, color2, NPC.rotation + MathHelper.Pi, texture2.Size() / 2, (NPC.ai[1] / 50) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), (SpriteEffects)NPC.spriteDirection, 0);
                }
            }
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, drawColor, NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

            if (NPC.ai[0] == 1|| NPC.ai[0] == -1)
            {
                spriteBatch.Draw(Glow.Value, NPC.position - screenPos + vector, null, Color.White, NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            else
            {
                spriteBatch.Draw(Glow.Value, NPC.position - screenPos + vector, null, new Color(20,20,20), NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.Opacity = 1f;
            }
        }
    }
}