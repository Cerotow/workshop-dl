using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 蘑菇飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
        }
        public override void PostAI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 40; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * (Main.rand.NextFloat(2.8f, 5f));
                NewDust(Projectile.Center - Projectile.oldVelocity.PerfectNormalize() * 4-new Vector2(4), 1, 1, ModContent.DustType<蘑菇粒子>(), projDirection.X, projDirection.Y,0,default,0.75f);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 vector = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(4, 6);
                int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Projectile.oldVelocity.PerfectNormalize() * 8, vector, ModContent.ProjectileType<蘑菇>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                Main.projectile[A].DamageType = DamageClass.Melee;
            }
        }
    }
}