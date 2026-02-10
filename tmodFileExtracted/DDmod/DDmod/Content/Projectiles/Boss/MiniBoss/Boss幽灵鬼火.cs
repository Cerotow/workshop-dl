using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss幽灵鬼火 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1600;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(193, 251, 255) * 0.003F);
            Projectile.ProjScaleChange();
            for (int a = 0; a < 2; a++)
            {
                int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.2F, 0.5F));
                Main.dust[A].velocity = new Vector2(0,Main.rand.NextFloat(-6));
                Main.dust[A].customData = 2.4F;
                Main.dust[A].alpha = 255- (int)(Projectile.ai[1]*255);
                Main.dust[A].noGravity = true;
            }
            Projectile.ai[2] ++;
            if (Projectile.ai[2] < 300)
            {
                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.005F;
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
            return Projectile.ai[1]>=0.5F;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0,texture.Height/4*Projectile.frame,texture.Width,texture.Height/4), Color.White* Projectile.ai[1], Projectile.rotation, new Vector2(10,22), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0,texture.Height/4*Projectile.frame,texture.Width,texture.Height/4), new Color(54, 247, 255, 0) * Projectile.ai[1], Projectile.rotation, new Vector2(10,22), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                for (int A = 0; A < 0.25F * (Projectile.oldPos.Length - i); A++)
                {
                    int Type = ModContent.DustType<光球粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(54, 247, 255, 0))];
                    dust.noGravity = true;
                    dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                    dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
                }
            }
        }
    }
}