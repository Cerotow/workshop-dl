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
using Terraria;
using System.Linq;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 妖精王 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Withered Acorn Spirit");
           //DisplayName.AddTranslation(7, "枯萎的橡果之灵");

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = -10,
                PortraitPositionXOverride = 30,
                Rotation = -2F,
            };

            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Wing;
        public static Asset<Texture2D> WingD;
        public static Asset<Texture2D> WingX;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            Wing = ModContent.Request<Texture2D>(Texture+"_Wing");
            WingD = ModContent.Request<Texture2D>(Texture+"_WingD");
            WingX = ModContent.Request<Texture2D>(Texture+"_WingX");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5150;
            NPC.damage = 35;
            NPC.defense = 4;
            NPC.knockBackResist = 0f;
            NPC.width = 34;
            NPC.height = 34;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.netAlways = true;
           // NPC.Dnpc().Deathrattle = true;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            //NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.BossLife = 1.2f;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.妖精王, -1);
        }
        public override void ModifyTypeName(ref string typeName)
        {
            if (NPC.Dnpc().Stage <= 0)
            {
                typeName = "???";
            }
        }
        public override void BossHeadRotation(ref float rotation)
        {
            //rotation = NPC.rotation+MathHelper.Pi;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo) || NPC.AnyNPCs(Type))
            {
                return 0;

            }
            int[] TileArray = { 109 };
            if (TileArray.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType))
            {
                if (spawnInfo.Player.HasBuff(ModContent.BuffType<神圣仙酒Buff>()))
                {
                    return 0.1f;
                }
                else if (NPCDowned.妖精王)
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
                Biomes.TheHallow,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.妖精王"))
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<神圣戒指>()));
            npcLoot.Add(new DropLocalPerClient(761,4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<妖精王纪念章物品>(), 10));
            npcLoot.Add(ItemDropRule.Common(501,1,12,40));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<妖精王圣物>()));
            //普通模式
            //int[] A = new int[] { 956, 957, 958 };
            //ItemDropRule.OneFromOptions(1, 256, 257, 258);
            //npcLoot.Add(ItemDropRule.NormalvsExpertOneFromOptions(1, 1, A));
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
       float W = 0;
        float W2 = 0;
        float W3 = 0;

        float W4 = 0;
        float W5 = 0;
        float W6 = 0;
        float W7 = 0;
        float W8 = 0;
        float W9 = 0;
        float W10 = 0;

        bool WB;
        bool WB2;
        bool WB3;
        public override void AI()
        {
            NPC.damage = 0;
            NPC.TargetClosest();
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }
            SoundStyle sound = SoundID.NPCHit5;
            sound.MaxInstances = 10;
            if (Main.rand.NextBool(3))
            {
                int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(0, (int)(17*NPC.scale)), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(255, 191, 0, 100), Scale: NPC.scale);
                Main.dust[A].velocity = Vector2.Zero;
                Main.dust[A].noGravity = true;
                Main.dust[A].customData = NPC.scale;
            }
            Player player = Main.player[NPC.target];
            if ((player.Center - NPC.Center).Length() > 2000)
            {
                NPC.active = false;
            }
            if(NPC.Dnpc().Stage==0)
            {
                NPC.scale = 0.3F;
                NPC.velocity = Vector2.Zero;
                NPC.boss = false;
                if (!Main.dedServ) Music = -1;
                NPC.dontTakeDamage = (player.Center - NPC.Center).Length() > 300;
                if (NPC.life<NPC.lifeMax)
                {
                    NPC.Dnpc().Stage = 1;
                }

                return;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center,15,false,0.15F);
                NPC.dontTakeDamage = true;
                if (NPC.scale<1.15F)
                {
                    NPC.scale+=0.01F;
                    for (int a = 0; a < 4; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(255, 191, 0, 100), Scale: 1.8F);
                        Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 3.5f + Main.dust[A].DustAI(3);
                    }
                }
                else
                {
                    NPC.scale = 1.15F;
                    if (NPC.Dnpc().Stage==1)
                    {
                        for (int A = 0; A < 120; A++)
                        {
                            int D = NewDust(NPC.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 100, new Color(255, 191, 0, 100), NPC.scale);
                            Main.dust[D].noGravity = true;
                            Main.dust[D].scale *= Main.rand.NextFloat(1.5F, 2.5F);
                            Main.dust[D].customData = 3F;
                            Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(4, 6)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                        }
                        sound.Pitch = 0.5F;
                        PlaySound(sound, NPC.Center);
                        NPC.Dnpc().Stage = 2;
                    }
                }
                return;
            }
            if (player.dead)
            {
                NPC.velocity.Y -= 0.3f;
                return;
            }
            NPC.dontTakeDamage = false;
            if (NPC.ai[2] < 60)
            {
                NPC.ai[2]++;
            }
            NPC.boss = true;
            NPC.Dnpc().Neutrality = false;
            NPC.chaseable = true;
            if (!Main.dedServ && Music == -1) Music = DDSystem.MiniBossMusic;

            //player.Dplayer().Bossperspective(NPC.Center,10,false,1F);
            Vector2 vector = player.Center - NPC.Center;
            NPC.rotation += NPC.velocity.X*0.03F;
            NPC.ai[0]++;
            if (NPC.ai[1]==0)
            {
                NPC.velocity = (NPC.velocity * 100 + vector.PerfectNormalize() * 8F) / 101;
                if (NPC.ai[0]>=120)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = -5;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                NPC.velocity = (NPC.velocity * 100 + vector.PerfectNormalize() * 8F) / 101;
                if (NPC.ai[0]%30==0 && NPC.ai[0] < 300)
                {
                    sound.Pitch = 1;
                    NPC.NewNPCProj(NPC.Center,vector.PerfectNormalize()*12,ModContent.ProjectileType<Boss妖精弹>(),13,0);
                    for (int A = 0; A < 40; A++)
                    {
                        int D = NewDust(NPC.Center -new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(255, 191, 0, 100), NPC.scale);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.2F;
                        Main.dust[D].customData = 4F;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0,10), MathHelper.TwoPi/40*A, default);
                    }
                    PlaySound(sound, NPC.Center);
                    NPC.velocity *= 0.1F;
                }
                if (NPC.ai[0] >= 300)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = -20;
                }
            }
            else if (NPC.ai[1] == 2)
            {
                NPC.velocity = (NPC.velocity * 100 + vector.PerfectNormalize() * 8F) / 101;
                if (NPC.ai[0] >=0&& NPC.ai[0] % 60<=30&& NPC.ai[0] % 5==0&& NPC.ai[0]<300)
                {
                    sound.Pitch = 1;
                    NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 12, ModContent.ProjectileType<Boss妖精弹>(), 13, 0);
                    for (int A = 0; A < 40; A++)
                    {
                        int D = NewDust(NPC.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(255, 191, 0, 100), NPC.scale);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.2F;
                        Main.dust[D].customData = 4F;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0, 10), MathHelper.TwoPi / 40 * A, default);
                    }
                    PlaySound(sound, NPC.Center);
                    NPC.velocity *= 0.1F;
                }
                if (NPC.ai[0] >= 300)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = -5;
                }
            }
            else if (NPC.ai[1] == 3)
            {
                NPC.velocity = (NPC.velocity * 100 + vector.PerfectNormalize() * 15F) / 101;

                if (NPC.ai[0] % 60 < 10 && NPC.ai[0] % 10 == 0 && NPC.ai[0] < 300)
                {
                    sound.Pitch = 1;
                    NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 8, ModContent.ProjectileType<Boss妖精弹>(), 20, 0,-1,1);
                    for (int A = 0; A < 40; A++)
                    {
                        int D = NewDust(NPC.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(255, 191, 0, 100), NPC.scale);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.2F;
                        Main.dust[D].customData = 4F;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0, 10), MathHelper.TwoPi / 40 * A, default);
                    }
                    PlaySound(sound, NPC.Center);
                    NPC.velocity *= 0.1F;
                }

                if (NPC.ai[0] >= 300)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = -5;
                }
            }
            else if (NPC.ai[1] == 4)
            {
                NPC.ai[2] -= 4;
                NPC.velocity *= 0.8F;
                if(NPC.ai[2]<=0&& NPC.ai[2]%20==0)
                {
                    for (int A = 0; A < 6; A++)
                    {
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 8, ModContent.ProjectileType<Boss妖精弹>(), 16, 0, -1, 3);
                    }
                    for (int A = 0; A < 40; A++)
                    {
                        int D = NewDust(NPC.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(255, 191, 0, 100), NPC.scale);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.2F;
                        Main.dust[D].customData = 4F;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0, 10), MathHelper.TwoPi / 40 * A, default);
                    }
                    sound.Pitch = 0.5F;
                    PlaySound(sound, NPC.Center);
                }
                if (NPC.ai[0] >= 600)
                {
                    NPC.ai[1]++;
                    NPC.ai[2] = 0;
                    NPC.ai[0] = -5;
                }
            }
            else if (NPC.ai[1] == 5)
            {
                NPC.velocity =Vector2.Zero;

                if (NPC.ai[0] % 5 == 0)
                {
                     NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 8, ModContent.ProjectileType<Boss妖精弹>(), 16, 0, -1, 4);
                    
                    for (int A = 0; A < 20; A++)
                    {
                        int D = NewDust(NPC.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 0, new Color(255, 191, 0, 100), NPC.scale);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1F;
                        Main.dust[D].customData = 3F;
                        GlobalDust.DustNPCOwner[D] = NPC.whoAmI;
                        Main.dust[D].velocity = Utils.RotatedBy(new Vector2(0, 6), MathHelper.TwoPi / 20 * A, default);
                    }
                    sound.Pitch = 0.5F;
                    PlaySound(sound, NPC.Center);
                }

                if (NPC.ai[0] >= 120)
                {
                    NPC.ai[1]=0;
                    NPC.ai[0] = -5;
                }
            }
            Lighting.AddLight(NPC.Center, new Color(255, 191, 0, 100).ToVector3());
        }
        public override void HitEffect(HitInfo hit)
        {
            if(NPC.ai[1]==4)
            {
               NPC.life+=hit.Damage*3;

                CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), hit.Damage * 3, true);
            }
            if (NPC.life > 0)
            {
                for (int num582 = 0; (double)num582 < hit.Damage / (double)NPC.lifeMax * 100.0; num582++)
                {
                    int D = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, -1f, NPC.alpha, new Color(255, 191, 0, 100), NPC.scale);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(0, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector;
                }

            }
            if (NPC.life <= 0)
            {
                
                for (int A = 0; A < 120; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 100, new Color(255, 191, 0, 100), NPC.scale);
                    Main.dust[D].noGravity = false;
                    Main.dust[D].scale *= 1f + Main.rand.NextFloat(0.6F, 1.2F);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(0, 12)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("妖精王翅膀1").Type;
                    int GoreType2 = Mod.Find<ModGore>("妖精王翅膀2").Type;
                    int GoreType3 = Mod.Find<ModGore>("妖精王翅膀3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(10, 0), new Vector2(3, 0), GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(-10, 0), new Vector2(-3, 0), GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(10, 0), new Vector2(3, 0), GoreType2, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(-10, 0), new Vector2(-3, 0), GoreType2, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(10, 0), new Vector2(3, 0), GoreType3, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center+new Vector2(-10, 0),new Vector2(-3, 0), GoreType3, NPC.scale);
                }

            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture;
            float CBSC = NPC.ai[2]/60;
            if(CBSC>1)
            {
                CBSC = 1;
            }
            if(CBSC<0)
            {
                CBSC = 0;
            }
            Vector2 vector = NPC.Size / 2;
            texture = Wing.Value;

            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(8, -8), null, drawColor, W, new Vector2(4, 20), NPC.scale * CBSC, 0, 0f);
            texture = WingX.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(14, 8), null, drawColor, W3, new Vector2(4, 16), NPC.scale * CBSC, 0, 0f);

            texture = WingD.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(10, 0), null, drawColor, W2, new Vector2(6, 28), NPC.scale * CBSC, 0, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(10, 0), null, drawColor * 0.4f, W4, new Vector2(6, 28), NPC.scale* CBSC, 0, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(10, 0), null, drawColor * 0.3f, W5, new Vector2(6, 28), NPC.scale * CBSC, 0, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(10, 0), null, drawColor * 0.2f, W6, new Vector2(6, 28), NPC.scale * CBSC, 0, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(10, 0), null, drawColor * 0.1f, W7, new Vector2(6, 28), NPC.scale * CBSC, 0, 0f);



            texture = Wing.Value;

            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-8, -8), null, drawColor, -W, new Vector2(32, 20), NPC.scale * CBSC, (SpriteEffects)1, 0f);
            texture = WingX.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-14, 8), null, drawColor, -W3, new Vector2(24, 16), NPC.scale * CBSC, (SpriteEffects)1, 0f);

            texture = WingD.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-10, 0), null, drawColor,  -W2, new Vector2(42, 28), NPC.scale * CBSC, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-10, 0), null, drawColor*0.4f,  -W4, new Vector2(42, 28), NPC.scale * CBSC, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-10, 0), null, drawColor*0.3f,  -W5, new Vector2(42, 28), NPC.scale * CBSC, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-10, 0), null, drawColor*0.2f,  -W6, new Vector2(42, 28), NPC.scale * CBSC, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector + NPC.scale * new Vector2(-10, 0), null, drawColor*0.1f,  -W7, new Vector2(42, 28), NPC.scale * CBSC, (SpriteEffects)1, 0f);


            texture = Glow.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 100), NPC.rotation, texture.Size() / 2, NPC.scale / 4 * NPC.Dnpc().Times[4], (SpriteEffects)NPC.spriteDirection, 0f);

            /*
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                Color color = new Color(255, 191, 0, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw(texture, vector2, null, color, NPC.rotation, texture.Size() / 2, NPC.scale/4, (SpriteEffects)NPC.spriteDirection, 0f);
            }*/

            texture = DDTextures.Round2.Value;
            float Sc = NPC.Dnpc().Times[2];
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), NPC.rotation, texture.Size() / 2, NPC.scale * (Sc % 1), (SpriteEffects)NPC.spriteDirection, 0f);
            Sc += 0.33f;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), NPC.rotation, texture.Size() / 2, NPC.scale  * (Sc % 1), (SpriteEffects)NPC.spriteDirection, 0f);
            Sc += 0.33F;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), NPC.rotation, texture.Size() / 2, NPC.scale  * (Sc % 1), (SpriteEffects)NPC.spriteDirection, 0f);
            
            texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(NPC.frame), Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);

            texture = DDTextures.Starlight3.Value;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 0), 0, texture.Size() / 2, NPC.scale*new Vector2(0.5F, 1) * 0.5f, (SpriteEffects)NPC.spriteDirection, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(0,0, 255, 0)*0.5F, 0, texture.Size() / 2, NPC.scale*new Vector2(0.5F, 1) * 0.5f, (SpriteEffects)NPC.spriteDirection, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(255, 191, 0, 0), MathHelper.PiOver2, texture.Size() / 2, NPC.scale*new Vector2(0.5F,1) * 0.5f, (SpriteEffects)NPC.spriteDirection, 0f);
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, null, new Color(0, 0, 255, 0)*0.5F, MathHelper.PiOver2, texture.Size() / 2, NPC.scale * new Vector2(0.5F, 1)* 0.5f, (SpriteEffects)NPC.spriteDirection, 0f);

            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            //光球
            DDHelper.BackAndForth(1, 1.3F, 0.03F, ref NPC.Dnpc().Times[4], ref NPC.Dnpc().Bool[4]);
            //翅膀
            //DDHelper.BackAndForth(-0.8F, 1.8F, 0.0125F, ref NPC.Dnpc().Times[3], ref NPC.Dnpc().Bool[3]);
            DDHelper.BackAndForth(-0.8F, 0.2F, 0.2F, ref W, ref WB);
            if (WB2)
            {
                DDHelper.BackAndForth(-0.4F, 1.7F, 0.3F, ref W2, ref WB2);
            }
            else
            {
                DDHelper.BackAndForth(-0.4F, 1.7F, 0.3F, ref W2, ref WB2);
            }
            DDHelper.BackAndForth(0.8F, 2.2F, 0.2F, ref W3, ref WB3);
            W10 = W9;
            W9 = W8;
            W8 = W7;
            W7 = W6;
            W6 = W5;
            W5 = W4;
            W4 = W2;
            NPC.Dnpc().Times[2] += 0.03f;
            if(NPC.Dnpc().Defaults)
            NPC.ai[2] = 60;
        }
    }
}