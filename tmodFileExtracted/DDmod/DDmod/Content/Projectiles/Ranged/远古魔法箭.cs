
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 远古魔法箭 : ModProjectile
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
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>(Texture + "_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(0,55,255,0);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            for (int a = 0; a < Projectile.velocity.Length() / 4; a++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center+ Projectile.velocity - new Vector2(4) - Projectile.velocity.PerfectNormalize() * (a * 4) + new Vector2(0, Projectile.ai[1]).RotatedBy(Projectile.velocity.ToRotation()), 1, 1, 172)];
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.5f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 200; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 6F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 172)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            DDHelper.绘制偏移头部(texture, Projectile, new Color(0, 55, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 10, 4);
            DDHelper.绘制偏移头部(texture, Projectile, new Color(0, 55, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 10, 4);

            return false;
        }
        internal Trailing TrailDrawer;
    }
}
