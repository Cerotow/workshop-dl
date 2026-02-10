namespace DDmod.Content.Projectiles.Summon
{
    public class 土 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Main.projFrames[Projectile.type] = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {

        }
        public override void AI()
        {
            for (float A = 0; A < 2; A ++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                dust.noGravity = false;
                dust.scale = Projectile.scale/2;
                dust.velocity *= 0.1f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (float A = 0; A < 22; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 0, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                dust.noGravity = false;
                dust.scale = Projectile.scale * 0.75F;
                dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2);
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