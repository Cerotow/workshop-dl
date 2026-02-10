using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss炼狱弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];

            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 30;
            }
            else
            {

                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 100, Color.White, Main.rand.NextFloat(0.3F, 1F));
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 5;
                    Main.dust[A].customData = 2.4F;
                    Main.dust[A].noGravity = true;
                }

                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100,new Color(254, 86, 4, 50), Main.rand.NextFloat(0.8F, 1.3F));
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = Projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
            }
            Lighting.AddLight(Projectile.Center, new Color(254, 86, 4).ToVector3());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(254, 86, 4, 50);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int L = Projectile.oldPos.Length;
            if (Projectile.scale<1)
            {
                L = (int)(L * Projectile.scale);
            }
            for (int i = 0; i < L; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                color2.A = (byte)(color2.A * 0.5F);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100)];
                dust.noGravity = true;
                dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }

        }
    }
}