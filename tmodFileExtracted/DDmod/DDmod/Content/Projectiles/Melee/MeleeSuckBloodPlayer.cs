namespace DDmod.Content.Projectiles.Melee
{
    public class MeleeSuckBloodPlayer : ModProjectile
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
                if (proj.active && proj.type == Projectile.ai[1] && proj.owner == Projectile.owner && proj.DProj().Times[0] < 30)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 3) / 21;
                    Projectile.velocity += player.velocity / 64;
                    if (vector.Length() < 10)
                    {
                        proj.DProj().Times[0] += Projectile.DProj().Times[0];
                        proj.netUpdate = true;
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.localAI[0] = 1;
                }
            }
            if (Projectile.localAI[0] == 1)
            {
                if (Projectile.ai[1] >= 0)
                {
                    Projectile.Kill();
                }
                else
                {
                    vector = player.Center - Projectile.Center;
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 3) / 21;
                    Projectile.velocity += player.velocity / 64;
                    if (vector.Length() < 10)
                    {
                        if (player.whoAmI == Main.myPlayer)
                            player.Heal((int)Projectile.DProj().Times[0]);
                        Projectile.Kill();
                    }
                }
            }
            Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 235)];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            int A = Projectile.damage / 10;
            DDHelper.MaxandMin(ref A, player.statLifeMax2 / 20, 1);
            Projectile.DProj().Times[0] += A;
            Projectile.damage = 0;
        }
        public override bool? CanDamage()
        {
            return null;
        }
    }
}