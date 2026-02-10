using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
    public class BrokenHeartArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 120;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 12);
            }
            if (Main.myPlayer != Projectile.owner)
            {
                return;
            }
            if (Projectile.ai[1] == 0)
            {
                for (int a = 0; a < 3; a++)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi))/4, ModContent.ProjectileType<HeartArrowFragment>(), Projectile.damage / 3, 0, Projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

        }
    }
}
