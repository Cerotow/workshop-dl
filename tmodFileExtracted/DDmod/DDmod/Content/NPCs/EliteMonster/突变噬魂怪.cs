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
using DDmod.Content.Items.Accessory;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Ranged.Make.Gun;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 突变噬魂怪 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Withered Acorn Spirit");
           //DisplayName.AddTranslation(7, "枯萎的橡果之灵");

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = -10,
                PortraitPositionXOverride = 30,
                Rotation = -2F,
            };

            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
        }
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 2150;
            NPC.damage = 35;
            NPC.defense = 4;
            NPC.knockBackResist = 0f;
            NPC.width = 104;
            NPC.height = 104;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
           // NPC.Dnpc().Deathrattle = true;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            //NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.075f;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.突变噬魂怪, -1);
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation+MathHelper.Pi;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo) || NPC.AnyNPCs(Type))
            {
                return 0;
            }
            if (NPC.downedBoss1 && spawnInfo.Player.ZoneCorrupt&& spawnInfo.SpawnTileY <= Main.worldSurface)
            {
                if (spawnInfo.Player.HasBuff(ModContent.BuffType<腐臭Buff>()))
                {
                    return 0.1f;
                }
                else if (NPCDowned.突变噬魂怪)
                {
                    return 0.001f;
                }
                else
                {
                    return 0.02f;
                }
            }
            return 0f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                Biomes.TheCorruption,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.突变噬魂怪"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<突变噬魂怪纪念章物品>(), 10));
            //普通模式
            int[] A = new int[] { 956, 957, 958 };
            //ItemDropRule.OneFromOptions(1, 256, 257, 258);
            npcLoot.Add(ItemDropRule.NormalvsExpertOneFromOptions(1, 1, A));
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<魔光戒指>()));
            npcLoot.SpecialLoot(ModContent.ItemType<噩梦降临>(), 1);
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<突变噬魂怪圣物>()));
        }
        public override bool CheckActive()
        {
            return true;
        }
        public override bool CheckDead()
        {
            NPC.boss = true;
            return true;
        }
        public override void AI()
        {
            NPC.boss = false;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if ((player.Center - NPC.Center).Length() > 2000)
            {
                NPC.active = false;
            }
            if (player.dead)
            {
                NPC.velocity.Y -= 0.3f;
                return;
            }
            Vector2 vector = player.Center - NPC.Center;

            NPC.ai[2]++;
            if (NPC.Dnpc().Bool[0] || ((player.Center - NPC.Center).Length() > 100 && (player.Center - NPC.Center).Length() < 600))
            {
                if (NPC.ai[2] > 180)
                {
                    NPC.Dnpc().Bool[0] = true;
                    NPC.velocity *= 0.94F;
                    if (NPC.velocity.Length() < 0.2F)
                    {
                        int type = ModContent.ProjectileType<Boss魔唾液>();
                        SoundStyle sound = SoundID.NPCDeath1;
                        sound.Pitch = -0.5F;
                        sound.MaxInstances = 10;
                        PlaySound(sound, NPC.Center);
                        for (int a = -4; a <= 4; a++)
                        {
                            DNPC.NewNPCProj(NPC, NPC.Center, vector.PerfectNormalize().RotatedBy(0.1F * a + Main.rand.NextFloat(-0.1F, 0.1F)) * Main.rand.NextFloat(8, 15), type, 14, 0);
                        }
                        for (int A = 0; A < 120; A++)
                        {
                            int D = NewDust(NPC.Center-new Vector2(4), 1, 1, 18, 0f, 0f, 100, NPC.color, NPC.scale);
                            Main.dust[D].noGravity = false;
                            Main.dust[D].scale *= 1f + Main.rand.NextFloat(0.4F, 1.2F);
                            Main.dust[D].velocity = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F)) * Main.rand.NextFloat(0, 15);
                            Main.dust[D].position += Main.dust[D].velocity.PerfectNormalize()*12;
                        }
                        NPC.ai[2] = 0;
                        NPC.velocity -= vector.PerfectNormalize() * 10;
                        NPC.Dnpc().Bool[0] = false;
                    }
                }
            }
            if (!DDHelper.SpecifyDirection(NPC.velocity.ToRotation(), vector.ToRotation(), 0.1F))
            {
                //NPC.velocity *= 0.99F;
            }
            if (NPC.velocity.Length() > 6)
            {
                //NPC.velocity = NPC.velocity.PerfectNormalize() * 6;
            }
            NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
            if (!NPC.Dnpc().Bool[0])
            {
                NPC.velocity = (NPC.velocity * 100 + vector.PerfectNormalize() * 12F) / 101;
            }
            NPC.spriteDirection = 0;
            if (vector.X > 0)
            {
                NPC.spriteDirection = 1;
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int num582 = 0; (double)num582 < hit.Damage / (double)NPC.lifeMax * 100.0; num582++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 18,0, -1f, NPC.alpha, NPC.color, NPC.scale * 1.8F);
                }

            }
            if (NPC.life <= 0)
            {
                for (int A = 0; A < 120; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 18, 0f, 0f, 100, NPC.color, NPC.scale);
                    Main.dust[D].noGravity = false;
                    Main.dust[D].scale *= 1f + Main.rand.NextFloat(1, 2);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(0, 4)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("突变噬魂怪1").Type;
                    int GoreType2 = Mod.Find<ModGore>("突变噬魂怪2").Type;
                    int GoreType3 = Mod.Find<ModGore>("突变噬魂怪3").Type;
                    int GoreType4 = Mod.Find<ModGore>("突变噬魂怪4").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-7, 0).RotatedBy(NPC.rotation), GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(7, 0).RotatedBy(NPC.rotation), GoreType2, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(3, 0).RotatedBy(NPC.rotation), GoreType3, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(3, 0).RotatedBy(NPC.rotation), GoreType4, NPC.scale);
                }

            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 vector = NPC.Size / 2;

            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), drawColor, NPC.rotation + MathHelper.Pi, NPC.frame.Size()/2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

 
            return false;
        }
        public override void FindFrame(int frameHeight)
        {

            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = 0;
            }
        }
    }
}