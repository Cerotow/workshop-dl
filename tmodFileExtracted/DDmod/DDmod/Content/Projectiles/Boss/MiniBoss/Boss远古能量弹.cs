using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss远古能量弹 : ModProjectile
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
            Projectile.localAI[0] = Main.rand.Next(3);
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.scale = Main.rand.NextFloat(0.6F, 1.2F);
                Projectile.ai[0] = 10;
            }
            else if (Projectile.ai[0] == 1)
            {
                Projectile.scale = Main.rand.NextFloat(0.6F, 1.2F);
                Projectile.ai[0] = 20;
            }
            else if (Projectile.ai[0] == 2)
            {
                Projectile.scale = 1.25F;
                Projectile.ai[0] = 30;
            }
            Color color = new Color(84, 107, 221);
            if (Projectile.localAI[0] == 1)
            {
                color = new Color(191, 128, 255);
            }
            if (Projectile.localAI[0] == 2)
            {
                color = new Color(248, 230, 188);
            }
            color.A = 0;

            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            else
            {

                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, color, Main.rand.NextFloat(0.3F, 1F));
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 5;
                    Main.dust[A].customData = 2.4F;
                    Main.dust[A].noGravity = true;
                }
            }
            Lighting.AddLight(Projectile.Center, color.ToVector3());
            if (Projectile.DProj().track > 30 && Projectile.DProj().track < 80&& Projectile.ai[0] ==10)
            {
                Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                Vector2 vector = player.Center - Projectile.Center;
                Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 8) / 21;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(84, 107, 221);
            if (Projectile.localAI[0] == 1)
            {
                color = new Color(191, 128, 255);
            }
            if (Projectile.localAI[0] == 2)
            {
                color = new Color(248, 230, 188);
            }
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
                Color color2 = Projectile.GetAlpha(color * 0.5F) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                color2.A = (byte)(color2.A * 0.5F);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
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