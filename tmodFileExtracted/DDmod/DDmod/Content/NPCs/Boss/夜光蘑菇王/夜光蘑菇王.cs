using DDmod.Content.Biome;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.夜光蘑菇王;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Tiles.农场;
using DDmod.Players;
using DDmod.Worlds;
using System.Linq;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace DDmod.Content.NPCs.Boss.夜光蘑菇王
{
    [AutoloadBossHead]
    public class 夜光蘑菇王 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DDSystem.HBar(NPC.type, "夜光蘑菇王", new Vector2(-10000, 2));
            Main.npcFrameCount[NPC.type] = 11;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (Main.netMode != 2)
            {
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
        }
        public override void SetDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 3;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 3080;
            NPC.damage = 20;
            NPC.defense = 8;
            NPC.boss = true;
            NPC.knockBackResist = 0f;
            NPC.width = 66;
            NPC.height = 66;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            if (!Main.dedServ) Music = DDSystem.Music(2,"夜光蘑菇王");
            NPC.netAlways = true;
            NPC.Dnpc().Deathrattle = true;
            NPC.scale = 1.3f;

            NPC.Dnpc().Properties.Fungi = true;
            NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (NPC.Dnpc().Deathrattle)
                {
                    NPC.life = 1;
                    NPC.dontTakeDamage = true;
                    NPC.localAI[0] = -1;
                }
            }
            else
            {
                for (int A = 0; A < 3; A++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 20, 0f, 0f, 0, default(Color), 1f);
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dust].scale = 0.1f;
                        Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
                    }
                }
            }
        }
        public override bool PreAI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            SoundStyle sound = SoundID.NPCDeath1;
            SoundStyle sound2 = SoundID.Roar;
            sound2.MaxInstances = 4;
            if(NPC.alpha>0)
            {
                NPC.alpha -= 5;
            }
            else
            {
                NPC.alpha = 0;
            }
            if (!(NPC.Dnpc().Bool[4] || (NPC.Dnpc().Stage == 1 && NPC.ai[1] == 3&& NPC.ai[0]>60)))
            {
                if (NPC.velocity == Vector2.Zero)
                {
                    if (vector.X < 0)
                    {
                        NPC.spriteDirection = 0;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                }
                else
                {
                    if (NPC.velocity.X < 0)
                    {
                        NPC.spriteDirection = 0;
                    }
                    else
                    {
                        NPC.spriteDirection = 1;
                    }
                }
            }
            // NPC和物块相撞
            Rectangle Position = new Rectangle((int)NPC.position.X / 16, (int)(NPC.position.Y+ NPC.height) / 16, NPC.width / 16, 1);
            bool TileCollision  = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 1)), NPC.width, 1);
            bool TileCollision2  = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y + NPC.height - 2)), NPC.width, 1);
            int j = Position.Y;

            
            int PO = (int)((player.position.Y + player.height)/16 - (NPC.position.Y + NPC.height)/16);
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
                if ((NPC.velocity.Y > 0 && PO <= 16) || NPC.velocity.X == 0)
                {
                    NPC.velocity.Y = 0;
                }
            }
            if (NPC.velocity.Y == 0)
            {
                if (NPC.velocity.X != 0)
                {
                    if (PO > 16 && Collision.CanHitLine(player.position + new Vector2(player.width / 2, player.height - 1), 1, 1, new Vector2(player.position.X + player.width / 2, NPC.position.Y + NPC.height - 1), 1, 1))
                    {
                        NPC.position.Y += NPC.Dnpc().Times[1];
                    }
                    if (PO <= 0)
                    {
                        if (TileCollision2)
                        {
                            NPC.position.Y -= NPC.Dnpc().Times[1] / 2;
                        }
                    }
                    NPC.Dnpc().Times[1] += NPC.gravity;
                    if (NPC.Dnpc().Times[1] > NPC.maxFallSpeed)
                    {
                        NPC.Dnpc().Times[1] = NPC.maxFallSpeed;
                    }
                }
                else
                {
                    NPC.Dnpc().Times[1] = 0;
                }
                if (NPC.localAI[0] >= 0)
                {
                    if ((PO < -300 && Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height) && !NPC.Dnpc().Bool[4])|| player.dead)
                    {
                        NPC.localAI[0]++;
                        if (NPC.localAI[0] > 180)
                        {
                            NPC.localAI[0] = 0;
                            NPC.Dnpc().Bool[4] = true;
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        NPC.localAI[0] = 0;
                    }
                }
            }
            if (NPC.localAI[0]<0)
            {
                NPC.localAI[0]--;
                NPC.velocity.X = 0;
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 30, false, 0.1F);
                if (NPC.localAI[0] <= -60)
                {
                    if (Main.netMode != NetmodeID.Server && NPC.localAI[0] == -60)
                    {
                        int GoreType = Mod.Find<ModGore>("夜光蘑菇王1").Type;

                        Gore.NewGore(NPC.GetSource_Death(), NPC.position+new Vector2(0,0), new Vector2(0, -40).RotatedBy(NPC.rotation), GoreType, NPC.scale);
                        int T = CombatText.NewText(NPC.getRect(), new Color(255, 32, 80), "XxX", true);
                        Main.combatText[T].lifeTime += 300;
                        SoundStyle s = SoundID.NPCDeath21;
                        s.Pitch = -1;
                        PlaySound(s, NPC.Center);
                    }
                    for (int a = 0; a < 2; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(NPC.width / 2, 16), NPC.width, 4, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-19, 19), -Main.rand.NextFloat(0.2F, 9), newColor: new Color(74, 189, 226,0), Scale: 1F);
                        Main.dust[A].velocity = new Vector2((Main.dust[A].position.X - NPC.Center.X) / 20, -Main.rand.NextFloat(1.5F, 4F));
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = Main.dust[A].DustAI(6) + 0.1F;
                    }
                    for (int a = 0; a < 5; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(NPC.width / 2, 16), NPC.width, 4, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-19, 19), -Main.rand.NextFloat(0.2F, 27), newColor: new Color(74, 189, 226, 0), Scale: 2F);
                        Main.dust[A].velocity = new Vector2((Main.dust[A].position.X - NPC.Center.X) / 50, -Main.rand.NextFloat(0.2f, 2));
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = Main.dust[A].DustAI(4);
                    }
                    if (Main.netMode != 1 && NPC.localAI[0] % 5 == 0)
                    {
                        SoundStyle s2 = SoundID.NPCHit5;
                        s2.MaxInstances = 10;
                        s2.Pitch = -0.5f;
                        PlaySound(s2, NPC.Center);
                    }
                    if (Main.netMode != 1 && NPC.localAI[0] % 20 == 0)
                    {
                        NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.NextFloat(NPC.width), NPC.height / 2 - 16), new Vector2(0, -1), ModContent.ProjectileType<Boss蘑菇弹>(), 20, 1, -1, 2);

                    }
                    if (NPC.localAI[0] <= -300)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            int GoreType = Mod.Find<ModGore>("夜光蘑菇王2").Type;

                            Gore.NewGore(NPC.GetSource_Death(), NPC.position+new Vector2(0,NPC.height/2), Vector2.Zero, GoreType, NPC.scale);
                        }
                        SoundStyle s = SoundID.NPCDeath21;
                        s.Pitch = -1;
                        PlaySound(s, NPC.Center);
                        NPC.Kill();
                    }
                }
                NPC.netUpdate = true;
                return false;
            }
            if (NPC.Dnpc().Stage == 1 && NPC.ai[0] >= -30&&NPC.ai[1]>=0)
            {
                if (NPC.Dnpc().Times[2] < 14)
                {
                    NPC.Dnpc().Times[2] += 0.1F;
                }
                else
                {
                    NPC.Dnpc().Times[2] = 14;
                }
                if (NPC.Dnpc().Bool[3])
                {
                    DDHelper.BackAndForth(-8, 4, 2F, ref NPC.Dnpc().Times[3], ref NPC.Dnpc().Bool[3]);
                    float I = 0;
                    for (int a = 0; a < NPC.Dnpc().Times[2]; a++)
                    {
                        if (a > 3)
                        {
                            I += 0.2F * a;
                            float l = 7 * a;
                            int A = NewDust(NPC.Center + new Vector2(l, NPC.Dnpc().Times[3] * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: (1 - a / 20f) + 1F);
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = -6;
                            GlobalDust.DustNPCOwner[A] = NPC.whoAmI;
                            GlobalDust.DustPreTile[A] = true;

                            l = 7 * a;
                            int A2 = NewDust(NPC.Center + new Vector2(-l, NPC.Dnpc().Times[3] * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226,0), Scale: (1 - a / 20f) + 1F);
                            Main.dust[A2].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                            Main.dust[A2].noGravity = true;
                            Main.dust[A2].customData = -6;
                            GlobalDust.DustNPCOwner[A2] = NPC.whoAmI;
                            GlobalDust.DustPreTile[A2] = true;
                        }
                    }
                }
                else
                {
                    DDHelper.BackAndForth(-8, 8, 0.4f, ref NPC.Dnpc().Times[3], ref NPC.Dnpc().Bool[3]);
                    float I = 0;
                    for (int a = 0; a < NPC.Dnpc().Times[2]; a++)
                    {
                        if (a > 3)
                        {
                            I += 0.2F * a;
                            float l = 7 * a;
                            int A = NewDust(NPC.Center + new Vector2(l, NPC.Dnpc().Times[3] * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: (1 - a / 20f)  + 1F);
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = -10;
                            GlobalDust.DustNPCOwner[A] = NPC.whoAmI;
                            GlobalDust.DustPreTile[A] = true;

                            l = 7 * a;
                            int A2 = NewDust(NPC.Center + new Vector2(-l, NPC.Dnpc().Times[3] * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: (1 - a / 20f) + 1F);
                            Main.dust[A2].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                            Main.dust[A2].noGravity = true;
                            Main.dust[A2].customData = -10;
                            GlobalDust.DustNPCOwner[A2] = NPC.whoAmI;
                            GlobalDust.DustPreTile[A2] = true;
                        }
                    }
                }
            }
            else
            {
                NPC.Dnpc().Times[2] = 0;
            }
            if (NPC.Dnpc().Stage == 0 && NPC.life <= NPC.lifeMax / 2)
            {
                NPC.Dnpc().Stage = 1;
                NPC.ai[1] = 0;
                NPC.ai[0] = -210;
                NPC.velocity.X = 0;
            }
            if (NPC.Dnpc().Stage == 1 && NPC.ai[0] < 0)
            {
                NPC.dontTakeDamage = true;
                if (NPC.velocity.Y == 0)
                {
                    NPC.ai[0]++;
                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 5, false, 0.1F);
                    if (NPC.ai[0] == -50)
                    {
                        for (int a = 0; a < 160; a++)
                        {
                            int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), newColor: new Color(74, 189, 226, 0), Scale: 1.2F);
                            Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-10, 10), -Main.rand.NextFloat(2, 12));
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = Main.dust[A].DustAI(5);
                        }
                        sound2.Pitch = -1;
                        PlaySound(sound2, NPC.position);
                        float PL = 40 - ((Main.LocalPlayer.Center - NPC.Center).Length() / 16);
                        if (PL < 0)
                        {
                            PL = 0;
                        }
                        NPC.life += 100;
                        if (NPC.life > NPC.lifeMax)
                        {
                            NPC.life = NPC.lifeMax;
                        }
                        int R = 20;
                        for (int a = 0; a < R; a++)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20).RotatedBy(MathHelper.TwoPi / R * a), new Vector2(0, 1).RotatedBy(MathHelper.TwoPi / R * a), ModContent.ProjectileType<Boss蘑菇弹>(), 20, 1, -1, 1);
                        }
                        CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 100, true);
                        Main.LocalPlayer.Dplayer().PlayerShake(50, PL);
                    }
                    else if (NPC.ai[0] > -160 && NPC.ai[0] < -50)
                    {
                        for (int a = 0; a < 5; a++)
                        {
                            int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                            Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = 7+ Main.dust[A].DustAI(3);
                        }
                        if (NPC.ai[3] % 5 == 0)
                        {
                            NPC.life += 3;
                            if (NPC.life > NPC.lifeMax)
                            {
                                NPC.life = NPC.lifeMax;
                            }
                            CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 3, false, true);
                        }
                    }
                }
                return false;
            }
            if (NPC.Dnpc().Bool[4])
            {
                NPC.ai[3]++;
                NPC.velocity.X = 0;
                NPC.netUpdate = true;
                if (NPC.ai[3] % 20 == 0)
                {
                    NPC.life += 5;
                    if(NPC.life> NPC.lifeMax)
                    {
                        NPC.life = NPC.lifeMax;
                    }
                    CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 5);
                    NPC.dontTakeDamage = true;
                }
                if (NPC.ai[3] % 5 == 0)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                        Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 7 + Main.dust[A].DustAI(3);
                    }
                }
                if (player.dead && NPC.ai[3] > 120)
                {
                    NPC.alpha += 10;
                }
                else if (NPC.ai[3] > 1200)
                {
                    NPC.alpha += 10;
                }
                if(NPC.alpha>255)
                {
                    NPC.active = false;
                }
                if (NPC.ai[3] % 300 == 0)
                {
                    if (PO >= -300 && !player.dead)
                    {
                        NPC.Dnpc().Bool[4] = false;
                        NPC.ai[3] = 0;
                        NPC.netUpdate = true;
                    }
                }
                return false;
            }
            
            NPC.dontTakeDamage = false;
            if (NPC.Dnpc().Stage == 0)
            {
                Lighting.AddLight(NPC.Center, 34 * 0.005f, 52 * 0.005f, 226 * 0.005f);
                //计时器
                if (NPC.ai[1] == 0)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 120)
                        {
                            NPC.ai[1] = 1;
                            NPC.ai[0] = 0;
                        }
                    }
                }
                else if (NPC.ai[1] == 1)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 60)
                        {
                            float sp = Math.Abs(vector.X);
                            if (sp > 12)
                            {
                                sp = 12;
                            }
                            NPC.velocity.X = vector.PerfectNormalize().X * sp;
                            NPC.velocity.Y = -14;
                            NPC.ai[1] = 2;
                            NPC.ai[0] = 0;
                            sound2.Pitch = 0;
                            PlaySound(sound2, NPC.position);
                        }
                    }
                }
                else if (NPC.ai[1] == 2)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                        if (!NPC.Dnpc().Bool[0])
                        {
                            NPC.Dnpc().Bool[0] = true;
                            for (int a = 0; a < 60; a++)
                            {
                                int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, 20, Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), Scale: 1.8F);
                                Main.dust[A].noGravity = true;
                            }
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 20; a++)
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(NPC.width), NPC.height - 2), new Vector2(Main.rand.NextFloat(-8, 8), -Main.rand.NextFloat(8, 12)), ModContent.ProjectileType<Boss夜光蘑菇>(), 7, 1);
                                    Main.dust[A].noGravity = true;
                                }
                            }
                            sound.Pitch = -1;
                            sound.Volume = 1;
                            PlaySound(sound, NPC.position);
                        }
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 120)
                        {
                            NPC.ai[1] = 3;
                            NPC.ai[0] = 0;
                            NPC.Dnpc().Bool[0] = false;
                        }
                    }
                }
                else if (NPC.ai[1] == 3)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] == 120)
                    {
                        NPC.velocity.Y = -4;
                        if (vector.X < 0)
                        {
                            NPC.velocity.X = -13;
                        }
                        else
                        {
                            NPC.velocity.X = 13;
                        }
                        sound2.Pitch = 0F;
                        PlaySound(sound2, NPC.position);
                    }
                    if (NPC.ai[0] > 120)
                    {
                        if (NPC.velocity.X == 0)
                        {
                            NPC.ai[1] = 4;
                            NPC.ai[0] = 0;
                        }
                        if (NPC.velocity.Y == 0)
                        {
                            for (int a = 0; a < 6; a++)
                            {
                                int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, 20, Scale: 1.8F);
                                Main.dust[A].noGravity = true;
                            }
                            if (NPC.ai[0] % 4 == 0)
                            {
                                sound = SoundID.Run;
                                sound.Volume = 0.4F;
                                PlaySound(sound, NPC.position);
                            }
                        }
                    }
                    if (NPC.ai[0] > 120 && NPC.ai[0] % 30 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            for (int a = -1; a <= 1; a++)
                            {
                                int A = NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(NPC.width), NPC.height - 2), new Vector2(0, -1), ModContent.ProjectileType<Boss蘑菇弹>(), 7, 1);
                                
                            }
                        }
                    }
                    if (NPC.ai[0] > 180)
                    {
                        if (NPC.velocity.X > 0)
                        {
                            if (vector.X < -800)
                            {
                                NPC.ai[1] = 4;
                                NPC.ai[0] = 0;
                            }
                        }
                        else
                        {
                            if (vector.X > 800)
                            {
                                NPC.ai[1] = 4;
                                NPC.ai[0] = 0;
                            }
                        }
                    }
                    if (NPC.ai[0] > 300)
                    {
                        NPC.ai[1] = 4;
                        NPC.ai[0] = 0;
                    }
                }
                else if (NPC.ai[1] == 4)
                {
                    if (vector.X < 0)
                    {
                        NPC.velocity.X -= 0.1F;
                        if (NPC.velocity.X <= -5)
                        {
                            NPC.velocity.X = -5;

                        }
                    }
                    else
                    {
                        NPC.velocity.X += 0.1F;
                        if (NPC.velocity.X >= 5)
                        {
                            NPC.velocity.X = 5;

                        }
                    }
                    if (NPC.ai[0] % 8 == 0)
                    {
                    }
                    if (Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height)||vector.Length()<300)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0] = 0;
                    }
                }
            }
            else
            if (NPC.Dnpc().Stage == 1)
            {

                Lighting.AddLight(NPC.Center, 34 * 0.01f, 52 * 0.01f, 226 * 0.01f);
                //计时器
                if (NPC.ai[1] == -1)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] % 10 == 0)
                        {
                            int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                            Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = 7 + Main.dust[A].DustAI(3);
                        }
                        if (NPC.ai[0] > 240)
                        {
                            int T = CombatText.NewText(NPC.getRect(), new Color(122, 132, 226), ".....");
                            NPC.ai[1] = 0;
                            NPC.ai[0] = 0;
                        }
                    }
                }
                else
                if (NPC.ai[1] == 0)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 120)
                        {
                            NPC.ai[1] = 1;
                            NPC.ai[0] = 0;
                        }
                    }
                }
                else if (NPC.ai[1] == 1)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                        if (!NPC.Dnpc().Bool[0])
                        {
                            NPC.Dnpc().Bool[0] = true;
                            for (int a = 0; a < 20; a++)
                            {
                                int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, 20, Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), Scale: 1.8F);
                                Main.dust[A].noGravity = true;
                            }
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 4; a++)
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(NPC.width), NPC.height - 2), new Vector2(Main.rand.NextFloat(-3, 3), -Main.rand.NextFloat(6, 8)), ModContent.ProjectileType<Boss夜光蘑菇>(), 12, 1, -1, 1);
                                    Main.dust[A].noGravity = true;
                                }
                            }
                            sound.Pitch = -1;
                            sound.Volume = 0.4F;
                            PlaySound(sound, NPC.position);
                        }
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 60)
                        {
                            float sp = Math.Abs(vector.X);
                            if (sp > 12)
                            {
                                sp = 12;
                            }
                            NPC.velocity.X = vector.PerfectNormalize().X * sp;
                            if (NPC.ai[2] == 4)
                            {
                                NPC.velocity.Y = -16;
                            }
                            else
                            {
                                NPC.velocity.Y = -6;
                            }
                            sound2.Pitch = 0.1F;
                            sound2.Volume = 0.3F;
                            PlaySound(sound2, NPC.position);
                            NPC.ai[2]++;
                            NPC.ai[1] = 1;
                            NPC.ai[0] = 30;
                            NPC.Dnpc().Bool[0] = false;
                        }
                        if (NPC.ai[2] > 4)
                        {
                            for (int a = 0; a < 60; a++)
                            {
                                int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, 20, Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), Scale: 1.8F);
                                Main.dust[A].noGravity = true;
                            }
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 15; a++)
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(NPC.width), NPC.height - 2), new Vector2(Main.rand.NextFloat(-8, 8), -Main.rand.NextFloat(8, 14)), ModContent.ProjectileType<Boss夜光蘑菇>(), 12, 1, -1, 1);
                                    Main.dust[A].noGravity = true;
                                }
                            }
                            sound2.Pitch = 0.4F;
                            sound2.Volume = 1;
                            PlaySound(sound2, NPC.position);
                            NPC.ai[2] = 0;
                            NPC.ai[1] = 2;
                            NPC.ai[0] = 0;
                            NPC.Dnpc().Bool[0] = false;
                        }
                    }
                }
                else if (NPC.ai[1] == 2)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        if (!NPC.Dnpc().Bool[0])
                        {
                            NPC.Dnpc().Bool[0] = true;
                            for (int a = 0; a < 60; a++)
                            {
                                int A = NewDust(NPC.position + new Vector2(0, NPC.height - 2), NPC.width, 2, 20, Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), Scale: 1.8F);
                                Main.dust[A].noGravity = true;
                            }
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 20; a++)
                                {
                                    int A = NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(Main.rand.Next(NPC.width), NPC.height - 2), new Vector2(Main.rand.NextFloat(-8, 8), -Main.rand.NextFloat(8, 16)), ModContent.ProjectileType<Boss夜光蘑菇>(), 12, 1, -1, 1);
                                    Main.dust[A].noGravity = true;
                                }
                            }
                            sound.Pitch = -1F;
                            sound.Volume = 1;
                            PlaySound(sound, NPC.position);
                        }
                        NPC.velocity.X = 0;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 120)
                        {
                            NPC.Dnpc().Bool[0] = false;
                            NPC.ai[1] = 3;
                            NPC.ai[0] = 0;
                        }
                    }
                }
                else if (NPC.ai[1] == 3)
                {
                    if (!Collision.CanHitLine(NPC.Center, 1, 1, player.position, player.width, player.height) && NPC.ai[0] < 70)
                    {
                        if (vector.X < 0)
                        {
                            NPC.velocity.X -= 0.1F;
                            if (NPC.velocity.X <= -5)
                            {
                                NPC.velocity.X = -5;

                            }
                        }
                        else
                        {
                            NPC.velocity.X += 0.1F;
                            if (NPC.velocity.X >= 5)
                            {
                                NPC.velocity.X = 5;

                            }
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y == 0)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        NPC.velocity.X = 0;
                        if (NPC.ai[0] == 70)
                        {
                            if (Main.netMode != 1)
                            {
                                for (int a = 0; a < 8; a++)
                                {
                                    NPC N = NPC.NewNPCDirect(NPC.GetSource_FromAI(), new((int)NPC.Center.X, (int)NPC.Center.Y), ModContent.NPCType<史莱菇>());
                                    N.velocity = new Vector2(Main.rand.NextFloat(-8, 8), -Main.rand.NextFloat(3, 7));
                                }
                            }
                        }
                        if (NPC.ai[0] > 70 && NPC.ai[0] % 20 == 0)
                        {
                            NPC.life += 6;
                            if (NPC.life > NPC.lifeMax)
                            {
                                NPC.life = NPC.lifeMax;
                            }
                            CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 6);
                        }
                        if (NPC.ai[0] > 70 && NPC.ai[0] % 5 == 0)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                                Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                                Main.dust[A].noGravity = true;
                                Main.dust[A].customData = 7 + Main.dust[A].DustAI(3);
                            }
                        }
                        if (NPC.ai[0] > 120 && !NPC.AnyNPCs(ModContent.NPCType<史莱菇>()))
                        {
                            NPC.ai[1] = 4;
                            NPC.ai[0] = 0;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else if (NPC.ai[1] == 4)
                {
                    if (!Collision.CanHitLine(NPC.Center, 1, 1, player.position, player.width, player.height) && NPC.ai[0] < 60)
                    {
                        if (vector.X < 0)
                        {
                            NPC.velocity.X -= 0.1F;
                            if (NPC.velocity.X <= -5)
                            {
                                NPC.velocity.X = -5;

                            }
                        }
                        else
                        {
                            NPC.velocity.X += 0.1F;
                            if (NPC.velocity.X >= 5)
                            {
                                NPC.velocity.X = 5;

                            }
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y == 0)
                        {
                            NPC.velocity.X = 0;
                        }
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.ai[0]++;
                        NPC.velocity.X = 0;
                        if (NPC.ai[0] > 60 && NPC.ai[0] % 5 == 0)
                        {
                            for (int a = 0; a < 2; a++)
                            {
                                int A = NewDust(NPC.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 3.6F);
                                Main.dust[A].velocity = (NPC.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                                Main.dust[A].noGravity = true;
                                Main.dust[A].customData = 7 + Main.dust[A].DustAI(3);
                            }
                            if (NPC.ai[0] % 5 == 0)
                            {
                                NPC.life += 1;
                                if (NPC.life > NPC.lifeMax)
                                {
                                    NPC.life = NPC.lifeMax;
                                }
                                CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 1, false, true);
                            }
                        }
                        if (NPC.ai[0] > 60 && NPC.ai[0] % 20 == 0)
                        {
                            if (Main.netMode != 1)
                            {
                                int R = 3;
                                for (int a = 0; a < R; a++)
                                {
                                    float Rand = Main.rand.NextFloat(MathHelper.TwoPi / R);
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20).RotatedBy(MathHelper.TwoPi / R * a + Rand), new Vector2(0, 1).RotatedBy(MathHelper.TwoPi / R * a + Rand), ModContent.ProjectileType<Boss蘑菇弹>(), 10, 1, -1, 1);

                                }
                            }
                        }
                        if (NPC.ai[0] > 420)
                        {
                            if (Main.netMode != 2)
                            {
                                for (int a = 0; a < 60; a++)
                                {
                                    int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 9), newColor: new Color(74, 189, 226, 0), Scale: 2.8F);
                                    Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-5, 5), -Main.rand.NextFloat(2, 5));
                                    Main.dust[A].noGravity = true;
                                }
                                int T = CombatText.NewText(NPC.getRect(), new Color(255, 32, 80), "X w X", true);
                                Main.combatText[T].lifeTime += 300;
                                sound2.Pitch = -1;
                                PlaySound(sound2, NPC.position);
                            }
                            float PL = 20 - ((Main.LocalPlayer.Center - NPC.Center).Length() / 16);
                            if (PL < 0)
                            {
                                PL = 0;
                            }
                            NPC.life += 30;
                            if (NPC.life > NPC.lifeMax)
                            {
                                NPC.life = NPC.lifeMax;
                            }
                            CombatText.NewText(NPC.getRect(), new Color(32, 255, 80), 30, true);
                            if (Main.netMode != 1)
                            {
                                int R = 10;
                                for (int a = 0; a < R; a++)
                                {
                                    float Rand = Main.rand.NextFloat(MathHelper.TwoPi / R);
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 20).RotatedBy(MathHelper.TwoPi / R * a + Rand), new Vector2(0, 1).RotatedBy(MathHelper.TwoPi / R * a + Rand), ModContent.ProjectileType<Boss蘑菇弹>(), 10, 1, -1, 1);
                                }
                            }
                            Main.LocalPlayer.Dplayer().PlayerShake(25, PL);

                            NPC.ai[1] = -1;
                            NPC.ai[0] = 0;
                            NPC.netUpdate = true;
                        }
                    }
                }

            }
            return base.PreAI();
        }
        int F = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 70;
            if (NPC.localAI[0] <= -60)
            {
                NPC.frame.Y = frameHeight * 10;
                NPC.frame.X = 0;
                return;
            }
            if (NPC.Dnpc().Bool[4]|| (NPC.Dnpc().Stage == 1&&(NPC.ai[1] == 3|| NPC.ai[1] == 4|| NPC.ai[1]==-1))|| (NPC.Dnpc().Stage == 1 && NPC.ai[0] < -50))
            {
                if(NPC.ai[0] <= -160)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = 0;
                }
                if (NPC.ai[0] < -50 || NPC.ai[0] > 60|| NPC.Dnpc().Bool[4] || NPC.ai[1] == -1)
                {
                    NPC.frame.X = 70;
                    if (F != 20)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = 0;
                        F = 20;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 2)
                    {
                        NPC.frame.Y = frameHeight * 2;
                    }
                }
                else if (NPC.velocity.X == 0)
                {
                    if (F != 0)
                    {
                        NPC.frameCounter = 0;
                        F = 0;
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y > frameHeight * 4)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    NPC.frameCounter += Math.Abs(NPC.velocity.X);
                    if (F != 1)
                    {
                        NPC.frameCounter = 0;
                        F = 1;
                        NPC.frame.Y = frameHeight * 5;
                    }
                    float SP = 14;
                    if (NPC.frameCounter > SP)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter -= SP;
                    }
                    if (NPC.frame.Y > frameHeight * 9)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
                return;
            }
            if (NPC.velocity.Y == 0)
            {
                NPC.frame.X = 70;
                if ((NPC.ai[1] == 3 && NPC.ai[0] > 120))
                {

                    NPC.frame.X = 0;
                    NPC.frame.Y = frameHeight * 9;
                    return;
                }
                if (F==3|| F==4|| F==5)
                {
                    if (F != 5)
                    {
                        NPC.frameCounter = 0;
                        F = 5;
                        NPC.frame.Y = frameHeight * 8;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 6)
                    {
                        if (NPC.frame.Y > frameHeight * 9)
                        {
                            NPC.frameCounter = 0;
                            F = 0;
                            NPC.frame.Y = 0;
                        }
                        else
                        {
                            NPC.frame.Y += frameHeight;
                            NPC.frameCounter = 0;
                        }
                    }
                    return;
                }
                if (NPC.ai[1] == 1|| (NPC.ai[1] == 3&& NPC.ai[0]<120))
                {
                    if (F != 2)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = 0;
                        F = 2;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 2)
                    {
                        NPC.frame.Y = frameHeight * 2;
                    }
                    return;
                }
                NPC.frame.X = 0;
                //1:0-3原地
                //1:4-9行走
                if (NPC.velocity.X == 0)
                {
                    if (F != 0)
                    {
                        NPC.frameCounter = 0;
                        F = 0;
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y > frameHeight * 4)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    NPC.frameCounter+=Math.Abs(NPC.velocity.X);
                    if (F != 1)
                    {
                        NPC.frameCounter = 0;
                        F = 1;
                        NPC.frame.Y = frameHeight * 5;
                    }
                    float SP = 14;
                    if (NPC.frameCounter > SP)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter -= SP;
                    }
                    if (NPC.frame.Y > frameHeight * 9)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
            }
            else
            {
                //2:0-2预备跳
                //2:3-5上升
                //2:6-7下落
                //2:8-10落地
                NPC.frame.X = 70;
                if (NPC.velocity.Y < 0)
                {
                    if (F != 3)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight *3;
                        F = 3;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
                else
                {
                    if (F != 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y = frameHeight * 6;
                        F = 4;
                    }
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 7)
                    {
                        NPC.frame.Y = frameHeight * 7;
                    }
                }
            }
        }
        public override void ModifyIncomingHit(ref HitModifiers modifiers)
        {
            if (NPC.Dnpc().Stage == 1)
            {
                if ((NPC.ai[1] == 3 || NPC.ai[1] == 4) && NPC.ai[0] > 70)
                {
                    modifiers.SourceDamage *= 0.4F;
                }
                else if(NPC.ai[0] >= 0&& NPC.ai[1] >= 0)
                {
                    modifiers.SourceDamage *= 0.8F;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.夜光蘑菇王")),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,

            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<夜光蘑菇王宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<夜光蘑菇王纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<夜光蘑菇王面具>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<夜光蘑菇王圣物>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<魔力菇>(), 4));
            
            //特别引用,普通模式
            //普通模式
            int[] A =
            [
                ModContent.ItemType<真菌双剑>(),
                ModContent.ItemType<夜光蘑菇炸弹>(),
                ModContent.ItemType<夜光蘑菇杖>(),
                ModContent.ItemType<蘑菇崽召唤杖>(),
            ];
            npcLoot.NormalLoot(1, ModContent.ItemType<夜光蘑菇炸弹>(), 80, 200);
            //特别引用,普通模式
            npcLoot.NormalLoot(1, A);
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.夜光蘑菇王, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            drawColor *= (1F - NPC.alpha / 255F);
            Color color = new Color(34, 172, 226, 0)*(1F-NPC.alpha/255F);
            Rectangle frame;
            frame = NPC.frame;
            if (NPC.ai[0] > -160 && NPC.ai[0] < -50)
            {
                float r = 0.5F - (Math.Abs(NPC.ai[0] + 50) / 110f) / 4;
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, drawColor, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, color * r, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] + NPC.Size / 2 - screenPos + new Vector2(0, NPC.height / 2 + 6);
                    Color oldcolor = color * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(texture, vector2, frame, oldcolor * r, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0f);
                }
            }
            if (NPC.Dnpc().Stage == 0 || NPC.ai[0] <= -160)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] + NPC.Size / 2 - screenPos + new Vector2(0, NPC.height / 2 + 6);
                    Color oldcolor = color * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(texture, vector2, frame, oldcolor, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0f);
                }
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, drawColor, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);

            }
            drawColor = Color.White * (1F - NPC.alpha / 255F);
            if (NPC.Dnpc().Stage ==1&& NPC.ai[0] >= -50)
            {
                float B = 0.16F;
                if ((NPC.ai[1] == 3 || NPC.ai[1] == 4) && NPC.ai[0] > 70)
                {
                    B = 0.31F;
                }
                if(NPC.ai[1]==-1)
                {
                    B = 0;
                }
                for (float i = 0.05f; i <= B; i += 0.05f)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, frame, color, NPC.rotation, new Vector2(frame.Width / 2, frame.Height / 2), NPC.scale + i, sprite, 0);
                }
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, drawColor, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, color, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);

                if (NPC.ai[1] >= 0)
                {
                    for (int i = 0; i < NPC.oldPos.Length; i++)
                    {
                        Vector2 vector2 = NPC.oldPos[i] + NPC.Size / 2 - screenPos + new Vector2(0, NPC.height / 2 + 6);
                        Color oldcolor = color * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                        spriteBatch.Draw(texture, vector2, frame, oldcolor, NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0f);
                    }
                }
            }
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos + new Vector2(0, NPC.height / 2 + 6), frame, new Color(255, 255, 255, 0) * (1F - NPC.alpha / 255F), NPC.rotation, new Vector2(frame.Width / 2, frame.Height), NPC.scale, sprite, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
    }
}