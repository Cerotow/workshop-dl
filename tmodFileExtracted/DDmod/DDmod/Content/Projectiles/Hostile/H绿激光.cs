using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Hostile
{
    public class H绿激光 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Orange Laser");
           //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
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
            NewDustChange4(30, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, true, 0.4F, 1.2F, 100, 0, new Color(20,255,20,0),3);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, 0) / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width/2), Projectile.scale, 0, 0f);

            return false;
        }
    }
}