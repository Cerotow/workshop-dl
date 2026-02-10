using DDmod.Content.Items;
using DDmod.Content.Projectiles;
using DDmod.Players;
using Terraria;

namespace DDmod.Helper
{
    public static class DDProj
    {
        public static int NewProjectileChange(IEntitySource spawnSource, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0, float ai1 = 0, float Magnification = 1)
        {
            if (Owner == -1)
            {
                Owner = Main.myPlayer;
            }
            int proj = NewProjectile(spawnSource, position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1);
            Main.projectile[proj].DProj().Magnification = Magnification;
            return proj;
        }
        public static int NewProjectileChange(this Projectile projectile, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner =-1, float ai0 = 0, float ai1 = 0, float ai2 =0, float Magnification = 1)
        {
            if (projectile.owner == Main.myPlayer)
            {
                if(Owner==-1)
                {
                    Owner = Main.myPlayer;
                }
                int proj = NewProjectile(projectile.GetSource_FromAI(), position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
                Main.projectile[proj].DProj().Magnification = Magnification;
                return proj;
            }
            return -1;
        }

        /// <summary>玩家距离实体物块：Detectiondistance最大检测距离,Minusdistance减去距离 </summary>
        public static float SolidTileDistanceDetection(this Projectile projectile, float Detectiondistance, float Minusdistance)
        {
            
            float SolidBlockDistance = 0;
            float[] array = new float[160];
            Collision.LaserScan(projectile.Player().MountedCenter, projectile.velocity, 16f, Detectiondistance, array);

            for (int i = 0; i < array.Length; i++)
            {
                SolidBlockDistance += array[i];
            }

            SolidBlockDistance /= array.Length;

            SolidBlockDistance -= Minusdistance;

            return SolidBlockDistance;
            /*
            float i =projectile.Center.X/16;
            float j =projectile.Center.Y/16;
            float r = -Minusdistance;
            for (int a = 0; a < Detectiondistance / 16; a++)
            {
                if (Main.tile[(int)i, (int)j].HasTile && Main.tileSolid[Main.tile[(int)i, (int)j].TileType] && !Main.tileSolidTop[Main.tile[(int)i, (int)j].TileType])
                {
                    break;
                }
                r += 16;
                i += projectile.velocity.PerfectNormalize().X;
                j += projectile.velocity.PerfectNormalize().Y;
            }
            return r;*/
        }
        public static float SolidTileDistanceDetection2(this Projectile projectile, float Detectiondistance, float Minusdistance)
        {
           if(Minusdistance==0)
            {
                Minusdistance = 16;
            }
            float SolidBlockDistance = 0;
            float[] array = new float[2];
            Collision.LaserScan(projectile.Center, projectile.velocity, Minusdistance, Detectiondistance, array);
            for (int i = 0; i < array.Length; i++)
            {
                SolidBlockDistance += array[i];
            }

            SolidBlockDistance /= array.Length;

            //SolidBlockDistance -= Minusdistance;

            return SolidBlockDistance;
            /*
            float i =projectile.Center.X/16;
            float j =projectile.Center.Y/16;
            float r = -Minusdistance;
            for (int a = 0; a < Detectiondistance / 16; a++)
            {
                if (Main.tile[(int)i, (int)j].HasTile && Main.tileSolid[Main.tile[(int)i, (int)j].TileType] && !Main.tileSolidTop[Main.tile[(int)i, (int)j].TileType])
                {
                    break;
                }
                r += 16;
                i += projectile.velocity.PerfectNormalize().X;
                j += projectile.velocity.PerfectNormalize().Y;
            }
            return r;*/
        }
        public static DDGlobalProjectile DProj(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<DDGlobalProjectile>();
        }
        public static MeleeProjectile MeleeProj(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<MeleeProjectile>();
        }
        /// <summary>
        /// 重置伤害帧
        /// </summary>
        /// <param name="proj"></param>
        public static void ClearInvincibleFrame(this Projectile proj)
        {
            for (int a = 0; a < 200; a++)
            {
                proj.localNPCImmunity[a] = 0;
            }
            proj.DProj().PenetrationProtection = 1;
        }
        /// <summary> 弹幕原始宽度 </summary>
        public static int OriginalWidth(this Projectile proj)
        {
            return ContentSamples.ProjectilesByType.TryGetValue(proj.type, out Projectile projectile) ? projectile.width : 0;
        }
        /// <summary> 弹幕原始高度 </summary>
        public static int OriginalHeight(this Projectile proj)
        {
            return ContentSamples.ProjectilesByType.TryGetValue(proj.type, out Projectile projectile) ? projectile.height : 0;
        }
        /// <summary> 弹幕原始大小 </summary>
        public static float OriginalScale(this Projectile proj)
        {
            return ContentSamples.ProjectilesByType.TryGetValue(proj.type, out Projectile projectile) ? projectile.scale : 0;
        }
        /// <summary> 弹幕原始尺寸 </summary>
        public static Vector2 OriginalSize(this Projectile projectile) => new(projectile.OriginalWidth(), projectile.OriginalHeight());

        /// <summary> 让弹幕碰撞箱获得武器的大小改动 </summary>
        public static void ProjScale2(this Projectile projectile, float ExtraScale = 0,Vector2 Size = default)
        {
            Player player = projectile.Player();
            projectile.scale = player.GetAdjustedItemScale(player.ActiveItem()) + ExtraScale;
            projectile.Resize((int)((projectile.OriginalWidth()+ Size.X) * projectile.scale), (int)((projectile.OriginalHeight() + Size.Y) * projectile.scale));
        }
        /// <summary> 让弹幕碰撞箱获得武器的大小改动 </summary>
        public static void ProjScale(this Projectile projectile, float ExtraScale = 0)
        {
            Player player = projectile.Player();
            projectile.scale = player.GetAdjustedItemScale(player.ActiveItem()) + ExtraScale;
            projectile.Resize((int)(projectile.OriginalWidth() * projectile.scale), (int)(projectile.OriginalHeight() * projectile.scale));
        }
        /// <summary> 让弹幕碰撞箱获得弹幕的大小改动(不吃物品加成) </summary>
        public static void ProjScaleChange2(this Projectile projectile, float ExtraScale = 0, Vector2 Size = default)
        {
            projectile.Resize((int)((projectile.OriginalWidth()+ Size.X) * (projectile.scale + ExtraScale)), (int)((projectile.OriginalHeight() + Size.Y) * (projectile.scale + ExtraScale)));
        }
        /// <summary> 让弹幕碰撞箱获得弹幕的大小改动(不吃物品加成) </summary>
        public static void ProjScaleChange(this Projectile projectile, float ExtraScale = 0)
        {
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                if (projectile.oldPos[i] != Vector2.Zero)
                    projectile.oldPos[i] = projectile.oldPos[i] + projectile.Size / 2;
            }
            projectile.Resize((int)(projectile.OriginalWidth() * (projectile.scale + ExtraScale)), (int)(projectile.OriginalHeight() * (projectile.scale + ExtraScale)));
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                if (projectile.oldPos[i] != Vector2.Zero)
                    projectile.oldPos[i] -= projectile.Size / 2;
            }
        }
        /// <summary>
        /// 追踪方向
        /// </summary>
        public static void SmoothVelocity(this Projectile projectile,Vector2 Speed,float Smooth) => projectile.velocity = (projectile.velocity * Smooth + Speed) / (Smooth+1);
        public static Vector2 PreviousCenter(this Projectile projectile) => projectile.DProj().PreviousPosition + projectile.Size / 2;
        /// <summary> 遍历弹幕数量 </summary>
        public static int ProjQuantity(int Type = 0)
        {
            int r = 0;
            for(int a =0;a<1000;a++)
            {
                if(Main.projectile[a].active&& Main.projectile[a].hostile&& Main.projectile[a].type==Type)
                {
                    r++;
                }
            }
            return r;
        }
        /// <summary> 玩家 </summary>
        public static Player Player(this Projectile projectile) => Main.player[projectile.owner];

        /// <summary> 缓慢看向某个地方,Rotation要达到的方向,Speed转向速度 </summary>
        public static void RotationSpeed(this Projectile projectile, float targetRotation, float speed)
        {
            // 规范化当前角度和目标角度
            float current = NormalizeAngle(projectile.rotation);
            float target = NormalizeAngle(targetRotation);

            // 计算最短角度差
            float diff = target - current;
            if (Math.Abs(diff) > MathHelper.Pi)
            {
                diff = diff > 0 ? diff - MathHelper.TwoPi : diff + MathHelper.TwoPi;
            }

            // 应用旋转
            if (Math.Abs(diff) <= speed)
            {
                projectile.rotation = target;
            }
            else
            {
                projectile.rotation = NormalizeAngle(current + Math.Sign(diff) * speed);
            }
        }

        private static float NormalizeAngle(float angle)
        {
            angle %= MathHelper.TwoPi;
            return angle < 0 ? angle + MathHelper.TwoPi : angle;
        }
        /// <summary> 手持剑使用,player玩家,AttackSpeed攻击频率,StaticNPCHitCooldown独立无敌帧,Rotate旋转半径,batter连击,Attack攻击 </summary>
        public static void HoldSword(this Projectile projectile, Player player, float AttackSpeed = -12, int StaticNPCHitCooldown = 12, float Rotate = 2, bool batter = true, bool Attack = true)
        {
            if (projectile.MeleeProj().AttackSpeed == 0)
            {
                projectile.MeleeProj().AttackSpeed = player.GetTotalAttackSpeed(DamageClass.Melee);
            }
            if (projectile.ai[1] < 2)
            {
                projectile.ai[1]++;
            }
            projectile.localNPCHitCooldown = StaticNPCHitCooldown;
            Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
            projectile.DProj().Times[1]++;
            if (projectile.DProj().Times[1] < 12)
            {
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    projectile.oldPos[i] = Vector2.Zero;
                }
            }
            if (projectile.DProj().Times[0] == 0)
            {
                projectile.DProj().Times[0] = player.direction;
                projectile.localAI[0] = Rotate * player.direction;
                projectile.localAI[1] = AttackSpeed;
                projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
            }
            int useTime = (int)(player.HeldItem.useTime / projectile.MeleeProj().AttackSpeed);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (projectile.localAI[1] < 0)
            {
                if (Attack)
                {
                    float A = projectile.localAI[0] + (projectile.localAI[1] / (AttackSpeed * 3) * projectile.DProj().Times[0]);
                    if (A > 0) A = -A;
                    projectile.localAI[1]++;
                    projectile.ai[0] = A * projectile.DProj().Times[0];
                    if (projectile.ai[1] == 3)
                    {
                        projectile.ai[1] = 2;
                    }
                }
                projectile.MeleeProj().ClearInvincibility = false;
            }
            else
            {
                projectile.localAI[1] = 1;
                projectile.direction = (int)projectile.DProj().Times[0];
                if (projectile.DProj().Times[2] >= useTime)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    projectile.Kill();
                }
                if (!projectile.MeleeProj().ClearInvincibility && StaticNPCHitCooldown == -1)
                {
                    projectile.ClearInvincibleFrame();
                    projectile.MeleeProj().ClearInvincibility = true;
                }
                if (projectile.ai[0] > Rotate)
                {
                    projectile.ai[0] = Rotate;
                    projectile.localAI[1] = AttackSpeed;
                    projectile.DProj().Times[2] -= AttackSpeed;
                    if ( projectile.DProj().Times[2] < useTime)
                    {
                        projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(projectile.DProj().vector[0].X > 0 ? 1 : -1);
                        projectile.netUpdate = true;
                    }
                    projectile.DProj().Times[0] = -1;
                    projectile.localAI[0] = projectile.ai[0];
                    projectile.DProj().Times[3]++;
                }
                if (projectile.ai[0] < -Rotate)
                {
                    projectile.ai[0] = -Rotate;
                    projectile.localAI[1] = AttackSpeed;
                    projectile.DProj().Times[2] -= AttackSpeed;
                    if ( projectile.DProj().Times[2] < useTime)
                    {
                        projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(projectile.DProj().vector[0].X > 0 ? 1 : -1);
                        projectile.netUpdate = true;
                    }
                    projectile.DProj().Times[0] = 1;
                    projectile.localAI[0] = projectile.ai[0];
                    projectile.DProj().Times[3]++;
                }

            }
            if (!batter)
            {
                projectile.DProj().Times[0] = player.direction;
            }
        }
        /// <summary> 手持剑使用2(刀光特判),player玩家,AttackSpeed攻击频率,StaticNPCHitCooldown独立无敌帧,Rotate旋转半径,batter连击,Attack攻击 </summary>
        public static void HoldSword2(this Projectile projectile, Player player, float AttackSpeed = -12, int StaticNPCHitCooldown = 12, float Rotate = 2, bool batter = true, bool Attack = true)
        {
            if (projectile.MeleeProj().AttackSpeed == 0)
            {
                projectile.MeleeProj().AttackSpeed = player.GetTotalAttackSpeed(DamageClass.Melee);
            }
            AttackSpeed = (int)(AttackSpeed / projectile.MeleeProj().AttackSpeed);
            //projectile.Kill();
            if (projectile.ai[1] < 2)
            {
                projectile.ai[1]++;
                return;
            }
            projectile.localNPCHitCooldown = StaticNPCHitCooldown;
            if (StaticNPCHitCooldown == -1)
            {
                projectile.localNPCHitCooldown = 1145141919;
            }
            Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
            if (projectile.MeleeProj().Anti)
            {
                projectile.DProj().Times[0] = -player.direction * player.gravDir;
                projectile.localAI[0] = Rotate * -player.direction;
                float U = projectile.localAI[0] + (projectile.localAI[1] / (AttackSpeed * 3));
                projectile.localAI[1]++;
                if (projectile.localAI[1] > 0)
                {
                    projectile.localAI[1] = 0;
                }
                projectile.ai[0] = -U;
                projectile.MeleeProj().Anti = false;
                projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
                projectile.netUpdate = true;
            }
            if (projectile.DProj().Times[0] == 0)
            {
                projectile.DProj().Times[0] = player.direction * player.gravDir;
                projectile.localAI[0] = Rotate * player.direction;
                //projectile.localAI[1] = AttackSpeed;
                float A = projectile.localAI[0] + (projectile.localAI[1] / (AttackSpeed * 3));
                projectile.localAI[1]++;
                if (projectile.localAI[1] > 0)
                {
                    projectile.localAI[1] = 0;
                }
                projectile.ai[0] = -A;

                projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
                if (projectile.DProj().vector[0].X > 0)
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
                }
                projectile.netUpdate = true;

            }

            int useTime = (int)(player.HeldItem.useAnimation * (projectile.extraUpdates + 1) / projectile.MeleeProj().AttackSpeed);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (projectile.localAI[1] < 0)
            {
                float A = projectile.localAI[0];
                if (A > 0) A = -A;
                if (Attack)
                {
                    projectile.localAI[1]++;
                    if (projectile.localAI[1] > 0)
                    {
                        projectile.localAI[1] = 0;
                    }
                    if (projectile.ai[1] == 3)
                    {
                        projectile.ai[1] = 2;
                    }
                }
                if (projectile.localAI[1] == 0)
                {
                    if (Attack)
                    {
                        if (batter)
                        {
                            if (projectile.DProj().Times[0] < 0)
                                projectile.DProj().Times[0] = -1;
                            else
                                projectile.DProj().Times[0] = 1;
                        }
                        else
                        {
                            projectile.DProj().Times[0] = -player.direction;
                        }
                    }
                    projectile.ai[0] = A * (-projectile.DProj().Times[0]);
                    if (projectile.DProj().Times[2] < useTime)
                    {
                        projectile.MeleeProj().AttackSpeed = player.GetTotalAttackSpeed(DamageClass.Melee);
                        projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector2).PerfectNormalize();
                        player.ChangeDir(projectile.DProj().vector[0].X > 0 ? 1 : -1);
                        projectile.netUpdate = true;
                    }
                    if (projectile.DProj().Times[0] < 0)
                        projectile.DProj().Times[0] = 1;
                    else
                        projectile.DProj().Times[0] = -1;

                    if (projectile.DProj().Times[2] >= useTime&& projectile.MeleeProj().DelayedKill<=0)
                    {
                        player.itemTime = 2;
                        player.itemAnimation = 2;
                        projectile.MeleeProj().oldPlayer = player.Center;
                        projectile.MeleeProj().DelayedKill = projectile.oldPos.Length;
                        projectile.DProj().Times[0] = -projectile.DProj().Times[0];
                        for (int i = 0; i < projectile.oldPos.Length; i++)
                        {
                            projectile.oldPos[i] = Vector2.Zero;

                            if (projectile.MeleeProj().oldVels != null)
                                projectile.MeleeProj().oldVels[i] = Vector2.Zero;
                        }
                        return;
                    }
                }
                if (projectile.DProj().vector[0].X > 0)
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
                }
                projectile.MeleeProj().ClearInvincibility = false;
            }
            else
            {
                if (!projectile.MeleeProj().ClearInvincibility && StaticNPCHitCooldown == -1)
                {
                    projectile.ClearInvincibleFrame();
                    projectile.MeleeProj().ClearInvincibility = true;
                }
                if (projectile.localAI[1] <= projectile.extraUpdates)
                {
                    if (!batter)
                        projectile.DProj().Times[0] = player.direction;
                    for (int i = 0; i < projectile.oldPos.Length; i++)
                    {
                        projectile.oldPos[i] = Vector2.Zero;
                        if(projectile.MeleeProj().oldVels!=null)
                        projectile.MeleeProj().oldVels[i] = Vector2.Zero;
                    }
                    projectile.localAI[1]++;
                }
                projectile.direction = (int)projectile.DProj().Times[0];
                if (projectile.ai[0] > Rotate)
                {
                    projectile.ai[0] = Rotate;
                    projectile.localAI[1] = AttackSpeed;
                    projectile.DProj().Times[2] -= AttackSpeed;
                    projectile.localAI[0] = projectile.ai[0];
                    projectile.DProj().Times[3]++;
                    projectile.DProj().Times[0] /= -AttackSpeed / 5;
                }
                if (projectile.ai[0] < -Rotate)
                {
                    projectile.ai[0] = -Rotate;
                    projectile.localAI[1] = AttackSpeed;
                    projectile.DProj().Times[2] -= AttackSpeed;
                    projectile.localAI[0] = projectile.ai[0];
                    projectile.DProj().Times[3]++;
                    projectile.DProj().Times[0] /= -AttackSpeed / 5;
                }
            }
            
            if (projectile.DProj().Times[0] == 0)
            {
                projectile.DProj().Times[0] = player.direction;
            }
        }
        /// <summary> 飞刀使用,player玩家,AttackSpeed攻击频率,Rotate旋转半径 </summary>
        public static void HoldFlyingKnife(this Projectile projectile, Player player, float AttackSpeed = -12, float Rotate = 2)
        {
            if (projectile.ai[1] < 2)
            {
                projectile.ai[1]++;
            }
            Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
            projectile.DProj().Times[1]++;
            if (projectile.DProj().Times[1] < 12)
            {
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    projectile.oldPos[i] = Vector2.Zero;
                }
            }
            if (projectile.DProj().Times[0] == 0)
            {
                projectile.DProj().Times[0] = player.direction;
                projectile.localAI[0] = Rotate * player.direction;
                projectile.localAI[1] = AttackSpeed;
                if (projectile.owner == Main.myPlayer)
                {
                    projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                    projectile.netUpdate = true;
                }
            }
            if (projectile.localAI[1] < 0)
            {
                float A = projectile.localAI[0] + (projectile.localAI[1] / (AttackSpeed * 3) * projectile.DProj().Times[0]);
                if (A > 0) A = -A;
                projectile.localAI[1]++;
                projectile.ai[0] = A * projectile.DProj().Times[0];
                if (projectile.ai[1] == 3)
                {
                    projectile.ai[1] = 2;
                }

                if (projectile.owner == Main.myPlayer)
                {
                    projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                    projectile.netUpdate = true;
                }
            }
            else
            {
                //projectile.extraUpdates = 5;
                if (projectile.ai[0] > Rotate)
                {
                    projectile.ai[0] = Rotate;
                    projectile.DProj().Times[3]++;
                }
                if (projectile.ai[0] < -Rotate)
                {
                    projectile.ai[0] = -Rotate;
                    projectile.DProj().Times[3]++;
                }
            }
        }
        public static void HoldChakram(this Projectile projectile, float Distance, Vector2 offset)
        {
            Player player = Main.player[projectile.owner];
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (channeling && !projectile.DProj().Bool[0])
            {
                if (projectile.DProj().Times[2] < 1)
                {
                    projectile.DProj().Times[2] += 0.004F;
                }
                else
                {
                    projectile.DProj().Times[2] = 1;
                }
                projectile.DProj().Times[3] += projectile.DProj().Times[2];
                projectile.HoldProj(player, Distance, 0, Vector2.Zero, projectile.DProj().Times[3], 0, true, 0);
                projectile.timeLeft = 600;
                projectile.damage = (int)(player.GetWeaponDamage(player.ActiveItem()) * projectile.DProj().Times[2]);
                projectile.localAI[1] = projectile.velocity.ToRotation();
            }
            else
            {

                if (projectile.DProj().Times[2] < 0.5F)
                {
                    projectile.Kill();
                }
                projectile.DProj().Times[3] += projectile.DProj().Times[2];
                if (!projectile.DProj().Bool[0])
                {
                    if (projectile.DProj().Times[4]++ > 20)
                    {
                        projectile.tileCollide = true;
                        projectile.extraUpdates = 2;
                        projectile.hide = false;
                        projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * projectile.Player().ActiveItem().shootSpeed * projectile.DProj().Times[2];
                        projectile.DProj().Bool[0] = true;
                        projectile.damage = (int)(player.GetWeaponDamage(player.ActiveItem()) * projectile.DProj().Times[2])*3;
                        projectile.ownerHitCheck = false;
                        player.itemTime = player.itemAnimation = 26;
                        player.PlayerAction().ThrowingProj(player.itemTime, 6, 0.6f * (projectile.DProj().Times[2]), projectile.localAI[1]);
                    }
                    else
                    {
                        projectile.localAI[1] -= 0.12f*player.direction;
                        projectile.HoldProj(player, Distance, 0, projectile.localAI[1].ToRotationVector2(), projectile.DProj().Times[3], 0, true, 0,false);
                        projectile.timeLeft = 600;
                    }
                }
                else
                {
                    if (projectile.soundDelay == 0)
                    {
                        projectile.soundDelay = 30;
                        SoundEngine.PlaySound(SoundID.Item7, projectile.position);
                    }
                    Player P = Main.player[projectile.owner];
                    projectile.DProj().Times[1]++;
                    if (projectile.DProj().Times[1] > 90)
                    {
                        projectile.tileCollide = false;
                        Vector2 vector4 = Vector2.Subtract(P.Center, projectile.Center);
                        DDHelper.RotateSpeed(ref projectile.localAI[0], vector4.ToRotation(), projectile.DProj().Times[0] * 0.03F);


                        projectile.velocity = projectile.localAI[0].ToRotationVector2() * projectile.DProj().Times[0];

                        Rectangle rectangle = new Rectangle((int)projectile.Center.X-12, (int)projectile.Center.Y-12, 24,24);
                        Rectangle value2 = new Rectangle((int)P.position.X, (int)P.position.Y, P.width, P.height);
                        if (rectangle.Intersects(value2))
                        {
                            projectile.Kill();
                        }
                    }
                    else
                    {
                        projectile.localAI[0] = projectile.velocity.ToRotation();
                        if (projectile.DProj().Times[0] == 0)
                        {
                            projectile.DProj().Times[0] = projectile.velocity.Length();
                        }
                    }
                }
            }
            float r = projectile.DProj().Times[2] * 1.5F;
            if (r > 0)
            {
                projectile.ProjScale2(0, offset * r-new Vector2(projectile.OriginalWidth()));
            }
            projectile.localNPCHitCooldown = (int)(40 - projectile.DProj().Times[2] * 30);
            projectile.rotation = projectile.DProj().Times[3] / 5 * player.direction;
        }
        /// <summary> 手持回旋镖使用</summary>
        /// Bool[4] 有没有飞出去
        /// projectile.DProj().Times[4]旋转程度
        /// projectile.DProj().Times[3]手旋转程度
        public static void HoldBoomerang(this Projectile projectile,Vector2 offset,float Speed)
        {
            Player player = Main.player[projectile.owner];
            //大小加成
            projectile.ProjScale();
            bool canShoot = player.channel && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;

            //蓄力
            if ((canShoot || projectile.DProj().Times[4] < 1F) && !projectile.DProj().Bool[4])
            {
                projectile.DProj().Times[1] = 0;
                //旋转
                if (projectile.DProj().Times[4] < 2f)
                {
                    projectile.DProj().Times[4] += ((1 - projectile.DProj().Times[4] / 2f) / 20+0.03F)/ player.HeldItem.useAnimation*30 * player.GetWeaponAttackSpeed(player.ActiveItem());
                }
                else
                {
                    projectile.DProj().Times[4] = 2f;
                }
                //速度
                projectile.DProj().Times[0] = Speed * (projectile.DProj().Times[4] / 2f);
                //手臂
                Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
                //方向
                projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                //
                Vector2 Pvelocity = Utils.RotatedBy(projectile.DProj().vector[0].PerfectNormalize(), 0, default);
                float v = 3.14f;
                if (player.direction == 1) v = -MathHelper.Pi;
                //手持弹幕
                projectile.HoldProj(player, 0, Pvelocity.ToRotation() - MathHelper.PiOver4 * player.direction + MathHelper.PiOver2 - projectile.DProj().Times[4] * player.direction, new Vector2(1f, 0), MathHelper.Pi, 0, true, 0, false);
                //伤害
                projectile.damage = 0;
                //手臂旋转
                projectile.DProj().Times[3] = new Vector2(0, -1).RotatedBy(Pvelocity.ToRotation() - projectile.DProj().Times[4] * player.direction).ToRotation();
                player.itemRotation = projectile.DProj().Times[3];
                player.PlayerAction().PlayerArmRotation(projectile.DProj().Times[3], 0);
                //弹幕位置调整
                projectile.position -= offset.RotatedBy(player.itemRotation + v);
                player.ChangeDir(projectile.DProj().vector[0].X >= 0 ? 1 : -1);
                projectile.timeLeft = 1800;
                player.itemTime = player.itemAnimation = 2;
                if (player.velocity.X == 0)
                {
                    player.PlayerAction().PlayerArmRotationBack(-0.1F, 0);
                }
                projectile.direction = player.direction;
            }
            //放手后自由移动
            else
            {
                projectile.ownerHitCheck = false;
                projectile.extraUpdates = 3;
                projectile.hide = false;
                if (!projectile.DProj().Bool[4])
                { 
                    //伤害
                    projectile.damage = (int)(player.GetWeaponDamage(player.ActiveItem()) * (projectile.DProj().Times[4] / 2f));
                    player.itemTime = player.itemAnimation = 26;
                    player.PlayerAction().ThrowingProj(player.itemTime, 6, 0.6f * (projectile.DProj().Times[4] / 2f), projectile.DProj().Times[3]);
                    projectile.velocity = projectile.DProj().vector[0].PerfectNormalize() * projectile.DProj().Times[0];
                    projectile.DProj().Bool[4] = true;
                }
                if (projectile.soundDelay == 0)
                {
                    projectile.soundDelay = 30;
                    SoundEngine.PlaySound(SoundID.Item7, projectile.position);
                }
                Player P = Main.player[projectile.owner];
                projectile.DProj().Times[1]++;
                if (projectile.DProj().Times[1] > 120)
                {
                    projectile.tileCollide = false;
                    Vector2 vector4 = Vector2.Subtract(P.Center, projectile.Center);
                    DDHelper.RotateSpeed(ref projectile.localAI[0], vector4.ToRotation(), projectile.DProj().Times[0] * 0.03F);


                    projectile.velocity = projectile.localAI[0].ToRotationVector2() * projectile.DProj().Times[0];

                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)P.position.X, (int)P.position.Y, P.width, P.height);
                    if (rectangle.Intersects(value2))
                    {
                        projectile.Kill();
                    }
                }
                else
                {
                    projectile.localAI[0] = projectile.velocity.ToRotation();
                    if (projectile.DProj().Times[0] == 0)
                    {
                        projectile.DProj().Times[0] = projectile.velocity.Length();
                    }
                }
                projectile.rotation += projectile.velocity.Length() * 0.05F * projectile.direction;
            }
            projectile.netUpdate = true;
        }
        /// <summary> 手持弹幕使用,player玩家,Distance武器距离玩家距离,ProjRotation弹幕旋转,velocity速度(方向),Rotation贴图旋转,Initial初始法阵大小,Kill禁用松开鼠标Kill弹幕,ValueSpeed增值速度,direction玩家翻转,AttackSpeeds是否启用攻速加成</summary>
        public static void HoldProj(this Projectile projectile, Player player, float Distance, float ProjRotation, Vector2 velocity, float Rotation = 0, float Initial = 0.25f, bool Kill = false, float ValueSpeed = 1, bool direction = true, bool AttackSpeed = true, bool sole = true)
        {
            if (player.dead)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                projectile.Kill();
                return;
            }
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (projectile.MeleeProj().SwordHitbox)
                player.fullRotation = 0;
            if (!projectile.DProj().Bool[2])
            {
                if (projectile.MeleeProj().SwordHitbox)
                {
                    if ((player.Dplayer().MouseWorld - player.MountedCenter).X >= 0)
                    {
                        player.direction = 1;
                    }
                    else
                    {
                        player.direction = -1;
                    }
                }
                else
                {
                    player.fullRotation = player.velocity.X * 0.03f;
                    player.statMana += player.ItemMana();
                }
                projectile.DProj().Bool[2] = true;
                projectile.netUpdate = true;
            }
            projectile.ownerHitCheck = true;
            if (projectile.DProj().Back > 0)
            {
                player.heldProj = projectile.whoAmI;

                projectile.hide = true;
            }
            else
            {
                projectile.hide = false;
            }
            if (projectile.ai[1] <= 0)
            {
                projectile.ai[1] = Initial;
            }
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (channeling || Kill)
            {
                if (AttackSpeed)
                {
                    projectile.ai[0] += ValueSpeed * player.GetTotalAttackSpeed(projectile.DamageType);
                }
                else
                {
                    projectile.ai[0] += ValueSpeed;
                }
                if (velocity == Vector2.Zero)
                {
                    projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                    projectile.netUpdate = true;
                    Vector2 Pvelocity = Utils.RotatedBy(projectile.DProj().vector[0].PerfectNormalize(), ProjRotation, default);
                    projectile.velocity = Pvelocity;
                }
                else
                {
                    Vector2 Pvelocity = Utils.RotatedBy(velocity.PerfectNormalize(), ProjRotation, default);
                    projectile.velocity = Pvelocity;
                    //projectile.netUpdate = true;
                }
            }
            else
            {
                if (!Kill)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    projectile.Kill();
                    return;
                }
            }
            Vector2 Velocity = Utils.RotatedBy(Vector2.Normalize(projectile.velocity), 0, default);
            Vector2 cen = Vector2.Zero;/*
            if (player.ActiveItem().type>0&& player.ActiveItem().GetGlobalItem<RangedGlobalItem>().Bow)
            {
                cen = new Vector2(2 * player.direction, 0);
                if (Velocity.Y > 0)
                {
                    cen = new Vector2(6 * player.direction, 0);
                    Distance += 8;
                    Velocity.Y *= 0.8F;
                }
                else
                {
                    Distance += 4;
                }
            }
            if (player.ActiveItem().type>0&&(player.ActiveItem().DamageType == DamageClass.Melee))
            {
                cen = new Vector2(2 * player.direction, 0);
                if (Velocity.Y > 0)
                {
                    cen = new Vector2(6 * player.direction, 0);
                }
                else
                {
                    Distance += 4;
                }
            }
            if (player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<MagicGlobalItem>().Handheld)
            {
                cen = new Vector2(2 * player.direction, 0);
                if (Velocity.Y > 0)
                {
                    cen = new Vector2(6 * player.direction, 0);
                    Distance += 8;
                }
                else
                {
                    Distance += 4;
                }
            }*/
            if (player.mount.Active)
            {
                if (projectile.DProj().Back < 0)
                {
                    projectile.Center = player.RotatedRelativePoint(player.ArmCenter() - cen, reverseRotation: false, addGfxOffY: false) + Velocity * (Distance) + new Vector2(0, player.gfxOffY) + new Vector2(10 * player.direction, 0);
                }
                else
                {
                    projectile.Center = player.RotatedRelativePoint(player.ArmCenter() - cen, reverseRotation: false, addGfxOffY: false) + Velocity * (Distance) + new Vector2(0, player.gfxOffY);
                }
            }
            else
            {
                if (projectile.DProj().Back < 0)
                {
                    projectile.Center = player.ArmCenter() - cen + Velocity * (Distance) + new Vector2(0, player.gfxOffY) + new Vector2(10 * player.direction, 0);
                }
                else
                {
                    projectile.Center = player.ArmCenter() - cen + Velocity * (Distance) + new Vector2(0, player.gfxOffY);
                }
            }
            projectile.rotation = projectile.velocity.ToRotation() + Rotation;
            projectile.timeLeft = 2;
            if (direction)
            {
                player.ChangeDir(projectile.direction);
            }

            if (projectile.MeleeProj().DelayedKill <= 0)
            {
                /*
                player.itemTime = (int)(player.ActiveItem().useTime * player.GetTotalAttackSpeed(projectile.DamageType));
                player.itemTimeMax = (int)(player.ActiveItem().useTime * player.GetTotalAttackSpeed(projectile.DamageType));
                player.itemAnimation = (int)(player.ActiveItem().useAnimation* player.GetTotalAttackSpeed(projectile.DamageType));
                player.itemAnimationMax = player.itemTimeMax;*/
                if (player.itemTime<2)
                {
                    player.itemTime = 2;
                }
                if (player.itemAnimation < 2)
                {
                    player.itemAnimation = 2;
                }
                    float v = 0;
                if (player.direction == -1) v = 3.14f;

                if (projectile.DProj().Back > 0)
                    player.itemRotation = projectile.velocity.ToRotation()* player.gravDir + v - player.fullRotation;
                else
                    player.PlayerAction().PlayerArmRotationBack(projectile.velocity.ToRotation() * player.gravDir + v-MathHelper.PiOver2*player.direction - player.fullRotation+0.3f*player.direction, 0);
            }
            if (sole)
            {
                int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
                projectile.damage = (int)(damageWithChargeAndStats);
            }
        }
        /// <summary> 手持书使用,mana消耗的魔力, Distance书距离玩家最大距离,reduceDistance减去距离,floating上下浮动速度,floatingDistance浮动最大距离</summary>
        public static void HoldBook(this Projectile projectile, int mana, float Distance, float reduceDistance, float floating, float floatingDistance)
        {
            Player player = projectile.Player();
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            //控制书的距离
            if (channeling)
            {
                if (projectile.DProj().Times[1] < projectile.SolidTileDistanceDetection(Distance, reduceDistance))
                {
                    projectile.DProj().Times[1] += 10;
                }
                else projectile.DProj().Times[1] = projectile.SolidTileDistanceDetection(Distance, reduceDistance);
            }
            else
            {
                projectile.DProj().Times[1]-=4;
            }
            //如果翻转
            if (projectile.DProj().Times[2] != player.direction)
            {
                projectile.DProj().Times[1] *= -1;
                projectile.DProj().Times[2] = player.direction;
            }
            //上下浮动
            DDHelper.BackAndForth(floating, -floating, floatingDistance, ref projectile.DProj().Times[3], ref projectile.DProj().Bool[0]);
            projectile.position.Y -= projectile.DProj().Times[3];
            //如果左键一直按着
            if (channeling)
            {
                if (player.statMana >= mana)
                {
                    //如果书出来了
                    if (projectile.DProj().Times[4] <= 1F)
                    {
                        projectile.DProj().Times[4] += 0.03f;
                    }
                    else
                    {
                        //消耗魔力时的帧图
                        projectile.frameCounter++;
                        if (projectile.frameCounter >= player.HeldItem.useAnimation / 5)
                        {
                            projectile.frameCounter = 0;
                            projectile.frame++;
                        }
                        if (projectile.frame > 6)
                        {
                            projectile.frame = 2;
                        }
                        if (projectile.frame >= 2 && projectile.frame <= 6)
                        {
                            projectile.DProj().Bool[1] = true;
                        }
                    }
                }
                else
                {
                    //没蓝时候的帧图
                    projectile.DProj().Bool[1] = false;
                    projectile.netUpdate = true;
                    projectile.frameCounter++;
                    if (projectile.frameCounter <= 5)
                    {
                        projectile.frame = 7;
                    }
                    else if (projectile.frameCounter <= 10)
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else
            {
                //收回时的帧图
                projectile.DProj().Bool[1] = false;
                projectile.frameCounter++;
                projectile.netUpdate = true;
                if (projectile.frameCounter <= 5)
                {
                    projectile.frame = 7;
                }
                else if (projectile.frameCounter <= 10)
                {
                    projectile.frame = 0;
                }
                else
                if (projectile.DProj().Times[1] <= 0)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    projectile.Kill();
                }
                if (projectile.DProj().Times[4] > 0.2F)
                {
                    projectile.DProj().Times[4] -= 0.03f;
                }
            }
            //伤害
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            projectile.damage = damageWithChargeAndStats;
        }
    }
}