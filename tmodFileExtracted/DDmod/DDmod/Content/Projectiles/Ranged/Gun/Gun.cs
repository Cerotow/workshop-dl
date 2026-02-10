using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Players;
using System.Reflection.Metadata;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public abstract class Gun : ModProjectile
    {
        public enum GunTypes : byte
        {
            /// <summary>步枪  </summary>
            Rifle,
            /// <summary>手枪 </summary>
            Pistol,
            /// <summary>狙击枪 </summary>
            Sniper,
            /// <summary>霰弹枪 </summary>
           Shotgun,
        }
        public virtual void Set()
        {

        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Set();
            /*
            if (Maxdithering > 0)
            {
                dithering = Main.rand.NextFloat(-Maxdithering, Maxdithering);
            }*/
        }
        public virtual GunTypes Types => GunTypes.Rifle;
        public float Maxdithering = 0;
        public float dithering = 0;
        /// <summary>
        /// 枪的后坐力
        /// </summary>
        public float GunRecoil;
        /// <summary>
        /// 枪的后坐力时间
        /// </summary>
        public int GunRecoilTime;
        /// <summary>
        /// 不需要弹药
        /// </summary>
        public bool NoAmmo;
        public Vector2 AmmoPo;
        /// <summary>
        /// 多发射击间隔cd
        /// </summary>
        public virtual int EveryShootCD => 0;
        public int EveryShootTimesCD = 0;
        public int EveryShootTimes;
        /// <summary>
        /// 一次性射出多少发,如果是负数则每发都消耗弹药
        /// </summary>
        public virtual int EveryMaxShootTimes => 1;
        /// <summary>
        /// 获取物品贴图
        /// </summary>
        public virtual int ID => -1;
        public virtual int GunRecoilMaxTime => 6;
        public virtual float PositionYoffset => -6;
        public virtual float PositionXoffset => 22;
        public virtual float DrawY => 0;
        public virtual float DrawX => 0;
        public int ShootTimes = 0;
        public Vector2 ShootPo;
        public bool CanShoot = true;
        //矫正
        public bool JZ;
        public static BulletData Bullet = new BulletData();
        public class BulletData
        {
            public BulletData(int projToShoot = 0, float speed = 0, int Damage = 0, float KnockBack = 0, int usedAmmoItemId = 0)
            {
                this.projToShoot = projToShoot;
                this.speed = speed;
                this.Damage = Damage;
                this.KnockBack = KnockBack;
                this.usedAmmoItemId = usedAmmoItemId;

            }
            public int projToShoot;
            public float speed;
            public int Damage;
            public float KnockBack;
            public int usedAmmoItemId;
            public Vector2 velocity;
        }
        public virtual void ExShoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
        }
        /// <summary>
        /// 为了保证粒子特效,这个生成额外弹幕需要判定客户端
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Pos"></param>
        /// <param name="Source"></param>
        /// <param name="projToShoot"></param>
        /// <param name="speed"></param>
        /// <param name="Damage"></param>
        /// <param name="KnockBack"></param>
        /// <param name="usedAmmoItemId"></param>
        public virtual void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if(Types==GunTypes.Sniper)
            {
                PlaySound(SoundID.Item40, Projectile.position);
            }
            else if (Types == GunTypes.Shotgun)
            {
                PlaySound(SoundID.Item36, Projectile.position);
            }
            else
            PlaySound(SoundID.Item11, Projectile.position);
        }
        public virtual void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
           NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
           ExShoot(player, Pos, Source, projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
        }
        public override bool PreAI()
        {
            if (Gunflames > 0)
            {
                Gunflames -= 0.5F;
            }
            float dithering = this.dithering;
            Player player = Main.player[Projectile.owner];
            RangedGlobalItem item = player.ActiveItem().GetGlobalItem<RangedGlobalItem>();
            if (item.Reload && player.ActiveItem().type == 800)
            {
                Projectile.Player().channel = false;
            }
            GunflamesPo += player.Dplayer().PrePosition / (Projectile.extraUpdates + 1);
            if (player.velocity.Length() > 0.1F)
            {
                dithering *= 2;
            }
            if (GunRecoilTime > 0)
            {
                GunRecoilTime -= 1;
            }
            else
            {
                GunRecoilTime = 0;
            }
            int direction = player.direction;
            if (Types == GunTypes.Pistol|| Types == GunTypes.Shotgun)
            {
                if (GunRecoilTime >= GunRecoilMaxTime-1)
                {
                    dithering -= (GunRecoilMaxTime-1) / 10F * direction;
                }
                else
                {
                    dithering -= (GunRecoilMaxTime - 1) / 10F * ((float)GunRecoilTime / (GunRecoilMaxTime - 1)) * direction;
                }
            }
            Vector2 vector = player.Dplayer().MouseWorld;
            if((player.Dplayer().MouseWorld-player.MountedCenter).Length()<200)
            {
                vector += (player.Dplayer().MouseWorld - player.MountedCenter).PerfectNormalize() *200;
            }
            Projectile.DProj().vector[0] = (vector - player.ArmCenter()).PerfectNormalize();
            Vector2 ShootPos = player.ArmCenter();
            Vector2 Pvelocity = Projectile.DProj().vector[0].RotatedBy(dithering);

            player.ChangeDir(Projectile.DProj().vector[0].X > 0 ? 1 : -1);

            if (Types == GunTypes.Pistol)
            {
                Projectile.HoldProj(player, PositionXoffset, 0, Pvelocity, 0, 0, true, (Projectile.ai[0] > 0) ? -player.GetAttackSpeed(DamageClass.Ranged) : 0, false);
            }
            else
            {
                Projectile.HoldProj(player, PositionXoffset - GunRecoil, 0, Pvelocity, 0, 0, true, (Projectile.ai[0] > 0) ? -player.GetAttackSpeed(DamageClass.Ranged) : 0, false);
            }


            if (player.direction == 1)
            {
                ShootPos += ShootPo.RotatedBy(Projectile.rotation);
            }
            else
            {
                ShootPos += ShootPo.RotatedBy(Projectile.rotation + MathHelper.Pi);
            }
            ShootPos += new Vector2(0, PositionYoffset * direction).RotatedBy(Projectile.rotation);
            Projectile.DProj().vector[0] = (vector - ShootPos).PerfectNormalize();
            Pvelocity = Projectile.DProj().vector[0].RotatedBy(dithering);
            Projectile.rotation = Pvelocity.ToRotation();
            Projectile.velocity = Pvelocity;

            Projectile.Center += new Vector2(0, PositionYoffset * direction).RotatedBy(Projectile.rotation);

            Player.CompositeArmStretchAmount amount = Player.CompositeArmStretchAmount.Full;

            if (Types != GunTypes.Pistol)
            {
                if (GunRecoilTime > 0)
                {
                    amount = Player.CompositeArmStretchAmount.ThreeQuarters;
                    GunRecoil = 0;
                }
                if (GunRecoilTime >= GunRecoilMaxTime * 0.33f)
                {
                    amount = Player.CompositeArmStretchAmount.Quarter;
                    GunRecoil = 3;
                }
                if (GunRecoilTime >= GunRecoilMaxTime * 0.66f)
                {
                    GunRecoil = 6;
                    amount = Player.CompositeArmStretchAmount.None;
                }
            }

            player.PlayerAction().PlayerArmRotation((Pvelocity).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, amount);
            if (Types != GunTypes.Pistol)
                player.PlayerAction().PlayerArmRotationBack((Pvelocity).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, amount);
            //vector = new Vector2(0, PositionYoffset * direction).RotatedBy(Projectile.rotation);
            //Projectile.velocity = Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - (player.ArmCenter() + vector)).PerfectNormalize().RotatedBy(dithering);
            bool canShoot = (player.channel|| (Types== GunTypes.Sniper&&player.controlUseTile)) && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if (canShoot || EveryShootTimes > 0|| !CanShoot)
            {
                if (EveryShootTimesCD > 0)
                {
                    EveryShootTimesCD--;
                }
                if (Projectile.ai[0] <= 0&& (Main.mouseLeft|| EveryShootTimes>0))
                {
                    //如果射出子弹小于应该射出的子弹数量
                    if (EveryShootTimes < Math.Abs(EveryMaxShootTimes)&& CanShoot)
                    {
                        if (EveryShootTimesCD <= 0)
                        {
                            if (EveryShootTimes == 0 || EveryMaxShootTimes < 0)
                            {
                                if (!NoAmmo)
                                {
                                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int Damage, out float KnockBack, out int usedAmmoItemId);
                                    Bullet = new BulletData(projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
                                    Bullet.velocity = Projectile.DProj().vector[0];
                                }
                                else
                                {
                                    Bullet = new BulletData(0, player.ActiveItem().shootSpeed, player.GetWeaponDamage(player.ActiveItem()), player.GetWeaponKnockback(player.ActiveItem()), 0);
                                }
                            }
                            IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, Bullet.usedAmmoItemId);
                            Bullet.KnockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], Bullet.KnockBack);
                            if (Main.myPlayer == Projectile.owner)
                            {
                                Shoot(player, ShootPos, Source, Bullet.projToShoot, Bullet.speed, Bullet.Damage, Bullet.KnockBack, Bullet.usedAmmoItemId);
                            }
                            Flamesrotation = Projectile.velocity.ToRotation();
                            GunflamesPo = Projectile.Center;
                            ShootEffect(player, ShootPos, Source, Bullet.projToShoot, Bullet.speed, Bullet.Damage, Bullet.KnockBack, Bullet.usedAmmoItemId);
                            GunRecoilTime = GunRecoilMaxTime;

                            if (!NoAmmo && Main.netMode != 2)
                            {
                                int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                                Gore.NewGore(player.GetSource_Death(), Projectile.Center+new Vector2(AmmoPo.X, AmmoPo.Y*player.direction).RotatedBy(Projectile.rotation), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                            }
                            Gunflames = 1.5F;
                            EveryShootTimes++;
                            EveryShootTimesCD = EveryShootCD;
                            if (Maxdithering > 0&& Types!=GunTypes.Shotgun)
                            {
                                this.dithering = Main.rand.NextFloat(-Maxdithering, Maxdithering);
                            }
                            if (item.Bullets <= 1)
                            {
                                item.Reload = true;
                            }
                            item.Bullets--;
                        }
                    }
                    else
                    {
                        Bullet.velocity = Vector2.Zero;
                        EveryShootTimes = 0;
                        if (CanShoot)
                        {
                            Projectile.ai[0] += player.HeldItem.useAnimation;
                        }
                        else
                        {
                            Projectile.ai[0] = 0;
                        }
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                if (Projectile.ai[0] <= 0)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Kill();
                }
            }
            return true;
        }
        public float Gunflames;
        public int Flamesframe =1;
        public float Flamesrotation;
        public Vector2 GunflamesOffset;
        public Vector2 GunflamesPo;
        public Color flamescolor = Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            if (ID <= 0)
            {
                texture = TextureAssets.Projectile[Projectile.type].Value;
            }
            else
            {
                texture = TextureAssets.Item[ID].Value;
            }
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipVertically, 0f);
            }
            Texture2D Flames = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            if (Gunflames > 0)
            {
                Rectangle rectangle = new Rectangle(0, Flames.Height / 4 * Flamesframe, Flames.Width, Flames.Height / 4);
                Vector2 vector = new Vector2(-texture.Width / 2- GunflamesOffset.X, rectangle.Height/2 - GunflamesOffset.Y*player.direction);

                Main.spriteBatch.Draw(Flames, GunflamesPo - Main.screenPosition, rectangle, flamescolor * Gunflames, Flamesrotation, vector, Projectile.scale, 0, 0f);
            }
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}