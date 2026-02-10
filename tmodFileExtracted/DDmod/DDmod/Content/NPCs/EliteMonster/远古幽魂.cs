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
using DDmod.Content.Items.Accessory;
using Terraria;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 远古幽魂 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 19;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/远古幽魂_Glow");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 3050;
            NPC.damage = 20;
            NPC.defense = 6;
            NPC.knockBackResist = 0;
            NPC.width = 34;
            NPC.height = 56;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.netAlways = true;
            NPC.alpha = 210;
            NPC.boss = true;
            NPC.scale = 1.2F;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.BossLife = 1.15f;
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
            SetEventFlagCleared(ref NPCDowned.远古幽魂, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                Biomes.TheDungeon,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.远古幽魂"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GhostFireLanternItem>(), 1));
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<远古魔戒>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<远古幽魂圣物>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<旧日秽灵纪念章物品>(), 10));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo)|| NPC.AnyNPCs(Type))
            {
                return 0;
            }
            if ((NPC.downedBoss3|| spawnInfo.Player.HasBuff(ModContent.BuffType<远古能量Buff>())))
            {
                if (spawnInfo.Player.HasBuff(ModContent.BuffType<远古能量Buff>()))
                {
                    return SpawnCondition.Dungeon.Chance * 0.1f;
                }
                else if (!NPCDowned.远古幽魂)
                {
                    return SpawnCondition.Dungeon.Chance * 0.02f;
                }
                else
                {
                    return SpawnCondition.Dungeon.Chance * 0.001f;
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
            Vector2 vector = player.Center - NPC.Center;
            NPC.spriteDirection = 0;
            int DI = 1;
            if (vector.X < 0)
            {
                NPC.spriteDirection = 1;
                DI = -1;
            }
            if (NPC.Dnpc().Stage == 0)
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
                if (NPC.lifeMax != NPC.life || vector.Length() < 500)
                {
                    NPC.ai[0] = 0;
                    NPC.Dnpc().Stage = 1;
                    for (int a = 0; a < 100; a++)
                    {
                        int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                        Main.dust[dust].velocity = (Main.dust[dust].position - NPC.Center).PerfectNormalize() * Main.rand.NextFloat(5);
                        Main.dust[dust].customData = 0.4F;
                        Main.dust[dust].noGravity = false;
                    }
                }
                NPC.damage = 0;
            }
            if (NPC.Dnpc().Stage == 1)
            {
                NPC.alpha = 80;
                //NPC.damage = NPC.defDamage;
                NPC.boss = true;
                NPC.Dnpc().Neutrality = false;
                NPC.chaseable = true;
                if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
                NPC.TargetClosest();
                bool QT = !Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
                if (QT && NPC.ai[0] % 300 <= 60)
                {
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * 14) / 21;
                }
                else
                {
                    NPC.velocity *= 0.92f;
                }
                NPC.ai[0]++;
                if (NPC.ai[0] % 300 > 60)
                {
                    if (NPC.ai[1] < 3)
                    {
                        if (NPC.ai[0] % 30 == 0)
                            NPC.NewNPCProj(NPC.Center + new Vector2(10 * DI, Main.rand.NextFloat(-30, 30)), new Vector2(4 * DI, 0), ModContent.ProjectileType<Boss远古能量弹>(), 20, 1);
                    }
                    else if (NPC.ai[1] == 3)
                    {
                        if (NPC.ai[0] % 10 == 0)
                            NPC.NewNPCProj(NPC.Center + new Vector2(10 * DI, Main.rand.NextFloat(-30, 30)), new Vector2(4 * DI, Main.rand.NextFloat(-4, 4)), ModContent.ProjectileType<Boss远古能量弹>(), 20, 1, -1, 1);
                    }
                    else if (NPC.ai[1] == 4)
                    {
                        if (NPC.ai[0] % 60 == 0)
                        {
                            Vector2 P = new Vector2(20 * DI, Main.rand.NextFloat(-30, 30));
                            for (int a = 0; a < 30; a++)
                            {
                                int dust = NewDust(NPC.Center + P - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                                Main.dust[dust].velocity = new Vector2(4).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].customData = 2.4F;
                                Main.dust[dust].noGravity = false;
                            }
                            if (Main.netMode != 1)
                            {
                                int N = DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center + P, 34, 0);
                                Main.npc[N].velocity = new Vector2(6 * DI, 0);
                                Main.npc[N].rotation = 0;
                            }
                        }
                    }
                }
                if (NPC.ai[0] % 300 == 0)
                {
                    if (NPC.ai[1] < 2)
                    {
                        CS();
                    }
                    else if (NPC.ai[1] < 3)
                    {
                        NPC.ai[2] = Main.rand.NextBool(2) ? 1 : -1;
                        CS2();
                        NPC.netUpdate = true;
                    }
                    else if (NPC.ai[1] < 4)
                    {
                        CS();
                    }
                    else
                    {

                        NPC.ai[1] = -1;
                        NPC.ai[0] = 0;
                    }
                    NPC.ai[1]++;
                }
            }
            void CS()
            {
                for (int a = 0; a < 100; a++)
                {
                    int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                    Main.dust[dust].velocity = (Main.dust[dust].position - NPC.Center).PerfectNormalize() * Main.rand.NextFloat(5);
                    Main.dust[dust].customData = 2.4F;
                    Main.dust[dust].noGravity = false;
                }
                if (!player.活着())
                {
                    NPC.Kill(false);
                    return;
                }
            NPC.Center = player.Center + new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
            for (int a = 0; a < 100; a++)
            {
                int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                Main.dust[dust].velocity = (Main.dust[dust].position - NPC.Center).PerfectNormalize() * Main.rand.NextFloat(5);
                Main.dust[dust].customData = 2.4F;
                Main.dust[dust].noGravity = false;
            } }
            void CS2()
            {
                for (int a = 0; a < 100; a++)
                {
                    int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                    Main.dust[dust].velocity = (Main.dust[dust].position - NPC.Center).PerfectNormalize() * Main.rand.NextFloat(5);
                    Main.dust[dust].customData = 2.4F;
                    Main.dust[dust].noGravity = false;
                }
                if (!player.活着())
                {
                    NPC.Kill(false);
                    return;
                }
                NPC.Center = player.Center + new Vector2(Main.rand.Next(200, 400) * NPC.ai[2], 0);
                for (int a = 0; a < 100; a++)
                {
                    int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.6F, 1.4F));
                    Main.dust[dust].velocity = (Main.dust[dust].position - NPC.Center).PerfectNormalize() * Main.rand.NextFloat(5);
                    Main.dust[dust].customData = 2.4F;
                    Main.dust[dust].noGravity = false;
                }
            }
            Find();
            NPC.rotation = NPC.velocity.X * 0.03F;

        }
        public void Find()
        {

            int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.2F, 0.4F));
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].customData = 0.4F;
            Main.dust[dust].noGravity = false;
            Lighting.AddLight(NPC.Center, new Vector3(84, 107, 221) * (0.001F));
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 300; a++)
                {
                    int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(84, 107, 221, 0), Main.rand.NextFloat(0.3F, 2F));
                    Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float SC = 0;
            if(NPC.ai[0] % 300 > 60)
            {
                SC = (NPC.ai[0] % 300 - 60) / 20;
            }
            if(SC>1)
            {
                SC = 1;
            }
            int DI = 1;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D G = Glow.Value;
            Vector2 vector = NPC.Size / 2;
            if(NPC.spriteDirection==1)
            {
                DI = -1;
            }
            if(NPC.spriteDirection==-1)
            {
                NPC.spriteDirection = 0;
                DI = -1;
            }
            NPC.Dnpc().Times[0] += 0.05F* DI;

            Rectangle rectangle = NPC.frame;
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                Color color = NPC.GetAlpha(new Color(84, 107, 221, 0)) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f);
                color.A = 0;
                spriteBatch.Draw(texture, vector2, new Rectangle?(rectangle), color, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            }
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size()/2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            
            spriteBatch.Draw(G, NPC.position - screenPos + vector, new Rectangle?(rectangle), NPC.GetAlpha(Color.White), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
          if (NPC.Dnpc().Stage!=0&& NPC.ai[0] % 300 > 60)
            {
                spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.position - screenPos + vector + new Vector2(30 * DI, 0), null, new Color(84, 107, 221, 0), NPC.rotation, DDTextures.VoidStar.Size() / 2, new Vector2(0.4F, 1) * SC, (SpriteEffects)NPC.spriteDirection, 0f);

                DDHelper.Compression(texture, new Color(84, 107, 221,0), new Vector2(1,0).ToRotation(), NPC.Opacity, new Vector2(5, 1), 1, NPC.Dnpc().Times[0], BlendState.Additive);
                spriteBatch.Draw(DDTextures.Circle[9].Value, NPC.position - screenPos + vector+new Vector2(30* DI, 0), null, new Color(84, 107, 221, 0), NPC.rotation, DDTextures.Circle[9].Size() / 2, SC/4, (SpriteEffects)NPC.spriteDirection, 0f);
                spriteBatch.Draw(DDTextures.Circle[9].Value, NPC.position - screenPos + vector+new Vector2(30* DI, 0), null, new Color(84, 107, 221, 0), NPC.rotation, DDTextures.Circle[9].Size() / 2, SC/4, (SpriteEffects)NPC.spriteDirection, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.Opacity = 1f;
            }
            NPC.frameCounter++;

            if (NPC.ai[0] % 300 <= 60|| NPC.Dnpc().Stage == 0)
            {
                if (NPC.frameCounter > 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 8)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                if (NPC.frameCounter > 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y < frameHeight * 8)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                if (NPC.frame.Y >= frameHeight * 19)
                {
                    NPC.frame.Y = frameHeight * 9;
                }
            }
        }
    }
}