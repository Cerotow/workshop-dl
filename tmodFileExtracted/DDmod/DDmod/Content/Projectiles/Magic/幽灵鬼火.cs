using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class 幽灵鬼火 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.scale = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1600;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(193, 251, 255) * 0.003F);
            Projectile.ProjScaleChange();
            for (int a = 0; a < 2; a++)
            {
                int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.2F, 0.5F));
                Main.dust[A].velocity = new Vector2(0, Main.rand.NextFloat(-6));
                Main.dust[A].customData = 2.4F;
                Main.dust[A].alpha = 255 - (int)(Projectile.ai[1] * 255);
                Main.dust[A].noGravity = true;
            }
            Projectile.ai[2]++;
            if (Projectile.ai[2] < 300)
            {
                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.02F;
                }
                else
                {
                    Projectile.ai[1] = 1;
                }
            }
            else
            {
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] -= 0.05F;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 8 % 4;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] >= 0.5F;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4), Color.White * Projectile.ai[1], Projectile.rotation, new Vector2(10, 22), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4), new Color(54, 247, 255, 0) * Projectile.ai[1], Projectile.rotation, new Vector2(10, 22), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                int Type = ModContent.DustType<光球粒子>();
                Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(54, 247, 255, 0)*0.5F)];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0, 2);
                dust.customData= dust.DustAI(1);
            }

        }
    }
}