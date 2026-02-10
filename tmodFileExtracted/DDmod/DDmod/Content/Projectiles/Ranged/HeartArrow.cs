using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
    public class HeartArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, ModContent.DustType<爱心粒子>());
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 5; A++)
            {
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, ModContent.DustType<爱心粒子>());
            }
            for (int i = 0; i < 15; i++)
            {
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 12);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(5))
            {
                for (int i = 0; i < Main.rand.Next(4, 8); i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Projectile.velocity, Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                    int A = NewProjectile(player.GetSource_FromAI(), Projectile.Center, projDirection * 0.5f, ModContent.ProjectileType<RangedHeart>(), Projectile.damage, 0f, 0, 0, 0);
                    Main.projectile[A].localAI[0] = target.whoAmI;
                }
            }
        }
    }
}
