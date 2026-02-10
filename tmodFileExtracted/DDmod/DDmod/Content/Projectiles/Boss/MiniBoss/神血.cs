using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class 神血 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.timeLeft =300;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            if (Projectile.ai[1]++ >45)
            {
                if (Projectile.ai[0] < 1)
                {
                    Projectile.ai[0] += 0.02F;
                }
            }
            Color startColor = new Color(150, 4, 4, 255);
            Color endColor = new Color(253, 152, 0, 150);
            Color drawColor = Color.Lerp(startColor, endColor, Projectile.ai[0]);
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                NewDustChange4(30, Projectile.Center - new Vector2(4),Vector2.Zero, ModContent.DustType<光球粒子>(),0,4,true,0.5F,1.2F,0,0, startColor);
                SoundEngine.PlaySound(SoundID.Item17, Projectile.position);
            }

            Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4),0,0,ModContent.DustType<光球粒子>(),0,0,0,drawColor)];
            dust.velocity = Projectile.velocity / 10;
            dust.customData = 3 ;
        }
        public override bool ShouldUpdatePosition()
        {
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(30, (int)(300 * (1F-Projectile.ai[0])));
            target.AddBuff(69, (int)(600 * (Projectile.ai[0])));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            Color endColor = new Color(253, 152, 0, 150);
            NewDustChange4(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 4, true, 0.5F, 1.2F, 0, 0, endColor);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.Starlight3.Value;
            Color startColor = new Color(150, 4, 4, 255);
            Color endColor = new Color(253, 152, 0, 150);
            Color drawColor = Color.Lerp(startColor, endColor, Projectile.ai[0]);
            Main.spriteBatch.Draw(texture,Projectile.Center-Main.screenPosition,null,drawColor,Projectile.rotation,texture.Size()/2,Projectile.scale,0,0);
            return false;
        }
    }
}