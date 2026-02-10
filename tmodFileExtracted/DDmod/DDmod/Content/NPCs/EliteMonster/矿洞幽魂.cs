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
using Terraria.ModLoader.Config;
using DDmod.Content.Items.Magic.Staff.NPCLoot;
using System.Linq;
using Terraria.ModLoader.Utilities;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 矿洞幽魂 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1250;
            NPC.damage = 20;
            NPC.defense = 2;
            NPC.knockBackResist = 0;
            NPC.width = 64;
            NPC.height = 74;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.scale = 1.2F;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.BossLife = 1.025f;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.矿洞幽魂, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                Biomes.Underground,
                Biomes.Caverns,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.矿洞幽魂"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<GhostFireLanternItem>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<幽魂之杖>(), 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<矿洞幽魂圣物>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<矿洞幽魂纪念章物品>(), 10));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo))
            {
                return 0;
            }
            if (NPC.downedSlimeKing && !NPC.AnyNPCs(Type))
            {
                if (!NPCDowned.矿洞幽魂)
                {
                    return SpawnCondition.Cavern.Chance * 0.02f;
                }
                else
                {
                    return SpawnCondition.Cavern.Chance * 0.001f;
                }
            }
            else
            {
                return 0;
            }
        }
        public override bool CheckActive()
        {
            return true;
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (Main.rand.NextBool(4))
            {
                NPC.Dnpc().Times[3] = Main.rand.NextFloat(1F, 1.2F);
            }
            NPC.spriteDirection = 0;
            if (NPC.Dnpc().Stage ==0)
            {
                NPC.boss = false;
                NPC.ai[0]++;
                if (NPC.ai[0] < 80)
                {
                    NPC.velocity.Y = 0.4f;
                }
                else
                {
                    NPC.velocity.Y = -0.4f;
                    if (NPC.ai[0] >= 160)
                    {
                        NPC.ai[0] = 0;
                    }
                }
                if(NPC.lifeMax!=NPC.life)
                {
                    NPC.ai[0] = 0;
                    NPC.Dnpc().Stage = 1;
                }
                NPC.damage = 0;
            }
            if (NPC.Dnpc().Stage == 1)
            {
                NPC.damage = NPC.defDamage;
                NPC.boss = true;
                NPC.Dnpc().Neutrality = false;
                NPC.chaseable = true;
                if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
                NPC.TargetClosest();
                NPC.ai[0]++;
                Vector2 vector = player.Center - NPC.Center;
                if (vector.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                //转圈圈
                if (NPC.ai[0] < 300)
                {
                    NPC.ai[1] += 0.03F;
                    NPC.velocity = new Vector2(1, 0).RotatedBy(NPC.ai[1]);
                    if (NPC.ai[0] % 10 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Vector2 PO = player.Center + new Vector2(0,Main.rand.Next(50,400)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                        }
                    }
                }
                //冲刺
                else if (NPC.ai[0] == 300)
                {
                    NPC.velocity = vector.PerfectNormalize() * 10;
                }
                //刹车
                else if (NPC.ai[0] >= 360 && NPC.ai[0] < 400)
                {
                    if (NPC.ai[0] == 361)
                    {
                        for (int a = 0; a < 160; a++)
                        {
                            int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 1F));
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[A].customData = 0.4F;
                            Main.dust[A].noGravity = false;
                        }
                        if (Main.netMode != 1)
                        {
                            for (int a = 0; a < 20; a++)
                            {
                                Vector2 PO = NPC.Center + new Vector2(0, Main.rand.Next(50, 500)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                            }
                        }
                    }
                    NPC.velocity *= 0.92F;
                }
                //5秒发射弹幕
                else if (NPC.ai[0] >= 400 && NPC.ai[0] < 700)
                {
                    NPC.velocity = vector.PerfectNormalize() * 1;
                    DDHelper.BackAndForth(-1, 1, 0.02F, ref NPC.ai[1], ref NPC.Dnpc().Bool[0]);
                    NPC.velocity.Y += NPC.ai[1];
                    if (NPC.ai[0] % 100 <= 60 && NPC.ai[0] % 5 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Vector2 PO = player.Center + new Vector2(0, Main.rand.Next(50, 400)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                        }
                    }
                }
                //冲刺
                else if (NPC.ai[0] == 800)
                {
                    NPC.velocity = vector.PerfectNormalize() * 10;
                }
                //刹车
                else if (NPC.ai[0] >= 860 && NPC.ai[0] < 900)
                {
                    if (NPC.ai[0] == 861)
                    {
                        for (int a = 0; a < 80; a++)
                        {
                            int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 1F));
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[A].customData = 0.4F;
                            Main.dust[A].noGravity = false;
                        }
                        if (Main.netMode != 1)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                Vector2 PO = NPC.Center + new Vector2(0, Main.rand.Next(50, 500)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                            }
                        }
                    }
                    NPC.velocity *= 0.92F;
                }
                //冲刺
                else if (NPC.ai[0] == 900)
                {
                    NPC.velocity = vector.PerfectNormalize() * 10;
                }
                //刹车
                else if (NPC.ai[0] >= 960 && NPC.ai[0] < 1000)
                {
                    if (NPC.ai[0] == 961)
                    {
                        for (int a = 0; a < 80; a++)
                        {
                            int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 1F));
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[A].customData = 0.4F;
                            Main.dust[A].noGravity = false;
                        }
                        if (Main.netMode != 1)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                Vector2 PO = NPC.Center + new Vector2(0, Main.rand.Next(50, 500)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                            }
                        }
                    }
                    NPC.velocity *= 0.92F;
                }
                //冲刺
                else if (NPC.ai[0] == 1000)
                {
                    NPC.velocity = vector.PerfectNormalize() * 10;
                }
                //刹车
                else if (NPC.ai[0] >= 1060 && NPC.ai[0] < 1100)
                {
                    if (NPC.ai[0] == 1061)
                    {
                        for (int a = 0; a < 80; a++)
                        {
                            int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 1F));
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            Main.dust[A].customData = 0.4F;
                            Main.dust[A].noGravity = false;
                        }
                        if (Main.netMode != 1)
                        {
                            for (int a = 0; a < 10; a++)
                            {
                                Vector2 PO = NPC.Center + new Vector2(0, Main.rand.Next(50, 500)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                NewProjectile(NPC.GetSource_FromAI(), PO, Vector2.Zero, ModContent.ProjectileType<Boss幽灵鬼火>(), 8, 1);
                            }
                        }
                    }
                    NPC.velocity *= 0.92F;
                }
                if (NPC.ai[0] >= 1100)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
            }


            int dust = NewDust(NPC.position + new Vector2(0, NPC.height - 20), NPC.width, 20, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 1F));
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].customData = 0.4F;
            Main.dust[dust].noGravity = false;
            Lighting.AddLight(NPC.Center, new Vector3(193, 251, 255) * (0.001F));
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 300; a++)
                {
                    int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 2F));
                    Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D G = Glow.Value;
            Vector2 vector = NPC.Size / 2;
            if(NPC.spriteDirection==-1)
            {
                NPC.spriteDirection = 0;
            }

            Rectangle rectangle = NPC.frame;
            float A = 0.35F;
            if(NPC.velocity.Length()>4)
            {
                A = 1;
            }
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                Color color = new Color(255, 255, 255, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f);
                spriteBatch.Draw(G, vector2, new Rectangle?(rectangle), color* A, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
                color.A = 0;
                spriteBatch.Draw(texture, vector2, new Rectangle?(rectangle), color* A, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            spriteBatch.Draw(G, NPC.position - screenPos + vector, new Rectangle?(rectangle), new Color(255, 255, 255, 0), NPC.rotation, NPC.frame.Size()/2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(rectangle), Color.White, NPC.rotation, NPC.frame.Size()/2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

            return false;
        }
        public override void FindFrame(int frameHeight)
        {
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
        }
    }
}