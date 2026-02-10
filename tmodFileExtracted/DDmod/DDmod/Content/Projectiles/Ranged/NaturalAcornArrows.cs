using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Magic;

namespace DDmod.Content.Projectiles.Ranged
{
    public class NaturalAcornArrows : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
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
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, ModContent.DustType<生命粒子>());
            }
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.velocity.Length();

            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0]==0)
            {
                NPCdirection.Track(Projectile, 400, 60, Projectile.localAI[0], 30, false);
            }
            else
            {
                NPCdirection.Track(Projectile, 400, 20, Projectile.localAI[0], 30, false);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1&&Projectile.owner ==Main.myPlayer)
            {
                int A = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center-new Vector2(0,8), Vector2.Zero, ModContent.ProjectileType<MagicSapling>(), Projectile.damage / 2, 0, Projectile.owner, Main.rand.Next(3));
                    Main.projectile[A].DamageType = DamageClass.Ranged;
            }
            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < Main.rand.Next(50, 80); i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Projectile.velocity/5, Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                    NewDust(Projectile.Center, 1, 1, ModContent.DustType<生命粒子>(), projDirection.X, projDirection.Y);
                }
            }
            else
            {
                for (int i = 0; i < Main.rand.Next(4, 8); i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Projectile.velocity, Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                    NewDust(Projectile.Center, 1, 1, ModContent.DustType<生命粒子>(), projDirection.X, projDirection.Y);
                }
            }
        }
    }
}