using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Hostile
{
    public class 绿岩激光 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.localAI[0]==0)
            {
                 int a=  NewDust(Projectile.Center-new Vector2(4),1,1,ModContent.DustType<星光粒子>(),0,0,100,new Color(100,255,100,0),3.5F);
                Main.dust[a].customData = 4002;
                Main.dust[a].velocity = Vector2.Zero;
                Projectile.localAI[0] = 1;
            }
            if (Projectile.ai[1] < 12)
            {
                Projectile.ai[1] += 1F;
            }
            else
            {
                Projectile.ai[1] = 12;
            }
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
            NewDustChange4(30, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 4, true, 0.4F, 1.2F, 100, 0, new Color(100, 255, 100, 0), 2);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(texture.Width / 2);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width,4), new Color(255, 255, 255, 20), Projectile.rotation + MathHelper.PiOver2, vector, Projectile.scale, spriteEffects, 0f);
            vector = new Vector2(texture.Width / 2, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.velocity.PerfectNormalize() * (4- texture.Width / 2) * Projectile.scale - Main.screenPosition, new Rectangle(0, 4, texture.Width, 2), new Color(255, 255, 255, 20), Projectile.rotation + MathHelper.PiOver2, vector, new Vector2(1, Projectile.velocity.Length()/2* Projectile.ai[1]) * Projectile.scale, spriteEffects, 0f);
            vector = new Vector2(texture.Width / 2, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center -(Projectile.velocity.PerfectNormalize() * (4- texture.Width / 2) * Projectile.scale) - Projectile.velocity* Projectile.ai[1] * Projectile.scale - Main.screenPosition, new Rectangle(0, 6, texture.Width, 4), new Color(255, 255, 255, 20), Projectile.rotation + MathHelper.PiOver2, vector, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}