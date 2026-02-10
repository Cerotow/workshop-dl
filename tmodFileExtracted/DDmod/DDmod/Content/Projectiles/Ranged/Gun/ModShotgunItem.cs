using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Players;
using Mono.Cecil;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class Shotgun : Gun
    {

        public override GunTypes Types => GunTypes.Shotgun;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => (int)Projectile.ai[2];
        public override int EveryShootCD => 0;
        public int MaxQuantity;
        public int MinQuantity;
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

            for (int A = 0; A < Main.rand.Next(MinQuantity-1, MaxQuantity); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Maxdithering, Maxdithering)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);
            }
            ExShoot(player, Pos, Source, projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
        }

    }
    public class 铁制猎枪Proj : Shotgun
    {
        public override int GunRecoilMaxTime => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.2F;
            ShootPo -= new Vector2(0, 3);
            GunflamesOffset -= new Vector2(0, 2);
            Flamesframe = 1;

            MaxQuantity = 3;
            MinQuantity = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 铅制猎枪Proj : 铁制猎枪Proj
    {
    }
    public class 银制霰弹枪Proj : Shotgun
    {
        public override int GunRecoilMaxTime => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.2F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 1;

            MaxQuantity = 5;
            MinQuantity = 3;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 钨制霰弹枪Proj : 银制霰弹枪Proj
    {
    }
    public class 爆裂熔岩Proj : Shotgun
    {
        public override int GunRecoilMaxTime => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.2F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 1;

            MaxQuantity = 4;
            MinQuantity = 3;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<狱火弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * Main.rand.Next(20, 40) / 40, ModContent.ProjectileType<狱火弹Proj>(), Damage, KnockBack, Projectile.owner,1);

            for (int A = 0; A < Main.rand.Next(MinQuantity - 1, MaxQuantity); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Maxdithering, Maxdithering)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

            }
            ExShoot(player, Pos, Source, projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
        }
    }
    public class 秘银双管猎枪Proj : Shotgun
    {
        public override int EveryMaxShootTimes => -2;
        public override int GunRecoilMaxTime => 5;
        public override int EveryShootCD => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.1F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 1;

            MaxQuantity = 6;
            MinQuantity = 5;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 山铜霰弹枪Proj : Shotgun
    {
        public override int GunRecoilMaxTime => 5;
        public override int EveryShootCD => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.15F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 1;

            MaxQuantity = 7;
            MinQuantity = 5;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

            for (int A = 0; A < Main.rand.Next(MinQuantity - 1, MaxQuantity); A++)
            {
                if (Main.rand.NextBool(3))
                {
                    NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Maxdithering, Maxdithering)) * speed * Main.rand.Next(20, 40) / 40, ModContent.ProjectileType<山铜花瓣>(), Damage, KnockBack, Projectile.owner);

                }
                else
                {

                    NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Maxdithering, Maxdithering)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

                }
            }
            ExShoot(player, Pos, Source, projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
        }

    }
    public class 神圣霰弹枪Proj : Shotgun
    {
        public override int GunRecoilMaxTime => 5;
        public override int EveryShootCD => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.15F;
            ShootPo -= new Vector2(0, 4);
            GunflamesOffset -= new Vector2(0, 3);
            Flamesframe = 1;

            MaxQuantity = 12;
            MinQuantity = 8;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if(projToShoot==14)
            {
                projToShoot = ModContent.ProjectileType<神圣弹Proj>();

            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

            for (int A = 0; A < Main.rand.Next(MinQuantity - 1, MaxQuantity); A++)
            {
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Maxdithering, Maxdithering)) * speed * Main.rand.Next(20, 40) / 40, projToShoot, Damage, KnockBack, Projectile.owner);

            }
            ExShoot(player, Pos, Source, projToShoot, speed, Damage, KnockBack, usedAmmoItemId);
        }

    }
}