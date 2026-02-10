using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 石像弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
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
            if(Projectile.ai[1]==0)
            {

                SoundStyle sound = SoundID.Item157;
                sound.Volume = 0.5F;
                PlaySound(sound, Projectile.position);
                Projectile.ai[1] = 1;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(255, 255, 56,0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
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
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            DDHelper.绘制偏移头部(texture, Projectile, Color.White, MathHelper.Pi);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i]  - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);

                SpriteEffects sprite = 0;
                Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
                if (Projectile.velocity.X < 0)
                {
                    sprite = SpriteEffects.FlipVertically;
                    Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
                }
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, color, Projectile.rotation, Origia, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, color, Projectile.rotation, Origia, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), sprite, 0f);
            }

            return false;
        }
    }
}