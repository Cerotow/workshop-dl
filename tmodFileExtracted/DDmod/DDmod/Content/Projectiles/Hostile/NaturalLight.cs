using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Hostile
{
    public class NaturalLight : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.scale /= 3;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(20, 255, 20) * 0.003F);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            for (float l = 0; l < Length; l++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                    Color color2 = Projectile.GetAlpha(new Color(0, 255, 0, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                    color2 = Projectile.GetAlpha(new Color(200, 0, 200, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                int Type = ModContent.DustType<生命粒子>();
                Dust dust = Main.dust[NewDust(Projectile.Center , 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.4f;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }
        }
    }
}