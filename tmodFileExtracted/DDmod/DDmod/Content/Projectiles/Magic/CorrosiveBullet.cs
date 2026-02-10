using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Projectiles.Magic
{
    public class CorrosiveBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.alpha = 1;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.DProj().track>2)
            for (int A = 0; A < 1; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0,0, 14, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                dust.noGravity = true;
                dust.scale *= 1.1f;
                dust.velocity = -Projectile.velocity/4;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 14, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2.5f * Projectile.scale, 4.5f * Projectile.scale);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.damage /= 2;
            if (target.HasBuff(ModContent.BuffType<EvilEntanglement>()))
            {
                Projectile.penetrate++;
            }
            else if (Main.rand.NextBool(6))
            {
                target.AddBuff(ModContent.BuffType<EvilEntanglement>(), Main.rand.Next(60, 600));
                Projectile.penetrate++;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Projectile.GetAlpha(lightColor);
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 1f);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}