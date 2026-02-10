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
using DDmod.Helper;
using DDmod.Sync;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Boss.先祖咒魂;

namespace DDmod.Content.NPCs.Boss.先祖咒魂
{
    [AutoloadBossHead]
    public class 先祖咒魂 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 0,
                PortraitPositionXOverride = 0F,
                PortraitScale = 0.8f,
                Position = new Vector2(0F, 40),
                Rotation = 0F,
                Direction = 0,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            DDSystem.HBar(NPC.type, "先祖咒魂", new Vector2(-1146, 0));
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 30500;
            NPC.damage = 60;
            NPC.defense = 12;
            NPC.knockBackResist = 0;
            NPC.width = 80;
            NPC.height = 120;
            NPC.value = Item.buyPrice(0, 12, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            SoundStyle sound = SoundID.NPCHit30;
            sound.Pitch = -0.5F;
            NPC.HitSound = sound; 
            sound = SoundID.NPCDeath33;
            sound.Pitch = -0.5F;
            NPC.DeathSound = sound;
            NPC.netAlways = true;
            NPC.boss = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            if (!Main.dedServ) Music = 39;
            NPC.localAI[1] = 1;
            NPC.Dnpc().Properties.ShadowFire = true;
            NPC.Dnpc().Goblins = true;
            NPC.NPCHB().HealthBarFrame(new Vector2(4, 4), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0));
            NPC.Dnpc().Properties.BossLife = 1.25F;
            //NPC.NPCHB().MiniBoss = true;
            //NPC.Dnpc().Neutrality = true;
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
            SetEventFlagCleared(ref NPCDowned.先祖咒魂, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.诅咒先魂"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<先祖咒魂宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<先祖咒魂纪念章Item>(), 10));
            //面具
            npcLoot.NormalLoot(10, ModContent.ItemType<先祖咒魂面具>());

            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<先祖咒魂圣物>()));
            //大师宠物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Content.Items.Talisman.EnchantedVoodooDollItme>(), 4));

            int[] A = [ModContent.ItemType<诅咒双刃>(), ModContent.ItemType<诅咒弹药箱>(), ModContent.ItemType<苦罪囚链>()];
            npcLoot.NormalLoot(1, A);
        }
        public override bool CheckActive()
        {
            return true;
        }
        public override bool CheckDead()
        {
            return true;
        }
        public void DrawEffect()
        {

            if (NPC.Dnpc().Stage == 0)
            {
                DDWorld.SunColor = new Color(211, 35, 221, 200);
                DDWorld.SunLightScale = 0.75F;
                DDWorld.SunLight = 0.05F;
            }
            else
            {
                if (NPC.Dnpc().Stage == 1)
                {
                    SkyDowned.BackgroundColor = new Color(61, 2, 61, 255)* (NPC.ai[1]/120);
                }
                else
                {
                    SkyDowned.BackgroundColor = new Color(61, 2, 61, 255);

                }
                DDWorld.SunColor = new Color(211, 35, 221, 200);
                DDWorld.SunLightScale = 0.75F;
                DDWorld.SunLight = 0.05F;
            }
        }
        public void Effect()
        {
            int dust = NewDust(NPC.position + new Vector2(0, NPC.height - 20), NPC.width, 20, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(151, 35, 221, 0), Main.rand.NextFloat(0.3F, 2.8F));
            Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(3, 6));
            Main.dust[dust].customData = 2;
            Main.dust[dust].noGravity = true;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.target == 255 || player.dead || !player.active)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
            }
            Vector2 vector = player.Center - NPC.Center;
            NPC.spriteDirection = 0;
            if (vector.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            NPC.localAI[3]-=3;
            Effect();
            if (player.dead)
            {
                if (NPC.timeLeft > 5)
                {
                    NPC.timeLeft = 5;
                }
                NPC.spriteDirection = 0;
                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                NPC.SmoothVelocity(-vector.PerfectNormalize() * 8);
                NPC.rotation = NPC.velocity.X * 0.03F;
                return;
            }
            if (NPC.ai[3]<0)
            {
                NPC.dontTakeDamage = true;
                if (NPC.Dnpc().Stage != 4)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.Dnpc().Stage = 4;
                }
                else
                {
                    NPC.spriteDirection = 0;
                    if (NPC.velocity.X > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                    NPC.velocity *= 0.92F;
                    NPC.ai[0]++;
                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 30, false, 0.1F);
                    if (NPC.ai[0] == 10)
                    {
                        for (int A = 0; A < 5; A++)
                        {
                            NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(200).RotatedBy(MathHelper.TwoPi / 5 * A), ModContent.NPCType<诅咒之魂>(), 0, 0, 0, NPC.whoAmI+1);
                        }
                    }
                    if (NPC.ai[0] == 60)
                    {
                        NPC.Dnpc().vector[0] = NPC.Center;
                        SoundStyle sound = SoundID.NPCDeath10;
                        sound.Pitch = -1F;
                        PlaySound(sound);
                        for (int t = 0; t < 12; t++)
                        {
                            for (int r = 0; r < 4; r++)
                            {
                                Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(151, 35, 221, 0))];
                                dust.noGravity = true;
                                dust.scale = 0.01f;
                                dust.alpha = -1;
                                dust.velocity = Vector2.Zero;
                                dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                            }
                        }
                    }

                    if (NPC.ai[0]>120)
                    {
                        if (NPC.ai[0] < 540)
                        {
                            if (NPC.ai[1]++ % 60 == 0)
                            {
                                NPC.ai[2] = Main.rand.NextFloat(MathHelper.TwoPi);
                            }
                            NPC.velocity = (NPC.velocity * 20 + NPC.ai[2].ToRotationVector2() * 32) / 21;
                        }
                        Vector2 v = NPC.Dnpc().vector[0] - NPC.Center;
                        float A = (20 * (1 - (NPC.ai[1] % 60) / 60));
                        if (A < 3)
                        {
                            A =3;
                        }
                        if(v.Length()>1)
                        {
                            NPC.position -= -v.PerfectNormalize()*v.Length()/ A;
                        }
                        else
                        {
                            NPC.Center = NPC.Dnpc().vector[0];
                        }
                    }

                    if (NPC.ai[0]==660)
                    {
                        SoundStyle sound = SoundID.NPCDeath10;
                        sound.Pitch = -1F;
                        PlaySound(sound);
                        for (int t = 0; t < 12; t++)
                        {
                            for (int r = 0; r < 4; r++)
                            {
                                Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(151, 35, 221, 0))];
                                dust.noGravity = true;
                                dust.scale = 0.01f;
                                dust.alpha = -1;
                                dust.velocity = Vector2.Zero;
                                dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                            }
                        }
                        NPC.Kill(true);
                    }

                }
                NPC.rotation = NPC.velocity.X * 0.03F;
                return;
            }
            if (NPC.localAI[2]++ % 30 == 0&& !NPC.Dnpc().Bool[4])
            {
                NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(8, 12));
            }
            NPC.Dnpc().Bool[4] = false;
            //停止移动NPC.ai[0]=120;
            if (NPC.ai[0]++ > 180)
            {
                float S = vector.Length() - 300;
                S /= 10;
                if (S > 12)
                {
                    S = 12;
                }
                if (vector.Length() > 300)
                {
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * S) / 21;
                }
                else
                {
                    NPC.velocity *= 0.92F;
                }
            }
            else if (NPC.ai[0] == 1)
            {
                Main.LocalPlayer.Dplayer().PlayerShake(50, 12);
                SoundStyle sound = SoundID.Zombie105;
                sound.Pitch = -0.75F;
                PlaySound(sound);
                for (int t = 0; t < 12; t++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(151, 35, 221, 0))];
                        dust.noGravity = true;
                        dust.scale = 0.01f;
                        dust.alpha = -1;
                        dust.velocity = Vector2.Zero;
                        dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                    }
                }
            }
            else
            {
                if(NPC.localAI[1]==0)
                NPC.velocity *= 0.92F;
                NPC.localAI[1] = 0;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.life <= NPC.lifeMax / 2)
                {
                    NPC.Dnpc().Stage = 1;
                    Refresh(0);
                }
                //攻击部分
                if (NPC.ai[3] == 0)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 180)
                    {
                        Refresh(1);
                    }
                }
                else if (NPC.ai[3] == 1)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] % 30 == 0)
                    {
                        NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize() * 18, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 0, 0.6f);
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        Refresh(2);
                    }
                }
                else if (NPC.ai[3] == 2)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 18, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 2, 1f);
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(8, 12));

                        }
                    }

                    if (NPC.ai[1] >= 200 && NPC.ai[1] % 15 == 0)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 6);
                        for (int A = 0; A < 6; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 6 * A + W) * 18, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 0, 0.4f);
                        }
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        if (NPC.ai[2] >= 2)
                        {
                            Refresh(3);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else if (NPC.ai[3] == 3)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(400, 600), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        vector = player.Center - NPC.Center;
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 18, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 4, 0.5f);
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(8, 12));

                        }

                        NPC.velocity = vector.PerfectNormalize() * 24;
                    }
                    if (NPC.ai[1] >= 90)
                    {
                        NPC.spriteDirection = 0;
                        if (NPC.velocity.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                        NPC.localAI[1] = 1;
                        if (NPC.ai[1] % 5 == 0)
                            NPC.NewNPCProj(NPC.Center + vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.Next(400), Vector2.Zero, ModContent.ProjectileType<Boss暗影火>(), 30, 0, -1);
                    }
                    if (NPC.ai[1] > 150)
                    {
                        if (vector.Length() > 400)
                        {
                            NPC.velocity *= 0.96F;
                        }
                    }
                    if (NPC.ai[1] >= 200)
                    {
                        if (NPC.ai[2] >= 4)
                        {
                            Refresh(4);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else if (NPC.ai[3] == 4)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[1] = player.Center + new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        vector = player.Center - NPC.Center;
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 18, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 4, 0.5f);
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(8, 12));

                        }
                    }
                    if (NPC.ai[1] >= 90)
                    {
                        NPC.localAI[1] = 1;
                        if (NPC.ai[1] % 15 == 0)
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(14, 20));

                    }
                    if (NPC.ai[1] >= 150)
                    {
                        if (NPC.ai[2] >= 4)
                        {
                            Refresh(5);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else
                {
                    Refresh(1);
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[1]++;
                if(NPC.ai[1]==2)
                {
                    Main.LocalPlayer.Dplayer().PlayerShake(50, 12);
                    SoundStyle sound = SoundID.Zombie105;
                    sound.Pitch = -0.25F;
                    PlaySound(sound);
                    for (int t = 0; t < 12; t++)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(151, 35, 221, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                        }
                    }
                }
                if(NPC.ai[1]>=120)
                {
                    Refresh(0);
                    NPC.Dnpc().Stage = 2;
                }
                NPC.ai[0] = 100;
            }
            else
            {

                //攻击部分
                if (NPC.ai[3] == 0)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(400, 600), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        vector = player.Center - NPC.Center;

                        NPC.velocity = vector.PerfectNormalize() * 24;
                    }
                    if (NPC.ai[2]>0)
                    {
                        NPC.spriteDirection = 0;
                        if (NPC.velocity.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                        NPC.localAI[1] = 1;
                        if (NPC.ai[1] % 5 == 0)
                            NPC.NewNPCProj(NPC.Center + vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.Next(400), Vector2.Zero, ModContent.ProjectileType<Boss暗影火>(), 30, 0, -1);
                    }
                    if (NPC.ai[1] >= 90)
                    {
                        if (NPC.ai[2] >= 4)
                        {
                            if (NPC.ai[1] >= 180)
                                Refresh(1);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else if (NPC.ai[3] == 1)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(400, 600), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        NewNPCs(NPC.GetSource_FromAI(),NPC.Center,ModContent.NPCType<诅咒之魂>(),0);
                    }

                    if (NPC.ai[1] >= 300)
                    {
                        Refresh(2);
                    }
                }
                else if (NPC.ai[3] == 2)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 12, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 5, 1f);
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(8, 12));
                        }
                    }

                    if (NPC.ai[1] > 90 && NPC.ai[1] <= 165 && NPC.ai[1] % 15 == 0)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 6);
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 12, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 5, 0.4f);
                        }
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        if (NPC.ai[2] >= 2)
                        {
                            Refresh(3);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else if (NPC.ai[3] == 3)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(400, 600), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        NewNPCs(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<诅咒之魂>(), 0);
                    }

                    if (NPC.ai[1] >= 300)
                    {
                        Refresh(4);
                    }
                }
                else if (NPC.ai[3] == 4)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                    }
                    if (NPC.ai[1] == 90)
                    {
                        float W = Main.rand.NextFloat(MathHelper.TwoPi / 8);
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        NPC.Center = NPC.Dnpc().vector[1];
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), 30, 0, -1, NPC.whoAmI + 1, Main.rand.Next(22, 26),2.5F);
                        }
                    }
                    if (NPC.ai[1] >= 300)
                    {
                        if (NPC.ai[2] >= 2)
                        {
                            Refresh(5);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else if (NPC.ai[3] == 5)
                {
                    NPC.ai[0] = 120;
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 30)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(Main.rand.Next(400, 600), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] < 60)
                    {
                        NPC.Dnpc().vector[1] = player.Center + NPC.Dnpc().vector[0];
                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                    {
                        NPC.Dnpc().Bool[4] = true;
                        NewDustChange4(4, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 4, 8, true, 1F, 1.2F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        Vector2 v = NPC.Dnpc().vector[1] - NPC.Dnpc().vector[0];
                        for (int A = 1; A < 5; A++)
                        {
                            NewDustChange4(2, v + NPC.Dnpc().vector[0].RotatedBy(MathHelper.TwoPi / 5 * A) - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 3, 6, true, 0.8F, 1F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        }

                    }
                    if (NPC.ai[1] == 90)
                    {
                        NewDustChange4(100, NPC.Dnpc().vector[1] - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                        Vector2 v = NPC.Dnpc().vector[1] - NPC.Dnpc().vector[0];
                        float W = Main.rand.NextFloat(MathHelper.TwoPi /12);
                        for (int A = 1; A<5; A++)
                        {
                            for (int B = 0; B < 12; B++)
                            {
                                NPC.NewNPCProj((v + NPC.Dnpc().vector[0].RotatedBy(MathHelper.TwoPi / 5 * A)), vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 12 * B + W) * 12, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 7, 0.4f);
                            }
                            NewDustChange4(30, v + NPC.Dnpc().vector[0].RotatedBy(MathHelper.TwoPi/5*A) - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 10, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2) ;
                        }
                        W = Main.rand.NextFloat(MathHelper.TwoPi / 8);

                        NPC.Center = NPC.Dnpc().vector[1];
                        for (int A = 0; A < 8; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, vector.PerfectNormalize().RotatedBy(MathHelper.TwoPi / 8 * A + W) * 8, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 6, 0.65f);
                        }
                        vector = player.Center - NPC.Center;

                        NPC.velocity = vector.PerfectNormalize() * 24;
                    }
                    if (NPC.ai[1] >= 90)
                    {
                        NPC.spriteDirection = 0;
                        if (NPC.velocity.X > 0)
                        {
                            NPC.spriteDirection = 1;
                        }
                        NPC.localAI[1] = 1;
                    }
                    if (NPC.ai[1] >= 150)
                    {
                        if (NPC.ai[2] >= 4)
                        {
                            if (NPC.ai[1] >= 180)
                                Refresh(6);
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                }
                else
                {
                    Refresh(0);
                }
            }
            void Refresh(int AI)
            {
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = AI;
            }
            NPC.rotation = NPC.velocity.X * 0.03F;
        }
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (NPC.Dnpc().Stage == 4)
                {
                    for (int a = 0; a < 300; a++)
                    {
                        int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(151, 35, 221, 0), Main.rand.NextFloat(0.3F, 2F));
                        Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        Main.dust[A].customData = 1F;
                        Main.dust[A].noGravity = true;
                    }
                }
                else
                {
                    NPC.life =666;
                    NPC.ai[3] = -100;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos - new Vector2(100, 50), null, new Color(50, 12, 74, 255), 0, Vector2.Zero, new Vector2(200, 100), 0, 0f);
            }
            else
            {

                DrawEffect();
            }
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D G = Glow.Value;
            Vector2 vector = NPC.Size / 2;
            if(NPC.spriteDirection==-1)
            {
                NPC.spriteDirection = 0;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                {
                    if (NPC.ai[3] == 3)
                    {
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                    }
                }
            }
            if (NPC.Dnpc().Stage == 2)
            {
                if (NPC.ai[1] > 30 && NPC.ai[1] < 90)
                {
                    if (NPC.ai[3] == 0)
                    {
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                    }
                    if (NPC.ai[3] == 5)
                    {
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                        spriteBatch.Draw(DDTextures.Wire.Value, NPC.Dnpc().vector[1] - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60), (Main.player[NPC.target].Center - NPC.Dnpc().vector[1]).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);

                        Vector2 v = NPC.Dnpc().vector[1] - NPC.Dnpc().vector[0];
                        for (int A = 1; A < 5; A++)
                        {
                            spriteBatch.Draw(DDTextures.Wire.Value, v + NPC.Dnpc().vector[0].RotatedBy(MathHelper.TwoPi / 5 * A) - screenPos, null, new Color(151, 35, 221, 0) * (1 - (NPC.ai[1] - 30) / 60)*0.5f, (Main.player[NPC.target].Center - (v + NPC.Dnpc().vector[0].RotatedBy(MathHelper.TwoPi / 5 * A))).ToRotation(), new Vector2(0, 1), new Vector2((NPC.ai[1] - 30) / 60 * 5, (NPC.ai[1] - 30) / 60 * 20), 0, 0f);
                        }
                    }
                }
            }
            Rectangle rectangle = NPC.frame;
            rectangle.Width = Glow.Width();
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + vector;
                Color color = new Color(255, 255, 255, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length)*0.5f;
                spriteBatch.Draw(G, vector2, new Rectangle?(rectangle), color, 0, rectangle.Size() / 2+new Vector2(0,20), NPC.scale, 0, 0f);
            }
            rectangle = NPC.frame;
            spriteBatch.Draw(texture, NPC.position - screenPos + vector, new Rectangle?(rectangle), Color.White, NPC.rotation, NPC.frame.Size()/2 + new Vector2(0, 20), NPC.scale, (SpriteEffects)NPC.spriteDirection, 0f);
            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White*0.5F, 0, Vector2.Zero, NPC.Size/2, 0, 0f);

            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.Opacity = 1f;
            }
            NPC.frameCounter++;
            if (NPC.frameCounter > 4)
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