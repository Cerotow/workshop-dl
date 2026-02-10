
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 叶绿光箭 : ModProjectile
    {
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.soundDelay == 0&& Projectile.timeLeft > 20)
            {
                Projectile.soundDelay = 1;
                Dust dust = Main.dust[NewDust(Projectile.Center-Projectile.velocity.PerfectNormalize()*26-new Vector2(4), 1, 1, ModContent.DustType<激光粒子>())];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.color = new Color(105,212,0,0);
                dust.scale = 0.7f;
                dust.customData = -0.5f;
                dust.rotation = Projectile.rotation + MathHelper.PiOver2;
                dust.velocity = Vector2.Zero;

                Dust dust2 = Main.dust[NewDust(Projectile.position -Projectile.velocity.PerfectNormalize()*26-new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<光球粒子>())];
                dust2.noGravity = true;
                dust2.alpha = 100;
                dust2.color = new Color(105,212,0,0);
                dust2.scale = 0.8f;
                dust2.customData = 1.2f;
                dust2.velocity = Vector2.Zero;
            }
            if (Projectile.timeLeft > 20)
                Projectile.Track(600, 20, 32,10);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if(Projectile.timeLeft>20)
                Projectile.timeLeft = 20;
            Projectile.velocity = Projectile.velocity.PerfectNormalize()*0.1f;
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 20)
                Projectile.timeLeft = 20;
            Projectile.velocity = oldVelocity.PerfectNormalize() * 0.1f;
            Projectile.netUpdate = true;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            lightColor = new Color(100, 255, 0, 0);
            if (Projectile.timeLeft <= 20)
            {
                lightColor *= Projectile.timeLeft / 20f;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 Cen = Projectile.oldPos[i] + Projectile.Size / 2;
                DDHelper.绘制偏移头部拖尾(texture, Cen, Projectile, lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f), MathHelper.Pi);
            }
            DDHelper.绘制偏移头部(texture, Projectile, lightColor, MathHelper.Pi);
            DDHelper.绘制偏移头部(texture, Projectile, lightColor, MathHelper.Pi);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 Pvelocity = Projectile.velocity.PerfectNormalize();
            projHitbox.X += (int)(Pvelocity.X * 14);
            projHitbox.Y += (int)(Pvelocity.Y * 14);
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            return null;
        }
    }
}
