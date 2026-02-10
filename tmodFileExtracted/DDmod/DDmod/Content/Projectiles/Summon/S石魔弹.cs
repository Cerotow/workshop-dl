using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Summon
{
    public class S石魔弹 : ModProjectile
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
            Projectile.extraUpdates = 5;
            Projectile.scale = 0.66F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.6F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(255, 170, 56,0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = dust.DustAI(9) + 2.8f;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.6F, 0.9F);
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
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, color, Projectile.rotation, Origia, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, color, Projectile.rotation, Origia, Projectile.scale, sprite, 0f);
            }

            return false;
        }
    }
}