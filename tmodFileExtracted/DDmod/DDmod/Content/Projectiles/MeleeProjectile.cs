using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.ball;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader.IO;
using static Terraria.Graphics.FinalFractalHelper;

namespace DDmod.Content.Projectiles
{
    public class MeleeProjectile : GlobalProjectile
    {
        //小冰弹贴图
        public static Asset<Texture2D> XiaoiceBombs;
        public static Asset<Texture2D> iceBombs;
        public override bool InstancePerEntity => true;
        /// <summary>启用剑碰撞箱</summary>
        public bool SwordHitbox;
        /// <summary>攻击次数</summary>
        public int Attacks;
        /// <summary>短剑冲刺距离</summary>
        public int DaggerDashDistance;
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        /// <summary>距离</summary>
        public float oldVels2;
        /// <summary>起点位置</summary>
        public float oldVels3;
        /// <summary>旧速度</summary>
        public int DelayedKill;
        /// <summary>旧玩家位置</summary>
        public Vector2 oldPlayer;
        /// <summary>近战清除无敌帧判定</summary>
        public bool ClearInvincibility;
        /// <summary>相反的方向挥刀</summary>
        public bool Anti;
        /// <summary>强制攻击</summary>
        public bool TrueDamage;
        /// <summary>短剑额外长度</summary>
        public int ExtraLength;
        /// <summary>武器使用时间</summary>
        public float AttackSpeed;
        public override void Load()
        {
            XiaoiceBombs = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/冰弹");
            iceBombs = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/冰弹2");
        }
        public override void SetDefaults(Projectile projectile)
        {
            //短剑
            if (projectile.aiStyle == 161)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = -1;
                SwordHitbox = true;
                if (projectile.ModProjectile == null)
                {
                    SwordHitbox = true;
                    projectile.height = 34;
                    projectile.extraUpdates = 2;
                }
                if(ExtraLength==0)
                {
                    ExtraLength = projectile.height;
                }
            }
                //血叉
            if (projectile.type == 153)
            {
                NPCHit(projectile, 60);
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
            }
            //星怒
            if (projectile.type == 9)
            {
                NPCHit(projectile, -1);
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            }
            //星怒
            if (projectile.type == 92)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            }
            //星怒
            if (projectile.type == 91)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            }
            //血脉球
            if (projectile.type == 543)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] =4;
            }
            //风暴长毛
            if (projectile.type == 730)
            {
                NPCHit(projectile, 30);
            }
            if (projectile.type == 228)
            {
                NPCHit(projectile, 30);
            }
                if (projectile.type == 542)
            {
                NPCHit(projectile, 30);
            }
            if (projectile.type == 156)
            {
                NPCHit(projectile, 30);
            }
            //死神镰刀
            if (projectile.type == 274)
            {
                NPCHit(projectile, 12);
            }
            //蘑菇
            if (projectile.type == 131)
            {
                NPCHit(projectile, 40);
                projectile.penetrate = -1;
            }
            //回旋镖
            if (projectile.type == 6 ||projectile.type == 19||projectile.type == 684 || projectile.type == 113 || projectile.type == 52 || projectile.type == 867 || projectile.type == 1000)
            {
                NPCHit(projectile, 20);
            }
            if (projectile.type == 106)
            {
                NPCHit(projectile, 20);
            }
            //强大球珠
            if (projectile.type == 229)
            {
                NPCHit(projectile, 120);
            }
        }
        public void NPCHit(Projectile projectile, int Hit)
        {
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = Hit;
            projectile.usesIDStaticNPCImmunity = false;
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Anti);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            Anti = binaryReader.ReadBoolean();
        }
        int Ti;
        int CC;
        public override bool PreAI(Projectile projectile)
        {
            Ti++;
            if (oldVels != null && oldVels.Length > 0)
            {
                if (projectile.extraUpdates == 0 || Ti % 2 == 0)
                {
                    for (int i = oldVels.Length - 1; i > 0; i--)
                    {
                        
                        if (projectile.localAI[1] <= 0|| i< projectile.extraUpdates)
                        {
                            oldVels[i] = oldVels[i - 1];
                        }
                        else
                        {
                            oldVels[i] = oldVels[i - 1]*(1+(0.05F/ oldVels.Length));
                        }

                        //oldVels[i] = oldVels[i - 1];
                        if (projectile.oldPos[i] == Vector2.Zero)
                        {
                            oldVels[i] = Vector2.Zero;
                        }
                    }
                }
                int R = 1;
                if (projectile.DProj().Times[0] < 0)
                {
                    R = -1;
                }
                if (oldVels2 != 0)
                {
                    oldVels[0] = projectile.velocity.RotatedBy((0.05F- oldVels3) * R) + projectile.velocity.RotatedBy((0.05F- oldVels3) * R) * (oldVels2 * (projectile.scale));
                }
                if (Math.Abs(projectile.DProj().Times[0]) < 1)
                {
                    oldVels[1] = Vector2.Zero;
                }
                if (projectile.extraUpdates == 0 || Ti % projectile.extraUpdates / 2 == 0)
                {
                    if (Math.Abs(projectile.DProj().Times[0]) < 1)
                    {
                        ttt++;
                        for (int i = oldVels.Length - 1; i > 0; i--)
                        {
                            if (i > 1 && i < ttt)
                            {
                                oldVels[i] = Vector2.Zero;
                            }
                        }
                    }
                    else
                    {
                        ttt = 0;
                    }
                }
            }
            if (projectile.MeleeProj().DelayedKill > 2)
            {
                projectile.timeLeft = projectile.MeleeProj().DelayedKill;
                projectile.MeleeProj().DelayedKill--;

                if (oldVels != null)
                    oldVels[0] = Vector2.Zero;
                return false;
            }
            else if (projectile.MeleeProj().DelayedKill >= 1)
            {
                projectile.Kill();
            }
            //短剑
            if (projectile.aiStyle == 161)
            {
                Player player = Main.player[projectile.owner];
                Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
                projectile.scale = player.GetAdjustedItemScale(player.HeldItem);
                projectile.ProjScaleChange();
                int Splayer = (int)(player.HeldItem.useAnimation / player.GetTotalAttackSpeed(DamageClass.Melee))*2 * (projectile.extraUpdates + 1);
                int ASplayer = 15;
                if(projectile.ai[2]< 15)
                {
                    projectile.ai[2]++;
                }
                if (projectile.ai[2] == 2)
                {
                    PlaySound(SoundID.Item1, projectile.position);
                }
                if (Main.myPlayer == projectile.owner)
                {
                    if (projectile.DProj().vector[0] == Vector2.Zero)
                    {
                        projectile.localAI[0] = -Splayer;
                        projectile.DProj().vector[0] = Main.MouseWorld - vector2;
                    }
                }
                if (projectile.ai[0] == 0 && player.controlUseTile && projectile.localAI[1] == 0 && !player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                {
                    player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 600);
                    CC = 15+DaggerDashDistance;
                    CC *= projectile.extraUpdates + 1;
                    projectile.DProj().Bool[0] = true;
                }
                if (projectile.DProj().Bool[0])
                {
                    CC--;
                    player.dashDelay = 5;
                    player.velocity.Y = -0.001f;
                    player.velocity.X = -0.001f;
                    player.position += Collision.TileCollision(player.position, projectile.DProj().vector[0].PerfectNormalize() * 30 / (projectile.extraUpdates + 1), player.width, player.height, false, false);
                    DDPlayer.移动玩家(player, 30, true);
                    if ((Collision.TileCollision(player.position, projectile.DProj().vector[0].PerfectNormalize() * 30 / (projectile.extraUpdates + 1), player.width, player.height, false, false)- projectile.DProj().vector[0].PerfectNormalize() * 30 / (projectile.extraUpdates + 1)).Length()>5)
                    {
                        if (((15 + DaggerDashDistance) * projectile.extraUpdates + 1) - CC < 10)
                        {
                            if (player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                            {
                                player.buffTime[player.FindBuffIndex(ModContent.BuffType<SpecialAttackCD>())] = (int)(player.buffTime[player.FindBuffIndex(ModContent.BuffType<SpecialAttackCD>())] * 0.2F);
                            }
                        }
                        projectile.Kill();
                    }
                    player.Aplayer().NoGravity = 2;
                }
                else
                {
                    CC =0;
                }
                projectile.ai[0] = 16 * (projectile.ai[2]/(float)ASplayer);
                //戳完后
                if (projectile.ai[0] >= 16&&CC<=0&& projectile.localAI[1] < 3)
                {
                    if (projectile.DProj().Bool[0])
                    {
                        player.noFallDmg = true;
                        player.velocity = Vector2.Zero;
                        projectile.DProj().Bool[0] = false;
                    }
                    projectile.ClearInvincibleFrame();
                    projectile.ai[2] = 0;
                    projectile.localAI[1]++;
                }
                if (projectile.ai[2] ==ASplayer-1)
                {
                    projectile.DProj().Bool[4] = true;  
                    projectile.netUpdate = true;
                }

                if (projectile.localAI[1] >= 2)
                {
                    if(projectile.localAI[0]>=0)
                    {
                        if (!player.controlUseItem)
                        {
                            projectile.Kill();
                            player.itemTime = 0;
                            player.itemAnimation = 0;
                            return false;
                        }
                        projectile.localAI[0] = -Splayer;
                        projectile.DProj().vector[0] = Main.MouseWorld - vector2;
                        projectile.localAI[1] = 0;
                        projectile.ai[2] = 0F;
                        projectile.ai[1] = 0F;
                        projectile.ai[0] = 0;
                    }
                    else
                    {
                        if (projectile.ai[0] == 16)
                        {
                            projectile.ai[1] = 0F;
                            projectile.ai[0] = 0;
                        }
                        projectile.localAI[0]++;
                    }
                }

                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                float a = 0;
                //if (player.direction == -1) a = 3.14f;

                Player.CompositeArmStretchAmount amount;
                if (projectile.ai[0]<5)
                {
                    amount = (Player.CompositeArmStretchAmount)1;
                }
                else if (projectile.ai[0] < 10F)
                {
                    amount = (Player.CompositeArmStretchAmount)2;
                }
                else if (projectile.ai[0] < 15F)
                {
                    amount = (Player.CompositeArmStretchAmount)3;
                }
                else
                {
                    amount = (Player.CompositeArmStretchAmount)0;
                }

                float A = projectile.ai[0];
                projectile.HoldProj(player, 24 * projectile.scale + A / 3, 0, projectile.DProj().vector[0].RotatedBy(projectile.ai[1]), MathHelper.PiOver2, 0, true, 0);
                if (projectile.type != 802)
                {
                    if (projectile.localAI[1] == 0)
                    {
                        projectile.ai[1] = 0;
                    }
                    else
                    if (projectile.localAI[1] == 1)
                    {
                        projectile.ai[1] = -0.2F;
                    }
                    else if (projectile.localAI[1] == 2)
                    {
                        projectile.ai[1] = 0.2F;
                    }
                    else if (projectile.localAI[1] == 3)
                    {
                        projectile.ai[1] = 0;
                    }
                }
                player.PlayerAction().PlayerArmRotation(player.itemRotation-MathHelper.PiOver2*player.direction, amount);

                projectile.spriteDirection = ((!(Vector2.Dot(projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));
                projectile.netUpdate = true;
                return false;
            }
            //附魔剑
            if (projectile.type == 173)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
                if(projectile.localAI[1]<15)
                {
                    projectile.localAI[1]++;
                }
                Color color = new Color(63, 72, 204) * (projectile.localAI[1] / 15F);
                Lighting.AddLight(projectile.Center, color.ToVector3());
                Dust dust = Main.dust[Dust.NewDust(projectile.position - projectile.velocity, projectile.width, projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(63, 72, 204, 0) * (projectile.localAI[1] / 15F), 1.4F * (projectile.localAI[1] / 15F))];
                dust.velocity = -projectile.velocity.PerfectNormalize() * 5;
                dust.rotation = dust.velocity.ToRotation();
                return false;
            }
            //血肉球
            if (projectile.type == 154)
            {
                if (projectile.ai[0] == 6)
                {
                    if (projectile.DProj().Times[1] < 0.5f)
                    {
                        projectile.DProj().Times[1] += 0.025f;
                    }
                    else
                    {
                        if (projectile.DProj().Times[2] > 0)
                        {
                            projectile.DProj().Times[2]--;
                        }
                        for (int a = 0; a < 200; a++)
                        {
                            NPC npc = Main.npc[a];
                            if (npc.CanBeChasedBy())
                            {
                                if (Main.rand.NextBool(10) && (npc.Center - projectile.Center).Length() < 128 && projectile.DProj().Times[2] <= 0 && projectile.DProj().Times[0] < 10)
                                {
                                    projectile.DProj().Times[2] = 10;
                                    NewProjectile(projectile.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MeleeSuckBloodPlayer>(), projectile.damage, 0, projectile.owner, projectile.whoAmI, projectile.type);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (projectile.DProj().Times[1] > 0)
                    {
                        projectile.DProj().Times[1] -= 0.5f;
                    }
                }
            }
            //链球
            if (projectile.type == 25)
            {
                //落地
                if (projectile.ai[0] == 1)
                {
                    if (Main.rand.NextBool(10))
                    {
                        NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity / 1.5f, ModContent.ProjectileType<CorruptionBall>(), projectile.damage / 2, 0, projectile.owner);
                    }
                }
                if (projectile.ai[0] == 6)
                {
                    projectile.DProj().Times[1]++;
                    if (projectile.DProj().Times[1] % 30 == 0)
                    {
                        for (int A = -1; A <= 1; A++)
                        {
                            NewProjectile(projectile.GetSource_FromAI(), projectile.Center, new Vector2(0, -5).RotatedBy(Main.rand.NextFloat(0.9F, 0.1F) * A), ModContent.ProjectileType<Spikes>(), projectile.damage / 2, 0, projectile.owner);
                        }
                    }
                }
            }
            //链球ai
            if (projectile.aiStyle == 15)
            {

                Player player = Main.player[projectile.owner];
                player.itemRotation = (projectile.Center - player.Center).ToRotation();
                player.PlayerAction().PlayerArmRotation(player.itemRotation - MathHelper.PiOver2, Player.CompositeArmStretchAmount.Full);
            }
            //附魔回旋镖
            if (projectile.type == 6|| projectile.type == 19 ||projectile.type == 113||projectile.type == 52 ||projectile.type == 867 ||projectile.type == 1000)
            {
                projectile.HoldBoomerang(new Vector2(0, 20),projectile.Player().ActiveItem().shootSpeed/2);
                return false;
            }
            //附魔回旋镖
            if (projectile.type == 333||projectile.type == 33)
            {
                projectile.HoldBoomerang(new Vector2(0, 20), projectile.Player().ActiveItem().shootSpeed / 2);
                if(projectile.type == 33)
                {
                    NewDust(projectile.position, projectile.width, projectile.height, 40);
                }
                return false;
            }
            //附魔回旋镖
            if (projectile.type == 106)
            {
                projectile.HoldBoomerang(new Vector2(0, 20), projectile.Player().ActiveItem().shootSpeed / 2);
                if (projectile.DProj().Times[1] >= 120 && projectile.ai[2] ==0)
                {
                    projectile.ai[2] = 1;
                    for (int B = 0; B < 5; B++)
                    {
                        if(B==0)
                        {
                            continue;
                        }
                        int a = NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*0.8F, projectile.type, projectile.damage, projectile.knockBack, -1, 0, 0, 2);
                        Main.projectile[a].DProj().Bool[4] = true;
                        Main.projectile[a].DProj().Times[4] = projectile.DProj().Times[4];
                    }
                }
                else
                {
                    if(projectile.DProj().Times[1] <120&& projectile.ai[2]==2)
                    {
                        projectile.Track(800, 20, projectile.DProj().Times[0]);
                    }
                }
                return false;
            }
            //血叉
            if (projectile.type == 153)
            {
                Player player = Main.player[projectile.owner];
                Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);

                if (Main.myPlayer == projectile.owner)
                {
                    if (projectile.DProj().vector[0] == Vector2.Zero)
                    {
                        projectile.DProj().vector[0] = Main.MouseWorld - vector2;
                    }
                    if (projectile.DProj().Times[0] == 10)
                    {
                        player.Dplayer().PlayerShake(5, 8);
                    }
                }
                Vector2 velocity = Utils.RotatedBy(projectile.DProj().vector[0].PerfectNormalize(), projectile.ai[1], default);
                projectile.velocity = velocity * 2;
                if (projectile.DProj().Times[0] > 0)
                {
                    projectile.DProj().Times[0]--;
                    projectile.ai[1] -= 0.04f * player.direction;
                    projectile.extraUpdates = 0;
                }
                else
                {
                    projectile.extraUpdates = 4;
                }
                if (projectile.localAI[1] == 0)
                {
                    projectile.ai[0] += 0.7f + (player.GetTotalAttackSpeed(projectile.DamageType)) - ((float)player.ActiveItem().useAnimation / 100);
                    //戳完后
                    if (projectile.ai[0] > 55)
                    {
                        projectile.localAI[1]++;
                        SoundStyle sound = SoundID.DD2_DarkMageAttack;
                        sound.Pitch = -0.8F;
                        PlaySound(sound, projectile.position);
                    }
                }
                else if (projectile.localAI[1] == 1)
                {
                    projectile.ai[1] += 0.05f * player.direction;
                    if (projectile.ai[1] > MathHelper.TwoPi * 1.1f || projectile.ai[1] < -MathHelper.TwoPi * 1.1f || projectile.DProj().Times[0] == 1)
                    {
                        projectile.localAI[1]++;
                    }
                }
                else
                {
                    projectile.ai[0] -= 1f + player.GetTotalAttackSpeed(projectile.DamageType) - ((float)player.ActiveItem().useAnimation / 100);
                    if (projectile.ai[0] <= 30)
                    {
                        projectile.localAI[1]++;
                    }
                }
                player.itemTime = 5;
                player.itemAnimation = 5;

                if (projectile.localAI[1] >= 3)
                {
                    projectile.Kill();
                }

                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                float a = 0;
                if (player.direction == -1) a = 3.14f;

                float Rotation = projectile.velocity.ToRotation();
                player.itemRotation = Rotation + a;

                float A = projectile.ai[0];
                projectile.Center = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false) + projectile.velocity * A;

                projectile.spriteDirection = ((!(Vector2.Dot(projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));
                player.heldProj = projectile.whoAmI;
                projectile.netUpdate = true;
                return false;
            }
            //血脉球
            if (projectile.type == 543)
            {
                if (projectile.localAI[1] > 0)
                {
                    projectile.localAI[1]--;
                    projectile.localAI[0]--;
                }
            }
            //抑郁球
            if (projectile.type == 542)
            {
                projectile.localNPCHitCooldown = 10;
                if (!Main.dayTime)
                {
                    projectile.localAI[0] = 0;
                    projectile.alpha = 140;
                    projectile.velocity *= 1.05f;
                    projectile.tileCollide = false;
                }
                else
                {
                    projectile.tileCollide = true;
                    projectile.alpha = 0;
                }

            }
            //真永夜
            if (projectile.type == 157)
            {
                projectile.ProjScaleChange();
                Dust dust = Main.dust[NewDust(projectile.position, projectile.width, projectile.height, 75, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = projectile.scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5);
            }
            //真神圣剑
            if (projectile.type == 156)
            {
                projectile.ProjScaleChange();
                Dust dust = Main.dust[NewDust(projectile.position, projectile.width, projectile.height, 73, projectile.oldVelocity.X, projectile.oldVelocity.Y, 255, default)];
                dust.noGravity = true;
                dust.scale = projectile.scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5);
            }
            if (projectile.type == 9)
            {
                if (projectile.localAI[2]==0)
                {
                    projectile.extraUpdates = 1;
                    projectile.velocity /= 2;
                    projectile.localAI[2] = 1;
                }
                Color color = new Color(255, 153, 183, 120);
                Lighting.AddLight(projectile.Center, color.ToVector3());
                projectile.scale = 0.75F;
                if (projectile.ai[2] < 0)
                {
                    projectile.penetrate = 1;
                }
                if (projectile.penetrate == 1)
                {
                    projectile.scale = 0.5F;
                    projectile.ProjScaleChange();
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 1500, false);
                    if (projectile.ai[2] >= 0)
                    {
                        npc = NPCdirection.FindClosest2(projectile.Center, 1500, false, new NPC[] { Main.npc[(int)projectile.ai[2]] });
                    }
                    if (npc != null)
                    {
                        projectile.Track(200, 10, 10, 0, false, npc.whoAmI);
                    }
                    if (Main.rand.NextBool(5))
                    {

                        Dust dust = Main.dust[Dust.NewDust(projectile.position - new Vector2(10), projectile.width + 20, projectile.height + 20, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 153, 183, 0), Main.rand.NextFloat(1F, 2.2F))];
                        dust.velocity = -projectile.velocity.PerfectNormalize() * 5;
                        dust.customData = 0.8F;
                        dust.rotation = dust.velocity.ToRotation();
                    }
                }
                else
                {

                    Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 153, 183, 120), 4)];
                    dust.velocity = -projectile.velocity.PerfectNormalize() * 5;
                    dust.rotation = dust.velocity.ToRotation();
                }
                if (projectile.Center.Y < projectile.ai[1])
                {
                    projectile.rotation += 0.3F;
                    projectile.tileCollide = false;
                    return false;
                }
            }
            if (projectile.type == 92)
            {
                Color color = new Color(201, 166, 204, 0);
                Lighting.AddLight(projectile.Center, color.ToVector3());
                Dust dust = Main.dust[Dust.NewDust(projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(),0,0,0, new Color(201, 166, 204, 150), 4)];
                dust.velocity = -projectile.velocity.PerfectNormalize()*5;
                dust.rotation = dust.velocity.ToRotation();
                dust.customData = 3;
                projectile.rotation += projectile.velocity.X * 0.01F;
                if (projectile.velocity.X > 0)
                {
                    projectile.rotation += Math.Abs(projectile.velocity.Y) * 0.01F;
                }
                else
                {
                    projectile.rotation -= Math.Abs(projectile.velocity.Y) * 0.01F;
                }
                if (projectile.position.Y > projectile.ai[1])
                    projectile.tileCollide = true;
                return false;
            }
            //波涌
            if(projectile.type == 451)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
                projectile.localAI[1]++;
                if(projectile.localAI[1]>600)
                {
                    projectile.ai[0] = 1;
                }
                if (!projectile.DProj().Bool[0])
                {
                    projectile.ai[2] = projectile.velocity.ToRotation();
                    projectile.DProj().Bool[0] = true;
                }
                if (projectile.ai[0] >= 1f)
                {
                    projectile.tileCollide = false;
                    projectile.velocity *= 0.86F;
                    if (projectile.alpha < 255)
                    {
                        projectile.alpha += 10;
                    }
                    else
                    projectile.Kill();
                }
                else
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 600, false);
                    
                    if(npc!=null&&projectile.DProj().track>10)
                    {
                        if(projectile.localAI[2]<0.1F)
                        {
                            projectile.localAI[2] += 0.005F;
                        }
                        else
                        {
                            projectile.localAI[2] = 0.1f;
                        }
                        DDHelper.RotateSpeed(ref projectile.ai[2], (npc.Center-projectile.Center).ToRotation(), projectile.localAI[2]);
                    }
                    else
                    {
                        if(projectile.localAI[2]>0)
                        {
                            projectile.localAI[2] -= 0.02F;
                        }
                        else
                        {
                            projectile.localAI[2] = 0;
                        }
                    }
                    projectile.velocity = projectile.ai[2].ToRotationVector2() * 18;
                    //projectile.Track(500, 20, 18, 10);
                    if (projectile.alpha > 0)
                    {
                        projectile.alpha -= 50;
                    }
                    else
                    {

                        if (Main.rand.NextBool(2))
                        {
                            int D = NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(97, 200, 225, 0)*0.25f, Main.rand.NextFloat(1.2F, 1.8F));
                            Main.dust[D].velocity = -projectile.velocity * Main.rand.NextFloat(0.5F, 0.8F);
                            Main.dust[D].rotation = projectile.velocity.ToRotation();
                            Main.dust[D].customData = 0.7F;
                        }
                        {
                            int D = NewDust(projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(97, 200, 225, 0) * 0.25f, 1.5f);
                            Main.dust[D].velocity = Vector2.Zero;
                            Main.dust[D].customData = 3F;
                            D = NewDust(projectile.Center - new Vector2(4) + projectile.velocity / 2, 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(97, 200, 225, 0) * 0.25f, 1.5f);
                            Main.dust[D].velocity = Vector2.Zero;
                            Main.dust[D].customData = 3F;
                            D = NewDust(projectile.Center - new Vector2(4) - projectile.velocity.PerfectNormalize() * 40, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(97, 200, 225, 0) * 0.25F, 4.5f);
                            Main.dust[D].velocity = Vector2.Zero;
                            Main.dust[D].rotation = projectile.velocity.ToRotation();
                            Main.dust[D].customData = 3F;
                        }
                    }
                }
                projectile.scale = 0.8F;
                return false;
            }
            if (projectile.type == 274)
            {
                if (projectile.ai[2]<3)
                {
                    projectile.Track(600, 20, 8, 10);
                }
            }
            if (projectile.type == 228)
            {
                    projectile.Track(300, 20, 8, 10);
            }
            if (projectile.type == 229)
            {
                projectile.frameCounter++;
                projectile.frame = (projectile.frameCounter / 4) % 4;
                if (projectile.ai[1]==0)
                {
                    projectile.velocity *= 1.5f;
                    
                    projectile.damage = (int)(projectile.damage*1.5F);
                    projectile.CritChance = 100;
                    projectile.DProj().Magnification = 1.5F;
                    projectile.ai[1] = projectile.velocity.Length();
                }
                if (projectile.ai[2]==1)
                {
                    if (!projectile.DProj().Bool[0])
                    {
                        projectile.timeLeft = 10;
                        NewDustChange4(300,projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<光球粒子>(),0,12,true,1.3F,2.4F,100,0,new Color(100,255,0,0),new Dust().DustAI(1)+3);
                        projectile.DProj().Bool[0] = true;
                    }
                    projectile.aiStyle = -1;
                    projectile.velocity = projectile.velocity.PerfectNormalize()*0.01F;
                    projectile.frame = 3;
                    projectile.localAI[0] += 0.1F;
                    projectile.scale += 0.9F;
                    projectile.alpha = 188;
                    projectile.ProjScaleChange();
                    projectile.tileCollide = false;
                }
                else
                {
                    NPC result = null;
                    if (projectile.velocity.Y > 0)
                    {
                        float num = 500;
                        for (int i = 0; i < 200; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (!npc.CanBeChasedBy()|| (projectile.Center.Y- npc.Center.Y)>0)
                            {
                                continue;
                            }
                            float num2 = (projectile.Center - npc.Center).Length();
                            if (!(num <= num2))
                            {
                                if (Collision.CanHitLine(projectile.Center, 1, 1, npc.position, npc.width, npc.height))
                                {
                                    num = num2;
                                    result = npc;
                                }
                            }
                        }
                        if (result != null && result.active && projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 0)
                        {
                            if (!projectile.hostile && projectile.friendly)
                            {
                                Vector2 vector = (result.Center - projectile.Center).PerfectNormalize() * projectile.ai[1];
                                projectile.velocity.X = (projectile.velocity.X * 10 + vector.X) / (10 + 1);
                            }
                        }
                    }
                    if(projectile.velocity.Y<12&& projectile.GetGlobalProjectile<DDGlobalProjectile>().track>30)
                    {
                        projectile.velocity.Y += 0.2F;
                    }
                    NewDustChange4(3, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, true, 0.3F, 1.4F, 100, 0, new Color(100, 255, 0, 0), (new Dust().DustAI(1) + 3));
                }
                return false;
            }
                return base.PreAI(projectile);
        }
        public override void PostAI(Projectile projectile)
        {
            if (projectile.stopsDealingDamageAfterPenetrateHits)
            {
                if (!projectile.MeleeProj().ClearInvincibility || (projectile.localNPCHitCooldown >= 0 && projectile.localNPCHitCooldown < 1000))
                {
                    DS = 0;
                }
                else
                {
                    //projectile.damage = (int)(projectile.damage * (1 - (int)(DS / 4) * 0.3F));
                }
            }
        }
        float DS = 0;
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if(projectile.aiStyle==15)
            {
                if(projectile.owner == Main.myPlayer)
                {
                    fallThrough = (Main.MouseWorld.Y) - (projectile.position.Y + projectile.height) >= 0;
                    if(fallThrough)
                    {
                        projectile.netUpdate = true;
                    }
                }
            }
            if (projectile.type == 9)
            {
                fallThrough = true;
            }

                return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            //附魔剑
            if (projectile.type == 173)
            {
                NewDustChange2(20, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 0.4F, 1.3F, 100, new Color(63, 72, 204, 0));
                if (NPCdirection.FindClosest(projectile.Center, 500, false) != null)
                {
                    projectile.Track(500, 0, oldVelocity.Length(), 0);
                }
                else
                {
                    if (projectile.velocity.X != oldVelocity.X)
                    {
                        projectile.velocity.X = -oldVelocity.X;
                    }
                    if (projectile.velocity.Y != oldVelocity.Y)
                    {
                        projectile.velocity.Y = -oldVelocity.Y;
                    }
                }
                projectile.DProj().Times[0]++;
                if (projectile.DProj().Times[0]>=5)
                {
                    projectile.Kill();
                }
                SoundEngine.PlaySound(SoundID.Item10, projectile.position);
                return false;
            }
            if (projectile.type == 6 || projectile.type == 19 || projectile.type == 113 || projectile.type == 52 || projectile.type == 867 || projectile.type == 1000)
            {
                projectile.localAI[0] = (-projectile.velocity).ToRotation();
                projectile.DProj().Times[1] += 120;
                return false;
            }
            //飞盘
            if (projectile.type == 333 || projectile.type == 33)
            {
                if(projectile.velocity.X!=oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if(projectile.velocity.Y!=oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                projectile.localAI[0] = (projectile.velocity).ToRotation();
                PlaySound(SoundID.Dig, projectile.Center);
                return false;
            }
            if (projectile.type == 106)
            {
                if(projectile.velocity.X!=oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if(projectile.velocity.Y!=oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                projectile.localAI[0] = (projectile.velocity).ToRotation();
                PlaySound(SoundID.Dig, projectile.Center);
                return false;
            }
            //强大球珠
            if (projectile.type == 229)
            {
                projectile.tileCollide = false;
                projectile.ai[2] = 1;
                return false;

            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool? CanDamage(Projectile projectile)
        {
            if (projectile.stopsDealingDamageAfterPenetrateHits)
            {
                if(DS>=16)
                {
                    return false;
                }
            }
                //短剑
                if (projectile.aiStyle == 161)
            {
                return projectile.localAI[1] <3&& (projectile.localAI[1] < 3 || projectile.ai[0] >= 6);
            }
            //剑碰撞箱
            if (SwordHitbox)
            {
                Player player = Main.player[projectile.owner];
                return projectile.localAI[1] >0|| TrueDamage;
            }
            if (projectile.MeleeProj().DelayedKill > 0)
            {
                return false;
            }
            //波涌
            if (projectile.type == 451)
            {
                if(projectile.DProj().track <10)
                {
                    return false;
                }
            }
                return base.CanDamage(projectile);
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            //短剑
            if (projectile.aiStyle == 161)
            {
                if(projectile.localAI[1]!=2)
                {
                    modifiers.Knockback *= 0.2F;
                }
                if (projectile.localAI[1] == 0)
                {
                    if (target.knockBackResist > 0)
                        target.velocity = Vector2.Zero;
                    projectile.ai[0] = 15 + DaggerDashDistance;
                    if (projectile.DProj().Bool[0])
                    {
                        player.immune = true;
                        player.immuneTime = 60;
                        player.immuneNoBlink = true;

                        player.noFallDmg = true;
                        target.AddBuff(ModContent.BuffType<Bleed>(), 300);
                        player.velocity = Vector2.Zero;
                        modifiers.SourceDamage *= 4;
                        modifiers.DefenseEffectiveness *= 4;
                        CC = 0;
                        projectile.ai[2] = 16;
                    }
                }
                if (target.HasBuff(ModContent.BuffType<Bleed>()))
                {
                    modifiers.SourceDamage *= 1.5F;
                }
            }
            //风暴长毛
            if (projectile.type == 732)
            {
                int A = NewProjectile(projectile.GetSource_FromAI(), projectile.Center - new Vector2(Main.rand.Next(-3000, 3000) / 10, 800), new Vector2(0, 10), ModContent.ProjectileType<闪电>(), projectile.damage, 3, projectile.owner, 0, 1, 400);
                Main.projectile[A].DamageType = DamageClass.Melee;
            }
            if(projectile.stopsDealingDamageAfterPenetrateHits)
            {
                modifiers.SourceDamage -= ((int)(DS / 4) * 0.3F);
                //projectile.damage = (int)(projectile.damage * (1 - (int)(DS / 4) * 0.3F));
            }
            //血叉
            if (projectile.type == 153)
            {
                modifiers.Knockback *= 2 - (target.Center - player.Center).Length() / 150;
                float Pi = Math.Abs(projectile.ai[1]);
                if (projectile.localAI[1] == 1 && Pi > MathHelper.TwoPi * 0.8f)
                {
                    modifiers.SourceDamage *= 2;
                    projectile.DProj().Times[0] = 10;
                    for (int a = 0; a < 10; a++)
                    {
                        target.HitEffect(0, 0);
                        //NewDust(target.position, target.width, target.height, 5,0,0,0,default,2);
                    }
                    target.AddBuff(30, Main.rand.Next(100, 300));
                    SoundStyle sound = SoundID.DD2_DarkMageAttack;
                    sound.Pitch = 0.8F;
                    PlaySound(sound, projectile.position);
                }
                if (target.HasBuff(BuffID.Bleeding))
                {
                    modifiers.SourceDamage += 0.2F;
                }
            }
            //血脉球
            if (projectile.type == 543)
            {
                if (projectile.localAI[1] > 0)
                {
                    modifiers.Knockback *= 0;
                    modifiers.SourceDamage += 0.5F;
                    projectile.localAI[0] -= 20;
                }
                if (target.HasBuff(BuffID.Bleeding))
                {
                    modifiers.SourceDamage += 0.2f;
                }
                if (Main.rand.NextBool(10) && !projectile.DProj().Bool[0])
                {
                    projectile.localAI[1] = 300;
                    projectile.DProj().Bool[0] = true;
                    projectile.netUpdate = true;
                }
            }
            //血肉球
            if (projectile.type == 154)
            {
                if (target.HasBuff(BuffID.Bleeding))
                {
                    modifiers.SourceDamage += 0.2f;
                }
            }
            //冰雪弹
            if (projectile.type == 118)
            {
                target.AddBuff(44,120);
            }

        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            DS++;
                Player player = Main.player[projectile.owner];
            if (projectile.type == 9)
            {
                projectile.ai[2] = target.whoAmI;
                NewDustChange2(20, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<天堂粒子>(), 0, 8, true, 0.4F, 1F, Alpha: 100);
                NewDustChange3(10, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 4, true, 0.4F, 1.2F, 100, 0, new Color(255, 153, 183, 120));
                for (int a = 0; a < 16; a++)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.position - new Vector2(10), projectile.width + 20, projectile.height + 20, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 153, 183, 120), Main.rand.NextFloat(2.5F, 4.2F))];
                    dust.velocity = -projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.6F,0.6F)) * 2*dust.scale;
                    dust.customData = 1.8F;
                    dust.rotation = dust.velocity.ToRotation();
                }
                projectile.velocity *= -1;
                projectile.damage /= 2;
                projectile.netUpdate = true;
            }
                //强大球珠
                if (projectile.type == 229)
            {
                projectile.ai[2] = 1;
                
            }
            //天顶剑
            if (projectile.type == 933)
            {
                FinalFractalProfile finalFractalProfile = GetFinalFractalProfile((int)projectile.ai[1]);
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = finalFractalProfile.trailColor;
                Main.projectile[A].DProj().color.A = 0 ;
                Main.projectile[A].localAI[0] = 0.6F;
                Main.projectile[A].scale = finalFractalProfile.trailWidth / 40;

            }
            //真空刃
            if (projectile.type == 595 || projectile.type == 735)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().Ecolor = new Color(80, 80, 119, 150) * 1F;
                Main.projectile[A].DProj().Ecolor2 = new Color(80, 80, 119, 0).Opposite() * 0.5F;
                if (projectile.type == 735)
                {
                    Main.projectile[A].DProj().Ecolor = new Color(80, 119, 80, 150) * 1F;
                    Main.projectile[A].DProj().Ecolor2 = new Color(80, 119, 80, 0).Opposite() * 0.5F;
                }
                Main.projectile[A].localAI[0] = 0.3F;
                Main.projectile[A].localAI[1] = 0.5F;
                Main.projectile[A].localAI[2] = 0F;
                Main.projectile[A].scale = 0.5F;
            }
            //死神镰刀
            if (projectile.type == 274)
            {
                projectile.ai[2]++;
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = new Color(170, 10, 255, 0)*0.3F;
                Main.projectile[A].localAI[0]=1F;
                Main.projectile[A].scale = 1f;
            }
            //死神镰刀
            if (projectile.type == 263)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = new Color(16, 120, 205, 0) * 0.3F;
                Main.projectile[A].localAI[0]=1F;
                Main.projectile[A].scale = 1f;
                if (!target.Dnpc().BossPhysique && target.realLife < 0)
                {
                    target.AddBuff(ModContent.BuffType<Freeze>(), (int)Main.rand.NextFloat(120, 300));
                }
                else
                {
                    target.AddBuff(ModContent.BuffType<Frozen>(), (int)Main.rand.NextFloat(120, 300));
                }
            }
            //血肉球
            if (projectile.type == 154)
            {
                int Blood = hit.SourceDamage / 50;
                DDHelper.MaxandMin(ref Blood, player.statLifeMax2 / 20, 1);
                if (projectile.DProj().Times[0] < 10)
                {
                    projectile.DProj().Times[0] += Blood;
                }
            }
            if (projectile.type == 19 || projectile.type == 52)
            {
                projectile.localAI[0] = (projectile.velocity).ToRotation();
                projectile.DProj().Times[1] += 120;
            }
            if (projectile.type == 867)
            {
                if (projectile.ai[1] <2)
                {
                    for (int a = 0; a < 5; a++)
                    {
                        NewProjectile(projectile.GetSource_FromAI(), projectile.Center, new Vector2(12, 0).RotatedBy(MathHelper.TwoPi / 5 * a), 131, projectile.damage / 5, 0, -1);
                    }
                }
                if (projectile.ai[1] >= 1)
                {
                    projectile.localAI[0] = (projectile.velocity).ToRotation();
                    projectile.DProj().Times[1] += 120;
                }
                else
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false, target);
                    if (npc != null)
                    {
                        projectile.velocity = (npc.Center - projectile.Center).PerfectNormalize() * projectile.DProj().Times[0];
                        projectile.DProj().Times[1] = 60;
                    }
                    else
                    {
                        projectile.localAI[0] = (projectile.velocity).ToRotation();
                        projectile.DProj().Times[1] += 120;
                    }
                }
                projectile.ai[1]++;
                projectile.damage = (int)(projectile.damage * 0.5F);
            }
            if (projectile.type == 6)
            {
                NewDustChange2(20, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 1.4F, 2.3F, 100, new Color(63, 72, 204, 0));
                if (projectile.ai[1] >= 3)
                {
                    projectile.localAI[0] = (projectile.velocity).ToRotation();
                    projectile.DProj().Times[1] += 120;
                }
                else
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false, target);
                    if (npc != null)
                    {
                        projectile.velocity = (npc.Center - projectile.Center).PerfectNormalize() * projectile.DProj().Times[0];
                        projectile.ai[1]++;
                        projectile.DProj().Times[1] = 60;
                    }
                    else
                    {
                        projectile.localAI[0] = (projectile.velocity).ToRotation();
                        projectile.DProj().Times[1] += 120;
                        projectile.ai[1]+=3;
                    }
                }
                projectile.damage = (int)(projectile.damage * 0.5F);
            }
            if (projectile.type == 113)
            {
                NewDustChange2(20, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 1.4F, 2.3F, 100, new Color(13, 172, 204, 0));
                if (projectile.ai[1] >= 3)
                {
                    projectile.localAI[0] = (projectile.velocity).ToRotation();
                    projectile.DProj().Times[1] += 120;
                }
                else
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false, target);
                    if (npc != null)
                    {
                        projectile.velocity = (npc.Center - projectile.Center).PerfectNormalize() * projectile.DProj().Times[0];
                        projectile.ai[1]++;
                        projectile.DProj().Times[1] = 60;
                    }
                    else
                    {
                        projectile.localAI[0] = (projectile.velocity).ToRotation();
                        projectile.DProj().Times[1] += 120;
                        projectile.ai[1]+=3;
                    }
                }
                projectile.damage = (int)(projectile.damage * 0.5F);
            }
            if (projectile.type == 1000)
            {
                NewDustChange2(20, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 1.4F, 2.3F, 100, new Color(63, 72, 204, 0));
                if (projectile.ai[1] >= 5)
                {
                    projectile.localAI[0] = (projectile.velocity).ToRotation();
                    projectile.DProj().Times[1] += 120;
                }
                else
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false, target);
                    if (npc != null)
                    {
                        projectile.velocity = (npc.Center - projectile.Center).PerfectNormalize() * projectile.DProj().Times[0];
                        projectile.ai[1]++;
                        projectile.DProj().Times[1] = 60;
                    }
                    else
                    {
                        projectile.localAI[0] = (projectile.velocity).ToRotation();
                        projectile.DProj().Times[1] += 120;
                        projectile.ai[1]+=5;
                    }
                }
                projectile.damage = (int)(projectile.damage * 0.75F);
            }
            if (projectile.type == 106)
            {

                projectile.DProj().Times[1] += 120;
            }
                if (projectile.type == 19)
            {
                NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, 978, projectile.damage, 0, -1);

                NewDustChange2(30, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 1, 2, 100, new Color(253, 62, 3, 0));

            }
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            //附魔剑
            if (projectile.type == 173)
            {
                NewDustChange2(30, projectile.Center-new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 0, 6, true, 0.4F, 2.5F, 100, new Color(63, 72, 204, 0));
                return false;
            }
            if ((projectile.type == 91 || (projectile.type == 92 && projectile.ai[0] > 0f)) && projectile.owner == Main.myPlayer)
            {
                float x = projectile.position.X + (float)Main.rand.Next(-400, 400);
                float y = projectile.position.Y - (float)Main.rand.Next(600, 900);
                Vector2 vector57 = new Vector2(x, y);
                float num590 = projectile.position.X + (float)(projectile.width / 2) - vector57.X;
                float num591 = projectile.position.Y + (float)(projectile.height / 2) - vector57.Y;
                int num592 = 22;
                float num593 = (float)Math.Sqrt(num590 * num590 + num591 * num591);
                num593 = (float)num592 / num593;
                num590 *= num593;
                num591 *= num593;
                int num594 = projectile.damage;
                if (projectile.type == 91)

                    num594 /= 3;

                int num595 = NewProjectile(projectile.GetSource_FromAI(), x, y, num590, num591, 92, num594, projectile.knockBack, projectile.owner);
                if (projectile.type == 91)
                {
                    Main.projectile[num595].ai[1] = projectile.position.Y;
                    Main.projectile[num595].ai[0] = 1f;
                }
                else
                {
                    Main.projectile[num595].ai[1] = projectile.position.Y;
                }
            }
            if (projectile.type == 92)
            {
                NewDustChange3(10, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 6, true, 0.4F, 2.5F, 100, 0, new Color(201, 166, 204, 150));
                return false;
            }
            if (projectile.type == 91)
            {
                NewDustChange3(10, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 6, true, 0.4F, 1.5F, 100, 0, new Color(201, 166, 204, 150));
                return false;
            }

            return base.PreKill(projectile, timeLeft);
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            //血肉球
            if (projectile.type == 154)
            {
                if (projectile.DProj().Times[0] >= 1)
                {
                    player.statLife += (int)projectile.DProj().Times[0];
                    player.HealEffect((int)projectile.DProj().Times[0]);
                    NetMessage.SendData(66, -1, -1, null, projectile.owner, 3, 0f, 0f, 0, 0, 0);
                }
            }
            if (projectile.type == 9)
            {
                NewDustChange2(30, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<天堂粒子>(), 0, 12,true,0.4F,1.2F,Alpha:100);
                NewDustChange3(10, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 6, true, 0.4F, 1.6F, 100,0, new Color(255, 153, 183, 120));
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/魔法");
                sound.Pitch = 1;
                PlaySound(sound, projectile.Center);
            }
        }
        int ttt;
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            SpriteEffects spriteEffects = 0;
            if (projectile.spriteDirection == 1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            /*
            //短剑
            if (projectile.aiStyle == 161)
            {
                Item item = projectile.Player().ActiveItem();
                Vector2 Velocity = Utils.RotatedBy(Vector2.Normalize(projectile.velocity), 0, default);
                Vector2 cen = Vector2.Zero;
                if (player.mount.Active)
                {
                    projectile.Center = player.RotatedRelativePoint(player.ArmCenter() - cen, reverseRotation: false, addGfxOffY: false) + Velocity * (TextureAssets.Item[item.type].Size().Length() / 2 * projectile.scale + 4) + new Vector2(0, player.gfxOffY);

                }
                else
                {
                    projectile.Center = player.ArmCenter() - cen + Velocity * (TextureAssets.Item[item.type].Size().Length() / 2 * projectile.scale + 4) + new Vector2(0, player.gfxOffY);
                }
                Vector2 Center = projectile.Center - Main.screenPosition;


                if (projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, projectile.rotation, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                }
                return false;
            }*/
            //强大球珠
            if (projectile.type == 229)
            {
                Rectangle value = new Rectangle(0, texture.Height / 4*projectile.frame, texture.Width, texture.Height/4);
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(value), Color.White*(1- projectile.localAI[0]), projectile.rotation, value.Size() / 2, projectile.scale, 0, 0f);
                return false;

            }
            //附魔回旋镖
            if (projectile.type == 6 || projectile.type == 19 || projectile.type == 113 || projectile.type == 52 || projectile.type == 867 || projectile.type == 1000)
            {
                Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height);
                SpriteEffects sprite = 0;
                if (projectile.DProj().vector[0].X > 0)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(value), lightColor, projectile.rotation, texture.Size() / 2, projectile.scale, sprite, 0f);
                return false;
            }
                //血肉球
                if (projectile.type == 154)
            {
                DDHelper.BackAndForth(0.3F, 0.6f, 0.01f, ref projectile.DProj().Times[3], ref projectile.DProj().Bool[1]);
                Texture2D Perlin = DDTextures.Perlin.Value;
                Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + projectile.Size / 2, null, Color.White, projectile.rotation, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                for (int A = 0; A < projectile.DProj().Times[0]; A++)
                {
                    Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + projectile.Size / 2, null, new Color(55, 0, 0, 0), projectile.rotation, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                projectile.DProj().Times[4] += 0.01f;
                DDHelper.AnnularShaders(1, new Color(155, 20, 20), projectile.DProj().Times[4]);
                Main.spriteBatch.Draw(Perlin, projectile.position - Main.screenPosition + projectile.Size / 2, null, Color.White, -MathHelper.PiOver2, Perlin.Size() / 2, projectile.DProj().Times[1], 0, 0f);
                Main.spriteBatch.Draw(Perlin, projectile.position - Main.screenPosition + projectile.Size / 2, null, Color.White, -MathHelper.PiOver2, Perlin.Size() / 2, projectile.DProj().Times[1], 0, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                return false;
            }
            //血脉球
            if (projectile.type == 543)
            {
                Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + projectile.Size / 2, null, Color.White, projectile.rotation, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                if (projectile.localAI[1] == 0) return false;

                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition;
                    Color oldcolor = new Color(255, 0, 0, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor, projectile.rotation, texture.Size() / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                }

                return false;
            }
            //血叉
            if (projectile.type == 153)
            {
                spriteEffects = 0;
                if (player.direction == 1)
                {
                    spriteEffects = (SpriteEffects)1;
                }
                float ro = projectile.rotation + MathHelper.PiOver4;
                if (player.direction == 1)
                {
                    ro = projectile.rotation - MathHelper.PiOver4;
                }
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition;
                    Color oldcolor = new Color(255, 0, 0, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2 + projectile.velocity.PerfectNormalize() * 8, new Rectangle?(new Rectangle(0, 0, 30, 30)), oldcolor, ro, new Vector2(30, 30) / 2, projectile.scale * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition - projectile.velocity.PerfectNormalize() * 38, null, lightColor, ro, new Vector2(texture.Width, texture.Height) / 2, projectile.scale, spriteEffects, 0f);
                return false;
            }
            if (projectile.type == 9)
            {
                spriteEffects = (SpriteEffects)1;
                if (projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture2 = DDTextures.MagicBall.Value;
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2-projectile.velocity.PerfectNormalize()*10;
                
                
                    Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition, null, new Color(237, 63, 133, 120) *0.5F, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 2, spriteEffects, 0f);
          
                    for (int i = 0; i < 3; i++)
                    {
                        Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition + new Vector2(6 * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10), null, new Color(237, 63, 133, 0), projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 2, spriteEffects, 0f);
                        for (int i2 = 0; i2 < projectile.oldPos.Length; i2++)
                        {
                            Vector2 vector2 = projectile.oldPos[i2] + vector - Main.screenPosition + new Vector2(6 * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10);
                            Color color = new Color(237, 63, 133, 120) * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length / 2);
                            //Main.spriteBatch.Draw(DDtexture2s.VoidStar.Value, vector2, null, color * 0.6f, projectile.rotation, ModContent.Request<texture22D>("DDmod/Image/VoidStar").Size() / 2, projectile.scale / 2 * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                            Main.spriteBatch.Draw(texture2, vector2, null, color, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 2 * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                        }
                    }
                    Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(237, 63, 133, 150), projectile.rotation, texture.Size() / 2, projectile.scale, spriteEffects, 0f);
                
                return false;
            }
            if (projectile.type == 92)
            {
                spriteEffects = (SpriteEffects)1;
                if (projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture2 = DDTextures.MagicBall.Value;
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2-projectile.velocity.PerfectNormalize()*10;Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition, null, new Color(201, 106, 204, 150) *0.25F, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 2, spriteEffects, 0f);

                for (int i = 0; i < 3; i++)
                {Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition + new Vector2(6 * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10), null, new Color(201, 106, 204, 150) * 0.25F, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale/2, spriteEffects, 0f);
                    for (int i2 = 0; i2 < projectile.oldPos.Length; i2++)
                    {
                        Vector2 vector2 = projectile.oldPos[i2] + vector - Main.screenPosition + new Vector2(6 * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10);
                         Color color = new Color(201, 106, 204, 150) *0.5f * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length / 2);
                        //Main.spriteBatch.Draw(DDtexture2s.VoidStar.Value, vector2, null, color * 0.6f, projectile.rotation, ModContent.Request<texture22D>("DDmod/Image/VoidStar").Size() / 2, projectile.scale / 2 * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture2, vector2, null, color, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 2 * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                    }
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(201, 106, 204, 150), projectile.rotation, texture.Size() / 2, projectile.scale, spriteEffects, 0f);
                return false;
            }
            if (projectile.type == 91)
            {
                spriteEffects = (SpriteEffects)1;
                if (projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture2 = DDTextures.MagicBall.Value;
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2+projectile.velocity.PerfectNormalize()*6;
                Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition, null, new Color(201, 106, 204, 150) * 0.75F, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 6, spriteEffects, 0f);

                for (int i = 0; i < 3; i++)
                {
                    Main.spriteBatch.Draw(texture2, projectile.position + vector - Main.screenPosition + new Vector2(0.5f * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10), null, new Color(201, 106, 204, 150) * 0.75F, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 6, spriteEffects, 0f);
                    for (int i2 = 0; i2 < projectile.oldPos.Length; i2++)
                    {
                        Vector2 vector2 = projectile.oldPos[i2] + vector - Main.screenPosition + new Vector2(0.5f * projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)projectile.timeLeft / 10);
                        Color color = new Color(201, 106, 204, 150) * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length / 2);
                        //Main.spriteBatch.Draw(DDtexture2s.VoidStar.Value, vector2, null, color * 0.6f, projectile.rotation, ModContent.Request<texture22D>("DDmod/Image/VoidStar").Size() / 2, projectile.scale / 2 * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture2, vector2, null, color, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 6 * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                        if (i2 < projectile.oldPos.Length - 1)
                        {
                            Main.spriteBatch.Draw(texture2, vector2 - (projectile.oldPos[i2]- projectile.oldPos[i2+1])/2, null, color, projectile.velocity.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, projectile.scale / 6 * ((projectile.oldPos.Length - i2) / (float)projectile.oldPos.Length), spriteEffects, 0f);
                        }
                    }
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, texture.Size() / 2, projectile.scale, spriteEffects, 0f);
                return false;
            }
            if (projectile.type == 118|| projectile.type == 128|| projectile.type == 129)
            {
                texture = XiaoiceBombs.Value;

                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), projectile.velocity.ToRotation(), texture.Size() / 2, projectile.scale/4, 0, 0f);
                return false;
            }
            if (projectile.type == 119)
            {
                texture = XiaoiceBombs.Value;
                Texture2D texture2 = iceBombs.Value;
                if (projectile.penetrate > 2)
                {
                    Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), projectile.velocity.ToRotation(), texture2.Size() / 2, projectile.scale / 4*1.2f, 0, 0f);
                }
                else
                if (projectile.penetrate > 1)
                {
                    Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), projectile.velocity.ToRotation(), texture2.Size() / 2, projectile.scale / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), projectile.velocity.ToRotation(), texture.Size() / 2, projectile.scale / 4, 0, 0f);
                }
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
        int MCD;
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {

            //剑碰撞箱
            if (SwordHitbox || (projectile.type == ModContent.ProjectileType<GlobalSword>() && projectile.Player().ActiveItem().type == ItemID.ZombieArm))
            {
                if (projectile.aiStyle == 161)
                {
                    Player player = Main.player[projectile.owner];
                    float num2 = 0.75f;
                    float LaserLength = MathHelper.Lerp(0, ExtraLength * projectile.scale, num2);
                    Vector2 Pvelocity = Utils.RotatedBy(projectile.velocity.PerfectNormalize(), 0, default);
                    float num = 0f;
                    return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), player.Center, projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num));
                }
                else
                {
                    MCD++;
                    if (MCD < 4)
                    {
                        return false;
                    }
                    Player player = Main.player[projectile.owner];
                    float num2 = 0.75f;
                    float LaserLength = MathHelper.Lerp(0, projectile.height, num2);
                    Vector2 Pvelocity = Utils.RotatedBy(projectile.velocity.PerfectNormalize(), 0, default);
                    float num = 0f;
                    return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), player.Center, projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num));


                }
            }
            return base.Colliding(projectile, projHitbox, targetHitbox);
        }

        public override void CutTiles(Projectile projectile)
        {
            if (SwordHitbox || (projectile.type == ModContent.ProjectileType<GlobalSword>() && projectile.Player().ActiveItem().type == ItemID.ZombieArm))
            {
                if (projectile.aiStyle == 161)
                {
                    DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
                    Vector2 start = projectile.Center;
                    Vector2 end = start + projectile.velocity.SafeNormalize(-Vector2.UnitY) * (ExtraLength * projectile.scale + 10);
                    Utils.PlotTileLine(start, end, projectile.width, DelegateMethods.CutTiles);
                }
                else
                {
                    DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
                    Vector2 start = projectile.Center;
                    Vector2 end = start + projectile.velocity.SafeNormalize(-Vector2.UnitY) * (projectile.height + 10);
                    Utils.PlotTileLine(start, end, projectile.width, DelegateMethods.CutTiles);
                }
            }
        }
    }
}