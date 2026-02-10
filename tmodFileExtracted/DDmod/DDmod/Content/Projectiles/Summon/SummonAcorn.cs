namespace DDmod.Content.Projectiles.Summon
{
    public class SummonAcorn : ModProjectile
    {
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override void SetStaticDefaults()
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.5f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int A158 = 0; A158 < 20; A158++)
            {
                int A159 = NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 7, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.5f);
                if (Main.rand.NextBool(3))
                {
                    Main.dust[A159].fadeIn = 1.1f + Main.rand.Next(-10, 11) * 0.01f;
                    Main.dust[A159].scale = 0.35f + Main.rand.Next(-10, 11) * 0.01f;
                    Main.dust[A159].type++;
                }
                else
                {
                    Main.dust[A159].scale = 1.2f + Main.rand.Next(-10, 11) * 0.01f;
                }
                Main.dust[A159].noGravity = true;
                Main.dust[A159].velocity *= 2.5f;
                Main.dust[A159].velocity -= Projectile.oldVelocity / 10f;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                int A160 = Main.rand.Next(0, 0);
                for (int A161 = 0; A161 < A160; A161++)
                {
                    Vector2 value12 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    while (value12.X == 0f && value12.Y == 0f)
                    {
                        value12 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    }
                    value12.Normalize();
                }
            }
        }
    }
}