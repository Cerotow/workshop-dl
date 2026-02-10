using DDmod.Content.Dusts;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Projectiles.Summon;
using DDmod.Worlds;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using DDmod.SubworldLibraryWorld;
using SubworldLibrary;
using DDmod.Content.Items.Boss.MiniBoss;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class WitheredAcornSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Withered Acorn Spirit");
           //DisplayName.AddTranslation(7, "枯萎的橡果之灵");
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Glow2;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/WitheredAcornSpirit_Glow");
            Glow2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/WitheredAcornSpirit_Glow2");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1250;
            if(SubworldSystem.IsActive<Graveyard2>())
            {
                NPC.lifeMax = 2500;
            }
            NPC.damage = 20;
            NPC.defense = 2;
            NPC.knockBackResist = 1f;
            NPC.width = 44;
            NPC.height = 44;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.netAlways = true;
            NPC.Dnpc().Deathrattle = true;
            NPC.boss = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.dontTakeDamage = true;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Neutrality = true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return NPC.noGravity;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.downedWitheredAcornSpirit, -1);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                Biomes.Surface,
                Biomes.Graveyard,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.WitheredAcornSpirit"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<DeadLeavesSpiritItem>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<枯萎橡果炮>(), 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<枯萎的橡果之灵圣物>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<枯萎的橡果之灵面具>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<枯萎的橡果之灵纪念章物品>(), 10));
        }
        public override bool CheckActive()
        {
            return !SubworldSystem.IsActive<Graveyard>();
        }
        public override bool CheckDead()
        {
            return false;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (NPC.Dnpc().Bool[0])
            {
                NPC.dontTakeDamage = true;
                NPC.ai[0]++;
                DDHelper.BackAndForth(120, 180, 8F, ref NPC.localAI[0], ref NPC.Dnpc().Bool[1]);
                if (Main.rand.NextBool(5))
                {
                    Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8f, 5), ModContent.ProjectileType<WitheringLight>(), 0, 0, Main.myPlayer, 4)];
                    projectile.tileCollide = true;
                }
                if (NPC.ai[0] > 210)
                {
                    NPC.Kill();
                    SoundStyle sound = SoundID.NPCDeath39;
                    sound.Pitch = -1;
                    PlaySound(sound, NPC.Center);
                    for (int A = 0; A < 400; A++)
                    {
                        int Type = ModContent.DustType<枯萎粒子>();
                        Dust dust = Main.dust[NewDust(NPC.Center, 1, 1, Type, NPC.oldVelocity.X, NPC.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(1F, 2F);
                        dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * dust.scale * 10;
                    }
                    if (Main.netMode != NetmodeID.Server)
                    {
                        int GoreType = Mod.Find<ModGore>("WitheredAcornSpirit").Type;
                        int GoreType2 = Mod.Find<ModGore>("WitheredAcornSpirit2").Type;
                        int GoreType3 = Mod.Find<ModGore>("WitheredAcornSpirit3").Type;

                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -4), GoreType, 1f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(4, 0), GoreType2, 1f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-4, 0), GoreType3, 1f);
                    }
                }
                NPC.velocity = new Vector2(Main.rand.NextFloat(-2.6f,2.6f), Main.rand.NextFloat(-2.6f, 0.6f));
                NPC.rotation = NPC.velocity.X * 0.03F;
                return;
            }
            if ((player.Center - NPC.Center).Length() > 2000&& !SubworldSystem.IsActive<Graveyard>())
            {
                NPC.active = false;
            }
            if (player.dead)
            {
                NPC.velocity.Y -= 0.3f;
                if (NPC.localAI[0] < 120)
                {
                    NPC.localAI[0]++;
                }
                NPC.noGravity = true;
                return;
            }
            NPC.damage = 0;
            if (NPC.velocity.Length() > 5)
            {
                NPC.damage = NPC.defDamage;
            }
            if (NPC.ai[0] == 1)
            {
                NPC.Dnpc().Neutrality = false;
                NPC.chaseable = true;
                if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
                NPC.noTileCollide = !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
                NPC.rotation = NPC.velocity.X * 0.03f;
                int D = 1;
                if (player.Center.X - NPC.Center.X > 0)
                {
                    D = -1;
                }
                //发射方向
                Vector2 direction = player.Center - NPC.Center;
                //移动方向
                Vector2 velocity = player.Center - NPC.Center;
                if (NPC.ai[3] == 0)
                {
                    if (NPC.localAI[0] < 120 && NPC.ai[1] == 0)
                    {
                        NPC.localAI[0]++;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.localAI[0] += 3;
                        }
                    }
                    else if (NPC.ai[1] == 0)
                    {
                        NPC.ai[1] = 1;
                    }
                    else
                    {
                        NPC.localAI[1]++;
                    }
                    velocity += new Vector2(300 * D, -250);
                    NPC.velocity = (NPC.velocity * 20 + velocity.PerfectNormalize() * 12 + (velocity.PerfectNormalize() * player.velocity.Length() / 4)) / 21;
                    if (NPC.ai[1] >= 1 && NPC.ai[1] <= 5 && NPC.localAI[1] % 40 == 0 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && velocity.Length() < 150)
                    {
                        direction += player.velocity * 30;
                        Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction.PerfectNormalize() * 10, ModContent.ProjectileType<WitheringLight>(), 9, 1, Main.myPlayer)];
                        projectile.tileCollide = true;
                        NPC.velocity = -direction.PerfectNormalize() * 6;
                        NPC.ai[1]++;
                    }
                    if (NPC.ai[1] > 5)
                    {
                        NPC.ai[1] = 0;
                        NPC.localAI[0] = 0;
                        NPC.ai[0] = 300;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.ai[0] *= 1.5f;
                        }
                        NPC.ai[3]++;
                    }
                }
                else if (NPC.ai[3] == 1)
                {
                    if (NPC.localAI[0] < 180 && NPC.ai[1] == 0)
                    {
                        NPC.localAI[0]++;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.localAI[0] += 3;
                        }
                    }
                    else if (NPC.ai[1] == 0)
                    {
                        NPC.ai[1] = 1;
                    }
                    else
                    {
                        NPC.localAI[1]++;
                    }
                    velocity += new Vector2(0, -200);
                    float SP = velocity.Length();
                    if (SP > 17)
                    {
                        SP = 17;
                    }
                    NPC.velocity = (NPC.velocity * 20 + velocity.PerfectNormalize() * SP) / 21;
                    if (NPC.ai[1] == 1 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && velocity.Length() < 10)
                    {
                        for (int A = -6; A <= 6; A++)
                        {
                            Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10).RotatedBy(0.15F * A), ModContent.ProjectileType<WitheringLight>(), 9, 1, Main.myPlayer, 1)];
                            projectile.tileCollide = true;
                            NPC.velocity = new Vector2(0, 10);
                            NPC.ai[1]++;
                        }
                    }
                    if (NPC.ai[1] > 1)
                    {
                        NPC.ai[1] = 0;
                        NPC.localAI[0] = 0;
                        NPC.ai[0] = 300;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.ai[0] *= 1.5f;
                        }
                        NPC.ai[3]++;
                    }
                }
                else
                {
                    NPC.noTileCollide = false;
                    if (NPC.localAI[0] < 210 && NPC.ai[1] == 0)
                    {
                        NPC.localAI[0]++;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.localAI[0] += 3;
                        }
                    }
                    else if (NPC.ai[1] == 0)
                    {
                        NPC.ai[1] = 1;
                    }
                    else
                    {
                        NPC.localAI[1]++;
                    }
                    velocity += new Vector2(0, -400);
                    float SP = velocity.Length();
                    if (SP > 17)
                    {
                        SP = 17;
                    }
                    NPC.velocity = (NPC.velocity * 20 + velocity.PerfectNormalize() * SP) / 21;
                    if (NPC.ai[1] == 1 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && velocity.Length() < 10)
                    {
                        Projectile projectile = Main.projectile[NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 12), ModContent.ProjectileType<WitheringLight>(), 20, 1, Main.myPlayer, 2)];
                        projectile.tileCollide = true;
                        NPC.velocity = new Vector2(0, -10);
                        NPC.ai[1]++;
                    }
                    if (NPC.ai[1] > 1)
                    {
                        NPC.ai[1] = 0;
                        NPC.localAI[0] = 0;
                        NPC.ai[0] = 300;
                        if (NPC.lifeMax / 2 > NPC.life)
                        {
                            NPC.ai[0] *= 1.5f;
                        }
                        NPC.ai[3] = 0;
                    }
                }
            }
            else
            {
                if (NPC.ai[2] == 0)
                {
                    NPC.velocity.Y = -2f;
                    NPC.velocity.X = Main.rand.NextFloat(-5, 5);
                }
                NPC.ai[2]++;
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
            NPC.dontTakeDamage = NPC.ai[2] <= 120;
            NPC.noGravity = NPC.ai[0] == 1;
            Lighting.AddLight(NPC.Center, new Vector3(255, 20, 20) * (0.001F * NPC.localAI[0] / 50));
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.ai[0] == 0)
            {
                NPC.ai[0] = 1;
                NPC.noGravity = true;
            }
            if (NPC.life <= 0 && NPC.Dnpc().Deathrattle)
            {
                NPC.dontTakeDamage = true;
                NPC.Dnpc().Bool[0] = true;
                NPC.ai[0] = 0;
                NPC.life = 10;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D texture2 = Glow2.Value;
            Vector2 vector = NPC.Size / 2;


            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                vector2.Y -= 2;
                Color color2 = NPC.GetAlpha(new Color(255, 0, 0, 0)) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f);

                if (!AnyNPCs(Type))
                {
                    color2 = new Color(255, 0, 0, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f);
                }
                spriteBatch.Draw(texture2, vector2, null, color2, NPC.rotation + MathHelper.Pi, texture2.Size() / 2, (NPC.localAI[0] / 100) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), (SpriteEffects)NPC.spriteDirection, 0);
            }
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), drawColor, NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

            if (NPC.ai[0] == 1)
            {
                spriteBatch.Draw(Glow.Value, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), Color.White, NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            else
            {
                spriteBatch.Draw(Glow.Value, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), new Color(20, 20, 20), NPC.rotation + MathHelper.Pi, vector, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = -30,
            };

            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;

            if (NPC.IsABestiaryIconDummy)
            {
                NPC.Opacity = 1f;
            }
            NPC.frameCounter++;
            if (NPC.frameCounter > 10)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = 0;
            }

            if (NPC.ai[0] != 1)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            if(!AnyNPCs(Type))
            {
                NPC.localAI[0] = 80;
                NPC.ai[0] = 1;
            }
        }
    }
}