using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Players;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 绿岩步枪Proj : Gun
    {
        public override void Set()
        {
            Projectile.width = 70;
            Projectile.height = 28;
            Maxdithering = 0.03F;
            Flamesframe = 1;
            ShootPo.Y -= 4; 
            GunflamesOffset = new Vector2(0, -3);
            flamescolor = new Color(100,255,100);
        }
        public override GunTypes Types => GunTypes.Rifle;
        public override float PositionYoffset => -2;
        public override float DrawY => 0;
        public override float PositionXoffset => 22;
        public override void ExShoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            ShootTimes++;
            if (ShootTimes > 3)
            {
                Vector2 Center = Utils.RotatedBy(new Vector2(0, -10* player.direction), Projectile.rotation, default);
                int A = NewProjectile(Source, Pos + Center, Projectile.velocity.PerfectNormalize() * speed, ModContent.ProjectileType<绿岩导弹>(), Damage * 2, KnockBack, Projectile.owner);
                Main.projectile[A].DamageType = DamageClass.Ranged;
                Main.projectile[A].scale = 0.3f;
                ShootTimes = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}