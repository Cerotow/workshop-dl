using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 水箭 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 33)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
            }
            if(Main.myPlayer == Projectile.owner)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(2f,0), ModContent.ProjectileType<海啸>(), Projectile.damage / 12, Projectile.knockBack / 4);
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0f,0), ModContent.ProjectileType<海啸>(), Projectile.damage / 12, Projectile.knockBack / 4,-1,0,1);
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(-2f, 0), ModContent.ProjectileType<海啸>(), Projectile.damage / 12, Projectile.knockBack / 4);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            DDHelper.绘制偏移头部(texture, Projectile, lightColor, MathHelper.Pi);

            return false;
        }
    }
}