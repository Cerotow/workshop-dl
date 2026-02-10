using DDmod.Content.Items.Melee.Sword;
using DDmod.Sync;
using DDmod.UI.BattlePetUI.技能;
using Terraria;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public abstract class AMagicStaff : ModProjectile
    {
        public virtual void Set()
        {

        }
        /// <summary>
        /// 0:持续发射法杖 1:蓄力大爆炸法杖 2:领域型法杖 3:飞行法杖 4:挥砍法杖 5:棱镜型法杖
        /// </summary>
        public virtual byte AIStyle => 0;
        public virtual int ProjShoot => 0;
        /// <summary>
        /// 生成的法阵
        /// </summary>
        public virtual int Formation => 0;
        /// <summary>
        /// 法杖使用时的动画
        /// </summary>
        public int UseAnimations = 0;
        public virtual float ProjShootSpeed => Projectile.Player().ActiveItem().shootSpeed;
        public float StaffRot = MathHelper.PiOver4;
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;

            Set();
        }
        /// <summary>
        /// 法阵大小
        /// </summary>
        public float Circle = 0;
        /// <summary>
        /// 增值速度
        /// </summary>
        public virtual float CircleValue => 0.3F;
        /// <summary>
        /// 最大法阵
        /// </summary>
        public virtual float MaxCircle => 1;
        public virtual float Distance => 0;
        public virtual float ShootDistance => 0;
        public bool SEffect = false;
        public float CircleA = 0;
        public float CircleA2 = 0;
        public float CircleA3 = 0;
        public float CircleE = 1;
        public float CircleE2 = 1;
        public float CircleE3 = 1;
        public int proj = 1;
        public float RotSpeed = 0.03f;
        public int MaxShoot = 1;
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (AIStyle == 0)
            {
                Projectile.HoldProj(player, Distance, 0, Vector2.Zero, StaffRot, 0);
                if (Circle < MaxCircle)
                {
                    Circle += CircleValue;
                    Projectile.ai[0] = 0;
                }
                else
                {
                    if (Math.Abs(Circle - MaxCircle) < CircleValue)
                    {
                        Circle = MaxCircle;
                    }
                    else
                    {
                        Circle -= CircleValue;
                    }
                    int A = player.ItemMana();
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (Projectile.ai[0] >= player.HeldItem.useAnimation)
                        {
                            if (player.statMana >= A)
                            {
                                player.statMana -= A;
                                Shoot(player);
                                PlaySound(Sound(), Projectile.position);
                                SEffect = true;
                            }
                            Projectile.ai[0] -= player.HeldItem.useAnimation;
                        }
                    }
                }
            }
            else if (AIStyle == 2)
            {
                Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
                if (player.controlUseItem && player.statMana >= player.ItemMana())
                {
                    player.Dplayer().Realm = 10;
                    int p = 0;
                    for (int a = 0; a < 1000; a++)
                    {
                        Projectile projectile = Main.projectile[a];
                        if (projectile.type > 0 && projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == Formation)
                        {
                            p++;
                        }
                    }
                    if (p == 0 && Projectile.ai[1] ==1)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, Formation, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].DProj().Bool[1] = true;
                        Main.projectile[proj].netUpdate = true;
                    }

                    if (player.Dplayer().MouseWorld.X - vector.X > 0)
                    {
                        Projectile.DProj().vector[0] = new Vector2(1, 0);
                    }
                    else
                    {
                        Projectile.DProj().vector[0] = new Vector2(-1, 0);
                    }
                    Projectile.netUpdate = true;
                    Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                    Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                    if (player.Dplayer().MouseWorld.X - vector.X > 0)
                    {
                        Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + player.fullRotation, 0, true);
                        Projectile.Center -= new Vector2(UseAnimations / 5, UseAnimations + 6);
                    }
                    else
                    {
                        Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0, true);
                        Projectile.Center -= new Vector2(-UseAnimations / 5, UseAnimations + 6);
                    }
                    float v = 0;
                    if (player.direction == -1) v = 3.14f;
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation - MathHelper.PiOver4 * (UseAnimations / 15) * player.direction;

                    //开始发射弹幕
                    if (UseAnimations <= 0)
                    {
                        if (Projectile.ai[1] == 0)
                        {
                            FormationEffect(player);
                            if (p == 0)
                            {
                                int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, Formation, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                                Main.projectile[proj].DProj().Bool[1] = true;
                                Main.projectile[proj].netUpdate = true;
                            }
                            Projectile.ai[1] = 1;
                            Projectile.ai[0] = 0;
                        }
                        else if (Projectile.ai[0] >= player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
                        {
                            int A = player.ItemMana();
                            player.statMana -= A;
                            Projectile.ai[0] -= player.ActiveItem().useAnimation;
                            Shoot(player);
                        }
                    }
                    else
                    {
                        Projectile.DProj().Times[3]++;
                        if (Projectile.DProj().Times[3] > 20)
                        {
                            UseAnimations -= 4;
                        }
                    }
                }
                else
                {
                        player.itemTime = 0;
                        player.itemAnimation = 0;
                        Projectile.Kill();
                }
            }
            else if (AIStyle == 4)
            {
                if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
                {
                    Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
                }
                Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

                Projectile.HoldProj(player, Distance, Projectile.ai[0], Projectile.DProj().vector[0], StaffRot, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);

                bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
                if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
                {
                    proj = 0;
                }
                else
                {
                    if (proj == 0)
                    {
                        PlaySound(Sound(), Projectile.position);
                        Shoot(player);
                        proj++;
                    }
                }
                Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            }
            else if (AIStyle == 5)
            {
                Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);

                Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                    Projectile.DProj().Bool[0] = true;
                }
                DDHelper.RotateSpeed(ref Projectile.DProj().Times[1], Projectile.DProj().vector[0].ToRotation(), RotSpeed);
                Projectile.HoldProj(player, 24, Projectile.DProj().Times[1], new Vector2(1, 0), MathHelper.PiOver4);
                int r = 0;
                for (int A = 0; A < 1000; A++)
                {
                    Projectile proj = Main.projectile[A];
                    if (proj.active && proj.owner == Projectile.owner && proj.type == Projectile.type)
                    {
                        r++;
                    }
                }
                if ((player.statMana <= 0 && Main.myPlayer == Projectile.owner) || r > 1)
                {
                    Projectile.Kill();
                }

                if (Circle < MaxCircle)
                {
                    Circle += CircleValue;
                    Projectile.ai[0] = 0;
                }
                else
                {
                    if (Math.Abs(Circle - MaxCircle) < CircleValue)
                    {
                        Circle = MaxCircle;
                    }
                    else
                    {
                        Circle -= CircleValue;
                    }
                    if (Projectile.ai[0] > player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                        Projectile.ai[0] -= player.ActiveItem().useAnimation;
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadowTentacles>()] < MaxShoot)
                        {
                            PlaySound(Sound(), Projectile.position);
                            Shoot(player);
                        }
                    }
                }
            }
            if (SEffect)
            {
                ShootEffect(player);
                SEffect = false;
            }
            Projectile.damage = (int)SetDamage(player);
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
        }
        public virtual void ShootEffect(Player player)
        {

        }
        public virtual SoundStyle Sound()
        {
            SoundStyle sound = SoundID.Item29;
            sound.Volume = 0.1F;
            return sound;
        }
        public  Vector2 SolidTileDistanceDetection(Player player,Vector2 Velocity)
        {
            if(Collision.CanHitLine(player.Center + Velocity, 1, 1, player.Center, 1, 1))
            {
                return Velocity;
            }
            else
            return Vector2.Zero;
        }
        public virtual void Shoot(Player player)
        {
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), player.MountedCenter + SolidTileDistanceDetection(player, vector * ShootDistance), vector * ProjShootSpeed, ProjShoot, Projectile.damage, Projectile.knockBack, Projectile.owner)];
            projectile.scale = 1;
        }
        /// <summary>
        /// 法阵出现时发生的事情
        /// </summary>
        /// <param name="player"></param>
        public virtual void FormationEffect(Player player)
        {
        }
        public virtual float SetDamage(Player player)
        {
            return player.GetWeaponDamage(player.HeldItem);
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}