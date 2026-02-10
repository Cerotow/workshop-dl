using DDmod.Content.Projectiles.Magic.Staff;

namespace DDmod.Content.Projectiles.Magic
{
    public class SuckBloodPlayer : ModProjectile
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
            Player player = Main.player[Projectile.owner];
            Projectile proj = Main.projectile[(int)Projectile.ai[0]];
            Vector2 vector = proj.Center + proj.velocity.PerfectNormalize() * 12 - Projectile.Center;
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] > 30)
            {
                if (proj.active && proj.type == Projectile.ai[1] && proj.owner == Projectile.owner)
                {
                    float CD = 1;
                    if(proj.type == ModContent.ProjectileType<SoulDrain>())
                    {
                        CD = 4;
                    }
                    if (proj.localAI[0] < CD)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 3) / 21;
                        Projectile.velocity += player.velocity / 64;
                        if (vector.Length() < 10)
                        {
                            proj.localAI[0] += 0.1F;
                            proj.netUpdate = true;
                            Projectile.Kill();
                        }
                    }
                    else
                    {
                        Projectile.localAI[0] = 1;
                    }
                }
                else
                {
                    Projectile.localAI[0] = 1;
                }
            }
            if (Projectile.localAI[0] == 1)
            {
                if (proj.type == ModContent.ProjectileType<SoulDrain>())
                {
                    Projectile.Kill();
                }
                vector = player.Center - Projectile.Center;
                Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 3) / 21;
                Projectile.velocity += player.velocity / 64;
                if (vector.Length() < 10)
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(3);
                    Projectile.Kill();
                }
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