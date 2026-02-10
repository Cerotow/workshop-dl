namespace DDmod.Content.Projectiles.Summon.Minions
{
    public abstract class Summons : ModProjectile
    {
        /// <summary> 让你知道召唤物的主人是谁 </summary>
        protected Player player;

        /// <summary> 让你知道召唤物的目标是谁 </summary>
        protected NPC npc;

        /// <summary> 攻击帧 </summary>
        protected int AttackFrame = 10;
        /// <summary> 命中计时器 </summary>
        protected int HitNPC = 0;

        /// <summary> 召唤物碰撞箱(防重叠) </summary>
        protected float hitbox = 1f;

        /// <summary> 召唤物惯性 </summary>
        protected float inertia = 21f;

        /// <summary> 召唤物速度 </summary>
        protected float Speed = 5f;

        /// <summary> 召唤物距离敌人有多远 </summary>
        protected float DistanceNPC;

        /// <summary> 召唤物距离玩家有多远 </summary>
        protected float DistancePlayer;

        /// <summary> 索敌范围 </summary>
        protected float SearchRange = 800;

        /// <summary> 召唤物索敌是否穿墙(如果是否也不会那么傻,因为他会检测玩家和怪物) </summary>
        protected bool IgnoreTile;

        /// <summary> 如果召唤物用得到弹幕 </summary>
        protected int shoot;

        /// <summary> 弹幕速度 </summary>
        protected float shootSpeed;

        /// <summary> 弹幕攻击速度 </summary>
        protected float AttackSpeed = 60;

        /// <summary> 追寻到目标 </summary>
        public bool target = false;

        /// <summary> 绑定和召唤物有关的buff </summary>
        protected int Minibuff = 0;

        /// <summary> 敌人和召唤物的距离 </summary>
        protected float KeepDistance = 0;

        /// <summary> 长方形碰撞箱 </summary>
        protected bool RectangularHitbox;

        /// <summary> 碰撞箱方向 </summary>
        protected float HitboxOrientation;

        /// <summary> 如果用到 </summary>
        protected float SpeedDirection;

        /// <summary> 移动缓冲 </summary>
        protected float SpeedCushioning;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AttackFrame);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AttackFrame = reader.ReadInt32();
        }
        /// <summary> 设置帧图 </summary>
        public virtual void Visual()
        {
        }
        /// <summary> 填写移动AI,返回为true使用默认 </summary>
        public virtual bool MobileAI()
        {
            return true;
        }
        /// <summary> 填写远程攻击AI,返回为true使用默认 </summary>
        public virtual bool AttackAI()
        {
            return true;
        }
        /// <summary> 设置默认值:buff绑定和召唤物有关的buff,Speed召唤物速度,Searchrang索敌范围, Shoot弹幕类型,ShootSpeed弹幕速度, Attackspeed攻击频率, Keepdistance敌人和召唤物距离,Ignoretile是否无视墙体,Hitbox召唤物碰撞箱(防重叠),Reboundspeed召唤物碰撞后反弹速度 </summary>
        protected void Defaults(int buff, float speed = 5, float Inertia = 21, float Searchrang = 800, int Shoot = 0, float ShootSpeed = 0, float Attackspeed = 60, float Keepdistance = 150, bool Ignoretile = false, float Hitbox = 1, float Reboundspeed = 0.5f)
        {
            Minibuff = buff;
            Speed = speed;
            inertia = Inertia;
            SearchRange = Searchrang;
            shoot = Shoot;
            shootSpeed = ShootSpeed;
            AttackSpeed = Attackspeed;
            KeepDistance = Keepdistance;
            IgnoreTile = Ignoretile;
            hitbox = Hitbox;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = Reboundspeed;
        }
        public override void SetDefaults()
        {
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 36000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            SetDefault();
        }
        public virtual void SetDefault()
        {
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public virtual bool CheckActive()
        {
            if (Minibuff != 0)
            {
                if (player.dead || !player.active)
                {
                    player.ClearBuff(Minibuff);

                    return false;
                }

                if (player.HasBuff(Minibuff))
                {
                    Projectile.timeLeft = 2;
                }
            }

            return true;
        }
        public bool TL;
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            if (!CheckActive())
            {
                Projectile.active = false;
                return;
            }
            if (AttackFrame>0)
            {
                AttackFrame--;
            }
            if (HitNPC > 0)
            {
                HitNPC--;
            }
            DistancePlayer = (player.Center - Projectile.Center).Length();
            float spacing = Projectile.width * hitbox;
            //遍历npc支持碰撞箱
            SummonProjectile summon = Projectile.GetGlobalProjectile<SummonProjectile>();
            Projectile.tileCollide = false;
            target = false;

            if (player.HasMinionAttackTargetNPC)
            {
                npc = null;
                npc = Main.npc[player.MinionAttackTargetNPC];
                if (IgnoreTile || (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) && !Projectile.sentry))
                {
                    if (Vector2.Distance(npc.Center, Projectile.Center) < SearchRange) target = true;
                }
                else if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && Vector2.Distance(npc.Center, Projectile.Center) < SearchRange)
                {
                    target = true;
                }
            }

            if (!target)
            {
                npc = null;
                if (!Projectile.sentry)
                {
                    npc = NPCdirection.FindClosest(player.Center, SearchRange, IgnoreTile);
                }
                if (npc == null)
                {
                    npc = NPCdirection.FindClosest(Projectile.Center, SearchRange, IgnoreTile);
                }
                if (npc != null)
                {
                    target = true;
                }
            }

            if (MobileAI())
            {
                if (target)
                {
                    Vector2 direction = npc.Center - Projectile.Center;
                    if (direction.Length() > KeepDistance)
                    {
                        direction.Normalize();
                        Projectile.velocity = (Projectile.velocity * inertia + direction * Speed) / (inertia + 1);
                    }
                    else
                    {
                        direction.Normalize();
                        Projectile.velocity = (Projectile.velocity * inertia + -direction * Speed) / (inertia + 1);
                    }
                }
                else
                {
                    Vector2 direction = player.Center - Projectile.Center;
                    if (Projectile.GetGlobalProjectile<SummonProjectile>().RotateAI)
                    {
                        int G = 0;
                        for (int T = 0; T < Projectile.whoAmI; T++)
                        {
                            if (Main.projectile[T].active&& Main.projectile[T].GetGlobalProjectile<SummonProjectile>().RotateAI)
                            {
                                if (Main.myPlayer == Projectile.owner)
                                {
                                    G++;
                                }
                            }
                        }
                        int R = 1 + (G / 10);
                        float E = 100 * R;

                        Vector2 vector2 = player.Center + Utils.RotatedBy(new Vector2(0f, E), (Math.PI * 2 / 10 * (G % 10)) + player.Dplayer().Rotate / 100, default);
                        Vector2 vector = Vector2.Subtract(vector2, Projectile.Center);
                        float A = vector.Length() / 10;
                        if (A > 50) A = 50;
                        if (A > 0)
                        {
                            Projectile.velocity = (Projectile.velocity * inertia + Vector2.Normalize(vector) * A) / (inertia + 1);
                        }
                    }
                    else
                    {
                        float speed = Speed;
                        if (player.velocity.Length() > 20 || direction.Length() > 600|| TL)
                        {
                            direction = player.Center - new Vector2(0, 100) - Projectile.Center;
                            speed = Speed*8;
                            if (DistancePlayer > 3000f)
                            {
                                Projectile.Center = player.Center;
                            }
                            DDHelper.RotateSpeed(ref SpeedDirection,direction.ToRotation(),0.1F);
                            TL = true;
                            if (direction.Length() < 120)
                            {
                                TL = false;
                            }
                        }
                        else
                        if (player.velocity.Length() > 20 || direction.Length() > 300)
                        {
                            direction = player.Center - new Vector2(0, 100) - Projectile.Center;
                            speed = Speed;
                            if (DistancePlayer > 3000f)
                            {
                                Projectile.Center = player.Center;
                            }
                            DDHelper.RotateSpeed(ref SpeedDirection,direction.ToRotation(),0.1F);
                        }
                        else if (direction.Length() < 120)
                        {
                            speed = Speed;
                            direction.Y -= 120f;
                            if (DistancePlayer > 3000f)
                            {
                                Projectile.Center = player.Center;
                            }
                            if (direction.Length() > 100f)
                            {
                                DDHelper.RotateSpeed(ref SpeedDirection, direction.ToRotation(), 0.1F);
                            }
                        }
                        speed/=4;
                        if (SpeedCushioning < speed)
                        {
                            SpeedCushioning += speed / 20;
                        }
                        else if (SpeedCushioning > (speed+ speed / 20))
                        {
                            SpeedCushioning -= speed / 5;
                        }
                        else
                        {
                            SpeedCushioning = speed;
                        }
                        Projectile.velocity = SpeedDirection.ToRotationVector2()* SpeedCushioning;
                        if (Projectile.velocity == Vector2.Zero)
                        {
                            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                        }
                        if(Main.rand.NextBool(500))
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (AttackAI())
            {
                if (target)
                {
                    Projectile.ai[0]++;
                    Vector2 direction = npc.Center - Projectile.Center;
                    DistanceNPC = direction.Length();
                    direction = direction.PerfectNormalize();
                    if (Projectile.ai[0] >= AttackSpeed)
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner)].originalDamage = Projectile.originalDamage;
                        }
                        Projectile.netUpdate = true;
                        Projectile.ai[0] = 0;
                    }
                }
            }

            Visual();
            if (summon.ReboundSpeed != 0)
            {
                for (int k = 0; k < 1000; k++)
                {
                    Projectile otherProj = Main.projectile[k];
                    if (k != Projectile.whoAmI && otherProj.active && otherProj.owner == Projectile.owner && Math.Abs(Projectile.position.X - otherProj.position.X) + Math.Abs(Projectile.position.Y - otherProj.position.Y) < spacing)
                    {
                        if (otherProj.GetGlobalProjectile<SummonProjectile>().ReboundSpeed != 0)
                        {
                            if (Projectile.position.X < Main.projectile[k].position.X)
                            {
                                Projectile.velocity.X -= summon.ReboundSpeed;
                            }
                            else
                            {
                                Projectile.velocity.X += summon.ReboundSpeed;
                            }
                            if (Projectile.position.Y < Main.projectile[k].position.Y)
                            {
                                Projectile.velocity.Y -= summon.ReboundSpeed;
                            }
                            else
                            {
                                Projectile.velocity.Y += summon.ReboundSpeed;
                            }
                        }
                    }
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.sentry)
            {
                return false;
            }
            return null;
        }
        public override bool MinionContactDamage()
        {
            if (AttackFrame <= 0) return true;
            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //剑碰撞箱
            if (RectangularHitbox)
            {

                Vector2 Pvelocity = Utils.RotatedBy(HitboxOrientation.ToRotationVector2(), 0, default);
                float num = 0f;
                bool R = false;
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * Projectile.height/2, projHitbox.Width, ref num))
                {
                    R = true;
                }
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * Projectile.height/2, projHitbox.Width, ref num))
                {
                    R = true;
                }
                return new bool?(R);
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}