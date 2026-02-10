using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Projectiles.Melee
{
    public class Spikes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 2;
                Projectile.velocity.Y += 0.1f;
            
            Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.1F, 0.3F);
            Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 14)];
            dust.velocity = projDirection;
            dust.noGravity = false;
            dust.alpha = 100;
            dust.scale = 0.3f;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.1F, 0.3F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 14)];
                dust.velocity = projDirection;
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 0.5f;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<EvilEntanglement>(), 300);
            }
            for (int A = 0; A < 10; A++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.1F, 0.3F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 14)];
                dust.velocity = projDirection;
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 0.5f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return null;
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}