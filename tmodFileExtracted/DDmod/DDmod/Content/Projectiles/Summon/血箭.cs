using DDmod.Content.Dusts;
using Terraria;
using Terraria.ModLoader.Config;

namespace DDmod.Content.Projectiles.Summon
{
    public class 血箭 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Default;
            Projectile.extraUpdates = 4;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item171, Projectile.Center);
                Projectile.localAI[0] = 1f;
                for (int num163 = 0; num163 < 8; num163++)
                {
                    Dust obj13 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Projectile.velocity.X, Projectile.velocity.Y, 100)];
                    obj13.velocity = (Main.rand.NextFloatDirection() * (float)Math.PI).ToRotationVector2() * 2f + Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
                    obj13.scale = 0.9f;
                    obj13.fadeIn = 1.1f;
                    obj13.position = Projectile.Center;
                }
            }

            Projectile.alpha -= 20;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            Dust obj14 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.velocity.X, Projectile.velocity.Y, 100,new Color(255,100,100,50))];
            obj14.velocity = Vector2.Zero;
            obj14.rotation = Projectile.velocity.ToRotation();
            obj14.scale = 3f;
            obj14.position = Projectile.Center;
            obj14.customData = 2;
            obj14.noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
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

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Color color = new Color(155, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, color, Projectile.rotation, texture.Size()/2, Projectile.scale,0, 0f);
            }

            return false;
        }
    }
}