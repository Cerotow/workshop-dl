using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon;
using Terraria;

namespace DDmod.Content.Projectiles
{
    public class SummonProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        /// <summary> 召唤物碰重置 </summary>
        protected bool MinionReset = false;

        /// <summary> 召唤物碰撞箱(防重叠) </summary>
        protected float hitbox = 1f;

        /// <summary> 召唤物碰撞后反弹速度 </summary>
        public float ReboundSpeed = 0;

        /// <summary> 追寻到目标 </summary>
        protected bool target = false;

        /// <summary> 召唤物索敌是否穿墙(如果是否也不会那么傻,因为他会检测玩家和怪物) </summary>
        protected bool IgnoreTile;

        /// <summary> 让你知道召唤物的主人是谁 </summary>
        protected Player player;

        /// <summary> 让你知道召唤物的目标是谁 </summary>
        protected NPC npc;

        /// <summary> 索敌范围 </summary>
        protected float SearchRange = 800;

        /// <summary> 参与旋转静态AI </summary>
        public bool RotateAI;
        public override void SetDefaults(Projectile projectile)
        {
            //小鬼
            if (projectile.type == 375)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
            }
            //双子魔眼
            if (projectile.type == 387)
            {
                projectile.aiStyle = -1;
                ReboundSpeed = 0.5f;
                MinionReset = true;
            }
            if (projectile.type == 388)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
                projectile.usesIDStaticNPCImmunity = false;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
            }

            //史莱姆仆从
            if (projectile.type == 266)
            {
                MinionReset = true;
                projectile.aiStyle = 26;
                projectile.usesIDStaticNPCImmunity = false;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
                projectile.friendly = true;
            }
            //小激光眼激光
            if (projectile.type == 389)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 60;

                projectile.usesIDStaticNPCImmunity = false;
            }
            //蜜蜂
            if (projectile.type == 373)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
            }
            //鲨龙卷
            if (projectile.type == 407)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
            }
            //鲨龙鱼
            if (projectile.type == 408)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
            }
            //致命球
            if (projectile.type == 533)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
                projectile.usesIDStaticNPCImmunity = false;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
            }
            //小雪怪
            if (projectile.type == 951)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
                projectile.usesIDStaticNPCImmunity = false;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
            }
            //矮人
            if (projectile.type >= 191 && projectile.type <= 194)
            {
                ReboundSpeed = 0.5f;
                MinionReset = true;
                projectile.aiStyle = -1;
                projectile.friendly = false;
            }
            if (projectile.type == 309)
            {
                projectile.usesIDStaticNPCImmunity = false;

                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = -1;
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            player = projectile.Player();
            //遍历npc支持碰撞箱
            if (projectile.DamageType == DamageClass.Summon&&player.HasBuff(ModContent.BuffType<星尘共鸣Buff>()))
            {
                if (Main.rand.NextBool(20 * (1 + projectile.extraUpdates)))
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), Main.rand.NextFloat(0.75F, 1.5F));
                }
                if (Main.rand.NextBool(15*(1+projectile.extraUpdates)))
                {
                   int A =  Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), Main.rand.NextFloat(0.5F, 1.25F));
                    Main.dust[A].customData = -1002;
                }
            }
            if (ReboundSpeed != 0)
            {
                float spacing = projectile.width * hitbox;
                for (int k = 0; k < 1000; k++)
                {
                    Projectile otherProj = Main.projectile[k];
                    if (k != projectile.whoAmI && otherProj.active && otherProj.owner == projectile.owner && Math.Abs(projectile.position.X - otherProj.position.X) + Math.Abs(projectile.position.Y - otherProj.position.Y) < spacing)
                    {
                        if (otherProj.GetGlobalProjectile<SummonProjectile>().ReboundSpeed != 0)
                        {
                            if (projectile.position.X < Main.projectile[k].position.X)
                            {
                                projectile.velocity.X -= ReboundSpeed;
                            }
                            else
                            {
                                projectile.velocity.X += ReboundSpeed;
                            }
                            if (projectile.position.Y < Main.projectile[k].position.Y)
                            {
                                projectile.velocity.Y -= ReboundSpeed;
                            }
                            else
                            {
                                projectile.velocity.Y += ReboundSpeed;
                            }
                        }
                    }
                }
            }
            //召唤物索敌系统
            if (MinionReset)
            {
                projectile.tileCollide = false;
                target = false;
                if (player.HasMinionAttackTargetNPC)
                {
                    npc = null;
                    npc = Main.npc[player.MinionAttackTargetNPC];
                    if (IgnoreTile || (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) && !projectile.sentry))
                    {
                        if (Vector2.Distance(npc.Center, projectile.Center) < SearchRange) target = true;
                    }
                    else if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && Vector2.Distance(npc.Center, projectile.Center) < SearchRange)
                    {
                        target = true;
                    }
                }
                if (!target)
                {
                    npc = null;
                    if (!projectile.sentry)
                    {
                        npc = NPCdirection.FindClosest(player.Center, SearchRange, IgnoreTile);
                    }
                    if (npc == null)
                    {
                        npc = NPCdirection.FindClosest(projectile.Center, SearchRange, IgnoreTile);
                    }
                    if (npc != null)
                    {
                        target = true;
                    }
                }
            }
            //小鬼
            if (projectile.type == 375)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].impMinion = false;
                }
                if (Main.player[projectile.owner].impMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 0;
                }
                Dust dust = Main.dust[NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, default, 1.4f)];
                dust.noGravity = true;
                projectile.DProj().Times[0] += 0.25f;
                projectile.rotation = projectile.velocity.X * 0.05f;
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;
                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    projectile.ai[0]++;
                    if (projectile.localAI[0] < projectile.ai[0])
                    {
                        projectile.localAI[0]+=2;
                    }
                    else if(projectile.localAI[0] - projectile.ai[0]>2)
                    {
                        projectile.localAI[0]*=0.86f;
                    }
                    if (projectile.ai[1] <= 3)
                    {
                        if (direction.Length() > 200)
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                        }
                        else
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                        }
                        if (projectile.ai[0] >= 35)
                        {
                            projectile.ai[1]++;

                            if (projectile.owner == Main.myPlayer)
                            {
                                int A = NewProjectile(projectile.GetSource_FromAI(), projectile.Center + direction.PerfectNormalize() * 30, direction * 12, ModContent.ProjectileType<FireBall>(), projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
                                Main.projectile[A].tileCollide = false;
                            }
                            projectile.localAI[0] *= 1.5f;
                            projectile.netUpdate = true;
                            projectile.ai[0] = 0;
                            projectile.velocity = -direction * 5;
                        }
                    }
                    else
                    {
                        if (direction.Length() > 500)
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                        }
                        else
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                        }
                        if (projectile.ai[0] >= 55)
                        {
                            projectile.ai[1] = 0;
                            if (projectile.owner == Main.myPlayer)
                            {
                                int A = NewProjectileChange(projectile.GetSource_FromAI(), projectile.Center + direction.PerfectNormalize() * 30, direction * 24, ModContent.ProjectileType<FireBall>(), (int)(projectile.damage * 1.75f), projectile.knockBack, projectile.owner, 0, 1, 1.75F);
                                Main.projectile[A].scale = 3;
                                Main.projectile[A].tileCollide = false;
                            }
                            projectile.localAI[0] *= 1.5f;
                            projectile.netUpdate = true;
                            projectile.ai[0] = 0;
                            projectile.velocity = -direction * 12;
                        }
                    }
                }
                else
                {
                    projectile.ai[0] = 0;
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            //激光眼
            if (projectile.type == 387)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].twinsMinion = false;
                }
                if (Main.player[projectile.owner].twinsMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
                projectile.DProj().Times[0] += 0.25f;
                projectile.spriteDirection = -1;
                projectile.rotation = projectile.velocity.ToRotation();
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;
                    projectile.rotation = direction.ToRotation();
                    projectile.ai[0]++;

                    if (direction.Length() > 200)
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                    }
                    else
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                    }
                    if (projectile.ai[0] >= 90 && projectile.ai[0] % 10 == 0)
                    {
                        projectile.ai[1]++;
                        if (projectile.owner == Main.myPlayer)
                        {
                            NewProjectile(projectile.GetSource_FromAI(), projectile.Center + direction.PerfectNormalize() * 30, direction * 7, ProjectileID.MiniRetinaLaser, projectile.damage, projectile.knockBack, projectile.owner);
                        }
                        projectile.netUpdate = true;
                        if (projectile.ai[1] > 5)
                        {
                            projectile.ai[0] = 0;
                            projectile.ai[1] = 0;
                        }
                    }
                }
                else
                {
                    projectile.ai[0] = 0;
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            //魔焰眼
            if (projectile.type == 388)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].twinsMinion = false;
                }
                if (Main.player[projectile.owner].twinsMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
                projectile.DProj().Times[0] += 0.25f;
                projectile.spriteDirection = -1;
                projectile.friendly = true;
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;
                    if (projectile.ai[1] > 15)
                    {
                        projectile.rotation = direction.ToRotation();
                        if (direction.Length() > 100)
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                        }
                        else
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 - direction * Speed) / 21;
                        }
                        projectile.ai[0]++;
                        if (projectile.ai[0] % 8 == 0)
                        {
                            Vector2 vector = Utils.RotatedBy(direction, Main.rand.NextFloat(-0.1f, 0.1f), default);
                            if (projectile.owner == Main.myPlayer)
                            {
                                NewProjectileChange(projectile.GetSource_FromAI(), projectile.Center + direction * 16, vector * 8, ModContent.ProjectileType<EyeFire>(), projectile.damage / 2, projectile.knockBack, projectile.owner, projectile.whoAmI, 0, 1);
                            }
                            projectile.netUpdate = true;
                        }
                        if (projectile.ai[0] > 300)
                        {
                            projectile.ai[0] = 0;
                            projectile.ai[1] = 0;
                        }
                    }
                    else
                    {
                        if (projectile.ai[0] > 0 && direction.Length() < 200 && direction.Length() > 150 && projectile.ai[0] < 1000)
                        {
                            projectile.ai[0] = 1000;
                        }
                        projectile.ai[0]++;
                        if (projectile.ai[0] > 1060)
                        {
                            if ((projectile.ai[0] > 1000 && direction.Length() > 50) || projectile.ai[0] > 1100)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.rotation = direction.ToRotation();
                                projectile.velocity = direction * 15;
                                projectile.ai[0] = -20;
                                projectile.ai[1]++;
                                npc.netUpdate = true;
                            }
                        }
                        else if (projectile.ai[0] > 0)
                        {
                            if (projectile.ai[0] > 20)
                                projectile.RotationSpeed(direction.ToRotation(), 0.1F);
                            if (direction.Length() > 200)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                            }
                            else
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 - direction * Speed) / 21;
                            }
                        }
                    }
                }
                else
                {
                    projectile.rotation = projectile.velocity.ToRotation();
                    projectile.ai[0] = 0;
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            
            //史莱姆仆从
            if (projectile.type == 266)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].slime = false;
                }
                if (Main.player[projectile.owner].slime)
                {
                    projectile.timeLeft = 2;
                }
                projectile.frameCounter++;

                if (projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
                projectile.rotation = 0;
                if (projectile.ai[1] == 0)
                {
                    if (projectile.frame > 1)
                    {
                        projectile.frame = 0;
                    }
                }
                else
                {
                    if (projectile.frame < 2 || projectile.frame > 5)
                    {
                        projectile.frame = 2;
                    }
                }
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;
                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    float Speed = Math.Abs(direction.X) / 20;
                    if (Speed > 10)
                    {
                        Speed = 10;
                    }
                    else if (Speed < 2f)
                    {
                        Speed = 2f;
                    }
                    float SpeedY = Math.Abs(direction.Y) / 10;
                    if (SpeedY > 15)
                    {
                        SpeedY = 15;
                    }
                    else if (SpeedY < 6)
                    {
                        SpeedY = 6;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.velocity.Y == 0)
                        {
                            projectile.velocity.X = 0;
                            if ((direction.Y <= projectile.height || Math.Abs(direction.X) > 5))
                            {
                                projectile.velocity.Y = -SpeedY;
                                direction = direction.PerfectNormalize();
                                projectile.velocity.X = direction.X * Speed;
                            }
                        }
                        else
                        {
                            if (projectile.velocity.Y < 5) projectile.velocity.Y += 0.4F;
                        }
                        if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                        {
                            projectile.ai[1] = 1;
                        }
                        ReboundSpeed = 0;
                    }
                    else
                    {
                        if (SpeedY < 12f)
                        {
                            SpeedY = 12f;
                        }
                        projectile.velocity = (projectile.velocity * 20 + direction.PerfectNormalize() * (SpeedY)) / 21;

                        projectile.rotation = projectile.velocity.X * 0.05f;
                        if (npc.active && npc.type > 0)
                        {
                            if ((Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                            {
                                for (int a = 0; a < 7; a++)
                                {
                                    if (Main.tile[(int)npc.Center.X / 16, (int)((projectile.Center.Y + projectile.height) / 16) + a].HasTile)
                                    {
                                        projectile.ai[1] = 0;
                                    }
                                }
                            }
                        }
                        ReboundSpeed = 0.5f;
                    }
                }
                else
                {
                    ReboundSpeed = 0;
                    int A = 0;
                    for (int k = 0; k < 1000; k++)
                    {
                        if (Main.projectile[k].active && Main.projectile[k].type == projectile.type && k <= projectile.whoAmI && projectile.owner == Main.myPlayer)
                        {
                            A++;
                        }
                    }
                    Vector2 direction = player.Center - projectile.Center;
                    direction.X -= ((10 + 40 * A) * player.direction);
                    float Speed = Math.Abs(direction.X) / 50;
                    if (Speed > 4)
                    {
                        Speed = 4;
                    }
                    else if (Speed < 0.5f)
                    {
                        Speed = 0.5f;
                    }
                    float SpeedY = Math.Abs(direction.X + direction.Y) / 50;
                    if (SpeedY > 15)
                    {
                        SpeedY = 15;
                    }
                    else if (SpeedY < 6)
                    {
                        SpeedY = 6;
                    }
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.velocity.Y == 0)
                        {
                            projectile.velocity.X = 0;
                            if ((direction.Y > 50 || Math.Abs(direction.X) > 5))
                            {
                                projectile.velocity.Y = -SpeedY;
                                direction = direction.PerfectNormalize();
                                projectile.velocity.X = direction.X * Speed;
                            }
                            else
                            {
                                if (direction.X > 0)
                                {
                                    projectile.spriteDirection = -1;
                                }
                                else
                                {
                                    projectile.spriteDirection = 0;
                                }
                            }
                        }
                        else
                        {
                            if (projectile.velocity.Y < 5) projectile.velocity.Y += 0.4F;
                        }
                        if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                        {
                            projectile.ai[1] = 1;
                        }
                    }
                    else
                    {
                        projectile.velocity = (projectile.velocity * 20 + direction.PerfectNormalize() * (SpeedY)) / 21;

                        projectile.rotation = projectile.velocity.X * 0.05f;
                        if (player.velocity.Y == 0 && (Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                        {
                            for (int a = 0; a < 7; a++)
                            {
                                if (Main.tile[(int)((player.Center.X - ((10 + 40 * A) * player.direction)) / 16), (int)((projectile.Center.Y + projectile.height) / 16) + a].HasTile)
                                {
                                    projectile.ai[1] = 0;
                                }
                            }
                        }
                    }
                }
                projectile.tileCollide = projectile.ai[1] == 0;
                return false;
            }
            //鲨龙卷
            if (projectile.type == 407)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].sharknadoMinion = false;
                }
                if (Main.player[projectile.owner].sharknadoMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
                projectile.spriteDirection = -1;
                projectile.rotation = projectile.velocity.X * 0.05f;
                projectile.ai[0]++;
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;

                    if (direction.Length() > 300)
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                    }
                    else
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                    }
                    if (projectile.ai[0] >= 60)
                    {
                        if (projectile.owner == Main.myPlayer)
                        {
                            NewProjectile(projectile.GetSource_FromAI(), projectile.Center, direction * 7, ProjectileID.MiniSharkron, projectile.damage, projectile.knockBack, projectile.owner);
                        }
                        projectile.netUpdate = true;
                        projectile.ai[0] = 0;
                    }
                }
                else
                {
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            //蜜蜂
            if (projectile.type == 373)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].hornetMinion = false;
                }
                if (Main.player[projectile.owner].hornetMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
                projectile.rotation = projectile.velocity.X * 0.05f;
                projectile.ai[0]++;
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center + new Vector2(0, 10);

                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    if (direction.Length() > 300)
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                    }
                    else
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                    }
                    if (projectile.ai[0] >= 60)
                    {
                        for (int A = 0; A < 3; A++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(direction, Main.rand.NextFloat(-0.1F, 0.1F), default);

                            if (projectile.owner == Main.myPlayer)
                                NewProjectile(projectile.GetSource_FromAI(), projectile.Center + new Vector2(0, 10), projDirection * 20, ProjectileID.HornetStinger, projectile.damage, projectile.knockBack, projectile.owner);
                        }
                        projectile.netUpdate = true;
                        projectile.ai[0] = 0;
                    }
                }
                else
                {
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            //鲨龙鱼
            if (projectile.type == 408)
            {
                float Speed = 18;

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
                projectile.alpha -= 5;
                if (target && projectile.ai[0] < 60)
                {
                    Vector2 direction = npc.Center - projectile.Center;
                    projectile.rotation = direction.ToRotation();
                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = 0;
                    }
                    else
                    {
                        projectile.spriteDirection = 1;
                    }
                    if (direction.Length() < 250)
                    {
                        projectile.ai[0]++;
                        if (projectile.ai[0] % 10 == 0)
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                int A = NewProjectile(projectile.GetSource_FromAI(), projectile.Center, direction.PerfectNormalize() * 5 + projectile.velocity / 3, ModContent.ProjectileType<SummonBubble>(), projectile.damage / 3, projectile.knockBack, projectile.owner, 0, 1);
                                Main.projectile[A].tileCollide = false;
                                Main.projectile[A].DProj().Magnification = 0.3333F;
                            }
                            projectile.netUpdate = true;
                        }
                    }

                    if (direction.Length() > 200)
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                    }
                    else
                    {
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                    }
                }
                else
                {
                    projectile.ai[0] = 60;
                    NPCdirection.Track(projectile, 1000, 20, 25);
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = 0;
                    }
                    else
                    {
                        projectile.spriteDirection = 1;
                    }
                    projectile.rotation = projectile.velocity.ToRotation();
                }
                return false;
            }
            //致命球
            if (projectile.type == 533)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].DeadlySphereMinion = false;
                }
                if (Main.player[projectile.owner].DeadlySphereMinion)
                {
                    projectile.timeLeft = 2;
                }
                float Speed = 12;
                float DistanceNPC = (player.Center - projectile.Center).Length();

                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.ai[1] <= 5)
                {
                    if (projectile.frame >= 10)
                    {
                        projectile.frame = 5;
                    }
                    else if (projectile.frame < 5)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                }
                else if (projectile.ai[1] <= 6)
                {
                    if (projectile.frame <= 10)
                    {
                        projectile.frame = 11;
                    }
                    if (projectile.frame >= 17)
                    {
                        projectile.frame = 14;
                    }
                    else if (projectile.frame < 14)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                }
                else if (projectile.ai[1] <= 11)
                {
                    if (projectile.frame <= 16)
                    {
                        projectile.frame = 17;
                    }
                    if (projectile.frame >= 21)
                    {
                        projectile.frame = 17;
                    }
                }
                if (projectile.ai[1] > 6)
                {

                    for (int A = 0; A < 4; A++)
                    {
                        int DustID = Utils.SelectRandom(Main.rand, 226, 228, 75);
                        Dust dust = Main.dust[NewDust(projectile.Center, 0, 0, DustID)];

                        Vector2 value13 = Vector2.One.RotatedBy(A * ((float)Math.PI / 2f)).RotatedBy(projectile.rotation);
                        dust.position = projectile.Center + value13 * 10f;
                        dust.velocity = value13 * 1f;
                        dust.scale = 0.6f + Main.rand.NextFloat() * 0.5f;
                        dust.noGravity = true;
                    }
                }
                if (projectile.localAI[0] < 12)
                {
                    projectile.localAI[0]++;
                    projectile.velocity = Vector2.Zero;
                }
                projectile.DProj().Times[0] += 0.25f;
                projectile.spriteDirection = -1;
                projectile.friendly = true;
                if (target)
                {
                    Vector2 direction = npc.Center - projectile.Center;

                    if (projectile.ai[1] <= 5)
                    {
                        projectile.localNPCHitCooldown = 60;
                        projectile.rotation += projectile.velocity.Length() * 0.08F;
                        if (projectile.ai[0] > 0 && direction.Length() < 200 && direction.Length() > 150 && projectile.ai[0] < 1000)
                        {
                            projectile.ai[0] = 1000;
                        }
                        projectile.ai[0]++;
                        if (projectile.ai[0] > 1060)
                        {
                            if ((projectile.ai[0] > 1000 && direction.Length() > 50) || projectile.ai[0] > 1100)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.rotation = direction.ToRotation();
                                if (projectile.ai[1] < 5)
                                {
                                    projectile.velocity = direction * 40;
                                }
                                else
                                {
                                    projectile.localAI[0] = 0;
                                }
                                projectile.ai[0] = 0;
                                projectile.ai[1]++;
                                npc.netUpdate = true;
                            }
                        }
                        else if (projectile.ai[0] > 0)
                        {
                            if (direction.Length() > 200)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                            }
                            else
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 - direction * Speed) / 21;
                            }
                        }
                    }
                    else if (projectile.ai[1] <= 6)
                    {
                        projectile.localNPCHitCooldown = 15;
                        projectile.rotation += 0.3F;
                        direction = direction.PerfectNormalize();
                        projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                        projectile.ai[0]++;
                        if (projectile.ai[0] > 180)
                        {
                            projectile.ai[1]++;
                            projectile.ai[0] = 0;
                            projectile.localAI[0] = 0;
                        }
                    }
                    else
                    {
                        projectile.localNPCHitCooldown = 60;
                        projectile.rotation += projectile.velocity.Length() * 0.08F;
                        if (projectile.ai[0] > 0 && direction.Length() < 200 && direction.Length() > 150 && projectile.ai[0] < 1000)
                        {
                            projectile.ai[0] = 1000;
                        }
                        projectile.ai[0]++;
                        if (projectile.ai[0] > 1060)
                        {
                            if ((projectile.ai[0] > 1000 && direction.Length() > 50) || projectile.ai[0] > 1100)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.rotation = direction.ToRotation();
                                projectile.velocity = direction * 40;
                                projectile.ai[1]++;
                                projectile.ai[0] = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else if (projectile.ai[0] > 0)
                        {
                            if (direction.Length() > 200)
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                            }
                            else
                            {
                                direction = direction.PerfectNormalize();
                                projectile.velocity = (projectile.velocity * 20 - direction * Speed) / 21;
                            }
                        }
                    }
                    if (projectile.ai[1] >= 12)
                    {
                        projectile.localAI[0] = 0;
                        projectile.ai[1] = 0;
                    }
                }
                else
                {
                    projectile.rotation += projectile.velocity.Length() * 0.08F;
                    projectile.ai[0] = 0;
                    Vector2 direction = player.Center - projectile.Center;
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            projectile.Center = player.Center;
                        }
                        if (projectile.velocity.Length() <= 5)
                        {
                            projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
                return false;
            }
            //小雪怪
            if (projectile.type == 951)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].flinxMinion = false;
                }
                if (Main.player[projectile.owner].flinxMinion)
                {
                    projectile.timeLeft = 2;
                }
                projectile.friendly = target;
                projectile.rotation = 0;
                if (projectile.ai[1] == 1)
                {
                    projectile.DProj().Times[1]++;

                    if (projectile.DProj().Times[1] > 4)
                    {
                        projectile.DProj().Times[1] = 0;
                        projectile.frame++;
                    }
                    if (projectile.frame > 2)
                    {
                        projectile.frame = 0;
                    }
                }
                else
                {
                    projectile.DProj().Times[1] += Math.Abs(projectile.velocity.X);

                    if (projectile.DProj().Times[1] > 8)
                    {
                        projectile.DProj().Times[1] -= 8;
                        projectile.frame++;
                    }
                    if (projectile.frame > 11)
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.tileCollide = projectile.ai[1] == 0;
                if (target)
                {
                    ReboundSpeed = 0.5f;
                    Vector2 direction = npc.Center - projectile.Center;
                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = 0;
                    }
                    else
                    {
                        projectile.spriteDirection = -1;
                    }
                    float Speed = Math.Abs(direction.X) / 20;
                    if (Speed > 10)
                    {
                        Speed = 10;
                    }
                    else if (Speed < 2f)
                    {
                        Speed = 2f;
                    }
                    float SpeedY = Math.Abs(direction.X + direction.Y) / 50;
                    if (SpeedY > 25)
                    {
                        SpeedY = 25;
                    }
                    else if (SpeedY < 6)
                    {
                        SpeedY = 6;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.velocity.Y == 0)
                        {
                            if (direction.Y < -100 || (Math.Abs(direction.X) > 5 && projectile.velocity.X == 0))
                            {
                                projectile.velocity.Y = -12;
                            }
                        }
                        if (projectile.velocity.Y < 10) projectile.velocity.Y += 0.4F;
                        if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                        {
                            projectile.ai[1] = 1;
                        }
                        if (direction.Y > projectile.height / 2)
                        {
                            projectile.tileCollide = false;
                        }
                        direction = direction.PerfectNormalize();
                        projectile.velocity.X = direction.X * Speed;

                        if (projectile.velocity.X > 0)
                        {
                            projectile.spriteDirection = 0;
                        }
                        else
                        {
                            projectile.spriteDirection = -1;
                        }
                    }
                    else
                    {
                        if (SpeedY < 18f)
                        {
                            SpeedY = 18f;
                        }
                        projectile.velocity = (projectile.velocity * 20 + direction.PerfectNormalize() * (SpeedY)) / 21;

                        projectile.rotation = projectile.velocity.X * 0.05f;
                        if ((Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                        {
                            for (int a = 0; a < 7; a++)
                            {
                                if (Main.tile[(int)npc.Center.X / 16, (int)((projectile.Center.Y + projectile.height) / 16) + a].HasTile)
                                {
                                    projectile.ai[1] = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ReboundSpeed = 0;
                    int A = 0;
                    for (int k = 0; k < 1000; k++)
                    {
                        if (Main.projectile[k].active && Main.projectile[k].type == projectile.type && k <= projectile.whoAmI && projectile.owner == Main.myPlayer)
                        {
                            A++;
                        }
                    }
                    Vector2 direction = player.Center - projectile.Center;
                    direction.X -= ((10 + 80 * A) * player.direction);
                    float Speed = Math.Abs(direction.X) / 50;
                    if (Speed > 12)
                    {
                        Speed = 12;
                    }
                    else if (Speed < 5f)
                    {
                        Speed = 5f;
                    }
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = 0;
                    }
                    else
                    {
                        projectile.spriteDirection = -1;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.velocity.Y == 0)
                        {
                            if (direction.Y < -100 || (Math.Abs(direction.X) > 5 && projectile.velocity.X == 0))
                            {
                                projectile.velocity.Y = -12;
                            }
                        }
                        if (projectile.velocity.Y < 10) projectile.velocity.Y += 0.4F;
                        if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                        {
                            projectile.ai[1] = 1;
                        }
                        if (direction.Length() <= 8)
                        {
                            projectile.frame = 0;
                        }
                        direction = direction.PerfectNormalize();
                        projectile.velocity.X = direction.X * Speed;

                        if (projectile.velocity.Length() < 2)
                        {
                            if (player.direction == 1)
                            {
                                projectile.spriteDirection = 0;
                            }
                            else
                            {
                                projectile.spriteDirection = -1;
                            }
                        }
                        if (direction.Y > projectile.height / 2)
                        {
                            projectile.tileCollide = false;
                        }
                    }
                    else
                    {
                        if (projectile.velocity.Length() < 2)
                        {
                            if (player.direction == 1)
                            {
                                projectile.spriteDirection = 0;
                            }
                            else
                            {
                                projectile.spriteDirection = -1;
                            }
                        }
                        projectile.velocity = (projectile.velocity * 20 + direction.PerfectNormalize() * 12) / 21;

                        projectile.rotation = projectile.velocity.X * 0.05f;
                        if (player.velocity.Y == 0 && (Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                        {
                            for (int a = 0; a < 7; a++)
                            {
                                if (Main.tile[(int)((player.Center.X - ((10 + 80 * A) * player.direction)) / 16), (int)((projectile.Center.Y + projectile.height) / 16) + a].HasTile)
                                {
                                    projectile.ai[1] = 0;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            //矮人
            if (projectile.type >= 191 && projectile.type <= 194)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].pygmy = false;
                }
                if (Main.player[projectile.owner].pygmy)
                {
                    projectile.timeLeft = 2;
                }
                projectile.friendly = target;
                projectile.rotation = 0;
                if (projectile.ai[1] == 1)
                {
                    if (!target && projectile.frame < 12)
                    {
                        projectile.frame = Main.rand.Next(12, 18);
                    }
                    if (target && projectile.ai[0] > 0)
                    {
                        if (projectile.ai[0] < 5)
                        {
                            projectile.frame = 1;
                        }
                        else if (projectile.ai[0] < 10)
                        {
                            projectile.frame = 2;
                        }
                        else
                        {
                            projectile.frame = 3;
                        }
                    }
                    else if (projectile.frame < 12)
                    {
                        projectile.frame = 0;
                    }
                }
                else
                {
                    projectile.DProj().Times[1] += Math.Abs(projectile.velocity.X);

                    if (projectile.DProj().Times[1] > 8)
                    {
                        projectile.DProj().Times[1] -= 8;
                        projectile.frame++;
                    }
                    if (!target || projectile.ai[0] == 0)
                    {
                        if (projectile.frame > 11)
                        {
                            projectile.frame = 5;
                        }
                        if (projectile.frame < 5)
                        {
                            projectile.frame = 5;
                        }
                        if (projectile.velocity.Y != 0)
                        {
                            projectile.frame = 4;
                        }
                        else if (Math.Abs(projectile.velocity.X) <= 0.01f)
                        {
                            projectile.frame = 0;
                        }
                    }
                    else
                    {
                        if (projectile.ai[0] < 10)
                        {
                            projectile.frame = 1;
                        }
                        else if (projectile.ai[0] < 20)
                        {
                            projectile.frame = 2;
                        }
                        else
                        {
                            projectile.frame = 3;
                        }
                    }
                }
                projectile.tileCollide = projectile.ai[1] == 0;
                if (target)
                {
                    ReboundSpeed = 0.5f;
                    Vector2 direction = npc.Center - projectile.Center;
                    if (direction.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    float Speed = Math.Abs(direction.X) / 20;
                    if (Speed > 10)
                    {
                        Speed = 10;
                    }
                    else if (Speed < 2f)
                    {
                        Speed = 2f;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        if (direction.Length() > 300)
                        {
                            projectile.ai[0] = 0;
                            direction = direction.PerfectNormalize();
                            projectile.velocity.X = (projectile.velocity.X * 20 + direction.X * Speed) / 21;
                        }
                        else if (direction.Length() < 30)
                        {
                            projectile.ai[0] = 0;
                            direction = direction.PerfectNormalize();
                            projectile.velocity.X = (projectile.velocity.X * 20 + -direction.X * Speed) / 21;
                        }
                        else
                        {
                            projectile.ai[0]++;
                            if (projectile.ai[0] == 20)
                            {
                                if (projectile.owner == Main.myPlayer)
                                {
                                    int A = NewProjectile(projectile.GetSource_FromAI(), projectile.Center - new Vector2(0, 10), direction.PerfectNormalize() * 15, 195, projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
                                    Main.projectile[A].tileCollide = false;
                                }
                                projectile.netUpdate = true;
                            }
                            if (projectile.ai[0] >= 30)
                            {
                                projectile.ai[0] = 1;
                                projectile.netUpdate = true;
                            }
                            projectile.velocity *= 0.9f;
                        }
                        if (projectile.velocity.Y < 10) projectile.velocity.Y += 0.4F;
                        if (!Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            projectile.ai[1] = 1;
                        }
                    }
                    else
                    {
                        if (direction.Length() < 500)
                        {
                            projectile.ai[0]++;
                            if (projectile.ai[0] == 20)
                            {
                                if (projectile.owner == Main.myPlayer)
                                {
                                    int A = NewProjectile(projectile.GetSource_FromAI(), projectile.Center - new Vector2(0, 10), direction.PerfectNormalize() * 15, 195, projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
                                    Main.projectile[A].tileCollide = false;
                                }
                                projectile.netUpdate = true;
                            }
                            if (projectile.ai[0] >= 30)
                            {
                                projectile.ai[0] = 1;
                                projectile.netUpdate = true;
                            }

                            for (int a = 0; a < 7; a++)
                            {
                                if (Main.tile[(int)npc.Center.X / 16, (int)((npc.Center.Y + npc.height) / 16) + a].HasTile && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                                {
                                    projectile.ai[1] = 0;
                                }
                            }
                        }
                        else
                        {
                            projectile.ai[0] = 0;
                        }
                        if (direction.Length() > 300)
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + direction * Speed) / 21;
                        }
                        else
                        {
                            direction = direction.PerfectNormalize();
                            projectile.velocity = (projectile.velocity * 20 + -direction * Speed) / 21;
                        }
                    }
                }
                else
                {
                    int A = 0;
                    for (int k = 0; k < 1000; k++)
                    {
                        if (Main.projectile[k].active && Main.projectile[k].type >= 191 && Main.projectile[k].type <= 194 && k <= projectile.whoAmI && projectile.owner == Main.myPlayer)
                        {
                            A++;
                        }
                    }
                    Vector2 direction = player.Center - projectile.Center;
                    float Speed = Math.Abs(direction.X) / 50;
                    if (Speed > 12)
                    {
                        Speed = 12;
                    }
                    else if (Speed < 5f)
                    {
                        Speed = 5f;
                    }
                    if (projectile.velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 0;
                    }
                    if (projectile.ai[1] == 0)
                    {
                        ReboundSpeed = 0;
                        direction.X -= ((10 + 80 * A) * player.direction);
                        if (projectile.velocity.Y == 0)
                        {
                            if (direction.Y < -100 || (Math.Abs(direction.X) > 5 && projectile.velocity.X == 0))
                            {
                                projectile.velocity.Y = -12;
                            }
                        }
                        if (projectile.velocity.Y < 10) projectile.velocity.Y += 0.4F;
                        if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                        {
                            projectile.ai[1] = 1;
                        }
                        direction = direction.PerfectNormalize();
                        projectile.velocity.X = direction.X * Speed;

                        if (projectile.velocity.Length() < 2)
                        {
                            if (player.direction == 1)
                            {
                                projectile.spriteDirection = -1;
                            }
                            else
                            {
                                projectile.spriteDirection = 0;
                            }
                        }
                        if (direction.Y > projectile.height / 2)
                        {
                            projectile.tileCollide = false;
                        }
                    }
                    else
                    {
                        projectile.rotation = projectile.velocity.X * 0.03F;
                        ReboundSpeed = 0.5F;
                        float DistanceNPC = (player.Center - projectile.Center).Length();
                        if (player.velocity.Y == 0)
                        {
                            direction = player.Center - projectile.Center;
                            direction.X -= ((10 + 80 * A) * player.direction);
                            if (projectile.velocity.Length() < 2)
                            {
                                if (player.direction == 1)
                                {
                                    projectile.spriteDirection = -1;
                                }
                                else
                                {
                                    projectile.spriteDirection = 0;
                                }
                            }
                            projectile.velocity = (projectile.velocity * 20 + direction.PerfectNormalize() * 12) / 21;

                            projectile.rotation = projectile.velocity.X * 0.05f;
                            if (player.velocity.Y == 0 && (Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                            {
                                for (int a = 0; a < 7; a++)
                                {
                                    if (Main.tile[(int)((player.Center.X - ((10 + 80 * A) * player.direction)) / 16), (int)((projectile.Center.Y + projectile.height) / 16) + a].HasTile)
                                    {
                                        projectile.ai[1] = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (player.velocity.Length() > 20 || direction.Length() > 300)
                            {
                                direction = player.Center - new Vector2(0, 100) - projectile.Center;
                                float speed = Speed + player.velocity.Length();
                                projectile.netUpdate = true;
                                if (DistanceNPC > 3000f)
                                {
                                    projectile.Center = player.Center;
                                }
                                direction = direction.PerfectNormalize();
                                direction *= speed;
                                float temp = 60;
                                projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                            }
                            else
                            {
                                float speed = 2f;
                                projectile.netUpdate = true;
                                direction.Y -= 350f;
                                if (DistanceNPC > 3000f)
                                {
                                    projectile.Center = player.Center;
                                }
                                if (projectile.velocity.Length() <= 5)
                                {
                                    projectile.velocity.X *= 1.02f;
                                }
                                if (direction.Length() > 300f)
                                {
                                    direction.Normalize();
                                    direction *= speed;
                                    float temp = 10;
                                    projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                                }
                            }
                        }
                    }
                }
                return false;
            }
            return base.PreAI(projectile);
        }
        public override bool MinionContactDamage(Projectile projectile)
        {
            //魔焰眼
            if (projectile.type == 388)
            {
                return true;
            }
            //致命球
            if (projectile.type == 533)
            {
                return true;
            }
            if (projectile.type == 266)
            {
                return true;
            }
            return base.MinionContactDamage(projectile);
        }
        public override bool? CanDamage(Projectile projectile)
        {
            return base.CanDamage(projectile);
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            if(projectile.DamageType==DamageClass.Summon)
            {
                if (player.HasBuff(ModContent.BuffType<星尘共鸣Buff>()))
                {
                    modifiers.SourceDamage *= 1.1F;
                    if (Main.rand.Next(100) < 15)
                    {
                        modifiers.SetCrit();
                        NewDustChangeRound3(4, target.Center, 1, ModContent.DustType<星光粒子>(), 0, 9, true, 1.25F, 1.75F, 100, new Color(40, 185, 255, 0), 1);
                        NewDustChangeRound3(50, target.Center, 1, ModContent.DustType<光球粒子>(), 0, 4, true, 1.25F, 2.75F, 100, new Color(40, 185, 255, 0), -new Dust().DustAI(1) - 4);

                        target.AddBuff(ModContent.BuffType<星尘共鸣Buff>(), 180);
                    }
                }
                if (player.HasBuff(ModContent.BuffType<AcornMarker>()))
                {
                    modifiers.FinalDamage.Base += 2;
                    modifiers.FinalDamage += 0.1F;
                }
            }
            if (projectile.DamageType == DamageClass.Summon)
            {
                if (target.Dnpc().MeteorMarker)
                {
                    modifiers.FinalDamage += 0.1f;
                }
                if (target.HasBuff(ModContent.BuffType<永恒诅咒>()))
                {
                    modifiers.FinalDamage += 0.15f;
                }
                if (target.HasBuff(ModContent.BuffType<星尘共鸣Buff>()))
                {
                    modifiers.FinalDamage.Base += 8;
                    modifiers.ArmorPenetration += 20;
                }
            }

            //史莱姆仆从
            if (projectile.type == 266)
            {
                if (projectile.ai[1] == 0)
                {
                    modifiers.SourceDamage *= 1.75F;
                }
            }
            //小雪怪
            if (projectile.type == 951)
            {
                projectile.ai[1] = 0;
            }
            //致命球
            if (projectile.type == 533)
            {
                if (projectile.ai[1] <= 5)
                {
                    modifiers.SourceDamage *= 2.5F;
                }
                if (projectile.ai[1] > 6)
                {
                    modifiers.SourceDamage *= 1.75F;
                    target.AddBuff(ModContent.BuffType<Charged>(), 300);
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if(projectile.type == 309)
            {
                target.AddBuff(ModContent.BuffType<Freeze>(),300);
            }
            
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            //小激光眼激光
            if (projectile.type == 389)
            {
                for (int A = 0; A < Main.rand.Next(3, 7); A++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center - projectile.velocity / 2f, 0, 0, DustID.TheDestroyer, 0f, 0f, 100, default, 1.6f)];
                    dust.velocity *= 1.5f;
                    dust.noGravity = true;
                }
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            //小鬼
            if (projectile.type == 375)
            {
                SpriteEffects sprite = 0;
                if (projectile.spriteDirection == -1)
                {
                    sprite = (SpriteEffects)1;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[projectile.type];
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[projectile.type] * projectile.frame, texture.Width, texture.Height / Main.projFrames[projectile.type])), lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, sprite, 0f);

                for (int A = 0; A < 2; A++)
                {
                    if (target)
                    {
                        Vector2 direction = npc.Center - projectile.Center;

                        Color color = new Color(253, 62, 3, 0);
                        Vector2 vector2 = projectile.Center - Main.screenPosition + direction.PerfectNormalize() * 30;
                        Main.spriteBatch.Draw(VoidStar, vector2, null, color, direction.ToRotation(), VoidStar.Size() / 2, new Vector2(projectile.localAI[0] / 100, projectile.localAI[0] / 50f), 0, 0f);
                        texture = DDTextures.Circle[9].Value;
                        DDHelper.Compression(texture, color, direction.ToRotation(), projectile.Opacity, new Vector2(1.5f + projectile.localAI[0] / 60, 1), 1, projectile.DProj().Times[0], BlendState.Additive);
                        DDHelper.Compression(texture, color, direction.ToRotation(), projectile.Opacity, new Vector2(1.5f + projectile.localAI[0] / 60, 1), 1, projectile.DProj().Times[0], BlendState.Additive);

                        Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, projectile.localAI[0] / 60/4, 0, 0);
                        Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, projectile.localAI[0] / 60/4, 0, 0);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                }
                return false;
            }

            //小激光眼
            if (projectile.type == 387)
            {
                Texture2D texture = (Texture2D)TextureAssets.Projectile[projectile.type];
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[projectile.type] * projectile.frame, texture.Width, texture.Height / Main.projFrames[projectile.type])), lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, (SpriteEffects)1, 0f);
                Main.spriteBatch.Draw(TextureAssets.EyeLaserSmall.Value, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[projectile.type] * projectile.frame, texture.Width, texture.Height / Main.projFrames[projectile.type])), Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, (SpriteEffects)1, 0f);
                texture = DDTextures.Starlight.Value;
                if (projectile.ai[0] >= 80 && projectile.ai[0] % 10 >= 8)
                {
                    Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + (projectile.rotation.ToRotationVector2().PerfectNormalize() * 30), null, new Color(255, 0, 0, 0), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(projectile.scale, projectile.scale / 2), 0, 0f);
                    Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + (projectile.rotation.ToRotationVector2().PerfectNormalize() * 30), null, new Color(0, 255, 255, 0), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(projectile.scale, projectile.scale / 2) / 2, 0, 0f);
                    Main.spriteBatch.Draw(VoidStar, projectile.Center - Main.screenPosition + (projectile.rotation.ToRotationVector2().PerfectNormalize() * 30), null, new Color(255, 0, 0, 0) * 0.5f, projectile.rotation, new Vector2(VoidStar.Width / 2, VoidStar.Height / 2), new Vector2(projectile.scale, projectile.scale / 2), 0, 0f);
                }
                return false;
            }
            //矮人
            if (projectile.type >= 191 && projectile.type <= 194)
            {
                SpriteEffects sprite = 0;
                if (projectile.spriteDirection == -1)
                {
                    sprite = (SpriteEffects)1;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[projectile.type];
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition - new Vector2(0, 10), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[projectile.type] * projectile.frame, texture.Width, texture.Height / Main.projFrames[projectile.type])), lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[projectile.type] / 2), projectile.scale, sprite, 0f);
                return false;
            }
            //小激光眼激光
            if (projectile.type == 389)
            {
                Texture2D texture = (Texture2D)TextureAssets.Projectile[projectile.type];
                Main.spriteBatch.Draw(texture, projectile.position + new Vector2(texture.Width / 2, 0) - Main.screenPosition, null, projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture.Width / 2, 0), projectile.scale, 0, 0f);

                return false;
            }
            //鲨龙鱼
            if (projectile.type == 408)
            {
                float rotation = projectile.rotation;
                if (projectile.spriteDirection == 1)
                {
                    rotation += MathHelper.Pi;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[projectile.type];

                for (int i = 0; i < projectile.oldPos.Length / 2; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] + new Vector2(projectile.width, projectile.height) / 2 - Main.screenPosition;
                    Color color = new Color(0, 55, 255, 255) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length / 4f);
                    Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, TextureAssets.Projectile[projectile.type].Height() / Main.projFrames[projectile.type] * projectile.frame, TextureAssets.Projectile[projectile.type].Width(), TextureAssets.Projectile[projectile.type].Height() / 2)), color, rotation, new Vector2(texture.Width, texture.Height / 2) / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Projectile[projectile.type].Height() / Main.projFrames[projectile.type] * projectile.frame, TextureAssets.Projectile[projectile.type].Width(), TextureAssets.Projectile[projectile.type].Height() / 2)), Color.White, rotation, new Vector2(texture.Width, texture.Height / 2) / 2, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            //小雪怪
            if (projectile.type == 951)
            {
                if (projectile.ai[1] == 1)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DDmod/Image/云").Value;
                    Vector2 vector = projectile.Center - Main.screenPosition;
                    vector.Y += projectile.height / 1.5f;
                    Main.spriteBatch.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 6), projectile.scale * 2, (SpriteEffects)1, 0f);
                }
            }
            //矮人
            if (projectile.type >= 191 && projectile.type <= 194)
            {
                SpriteEffects sprite = 0;
                if (projectile.spriteDirection == -1)
                {
                    sprite = (SpriteEffects)1;
                }
                if (projectile.ai[1] == 1 && target && projectile.ai[0] > 0)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DDmod/Image/长矛").Value;
                    Vector2 vector = projectile.Center - Main.screenPosition;
                    vector.Y += projectile.height * 0.75f;
                    Main.spriteBatch.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, sprite, 0f);
                }
            }
            //致命球
            if (projectile.type == 533)
            {
                if (projectile.localAI[0] < 12)
                {
                    Color color;
                    if (projectile.ai[1] <= 5)
                    {
                        color = new Color(255, 0, 0, 0);
                    }
                    else if (projectile.ai[1] <= 6)
                    {
                        color = new Color(155, 55, 55, 0);
                    }
                    else
                    {
                        color = new Color(100, 150, 255, 0);
                    }
                    Main.spriteBatch.Draw(VoidStar, projectile.Center - Main.screenPosition, null, color * 0.5f, projectile.rotation, new Vector2(VoidStar.Width / 2, VoidStar.Height / 2), new Vector2(projectile.scale, projectile.scale) * projectile.localAI[0] / 4, 0, 0f);
                    Main.spriteBatch.Draw(VoidStar, projectile.Center - Main.screenPosition, null, color * 0.5f, projectile.rotation, new Vector2(VoidStar.Width / 2, VoidStar.Height / 2), new Vector2(projectile.scale, projectile.scale) * projectile.localAI[0] / 8, 0, 0f);

                }
            }
        }
    }
}