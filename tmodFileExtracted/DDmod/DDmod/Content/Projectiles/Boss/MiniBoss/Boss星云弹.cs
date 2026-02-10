using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss星云弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.scale = Main.rand.NextFloat(1.5F, 2F);
                Projectile.ai[0] = 1;
            }
            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            else
            {

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
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
                Color color2 = Projectile.GetAlpha(new Color(255, 31, 174,0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                color2.A = (byte)(color2.A * 0.5F);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, null, new Color(255, 31, 174, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 , SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, null, new Color(255, 31, 174, 0).Opposite(), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 , SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                int Type = ModContent.DustType<光球粒子>();
                Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 31, 174, 0))];
                dust.noGravity = true;
                dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }

        }
    }
}