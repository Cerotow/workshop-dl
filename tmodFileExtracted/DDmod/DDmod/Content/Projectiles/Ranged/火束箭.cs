
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 火束箭 : ModProjectile
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
            Projectile.penetrate = 1;
            Projectile.coldDamage = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            DDGlobalProjectile.ScaleGlow[Projectile.type] = 1;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>(Texture);
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("True Night Arrow");
           //DisplayName.AddTranslation(7, "真永夜箭");
        }
        public override void AI()
        {
            if (false&&!Projectile.DProj().Bool[0])
            {
                Projectile.DProj().Bool[0] = true;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0F, 1.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 6)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 1.7f;
                }
            }
            Projectile.Track(500, 21, 22);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 140);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 6)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                DDHelper.绘制偏移头部拖尾(texture, vector2, Projectile, color, MathHelper.Pi);
            }
            DDHelper.绘制偏移头部(texture, Projectile, Color.White, MathHelper.Pi);
            {
                Vector2 vector = Projectile.Size/2;
                texture = DDTextures.MiniVoidStar.Value;
                Color color = new Color(252, 102, 22,0);
                vector -= Projectile.velocity.PerfectNormalize() * 10;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(0.5F, 1F), 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(0.75F, 1.5F), 0, 0f);
            }
            return false;
        }
        internal Trailing TrailDrawer;
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
