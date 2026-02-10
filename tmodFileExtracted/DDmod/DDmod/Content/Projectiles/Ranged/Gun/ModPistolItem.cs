using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Players;
using Mono.Cecil;
using Terraria;
using static DDmod.Content.Projectiles.Ranged.Gun.Gun;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class Pistol : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => (int)Projectile.ai[2];
    }

    public class 叶绿枪Proj : Gun
    {
        public override GunTypes Types => GunTypes.Pistol;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => ModContent.ItemType<叶绿枪>();
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 5;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.03F;
            ShootPo -= new Vector2(0, 0);
            GunflamesOffset -= new Vector2(0, 1);
            Flamesframe = 2;
            flamescolor = new Color(0, 155, 0, 155);

        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void ExShoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            //NewProjectile(Source, Pos, Projectile.velocity * projToShoot, 207, Damage, KnockBack);
        }
    }
    public class 铜制手枪Proj : Pistol
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 3);
            GunflamesOffset -= new Vector2(-1, 2);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);

        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 锡制手枪Proj : 铜制手枪Proj
    {
    }
    public class 银制手枪Proj : Pistol
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 5);
            GunflamesOffset -= new Vector2(-1, 4);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);


        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 钨制手枪Proj : 银制手枪Proj
    {
    }
    public class 恶魔猎手Proj : Pistol
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 5);
            GunflamesOffset -= new Vector2(-1, 4);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);
            flamescolor = new Color(99, 74, 187,150);


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
                projToShoot = ModContent.ProjectileType<魔金弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
    }
    public class 噩梦降临Proj : Pistol
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 5);
            GunflamesOffset -= new Vector2(-1, 4);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);
            flamescolor = new Color(99, 74, 187, 150);


        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {

            if (Main.rand.NextBool(8) && projToShoot == ModContent.ProjectileType<魔金弹Proj>())
            {
                for (int A = 0; A < Main.rand.Next(3, 8); A++)
                {
                    NewProjectile(Source, Pos, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * speed / 6, ModContent.ProjectileType<魔金弹Proj>(), Damage, KnockBack, Projectile.owner, 0, 2);
                }
            }
            else
            {
                if (projToShoot == 14)
                {
                    projToShoot = ModContent.ProjectileType<魔金弹Proj>();
                }
                NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 精金手枪Proj : Pistol
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 8);
            GunflamesOffset -= new Vector2(-1, 7);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);


        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
            if(Main.rand.NextBool(10))
            {
                Projectile.DProj().Bool[1] = true;
                Projectile.netUpdate = true;
            }
        }
        public override void AI()
        {
            if (Projectile.DProj().Bool[1])
            {
                if (Projectile.localAI[0] < 0.25F)
                {
                    Projectile.localAI[0] += 0.01F;
                }
                else
                {
                    Projectile.localAI[0] = 0;
                    Projectile.DProj().Bool[1] = false;
                }
                Projectile.ai[0] -= 5;
                Maxdithering = Projectile.localAI[0];
            }
            else
            {
                Maxdithering = 0.01F;
            }
        }
    }
    public class 钛金手枪Proj : Pistol
    {
        public override int EveryMaxShootTimes => -8;
        public override int EveryShootCD => 6;
        public override int GunRecoilMaxTime => 3;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 8);
            GunflamesOffset -= new Vector2(-1, 7);
            Flamesframe = 2;
            AmmoPo -= new Vector2(8);


        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
    }
}