using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class 幽魂弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.scale = 1;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(193, 251, 255) * 0.003F);
            Projectile.scale = 0.4F;
            Projectile.ProjScaleChange();
            for (int a = 0; a < 2; a++)
            {
                int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(54, 247, 255, 0), Main.rand.NextFloat(0.3F, 0.8F)* Projectile.scale);
                Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 5;
                Main.dust[A].customData = 2.4F;
                Main.dust[A].noGravity = true;
            }
            if (Projectile.velocity.Length() < 20)
                Projectile.velocity *= 1.02F;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            for (float l = 0; l < Length; l++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                    Color color2 = Projectile.GetAlpha(new Color(255, 255, 255, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale/4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale/4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                }
            }
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
                    dust.scale = 0.2f * (Projectile.oldPos.Length - i);
                    dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0, 2) * Projectile.scale;
                }
            }
        }
    }
}