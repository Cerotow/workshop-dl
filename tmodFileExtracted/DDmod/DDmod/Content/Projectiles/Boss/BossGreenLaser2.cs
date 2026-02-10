using DDmod.Content.Dusts;
using Terraria;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossGreenLaser2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Green Laser");
           //DisplayName.AddTranslation(7, "绿激光");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] =40;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 0.4F;
            Projectile.timeLeft = 3000;
            Projectile.extraUpdates = 5;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 200)
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    int Type = ModContent.DustType<绿激光粒子>();
                    for (int A = 0; A < 30; A++)
                    {
                        Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1;
                        dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3.5f, 7.5f);
                        dust.rotation = Projectile.rotation;
                    }
                    Projectile.velocity = Vector2.Zero;
                }
                if (Projectile.ai[0] > 240)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int Type = ModContent.DustType<绿激光粒子>();
            for (int A = 0; A < 10; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 2.5f);
                dust.rotation = Projectile.rotation;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(1, 190, 7, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) + 0.4f, spriteEffects, 0f);
                color = new Color(255 - 1, 255 - 190, 150, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) + 0.2f, spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(1, 190, 7, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale + 0.4f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255 - 1, 255 - 190, 150, 0), Projectile.rotation, texture.Size() / 2,  Projectile.scale/2 + 0.2f, spriteEffects, 0f);

            return false;
        }
    }
}