namespace DDmod.Content.Projectiles.Magic
{
    public class CrescentMoon : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.timeLeft =1000;
            Projectile.extraUpdates = 1;
        }
        public override bool PreAI()
        {
            Projectile.rotation += 0.2f;
            Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 229)];
            dust.noGravity = true;

            dust.velocity = (dust.position - Projectile.Center).PerfectNormalize()*2;
            dust.scale= 0.7F;
            Projectile.Track(500,20,12);
            if(Projectile.Center.Y>Projectile.ai[1])
            {
                Projectile.tileCollide = true;
            }
            return false;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange(20, Projectile.position, Projectile.Size, Type, 4, 4, true,1);
        }
    }
}