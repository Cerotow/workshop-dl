namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class ShadowWoodArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 2;
                NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0, 0, 0, default, 0.7f);
            }
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(BuffID.ShadowFlame, Main.rand.Next(100, 300));
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 27)];
                dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.5F, 1F);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, 14, 14)), Color.White, Projectile.rotation, new Vector2(texture.Width/2,texture.Height/4), Projectile.scale, 0, 0f);

        }
    }
}