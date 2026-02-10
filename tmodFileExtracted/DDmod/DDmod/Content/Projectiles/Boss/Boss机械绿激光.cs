using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss机械绿激光 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.localAI[0] < Projectile.velocity.Length() / 4 * (Projectile.extraUpdates + 1))
            {
                Projectile.localAI[0] += Projectile.velocity.Length() / 4 * (Projectile.extraUpdates + 1) / 10;
            }
            else
            {
                Projectile.localAI[0] = Projectile.velocity.Length() / 4 * (Projectile.extraUpdates + 1);
            }
            if (Projectile.ai[1] != 0)
            {
                Projectile.scale = Projectile.ai[1];
            }
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange4(30, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 4, true, 0.4F, 1.2F, 100, 0, new Color(50, 255, 57, 0), 2);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Color color = new Color(50, 255, 57, 20);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(texture.Width / 2, 10);
            Rectangle rectangle = new Rectangle(0, 0, texture.Width, 14);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, color, Projectile.rotation + MathHelper.PiOver2, vector, Projectile.scale, spriteEffects, 0f);

            vector = new Vector2(texture.Width / 2, 0);
            rectangle = new Rectangle(0, 14, texture.Width, 2);
            Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.velocity.PerfectNormalize() * 4 * Projectile.scale - Main.screenPosition, rectangle, color, Projectile.rotation + MathHelper.PiOver2, vector, new Vector2(1, Projectile.velocity.Length() / 2 * Projectile.localAI[0]) * Projectile.scale, spriteEffects, 0f);

            vector = new Vector2(texture.Width / 2, 0);
            rectangle = new Rectangle(0, 16, texture.Width, 14);
            Main.spriteBatch.Draw(texture, Projectile.Center - (Projectile.velocity.PerfectNormalize() * 4 * Projectile.scale) - Projectile.velocity * Projectile.localAI[0] * Projectile.scale - Main.screenPosition, rectangle, color, Projectile.rotation + MathHelper.PiOver2, vector, Projectile.scale, spriteEffects, 0f);

            // Main.spriteBatch.Draw(DDTextures.WhitePng.Value,Projectile.position-Main.screenPosition,null,Color.White,0,Vector2.Zero,Projectile.Size/2,0,0);
            return false;
        }
    }
}