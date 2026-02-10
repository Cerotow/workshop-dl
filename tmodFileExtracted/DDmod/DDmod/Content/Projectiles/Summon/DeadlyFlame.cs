namespace DDmod.Content.Projectiles.Summon
{
    public class DeadlyFlame : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 60;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void SetStaticDefaults()
        {
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            //Projectile.position += Main.projectile[(int)Projectile.ai[0]].velocity;
            //Projectile.rotation += Main.projectile[(int)Projectile.ai[0]].velocity.Length()*0.08F;
            int num565 = Utils.SelectRandom(Main.rand, 226, 228, 75);
            Projectile.ai[1] += 1f;
            if (Main.rand.NextBool(4))
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num565, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f)];
                dust.noGravity = true;
                if (num565 == 226)
                    dust.scale /= 1;
            }
            if (Projectile.ai[1] >= 180)
            {
                Projectile.active = false;
            }
        }
    }
}