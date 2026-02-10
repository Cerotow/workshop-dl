using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Mono.Cecil;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class Sniper : Gun
    {

        public override GunTypes Types => GunTypes.Sniper;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => (int)Projectile.ai[2];
        public override int EveryShootCD => 0;
        public float Repelled = 1.5F;
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item40, Projectile.position);
            player.velocity -= Projectile.velocity * Repelled;
        }

    }
    public class 金制狙击枪Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 2);
            GunflamesOffset -= new Vector2(0, 1);
            Maxdithering = 0;
            Flamesframe = 0;
            Repelled = 1.5f;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 22;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            /*
            if (projToShoot == 14)
            {
                projToShoot = 242;
            }*/
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
    }
    public class 铂金狙击枪Proj : 金制狙击枪Proj
    {

    }
    public class 泣血Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 3);
            GunflamesOffset -= new Vector2(0, 2);
            Maxdithering = 0;
            Flamesframe = 2;
            flamescolor = new Color(100, 10, 10, 155);
            Repelled = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 16;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {

            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<猩红弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
    }
    public class 染血之眼Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 0);
            GunflamesOffset -= new Vector2(-1, 0);
            Maxdithering = 0;
            Flamesframe = 1;
            flamescolor = new Color(100, 10, 10, 155);
            Repelled = 0;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -6;
        public override float PositionXoffset => 20;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == ModContent.ProjectileType<猩红弹Proj>() && player.statLife >= player.Aplayer().LifeMax * 0.5f)
            {
                int damage = (int)(player.Aplayer().LifeMax * 0.1f);
                player.FixedDamage(damage, PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI));
                int a=  NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, (int)(Damage*1.5F), KnockBack, Projectile.owner,0,1);
                Main.projectile[a].DProj().Magnification =2;
            }
            else
            {
                if (projToShoot == 14)
                {
                    projToShoot = ModContent.ProjectileType<猩红弹Proj>();
                }
                NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
            }
        }
    }
    public class 钴枪Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 6.5f);
            GunflamesOffset -= new Vector2(0, 5);
            Maxdithering = 0;
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 22;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            /*
            if (projToShoot == 14)
            {
                projToShoot = 242;
            }*/
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item11, Projectile.position);
        }
    }
    public class 钯金枪Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 6.25f);
            GunflamesOffset -= new Vector2(0, 5);
            Maxdithering = 0;
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 22;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
    }
    public class 蘑菇狙击枪Proj : Sniper
    {
        public override int GunRecoilMaxTime => 6;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 0.5f);
            GunflamesOffset -= new Vector2(-1, 0);
            Maxdithering = 0;
            Flamesframe = 0;
            Repelled = 2f;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -4;
        public override float PositionXoffset => 30;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<蘑菇子弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item40, Projectile.position);
            player.velocity -= Projectile.velocity * Repelled;
        }
    }
    public class 绿岩狙击枪Proj : Sniper
    {
        public override int GunRecoilMaxTime => 12;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 3.5f);
            GunflamesOffset -= new Vector2(-1, 3);
            flamescolor = new Color(101, 255, 101, 0);
            Maxdithering = 0;
            Flamesframe = 0;
            Repelled = 2.5f;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 26;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<绿岩弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner);
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            player.velocity -= Projectile.velocity * Repelled;
            SoundStyle sound = DDHelper.SoundStyle(1, "狙击枪");
            sound.Pitch = 1F;
            PlaySound(sound, Projectile.position);
        }
    }
    public class 神圣裁决Proj : Sniper
    {
        public override int GunRecoilMaxTime => Projectile.Player().Aplayer().HolyShield >= 90?40:20;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            ShootPo -= new Vector2(0, 5.25f);
            GunflamesOffset -= new Vector2(0, 5);
            Maxdithering = 0;
            Flamesframe = 0;
            Repelled = 5f;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 22;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<神圣弹Proj>();
            }
            if (player.Aplayer().HolyShield >= 80)
            {
                SoundStyle sound = DDHelper.SoundStyle(1, "狙击枪");
                sound.Pitch = -1;
                PlaySound(sound, Projectile.position);
                player.Aplayer().HolyShield = 0;
                if (Main.myPlayer == Projectile.owner)
                {
                    int P = NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage * 3, KnockBack, Projectile.owner, 0, 1);
                    Main.projectile[P].DProj().Magnification = 2;
                }
            }
            else
            {
                SoundStyle sound = DDHelper.SoundStyle(1, "狙击枪");
                sound.Pitch = -0.2F;
                PlaySound(sound, Projectile.position);
                if (Main.myPlayer == Projectile.owner)
                {
                    NewProjectile(Source, Pos, Projectile.velocity * speed * 2, projToShoot, Damage, KnockBack, Projectile.owner, 0, 0);
                }
            }
            player.velocity -= Projectile.velocity * Repelled;
        }
    }
}

