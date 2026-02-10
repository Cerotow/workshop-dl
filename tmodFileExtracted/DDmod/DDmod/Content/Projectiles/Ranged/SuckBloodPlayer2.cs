namespace DDmod.Content.Projectiles.Ranged
{
    public class SuckBloodPlayer2 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            Vector2 vector = player.Center - Projectile.Center;
            Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 5) / 21;
            if (vector.Length() < 10)
            {
                if (player.whoAmI == Main.myPlayer)
                    player.Heal(1);
                
                Projectile.Kill();
            }
            Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 235)];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
            return false;
        }
        public override bool? CanDamage()
        {
            return null;
        }
    }
}