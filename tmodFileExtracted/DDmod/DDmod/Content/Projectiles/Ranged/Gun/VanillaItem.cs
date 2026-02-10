using DDmod.Content.Items;
using DDmod.Players;
using Mono.Cecil;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Projectiles.Ranged.Gun
{

    public class 发条步枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 434;
        public override int EveryShootCD => 4;
        public override int EveryMaxShootTimes => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.05F;
            GunflamesOffset -= new Vector2(0, 2);
        }
        public override float DrawY => 4;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 32;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 燧发枪 : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 95;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 左轮手枪 : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 2269;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 5);
            GunflamesOffset -= new Vector2(2, 4);
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 夺命枪 : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 800;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
        public override void AI()
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            RangedGlobalItem item = player.ActiveItem().GetGlobalItem<RangedGlobalItem>();
            if (item.Bullets == 1)
            {
                projToShoot = ModContent.ProjectileType<BloodBullet>();
            }
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
        }
        public override void ExShoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
        }
    }
    public class 凤凰爆破枪 : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 219;
        public override int EveryShootCD => 0;
        public int R = 4;
        public override int GunRecoilMaxTime => R;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 10);
            GunflamesOffset -= new Vector2(0, 9);
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (Projectile.ai[2] % 5 != 4)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }

        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            R = 4;

            PlaySound(SoundID.Item41, Projectile.position);
            if (Projectile.ai[2] % 5 == 4)
            {
                R = 10;
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, 706, Damage, KnockBack, Projectile.owner);
                for (int a = 0; a < 50; a++)
                {
                    int dust = NewDust(Pos - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 40, 1, 1, 6, 0, 0, 0, default, 2.4F);
                    Main.dust[dust].velocity = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0, 5);
                    Main.dust[dust].noGravity = true;
                }
            }
            Projectile.ai[2]++;
            Projectile.netUpdate = true;
        }
    }
    public class 链式机枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override int ID => 1929;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 0;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.075F;
            ShootPo -= new Vector2(0, 2);
            GunflamesOffset = new Vector2(-9, -1);
            AmmoPo.X -= 10;
            AmmoPo.Y -= 8;
            Flamesframe = 3;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void AI()
        {
            Player player = Projectile.Player();
            bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            CanShoot = false;
            Projectile.localAI[0] += Projectile.ai[2] / 100;
            if (Projectile.localAI[0] >= player.ActiveItem().useAnimation * 2)
            {
                Projectile.frame++;
                Projectile.localAI[0] -= player.ActiveItem().useAnimation * 2;
            }
            if (Projectile.ai[2] < 600)
            {
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                    GunflamesOffset = new Vector2(-3, -1);
                    CanShoot = true;
                }
                Projectile.ai[2] += Projectile.Player().GetAttackSpeed(DamageClass.Ranged);
                if (!canShoot)
                {
                    Projectile.ai[2] += Projectile.Player().GetAttackSpeed(DamageClass.Ranged);
                }
            }
            else
            {
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
                CanShoot = true;
                Projectile.ai[2] = 600;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item41, Projectile.position);
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle1 = new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width / 2, texture.Height / 4);
            if (Projectile.ai[2] >= 600)
            {
                rectangle1 = new Rectangle(texture.Width / 2, texture.Height / 4 * Projectile.frame, texture.Width / 2, texture.Height / 4);
            }
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, rectangle1, lightColor, Projectile.rotation, rectangle1.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, rectangle1, lightColor, Projectile.rotation, rectangle1.Size() / 2, Projectile.scale, SpriteEffects.FlipVertically, 0f);
            }
            Texture2D Flames = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            if (Gunflames > 0)
            {
                Rectangle rectangle = new Rectangle(0, Flames.Height / 4 * Flamesframe, Flames.Width, Flames.Height / 4);
                Vector2 vector = new Vector2(-texture.Width / 4 - GunflamesOffset.X, rectangle.Height / 2 - GunflamesOffset.Y * player.direction);

                Main.spriteBatch.Draw(Flames, GunflamesPo - Main.screenPosition, rectangle, new Color(255, 255, 255, 255) * Gunflames, Flamesrotation, vector, Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
    public class 乌兹冲锋枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 1265;
        public override int EveryShootCD => 3;
        public override int EveryMaxShootTimes => -20;
        public override int GunRecoilMaxTime => 0;

        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 10);
            GunflamesOffset -= new Vector2(0, 9);
            Flamesframe = 2;
            flamescolor = new Color(0, 155, 0, 155);
            AmmoPo -= new Vector2(16, 12);
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 12;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = 242;
            }
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);

        }
    }
    public class 三发猎枪 : Gun
    {
        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 964;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 8;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 6);
            GunflamesOffset = new Vector2(0, -5);
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize()* speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            for (int A = 0; A < Main.rand.Next(2, 4); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 四管霰弹枪 : Gun
    {
        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 4703;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 2);
            GunflamesOffset = new Vector2(0, -1);
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            for (int A = 0; A < Main.rand.Next(3, 5); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 霰弹枪 : Gun
    {
        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 534;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 6);
            GunflamesOffset = new Vector2(0, -5);
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            for (int A = 0; A < Main.rand.Next(2, 4); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 玛瑙爆破枪 : Gun
    {
        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 3788;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 5);
            GunflamesOffset = new Vector2(-1, -4);
            Flamesframe = 0;
            flamescolor = new Color(184, 37, 253, 255);
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            A++;
            for (int A = 0; A < 4; A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
            NewProjectileChange(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, 661, (int)(Damage * 2.5F), KnockBack, Projectile.owner, 0, 0, 2.5F);
            if (A >= 3)
            {
                A = 0;
            }
            if (A == 0)
            {
                for (int A = -1; A <= 1; A += 2)
                {
                    NewProjectileChange(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0.1F, 0.2F) * A) * speed, 661, (int)(Damage * 2.5F), KnockBack, Projectile.owner, 0, 0, 2.5F);

                }
            }
        }
    }
    public class 战术霰弹枪 : Gun
    {
        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 679;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 6);
            GunflamesOffset = new Vector2(0, -5);
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            for (int A = -4; A < 4; A++)
            {
                if (A != 0)
                    NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(0.05F * A + Main.rand.NextFloat(-0.05F, 0.05F)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 太空海豚机枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 1553;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo += new Vector2(0, 3);
            GunflamesOffset = new Vector2(0, 2);
            Maxdithering = 0.01F;
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        
    }
    public class 维纳斯万能枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 1255;
        public override int EveryShootCD => 3;
        public override int EveryMaxShootTimes => -10;
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 8);
            GunflamesOffset = new Vector2(1, -6);
            Flamesframe = 0;
            flamescolor = new Color(0, 255, 0, 255);

        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = 242;
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed, projToShoot, Damage, KnockBack, Projectile.owner);
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item41, Projectile.position);
            if (EveryShootTimes == Math.Abs(EveryMaxShootTimes) - 1)
            {
                PlaySound(SoundID.Item157, Projectile.position);
                if (Main.myPlayer == Projectile.owner)
                    NewProjectile(Projectile.GetSource_FromAI(), Pos + new Vector2(0, 9 * Projectile.Player().direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * speed, ModContent.ProjectileType<维纳斯能量>(), Projectile.damage * 3, 0, -1);
            }
        }
    }
    public class 红莱德枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 1870;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 3);
            GunflamesOffset = new Vector2(0, -2);
            Flamesframe = 1;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 鳄鱼机关枪 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 2270;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 1);
            GunflamesOffset -= new Vector2(4, -0);
            Maxdithering = MathHelper.ToRadians(15f);
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item41, Projectile.position);
        }

    }
    public class 狙击步枪 : Gun
    {
        public override GunTypes Types => GunTypes.Sniper;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 1254;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 20;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset = new Vector2(0, -3);
            Maxdithering = 0;
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 22;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            player.velocity -= Projectile.velocity * 2;
            if (projToShoot == 14)
            {
                projToShoot = 242;
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed, projToShoot, Damage, KnockBack, Projectile.owner);
        }

    }
    public class 巨兽鲨 : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => 533;
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo += new Vector2(0, 4);
            GunflamesOffset = new Vector2(0, 3);
            Maxdithering = 0.02F;
            Flamesframe = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public int A = 0;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);

        }
    }
}