
namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class UnholyArrowRemnant : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                Dust dust = Main.dust[NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 14)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            if (Projectile.ai[0] > 30)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (Projectile.ai[0] != 100)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(1.8F, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 14)];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.alpha = 100;
                        dust.scale = 1.3f;

                    }
                    Projectile.ai[0] = 100;
                }
                Projectile.Track(500, 20, 16, 30, true, (int)Projectile.ai[1]);
                if (!Projectile.HaveGoal(500, 30, true, (int)Projectile.ai[1]))
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.rotation += Projectile.velocity.Length() / 10 + 0.4f;
                Projectile.ai[0]++;
                Projectile.velocity *= 0.80f;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] > 30;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(2.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 14)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                for (int a = 0; a < 1; a++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(42, 39, 82, 200) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    if (Projectile.ai[0] > 30)
                    {
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width, 0) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    }
                }
            }
            return false;
        }
    }
}
